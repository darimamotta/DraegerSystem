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
        private ConnectionConfiguration? connectionConfiguration;
        //private GlobalConfiguration? globalConfiguration;
        private int delay;
        private ITimestampUpdater timestampUpdater;

        //temporary timestamps 
      //private DateTime[] temporaryDateTimes = new DateTime[]
      //{
      //    new DateTime(2023,6,8,12,30,0,DateTimeKind.Local),
      //    new DateTime(2023,6,8,14,30,0,DateTimeKind.Local),
      //    new DateTime(2023,6,8,16,30,0,DateTimeKind.Local),
      //    new DateTime(2023,6,8,18,30,0,DateTimeKind.Local),
      //    new DateTime(2023,6,8,13,10,0,DateTimeKind.Local),
      //    new DateTime(2023,6,8,13,20,0,DateTimeKind.Local),
      //    new DateTime(2023,6,8,13,30,0,DateTimeKind.Local),
      //    new DateTime(2023,6,8,13,40,0,DateTimeKind.Local),
      //    new DateTime(2023,6,8,13,50,0,DateTimeKind.Local),
      //    new DateTime(2023,6,8,14,00,0,DateTimeKind.Local),
      //    new DateTime(2023,6,8,14,10,0,DateTimeKind.Local),
      //    new DateTime(2023,6,8,14,20,0,DateTimeKind.Local),
      //    new DateTime(2023,6,8,14,30,0,DateTimeKind.Local),
      //    new DateTime(2023,6,8,14,40,0,DateTimeKind.Local),
      //    new DateTime(2023,6,8,14,50,0,DateTimeKind.Local),
      //    new DateTime(2023,6,8,15,00,0,DateTimeKind.Local),
      //    new DateTime(2023,5,10,9,30,0,DateTimeKind.Local),
      //    new DateTime(2023,5,10,9,40,0,DateTimeKind.Local),
      //    new DateTime(2023,5,10,9,50,0,DateTimeKind.Local),
      //    new DateTime(2023,5,10,10,0,0,DateTimeKind.Local),
      //    new DateTime(2023,5,10,10,10,0,DateTimeKind.Local),
      //    new DateTime(2023,5,10,10,20,0,DateTimeKind.Local),
      //    new DateTime(2023,5,10,10,30,0,DateTimeKind.Local),
      //    new DateTime(2023,5,10,10,40,0,DateTimeKind.Local),
      //    new DateTime(2023,5,10,10,50,0,DateTimeKind.Local),
      //    new DateTime(2023,5,10,11,0,0,DateTimeKind.Local)
      //
      //
      //};

        public RequestManagerByTime(int delay, ITimestampUpdater timestampUpdater)
        {
            this.delay = delay;     
            this.timestampUpdater = timestampUpdater;
        }
        private void SetTimer()
        {
            timer = new System.Timers.Timer(delay);
            timer.Elapsed += BuildJsonHandler;
            timer.AutoReset = true;
            timer.Enabled = true;
            
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
            SetUpTimestamps();
            hospitalProvider = new DraegerHospitalProvider(
                connectionConfiguration!.Certificate,
                connectionConfiguration.CertificateFilePassword,
                connectionConfiguration.ClappId,
                connectionConfiguration.ServerHostName,
                connectionConfiguration.ServerPort,
                connectionConfiguration.DomainId,
                timestampUpdater.PastTimestamp,
                timestampUpdater.CurrentTimestamp
            );


            IJsonProcessor jsonProcessor = new FileJsonProcessor("data/stamp_" + timestampUpdater.CurrentTimestamp.ToString("yyyy.MM.dd_HH.mm.ss") + ".json");
            ConverterJson converterJson = new ConverterJson();
            Console.WriteLine("Process from {0} to {1}...", timestampUpdater.PastTimestamp.ToString("yyyy.MM.dd_HH.mm.ss"), timestampUpdater.CurrentTimestamp.ToString("yyyy.MM.dd_HH.mm.ss"));
            Hospital? hospital = hospitalProvider!.GetHospital();
            if (hospital != null)
            {
                hospital.Timestamp = timestampUpdater.CurrentTimestamp;
                jsonProcessor.ProcessJson(converterJson.Convert(hospital));
            }
            Console.WriteLine("OK. Enter 'Exit' for Stop ");
            File.WriteAllText("lastTimestamp", timestampUpdater.CurrentTimestamp.ToString());

        }

        //private int temporaryIndex =0;
        private void SetUpTimestamps()
        {
            timestampUpdater.UpdateTimestamps();
            //actual timestamps
            //pastTimestamp = currentTimestamp;
            //currentTimestamp = DateTime.Now.AddSeconds(globalConfiguration!.TimestampsOffsetInSeconds);
            //temporary timestamps 
            //pastTimestamp = temporaryDateTimes[temporaryIndex];
            //currentTimestamp = temporaryDateTimes[temporaryIndex+1];
            //temporaryIndex++;
        }

        private static ConnectionConfiguration? ReadConfiguration()
        {
            if (!File.Exists("config/connectionConfig.json"))
            {
                Console.WriteLine("Configuration File not found");
                System.Environment.Exit(1);
            }
            return
            JsonSerializer.Deserialize<ConnectionConfiguration>(File.ReadAllText("config/connectionConfig.json"));
        }
        public void StartRequests()
        {
                     
            connectionConfiguration = ReadConfiguration();
            if (connectionConfiguration == null)
                throw new ReadConfigException("Configuration creation failed");
            SetStartTimestamp();
            BuildJson();
            SetTimer();            
            Console.WriteLine("Application started at " + DateTime.Now);
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
        private void SetStartTimestamp()
        {
            if (File.Exists("lastTimestamp"))
                ReadTimestampFromFile();
            else
                ReadTimestampFromConsole();
        }
        private void ReadTimestampFromFile()
        {
            try
            {
                timestampUpdater.CurrentTimestamp = DateTime.Parse(File.ReadAllText("lastTimestamp"));
            }
            catch (Exception e)
            {
                throw new FileFormatException("Error reading from file 'lastTimestamp'");
            }
        }

        private void ReadTimestampFromConsole()
        {
            Console.WriteLine("Enter initial value of timestamp (empty line for {0})", defaultStartTimestamp);
            string? input = Console.ReadLine();
            if (input == null)
                throw new UnknownErrorException("Error reading from console");
            if (input.Trim().Length == 0)
            {
                currentTimestamp = defaultStartTimestamp;
                return;
            }
            try
            {
                currentTimestamp = DateTime.Parse(input);
            }
            catch (Exception e)
            {
                throw new InputFormatException("Incorrect format of Timestamp");
            }

        }

    }
}
