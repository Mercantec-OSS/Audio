using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MetaDataLink
    {
        public double bitdepth { get; set; }

        public int channels { get; set; }

        public double duration { get; set; }

        public int filesize { get; set; }

        public int samplerate { get; set; }
    }
}
