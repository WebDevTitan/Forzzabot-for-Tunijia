using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Forzza
{
    public delegate void WriteLogDelegate(string strLog);
    internal class WriteLog
    {
        public static WriteLogDelegate WrittingLog = null;
        public static bool End = false;
        public static CookieContainer cookieContainer = null;
        public static string GTM = "";
    }
}

