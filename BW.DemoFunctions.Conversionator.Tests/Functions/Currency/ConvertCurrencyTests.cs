using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http.Internal;
using System.IO;
using BW.Demo.Functions.Conversionator;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace BW.Demo.Functions.Conversionator.Tests
{
    [TestClass]
    public class ConvertCurrencyTests : FunctionTest
    {
        [TestMethod]
        public async Task CurrencyTest_Positive()
        {
            decimal d = 10.52m;
            string s = "USD";
            string t = "GBP";

            var query = new Dictionary<String, StringValues>();
            var body = JsonConvert.SerializeObject(new
            {
                input = d.ToString(),
                source = s,
                target = t
            });

            var result = await BW.Demo.Functions.Conversionator.ConvertCurrency.Run(req: HttpRequestSetup(query, body), log: log, context: new Microsoft.Azure.WebJobs.ExecutionContext());
            var resultObject = (OkObjectResult)result;
            BW.Demo.Functions.Conversionator.NumericDecimalResponse response = (BW.Demo.Functions.Conversionator.NumericDecimalResponse)resultObject.Value;
            bool b = response.value > 0;
            Assert.AreEqual(true, b);
        }
    }
}