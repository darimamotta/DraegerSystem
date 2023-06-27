using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerJson
{
    public class ParameterHistory
    {
        private SortedSet< Parameter> history;
       
        public ParameterHistory()
        {
            this.history = new SortedSet< Parameter>();
            
        }
        public void Add(Parameter parameter)
        {
            history.Add(parameter);
        }
        public bool Contains(Parameter parameter)
        {

            return history.Contains(parameter);
        }
        public void RemoveOld(DateTime cutOffDate)
        {
            while (history.Count > 0 && history.Min!.Date < cutOffDate) 
            { 
                history.Remove(history.Min);
            }
        }
    }
}
