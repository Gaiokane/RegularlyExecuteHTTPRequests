using Gaiokane;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularlyExecuteHTTPRequests
{
    class ConfigSettings
    {
        public static string default_url, default_json, default_cron, control_enable;
        public static string ConfigPath = "./RegularlyExecuteHTTPRequests.exe";

        #region 获取配置文件中配置
        /// <summary>
        /// 获取配置文件中配置
        /// </summary>
        public static void getConfigSettings()
        {
            default_url = RWConfig.GetappSettingsValue("default_url", ConfigPath);
            default_json = RWConfig.GetappSettingsValue("default_json", ConfigPath);
            default_cron = RWConfig.GetappSettingsValue("default_cron", ConfigPath);
            control_enable = RWConfig.GetappSettingsValue("control_enable", ConfigPath);
        }
        #endregion

        #region 如果配置文件中无配置，则新建配置，值默认
        /// <summary>
        /// 如果配置文件中无配置，则新建配置，值默认
        /// </summary>
        public static void setDefaultConfigSettings()
        {
            if (string.IsNullOrEmpty(default_url))
            {
                RWConfig.SetappSettingsValue("default_url", "http://127.0.0.1:9902/api/service", ConfigPath);
            }
            if (string.IsNullOrEmpty(default_json))
            {
                RWConfig.SetappSettingsValue("default_json", "{\"fromTime\":\"{{onthehour-48}}\",\"toTime\":\"{{onthehour-1}}\"}", ConfigPath);
            }
            if (string.IsNullOrEmpty(default_cron))
            {
                RWConfig.SetappSettingsValue("default_cron", "0 17 0 * * ?", ConfigPath);
            }
            if (string.IsNullOrEmpty(control_enable))
            {
                RWConfig.SetappSettingsValue("control_enable", "false", ConfigPath);
            }
        }
        #endregion
    }
}
