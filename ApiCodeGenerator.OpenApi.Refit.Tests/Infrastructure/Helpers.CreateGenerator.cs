#pragma warning disable SA1601 // Partial elements should be documented
using ApiCodeGenerator.OpenApi.Refit;

namespace ApiCodeGenerator.Tests
{
    internal static partial class Helpers
    {
        private static RefitCodeGenerator CreateGenerator(OpenApiDocument openApiDocument, CSharpClientGeneratorSettings settings)
            => new(openApiDocument, (RefitCodeGeneratorSettings)settings);

        private static string GetAdditionalUsings() => "using Refit;\n\n";
    }
}
#pragma warning restore SA1601 // Partial elements should be documented
