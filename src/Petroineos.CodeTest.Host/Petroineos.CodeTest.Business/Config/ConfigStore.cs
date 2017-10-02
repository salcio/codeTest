using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petroineos.CodeTest.Business.Config
{
    public class ConfigStore: IConfigStore
    {
        public int ReportIntervalInMinutes => Get("ReportIntervalInMinutes", false, 1);
        public int MaxRetriesOnServiceError => Get("MaxRetriesOnServiceError", false, 3);
        public int DelayBetweenRetiesInMiliseconds => Get("DelayBetweenRetiesInMiliseconds", false, 500);

        private T Get<T>(string reportinterval, bool required, T defaultValue)
        {
            var value = ConfigurationManager.AppSettings[reportinterval];
            if (string.IsNullOrWhiteSpace(value))
            {
                if (required)
                {
                    throw new ConfigurationErrorsException($"Missing value for '{reportinterval}' app setting.");
                }
                return defaultValue;
            }
            return (T) new TypeConverter().ConvertTo(value,typeof(T));
        }
    }
}
