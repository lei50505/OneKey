using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace OneKey.src.util
{
    public static class UMD5
    {
        private static MD5 md5 = new MD5CryptoServiceProvider();
        private static byte[] salt = UConvert.strToBytes("poiuytrewq321");
        private static byte[] bytesToBytes(byte[] bytes)
        {
            return md5.ComputeHash(bytes);
        }
        private static byte[] bytesToSaltBytes(byte[] bytes)
        {
            byte[] newBytes = new byte[bytes.Length+salt.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                newBytes[i] = bytes[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                newBytes[bytes.Length + i] = salt[i];
            }
            return bytesToBytes(newBytes);
        }
        private static byte[] strToBytes(string str)
        {
            byte[] bytes = UConvert.strToBytes(str);
            return bytesToBytes(bytes);
        }
        private static byte[] strToSaltBytes(string str)
        {
            byte[] bytes = UConvert.strToBytes(str);
            return bytesToSaltBytes(bytes);
        }
        private static string bytesToBase64Str(byte[] bytes)
        {
            byte[] md5Bytes = bytesToBytes(bytes);
            return UConvert.bytesToBase64Str(md5Bytes);
        }
        private static string bytesToSaltBase64Str(byte[] bytes)
        {
            byte[] md5Bytes = bytesToSaltBytes(bytes);
            return UConvert.bytesToBase64Str(md5Bytes);
        }
        private static string strToBase64Str(string str)
        {
            byte[] md5Bytes = strToBytes(str);
            return UConvert.bytesToBase64Str(md5Bytes);
        }
        public static string strToSaltBase64Str(string str)
        {
            byte[] md5Bytes = strToSaltBytes(str);
            return UConvert.bytesToBase64Str(md5Bytes);
        }
    }
}
