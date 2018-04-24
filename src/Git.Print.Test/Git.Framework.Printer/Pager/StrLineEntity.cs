/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2018/4/24 15:11:08
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2018/4/24 15:11:08       情缘
 * 吉特仓储管理系统 开源地址 https://github.com/hechenqingyuan/gitwms
 * 项目地址:http://yun.gitwms.com/
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Framework.Printer.Pager
{
    /// <summary>
    /// 直线坐标系
    /// </summary>
    public partial class StrLineEntity:ContentEntity
    {
        public StrLineEntity() { }

        public float StartX { get; set; }

        public float StartY { get; set; }

        public float EndX { get; set; }

        public float EndY { get; set; }
    }
}
