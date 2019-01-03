using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorV2
{
    class DeviceModel
    {
        public class deviceinfo
        {
            public string windowsversion { get; set; }
            public string cpu { get; set; }
            public string availablememory { get; set; }
            public string exipaddress { get; set; }
            public string antivirus { get; set; }
            public string deviceuptime { get; set; }
            public string lastupdated { get; set; }
        }

        public class Harddrivespace
        {
            public string totalspace { get; set; }
            public string freespace { get; set; }
            public string usedspace { get; set; }
        }

        public class Devicestatus
        {
            public string cpu { get; set; }
            public string memory { get; set; }
            public string network { get; set; }
        }

        public class RootObject
        {
            public deviceinfo deviceinfo { get; set; }
            public Harddrivespace harddrivespace { get; set; }
            public Devicestatus devicestatus { get; set; }
            public List<object> ocslogfile { get; set; }
            public string _id { get; set; }
            public string pcname { get; set; }
            public string ipaddress { get; set; }
            public string macaddress { get; set; }
            public string company { get; set; }
            public string site { get; set; }
            public string owner { get; set; }
            public int __v { get; set; }
            public string status { get; set; }
        }

        public class Auth
        {
            public string message { get; set; }
            public string token { get; set; }
        }
    }
}
