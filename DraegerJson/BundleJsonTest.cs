using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerJson
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Category
    {
        public List<Coding> coding { get; set; }
        public string text { get; set; }
    }

    public class Code
    {
        public List<Coding> coding { get; set; }
        public string text { get; set; }
    }

    public class Coding
    {
        public string system { get; set; }
        public string code { get; set; }
        public string display { get; set; }
    }

    public class Entry
    {
        public Resource resource { get; set; }
    }

    public class PartOf
    {
        public string reference { get; set; }
    }

    public class Recorder
    {
        public string reference { get; set; }
    }

    public class Resource
    {
        public string resourceType { get; set; }
        public string id { get; set; }
        public PartOf partOf { get; set; }
        public string status { get; set; }
        public Category category { get; set; }
        public Code code { get; set; }
        public Subject subject { get; set; }
        public Recorder recorder { get; set; }
        public DateTime performedDateTime { get; set; }
    }

    public class Root
    {
        public string resourceType { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public List<Entry> entry { get; set; }
    }

    public class Subject
    {
        public string reference { get; set; }
    }
    //Serialze Objects
    public class ConverterJson
    {
        public string Convert(Root root)
        {
            return JsonConvert.SerializeObject(root);
        }
    }

}
