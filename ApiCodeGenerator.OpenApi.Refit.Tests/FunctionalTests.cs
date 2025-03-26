using System;
using System.Linq;
using NSwag;
using NSwag.CodeGeneration.OperationNameGenerators;
using static ApiCodeGenerator.Tests.Helpers;

namespace ApiCodeGenerator.OpenApi.Refit.Tests
{
    public class FunctionalTests
    {
        [Test]
        public void GenerateClientInterface()
        {
            //Arrange
            var settings = new RefitCodeGeneratorSettings
            {
                GenerateClientInterfaces = true,
            };
            settings.CSharpGeneratorSettings.Namespace = "TestNS";
            var expected =
                "    public partial interface IClient\n" +
                "    {\n" +
                "        /// <summary>\n" +
                "        /// Test operation\n" +
                "        /// </summary>\n" +
                "        /// <param name=\"headerParametr\">Параметр передаваемый в качестве заголовка</param>\n" +
                "        /// <param name=\"queryParametr\">Параметр передаваемый в строке запроса</param>\n" +
                "        /// <returns>Запрос успешно принят</returns>\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Get(\"/testService/testOper\")]\n" +
                "        System.Threading.Tasks.Task<TestOperResponse> GetTestOper([Header(\"headerParametr\")]string headerParametr, [Query]int? queryParametr);\n" +
                "\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Post(\"/testService/testOper\")]\n" +
                "        System.Threading.Tasks.Task CreateTestOper();\n" +
                "\n" +
                "    }\n";

            //Act & Assert
            RunTest(settings, expected);
        }

        [Test]
        public void GenerateClientInterface_Query()
        {
            //Arrange
            var settings = new RefitCodeGeneratorSettings
            {
                GenerateClientInterfaces = true,
            };
            settings.CSharpGeneratorSettings.Namespace = "TestNS";
            var expected =
                "    public partial interface IClient\n" +
                "    {\n" +
                "        /// <summary>\n" +
                "        /// Test operation\n" +
                "        /// </summary>\n" +
                "        /// <param name=\"headerParametr\">Параметр передаваемый в качестве заголовка</param>\n" +
                "        /// <param name=\"queryParametr\">Параметр передаваемый в строке запроса</param>\n" +
                "        /// <param name=\"doted_queryParametr\">Параметр передаваемый в строке запроса содержащий точку</param>\n" +
                "        /// <returns>Запрос успешно принят</returns>\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Get(\"/testService/testOper\")]\n" +
                "        System.Threading.Tasks.Task<TestOperResponse> GetTestOper([Header(\"headerParametr\")]string headerParametr, [Query(CollectionFormat.Multi)]System.Collections.Generic.IEnumerable<string> queryParametr, [Query][AliasAs(\"doted.queryParametr\")]string doted_queryParametr);\n" +
                "\n" +
                "        /// <returns>OK</returns>\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Post(\"/testService/testOper\")]\n" +
                "        System.Threading.Tasks.Task CreateTestOper([Body]string a);\n" +
                "\n" +
                "    }\n";

            //Act & Assert
            RunTest(settings, expected, "testSchema2.json");
        }

        [Test]
        public void GenerateClientInterface_HeaderWithAlias()
        {
            //Arrange
            var headerParam = new OpenApiParameter { Name = "doted.name", Kind = OpenApiParameterKind.Header, Type = NJsonSchema.JsonObjectType.String };
            var operation = new OpenApiOperation
            {
                OperationId = "operation",
            };
            operation.Parameters.Add(headerParam);
            var apidocument = new OpenApiDocument();
            apidocument.Paths["operation"] = new OpenApiPathItem
            {
                [OpenApiOperationMethod.Get] = operation,
            };

            var testOperResponseText = "    \n";

            var settings = new RefitCodeGeneratorSettings
            {
                GenerateClientInterfaces = true,
                CSharpGeneratorSettings = { Namespace = "TestNS" },
            };

            var expected =
                "    public partial interface IClient\n" +
                "    {\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Get(\"/operation\")]\n" +
                "        System.Threading.Tasks.Task Operation([Header(\"doted.name\")]string doted_name);\n" +
                "\n" +
                "    }\n";

            //Act & Assert
            RunTest(settings, expected, apidocument, testOperResponseText);
        }

        [Test]
        public void GenerateClientInterface_BaseInterface()
        {
            //Arrange
            var settings = new RefitCodeGeneratorSettings
            {
                GenerateClientInterfaces = true,
                ClientBaseInterface = "IBase, IBase2",
                CSharpGeneratorSettings = { Namespace = "TestNS" },
            };
            var expected =
                "    public partial interface IClient : IBase, IBase2\n" +
                "    {\n" +
                "        /// <summary>\n" +
                "        /// Test operation\n" +
                "        /// </summary>\n" +
                "        /// <param name=\"headerParametr\">Параметр передаваемый в качестве заголовка</param>\n" +
                "        /// <param name=\"queryParametr\">Параметр передаваемый в строке запроса</param>\n" +
                "        /// <returns>Запрос успешно принят</returns>\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Get(\"/testService/testOper\")]\n" +
                "        System.Threading.Tasks.Task<TestOperResponse> GetTestOper([Header(\"headerParametr\")]string headerParametr, [Query]int? queryParametr);\n" +
                "\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Post(\"/testService/testOper\")]\n" +
                "        System.Threading.Tasks.Task CreateTestOper();\n" +
                "\n" +
                "    }\n";

            //Act & Assert
            RunTest(settings, expected);
        }

        [Test]
        public void GenerateClientInterface_InterfaceAccesModifier()
        {
            //Arrange
            var settings = new RefitCodeGeneratorSettings
            {
                GenerateClientInterfaces = true,
                InterfaceAccessModifier = "internal",
            };
            settings.CSharpGeneratorSettings.Namespace = "TestNS";
            var expected =
                "    internal partial interface IClient\n" +
                "    {\n" +
                "        /// <summary>\n" +
                "        /// Test operation\n" +
                "        /// </summary>\n" +
                "        /// <param name=\"headerParametr\">Параметр передаваемый в качестве заголовка</param>\n" +
                "        /// <param name=\"queryParametr\">Параметр передаваемый в строке запроса</param>\n" +
                "        /// <returns>Запрос успешно принят</returns>\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Get(\"/testService/testOper\")]\n" +
                "        System.Threading.Tasks.Task<TestOperResponse> GetTestOper([Header(\"headerParametr\")]string headerParametr, [Query]int? queryParametr);\n" +
                "\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Post(\"/testService/testOper\")]\n" +
                "        System.Threading.Tasks.Task CreateTestOper();\n" +
                "\n" +
                "    }\n";

            //Act & Assert
            RunTest(settings, expected);
        }

        [Test]
        public void GenerateClientInterface_AsyncSuffix_CancelationToken()
        {
            //Arrange
            var settings = new RefitCodeGeneratorSettings
            {
                GenerateClientInterfaces = true,
                OperationAsyncSuffix = true,
                GenerateOptionalParameters = true,
                OperationCancelationToken = true,
                CSharpGeneratorSettings = { Namespace = "TestNS" },
            };

            var expected =
                "    public partial interface IClient\n" +
                "    {\n" +
                "        /// <param name=\"cancellationToken\">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>\n" +
                "        /// <summary>\n" +
                "        /// Test operation\n" +
                "        /// </summary>\n" +
                "        /// <param name=\"headerParametr\">Параметр передаваемый в качестве заголовка</param>\n" +
                "        /// <param name=\"queryParametr\">Параметр передаваемый в строке запроса</param>\n" +
                "        /// <returns>Запрос успешно принят</returns>\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Get(\"/testService/testOper\")]\n" +
                "        System.Threading.Tasks.Task<TestOperResponse> GetTestOperAsync([Header(\"headerParametr\")]string headerParametr, [Query]int? queryParametr = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));\n" +
                "\n" +
                "        /// <param name=\"cancellationToken\">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Post(\"/testService/testOper\")]\n" +
                "        System.Threading.Tasks.Task CreateTestOperAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));\n" +
                "\n" +
                "    }\n";

            //Act & Assert
            RunTest(settings, expected);
        }

        [Test]
        public void GenerateClientInterface_ApiResponseOperations()
        {
            //Arrange
            var settings = new RefitCodeGeneratorSettings
            {
                GenerateClientInterfaces = true,
                WrapResponses = true,
                WrapResponseMethods = new[] { "Client.GetTestOper" },
            };
            settings.CSharpGeneratorSettings.Namespace = "TestNS";
            var expectedIntefaceDeclaration =
                "    public partial interface IClient\n" +
                "    {\n" +
                "        /// <summary>\n" +
                "        /// Test operation\n" +
                "        /// </summary>\n" +
                "        /// <param name=\"headerParametr\">Параметр передаваемый в качестве заголовка</param>\n" +
                "        /// <param name=\"queryParametr\">Параметр передаваемый в строке запроса</param>\n" +
                "        /// <returns>Запрос успешно принят</returns>\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Get(\"/testService/testOper\")]\n" +
                "        System.Threading.Tasks.Task<TestOperResponse> GetTestOper([Header(\"headerParametr\")]string headerParametr, [Query]int? queryParametr);\n" +
                "\n" +
                "        /// <summary>\n" +
                "        /// Test operation\n" +
                "        /// </summary>\n" +
                "        /// <param name=\"headerParametr\">Параметр передаваемый в качестве заголовка</param>\n" +
                "        /// <param name=\"queryParametr\">Параметр передаваемый в строке запроса</param>\n" +
                "        /// <returns>Запрос успешно принят</returns>\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Get(\"/testService/testOper\")]\n" +
                "        System.Threading.Tasks.Task<IApiResponse<TestOperResponse>> GetTestOperApiResponse([Header(\"headerParametr\")]string headerParametr, [Query]int? queryParametr);\n" +
                "\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Post(\"/testService/testOper\")]\n" +
                "        System.Threading.Tasks.Task CreateTestOper();\n" +
                "\n" +
                "    }\n";

            //Act & Assert
            RunTest(settings, expectedIntefaceDeclaration);
        }

        [Test]
        public void GenerateClientInterface_ApiResponseOperations_PathExtract()
        {
            //Arrange
            var settings = new RefitCodeGeneratorSettings
            {
                GenerateClientInterfaces = true,
                PathExtractExpression = @"^testService\/(.*)$",
                WrapResponses = true,
                WrapResponseMethods = new[] { "Client.GetTestOper" },
            };
            settings.CSharpGeneratorSettings.Namespace = "TestNS";
            var expectedIntefaceDeclaration =
                "    public partial interface IClient\n" +
                "    {\n" +
                "        /// <summary>\n" +
                "        /// Test operation\n" +
                "        /// </summary>\n" +
                "        /// <param name=\"headerParametr\">Параметр передаваемый в качестве заголовка</param>\n" +
                "        /// <param name=\"queryParametr\">Параметр передаваемый в строке запроса</param>\n" +
                "        /// <returns>Запрос успешно принят</returns>\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Get(\"/testOper\")]\n" +
                "        System.Threading.Tasks.Task<TestOperResponse> GetTestOper([Header(\"headerParametr\")]string headerParametr, [Query]int? queryParametr);\n" +
                "\n" +
                "        /// <summary>\n" +
                "        /// Test operation\n" +
                "        /// </summary>\n" +
                "        /// <param name=\"headerParametr\">Параметр передаваемый в качестве заголовка</param>\n" +
                "        /// <param name=\"queryParametr\">Параметр передаваемый в строке запроса</param>\n" +
                "        /// <returns>Запрос успешно принят</returns>\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Get(\"/testOper\")]\n" +
                "        System.Threading.Tasks.Task<IApiResponse<TestOperResponse>> GetTestOperApiResponse([Header(\"headerParametr\")]string headerParametr, [Query]int? queryParametr);\n" +
                "\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Post(\"/testOper\")]\n" +
                "        System.Threading.Tasks.Task CreateTestOper();\n" +
                "\n" +
                "    }\n";

            //Act & Assert
            RunTest(settings, expectedIntefaceDeclaration);
        }

        [Test]
        public void GenerateClientInterface_MultipleClientsFromPathSegments()
        {
            var settings = new RefitCodeGeneratorSettings
            {
                GenerateClientInterfaces = true,
                OperationNameGenerator = new MultipleClientsFromPathSegmentsOperationNameGenerator(),
            };
            settings.CSharpGeneratorSettings.Namespace = "TestNS";
            var document = OpenApiDocument.FromFileAsync(GetSwaggerPath("testSchema.json")).Result;
            var openApiPathItem = new OpenApiPathItem
            {
                ["get"] = new OpenApiOperation { OperationId = "GetValueUsingGET" },
            };
            document.Paths["testService2/Value"] = openApiPathItem;
            var generator = new RefitCodeGenerator(document, settings);

            var expectedInterfaceDeclaration =
                "    " + GENERATED_CODE_ATTRIBUTE + "\n" +
                "    public partial interface ITestServiceClient\n" +
                "    {\n" +
                "        /// <summary>\n" +
                "        /// Test operation\n" +
                "        /// </summary>\n" +
                "        /// <param name=\"headerParametr\">Параметр передаваемый в качестве заголовка</param>\n" +
                "        /// <param name=\"queryParametr\">Параметр передаваемый в строке запроса</param>\n" +
                "        /// <returns>Запрос успешно принят</returns>\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Get(\"/testService/testOper\")]\n" +
                "        System.Threading.Tasks.Task<TestOperResponse> TestOperGet([Header(\"headerParametr\")]string headerParametr, [Query]int? queryParametr);\n" +
                "\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Post(\"/testService/testOper\")]\n" +
                "        System.Threading.Tasks.Task TestOperPost();\n" +
                "\n" +
                "    }\n" +
                "\n" +
                "    " + GENERATED_CODE_ATTRIBUTE + "\n" +
                "    public partial interface ITestService2Client\n" +
                "    {\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Get(\"/testService2/Value\")]\n" +
                "        System.Threading.Tasks.Task Value();\n" +
                "\n" +
                "    }\n";

            var expected = GetExpectedCode(expectedInterfaceDeclaration);

            //Act
            var actual = generator.GenerateFile();

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GenerateClientInterface_PathExtract()
        {
            //Arrange
            var settings = new RefitCodeGeneratorSettings
            {
                GenerateClientInterfaces = true,
                PathExtractExpression = @"^testService\/(.*)$",
            };
            settings.CSharpGeneratorSettings.Namespace = "TestNS";
            var expected =
                "    public partial interface IClient\n" +
                "    {\n" +
                "        /// <summary>\n" +
                "        /// Test operation\n" +
                "        /// </summary>\n" +
                "        /// <param name=\"headerParametr\">Параметр передаваемый в качестве заголовка</param>\n" +
                "        /// <param name=\"queryParametr\">Параметр передаваемый в строке запроса</param>\n" +
                "        /// <returns>Запрос успешно принят</returns>\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Get(\"/testOper\")]\n" +
                "        System.Threading.Tasks.Task<TestOperResponse> GetTestOper([Header(\"headerParametr\")]string headerParametr, [Query]int? queryParametr);\n" +
                "\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Post(\"/testOper\")]\n" +
                "        System.Threading.Tasks.Task CreateTestOper();\n" +
                "\n" +
                "    }\n";

            //Act & Assert
            RunTest(settings, expected);
        }

        [Test]
        public void GenerateClientInterface_PathExtract_NotMatched()
        {
            //Arrange
            var settings = new RefitCodeGeneratorSettings
            {
                GenerateClientInterfaces = true,
                PathExtractExpression = @"^notmatch\/(.*)$",
            };
            settings.CSharpGeneratorSettings.Namespace = "TestNS";
            var expected =
                "    public partial interface IClient\n" +
                "    {\n" +
                "        /// <summary>\n" +
                "        /// Test operation\n" +
                "        /// </summary>\n" +
                "        /// <param name=\"headerParametr\">Параметр передаваемый в качестве заголовка</param>\n" +
                "        /// <param name=\"queryParametr\">Параметр передаваемый в строке запроса</param>\n" +
                "        /// <returns>Запрос успешно принят</returns>\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Get(\"/testService/testOper\")]\n" +
                "        System.Threading.Tasks.Task<TestOperResponse> GetTestOper([Header(\"headerParametr\")]string headerParametr, [Query]int? queryParametr);\n" +
                "\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Post(\"/testService/testOper\")]\n" +
                "        System.Threading.Tasks.Task CreateTestOper();\n" +
                "\n" +
                "    }\n";

            //Act & Assert
            RunTest(settings, expected);
        }

        [Test]
        public void GenerateClientInterface_BringDefaultParamsToEnd()
        {
            //Arrange
            var settings = new RefitCodeGeneratorSettings
            {
                GenerateClientInterfaces = true,
            };
            settings.CSharpGeneratorSettings.Namespace = "TestNS";
            settings.GenerateOptionalParameters = true;
            var expected =
                "    public partial interface IClient\n" +
                "    {\n" +
                "        /// <summary>\n" +
                "        /// Test operation\n" +
                "        /// </summary>\n" +
                "        /// <param name=\"queryParametr\">Параметр передаваемый в строке запроса</param>\n" +
                "        /// <param name=\"paramWithDef\">Параметр со значением по умолчанию</param>\n" +
                "        /// <returns>Запрос успешно принят</returns>\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Get(\"/testService/testOper\")]\n" +
                "        System.Threading.Tasks.Task<TestOperResponse> GetTestOper([Query]string queryParametr, [Query]string paramWithDef = null);\n" +
                "\n" +
                "    }\n";

            //Act & Assert
            RunTest(settings, expected, "defaultParamTestSchema.json");
        }

        [Test]
        public void GenerateClientInterface_NotBringDefaultParamsToEnd_IfOptParamDisabled()
        {
            //Arrange
            var settings = new RefitCodeGeneratorSettings
            {
                GenerateClientInterfaces = true,
            };
            settings.CSharpGeneratorSettings.Namespace = "TestNS";
            settings.GenerateOptionalParameters = false;
            var expected =
                "    public partial interface IClient\n" +
                "    {\n" +
                "        /// <summary>\n" +
                "        /// Test operation\n" +
                "        /// </summary>\n" +
                "        /// <param name=\"paramWithDef\">Параметр со значением по умолчанию</param>\n" +
                "        /// <param name=\"queryParametr\">Параметр передаваемый в строке запроса</param>\n" +
                "        /// <returns>Запрос успешно принят</returns>\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Get(\"/testService/testOper\")]\n" +
                "        System.Threading.Tasks.Task<TestOperResponse> GetTestOper([Query]string paramWithDef, [Query]string queryParametr);\n" +
                "\n" +
                "    }\n";

            //Act & Assert
            RunTest(settings, expected, "defaultParamTestSchema.json");
        }

        [Test]
        public void ExcludeParam()
        {
            var settings = new RefitCodeGeneratorSettings
            {
                GenerateClientInterfaces = true,
                ExcludedParameterNames = [
                    "paramWithDef"
                ],
                CSharpGeneratorSettings = { Namespace = "TestNS" },
            };

            var expected =
                        "    public partial interface IClient\n" +
                        "    {\n" +
                        "        /// <summary>\n" +
                        "        /// Test operation\n" +
                        "        /// </summary>\n" +
                        "        /// <param name=\"queryParametr\">Параметр передаваемый в строке запроса</param>\n" +
                        "        /// <returns>Запрос успешно принят</returns>\n" +
                        "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                        "        [Get(\"/testService/testOper\")]\n" +
                        "        System.Threading.Tasks.Task<TestOperResponse> GetTestOper([Query]string queryParametr);\n" +
                        "\n" +
                        "    }\n";

            //Act & Assert
            RunTest(settings, expected, "defaultParamTestSchema.json");
        }

        [Test]
        public void GenerateClientInterface_AuthHeaderParameter()
        {
            //Arrange
            var settings = new RefitCodeGeneratorSettings
            {
                GenerateClientInterfaces = true,
                AuthorizationHeaderParameter = true,
            };
            settings.CSharpGeneratorSettings.Namespace = "TestNS";
            settings.GenerateOptionalParameters = false;
            var expected =
                "    public partial interface IClient\n" +
                "    {\n" +
                "        /// <summary>\n" +
                "        /// Test operation\n" +
                "        /// </summary>\n" +
                "        /// <param name=\"queryParametr\">Параметр передаваемый в строке запроса</param>\n" +
                "        /// <returns>Запрос успешно принят</returns>\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Get(\"/testService/testOper\")]\n" +
                "        System.Threading.Tasks.Task<TestOperResponse> GetTestOper([Query]string queryParametr, [Header(\"Authorization\")]string authorization);\n" +
                "\n" +
                "    }\n";

            //Act & Assert
            RunTest(settings, expected, "authSchema.json");
        }

        [Test]
        public void GenerateClientInterface_BinaryResponse()
        {
            var settings = new RefitCodeGeneratorSettings
            {
                BinaryResponseType = "System.Net.Http.HttpContent",
                GenerateClientInterfaces = true,
                GenerateOptionalParameters = false,
                CSharpGeneratorSettings =
                {
                    Namespace = "TestNS",
                },
            };

            var expected =
                "    public partial interface IClient\n" +
                "    {\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Get(\"/download\")]\n" +
                "        System.Threading.Tasks.Task<System.Net.Http.HttpContent> Download();\n" +
                "\n" +
                "    }\n";

            RunTest(settings, expected, "streamResponse.yaml", "    \n");
        }
    }
}
