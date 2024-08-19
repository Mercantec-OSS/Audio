using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Audio
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public bool Hidden { get; set; }

        public string Description { get; set; }

        public double Duration { get; set; }

        public int SampleRate { get; set; }

        public int BitDepth { get; set; }

        public string FileSize { get; set; }

        public DateTime UploadDate { get; set; }

        public int Downloads { get; set; }

        public string Format { get; set; }

        public string FilePlacement { get; set; }

        public int UserID { get; set; }

        public List<Category> Category { get; set; }

        public List<Genre> Genre { get; set; }

        public Type Type { get; set; }

        public Loudness Loudness { get; set; }

        public Instrument Instrument { get; set; }

        public Mood Mood { get; set; }

        public Other Other { get; set; }
    }
}
