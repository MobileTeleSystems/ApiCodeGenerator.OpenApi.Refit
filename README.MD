The [ApiCodeGenerator](https://github.com/MobileTeleSystems/ApiCodeGenerator) extension module implements code generation for the [Refit](https://github.com/reactiveui/refit) library.

# Installation
1. Add the `ApiCodeGenerator.OpenApi.Refit` and `Refit` packages to your project.
2. Add a document describing the API (in json or yaml format) and a corresponding nswag file with the same name ([format description](https://github.com/MobileTeleSystems/ApiCodeGenerator#nswag-file-format)).
1. In the nswag file, select the `OpenApiToRefitClient` generator.
3. Rebuild the project.

Now, an interface with the name specified in the nswag file (by default, it matches the document file name) will be available in the project.

You can read more about other generation settings in the [ApiCodeGenerator](https://github.com/MobileTeleSystems/ApiCodeGenerator) project description.
