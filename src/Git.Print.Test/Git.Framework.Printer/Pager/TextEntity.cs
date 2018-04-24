/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2018/4/24 15:09:01
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2018/4/24 15:09:01       情缘
 * 吉特仓储管理系统 开源地址 https://github.com/hechenqingyuan/gitwms
 * 项目地址:http://yun.gitwms.com/
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Framework.Printer.Pager
{
    public partial class TextEntity : ContentEntity
    {
        public TextEntity() { }

        /// <summary>
        /// 距离上层元素的左边距
        /// </summary>
        public float Left { get; set; }

        /// <summary>
        /// 距离上层元素的上边距
        /// </summary>
        public float Top { get; set; }

        /// <summary>
        /// 打印的字体大小
        /// </summary>
        public string FontSize { get; set; }

        /// <summary>
        /// 字体名称
        /// </summary>
        public string FontName { get; set; }

        /// <summary>
        /// 显示位置
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// 结束位置
        /// </summary>
        public int End { get; set; }
    }
}
