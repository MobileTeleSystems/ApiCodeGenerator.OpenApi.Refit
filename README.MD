[![NuGet Version](https://img.shields.io/nuget/vpre/ApiCodeGenerator.OpenApi.Refit?style=flat-square)](https://www.nuget.org/packages/ApiCodeGenerator.OpenApi.Refit)
[![GitHub License](https://img.shields.io/github/license/MobileTeleSystems/ApiCodeGenerator.OpenApi.Refit?style=flat-square)](https://github.com/MobileTeleSystems/ApiCodeGenerator.OpenApi.Refit/blob/dev/LICENSE)
[![Codacy coverage](https://img.shields.io/codacy/coverage/b69c05d5db6e4569b18117ee90680340?style=flat-square&logo=codacy)](https://app.codacy.com/gh/MobileTeleSystems/ApiCodeGenerator.OpenApi.Refit/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_coverage)


The [ApiCodeGenerator](https://github.com/MobileTeleSystems/ApiCodeGenerator) extension module implements code generation for the [Refit](https://github.com/reactiveui/refit) library.


# Installation
1. Add the `ApiCodeGenerator.OpenApi.Refit` and `Refit` packages to your project.
2. Add a document describing the API (in json or yaml format) and a corresponding nswag file with the same name ([format description](https://github.com/MobileTeleSystems/ApiCodeGenerator#nswag-file-format)).
1. In the nswag file, select the `OpenApiToRefitClient` generator.
3. Rebuild the project.

Now, an interface with the name specified in the nswag file (by default, it matches the document file name) will be available in the project.

You can read more about other generation settings in the [ApiCodeGenerator](https://github.com/MobileTeleSystems/ApiCodeGenerator) project description.
