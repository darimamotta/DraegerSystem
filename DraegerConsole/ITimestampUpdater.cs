﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerConsole
{
    public interface ITimestampUpdater
    {
        DateTime CurrentTimestamp { get; set; }
        DateTime PastTimestamp { get; set; }
        void UpdateTimestamps();
    }
}