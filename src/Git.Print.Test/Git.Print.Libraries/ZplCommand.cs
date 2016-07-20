using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Git.Print.Libraries
{
    public partial class ZplCommand
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ZplCommand()
        {
        }

        /// <summary>
        /// 开始指令 ^XA
        /// </summary>
        /// <returns></returns>
        public string ZPL_Start()
        {
            return "^XA";
        }

        /// <summary>
        /// 结束指令
        /// </summary>
        /// <returns></returns>
        public string ZPL_End()
        {
            return "^XZ";
        }

        /// <summary>
        /// 设置打印纸张大小
        /// ^PW{0}：打印宽度，如果宽度较小，则会出现打印不全的效果
        /// ^LL{0} 标签长度
        /// </summary>
        /// <param name="Width">打印纸的宽度</param>
        /// <param name="Height">打印纸长度</param>
        /// <returns></returns>
        public string ZPL_PageSize(int Width,int Height)
        {
            return string.Format("^PW{0}^LL{1}", Width, Height);
        }

        /// <summary>
        /// 设置打印纸边距
        /// ^LH{0},{1}
        /// </summary>
        /// <param name="Left">左边距</param>
        /// <param name="Top">右边距</param>
        /// <returns></returns>
        public string ZPL_MarginBorder(int Left, int Top)
        {
            return string.Format("^LH{0},{1}", Left, Top);
        }

        /// <summary>
        /// 切刀指令
        /// ^MM  C = 切刀 Y = 非连续纸
        /// </summary>
        /// <returns></returns>
        public string ZPL_Cutter()
        {
            return "^MMC,Y";
        }

        /// <summary>
        /// 打印英文文本
        /// </summary>
        /// <param name="Content">打印文本内容</param>
        /// <param name="FontName">打印字体</param>
        /// <param name="Left">左边距</param>
        /// <param name="Top">顶边距</param>
        /// <param name="Orient">旋转角度 N = 正常 （Normal) R = 顺时针旋转90度（Roated) I = 顺时针旋转180度（Inverted) B = 顺时针旋转270度 (Bottom)</param>
        /// <param name="Height">字体高度</param>
        /// <param name="Width">字体宽度</param>
        /// <returns></returns>
        public string ZPL_EnText(string Content, string FontName, int Left, int Top, string Orient, int Height, int Width)
        {
            string command = "^FO{1},{2}^A" + FontName + "{3},{4},{5}^FD{0}^FS";
            return string.Format(command, Content, Left, Top, Orient, Height, Width);
        }

        /// <summary>  
        /// 中文处理  
        /// </summary>  
        /// <param name="ChineseText">待转变中文内容</param>  
        /// <param name="FontName">字体名称</param>  
        /// <param name="Orient">旋转角度0,90,180,270</param>  
        /// <param name="Height">字体高度</param>  
        /// <param name="Width">字体宽度，通常是0</param>  
        /// <param name="IsBold">1 变粗,0 正常</param>  
        /// <param name="IsItalic">1 斜体,0 正常</param>  
        /// <param name="ReturnPicData">返回的图片字符</param>  
        /// <returns></returns>  
        [DllImport("fnthex32.dll")]
        public static extern int GETFONTHEX(string ChineseText,string FontName,int Orient,int Height,int Width,int IsBold,int IsItalic,StringBuilder ReturnPicData);

        /// <summary>  
        /// 中文处理  
        /// </summary>  
        /// <param name="ChineseText">待转变中文内容</param>  
        /// <param name="FontName">字体名称</param>  
        /// <param name="FileName">返回的图片字符重命</param>  
        /// <param name="Orient">旋转角度0,90,180,270</param>  
        /// <param name="Height">字体高度</param>  
        /// <param name="Width">字体宽度，通常是0</param>  
        /// <param name="IsBold">1 变粗,0 正常</param>  
        /// <param name="IsItalic">1 斜体,0 正常</param>  
        /// <param name="ReturnPicData">返回的图片字符</param>  
        /// <returns></returns>  
        [DllImport("fnthex32.dll")]
        public static extern int GETFONTHEX(string ChineseText,string FontName,string FileName,int Orient,int Height,int Width,int IsBold,int IsItalic,StringBuilder ReturnPicData);

        /// <summary>
        /// 打印Code128条码
        /// </summary>
        /// <param name="Left">左边距</param>
        /// <param name="Top">顶边距</param>
        /// <param name="Width">模块（窄条）宽 初始化值：2点 可接受的数值：1-10点</param>
        /// <param name="Ratio">宽条与窄条的比例 初始化值：３.0 可接受的数值：２.0到3.0，０.1的增量（对固定比例的条码无效）</param>
        /// <param name="Height">条码高度开机初始化值：１０点 可接受的数值：１点到标签高度。</param>
        /// <param name="Content">条码内容 条码只能是英文字符</param>
        /// <returns></returns>
        public string ZPL_Barcode128(int Left, int Top, int Width, int Ratio, int Height, string Content)
        {
            string command = "^FO{0},{1}^BY{2},{3}^BCN,{4},N,N^FD{5}^FS";
            return string.Format(command, Left, Top, Width, Ratio, Height, Content);
        }

        /// <summary>
        /// 打印二维码
        /// </summary>
        /// <param name="Left">左边距</param>
        /// <param name="Top">顶边距</param>
        /// <param name="Conent">二维码内容</param>
        /// <returns></returns>
        public string ZPL_QRCode(int Left, int Top,string Conent)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("^FO{0},{1}", Left, Top);
            sb.Append("^BQ,2,7");
            sb.AppendFormat("^FDLA,{0}^FS", Conent);
            return sb.ToString();
        }
    }
}
