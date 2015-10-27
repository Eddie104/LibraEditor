using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LibraEditor
{
    class Config
    {
        private static List<string> recentEgretResourcePath = new List<string>();

        private const string XML_NAME = "config.xml";

        public static void init()
        {
            if (File.Exists(XML_NAME))
            {
                XmlDocument cfgXml = new XmlDocument();
                cfgXml.Load(XML_NAME);
                XmlNodeList recentEgretResourcePathNodeList = cfgXml.GetElementsByTagName("recentEgretResourcePath");
                XmlNode recentEgretResourcePathNode = recentEgretResourcePathNodeList[0];
                foreach (XmlNode node in recentEgretResourcePathNode.ChildNodes)
                {
                    recentEgretResourcePath.Add(node.InnerText);
                }
            }
        }

        public static void Save()
        {
            XmlDocument cfgXml = new XmlDocument();

            XmlElement element = cfgXml.CreateElement("recentEgretResourcePath");
            XmlElement pathElement = null;
            foreach (string path in recentEgretResourcePath)
            {
                pathElement = cfgXml.CreateElement("path");
                pathElement.InnerText = path;
                element.AppendChild(pathElement);
            }
            cfgXml.AppendChild(element);

            cfgXml.Save(XML_NAME);
        }

        public static string GetFirstRecentEgretResourcePath()
        {
            return recentEgretResourcePath.Count > 0 ? recentEgretResourcePath[0] : null;
        }

        public static void AddRecentEgretResourcePath(string projectPath)
        {
            if (!recentEgretResourcePath.Contains(projectPath))
            {
                recentEgretResourcePath.Add(projectPath);
            }
        }

    }
}
