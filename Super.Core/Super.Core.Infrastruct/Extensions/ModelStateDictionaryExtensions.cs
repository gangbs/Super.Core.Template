using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;


namespace Super.Core.Infrastruct.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        public static string GetErrorMessage(this ModelStateDictionary dictionary)
        {
            string errorMsg = string.Empty;
            foreach (var item in dictionary.Values)
            {
                if (item.Errors != null && item.Errors.Count > 0)
                {
                    errorMsg = item.Errors[0].ErrorMessage;
                    break;
                }
            }
            return errorMsg;
        }

        public static List<string> GetAllErrorMessage(this ModelStateDictionary dictionary)
        {
            var lstError = new List<string>();
            foreach (var item in dictionary.Values)
            {
                if (item.Errors != null && item.Errors.Count > 0)
                {
                    foreach (var error in item.Errors)
                    {
                        lstError.Add(error.ErrorMessage);
                    }
                }
            }
            return lstError;
        }
    }
}
