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
using System.Runtime.InteropServices;

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
            
            if (args.Length == 0|| args.Length == 2 && (args[0]=="-m" || args[0] == "--mode") && args[1] == "now")
            {
                timestampUpdater = new NowTimestampUpdater(appConfig!.TimestampsOffsetInSeconds, appConfig.FirstTimestamp);
            }
            else if(args.Length==3&& (args[0] == "-m" || args[0] == "--mode") && args[1] == "array")
            {
                timestampUpdater = LoadDateTimeArrayFromFile(args[2]);
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
        Console.WriteLine("usage: DragerConsole [-h | --help | -m now | --mode now | -m array <file> |\n       --mode array <file>]");
    }
    private static TimestampHistoryManager BuildHistoryManager()
    {
        TimestampHistoryManager thm = new TimestampHistoryManager(appConfig!.PathToHistory+"/history.json");
        if (File.Exists(appConfig!.PathToHistory + "/history.json"))        
            thm.Load();        
        else 
            thm.Initialize(ReadTimestampFromConsole());       
        return thm;
    }
    private static DateTime ReadTimestampFromConsole()
    {
        Console.WriteLine("Enter initial value of timestamp (empty line for {0})", TimestampHistoryManager.DefaultStartTimestamp );
        string? input = Console.ReadLine();
        if (input == null)
            throw new UnknownErrorException("Error reading from console");
        if (input.Trim().Length == 0)
        {
            
            return TimestampHistoryManager.DefaultStartTimestamp;
        }
        try
        {
            return DateTime.Parse(input);
        }
        catch (Exception e)
        {
            throw new InputFormatException("Incorrect format of Timestamp");
        }

    }

    private static ITimestampUpdater BuildTimestampUpdater()
    {
        // return new NowTimestampUpdater(
        //appConfig!.TimestampsOffsetInSeconds,
        // historyManager!.History!.Units.Last().To
        // );
        return new FromArrayTimestampUpdater(new DateTime[]
         {

          //new DateTime(1990,1,1,0,0,0,DateTimeKind.Local),
       
          new DateTime(2023,8,18,13,00,0,DateTimeKind.Local),
          new DateTime(2023,8,18,14,00,0,DateTimeKind.Local),
          new DateTime(2023,8,18,14,30,0,DateTimeKind.Local),
          new DateTime(2023,8,18,15,00,0,DateTimeKind.Local),
          new DateTime(2023,8,18,15,30,0,DateTimeKind.Local),
          new DateTime(2023,8,18,16,00,0,DateTimeKind.Local),
          new DateTime(2023,8,18,16,30,0,DateTimeKind.Local),
          new DateTime(2023,8,18,16,00,0,DateTimeKind.Local),
          new DateTime(2023,8,18,16,30,0,DateTimeKind.Local),
          new DateTime(2023,8,18,17,00,0,DateTimeKind.Local),
          new DateTime(2023,8,18,17,30,0,DateTimeKind.Local),
          new DateTime(2023,8,18,18,00,0,DateTimeKind.Local)



          });
        
        //return new NowTimestampUpdater(appConfig!.TimestampsOffsetInSeconds, appConfig!.FirstTimestamp);
    }

  
    private static void ReadConfiguration()
    {
        if (!File.Exists("config/appConfig.json"))
        {
            Console.WriteLine("Configuration File not found");
            System.Environment.Exit(1);
        }
        appConfig = JsonSerializer.Deserialize<AppConfiguration>(File.ReadAllText("config/appConfig.json"));
        if (appConfig == null)
            throw new ReadConfigException("Configuration creation failed");      
    }
}

