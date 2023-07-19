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
            historyManager = BuildHistoryManager();
            timestampUpdater = BuildTimestampUpdater();
            RequestManagerByTime request = new RequestManagerByTime(
                timestampUpdater, 
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
        //return new FromArrayTimestampUpdater(new DateTime[]
        // {
        //
        //  new DateTime(1990,1,1,0,0,0,DateTimeKind.Local),
        // new DateTime(2023,6,13,9,0,0,DateTimeKind.Local),
        //  new DateTime(2023,6,13,9,30,0,DateTimeKind.Local),
        //  new DateTime(2023,6,8,16,30,0,DateTimeKind.Local),
        //  new DateTime(2023,6,8,18,30,0,DateTimeKind.Local),
        //  new DateTime(2023,6,8,13,10,0,DateTimeKind.Local),
        //  new DateTime(2023,6,19,11,00,0,DateTimeKind.Local),
        //  new DateTime(2023,6,19,11,30,0,DateTimeKind.Local),
        //  new DateTime(2023,6,19,12,00,0,DateTimeKind.Local),
        //  new DateTime(2023,6,8,13,50,0,DateTimeKind.Local),
        //  new DateTime(2023,6,8,14,00,0,DateTimeKind.Local),
        //  new DateTime(2023,6,8,14,10,0,DateTimeKind.Local),
        //  new DateTime(2023,6,8,14,20,0,DateTimeKind.Local),
        //  new DateTime(2023,6,8,14,30,0,DateTimeKind.Local),
        //  new DateTime(2023,6,8,14,40,0,DateTimeKind.Local),
        //  new DateTime(2023,6,8,14,50,0,DateTimeKind.Local),
        //  new DateTime(2023,6,8,15,00,0,DateTimeKind.Local),
        //  new DateTime(2023,5,10,9,30,0,DateTimeKind.Local),
        //  new DateTime(2023,5,10,9,40,0,DateTimeKind.Local),
        //  new DateTime(2023,5,10,9,50,0,DateTimeKind.Local),
        //  new DateTime(2023,5,10,10,0,0,DateTimeKind.Local),
        //  new DateTime(2023,5,10,10,10,0,DateTimeKind.Local),
        //  new DateTime(2023,5,10,10,20,0,DateTimeKind.Local),
        //  new DateTime(2023,5,10,10,30,0,DateTimeKind.Local),
        //  new DateTime(2023,5,10,10,40,0,DateTimeKind.Local),
        //  new DateTime(2023,5,10,10,50,0,DateTimeKind.Local),
        //  new DateTime(2023,5,10,11,0,0,DateTimeKind.Local)
        //
        //
        //  });
        return new NowTimestampUpdater(appConfig!.TimestampsOffsetInSeconds, appConfig!.FirstTimestamp);
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

