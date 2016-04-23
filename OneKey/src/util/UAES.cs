using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace OneKey.src.util
{
    public static class UAES
    {
        private static byte[] IV = UConvert.strToBytes("qwertyuiop123456");
        private static RijndaelManaged rm = new RijndaelManaged();
        static UAES()
        {
            rm.IV = IV;
            rm.Padding = PaddingMode.PKCS7;
            rm.Mode = CipherMode.CBC;
        }
        public static byte[] bytesToKey(byte[] bytes)
        {
            byte[] key = new byte[32];
            if (bytes.Length < 32)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    key[i] = bytes[i];
                }
                for (int i = bytes.Length; i < key.Length; i++)
                {
                    key[i] = 97;
                }
            }
            else
            {
                for (int i = 0; i < key.Length; i++)
                {
                    key[i] = bytes[i];
                }
            }
            return key;
        }
        public static byte[] encryptBytesToBytes(byte[] bytes,byte[] key)
        {
            rm.Key = bytesToKey(key);
            ICryptoTransform ct = rm.CreateEncryptor();
            byte[] encryptBytes= ct.TransformFinalBlock(bytes, 0, bytes.Length);
            return encryptBytes;
        }
        public static byte[] encryptBytesToBytes(byte[] bytes, string key)
        {
            byte[] bytesKey = UConvert.strToBytes(key);
            return encryptBytesToBytes(bytes, bytesKey);
        }
        public static byte[] decryptBytesToBytes(byte[] bytes, byte[] key)
        {
            rm.Key = bytesToKey(key);
            ICryptoTransform ct = rm.CreateDecryptor();
            byte[] decryptBytes = ct.TransformFinalBlock(bytes, 0, bytes.Length);
            return decryptBytes;
        }
        public static byte[] decryptBytesToBytes(byte[] bytes, string key)
        {
            byte[] bytesKey = UConvert.strToBytes(key);
            return decryptBytesToBytes(bytes,bytesKey);
        }
        public static byte[] encryptStrToBytes(string str, byte[] key)
        {
            byte[] bytes = UConvert.strToBytes(str);
            return encryptBytesToBytes(bytes,key);
        }
        public static byte[] encryptStrToBytes(string str, string key)
        {
            byte[] bytesKey = UConvert.strToBytes(key);
            return encryptStrToBytes(str, bytesKey);
        }
        public static string decryptBytesToStr(byte[] bytes, byte[] key)
        {
            byte[] decryptBytes=decryptBytesToBytes(bytes, key);
            return UConvert.bytesToStr(decryptBytes);
        }
        public static string decryptBytesToStr(byte[] bytes, string key)
        {
            byte[] bytesKey = UConvert.strToBytes(key);
            return decryptBytesToStr(bytes, bytesKey);
        }
        public static string encryptStrToStr(string str, byte[] key)
        {
            byte[] bytes = encryptStrToBytes(str,key);
            return UConvert.bytesToBase64Str(bytes);
        }
        public static string encryptStrToStr(string str, string key)
        {
            byte[] bytesKey = UConvert.strToBytes(key);
            return encryptStrToStr(str,bytesKey);
        }
        public static string decryptStrToStr(string str, byte[] key)
        {
            byte[] bytes = UConvert.base64StrToBytes(str);
            return decryptBytesToStr(bytes, key);
        }
        public static string decryptStrToStr(string str, string key)
        {
            byte[] bytesKey = UConvert.strToBytes(key);
            return decryptStrToStr(str, bytesKey);
        }
    }
}
