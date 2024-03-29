﻿using Gaiokane;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularlyExecuteHTTPRequests
{
    class ConfigSettings
    {
        public static string default_url, default_json, default_cron,
            control_enable, isAuthorization, login_url, login_uid, login_pid,
            form_text, result_substring,
            close_warning, close_warning_fontsize, is_show_mid_close_warning, mid_close_warning, mid_close_warning_fontsize;
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
            form_text = RWConfig.GetappSettingsValue("form_text", ConfigPath);
            result_substring = RWConfig.GetappSettingsValue("result_substring", ConfigPath);
            close_warning = RWConfig.GetappSettingsValue("close_warning", ConfigPath);
            close_warning_fontsize = RWConfig.GetappSettingsValue("close_warning_fontsize", ConfigPath);
            is_show_mid_close_warning = RWConfig.GetappSettingsValue("is_show_mid_close_warning", ConfigPath);
            mid_close_warning = RWConfig.GetappSettingsValue("mid_close_warning", ConfigPath);
            mid_close_warning_fontsize = RWConfig.GetappSettingsValue("mid_close_warning_fontsize", ConfigPath);
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
            if (string.IsNullOrEmpty(form_text))
            {
                RWConfig.SetappSettingsValue("form_text", "未配置窗体名称，可在配置文件中自定义", ConfigPath);
            }
            if (string.IsNullOrEmpty(result_substring))
            {
                RWConfig.SetappSettingsValue("result_substring", "", ConfigPath);
            }
            if (string.IsNullOrEmpty(close_warning))
            {
                RWConfig.SetappSettingsValue("close_warning", "数据同步中，请勿关闭！", ConfigPath);
            }
            if (string.IsNullOrEmpty(close_warning_fontsize))
            {
                RWConfig.SetappSettingsValue("close_warning_fontsize", "14", ConfigPath);
            }
            if (string.IsNullOrEmpty(is_show_mid_close_warning))
            {
                RWConfig.SetappSettingsValue("is_show_mid_close_warning", "1", ConfigPath);
            }
            if (string.IsNullOrEmpty(mid_close_warning))
            {
                RWConfig.SetappSettingsValue("mid_close_warning", "数据同步中，请勿关闭！", ConfigPath);
            }
            if (string.IsNullOrEmpty(mid_close_warning_fontsize))
            {
                RWConfig.SetappSettingsValue("mid_close_warning_fontsize", "30", ConfigPath);
            }
        }
        #endregion
    }
}