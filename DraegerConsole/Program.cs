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
using System.Runtime.InteropServices;
using Hl7.Fhir.Specification;

class Program
{
    private static AppConfiguration? appConfig;
    private static TimestampHistoryManager? historyManager;
    private static ITimestampUpdater? timestampUpdater;    
    private static void ProcessError(Exception ex)
    {
        Console.WriteLine(ex.ToString());
        System.Environment.Exit(1);
    }
    static int Main(string[] args)
    {       
        try
        {
            
            ReadConfiguration();
            if (!CheckConfigs())
                return 0;
            
            
            if (args.Length == 0|| args.Length == 2 && (args[0]=="-m" || args[0] == "--mode") && args[1] == "interval")
            {
                timestampUpdater = new IntervalTimestampUpdater(appConfig!.TimestampsIntervalInSeconds, appConfig!.FirstTimestamp, DateTime.Now);
            }
            else if(args.Length==3&& (args[0] == "-m" || args[0] == "--mode") && args[1] == "array")
            {
                timestampUpdater = LoadDateTimeArrayFromFile(args[2]);
            }
            else if (args.Length == 3 && (args[0] == "-m" || args[0] == "--mode") && args[1] == "now")
            {
                timestampUpdater = new NowTimestampUpdater(appConfig!.TimestampsOffsetInSeconds, appConfig.FirstTimestamp);
            }
            else
            {
                PrintHelp();
                return 0;
            }
            historyManager = BuildHistoryManager();
            
            RequestManagerByTime request = new RequestManagerByTime(
                timestampUpdater!, 
                historyManager,
                appConfig!,
                new PerformedPeriodToPerformedDateTime()
            );
            request.StartRequests();
        }
        catch (Exception ex) 
        {
            ProcessError(ex);
        }
        return 0;      
    }
       
    public static bool CheckConfigs()
    {
        return CheckRequestIntervalAndTimestampIntervalEquality()&&
               CheckStoreNameAndStoreLocationValues();
    }

    private static bool CheckStoreNameAndStoreLocationValues()
    {
        StoreLocation storeLocation;
        if (!Enum.TryParse(appConfig!.StoreLocation, out storeLocation))
        {
            LogManager.Log($"[CONFIG ERROR]: {appConfig!.StoreLocation} is invalid for StoreLocation!");
            return false;
        }
        StoreName storeName;
        if(!Enum.TryParse(appConfig!.StoreName, out storeName))
        {
            LogManager.Log($"[CONFIG ERROR]: {appConfig!.StoreName} is invalid for StoreName!");
            return false;
        }
        return true;
    }

    private static bool CheckRequestIntervalAndTimestampIntervalEquality()
    {
        if (appConfig!.RequestsIntervalInSeconds == appConfig.TimestampsIntervalInSeconds)
        {
            return true;
        }
        LogManager.Log("RequestsIntervalInSeconds and TimestampsIntervalInSeconds are different. Would you like to continue? Y/n");
        if (Console.ReadLine()!.ToLower().Trim() == "n")
            return false;
        return true;
    }

    private static ITimestampUpdater? LoadDateTimeArrayFromFile(string filename)
    {
        List<DateTime> dateTimestamps = new List<DateTime>();
        using(StreamReader reader = new StreamReader(new FileStream(filename, FileMode.Open)))
        {
            while (!reader.EndOfStream)
            {
                DateTime dt;
                if( DateTime.TryParse(reader.ReadLine(), out dt))
                {
                    dateTimestamps.Add(dt);
                }
            }
        }
        return new FromArrayTimestampUpdater(dateTimestamps.ToArray());
    }
    private static void PrintHelp()
    {
        Console.WriteLine("usage: DragerConsole [-h | --help | -m now | --mode now | -m array <file> |\n       --mode array <file> | -m interval | --mode interval]");
    }
    private static TimestampHistoryManager BuildHistoryManager()
    {
        TimestampHistoryManager thm = new TimestampHistoryManager(appConfig!.PathToHistory+"/history.json");
        if (File.Exists(appConfig!.PathToHistory + "/history.json"))        
            thm.Load();        
       // else 
           // thm.Initialize(appConfig!.FirstTimestamp);       
        return thm;
    } 
    private static void ReadConfiguration()
    {
        if (!File.Exists("config/appConfig.json"))
        {
            LogManager.Log
                ("Configuration File not found");
            System.Environment.Exit(1);
        }
        appConfig = JsonSerializer.Deserialize<AppConfiguration>(File.ReadAllText("config/appConfig.json"));
        if (appConfig == null)
            throw new ReadConfigException("Configuration creation failed");      
    }
}

