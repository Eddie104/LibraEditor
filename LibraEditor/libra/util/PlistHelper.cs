﻿using libra.log4CSharp;
using libra.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LibraEditor.libra.util
{

    enum EnumValueType
    {
        DICT,
        ARRAY,
        NUMBER,
        STRING,
        DTAE,
        BOOLEAN,
        DATA,
        INTEGER,
        REAL,
        KEY
    }

    class DataType
    {
        public int ID { get; set; }//元素ID
        public string DataName { get; set; }//元素数据名称
        public EnumValueType ValueType { get; set; }//元素值类型
        public string Value { get; set; }//元素值
        public int parentID { get; set; }//元素父节点ID
        public List<int> childrenID { get; set; }//元素子节点

        public override string ToString()
        {
            return string.Format("id = {0}, name = {1}, val = {2}, parentID = {3}", ID, DataName, Value, parentID);
        }
    }

    class PlistHelper
    {
        public List<DataType> DataList { get; set; }
        private DataType rootNode;
        private int count;

        public PlistHelper()
        {
            count = 1;
            rootNode = new DataType();
            rootNode.ID = count;
            rootNode.DataName = null;
            rootNode.parentID = 0;
            rootNode.ValueType = EnumValueType.DICT;
            rootNode.childrenID = new List<int>();
            this.DataList = new List<DataType>();
            DataList.Add(rootNode);
        }

        private XDocument LoadFromFile(string path)
        {
            return XDocument.Load(path);
        }

        public void XMLparser(string path)
        {
            XDocument doc = LoadFromFile(path);
            XElement FirstElement = doc.Root.Element("dict");
            DataList[0].childrenID = XMLOnce(FirstElement, 1);
            foreach (var item in DataList)
            {
                if (item.Value == "FALSE" || item.Value == "TRUE")
                {
                    item.Value = item.Value.ToLower();
                }
                if (item.DataName != null && Char.IsNumber(item.DataName[0]))
                {
                    item.DataName.Insert(0, "_");
                }
            }
        }

        private List<int> XMLOnce(XElement nowElement, int parentid)
        {
            List<DataType> DataTemp = new List<DataType>();
            List<int> IDList = new List<int>();
            List<int> childrenIDList = new List<int>();
            var keys = from k in nowElement.Elements("key")
                       select k;
            var values = from v in nowElement.Elements()
                         where v.Name != "key"
                         select v;
            var valList = values.ToList();
            for (int i = 0; i < valList.Count; i++)
            {
                int id = ++count;
                EnumValueType valuetype = (EnumValueType)Enum.Parse(typeof(EnumValueType), valList[i].Name.LocalName.ToString().ToUpper(), true);
                string value = null;
                if (valuetype == EnumValueType.ARRAY)
                {
                    XElement newElement = nowElement.Elements().Except(nowElement.Elements("key")).ElementAt(i);
                    int num = newElement.Elements().Count();
                    for (int j = 0; j < num; j++)
                    {
                        newElement.AddFirst(new XElement("key", "item"));
                    }
                    childrenIDList = XMLOnce(newElement, id);
                }
                else if (valuetype == EnumValueType.DICT)
                {
                    XElement newElement = nowElement.Elements().Except(nowElement.Elements("key")).ElementAt(i);
                    childrenIDList = XMLOnce(newElement, id);
                }
                else
                {
                    value = valList[i].Value.ToString();
                }

                try
                {
                    DataTemp.Add(new DataType()
                    {
                        DataName = keys.ToList()[i].Value.ToString(),
                        ValueType = valuetype,
                        ID = id,
                        Value = value,
                        parentID = parentid,
                        childrenID = childrenIDList
                    });
                }
                catch (System.Exception ex)
                {
                    DataTemp.Add(new DataType()
                    {
                        DataName = "itemContent",
                        ValueType = valuetype,
                        ID = id,
                        Value = value,
                        parentID = parentid,
                        childrenID = childrenIDList
                    });
                }

            }
            foreach (var item in DataTemp)
            {
                IDList.Add(item.ID);
            }
            DataList.AddRange(DataTemp);
            return IDList;
        }

        public PlistData CreatePlistData()
        {
            PlistData plistData = new PlistData();
            foreach (DataType item in DataList)
            {
                if (item.DataName == "frames")
                {
                    List<DataType> children = GetDataType(item.childrenID);
                    foreach (DataType child in children)
                    {
                        FrameData frameData = new FrameData();
                        frameData.PngName = child.DataName;
                        Type type = frameData.GetType();

                        List<DataType> frameDataList = GetDataType(child.childrenID);
                        foreach (DataType data in frameDataList)
                        {
                            System.Reflection.PropertyInfo propertyInfo = type.GetProperty(StringHelper.ToTitleCase(data.DataName));
                            if (propertyInfo != null)
                            {
                                propertyInfo.SetValue(frameData, data.Value);
                            }
                            else
                            {
                                Logger.Wran(type.ToString() + "不包含属性:" + data.DataName);
                            }
                        }
                        plistData.Frames.Add(frameData);
                    }
                }
                else if (item.DataName == "metadata")
                {
                    Metadata metadata = new Metadata();
                    List<DataType> children = GetDataType(item.childrenID);
                    foreach (DataType child in children)
                    {
                        Type type = metadata.GetType();
                        System.Reflection.PropertyInfo propertyInfo = type.GetProperty(StringHelper.ToTitleCase(child.DataName));
                        if (propertyInfo != null)
                        {
                            propertyInfo.SetValue(metadata, child.Value);
                        }
                        else
                        {
                            Logger.Wran(type.ToString() + "不包含属性:" + child.DataName);
                        }
                    }
                    plistData.Metadata = metadata;
                }
            }
            return plistData;
        }

        private List<DataType> GetDataType(List<int> idList)
        {
            List<DataType> result = new List<DataType>();
            foreach (int id in idList)
            {
                result.Add(GetDataType(id));
            }
            return result;
        }

        private DataType GetDataType(int ID)
        {
            foreach (DataType item in DataList)
            {
                if (item.ID == ID)
                {
                    return item;
                }
            }
            return null;
        }
    }

    public class PlistData
    {
        public List<FrameData> Frames { get; set; }

        public Metadata Metadata { get; set; }

        public PlistData()
        {
            this.Frames = new List<FrameData>();
        }
    }

    public class FrameData
    {
        public string PngName { get; set; }

        public string Width { get; set; }

        public string Height { get; set; }

        public string OriginalWidth { get; set; }

        public string OriginalHeight { get; set; }

        public string X { get; set; }

        public string Y { get; set; }

        public string OffsetX { get; set; }

        public string OffsetY { get; set; }
    }

    public class Metadata
    {
        public string Format { get; set; }

        public string Size { get; set; }
    }

    #region xml
    //class ConvertXML
    //{
    //    List<DataType> DataList { get; set; }

    //    public ConvertXML(List<DataType> datalist)
    //    {
    //        DataList = datalist;
    //    }

    //    public XDocument xdoc = new XDocument(new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement("Model")));
    //    public XDocument creatXML()
    //    {
    //        xdoc.Element("Model").SetAttributeValue("id", "1");
    //        foreach (var item in DataList)
    //        {
    //            if (DataList[0].childrenID.Contains(item.ID))
    //            {
    //                //xdoc.Element("Model").Add(new XElement(item.DataName, item.Value));
    //                XElement newElement = xdoc.Descendants().Where(e => e.Attribute("id").Value == "1").First();
    //                newElement.Add(new XElement(item.DataName, item.Value, new XAttribute("id", item.ID)));
    //                if (item.ValueType == EnumValueType.ARRAY || item.ValueType == EnumValueType.DICT)
    //                {
    //                    //XElement newElement = xdoc.Element("Model").Element(item.DataName);
    //                    creatOnce(newElement, item);
    //                }

    //            }
    //        }
    //        return xdoc;
    //    }

    //    public void creatOnce(XElement doc, DataType parent)
    //    {
    //        foreach (var item in DataList)
    //        {
    //            if (parent.childrenID.Contains(item.ID))
    //            {
    //                string parentID = parent.ID.ToString();
    //                XElement newElement = doc.Descendants().Where(e => e.Attribute("id").Value.Equals(parentID)).First();
    //                newElement.Add(new XElement(item.DataName, item.Value, new XAttribute("id", item.ID)));
    //                if (item.ValueType == EnumValueType.ARRAY || item.ValueType == EnumValueType.DICT)
    //                {
    //                    //nowElement = nowElement.Element(item.DataName);
    //                    creatOnce(newElement, item);
    //                }
    //            }
    //        }
    //    }

    //}

    //class PlistSerializer : XmlSerializer
    //{
    //    public object Deserialize(string path, string assemblyName)
    //    {
    //        MemoryStream stream = new MemoryStream();
    //        PlistHelper rp = new PlistHelper();
    //        rp.XMLparser(path);
    //        ConvertXML cx = new ConvertXML(rp.DataList);
    //        //Console.WriteLine(cx.creatXML());
    //        XDocument doc = cx.creatXML();
    //        doc.Save(stream);
    //        stream.Seek(0, SeekOrigin.Begin);
    //        Type modelType = Type.GetType(assemblyName);
    //        XmlSerializer serializer = new XmlSerializer(modelType);

    //        try
    //        {
    //            object sender = serializer.Deserialize(stream);
    //            stream.Close();
    //            return sender;
    //        }
    //        catch (Exception e)
    //        {
    //            Console.WriteLine(e.Message);
    //            stream.Close();
    //            return null;
    //        }

    //    }


    //}
    #endregion
}
