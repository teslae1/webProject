using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbsongPro
{
    public static class TestTools
    {
        public static void print(object o)
        {
            System.Diagnostics.Debug.Write("");
            System.Diagnostics.Debug.WriteLine("TestTools print: ");
            System.Diagnostics.Debug.WriteLine(o);
            System.Diagnostics.Debug.Write("");
        }
    }
}
