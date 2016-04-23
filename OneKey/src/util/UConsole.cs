using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneKey.src.util
{
    public static class UConsole
    {
        public static void writeArray(byte[] objs)
        {
            StringBuilder sb = new StringBuilder("{");
            for (int i = 0; i < objs.Length-1;i++ )
            {
                sb.Append(objs[i].ToString()).Append(", ");
            }
            sb.Append(objs[objs.Length - 1].ToString()).Append("}");
            Console.WriteLine(sb.ToString());
        }
    }
}
