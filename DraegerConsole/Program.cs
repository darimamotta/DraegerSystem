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

class Program
{
    private static System.Timers.Timer? timer;
    private static ConnectionConfiguration? configuration;
    private static void SetTimer()
    {
        timer = new System.Timers.Timer(10000);
        //event is called by timer, we can subscribe to this event trigger
        //BuildJson is a handler like a  "trigger" for schedule
        //Elapsed is event to which we subscribe by method BuildJson
        timer.Elapsed += BuildJson;
        //when trigger occured it starts an operation
        timer.AutoReset = true;
        timer.Enabled = true;
    }
    private static IHospitalProvider? hospitalProvider = null;
    private static DateTime pastTimestamp;
    private static DateTime currentTimestamp;
    private static readonly DateTime defaultStartTimestamp = new DateTime(1990, 1, 1, 0, 0, 0);

    //Buildjson function is called every minute
    private static void BuildJson(object? source, ElapsedEventArgs e)  
    {
        //IObjectsForJsonProvider jsonProvider = new DraegerJsonProvider
        //(
        //    "361609fb-09a8-47c8-9bc3-d2b4db36eb2d.pfx",
        //    "fb6b456b-641f-4a3f-b31b-938ec2149824",
        //    "CLAPP1",
        //    "clappathon",
        //    "CLTDEMOCLAPP01",
        //    "clappathon"
        //);
        pastTimestamp = currentTimestamp;
        currentTimestamp = DateTime.Now;
        hospitalProvider =new DraegerHospitalProvider(
            configuration.Certificate,
            configuration.CertificateFilePassword,
            configuration.ClappId,
            configuration.ServerHostName,
            configuration.ServerPort,
            configuration.DomainId,
            pastTimestamp,
            currentTimestamp
        );     

        //create a json file name for specific time
        IJsonProcessor jsonProcessor = new FileJsonProcessor("data/stamp_" + currentTimestamp.ToString("yyyy.MM.dd_HH.mm.ss") + ".json");
        //from object Root create a txt format of json
        ConverterJson converterJson = new ConverterJson();
        //find an error in json 
        try
        {
            Console.WriteLine("Process from {0} to {1}...", pastTimestamp.ToString("yyyy.MM.dd_HH.mm.ss"), currentTimestamp.ToString("yyyy.MM.dd_HH.mm.ss"));
            Hospital? hospital = hospitalProvider!.GetHospital();
            if(hospital != null )
            {
                hospital.Timestamp = currentTimestamp;
                jsonProcessor.ProcessJson(converterJson.Convert(hospital));
            }
            Console.WriteLine("OK. Enter 'Exit' for Stop ");
            File.WriteAllText("lastTimestamp", currentTimestamp.ToString());
        }
        //process this error in block catch  
        catch (Exception ex) 
        {
            ProcessError(ex);
        }
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

    //function for error  processng where ex is error value 
    private static void ProcessError(Exception ex)
    {
        Console.WriteLine(ex.ToString());
        System.Environment.Exit(1);
    }
    //launch app and show time of start
    static int Main(string[] args)
    {
        // CLAPP-Verbindungskonfiguration erstellen
        CLAPPConfiguration config = new CLAPPConfiguration()
        {
            Certificate = "C:\\DraegerApp\\DraegerSystem\\DraegerConsole\\certificate\\92e4a881-3157-4581-8926-69d54e44db6a.pfx", // Zertifikat: Zertifikatname (Windows Certificate Store) oder Pfad zur Zertifikatdatei (PFX)
            CertificateFilePassword = "434afeea-c27f-42b9-a10d-e5fe8e69131b".ToSecureString(), // Passwort zum Öffnen des PFX
            CLAPPID = "Clapp1", // CLAPP ID
            DomainID = "clappathon", // Domain ID
            ServerHostname = "SRVDEMOICM05V.DRAEGER.DEMO.CH",
            ServerPort = 25000
           
            //    "361609fb-09a8-47c8-9bc3-d2b4db36eb2d.pfx",
            //    "fb6b456b-641f-4a3f-b31b-938ec2149824",
            //    "CLAPP1",
            //    "clappathon",
            
            //Client name – CLTDEMOCLAPP01
            //DSI name – clappathon
            
        };

        configuration = ReadConfiguration();
        if (configuration == null)
        {
            Console.WriteLine("Configuration Creation failed");
            System.Environment.Exit(1);
        }
        SetTimer();
        SetStartTimestamp();
        Console.WriteLine("Application started at "+ DateTime.Now);
        //enter exit for stop 
        Console.WriteLine("Enter 'Exit' for stop application");
        //a loop for execution while user not entering stop
        do 
        {
            string? userInput = Console.ReadLine();
            if (userInput != null &&  userInput.ToLower().Equals("exit"))
                break;
            
        } 
        while (true);
        if (timer!=null)
        { 
            timer.Stop();
            timer.Dispose();
        
        }
       
        return 0;
      
    }

    private static void SetStartTimestamp()
    {
        if (File.Exists("lastTimestamp"))
            ReadTimestampFromFile();
        else
            ReadTimestampFromConsole();
    }

    private static void ReadTimestampFromFile()
    {
        try
        {
            currentTimestamp = DateTime.Parse(File.ReadAllText("lastTimestamp"));
        }
        catch (Exception e)
        {
            Console.WriteLine("Error reading from file 'lastTimestamp'");
            ProcessError(e);
        }
    }

    private static void ReadTimestampFromConsole()
    {
        Console.WriteLine("Enter initial value of timestamp (empty line for {0})", defaultStartTimestamp);
        string? input = Console.ReadLine();
        if (input == null)
        {
            Console.WriteLine("Error reading from console");
            System.Environment.Exit(1);
        }
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
            Console.WriteLine("Incorrect format of Timestamp");
            ProcessError(e);
        }

    }
}

