/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2018/4/24 15:42:20
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2018/4/24 15:42:20       情缘
 * 吉特仓储管理系统 开源地址 https://github.com/hechenqingyuan/gitwms
 * 项目地址:http://yun.gitwms.com/
*********************************************************************************/

using Git.Framework.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Git.Framework.Printer
{
    public static class XmlElementExtension
    {
        /// <summary>
        /// 获取属性的值
        /// </summary>
        /// <param name="Attr"></param>
        /// <returns></returns>
        public static string Value(this XAttribute Attr)
        {
            if (Attr != null)
            {
                return string.Empty;
            }

            return Attr.Value;
        }

        /// <summary>
        /// 获取属性的值 并且转化为特定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Attr"></param>
        /// <returns></returns>
        public static T Value<T>(this XAttribute Attr)
        {
            T Result = default(T);
            if (Attr != null)
            {
                return Result;
            }

            return ConvertHelper.ToType<T>(Attr.Value);
        }

        /// <summary>
        /// 获取属性的值
        /// </summary>
        /// <param name="EL"></param>
        /// <param name="AttrName"></param>
        /// <returns></returns>
        public static string Value(this XElement EL, string AttrName)
        {
            string Result = string.Empty;
            if (EL == null || EL.Attribute(AttrName) == null)
            {
                return Result;
            }
            Result = EL.Attribute(AttrName).Value;
            return Result;
        }

        /// <summary>
        /// 获取属性的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="EL"></param>
        /// <param name="AttrName"></param>
        /// <returns></returns>
        public static T Value<T>(this XElement EL, string AttrName)
        {
            T Result = default(T);
            if (EL == null || EL.Attribute(AttrName) == null)
            {
                return Result;
            }
            string Val= EL.Attribute(AttrName).Value;
            Result = ConvertHelper.ToType<T>(Val);
            return Result;
        }
    }
}
