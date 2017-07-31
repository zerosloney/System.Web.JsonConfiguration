using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace System.Web.JsonConfiguration
{
    public static class JsonConfigurationManager
    {
        static IJsonConfigurationMonitor montior = new RxJsonConfigurationMonitor();
        static string _jsonFileName;
        static string JsonFileName
        {
            get
            {
                if (string.IsNullOrEmpty(_jsonFileName))
                {
                    throw new ArgumentNullException("请先确保Set方法已经执行");
                }
                return _jsonFileName;
            }
        }
        /// <summary>
        /// 设置并监听
        /// </summary>
        /// <param name="jsonFileName">json文件(eg:'settings.json')</param>
        /// <param name="jsonPath">json文件所在目录</param>
        public static void SetWatcher(string jsonFileName, string jsonPath)
        {
            _jsonFileName = jsonFileName;
            if (JsonConfigurationProvider.Set(jsonFileName, File.ReadAllText(string.Concat(jsonPath, jsonFileName))))
            {
                Watcher(jsonPath, jsonFileName);
            }
            else
            {
                //失败日志
            }
        }
        private static void Watcher(string path, string fileName)
        {
            montior.Start(new JsonConfigurationPath(path, fileName));
        }
        /// <summary>
        /// 根据配置文件的键获取值(字符串)
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string Get(string key)
        {
            return JsonConfigurationProvider.GetSection(JsonFileName, key);
        }
        /// <summary>
        /// 根据配置文件的键获取值(数组)
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static IEnumerable<string> Gets(string key)
        {
            return JsonConfigurationProvider.GetSections(JsonFileName, key);
        }
        /// <summary>
        /// 根据配置文件的键获取值(整型)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetInt(string key)
        {
            return int.Parse(Get(key));
        }
        /// <summary>
        /// 根据配置文件的键获取值(浮点型)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static float GetFloat(string key)
        {
            return float.Parse(Get(key));
        }
        /// <summary>
        /// 根据配置文件的键获取值(浮点数组)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IEnumerable<float> GetFloats(string key)
        {
            return Gets(key).Select(x => float.Parse(x));
        }
        /// <summary>
        /// 根据配置文件的键获取值(整型数组)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IEnumerable<int> GetInts(string key)
        {
            return Gets(key).Select(x => int.Parse(x));
        }
        /// <summary>
        /// 根据配置文件的键获取值(对象)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetSection<T>(string section) where T : class
        {
            return JsonConfigurationProvider.GetSection<T>(JsonFileName, section);
        }
        /// <summary>
        /// 根据配置文件的键获取值(对象集合)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetSections<T>(string section) where T : class
        {
            return JsonConfigurationProvider.GetSections<T>(JsonFileName, section);
        }
    }
}
