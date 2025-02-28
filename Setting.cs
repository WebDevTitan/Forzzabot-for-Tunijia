using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forzza
{
    public class Setting
    {
        private static Setting _instance = null;
        public static Setting Instance
        {
            get
            {
                if(_instance == null)   
                    _instance = new Setting();
                return _instance;
            }
        }

        public string username { get; set; }
        public string password { get; set; }
        public string domain { get; set; }
        public string license { get; set; }

        public string token { get; set; }
        public string marketID { get; set; }
        public string eventID { get; set; }
        public double stake { get; set; }

        public string SeverUrl { get; set; }

        

    }
}
