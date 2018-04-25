/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2018/4/25 11:13:07
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2018/4/25 11:13:07       情缘
 * 吉特仓储管理系统 开源地址 https://github.com/hechenqingyuan/gitwms
 * 项目地址:http://yun.gitwms.com/
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZXing;

namespace Git.Framework.Printer
{
    public partial class BarCodeOption
    {
        /// <summary>
        /// 获取条码的类型
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static BarcodeFormat GetBarcodeFormat(int Value)
        {
            BarcodeFormat Result = BarcodeFormat.CODE_128;
            if (Value == 4)
            {
                Result = BarcodeFormat.CODE_39;
            }
            else if (Value == 8)
            {
                Result = BarcodeFormat.CODE_93;
            }
            else if (Value == 16)
            {
                Result = BarcodeFormat.CODE_128;
            }
            else if (Value == 64)
            {
                Result = BarcodeFormat.EAN_8;
            }
            else if (Value == 128)
            {
                Result = BarcodeFormat.EAN_13;
            }
            else if (Value == 2048)
            {
                Result = BarcodeFormat.QR_CODE;
            }
            return Result;
        }
    }
}
