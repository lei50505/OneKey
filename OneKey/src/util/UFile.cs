using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OneKey.src.util
{
    class UFile
    {
        private static string fileName = "config.txt";
        private static string filePath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + fileName;
        private static Dictionary<string, string> titleKeys = new Dictionary<string, string>();
        //加密后
        private static string[] lines = new string[0];
        public static void createFile()
        {
            StreamWriter sw = new StreamWriter(filePath, true, Encoding.UTF8);
            sw.Close();
        }
        public static bool existsFile()
        {
            return File.Exists(filePath);
        }
        private static void readAllLines()
        {
            lines = File.ReadAllLines(filePath, Encoding.UTF8);
        }
        private static void writeAllLines()
        {
            File.WriteAllLines(filePath, lines, Encoding.UTF8);
        }
        public static void addContent(string[] contents)
        {
            readAllLines();
            //标题，匹配字符串，多行内容
            string[] newLines = new string[lines.Length + contents.Length + 1];
            for (int i = 0; i < lines.Length; i++)
            {
                newLines[i] = lines[i];
            }
            newLines[lines.Length] = string.Empty;
            for (int i = 0; i < contents.Length; i++)
            {
                newLines[lines.Length + i + 1] = contents[i];
            }
            lines = newLines;
            writeAllLines();
            loadKeyTitles();
        }
        public static bool existsTitle(string title)
        {
            readAllLines();
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Equals(string.Empty) && i + 1 < lines.Length&&!lines[i+1].Equals(string.Empty))
                {
                    if (title.Equals(lines[i + 1]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static string[] getByTitle(string title)
        {
            readAllLines();
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Equals(string.Empty) && i + 1 < lines.Length && !lines[i + 1].Equals(string.Empty))
                {
                    if (title.Equals(lines[i + 1]))
                    {
                        List<string> contentList = new List<string>();
                        for (int j = i + 1; j < lines.Length; j++)
                        {
                            if (lines[j].Equals(string.Empty))
                            {
                                break;
                            }
                            contentList.Add(lines[j]);
                        }
                        return contentList.ToArray<string>();
                    }
                }
            }
            return new string[0];
        }
        public static bool validContent(string[] content)
        {
            if (content==null||content.Length<3)
            {
                return false;
            }
            for (int i = 0; i < content.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(content[i]))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool existsPsw()
        {
            readAllLines();
            if (0 < lines.Length && !string.IsNullOrWhiteSpace(lines[0]))
            {
                return true;
            }
            return false;
        }
        public static void createPsw(string psw)
        {
            readAllLines();
            lines = new string[] { psw };
            writeAllLines();
        }
        private static void changePsw(string psw)
        {
            readAllLines();
            lines[0] = psw;
            writeAllLines();
        }
        public static bool validPsw(string psw)
        {
            readAllLines();
            return psw.Equals(lines[0]);
        }
        private static void loadKeyTitles()
        {
            readAllLines();
            titleKeys = new Dictionary<string, string>();
            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])&&i+2<lines.Length)
                {
                    titleKeys.Add(lines[i + 1], lines[i + 2]);
                }
            }
        }
        public static string[] getTitlesByKey(string searchKey)
        {
            if (string.IsNullOrWhiteSpace(searchKey))
            {
                return getAllTitles();
            }
            string key = searchKey.ToUpper();
            loadKeyTitles();
            if (string.IsNullOrWhiteSpace(key))
            {
                return new string[0];
            }
            List<string> titleList = new List<string>();
            foreach(KeyValuePair<string,string> kv in titleKeys)
            {
                if (kv.Value.Contains(key))
                {
                    titleList.Add(kv.Key);
                }
            }
            return titleList.ToArray<string>();
        }
        public static string[] getAllTitles()
        {
            loadKeyTitles();
            return titleKeys.Keys.ToArray<string>();
        }
        public static void encryptAllContentAndChgPsw(string oldPsw, string newPsw)
        {
            if (string.IsNullOrWhiteSpace(oldPsw) || string.IsNullOrWhiteSpace(newPsw))
            {
                return;
            }
            if (oldPsw.Equals(newPsw))
            {
                return;
            }
            readAllLines();
            string md5OldPsw = UMD5.strToSaltBase64Str(oldPsw);
            if(!md5OldPsw.Equals(lines[0])){
                return;
            }
            string md5NewPsw = UMD5.strToSaltBase64Str(newPsw);
            lines[0] = md5NewPsw;
            for (int i = 0; i < lines.Length; i++)
            {
                if (string.Empty.Equals(lines[i]))
                {
                    for (int j = i+3; j < lines.Length; j++)
                    {
                        if (lines[j].Equals(string.Empty))
                        {
                            break;
                        }
                        string oldLine = lines[j];
                        string decryptLine = UAES.decryptStrToStr(oldLine, oldPsw);
                        string newLine = UAES.encryptStrToStr(decryptLine, newPsw);
                        lines[j] = newLine;
                    }
                }
            }
            writeAllLines();
        }
        public static void deleteByTitle(string title)
        {
            readAllLines();
            int n,m = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Equals(title))
                {
                    n = i;
                    for (int j = i; j < lines.Length; j++)
                    {
                        if (lines[j].Equals(string.Empty))
                        {
                            m = j;
                            break;
                        }
                        m=lines.Length;
                    }
                    string[] newLines = new string[lines.Length - m + n - 1];
                    for (int ii = 0; ii < n - 1; ii++)
                    {
                        newLines[ii] = lines[ii];
                    }
                    for (int k = n - 1; k < newLines.Length; k++)
                    {
                        newLines[k] = lines[m - n + 1 + k];
                    }
                    lines = newLines;
                    writeAllLines();
                    return;
                }
            }
        }
        public static void addDecryptContent(string[] decryptContent,string psw)
        {
            if (existsTitle(decryptContent[0]))
            {
                return;
            }
            string[] content = new string[decryptContent.Length + 1];
            content[0] = decryptContent[0];
            content[1] = PinYinUtils.getSearchStr(decryptContent[0]);
            for (int i = 2; i < content.Length; i++)
            {
                string s = decryptContent[i - 1];
                string es = UAES.encryptStrToStr(s, psw);
                content[i] = es;
            }
            UFile.addContent(content);
        }
        public static void importFile(string importFilePath,string psw)
        {
            string[] decryptImportLines = File.ReadAllLines(importFilePath, Encoding.UTF8);
            int decryptImportLinesLength = decryptImportLines.Length;
            List<string> decryptContent = new List<string>();
            for (int i = 0; i < decryptImportLinesLength; i++)
            {
                if (!string.IsNullOrWhiteSpace(decryptImportLines[i])) 
                {
                    decryptContent.Add(decryptImportLines[i]);
                }
                else
                {
                    if (decryptContent.Count >= 2)
                    {
                        string[] decryptContentArray = decryptContent.ToArray<String>();
                        int tmp = 2;
                        string title = decryptContentArray[0];
                        while (existsTitle(decryptContentArray[0]))
                        {
                            decryptContentArray[0] = title + "_" + tmp.ToString();
                            tmp++;
                        }
                        addDecryptContent(decryptContentArray, psw);
                    }
                    decryptContent = new List<string>();
                }
            }
            if (decryptContent.Count >= 2)
            {
                string[] decryptContentArray = decryptContent.ToArray<String>();
                int tmp = 2;
                string title = decryptContentArray[0];
                while (existsTitle(decryptContentArray[0]))
                {
                    decryptContentArray[0] =title+"_"+ tmp.ToString();
                    tmp++;
                }
                addDecryptContent(decryptContentArray, psw);
            }
        }
        public static void exportFile(string exportFilePath,string psw){
            List<string> decryptLines = new List<string>();
            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    if (i != 1)
                    {
                        decryptLines.Add(string.Empty);
                    }
                    if(i+1<lines.Length){
                        decryptLines.Add(lines[i+1]);
                    }
                    for (int j = i + 3; j < lines.Length; j++)
                    {
                        if (string.IsNullOrWhiteSpace(lines[j]))
                        {
                            break;
                        }
                        decryptLines.Add(UAES.decryptStrToStr(lines[j], psw));
                    }
                }
            }
            File.WriteAllLines(exportFilePath, decryptLines.ToArray<string>(), Encoding.UTF8);
        }

    }
}
