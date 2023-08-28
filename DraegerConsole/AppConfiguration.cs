using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int TimestampsOffsetInSeconds { get; set; }
        public string PathToJsonFiles { get; set; } = string.Empty;
        public string PathToHistory { get;set; } = string.Empty;
        public int HistoryTimeInMinutes { get; set; }
        public DateTime FirstTimestamp { get; set; }
        public int TimestampsIntervalInSeconds { get; set; }
        public DateTime StartIntervalTimestamp { get; set; } 
    }
}
