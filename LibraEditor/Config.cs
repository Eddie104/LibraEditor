using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace LibraEditor
{
    class Config
    {

        private static Config instance;

        private static string cfgPath = "cfg.json";

        /// <summary>
        /// 白鹭引擎项目路径
        /// </summary>
        public List<string> EgretProjects { get; set; }

        /// <summary>
        /// plist文件路径
        /// </summary>
        public List<string> Plists { get; set; }

        /// <summary>
        /// 地图项目路径
        /// </summary>
        public List<string> MapPropjects { get; set; }

        public Config()
        {
            EgretProjects = new List<string>();
            Plists = new List<string>();
            MapPropjects = new List<string>();
        }

        public string GetFirstEgretProject()
        {
            return EgretProjects.Count > 0 ? EgretProjects[0] : null;
        }

        public string GetFirstPlist()
        {
            return Plists.Count > 0 ? Plists[0] : null;
        }

        public static Config GetInstance()
        {
            if (instance == null)
            {
                //读取配置文件，进行反序列化
                if (File.Exists(cfgPath))
                {
                    using (StreamReader sr = new StreamReader(cfgPath))
                    {
                        string jsonTxt = sr.ReadToEnd();
                        instance = JsonConvert.DeserializeObject(jsonTxt, typeof(Config)) as Config;
                    }
                }
                else
                {
                    instance = new Config();
                }
            }
            return instance;
        }

        public static void Save()
        {
            string cfgJson = JsonConvert.SerializeObject(instance, Formatting.Indented);
            using (StreamWriter sr = new StreamWriter(cfgPath))
            {
                sr.Write(cfgJson);
            }
        }
    }
}
