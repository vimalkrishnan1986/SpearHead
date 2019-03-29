using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;

namespace SpearHead.Common.Helpers
{
    public static class ConfigHelper
    {
        public static T GetAppSettingValue<T>(string key)
        {
            if (ConfigurationManager.AppSettings[key] == null)
            {
                throw new ArgumentNullException(key);
            }

            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(ConfigurationManager.AppSettings[key]);
        }


        public static string GetConnectionString(string key)
        {
            var configuration = ConfigurationManager.ConnectionStrings[key];

            if (configuration == null)
            {
                throw new KeyNotFoundException(nameof(key));
            }
            return configuration.ConnectionString;
        }
    }
}
