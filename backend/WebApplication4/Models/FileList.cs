using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class FileList
    {
        public int Network_SID { get; set; }

        public DateTime DateTime_Key { get; set; }
        public string NEID { get; set; }
        public string Object { get; set; }
        public DateTime Time { get; set; }
        public int Interval { get; set; }
        public string Direction { get; set; }
        public string NeAlias { get; set; }
        public string NeType { get; set; }
        public string RxLevelBelowTS1 { get; set; }
        public string RxLevelBelowTS2 { get; set; }
        public float MinRxLevel { get; set; }
        public float MaxRxLevel { get; set; }
        public string TxLevelAboveTS1 { get; set; }
        public float MinTxLevel { get; set; }
        public float MaxTxLevel { get; set; }
        public string FailureDescription { get; set; }
        public string Link { get; set; }
        public string TID { get; set; }
        public string FARENDTID { get; set; }
        public string Slot { get; set; }
        public string Port { get; set; }
        public string FileName { get; set; }

    }
}
