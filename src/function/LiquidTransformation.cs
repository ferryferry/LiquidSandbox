using System;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MasterData.Repositories.Helpers;
using Microsoft.OpenApi.Models;

namespace LiquidTransformation
{
    public static class LiquidTransformation
    {
        [FunctionName("LiquidTransformation")]
        [OpenApiOperation("transform", "Liquid Json Transformation")]
        [OpenApiRequestBody("application/json", typeof(LiquidTransformationProperties))]
        [OpenApiResponseBody(HttpStatusCode.OK, "application/json", typeof(string))]
        public static async Task<IActionResult> TransformJson(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var liquidTransformationProperties = JsonConvert.DeserializeObject<LiquidTransformationProperties>(requestBody);

            if(string.IsNullOrEmpty(liquidTransformationProperties.Json) || string.IsNullOrWhiteSpace(liquidTransformationProperties.Json))
                return new BadRequestObjectResult("Specified Json is empty!");

            if(string.IsNullOrEmpty(liquidTransformationProperties.LiquidTemplate) || string.IsNullOrWhiteSpace(liquidTransformationProperties.LiquidTemplate))
                return new BadRequestObjectResult("Specified Liquid template is empty!");
                

            var transformedJson = LiquidTransformationHelper.Transform(liquidTransformationProperties.Json, liquidTransformationProperties.LiquidTemplate);

            return new OkObjectResult(transformedJson);
        }
    }
}
