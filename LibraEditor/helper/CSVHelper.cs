using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraEditor.helper
{
    class CSVHelper
    {

        private List<string> keys;

        private List<string> marks;

        private List<string> comments;

        private List<string> dataTypes;

        private List<List<string>> contents;

        private string csvPath;
        
        public CSVHelper(string csvPath)
        {
            this.csvPath = csvPath;
            using (StreamReader sr = new StreamReader(csvPath, Encoding.GetEncoding("GB2312")))
            {
                string csv = sr.ReadToEnd().Replace("\r", "");
                string[] rows = csv.Split(new char[] { '\n' });
                //第一行是key，第二行是标识，0为前后端都用的数据，1为后端用的数据，2为前端用的数据
                //第三行是中文注释，第四行是数据类型
                var splitChar = new char[] { ',' };
                keys = new List<string>(rows[0].Split(splitChar));
                marks = new List<string>(rows[1].Split(splitChar));
                comments = new List<string>(rows[2].Split(splitChar));
                dataTypes = new List<string>(rows[3].Split(splitChar));

                contents = new List<List<string>>();
                for (int row = 5; row < rows.Length; row++)
                {
                    contents.Add(new List<string>(rows[row].Split(splitChar)));
                }
                //清除空行
                List<List<string>> tmpContents = new List<List<string>>(contents.ToArray());
                bool empty = true;
                List<int> delLine = new List<int>();
                for (int i = tmpContents.Count - 1; i > -1; i--)
                {
                    empty = true;
                    foreach (string str in tmpContents[i])
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            empty = false;
                            break;
                        }
                    }
                    if (empty)
                    {
                        delLine.Add(i);
                    }
                }
                foreach (int line in delLine)
                {
                    contents.RemoveAt(line);
                }
            }
        }

        public void Save(string csvPath = null)
        {
            csvPath = csvPath == null ? this.csvPath : csvPath;
            //using (StreamWriter sw = new StreamWriter(csvPath, false, Encoding.GetEncoding("GB2312")))
            using (StreamWriter sw = new StreamWriter(csvPath, false, Encoding.UTF8))
            {
                string str = string.Join(",", keys);
                str += "\r\n" + string.Join(",", marks);
                str += "\r\n" + string.Join(",", comments);
                str += "\r\n" + string.Join(",", dataTypes);
                foreach (List<string> strList in contents)
                {
                    str += "\r\n" + string.Join(",", strList);
                }

                sw.Write(str);
            }
        }
    }
}
