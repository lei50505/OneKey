using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.International.Converters.PinYinConverter;
using System.Text.RegularExpressions;
namespace OneKey.src.util
{
    public static class PinYinUtils
    {
       
        private static string[] charToFull(char ch, ref string error)
        {
            try
            {
                HashSet<string> purePinYinSet = new HashSet<string>();
                ChineseChar chChar = new ChineseChar(ch);
                for (int i = 0; i < chChar.PinyinCount; i++)
                {
                    string tonePinYin = chChar.Pinyins[i].ToString();
                    string purePinYin = tonePinYin.Substring(0, tonePinYin.Length - 1);
                    purePinYinSet.Add(purePinYin);
                }
                return purePinYinSet.ToArray<string>();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return null;
        }
        private static string[] charToFirst(char ch, ref string error)
        {
            if (!Regex.IsMatch(ch.ToString(), @"[\u4e00-\u9fa5]"))
            {
                error = "Not a Chinese Char";
                return new string[0];
            }
            try
            {
                HashSet<string> purePinYinSet = new HashSet<string>();
                ChineseChar chChar = new ChineseChar(ch);
                for (int i = 0; i < chChar.PinyinCount; i++)
                {
                    string tonePinYin = chChar.Pinyins[i].ToString();
                    string purePinYin = tonePinYin.Substring(0, 1);
                    purePinYinSet.Add(purePinYin);
                }
                return purePinYinSet.ToArray<string>();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return null;
        }
        private static string[][] getStrsArray(string strLine)
        {
            List<string[]> strsList = new List<string[]>();
            
            foreach (char ch in strLine)
            {
                string error = null;
                string[] strs = charToFull(ch,ref error);
                if (error == null)
                {
                    strsList.Add(strs);
                }
                else
                {
                    char cc = char.ToUpper(ch);
                    string ss = cc.ToString();
                    if (!string.IsNullOrWhiteSpace(ss))
                    {
                        strsList.Add(new string[] { ss });
                    }
                }
            }
            return strsList.ToArray<string[]>();
        }
        private static string[][] getStrsArrayFirst(string strLine)
        {
            List<string[]> strsList = new List<string[]>();

            foreach (char ch in strLine)
            {
                string error = null;
                string[] strs = charToFirst(ch, ref error);
                if (error == null)
                {
                    strsList.Add(strs);
                }
                else
                {
                    char cc = char.ToUpper(ch);
                    string ss = cc.ToString();
                    if (!string.IsNullOrWhiteSpace(ss))
                    {
                        strsList.Add(new string[] { ss });
                    }
                }
            }
            return strsList.ToArray<string[]>();
        }
        private static void mixStrsArray(ref string[][] strsArray, ref string[] strs, int index)
        {
            if (index >= strsArray.Length)
            {
                return;
            }
            if (index == 0)
            {
                int l = strsArray[0].Length;
                strs = new string[l];
                for (int i = 0; i < l; i++)
                {
                    strs[i] = strsArray[0][i];
                }
                index++;
                if (index >= strsArray.Length)
                {
                    return;
                }
            }
            int strsLength = strsArray[index].Length;
            string[] newStrs = new string[strs.Length * strsLength];
            int newIndex = 0;
            for (int i = 0; i < strsLength; i++)
            {
                for (int j = 0; j < strs.Length; j++)
                {
                    newStrs[newIndex] = strs[j] + strsArray[index][i];
                    newIndex++;
                }
            }
            strs = newStrs;
            mixStrsArray(ref strsArray, ref strs, ++index);
        }
        public static string[] strToFull(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return new string[0];
            }
            string[][] strsArray = getStrsArray(str);
            string[] strs = null;
            mixStrsArray(ref strsArray, ref strs, 0);
            return strs;
        }
        public static string[] strToFirst(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return new string[0];
            }
            string[][] strsArray = getStrsArrayFirst(str);
            string[] strs = null;
            mixStrsArray(ref strsArray, ref strs, 0);
            return strs;
        }
        public static string getSearchStr(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }
            string[] firstStrs = strToFirst(str);
            string[] fullStrs = strToFull(str);
            StringBuilder sb = new StringBuilder();
            foreach (string s in firstStrs)
            {
                sb.Append(s).Append('@');
            }
            foreach (string s in fullStrs)
            {
                sb.Append(s).Append('@');
            }
            sb.Append(str);
            return sb.ToString();
        }
    }
} 
