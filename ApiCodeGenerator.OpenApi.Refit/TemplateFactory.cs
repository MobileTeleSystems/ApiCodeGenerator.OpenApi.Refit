using System;
using System.Reflection;

namespace ApiCodeGenerator.OpenApi.Refit;

internal class TemplateFactory : DefaultTemplateFactory
{
    private static readonly string _toolchainVersion = typeof(TemplateFactory).GetTypeInfo().Assembly.GetName().Version.ToString();
    public TemplateFactory(NJsonSchema.CodeGeneration.CodeGeneratorSettingsBase settings, params Assembly[] assemblies)
        : base(settings, assemblies)
    {
    }

    protected override string GetToolchainVersion(Func<string> @base)
        => $"{_toolchainVersion} (NSwag v{base.GetToolchainVersion(@base)})";
}
