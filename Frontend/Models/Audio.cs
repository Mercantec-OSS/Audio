using System.Diagnostics.Metrics;

namespace Models
{
    public class Audio
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Hidden { get; set; }

        public string Description { get; set; }

        public int Duration { get; set; }

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

        public List<MadeOf> MadeOf { get; set; }

        public List<UsedIn> UsedIn { get; set; }

    }
}
