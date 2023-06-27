using Draeger.Pdms.Services.Extensions;
using Draeger.Pdms.Services.Json.Entities;
using Draeger.Pdms.Services.Json;
using DraegerConsole;
using DraegerJson;
using Draft_Draeger;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Timers;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Text.Json;
using DraegerConsole.exceptions;
using System;

namespace DraegerConsole
{

    public class RequestManagerByTime
    {
        private System.Timers.Timer? timer;
        private AppConfiguration? appConfig;
        //private GlobalConfiguration? globalConfiguration;      
        private ITimestampUpdater timestampUpdater;
        private TimestampHistoryManager historyManager;
        private DraegerJson.ParameterHistory parameterHistory;

        //temporary timestamps 
        //private DateTime[] temporaryDateTimes = 

        public RequestManagerByTime(
            ITimestampUpdater timestampUpdater, 
            TimestampHistoryManager historyManager,
            AppConfiguration appConfig
        )
        {
            this.appConfig = appConfig;
            this.timestampUpdater = timestampUpdater;
            this.historyManager = historyManager;            
            this.parameterHistory = new DraegerJson.ParameterHistory();
        }
        private void SetTimer()
        {
            timer = new System.Timers.Timer(appConfig!.DelayBetweenRequestsInMilliseconds);
            timer.Elapsed += BuildJsonHandler;
            timer.AutoReset = true;
            timer.Enabled = true;
            
        }

        private void SetUp()

        { 
            SetTimer();
            if(!Directory.Exists(appConfig!.PathToJsonFiles))
            {
                Directory.CreateDirectory(appConfig!.PathToJsonFiles);
            }
        }
        private void BuildJsonHandler(object? sender, ElapsedEventArgs e)
        {
            BuildJson();
        }

        private IHospitalProvider? hospitalProvider = null;
       //private DateTime pastTimestamp;
       //private DateTime currentTimestamp;
        private readonly DateTime defaultStartTimestamp = new DateTime(1990, 1, 1, 0, 0, 0);
        private void BuildJson()
        {
           
            hospitalProvider = new DraegerHospitalProvider(
                appConfig!.Certificate,
                appConfig.CertificateFilePassword,
                appConfig.ClappId,
                appConfig.ServerHostName,
                appConfig.ServerPort,
                appConfig.DomainId,
                timestampUpdater.PastTimestamp,
                timestampUpdater.CurrentTimestamp

            );


            string path = appConfig.PathToJsonFiles;
            ConverterJson converterJson = new ConverterJson(parameterHistory);
            Console.WriteLine("Process from {0} to {1}...", timestampUpdater.PastTimestamp.ToString("yyyy.MM.dd_HH.mm.ss"), timestampUpdater.CurrentTimestamp.ToString("yyyy.MM.dd_HH.mm.ss"));
            Hospital? hospital = hospitalProvider!.GetHospital();
            if (hospital != null)
            {
                hospital.Timestamp = timestampUpdater.CurrentTimestamp;
                               
                var jsons = converterJson.Convert(hospital);
                foreach (var pId in  jsons.Keys) 
                {
                    IJsonProcessor jsonProcessor = new FileJsonProcessor(path+"/patId_"+pId+"_stamp_" + timestampUpdater.CurrentTimestamp.ToString("yyyy.MM.dd_HH.mm.ss") + ".json");
                    jsonProcessor.ProcessJson(jsons[pId]);             
                }                
            }
            DateTime cutOffDate = timestampUpdater.CurrentTimestamp.AddMinutes(-appConfig.HistoryTimeInMinutes);
            parameterHistory.RemoveOld(cutOffDate);
            Console.WriteLine("OK. Enter 'Exit' for Stop ");
            historyManager.History!.Units.Add(
                new TimestampHistoryUnit 
                {
                    From = timestampUpdater.PastTimestamp, 
                    To =timestampUpdater.CurrentTimestamp
                }
            );
            historyManager.Save();
            SetUpTimestamps();

        }

        //private int temporaryIndex =0;
        private void SetUpTimestamps()
        {
            timestampUpdater.UpdateTimestamps();           
        }

       
        public void StartRequests()
        {
                     
           
            
            Console.WriteLine("Application started at " + DateTime.Now);
            BuildJson();
            SetUp();            
            
            Console.WriteLine("Enter 'Exit' for stop application");
            do
            {
                string? userInput = Console.ReadLine();
                if (userInput != null && userInput.ToLower().Equals("exit"))
                    break;

            }
            while (true);
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();

            }

        

        }
       
    }
}
