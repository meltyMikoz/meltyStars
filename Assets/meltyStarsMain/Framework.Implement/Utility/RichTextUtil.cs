using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KuusouEngine
{
    /// <summary>
    /// 富文本转化工具
    /// Text和日志都能用
    /// </summary>
    public static class RichTextUtil
    {
        public const string White = "FFFFFF";
        public const string Black = "000000";
        public const string Red = "FF0000";
        public const string Green = "00FF18";
        public const string Oringe = "FF9400";
        /// <summary>
        /// 将字符串转换为富文本格式
        /// </summary>
        /// <param name="str"></param>
        /// <param name="richTextColor"></param>
        /// <returns></returns>
        public static string ToRichText(this string str, string richTextColor)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            return $"<color=#{richTextColor}>{str}</color>";
        }
    }
}