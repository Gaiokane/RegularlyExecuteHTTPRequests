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
        public static string default_url, default_json, default_cron, control_enable, isAuthorization, login_url, login_uid, login_pid;
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
            isAuthorization = RWConfig.GetappSettingsValue("isAuthorization", ConfigPath);
            login_url = RWConfig.GetappSettingsValue("login_url", ConfigPath);
            login_uid = RWConfig.GetappSettingsValue("login_uid", ConfigPath);
            login_pid = RWConfig.GetappSettingsValue("login_pid", ConfigPath);
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
            if (string.IsNullOrEmpty(isAuthorization))
            {
                RWConfig.SetappSettingsValue("isAuthorization", "true", ConfigPath);
            }
            if (string.IsNullOrEmpty(login_url))
            {
                RWConfig.SetappSettingsValue("login_url", "http://192.168.30.73:9092/api/proxy/auth/login", ConfigPath);
            }
            if (string.IsNullOrEmpty(login_uid))
            {
                RWConfig.SetappSettingsValue("login_uid", "用户名转的uid", ConfigPath);
            }
            if (string.IsNullOrEmpty(login_pid))
            {
                RWConfig.SetappSettingsValue("login_pid", "密码转的pid", ConfigPath);
            }
        }
        #endregion
    }
}