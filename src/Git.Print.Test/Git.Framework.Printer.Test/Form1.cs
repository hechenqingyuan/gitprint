
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Git.Framework.Printer.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnDocumentDic_Click(object sender, EventArgs e)
        {
            string tempalte = System.AppDomain.CurrentDomain.BaseDirectory + "\\Template\\Template.xml";
            Dictionary<string, object> dic = GetDataSource();
            IPrint instance = new DocumentPrint(tempalte, "", dic);
            instance.Init().Print();
        }

        private Dictionary<string, object> GetDataSource()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic.Add("Logo", System.AppDomain.CurrentDomain.BaseDirectory + "abc.jpg");
            dic.Add("OrderCode", "V3454596546");
            dic.Add("DtReceive", "菜霞");
            dic.Add("ReceiveAddress", "634");
            dic.Add("ReceiveUser", "120457");
            dic.Add("ReceiverPhone", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            dic.Add("DtCreate", "65223.00");
            dic.Add("DAmount", "1254");
            dic.Add("DCount", "232");
            dic.Add("StrComment", "MU54232");

            List<Dictionary<string, object>> Info = new List<Dictionary<string, object>>()
            {
                new Dictionary<string, object>() { { "Index", "1"},{ "StrID", "120223"},{ "StrName", "中华烟"},{ "DCount", "2"},{ "DPrice", "49"},{ "DAmount", "98"} },
                new Dictionary<string, object>() { { "Index", "2"},{ "StrID", "565666"},{ "StrName", "玻璃杯"},{ "DCount", "7"},{ "DPrice", "45"},{ "DAmount", "45545"} },
                new Dictionary<string, object>() { { "Index", "3"},{ "StrID", "897845"},{ "StrName", "烟灰缸"},{ "DCount", "5"},{ "DPrice", "2435"},{ "DAmount", "67767"} },
                new Dictionary<string, object>() { { "Index", "4"},{ "StrID", "904395"},{ "StrName", "茶几"},{ "DCount", "3"},{ "DPrice", "45245"},{ "DAmount", "6767"} },

            };
            dic.Add("Detials", Info);

            return dic;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tempalte = System.AppDomain.CurrentDomain.BaseDirectory + "\\Template\\Bluetooth.xml";
            Dictionary<string, object> dic = GetDataSource();
            IPrint instance = new HMA300Print(tempalte, "", dic);
            instance.Init().Print();

            //int result = 0;

            //cpcl_dll dll = new cpcl_dll();
            //dll.printer = cpcl_dll.PrinterCreatorS("HM-A300");
            //// dll.printer = cpcl_dll.PrinterCreatorS("HM-Z3-605F");
            ////dll.printer = cpcl_dll.PrinterCreatorS("COM5");
            //if (0 == dll.printer)
            //{
            //    MessageBox.Show("Create Model False");
            //    return;
            //}
            ////result = cpcl_dll.PortOpen(dll.printer, "USB");
            //result = cpcl_dll.PortOpen(dll.printer, "COM5,BAUDRATE_9600");
            //if (0 != result)
            //{
            //    MessageBox.Show("Port Open False");
            //    return;
            //}

            ////Set Size
            //result = cpcl_dll.CPCL_AddLabel(dll.printer, 0, 1000, 1);
            ///*
            ////Set Text Underline  and Bold
            //result = cpcl_dll.CPCL_SetTextUnderline(dll.printer, 1);
            //result = cpcl_dll.CPCL_SetTextBold(dll.printer, 2);
            //result = cpcl_dll.CPCL_AddText(dll.printer, cpcl_dll.ROTATE_NONE, 4,7,8, 0, "SetTextUnderline  TextBold");
            // */
            ////normal Text
            ////result = cpcl_dll.CPCL_SetTextUnderline(dll.printer, 0);
            ////result = cpcl_dll.CPCL_SetTextBold(dll.printer, 0);
            //result = cpcl_dll.CPCL_AddText(dll.printer, cpcl_dll.ROTATE_NONE, 0, 0, 0, 0, "12345678900000");


            ////ACTIVE BARCODE TEXT
            //result = cpcl_dll.CPCL_AddBarCodeText(dll.printer, 1, 7, 0, 0);
            ////CODE 128
            //result = cpcl_dll.CPCL_AddBarCode(dll.printer, cpcl_dll.ROTATE_NONE, cpcl_dll.BARCODE_128, 2, 2, 50, 8, 98, "code128");


            ////CODE 39
            //result = cpcl_dll.CPCL_AddBarCode(dll.printer, cpcl_dll.ROTATE_NONE, cpcl_dll.BARCODE_39, 2, 2, 50, 8, 176, "CODE39");

            ////CODE 93
            //result = cpcl_dll.CPCL_AddBarCode(dll.printer, cpcl_dll.ROTATE_NONE, cpcl_dll.BARCODE_93, 2, 2, 50, 8, 252, "CODE93");

            ////BARCODE
            //result = cpcl_dll.CPCL_AddBarCode(dll.printer, cpcl_dll.ROTATE_NONE, cpcl_dll.BARCODE_CODABAR, 2, 2, 50, 8, 328, "A1234567890A");

            ////ENA13
            //result = cpcl_dll.CPCL_AddBarCode(dll.printer, cpcl_dll.ROTATE_NONE, cpcl_dll.BARCODE_EAN13, 2, 2, 50, 8, 400, "123456789012");

            ////ENA8
            //result = cpcl_dll.CPCL_AddBarCode(dll.printer, cpcl_dll.ROTATE_NONE, cpcl_dll.BARCODE_EAN8, 2, 2, 50, 8, 472, "1234567");

            ////QRCODE 2
            //result = cpcl_dll.CPCL_AddQRCode(dll.printer, cpcl_dll.ROTATE_NONE, 8, 560, 2, 5, cpcl_dll.ECC_LEVEL_H, "QRCODE654321");
            ////QRCODE 1
            //result = cpcl_dll.CPCL_AddQRCode(dll.printer, cpcl_dll.ROTATE_NONE, 200, 560, 1, 5, cpcl_dll.ECC_LEVEL_H, "QRCODE123456");

            ////Image
            //// result = cpcl_dll.CPCL_AddImage(dll.printer, cpcl_dll.ROTATE_NONE, 200, 700, "0.JPG");

            ////LINE
            //result = cpcl_dll.CPCL_AddLine(dll.printer, 8, 700, 8, 900, 2);

            ////BOX
            //result = cpcl_dll.CPCL_AddBox(dll.printer, 24, 700, 100, 900, 2);



            //// result = cpcl_dll.CPCL_PreFeed(dll.printer, 20);
            //result = cpcl_dll.CPCL_Print(dll.printer);
            //result = cpcl_dll.PortClose(dll.printer);
            //result = cpcl_dll.PrinterDestroy(dll.printer);
        }

        /// <summary>
        /// 生成二维码图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>()
            {
                "TK101","TK102","TK103","TK104",

                "TK201","TK202","TK203","TK204","TK205","TK206","TK207","TK208","TK209","TK210","TK211","TK212","TK213","TK214",

                "TK301","TK302","TK303","TK304",

                "TK401","TK402","TK403","TK404","TK405","TK406","TK407","TK408","TK409","TK410","TK411","TK412","TK413","TK414",
            };
            foreach (string str in list)
            {
                string Content = str;
                QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                QrCode qrCode = new QrCode();
                qrEncoder.TryEncode(Content, out qrCode);
                using (MemoryStream ms = new MemoryStream())
                {
                    GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(18, QuietZoneModules.Two), Brushes.White, Brushes.MidnightBlue);
                    
                    renderer.WriteToStream(qrCode.Matrix, ImageFormat.Jpeg, ms);
                    Image image = Image.FromStream(ms);
                    image.Save(str+".jpg");
                }
            }
        }
    }


    public class cpcl_dll
    {
        [DllImport("CPCL_SDK")]
        public static extern int PrinterCreator(ref IntPtr printer, string model);

        [DllImport("CPCL_SDK")]
        public static extern int PrinterCreatorS(string model);

        [DllImport("CPCL_SDK")]
        public static extern int PortOpen(int printer, string portSetting);

        [DllImport("CPCL_SDK")]
        public static extern int PortClose(int printer);

        [DllImport("CPCL_SDK")]
        public static extern int PrinterDestroy(int printer);

        [DllImport("CPCL_SDK")]
        public static extern int DirectIO(int printer, byte[] writedata, int wirteNum, byte[] readdata, int readNum, ref int preadedNum);

        [DllImport("CPCL_SDK")]
        public static extern int CPCL_AddLabel(int printer, int offSet, int height, int qty);

        [DllImport("CPCL_SDK")]
        public static extern int CPCL_AddAlign(int printer, int align);

        [DllImport("CPCL_SDK")]
        public static extern int CPCL_AddText(int printer, int rotate, int fontType, int fontSize, int xPos, int yPos, string data);

        [DllImport("CPCL_SDK")]
        public static extern int CPCL_AddBarCode(int printer, int rotate, int type, int width, int ratio, int height, int xPos, int yPos, string data);

        [DllImport("CPCL_SDK")]
        public static extern int CPCL_AddBarCodeText(int printer, int enable, int fontType, int fontSize, int offSet);

        [DllImport("CPCL_SDK")]
        public static extern int CPCL_AddQRCode(int printer, int rotate, int xPos, int yPos, int model, int unitWidth, int eccLevel, string data);

        [DllImport("CPCL_SDK")]
        public static extern int CPCL_AddPDF417(int printer, int rotate, int xPos, int yPos, int xDots, int yDots, int columns, int rows, int eccLevel, string data);

        [DllImport("CPCL_SDK")]
        public static extern int CPCL_AddBox(int printer, int xPos, int yPos, int endXPos, int endYPos, int thickness);

        [DllImport("CPCL_SDK")]
        public static extern int CPCL_AddLine(int printer, int xPos, int yPos, int endXPos, int endYPos, int thickness);

        [DllImport("CPCL_SDK")]
        public static extern int CPCL_AddImage(int printer, int rotate, int xPos, int yPos, string imagePath);

        [DllImport("CPCL_SDK")]
        public static extern int CPCL_AddImageData(int printer, int rotate, int widthBytes, int height, int xPos, int yPos, byte[] data);

        [DllImport("CPCL_SDK")]
        public static extern int CPCL_SetFontSize(int printer, int width, int height);


        [DllImport("CPCL_SDK")]
        public static extern int CPCL_SetDensity(int printer, int density);

        [DllImport("CPCL_SDK")]
        public static extern int CPCL_SetSpeed(int printer, int speed);

        [DllImport("CPCL_SDK")]
        public static extern int CPCL_SetTextSpacing(int printer, int Spacing);
        [DllImport("CPCL_SDK")]
        public static extern int CPCL_SetLeftMargin(int printer, int Margin);

        [DllImport("CPCL_SDK")]
        public static extern int CPCL_SetTextBold(int printer, int bold);

        [DllImport("CPCL_SDK")]
        public static extern int CPCL_SetTextUnderline(int printer, int underline);

        [DllImport("CPCL_SDK")]
        public static extern int CPCL_Abort(int printer);
        [DllImport("CPCL_SDK")]
        public static extern int CPCL_Print(int printer);

        [DllImport("CPCL_SDK")]
        public static extern int CPCL_NextLabelPos(int printer);

        [DllImport("CPCL_SDK")]
        public static extern int CPCL_PreFeed(int printer, int distance);


        [DllImport("CPCL_SDK")]
        public static extern int CPCL_PostFeed(int printer, int distance);

        public int printer;

        public const int BARCODE_128 = 0;
        public const int BARCODE_128A = 1;
        public const int BARCODE_128B = 2;
        public const int BARCODE_128C = 3;
        public const int BARCODE_128E = 4;
        public const int BARCODE_39 = 5;
        public const int BARCODE_39C = 6;
        public const int BARCODE_93 = 7;
        public const int BARCODE_CODABAR = 8;
        public const int BARCODE_CODABAR16 = 9;
        public const int BARCODE_EAN13 = 10;
        public const int BARCODE_EAN132 = 11;
        public const int BARCODE_EAN135 = 12;
        public const int BARCODE_EAN8 = 13;
        public const int BARCODE_EAN82 = 14;

        //rotate
        public const int ROTATE_NONE = 0;
        public const int ROTATE_90 = 1;
        public const int ROTATE_180 = 2;
        public const int ROTATE_270 = 3;

        //ecc level
        public const int ECC_LEVEL_L = 0;
        public const int ECC_LEVEL_M = 1;
        public const int ECC_LEVEL_Q = 2;
        public const int ECC_LEVEL_H = 3;
    }
}
