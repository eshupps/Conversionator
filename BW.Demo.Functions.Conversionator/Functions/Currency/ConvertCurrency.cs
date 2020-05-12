using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BW.Demo.Functions.Conversionator
{
    public static class ConvertCurrency
    {
        [FunctionName("ConvertCurrency")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get","post", Route = "api/convertcurrency")] HttpRequest req,
            ILogger log, Microsoft.Azure.WebJobs.ExecutionContext context)
        {
            //Global variables
            string product = Constants.Product_CoreFunctions;
            Guid correlationId = Guid.NewGuid();
            string inputValue = req.Query["input"];
            string sourceValue = req.Query["source"];
            string targetValue = req.Query["target"];

            try
            {
                string requestBody = string.Empty;
                using (StreamReader sr = new StreamReader(req.Body))
                {
                    requestBody = await sr.ReadToEndAsync(); 
                }
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                inputValue = inputValue ?? data?.input.ToString();
                sourceValue = sourceValue ?? data?.source.ToString();
                targetValue = targetValue ?? data?.target.ToString();

                log.LogInformation(String.Format("{0},{1},{2},{3},Executing request to convert currency", correlationId, DateTimeUtility.GetCurrentDateTime(), context.FunctionName, product));
            }
            catch (System.Exception ex)
            {
                log.LogError(String.Format("{0},{1},{2},{3},{4},Request data error. Message: {4}'", correlationId, DateTimeUtility.GetCurrentDateTime(), context.FunctionName, product, ex.Message));

                return new BadRequestObjectResult(new BasicStringResponse { value = "Error: Invalid request data - " + ex.Message });
            }

            log.LogInformation(String.Format("{0},{1},{2},Initiated function", correlationId, DateTimeUtility.GetCurrentDateTime(), context.FunctionName));

            //Execute function logic

            if (!string.IsNullOrEmpty(inputValue) && !string.IsNullOrEmpty(sourceValue) && !string.IsNullOrEmpty(targetValue))
            {
                decimal cResult = CurrencyUtility.ConvertCurrency(Convert.ToDecimal(inputValue), sourceValue, targetValue);

                if (cResult > 0)
                {
                    NumericDecimalResponse response = new NumericDecimalResponse
                    {
                        value = Decimal.Round(cResult, 2)
                    };

                    log.LogInformation(String.Format("{0},{1},{2},{3},Operation succeeded.", correlationId, DateTimeUtility.GetCurrentDateTime(), context.FunctionName, product));

                    return (ActionResult)new OkObjectResult(response);
                }
                else
                {
                    log.LogError(String.Format("{0},{1},{2},Function error: {3}", correlationId, DateTimeUtility.GetCurrentDateTime(), context.FunctionName, Localization.Global.Error_Text_OperationFailed));

                    return new BadRequestObjectResult(new BasicStringResponse { value = "Error: Invalid request" });
                }
            }
            else
            {
                log.LogError(String.Format("{0},{1},{2},Function error: {3}", correlationId, DateTimeUtility.GetCurrentDateTime(), context.FunctionName, Localization.Global.Error_Text_ParameterMissing));

                return new BadRequestObjectResult(new BasicStringResponse { value = "Error: " + Localization.Global.Error_Text_ParameterMissing });
            }

        }
    }
}
