using System.Collections.Generic;
using NJsonSchema;
using NJsonSchema.CodeGeneration;
using NSwag;
using NSwag.CodeGeneration;
using NSwag.CodeGeneration.CSharp.Models;

namespace ApiCodeGenerator.OpenApi.Refit.Model
{
    public class RefitParameterModel : CSharpParameterModel
    {
        public RefitParameterModel(string parameterName, string variableName, string variableIdentifier, string typeName, OpenApiParameter parameter, IList<OpenApiParameter> allParameters, CodeGeneratorSettingsBase settings, IClientGenerator generator, TypeResolverBase typeResolver)
            : base(parameterName, variableName, variableIdentifier, typeName, parameter, allParameters, settings, generator, typeResolver)
        {
            CollectionFormat = settings.SchemaType == NJsonSchema.SchemaType.OpenApi3
                ? GetCollectionFormatOpenApi3(parameter)
                : GetCollectionFormatString(parameter);

            PropertyName = ConversionUtilities.ConvertToUpperCamelCase(variableName, false);
        }

        public string CollectionFormat { get; }

        public bool IsAliased
            => !IsHeader && Name != VariableName;

        public string PropertyName { get; }

        private static string GetCollectionFormatString(OpenApiParameter parameter)
        {
            return !parameter.IsArray
                ? null
                : parameter.CollectionFormat == OpenApiParameterCollectionFormat.Undefined
                    ? "Multi"
                    : parameter.CollectionFormat.ToString();
        }

        private static string GetCollectionFormatOpenApi3(OpenApiParameter parameter)
        {
            if (parameter.Schema?.IsArray == true)
            {
                if (parameter.Explode)
                {
                    if (parameter.Style == OpenApiParameterStyle.Form
                        || parameter.Style == OpenApiParameterStyle.Undefined
                        || parameter.Style == OpenApiParameterStyle.SpaceDelimeted
                        || parameter.Style == OpenApiParameterStyle.PipeDelimited)
                    {
                        return "Multi";
                    }
                }
                else
                {
                    switch (parameter.Style)
                    {
                        case OpenApiParameterStyle.Form:
                            return "Csv";
                        case OpenApiParameterStyle.SpaceDelimeted:
                            return "Ssv";
                        case OpenApiParameterStyle.PipeDelimited:
                            return "Pipes";
                        case OpenApiParameterStyle.Undefined:
                            return "Multi";
                    }
                }

                return $"\"Collection style '{parameter.Style}'\" not supported";
            }

            return null;
        }
    }
}
