using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using ZXing;
using ZXing.QrCode;

namespace Git.Print.Libraries
{
    public partial class DocumentPrint:IPrint
    {
        /// <summary>
        /// 打印模板文件路径
        /// </summary>
        private string FilePath { get; set; }

        /// <summary>
        /// 打印机名称
        /// </summary>
        private string PrintName { get; set; }

        /// <summary>
        /// 纸张是否自动高度
        /// </summary>
        private bool IsAutoHeight { get; set; }

        /// <summary>
        /// 打印数据源
        /// </summary>
        private Dictionary<string, object> DataSource { get; set; }

        /// <summary>
        /// 打印Document
        /// </summary>
        private PrintDocument printDocument;

        /// <summary>
        /// 打印对话框
        /// </summary>
        private PrintDialog printDialog;

        /// <summary>
        /// XML解析文档
        /// </summary>
        private XDocument root;

        public DocumentPrint()
        {
        }

        public DocumentPrint(string filePath, string printName, bool isAutoHeigth, Dictionary<string, object> dataSource)
        {
            this.FilePath = FilePath;
            this.PrintName = printName;
            this.IsAutoHeight = isAutoHeigth;
            this.DataSource = dataSource;
        }

        public DocumentPrint(string filePath, string printName, bool isAutoHeigth, string dataSource)
        {
            this.FilePath = FilePath;
            this.PrintName = printName;
            this.IsAutoHeight = isAutoHeigth;

            JsonDataSource convert = new JsonDataSource();
            this.DataSource = convert.To(dataSource);
        }

        /// <summary>
        /// 初始化打印
        /// </summary>
        /// <returns></returns>
        public IPrint Init()
        {
            this.printDialog = new PrintDialog();
            this.printDocument = new PrintDocument();
            this.printDialog.Document = this.printDocument;
            this.printDocument.PrintPage += PrintDocument_PrintPage;

            this.Begin();

            return this;
        }

        /// <summary>
        /// 开始初始化内部变量
        /// </summary>
        private void Begin()
        {
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

            //计算文档高度
            if (this.IsAutoHeight)
            {
                float PageHeith = 0;
                foreach (XElement item in root.Element("Page").Elements())
                {
                    if (item.Name == "Line")
                    {
                        XAttribute attribute = item.Attribute("Height");
                        if (attribute != null && string.IsNullOrWhiteSpace(attribute.Value))
                        {
                            float LineHeigth = string.IsNullOrWhiteSpace(item.Attribute("Height").Value) ? 0 : Convert.ToSingle(item.Attribute("Height").Value);
                            PageHeith += LineHeigth;
                        }
                    }
                    else if (item.Name == "Loop")
                    {
                        string Values = item.Attribute("Values").Value;
                        List<Dictionary<string, object>> listValues = null;
                        listValues = this.DataSource[Values] as List<Dictionary<string, object>>;
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
            this.printDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize(string.Format("{0}*{1}", strWidth, strHeigth), (int)Math.Ceiling(Convert.ToSingle(strWidth)), (int)Math.Ceiling(Convert.ToSingle(strHeigth)));
            this.printDocument.PrinterSettings.PrinterName = string.IsNullOrWhiteSpace(this.PrintName) ? DefaultPrinter : this.PrintName;
        }

        /// <summary>
        /// 打印触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Brush bru = Brushes.Black;
            Graphics g = e.Graphics;

            float totalHeight = 0;
            int rowIndex = 0;

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
                    g.DrawString(content.Replace("{{" + key + "}}", row[key].ToString()), new Font(FontName, FontSize, FontStyle.Regular), bru, new PointF(Left, Top));
                }
                else
                {
                    g.DrawString(content, new Font(FontName, FontSize, FontStyle.Regular), bru, new PointF(Left, Top));
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
                    Image image = Image.FromFile(row[key].ToString());

                    if (Width == 0 || Heigth == 0)
                    {
                        g.DrawImage(image, new PointF(Left, Top));
                    }
                    else
                    {
                        g.DrawImage(image, Left, Top, Width, Heigth);
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

                QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                QrCode qrCode = new QrCode();
                qrEncoder.TryEncode(content, out qrCode);
                using (MemoryStream ms = new MemoryStream())
                {
                    GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(3, QuietZoneModules.Two));
                    renderer.WriteToStream(qrCode.Matrix, ImageFormat.Jpeg, ms);
                    Image image = Image.FromStream(ms);
                    g.DrawImage(image, new PointF(Left, Top));
                }
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

                QrCodeEncodingOptions options = new QrCodeEncodingOptions
                {
                    DisableECI = true,
                    CharacterSet = "UTF-8",
                    Width = (int)Math.Ceiling(Width),
                    Height = (int)Math.Ceiling(Height),
                };
                BarcodeWriter writer = new BarcodeWriter();
                writer.Format = BarcodeFormat.CODE_128;
                writer.Options = options;
                Bitmap bitmap = writer.Write(content);
                g.DrawImage(bitmap, new PointF(Left, Top));
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
                            ActionText(child, this.DataSource);
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
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 打印调用执行
        /// </summary>
        /// <returns></returns>
        public IPrint Print()
        {
            this.printDocument.Print(); //触发打印

            return this;
        }

        /// <summary>
        /// 打印文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public IPrint PrintFile(string fileName)
        {
            this.printDocument.Print(); //触发打印
            return this;
        }
    }
}
