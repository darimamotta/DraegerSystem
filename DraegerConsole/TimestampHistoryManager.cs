using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using DraegerConsole.Exceptions;

namespace DraegerConsole
{
    public class TimestampHistoryManager
    {
        public static readonly DateTime DefaultStartTimestamp = new DateTime(1990, 1, 1);
        private string pathToHistory;
        public TimestampHistory? History
        {
            get; private set;
        }
       
        
        public TimestampHistoryManager(string pathToHistory) 
        { 
            this.pathToHistory = pathToHistory;
            History = new TimestampHistory();
        }
        public void Load()
        {
            if(File.Exists(pathToHistory))
            {
                History = JsonSerializer.Deserialize<TimestampHistory>(File.ReadAllText(pathToHistory));

            }
            else 
            {
                throw new FileNotFoundException("File of history was not found");
               
            }
           
        }
        public void Save()
        {
            if (History == null)
                throw new OrderOfOperationsException("History must be initialized before saving");
            string? dirName = Path.GetDirectoryName(pathToHistory);
            if (dirName == null)
            { throw new UnknownErrorException("Directory name is null"); }
            if(!Directory.Exists(dirName))
            { 
                Directory.CreateDirectory(dirName); 
            }
            File.WriteAllText(pathToHistory, JsonSerializer.Serialize(History));
        }
        public void Initialize()
        {
            History = new TimestampHistory();
            History.Units.Add(new TimestampHistoryUnit
            {
                From = DefaultStartTimestamp,
                To = DefaultStartTimestamp

            }
            );
        }
        public void Initialize(DateTime startTimestamp)
        {
            History = new TimestampHistory();
            History.Units.Add(new TimestampHistoryUnit
            {
                From = startTimestamp,
                To = startTimestamp

            }
            );

        }
    }
}
