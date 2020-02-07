using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Demo.Functions.Conversionator
{
    public class DateTimeUtility
    {
        public static string GetCurrentDateTime()
        {
            string result = string.Empty;

            try
            {
                result = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            }
            catch
            {
                //TODO: Log Error
            }

            return result;
        }

        public static DateTime? GetWorldDateTime(DateTime input, string sourceZone, string targetZone)
        {
            try
            {
                TimeZoneInfo sz = TimeZoneInfo.FindSystemTimeZoneById(sourceZone);
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(targetZone);
                return TimeZoneInfo.ConvertTime(input, sz, tz);
            }
            catch
            {
                return null;
            }
        }

        public static double GetGMTOffset(string zone)
        {
            double result = 0;

            switch (zone)
            {
                case "GMT":
                    result = 0;
                    break;
                case "UTC":
                    result = 0;
                    break;
                case "ECT":
                    result = 1;
                    break;
                case "EET":
                    result = 2;
                    break;
                case "ART":
                    result = 2;
                    break;
                case "EAT":
                    result = 3;
                    break;
                case "MET":
                    result = 3.5;
                    break;
                case "NET":
                    result = 4;
                    break;
                case "PLT":
                    result = 5;
                    break;
                case "IST":
                    result = 5.5;
                    break;
                case "BST":
                    result = 6;
                    break;
                case "VST":
                    result = 7;
                    break;
                case "CTT":
                    result = 8;
                    break;
                case "JST":
                    result = 9;
                    break;
                case "ACT":
                    result = 9.5;
                    break;
                case "AET":
                    result = 10;
                    break;
                case "SST":
                    result = 11;
                    break;
                case "NST":
                    result = 12;
                    break;
                case "MIT":
                    result = -11;
                    break;
                case "HST":
                    result = -10;
                    break;
                case "AST":
                    result = -9;
                    break;
                case "PST":
                    result = -8;
                    break;
                case "PNT":
                    result = -7;
                    break;
                case "MST":
                    result = -7;
                    break;
                case "CST":
                    result = -6;
                    break;
                case "EST":
                    result = -5;
                    break;
                case "IET":
                    result = -5;
                    break;
                case "PRT":
                    result = -4;
                    break;
                case "CNT":
                    result = -3.5;
                    break;
                case "AGT":
                    result = -3;
                    break;
                case "BET":
                    result = -3;
                    break;
                case "CAT":
                    result = -1;
                    break;
                default:
                    result = 0;
                    break;
            }

            return result;
        }
    }
}
