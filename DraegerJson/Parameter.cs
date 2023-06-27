using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DraegerJson
{
    public class Parameter:IComparable<Parameter>
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public DateTime Date { get; set; }
        public string PatientId { get; set; } = "";

        public override int GetHashCode()
        {
            return System.HashCode.Combine(Id, Name, Date, PatientId);
        }
        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", Id, Date, Name);
        }
        public override bool Equals(object? obj)
        {
            if(obj == null) return false;
            Parameter? other = obj as Parameter;
            if (other == null) return false;
            return Id.Equals(other.Id)&&Name.Equals( other.Name)&& Date.Equals(other.Date)&& PatientId.Equals(other.PatientId);
         
        }

        public int CompareTo(Parameter? other)
        {
           if(other == null) return -1;
           if(Date!=other.Date) return Date.CompareTo(other.Date);
           if( Name.CompareTo(other.Name)!= 0)
            return Name.CompareTo(other.Name);
          
           return PatientId.CompareTo(other.PatientId);
        }
    }
}
