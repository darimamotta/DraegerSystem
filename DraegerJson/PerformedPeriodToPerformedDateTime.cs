using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;

namespace DraegerJson
{
    public class PerformedPeriodToPerformedDateTime : IJsonModifier
    {
        public string Modify(string json)
        {
            StringBuilder result = new StringBuilder();
            int pos = json.IndexOf("performedPeriod");
            int past = 0;
            while (pos >= 0)
            {
                result.Append(json.Substring(past, pos - past))
                    .Append("performedDateTime\":\"");
                past = json.IndexOf("\"start\"",pos)+8;
                pos = json.IndexOf('\"', past)+1;
                past = json.IndexOf('\"', pos) + 1;
                result.Append(json.Substring (pos, past - pos));
                past = json.IndexOf('}',past)+1;
                pos = json.IndexOf("performedPeriod", past);                
             
            }
            result.Append(json.Substring(past));
            return result.ToString();


        }
    }
}
