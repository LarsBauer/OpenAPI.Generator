using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.OpenApi;
using Microsoft.OpenApi.CSharpAnnotations.DocumentGeneration;
using Microsoft.OpenApi.Extensions;

namespace OpenAPI.Generator
{
    class Program
    {
        static int Main(string[] args)
        {
            var rootCommand = new RootCommand(".NET tool to generate OpenAPI documents from C# annotations")
            {
                new Option<FileInfo[]>("--annotation-xml", "Path to a annotation XML documentation file.")
                        { IsRequired = true }
                    .ExistingOnly(),
                new Option<FileInfo[]>("--assembly",
                            "Path to the assembly (DLL or EXE) that contains the data types referenced in the comments.")
                        { IsRequired = true }
                    .ExistingOnly(),
                new Option<string>("--document-version", () => "V1",
                    "Version of the OpenAPI document. Note this is not the OpenAPI specification version. This corresponds to the version field of the Info object in an OpenAPI document."),
                new Option<bool>("--camel-case", () => false,
                    "Use camelCase instead of PascalCase for property names in schema."),
                new Option<OpenApiFormat>("--format", () => OpenApiFormat.Yaml, "Specify the OpenAPI document format."),
                new Option<DirectoryInfo>("--output", "Directory in which to place the generated OpenAPI documents.")
                        { IsRequired = true }
                    .LegalFilePathsOnly(),
            };

            rootCommand.Handler =
                CommandHandler.Create<FileInfo[], FileInfo[], string, bool, OpenApiFormat, DirectoryInfo>(
                    (annotationXmlPaths, assemblyPaths, documentVersion, useCamelCase, format, outputDirectory) =>
                    {
                        var annotationXmls = annotationXmlPaths.Select(x => XDocument.Load(x.FullName)).ToList();
                        var assemblies = assemblyPaths.Select(x => x.FullName).ToList();
                        var config = new OpenApiGeneratorConfig(annotationXmls, assemblies, documentVersion,
                            FilterSetVersion.V1);

                        var generator = new OpenApiGenerator();

                        var nameResolver = useCamelCase
                            ? new CamelCasePropertyNameResolver()
                            : new DefaultPropertyNameResolver();
                        var setting = new OpenApiDocumentGenerationSettings(new SchemaGenerationSettings(nameResolver));

                        var openApiDocuments = generator.GenerateDocuments(config, out var result, setting);

                        var errors = result.DocumentGenerationDiagnostic.Errors;
                        if (errors.Any())
                        {
                            foreach (var error in errors)
                            {
                                Console.WriteLine(error.Message);
                            }

                            return;
                        }

                        foreach (var (_, doc) in openApiDocuments)
                        {
                            var documentName = format == OpenApiFormat.Json
                                ? $"{doc.Info.Title}.json"
                                : $"{doc.Info.Title}.yml";

                            doc.Serialize(File.Create(Path.Combine(outputDirectory.FullName, documentName)),
                                OpenApiSpecVersion.OpenApi3_0, format);
                        }
                    });

            // Parse the incoming args and invoke the handler
            return rootCommand.InvokeAsync(args).Result;
        }
    }
}