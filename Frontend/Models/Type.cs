﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Type
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Channels { get; set; }

        public int AverageBPM { get; set; }

        public string Key { get; set; }

    }
}
