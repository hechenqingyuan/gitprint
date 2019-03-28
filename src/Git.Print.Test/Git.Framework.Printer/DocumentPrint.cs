/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2018/4/24 15:28:53
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2018/4/24 15:28:53       情缘
 * 吉特仓储管理系统 开源地址 https://github.com/hechenqingyuan/gitwms
 * 项目地址:http://yun.gitwms.com/
*********************************************************************************/

using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.Printer.Pager;
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
using ZXing;
using ZXing.QrCode;

namespace Git.Framework.Printer
{
    public partial class DocumentPrint : IPrint
    {
        /// <summary>
        /// 打印Document
        /// </summary>
        private PrintDocument PrintDoc { get; set; }

        /// <summary>
        /// 打印页面参数
        /// </summary>
        private PageEntity Page { get; set; }

        /// <summary>
        /// 打印数据源
        /// </summary>
        private Dictionary<string, object> DataSource { get; set; }

        private Brush bru { get; set; }

        private Graphics g { get; set; }

        /// <summary>
        /// 当前高度
        /// </summary>
        private float CurrentHeight { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="printName"></param>
        /// <param name="dataSource"></param>
        public DocumentPrint(string filePath, string printName,Dictionary<string, object> dataSource)
        {
            PageXmlReader Reader = new PageXmlReader(filePath);
            this.Page=Reader.Read();
            if (printName.IsNotEmpty())
            {
                this.Page.DefaultPrinter = printName;
            }
            this.DataSource = dataSource;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public IPrint Init()
        {
            this.PrintDoc= new PrintDocument();
            this.PrintDoc.PrintPage += PrintDocument_PrintPage;

            float PageHeight = 0;
            if (this.Page.AutoHeight)
            {
                foreach (RowEntity Row in this.Page.Rows)
                {
                    if (Row.RowType == (int)ERowType.Line)
                    {
                        LineEntity RowItem = Row as LineEntity;
                        PageHeight += RowItem.Height;
                    }
                    else if (Row.RowType == (int)ERowType.Loop)
                    {
                        LoopEntity RowItem = Row as LoopEntity;
                        string Key = RowItem.KeyName;
                        object dataList = this.DataSource.Value<string,object>(Key);
                        if (dataList!=null && dataList is List<Dictionary<string, object>>)
                        {
                            List<Dictionary<string, object>> list = dataList as List<Dictionary<string, object>>;
                            if (!list.IsNullOrEmpty())
                            {
                                float ItemHeight = RowItem.ListLine.Sum(a => a.Height);
                                PageHeight += ItemHeight * list.Count();
                            }
                        }
                    }
                    else if (Row.RowType == (int)ERowType.Table)
                    {
                        TableEntity RowItem = Row as TableEntity;
                        if (RowItem.Head != null)
                        {
                            PageHeight += RowItem.Head.Height;
                        }
                        string Key = RowItem.KeyName;
                        object dataList = this.DataSource.Value<string, object>(Key);
                        if (dataList != null && dataList is List<Dictionary<string, object>>)
                        {
                            List<Dictionary<string, object>> list = dataList as List<Dictionary<string, object>>;
                            if (!list.IsNullOrEmpty())
                            {
                                float ItemHeight = RowItem.ListTR.Sum(a => a.Height);
                                PageHeight += ItemHeight * list.Count();
                            }
                        }
                    }

                    this.Page.Heigth = PageHeight;
                }
            }
            //this.PrintDoc.DefaultPageSettings.PaperSize.Kind = PaperKind.Custom;
            //this.PrintDoc.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize(string.Format("{0}*{1}", this.Page.Width, this.Page.Heigth), (int)Math.Ceiling(this.Page.Width), (int)Math.Ceiling(this.Page.Heigth));

            this.PrintDoc.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom", (int)Math.Ceiling(this.Page.Width), (int)Math.Ceiling(this.Page.Heigth));

            this.PrintDoc.PrinterSettings.PrinterName = this.Page.DefaultPrinter;

            return this;
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <returns></returns>
        public IPrint Print()
        {
            this.PrintDoc.Print();
            return this;
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            this.bru = Brushes.Black;
            this.g = e.Graphics;

            foreach (RowEntity row in this.Page.Rows.Where(item => item.RowType == (int)ERowType.Table))
            {
                TableEntity RowItem = row as TableEntity;
                float TabLeft = RowItem.Left;

                Action<List<TdEntity>> action = (List<TdEntity> listTD) => 
                {
                    float CurrentLeft = TabLeft;
                    foreach (TdEntity Td in listTD)
                    {
                        if (!Td.ListContent.IsNullOrEmpty())
                        {
                            foreach (ContentEntity item in Td.ListContent)
                            {
                                if (item is StrLineEntity)
                                {
                                }
                                else if (item is TextEntity)
                                {
                                    TextEntity Content = item as TextEntity;
                                    Content.Left = Content.Left + CurrentLeft;
                                }
                                else if (item is ImageEntity)
                                {
                                    ImageEntity Content = item as ImageEntity;
                                    Content.Left = Content.Left + CurrentLeft;
                                }
                                else if (item is QRCodeEntity)
                                {
                                    QRCodeEntity Content = item as QRCodeEntity;
                                    Content.Left = Content.Left + CurrentLeft;
                                }
                                else if (item is BarCodeEntity)
                                {
                                    BarCodeEntity Content = item as BarCodeEntity;
                                    Content.Left = Content.Left + CurrentLeft;
                                }
                            }
                        }
                        CurrentLeft = CurrentLeft + Td.Width;
                    }
                };
                if (RowItem.Head != null)
                {
                    action(RowItem.Head.ListTD);
                }
                if (!RowItem.ListTR.IsNullOrEmpty())
                {
                    foreach (TrEntity td in RowItem.ListTR)
                    {
                        action(td.ListTD);
                    }
                }
            }

            foreach (RowEntity row in this.Page.Rows)
            {
                if (row.RowType == (int)ERowType.Line)
                {
                    LineEntity RowItem = row as LineEntity;
                    this.WriteLine(RowItem,this.DataSource);
                }
                else if (row.RowType == (int)ERowType.Loop)
                {
                    LoopEntity RowItem = row as LoopEntity;
                    string KeyName = RowItem.KeyName;
                    object ds = this.DataSource.Value<string, object>(KeyName);
                    List<Dictionary<string, object>> listSource = ds as List<Dictionary<string, object>>;
                    if (!listSource.IsNullOrEmpty())
                    {
                        this.WriteLoop(RowItem, listSource);
                    }
                }
                else if (row.RowType == (int)ERowType.Table)
                {
                    TableEntity RowItem = row as TableEntity;
                    string KeyName = RowItem.KeyName;
                    object ds = this.DataSource.Value<string, object>(KeyName);
                    List<Dictionary<string, object>> listSource = ds as List<Dictionary<string, object>>;
                    if (!listSource.IsNullOrEmpty())
                    {
                        this.WriteTable(RowItem, listSource);
                    }
                }
            }
        }

        /// <summary>
        /// 写入表格
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="listSource"></param>
        private void WriteTable(TableEntity entity, List<Dictionary<string, object>> listSource)
        {
            int RowCount = 0; //行数
            int ColCount = 0;
            if (entity.Head != null)
            {
                RowCount += 1;
            }
            if (!listSource.IsNullOrEmpty())
            {
                RowCount += listSource.Count();
            }
            if (!entity.ListTR.IsNullOrEmpty())
            {
                ColCount = entity.ListTR[0].ListTD.Count();
            }
            this.CurrentHeight += entity.Top; //当前画布的高度，要保留这个高度用于写入内容

            float TabHeight = 0;
            //画表格的横线
            if (true)
            {
                float TabCurrentHeight = 0;
                TabCurrentHeight = this.CurrentHeight;

                float StartX = entity.Left;
                float EndX = entity.Left + entity.Width;
                float StartY = TabCurrentHeight;
                float EndY = TabCurrentHeight;
                g.DrawLine(new Pen(bru), StartX, StartY, EndX, EndY);

                if (entity.Head != null)
                {
                    TabCurrentHeight = TabCurrentHeight + entity.Head.Height;
                    StartY = TabCurrentHeight;
                    EndY = TabCurrentHeight;
                    g.DrawLine(new Pen(bru), StartX, StartY, EndX, EndY);
                }

                if (!listSource.IsNullOrEmpty())
                {
                    TrEntity trEntity = entity.ListTR[0];
                    foreach(Dictionary<string, object> item in listSource)
                    {
                        TabCurrentHeight = TabCurrentHeight + trEntity.Height;
                        StartY = TabCurrentHeight;
                        EndY = TabCurrentHeight;
                        g.DrawLine(new Pen(bru), StartX, StartY, EndX, EndY);
                    }
                }
                TabHeight = TabCurrentHeight - this.CurrentHeight;
            }

            if (true)
            {
                float StartX = entity.Left;
                float EndX = entity.Left;
                float StartY = this.CurrentHeight;
                float EndY = this.CurrentHeight + TabHeight;
                g.DrawLine(new Pen(bru), StartX, StartY, EndX, EndY);

                if (!listSource.IsNullOrEmpty())
                {
                    TrEntity trEntity = entity.ListTR[0];
                    foreach (TdEntity td in trEntity.ListTD)
                    {
                        StartX = StartX + td.Width;
                        EndX = EndX + td.Width;
                        g.DrawLine(new Pen(bru), StartX, StartY, EndX, EndY);
                    }
                }
            }

            if (true)
            {
                if (entity.Head != null && !entity.Head.ListTD.IsNullOrEmpty())
                {
                    Dictionary<string, object> dicSource = new Dictionary<string, object>();
                    entity.Head.ListTD.ForEach(item => { this.WriteTd(item, dicSource); });
                    this.CurrentHeight += entity.Head.Height;
                }
            }

            if (true)
            {
                if (!listSource.IsNullOrEmpty() && !entity.ListTR.IsNullOrEmpty())
                {
                    TrEntity trEntity = entity.ListTR[0];
                    float TrHeight = trEntity.Height;
                    foreach (Dictionary<string, object> dicSource in listSource)
                    {
                        trEntity.ListTD.ForEach(item => { this.WriteTd(item, dicSource); });
                        this.CurrentHeight += TrHeight;
                    }
                }
            }
        }

        /// <summary>
        /// 写入表格信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dicSource"></param>
        private void WriteTd(TdEntity entity, Dictionary<string, object> dicSource)
        {
            if (!entity.ListContent.IsNullOrEmpty())
            {
                foreach (ContentEntity item in entity.ListContent)
                {
                    if (item is StrLineEntity)
                    {
                        StrLineEntity Content = item as StrLineEntity;
                        this.WriteLine(Content);
                    }
                    else if (item is TextEntity)
                    {
                        TextEntity Content = item as TextEntity;
                        this.WriteText(Content, dicSource);
                    }
                    else if (item is ImageEntity)
                    {
                        ImageEntity Content = item as ImageEntity;
                        this.WriteImage(Content, dicSource);
                    }
                    else if (item is QRCodeEntity)
                    {
                        QRCodeEntity Content = item as QRCodeEntity;
                        this.WriteQRCode(Content, dicSource);
                    }
                    else if (item is BarCodeEntity)
                    {
                        BarCodeEntity Content = item as BarCodeEntity;
                        this.WriteBarCode(Content, dicSource);
                    }
                }
            }
        }

        /// <summary>
        /// 循环写入
        /// </summary>
        /// <param name="entity"></param>
        private void WriteLoop(LoopEntity entity, List<Dictionary<string, object>> listSource)
        {
            if (!entity.ListLine.IsNullOrEmpty())
            {
                foreach (Dictionary<string, object> ds in listSource)
                {
                    foreach (LineEntity item in entity.ListLine)
                    {
                        this.WriteLine(item,ds);
                    }
                }
            }
        }

        /// <summary>
        /// 写入单行
        /// </summary>
        /// <param name="entity"></param>
        private void WriteLine(LineEntity entity, Dictionary<string, object> dicSource)
        {
            if (!entity.ListContent.IsNullOrEmpty())
            {
                foreach (ContentEntity item in entity.ListContent)
                {
                    if (item is StrLineEntity)
                    {
                        StrLineEntity Content = item as StrLineEntity;
                        this.WriteLine(Content);
                    }
                    else if (item is TextEntity)
                    {
                        TextEntity Content = item as TextEntity;
                        this.WriteText(Content,dicSource);
                    }
                    else if (item is ImageEntity)
                    {
                        ImageEntity Content = item as ImageEntity;
                        this.WriteImage(Content, dicSource);
                    }
                    else if (item is QRCodeEntity)
                    {
                        QRCodeEntity Content = item as QRCodeEntity;
                        this.WriteQRCode(Content, dicSource);
                    }
                    else if (item is BarCodeEntity)
                    {
                        BarCodeEntity Content = item as BarCodeEntity;
                        this.WriteBarCode(Content,dicSource);
                    }
                }
            }
            this.CurrentHeight += entity.Height;
        }

        /// <summary>
        /// 写入直线
        /// </summary>
        private void WriteLine(StrLineEntity entity)
        {
            g.DrawLine(new Pen(bru), entity.StartX, entity.StartY, entity.EndX, entity.EndY);
        }

        /// <summary>
        /// 写入文本内容
        /// </summary>
        /// <param name="entity"></param>
        private void WriteText(TextEntity entity, Dictionary<string, object> dicSource)
        {
            float CurrentTop = entity.Top + this.CurrentHeight;

            
            if (entity.ContentType == 1)
            {
                string Value = entity.Content;
                FontStyle style = FontStyleOption.GetFontStyleFormat(entity.FontStyle);
                this.g.DrawString(Value, new Font(entity.FontName, entity.FontSize, style), bru, new PointF(entity.Left, CurrentTop));
            }
            else if (entity.ContentType == 2)
            {
                string content = entity.Content;
                int beginIndex = content.IndexOf("{{");
                int endIndex = content.LastIndexOf("}}");
                string key = content.Substring(beginIndex + 2, endIndex - beginIndex - 2);
                string Value = dicSource.Value<string,object>(key) as string;
                if (entity.Start > -1 && entity.End > 0 && Value.IsNotEmpty())
                {
                    Value = Value.SubStr(entity.Start, entity.End);
                }

                FontStyle style = FontStyleOption.GetFontStyleFormat(entity.FontStyle);

                this.g.DrawString(content.Replace("{{" + key + "}}", Value), new Font(entity.FontName, entity.FontSize, style), bru, new PointF(entity.Left, CurrentTop));
            }
        }

        /// <summary>
        /// 写入图片
        /// </summary>
        /// <param name="entity"></param>
        private void WriteImage(ImageEntity entity, Dictionary<string, object> dicSource)
        {
            float CurrentTop = entity.Top + this.CurrentHeight;
            if (entity.ContentType == 2)
            {
                string content = entity.Content;
                int beginIndex = content.IndexOf("{{");
                int endIndex = content.LastIndexOf("}}");
                string key = content.Substring(beginIndex + 2, endIndex - beginIndex - 2);
                string Value = dicSource.Value<string, object>(key) as string;
                if (Value.IsNotEmpty() && File.Exists(Value))
                {
                    Image image = Image.FromFile(Value);
                    if (entity.Width == 0 || entity.Heigth == 0)
                    {
                        g.DrawImage(image, new PointF(entity.Left, CurrentTop));
                    }
                    else
                    {
                        g.DrawImage(image, entity.Left, CurrentTop, entity.Width, entity.Heigth);
                    }
                }
            }
            else if (entity.ContentType == 1)
            {
                string content = entity.Content;
                if (content.IsNotEmpty() && File.Exists(content))
                {
                    Image image = Image.FromFile(content);
                    if (entity.Width == 0 || entity.Heigth == 0)
                    {
                        g.DrawImage(image, new PointF(entity.Left, CurrentTop));
                    }
                    else
                    {
                        g.DrawImage(image, entity.Left, CurrentTop, entity.Width, entity.Heigth);
                    }
                }
            }
        }

        /// <summary>
        /// 写入二维码
        /// </summary>
        /// <param name="entity"></param>
        private void WriteQRCode(QRCodeEntity entity, Dictionary<string, object> dicSource)
        {
            float CurrentTop = entity.Top + this.CurrentHeight;
            string content = string.Empty;
            if (entity.ContentType == 2)
            {
                content = entity.Content;
                int beginIndex = content.IndexOf("{{");
                int endIndex = content.LastIndexOf("}}");
                string key = content.Substring(beginIndex + 2, endIndex - beginIndex - 2);
                string Value = dicSource.Value<string, object>(key) as string;
                content = Value;
            }
            else if (entity.ContentType == 1)
            {
                content = entity.Content;
            }

            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
            QrCode qrCode = new QrCode();
            qrEncoder.TryEncode(content, out qrCode);
            using (MemoryStream ms = new MemoryStream())
            {
                if (entity.ModuleSize <= 0 || entity.ModuleSize > 5)
                {
                    entity.ModuleSize = 3;
                }
                GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(entity.ModuleSize, QuietZoneModules.Two));
                renderer.WriteToStream(qrCode.Matrix, ImageFormat.Jpeg, ms);
                Image image = Image.FromStream(ms);
                g.DrawImage(image, new PointF(entity.Left, CurrentTop));
            }
        }

        /// <summary>
        /// 写入条码
        /// </summary>
        /// <param name="entity"></param>
        private void WriteBarCode(BarCodeEntity entity,Dictionary<string,object> dicSource)
        {
            float CurrentTop = entity.Top + this.CurrentHeight;
            string content = string.Empty;
            if (entity.ContentType == 2)
            {
                content = entity.Content;
                int beginIndex = content.IndexOf("{{");
                int endIndex = content.LastIndexOf("}}");
                string key = content.Substring(beginIndex + 2, endIndex - beginIndex - 2);
                string Value = dicSource.Value<string, object>(key) as string;
                content = Value;
            }
            else if (entity.ContentType == 1)
            {
                content = entity.Content;
            }

            QrCodeEncodingOptions options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = (int)Math.Ceiling(entity.Width),
                Height = (int)Math.Ceiling(entity.Height),
            };
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarCodeOption.GetBarcodeFormat(entity.BarCodeFormat);
            writer.Options = options;
            Bitmap bitmap = writer.Write(content);
            g.DrawImage(bitmap, new PointF(entity.Left, CurrentTop));
        }
    }
}
