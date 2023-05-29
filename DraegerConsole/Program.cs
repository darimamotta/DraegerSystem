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
  
    private static void ProcessError(Exception ex)
    {
        Console.WriteLine(ex.ToString());
        System.Environment.Exit(1);
    }
    static int Main(string[] args)
    {
        RequestManagerByTime request = new RequestManagerByTime(10000);
        try
        {
            request.StartRequests();
        }
        catch (Exception ex) 
        {
            ProcessError(ex);
        }
        return 0;
      
    }

 

    
}

