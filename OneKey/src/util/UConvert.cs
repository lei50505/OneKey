using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneKey.src.util
{
    public static class UConvert
    {
        public static string bytesToBase64Str(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
        public static byte[] base64StrToBytes(string str)
        {
            return Convert.FromBase64String(str);
        }
        public static byte[] strToBytes(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
        public static string bytesToStr(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
