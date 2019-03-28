/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2018/4/24 15:35:14
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2018/4/24 15:35:14       情缘
 * 吉特仓储管理系统 开源地址 https://github.com/hechenqingyuan/gitwms
 * 项目地址:http://yun.gitwms.com/
*********************************************************************************/

using Git.Framework.Io;
using Git.Framework.Printer.Pager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Git.Framework.Printer
{
    public partial class PageXmlReader
    {
        public string FilePath { get; set; }

        private XDocument Root { get; set; }

        private int RowIndex { get; set; }

        public PageXmlReader(string FilePath)
        {
            this.FilePath = FilePath;
        }

        /// <summary>
        /// 获取文档对象
        /// </summary>
        /// <returns></returns>
        public PageEntity Read()
        {
            PageEntity Result = new PageEntity();

            if (!FileManager.FileExists(this.FilePath))
            {
                throw new Exception("打印模板文件不存在");
            }

            this.Root = XDocument.Load(this.FilePath);

            float Width = this.Root.Element("Page").Value<float>("Width");
            float Heigth = this.Root.Element("Page").Value<float>("Heigth");
            string DefaultPrinter = this.Root.Element("Page").Value("DefaultPrinter");
            bool AutoHeight = this.Root.Element("Page").Value<bool>("AutoHeight");

            Result.Width = Width;
            Result.Heigth = Heigth;
            Result.DefaultPrinter = DefaultPrinter;
            Result.AutoHeight = AutoHeight;
            Result.Rows = new List<RowEntity>();

            foreach (XElement item in this.Root.Element("Page").Elements())
            {
                if (item.Name == "Line")
                {
                    LineEntity LineRow = this.ReadLine(item);
                    Result.Rows.Add(LineRow);
                }
                else if (item.Name == "Loop")
                {
                    LoopEntity LineRow = this.ReadLoop(item);
                    Result.Rows.Add(LineRow);
                }
                else if (item.Name == "Table")
                {
                    TableEntity LineRow = this.ReadTable(item);
                    Result.Rows.Add(LineRow);
                }
            }

            return Result;
        }

        /// <summary>
        /// 读取表格
        /// </summary>
        /// <param name="ETable"></param>
        /// <returns></returns>
        private TableEntity ReadTable(XElement ETable)
        {
            TableEntity Result = new TableEntity();
            string Values = ETable.Value<string>("Values");
            float BorderWidth = ETable.Value<float>("BorderWidth");
            float Width = ETable.Value<float>("Width");
            float Left = ETable.Value<float>("Left");
            float Top = ETable.Value<float>("Top");

            Result.Index = RowIndex;
            Result.RowType = (int)ERowType.Table;
            Result.KeyName = Values;
            Result.BorderWidth = BorderWidth;
            Result.Width = Width;
            Result.Left = Left;
            Result.Top = Top;
            Result.Head = this.ReadTHead(ETable);
            Result.ListTR = this.ReadTR(ETable);
            return Result;
        }

        /// <summary>
        /// 读取表头信息
        /// </summary>
        /// <returns></returns>
        private THeadEntity ReadTHead(XElement ETable)
        {
            THeadEntity Result = null;
            XElement THead = ETable.Element("THead");
            if (THead != null)
            {
                Result = new THeadEntity();
                float Height = THead.Value<float>("Height");
                Result.Height = Height;
                Result.ListTD = this.ReadTd(THead);
            }
            return Result;
        }

        /// <summary>
        /// 读取TR中的内容
        /// </summary>
        /// <param name="ETable"></param>
        /// <returns></returns>
        private List<TrEntity> ReadTR(XElement ETable)
        {
            List<TrEntity> listResult = new List<TrEntity>();
            foreach (XElement TR in ETable.Elements("Tr"))
            {
                TrEntity entity = new TrEntity();
                float Height = TR.Value<float>("Height");
                entity.Height = Height;
                entity.ListTD = this.ReadTd(TR);
                listResult.Add(entity);
            }
            return listResult;
        }

        /// <summary>
        /// 读取TD集合
        /// </summary>
        /// <param name="Tr"></param>
        /// <returns></returns>
        private List<TdEntity> ReadTd(XElement Tr)
        {
            List<TdEntity> listResult = new List<TdEntity>();
            if (Tr != null)
            {
                foreach (XElement td in Tr.Elements("Td"))
                {
                    TdEntity entity = new TdEntity();
                    float Width = td.Value<float>("Width");
                    entity.Width = Width;
                    entity.ListContent = this.ReadContent(td);
                    listResult.Add(entity);
                }
            }
            return listResult;
        }

        /// <summary>
        /// 读取Loop节点
        /// </summary>
        /// <param name="ELoop"></param>
        /// <returns></returns>
        private LoopEntity ReadLoop(XElement ELoop)
        {
            LoopEntity Result = new LoopEntity();
            string Values = ELoop.Value<string>("Values");
            Result.Index = RowIndex;
            Result.KeyName = Values;
            Result.RowType = (int)ERowType.Loop;
            Result.ListLine = new List<LineEntity>();
            foreach (XElement Node in ELoop.Elements("Line"))
            {
                LineEntity LineRow = this.ReadLine(Node);
                Result.ListLine.Add(LineRow);
            }
            return Result;
        }

        /// <summary>
        /// 读取行Line
        /// </summary>
        /// <param name="ELine"></param>
        /// <returns></returns>
        private LineEntity ReadLine(XElement ELine)
        {
            LineEntity Result = new LineEntity();

            float Height = ELine.Value<float>("Height");
            Result.Index = RowIndex;
            Result.RowType = (int)ERowType.Line;
            Result.Height = Height;
            Result.ListContent = this.ReadContent(ELine);
            return Result;
        }

        /// <summary>
        /// 读取行元素中的内容元素
        /// </summary>
        /// <param name="EL"></param>
        /// <returns></returns>
        private List<ContentEntity> ReadContent(XElement EL)
        {
            List<ContentEntity> listResult = new List<ContentEntity>();
            foreach (XElement Node in EL.Elements())
            {
                if (Node.Name == "Text")
                {
                    TextEntity Result = this.ReadText(Node);
                    listResult.Add(Result);
                }
                else if (Node.Name == "StrLine")
                {
                    StrLineEntity Result = this.ReadStrLine(Node);
                    listResult.Add(Result);
                }
                else if (Node.Name == "Image")
                {
                    ImageEntity Result = this.ReadImage(Node);
                    listResult.Add(Result);
                }
                else if (Node.Name == "QRCode")
                {
                    QRCodeEntity Result = this.ReadQRCode(Node);
                    listResult.Add(Result);
                }
                else if (Node.Name == "BarCode")
                {
                    BarCodeEntity Result = this.ReadBarCode(Node);
                    listResult.Add(Result);
                }
            }
            return listResult;
        }

        /// <summary>
        /// 读取文本节点信息
        /// </summary>
        /// <param name="Node"></param>
        /// <returns></returns>
        private TextEntity ReadText(XElement Node)
        {
            TextEntity Result = new TextEntity();

            float Left = Node.Value<float>("Left");
            float Top = Node.Value<float>("Top");
            float FontSize = Node.Value<float>("FontSize");
            string FontName = Node.Value("FontName");
            int Start = Node.Value<int>("Start");
            int End = Node.Value<int>("End");
            int FontStyle = Node.Value<int>("FontStyle");

            string Content = Node.Value;
            if (Content.Contains("{{") && Content.Contains("}}"))
            {
                Result.ContentType = 2;
            }
            else
            {
                Result.ContentType = 1;
            }
            Result.Content = Content;
            Result.Left = Left;
            Result.Top = Top;
            Result.FontSize = FontSize;
            Result.FontName = FontName;
            Result.Start = Start;
            Result.End = End;
            Result.FontStyle = FontStyle;

            return Result;
        }

        /// <summary>
        /// 读取直线节点
        /// </summary>
        /// <param name="Node"></param>
        /// <returns></returns>
        private StrLineEntity ReadStrLine(XElement Node)
        {
            StrLineEntity Result = new StrLineEntity();

            float StartX = Node.Value<float>("StartX");
            float StartY = Node.Value<float>("StartY");
            float EndX = Node.Value<float>("EndX");
            float EndY = Node.Value<float>("EndY");
            
            Result.ContentType = 1;
            Result.StartX = StartX;
            Result.StartY = StartY;
            Result.EndX = EndX;
            Result.EndY = EndY;

            return Result;
        }

        /// <summary>
        /// 读取图片的节点
        /// </summary>
        /// <param name="Node"></param>
        /// <returns></returns>
        private ImageEntity ReadImage(XElement Node)
        {
            ImageEntity Result = new ImageEntity();

            float Left = Node.Value<float>("Left");
            float Top = Node.Value<float>("Top");
            float Width = Node.Value<float>("Width");
            float Heigth = Node.Value<float>("Heigth");
            string Content = Node.Value;

            if (Content.Contains("{{") && Content.Contains("}}"))
            {
                Result.ContentType = 2;
            }
            else
            {
                Result.ContentType = 1;
            }
            Result.Content = Content;
            Result.Left = Left;
            Result.Top = Top;
            Result.Width = Width;
            Result.Heigth = Heigth;

            return Result;
        }

        /// <summary>
        /// 读取二维码节点
        /// </summary>
        /// <param name="Node"></param>
        /// <returns></returns>
        private QRCodeEntity ReadQRCode(XElement Node)
        {
            QRCodeEntity Result = new QRCodeEntity();

            float Left = Node.Value<float>("Left");
            float Top = Node.Value<float>("Top");
            int ModuleSize = Node.Value<int>("ModuleSize");

            string Content = Node.Value;

            if (Content.Contains("{{") && Content.Contains("}}"))
            {
                Result.ContentType = 2;
            }
            else
            {
                Result.ContentType = 1;
            }
            Result.Content = Content;
            Result.Left = Left;
            Result.Top = Top;
            Result.ModuleSize = ModuleSize;

            return Result;
        }

        /// <summary>
        /// 读取条码节点
        /// </summary>
        /// <param name="Node"></param>
        /// <returns></returns>
        private BarCodeEntity ReadBarCode(XElement Node)
        {
            BarCodeEntity Result = new BarCodeEntity();

            float Left = Node.Value<float>("Left");
            float Top = Node.Value<float>("Top");
            float Width = Node.Value<float>("Width");
            float Height = Node.Value<float>("Height");
            int BarCodeFormat = Node.Value<int>("BarCodeFormat");

            string Content = Node.Value;

            if (Content.Contains("{{") && Content.Contains("}}"))
            {
                Result.ContentType = 2;
            }
            else
            {
                Result.ContentType = 1;
            }
            Result.Content = Content;
            Result.Left = Left;
            Result.Top = Top;
            Result.Width = Width;
            Result.Height = Height;
            Result.BarCodeFormat = BarCodeFormat;

            return Result;
        }
    }
}
