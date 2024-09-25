using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiCodeGenerator.Tests;
using static ApiCodeGenerator.Tests.Helpers;

namespace ApiCodeGenerator.OpenApi.Refit.Tests
{
    /// <summary>При указании в схеме максимальной длины строкового типа для него всегда генерится атрибут длины,
    /// но если заполнен атрибут format то в коде в свойствах могут использоваться типы отличные от string, что приводит к ошибкам валидации.</summary>
    public class Bug_2797046_StringLengthAttribute
    {
        [Test]
        public async Task NotSetStringLengthOnNotStringProperty()
        {
            //Arrange
            var settings = new RefitCodeGeneratorSettings
            {
                GenerateClientInterfaces = true,
                CSharpGeneratorSettings =
                {
                    Namespace = "TestNS",
                },
            };

            var json = @"{
              ""openapi"": ""3.0.1"",
              ""info"": {
                ""version"": ""1.0.0"",
                ""title"": ""test"",
                ""contact"": {
                  ""email"": ""apiteam@swagger.io""
                }
              },
              ""components"": {
                ""schemas"":{
                    ""MyType"" : {
                      ""type"": ""object"",
                      ""properties"": {
                        ""propDateTime"":{
                          ""type"": ""string"",
                          ""format"": ""date-time"",
                          ""maxLength"": 20,
                          ""minLength"": 10
                        },
                        ""propDuration"":{
                          ""type"": ""string"",
                          ""format"": ""duration"",
                          ""maxLength"": 20,
                          ""minLength"": 10
                        }
                      },
                      ""additionalProperties"": false
                    }
                }
              }
            }";
            var doc = await OpenApiDocument.FromJsonAsync(json);
            var expected = "    \n\n    " + GENERATED_CODE + @"
    public partial class MyType
    {
        [Newtonsoft.Json.JsonProperty(""propDateTime"", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset PropDateTime { get; set; }

        [Newtonsoft.Json.JsonProperty(""propDuration"", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.TimeSpan PropDuration { get; set; }

    }
".Replace("\r\n", "\n");

            //Act & Assert
            RunTest(settings, string.Empty, doc, expected);
        }

        [Test]
        public async Task NotSetStringLengthOnNotStringNestedProperty()
        {
            //Arrange
            var settings = new RefitCodeGeneratorSettings
            {
                GenerateClientInterfaces = true,
                CSharpGeneratorSettings =
                {
                    Namespace = "TestNS",
                },
            };

            var json = @"{
              ""openapi"": ""3.0.1"",
              ""info"": {
                ""version"": ""1.0.0"",
                ""title"": ""test"",
                ""contact"": {
                  ""email"": ""apiteam@swagger.io""
                }
              },
              ""components"": {
                ""schemas"":{
                    ""MyType"" : {
                      ""type"": ""object"",
                      ""properties"": {
                        ""prop"":{
                          ""type"": ""object"",
                          ""properties"":{
                              ""propDuration"":{
                                  ""type"": ""string"",
                                  ""format"": ""duration"",
                                  ""maxLength"": 20,
                                  ""minLength"": 10
                              }
                          },
                          ""additionalProperties"": false
                        },
                      },
                      ""additionalProperties"": false
                    },
                    ""Type2"":{
                      ""type"":""object"",
                      ""properties"":{
                        ""prop"":{
                            ""allOf"":[{""$ref"":""#/components/schemas/Type2""}]
                        }
                      },
                      ""additionalProperties"": false
                    }
                }
              }
            }";
            var doc = await OpenApiDocument.FromJsonAsync(json);
            var expected = ("    \n\n    " + GENERATED_CODE + @"
    public partial class MyType
    {
        [Newtonsoft.Json.JsonProperty(""prop"", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Prop Prop { get; set; }

    }

    " + GENERATED_CODE + @"
    public partial class Type2
    {
        [Newtonsoft.Json.JsonProperty(""prop"", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Type2 Prop { get; set; }

    }

    " + GENERATED_CODE + @"
    public partial class Prop
    {
        [Newtonsoft.Json.JsonProperty(""propDuration"", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.TimeSpan PropDuration { get; set; }

    }
").Replace("\r\n", "\n");

            //Act & Assert
            RunTest(settings, string.Empty, doc, expected);
        }
    }
}
