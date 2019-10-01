using System;
using System.Collections.Generic;
using System.Xml;

namespace GPX.Common
{
#pragma warning disable IDE1006 // Benennungsstile
    public class GpxFile
    {
        public string Version { get; set; }
        public string Creator { get; set; }
        public GpxMetadata Metadata { get; set; }

        public XmlElement[] Extensions { get; set; }

        public List<GpxTrack> Tracks { get; set; }
        public List<GpxWaypoint> Waypoints { get; set; }

        public GpxFile()
        {
            Tracks = new List<GpxTrack>();
            Waypoints = new List<GpxWaypoint>();
        }
    }

    public class GpxTrack
    {
        public string Name { get; set; }
        public string Cmt { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }

        public XmlElement[] Extensions { get; set; }

        public List<GpxTrackSegment> Segments { get; set; }

        public GpxTrack()
        {
            Segments = new List<GpxTrackSegment>();
        }
    }

    public class GpxTrackSegment
    {
        public List<GpxWaypoint> TrackPoints { get; set; }

        public XmlElement[] Extensions { get; set; }

        public GpxTrackSegment()
        {
            TrackPoints = new List<GpxWaypoint>();
        }
    }

    public class GpxMetadata
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public GpxPerson Author { get; set; }

        //TODO Copyright

        public DateTime Time { get; set; }

        public string Keywords { get; set; }

        //TODO Bounds

        public XmlElement[] Extensions { get; set; }

        public GpxLink[] link { get; set; }
    }

    public class GpxPerson
    {
        public string Name { get; set; }

        public GpxEmail Email { get; set; }

        public GpxLink link { get; set; }

    }

    public class GpxEmail
    {
        public string Id { get; set; }

        public string Domain { get; set; }
    }

    public class GpxLink
    {
        public string Text { get; set; }

        public string Type { get; set; }

        public string Href { get; set; }
    }

    public class GpxCoordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
    }

    public class GpxWaypoint
    {
        public GpxCoordinate Coordinate { get; set; }

        public XmlElement[] Extensions { get; set; }

        public DateTime Time { get; set; }

        public bool TimeSpecified { get; set; }

        public decimal Magvar { get; set; }

        public bool MagvarSpecified { get; set; }

        public decimal GeoIdHeight { get; set; }

        public bool GeoIdHeightSpecified { get; set; }

        public string Name { get; set; }

        public string Cmt { get; set; }

        public string Desc { get; set; }

        public string Src { get; set; }

        public string sym { get; set; }

        public string type { get; set; }

        public string sat { get; set; }

        public decimal hdop { get; set; }

        public bool hdopSpecified { get; set; }

        public decimal vdop { get; set; }

        public bool vdopSpecified { get; set; }

        public decimal pdop { get; set; }

        public bool pdopSpecified { get; set; }

        public decimal ageofdgpsdata { get; set; }

        public bool ageofdgpsdataSpecified { get; set; }

        public string dgpsid { get; set; }

        public List<GpxLink> Link { get; set; }
    }

#pragma warning restore IDE1006 // Benennungsstile
}

