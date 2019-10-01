using System;
namespace GPX.Common
{
    public interface IGpxDataAccess
    {
        GpxFile Read(string gpxContent);
        string Write(GpxFile content);

        GradMinutesSeconds CalculateGradMinutesSeconds(double koordinate);
    }

    public class GradMinutesSeconds {
        public int Grad { get; set; }
        public int Minutes { get; set; }
        public double Seconds { get; set; }
    }
}
