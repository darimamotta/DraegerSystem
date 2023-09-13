using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace DraegerConsole
{
    public class AppConfiguration
    {
        public string Certificate { get; set; } = string.Empty;
        public string CertificateFilePassword { get; set; } = string.Empty;
        public string ClappId { get; set; } = string.Empty;
        public string ServerHostName { get; set; } = string.Empty;
        public string DomainId { get; set; } = string.Empty;
        public int ServerPort { get; set; }
        public string StoreLocation { get; set; } = string.Empty;
        public string StoreName { get; set; } = string.Empty;  
        public int TimestampsOffsetInSeconds { get; set; }
        public string PathToJsonFiles { get; set; } = string.Empty;
        public string PathToHistory { get;set; } = string.Empty;
        public string LogFileName { get; set; } = string.Empty;
        public int HistoryTimeInMinutes { get; set; }
        public DateTime FirstTimestamp { get; set; }
        public int TimestampsIntervalInSeconds { get; set; }
        public int RequestsIntervalInSeconds { get; set; }


    }
}
