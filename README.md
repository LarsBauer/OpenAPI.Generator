# OpenAPI.Generator

> ⚠️ **This project is no longer maintained.**

## Deprecation Notice

**OpenAPI.Generator** was a .NET tool to generate OpenAPI documents from C# annotations. As of June 2026, this project is **deprecated and archived**.

### Why?

- **Core dependency abandoned** — The underlying library [`Microsoft.OpenApi.CSharpAnnotations.DocumentGeneration`](https://github.com/microsoft/OpenAPI.NET.CSharpAnnotations) has been **archived by Microsoft** with 41 unresolved issues, unmerged security patches, and no maintainer activity since 2019.
- **End-of-life runtime** — The project targets .NET 5.0, which has been out of support since May 2022 and no longer receives security updates.
- **No path forward** — With the core dependency dead, there is no viable way to address bugs or security vulnerabilities.

### Recommended Alternatives

The .NET ecosystem now offers better-supported options for OpenAPI document generation:

| Tool | Description |
|---|---|
| [Microsoft.AspNetCore.OpenApi](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/overview) | Built-in OpenAPI support in ASP.NET Core (available since .NET 9). |
| [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) | Widely adopted Swagger/OpenAPI tooling for ASP.NET Core. |
| [NSwag](https://github.com/RicoSuter/NSwag) | Full-featured OpenAPI toolchain for .NET with code generation support. |

### Thank You

Thank you to everyone who used or contributed to this project. If you have questions about migrating, feel free to open a discussion.

---

*This project is archived and will not receive any further updates.*
