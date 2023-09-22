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
using DraegerConsole.Exceptions;
using System;

namespace DraegerConsole
{

    public class RequestManagerByTime
    {
        private System.Timers.Timer? timer;
        private AppConfiguration? appConfig;           
        private ITimestampUpdater timestampUpdater;
        private TimestampHistoryManager historyManager;
        private DraegerJson.ParameterHistory parameterHistory;
        private IJsonModifier modifier;
        private MessageWriter messageWriter;
        private IHospitalProvider? hospitalProvider = null;
        private IConverterJson converterJson;

        public RequestManagerByTime(
            ITimestampUpdater timestampUpdater, 
            TimestampHistoryManager historyManager,
            AppConfiguration appConfig,
            IJsonModifier modifier,
            IConverterJson converterJson
        )
        {
            this.appConfig = appConfig;
            this.timestampUpdater = timestampUpdater;
            this.historyManager = historyManager;            
            this.parameterHistory = new DraegerJson.ParameterHistory();
            this.modifier = modifier;
            this.messageWriter = new MessageWriter(timestampUpdater);
            this.converterJson = converterJson;
        }
        private void SetTimer()
        {
            timer = new System.Timers.Timer(
                appConfig!.RequestsIntervalInSeconds*1000
            );
            timer.Elapsed += BuildJsonHandler; 
            timer.AutoReset = true;
            timer.Enabled = true;            
        }     
        private void BuildJsonHandler(object? sender, ElapsedEventArgs e)
        {
            BuildJson();
        }            
        private void BuildJson(bool useAdmissionDateTime=false)
        {
            CreateHospitalProvider(useAdmissionDateTime,GetHistoryTimestamp());            
            messageWriter.WriteProcessMessage();            
            PrepareAndProcessJson();
            UpdateHistory();
            timestampUpdater.UpdateTimestamps();
            messageWriter.WriteOKMessage();            
        }

        private DateTime? GetHistoryTimestamp()
        {
            if (historyManager.History!.Units.Count == 0)
                return null;
            return historyManager.History.Units[historyManager.History.Units.Count-1].To;
        }

        private void UpdateHistory()
        {
            parameterHistory.RemoveOld(CreateCutoffDateTime());
            historyManager.History!.Units.Add(
                new TimestampHistoryUnit
                {
                    From = timestampUpdater.PastTimestamp,
                    To = timestampUpdater.CurrentTimestamp
                }
            );
            historyManager.Save();            
        }
        private DateTime CreateCutoffDateTime()
        {
            return timestampUpdater.CurrentTimestamp.AddMinutes(
                -appConfig!.HistoryTimeInMinutes
            );
        }
        private void PrepareAndProcessJson()
        {
            foreach (var pj in CreateJsons())
            {
                IJsonProcessor jsonProcessor = CreateJsonProcessor(pj);                
                jsonProcessor.ProcessJson(modifier.Modify(pj.Json));
            }
        }
        private IJsonProcessor CreateJsonProcessor(PatientJson pj)
        {
            return new FileJsonProcessor(
                appConfig!.PathToJsonFiles +
                "/patNr_" +
                pj.PatientId +
                "_AufnahmeNr_" +
                pj.AufnahmeNr +
                "_stamp_" +
                timestampUpdater.CurrentTimestamp.ToString("yyyy.MM.dd_HH.mm.ss") +
                ".json"
            );
        }
        private List<PatientJson> CreateJsons()
        {
            Hospital? hospital = hospitalProvider!.GetHospital();
            if (hospital == null) return new List<PatientJson>();           
            return converterJson.Convert(hospital);
        }
        private void CreateHospitalProvider(bool useAdmissionDateTime, DateTime? historyTimestamp)
        {
            hospitalProvider = new DraegerHospitalProvider(
                appConfig!.Certificate,
                appConfig.CertificateFilePassword,
                appConfig.ClappId,
                appConfig.ServerHostName,
                appConfig.ServerPort,
                appConfig.DomainId,
                Enum.Parse<StoreLocation>(appConfig.StoreLocation),
                Enum.Parse<StoreName>(appConfig.StoreName),
                timestampUpdater.PastTimestamp,
                timestampUpdater.CurrentTimestamp,
                historyTimestamp,
                useAdmissionDateTime
            );
        }
        public void StartRequests()
        {
            messageWriter.WriteStartMessage();
            BuildJson(true);
            SetTimer();
            WaitForStop();
            DestroyTimer();
        }
            
        private void DestroyTimer()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
        }
        private static void WaitForStop()
        {
            do
            {
                string? userInput = Console.ReadLine();
                if (userInput != null && userInput.ToLower().Equals("exit"))
                    break;
            }
            while (true);
        }
    }
}
