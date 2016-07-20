using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Git.Print.Libraries
{
    public partial class ZPLPrint:IPrint
    {
        /// <summary>
        /// 打印模板文件路径
        /// </summary>
        private string FilePath { get; set; }

        /// <summary>
        /// 打印机名称(COM 口)
        /// </summary>
        private string ComName { get; set; }

        /// <summary>
        /// 纸张是否自动高度
        /// </summary>
        private bool IsAutoHeight { get; set; }

        /// <summary>
        /// 打印数据源
        /// </summary>
        private Dictionary<string, object> DataSource { get; set; }

        /// <summary>
        /// XML解析文档
        /// </summary>
        private XDocument root;

        public static object lockObject = new object();

        public ZPLPrint(string filePath,string comName,bool isAutoHeight,Dictionary<string,object> dataSource)
        {
            this.FilePath = filePath;
            this.ComName = comName;
            this.IsAutoHeight = isAutoHeight;
            this.DataSource = dataSource;
        }

        public ZPLPrint(string filePath, string comName, bool isAutoHeight, string dataSource)
        {
            this.FilePath = filePath;
            this.ComName = comName;
            this.IsAutoHeight = isAutoHeight;
            JsonDataSource convert = new JsonDataSource();
            this.DataSource = convert.To(dataSource);
        }

        public IPrint Init()
        {
            return this;
        }

        public IPrint Print()
        {
            lock (lockObject)
            {
                try
                {
                    SerialPort ports = new SerialPort();
                    ports.BaudRate = 9600;
                    ports.PortName = this.ComName;
                    if (ports.IsOpen)
                    {
                        ports.Close();
                    }
                    ports.Open();
                    ports.WriteLine(GetCommand());
                    ports.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return this;
        }

        public IPrint PrintFile(string fileName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获得打印指令
        /// </summary>
        /// <returns></returns>
        private string GetCommand()
        {
            StringBuilder builder = new StringBuilder();

            if (!File.Exists(this.FilePath))
            {
                throw new Exception("打印模板文件不存在");
            }

            this.root = XDocument.Load(this.FilePath);

            string strWidth = root.Element("Page").Attribute("Width").Value;
            string strHeigth = root.Element("Page").Attribute("Heigth").Value;
            strWidth = string.IsNullOrWhiteSpace(strWidth) ? "0" : strWidth;
            strHeigth = string.IsNullOrWhiteSpace(strHeigth) ? "0" : strHeigth;
            string DefaultPrinter = root.Element("Page").Attribute("DefaultPrinter").Value;

            this.ComName = string.IsNullOrEmpty(this.ComName) ? DefaultPrinter : this.ComName;

            if (this.IsAutoHeight)
            {
                float PageHeith = 0;
                foreach (XElement item in root.Element("Page").Elements())
                {
                    if (item.Name == "Line")
                    {
                        float LineHeigth = string.IsNullOrWhiteSpace(item.Attribute("Height").Value) ? 0 : Convert.ToSingle(item.Attribute("Height").Value);
                        PageHeith += LineHeigth;
                    }
                    else if (item.Name == "Loop")
                    {
                        string Values = item.Attribute("Values").Value;
                        List<Dictionary<string, object>> listValues = this.DataSource[Values] as List<Dictionary<string, object>>;
                        if (listValues != null)
                        {
                            XElement lineItem = item.Element("Line");
                            float LineHeigth = string.IsNullOrWhiteSpace(lineItem.Attribute("Height").Value) ? 0 : Convert.ToSingle(lineItem.Attribute("Height").Value);
                            PageHeith += LineHeigth * listValues.Count();
                        }
                    }
                }
                strHeigth = (PageHeith + 10).ToString();
            }

            float totalHeight = 0;
            int rowIndex = 0;

            ZplCommand zh = new ZplCommand();
            builder.AppendLine(zh.ZPL_Start());
            builder.AppendLine(zh.ZPL_Cutter());
            builder.AppendLine(zh.ZPL_PageSize(Convert.ToInt32(strWidth), Convert.ToInt32(strHeigth)));

            Action<XElement, Dictionary<string, object>> ActionText = (el, row) =>
            {
                float Left = string.IsNullOrWhiteSpace(el.Attribute("Left").Value) ? 0 : Convert.ToSingle(el.Attribute("Left").Value);
                float Top = string.IsNullOrWhiteSpace(el.Attribute("Top").Value) ? 0 : Convert.ToSingle(el.Attribute("Top").Value);
                float FontSize = string.IsNullOrWhiteSpace(el.Attribute("FontSize").Value) ? 0 : Convert.ToSingle(el.Attribute("FontSize").Value);
                string FontName = el.Attribute("FontName") != null ? el.Attribute("FontName").Value : "";
                FontName = string.IsNullOrWhiteSpace(FontName) ? "宋体" : FontName;

                Top = totalHeight + Top;
                string content = el.Value;
                if (content.Contains("{{") && content.Contains("}}"))
                {
                    int beginIndex = content.IndexOf("{{");
                    int endIndex = content.LastIndexOf("}}");
                    string key = content.Substring(beginIndex + 2, endIndex - beginIndex - 2);
                    builder.AppendLine(zh.ZPL_CHText(content.Replace("{{" + key + "}}", row[key].ToString()), "宋体", (int)Left, (int)Top, 0, (int)FontSize, (int)FontSize, 0, 0));
                }
                else
                {
                    builder.AppendLine(zh.ZPL_CHText(content, "宋体", (int)Left, (int)Top, 0, (int)FontSize, (int)FontSize, 0, 0));
                }
            };

            Action<XElement, Dictionary<string, object>> ActionImage = (el, row) =>
            {
                float Left = string.IsNullOrWhiteSpace(el.Attribute("Left").Value) ? 0 : Convert.ToSingle(el.Attribute("Left").Value);
                float Top = string.IsNullOrWhiteSpace(el.Attribute("Top").Value) ? 0 : Convert.ToSingle(el.Attribute("Top").Value);
                int Width = 0;
                int Heigth = 0;

                if (el.Attribute("Width") != null)
                {
                    Width = string.IsNullOrWhiteSpace(el.Attribute("Width").Value) ? 0 : Convert.ToInt32(el.Attribute("Width").Value);
                }
                if (el.Attribute("Heigth") != null)
                {
                    Heigth = string.IsNullOrWhiteSpace(el.Attribute("Heigth").Value) ? 0 : Convert.ToInt32(el.Attribute("Heigth").Value);
                }

                Top = totalHeight + Top;
                string content = el.Value;
                if (content.Contains("{{") && content.Contains("}}"))
                {
                    int beginIndex = content.IndexOf("{{");
                    int endIndex = content.LastIndexOf("}}");
                    string key = content.Substring(beginIndex + 2, endIndex - beginIndex - 2);

                    if (Width == 0 || Heigth == 0)
                    {
                        builder.AppendLine(zh.ZPL_Image((int)Left, (int)Top, 0, 0, row[key].ToString()));
                    }
                    else
                    {
                        builder.AppendLine(zh.ZPL_Image((int)Left, (int)Top, Width, Heigth, row[key].ToString()));
                    }
                }
            };

            Action<XElement, Dictionary<string, object>> ActionQRCode = (el, row) =>
            {
                string content = string.Empty;
                float Left = string.IsNullOrWhiteSpace(el.Attribute("Left").Value) ? 0 : Convert.ToSingle(el.Attribute("Left").Value);
                float Top = string.IsNullOrWhiteSpace(el.Attribute("Top").Value) ? 0 : Convert.ToSingle(el.Attribute("Top").Value);

                Top = totalHeight + Top;
                content = el.Value;
                if (content.Contains("{{") && content.Contains("}}"))
                {
                    int beginIndex = content.IndexOf("{{");
                    int endIndex = content.LastIndexOf("}}");
                    string key = content.Substring(beginIndex + 2, endIndex - beginIndex - 2);
                    content = content.Replace("{{" + key + "}}", row[key].ToString());
                }
                builder.AppendLine(zh.ZPL_QRCode((int)Left, (int)Top,content));
            };

            Action<XElement, Dictionary<string, object>> ActionBarCode = (el, row) =>
            {
                string content = string.Empty;

                float Left = string.IsNullOrWhiteSpace(el.Attribute("Left").Value) ? 0 : Convert.ToSingle(el.Attribute("Left").Value);
                float Top = string.IsNullOrWhiteSpace(el.Attribute("Top").Value) ? 0 : Convert.ToSingle(el.Attribute("Top").Value);

                float Width = string.IsNullOrWhiteSpace(el.Attribute("Width").Value) ? 0 : Convert.ToSingle(el.Attribute("Width").Value);
                float Height = string.IsNullOrWhiteSpace(el.Attribute("Height").Value) ? 0 : Convert.ToSingle(el.Attribute("Height").Value);

                Top = totalHeight + Top;
                content = el.Value;
                if (content.Contains("{{") && content.Contains("}}"))
                {
                    int beginIndex = content.IndexOf("{{");
                    int endIndex = content.LastIndexOf("}}");
                    string key = content.Substring(beginIndex + 2, endIndex - beginIndex - 2);
                    content = content.Replace("{{" + key + "}}", row[key].ToString());
                }
                builder.AppendLine(zh.ZPL_Barcode128((int)Left, (int)Top, 2, 2, 100, content));
            };

            foreach (XElement item in root.Element("Page").Elements())
            {
                if (item.Name == "Line")
                {
                    float LineHeigth = string.IsNullOrWhiteSpace(item.Attribute("Height").Value) ? 0 : Convert.ToSingle(item.Attribute("Height").Value);

                    foreach (XElement child in item.Elements())
                    {
                        if (child.Name == "Text")
                        {
                            ActionText(child,this.DataSource);
                        }
                        else if (child.Name == "Image")
                        {
                            ActionImage(child, this.DataSource);
                        }
                        else if (child.Name == "QRCode")
                        {
                            ActionQRCode(child, this.DataSource);
                        }
                        else if (child.Name == "BarCode")
                        {
                            ActionBarCode(child, this.DataSource);
                        }
                    }
                    totalHeight += LineHeigth;
                    rowIndex++;
                }
                else if (item.Name == "Loop")
                {
                    string Values = item.Attribute("Values").Value;
                    List<Dictionary<string, object>> listValues = this.DataSource[Values] as List<Dictionary<string, object>>;
                    if (listValues != null)
                    {
                        XElement lineItem = item.Element("Line");
                        float LineHeigth = string.IsNullOrWhiteSpace(lineItem.Attribute("Height").Value) ? 0 : Convert.ToSingle(lineItem.Attribute("Height").Value);

                        for (int i = 0; i < listValues.Count(); i++)
                        {
                            Dictionary<string, object> dicRow = listValues[i];
                            foreach (XElement child in lineItem.Elements())
                            {
                                if (child.Name == "Text")
                                {
                                    ActionText(child, dicRow);
                                }
                                else if (child.Name == "Image")
                                {
                                    ActionImage(child, dicRow);
                                }
                                else if (child.Name == "QRCode")
                                {
                                    ActionQRCode(child, dicRow);
                                }
                                else if (child.Name == "BarCode")
                                {
                                    ActionBarCode(child, dicRow);
                                }
                            }
                            totalHeight += LineHeigth;
                            rowIndex++;
                        }
                    }
                }
            }
            builder.AppendLine(zh.ZPL_End());
            return builder.ToString();

        }
    }
}
