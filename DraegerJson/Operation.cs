using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerJson
{
    public class Operation
    {
        public string ResourceType { get; set; } = "Procedure";
        public string Id { get; set; } = "";
        public string Status { get; set; } = "";
        public List<Parameter> Params { get; set; } = new List<Parameter>();

        public bool Exist 
        { 
            get 
            {
                return Id.Length > 0;
            } 
        }
        public override bool Equals(object? obj)
        {
            Operation? op = obj as Operation;
            if(op == null)
            {
                return false;
            }
            return ResourceType.Equals(op.ResourceType) &&
                   Id.Equals(op.Id) &&
                   Status.Equals(op.Status) &&
                   ParamsEquals(op);
        }

        private bool ParamsEquals(Operation op)
        {
            if (Params.Count != op.Params.Count) 
            {
                return false;
            }
            foreach (Parameter param in Params)
            {
                if(!op.Params.Contains(param))
                {
                    return false;
                }
            }
            foreach(Parameter param in op.Params)
            {
                if (!Params.Contains(param))
                    return false;
            }
            return true;
        }
    }
}
