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

namespace DraegerConsole
{

    public class RequestManagerByTime
    {
        private System.Timers.Timer? timer;
        private ConnectionConfiguration? configuration;
        private int delay;

        public RequestManagerByTime(int delay)
        {
            this.delay = delay;
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
        private DateTime pastTimestamp;
        private DateTime currentTimestamp;
        private readonly DateTime defaultStartTimestamp = new DateTime(1990, 1, 1, 0, 0, 0);
        private void BuildJson()
        {
           
            pastTimestamp = currentTimestamp;
            currentTimestamp = DateTime.Now;
            hospitalProvider = new DraegerHospitalProvider(
                configuration!.Certificate,
                configuration.CertificateFilePassword,
                configuration.ClappId,
                configuration.ServerHostName,
                configuration.ServerPort,
                configuration.DomainId,
                pastTimestamp,
                currentTimestamp
            );

          
            IJsonProcessor jsonProcessor = new FileJsonProcessor("data/stamp_" + currentTimestamp.ToString("yyyy.MM.dd_HH.mm.ss") + ".json");
            ConverterJson converterJson = new ConverterJson();        
            Console.WriteLine("Process from {0} to {1}...", pastTimestamp.ToString("yyyy.MM.dd_HH.mm.ss"), currentTimestamp.ToString("yyyy.MM.dd_HH.mm.ss"));
            Hospital? hospital = hospitalProvider!.GetHospital();
            if (hospital != null)
            {
                hospital.Timestamp = currentTimestamp;
                jsonProcessor.ProcessJson(converterJson.Convert(hospital));
            }
            Console.WriteLine("OK. Enter 'Exit' for Stop ");
            File.WriteAllText("lastTimestamp", currentTimestamp.ToString());
                    
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
                     
            configuration = ReadConfiguration();
            if (configuration == null)
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
                currentTimestamp = DateTime.Parse(File.ReadAllText("lastTimestamp"));
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
