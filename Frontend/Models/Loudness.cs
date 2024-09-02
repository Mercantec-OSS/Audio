using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Loudness
    {
        public int Id { get; set; }

        public double PeakAmplitude { get; set; }

        public double RmsLevel { get; set; }
    }
}
