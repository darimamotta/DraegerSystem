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
    private static GlobalConfiguration? globConfiguration;
    private static TimestampHistoryManager? historyManager;
    private static ITimestampUpdater? timestampUpdater;
    private const string PathToHistory = "history/history.json";
    private static void ProcessError(Exception ex)
    {
        Console.WriteLine(ex.ToString());
        System.Environment.Exit(1);
    }

    static int Main(string[] args)
    {
        
        try
        {
            ReadConfigs();
            historyManager = BuildHistoryManager();
            timestampUpdater = BuildTimestampUpdater();
            RequestManagerByTime request = new RequestManagerByTime(
                globConfiguration!.DelayBetweenRequestsInMilliseconds, 
                timestampUpdater, 
                historyManager
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
        TimestampHistoryManager thm = new TimestampHistoryManager(PathToHistory);
        if (File.Exists(PathToHistory))
        {
            thm.Load();
        }
        else 
        { 
            DateTime start = ReadTimestampFromConsole();
            thm.Initialize(start);          
        }
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
        return new NowTimestampUpdater(
            globConfiguration!.TimestampsOffsetInSeconds,
            historyManager!.History!.Units.Last().To
        );
    }

    private static void ReadConfigs()
    {
        globConfiguration = JsonSerializer.Deserialize<GlobalConfiguration>(File.ReadAllText("config/globalConfig.json"));
        
    }
}

