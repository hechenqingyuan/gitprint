/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2018/5/26 16:35:00
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2018/5/26 16:35:00       情缘
 * 吉特仓储管理系统 开源地址 https://github.com/hechenqingyuan/gitwms
 * 项目地址:http://yun.gitwms.com/
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Git.Framework.Printer
{
    public partial class FontStyleOption
    {
        public static FontStyle GetFontStyleFormat(int Value)
        {
            FontStyle Result = FontStyle.Regular;
            if (Value == 0)
            {
                Result = FontStyle.Regular;
            }
            else if (Value == 1)
            {
                Result = FontStyle.Bold;
            }
            else if (Value == 2)
            {
                Result = FontStyle.Italic;
            }
            else if (Value == 4)
            {
                Result = FontStyle.Underline;
            }
            else if (Value == 8)
            {
                Result = FontStyle.Strikeout;
            }
            return Result;
        }
    }
}
