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

class Program
{
    private static System.Timers.Timer? timer;
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
        IHospitalProvider jsonProvider = new TestObjectForJsonProvider();
        //create a json file name for specific time
        IJsonProcessor jsonProcessor = new FileJsonProcessor("data/stamp_" + DateTime.Now.ToString("yyyy.MM.dd_HH_mm_ss") + ".json");
        //from object Root create a txt format of json
        ConverterJson converterJson = new ConverterJson();
        //find an error in json 
        try
        {
            jsonProcessor.ProcessJson(converterJson.Convert(jsonProvider.GetHospital()));
        }
        //process this error in block catch  
        catch (Exception ex) 
        {
            ProcessError(ex);
        }
    }
    //function for error  processng where ex is error value 
    private static void ProcessError(Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
    //launch app and show time of start
    static int Main(string[] args)
    {
        // CLAPP-Verbindungskonfiguration erstellen
        //CLAPPConfiguration config = new CLAPPConfiguration()
        //{
        //    Certificate = "361609fb-09a8-47c8-9bc3-d2b4db36eb2d.pfx", // Zertifikat: Zertifikatname (Windows Certificate Store) oder Pfad zur Zertifikatdatei (PFX)
        //    CertificateFilePassword = "fb6b456b-641f-4a3f-b31b-938ec2149824".ToSecureString(), // Passwort zum Öffnen des PFX
        //    CLAPPID = "CLAPP1", // CLAPP ID
        //    DomainID = "clappathon", // Domain ID
        //    ServerHostname = "SRVDEMOICM05V.DRAEGER.DEMO.CH",
        //    ServerPort = 25000
        //   
        //    //    "361609fb-09a8-47c8-9bc3-d2b4db36eb2d.pfx",
        //    //    "fb6b456b-641f-4a3f-b31b-938ec2149824",
        //    //    "CLAPP1",
        //    //    "clappathon",
        //    
        //    //Client name – CLTDEMOCLAPP01
        //    //DSI name – clappathon
        //    
        //};
        //try
        //{
        //    // CLAPP-Objekt erstellen / Verbindung aufbauen
        //    using (CLAPP clapp = new CLAPP(config))
        //    {
        //        PatientsList pList = clapp.GetPatientsList(); // Liste der Patienten auslesen
        //        foreach (Patient p in pList.PatientList) // Über alle Patienten iterieren
        //        {
        //            clapp.SetPatient(p.CaseID); // Patienten selektieren
        //            PatientDemographics pd = clapp.GetPatientDemographics(p.CaseID); // Demografische Daten des Patienten auslesen
        //            Console.WriteLine("Patient's Name:\t{0}", pd.PatientName); // Patientennamen ausgeben
        //            clapp.ReleasePatient(); // Patienten deselektieren
        //        }
        //    } // Ende von using-Block: Automatisches CLAPP.Dispose()
        //}
        //catch (Exception e)
        //{
        //    Console.WriteLine("Exception:\t{0}", e.ToString());
        //}
        //
        SetTimer();
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
}

