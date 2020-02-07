using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BW.Demo.Functions.Conversionator
{
    public class CurrencyUtility
    {
        public static decimal ConvertCurrency(decimal Amount, string Source, string Target)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(Constants.CurrencyConversionUrl, Source, Target));
                request.Method = "GET";
                request.ContentType = "application/json;charset=utf-8";
                request.Accept = "application/json";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream data = response.GetResponseStream();
                StreamReader reader = new StreamReader(data);
                string result = reader.ReadToEnd();
                reader.Close();

                dynamic conversion = JsonConvert.DeserializeObject(result);
                decimal val = Convert.ToDecimal(conversion.rates[Target]);
                return Amount * val;
            }
            catch
            {
                return 0;
            }
        }
    }
}