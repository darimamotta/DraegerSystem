﻿using System;
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
    }
}