using AutoMapper;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace GPX.Common
{
    public class GpxDataAccess : GPX.Common.IGpxDataAccess
    {
        private readonly XmlSerializer _serializer;

        static GpxDataAccess()
        {
            Mapper.CreateMap<XmlElement[], extensionsType>()
                .ForMember(d => d.Any, opt => opt.MapFrom(src => src));

            Mapper.CreateMap<double, decimal>()
                .ConvertUsing((doub) =>
            {
                if (double.NaN.Equals(doub))
                {
                    return decimal.Zero;
                }
                return Convert.ToDecimal(doub);
            });

            Mapper.CreateMap<gpxType, GpxFile>()
                .ForMember(d => d.Tracks, opt => opt.MapFrom(s => s.trk))
                .ForMember(d => d.Extensions, opt => opt.MapFrom(s => s.extensions.Any));
            Mapper.CreateMap<GpxFile, gpxType>()
                .ForMember(d => d.trk, opt => opt.MapFrom(s => s.Tracks));

            Mapper.CreateMap<trkType, GpxTrack>()
                .ForMember(d => d.Segments, opt => opt.MapFrom(s => s.trkseg))
                .ForMember(d => d.Extensions, opt => opt.MapFrom(s => s.extensions.Any));
            Mapper.CreateMap<GpxTrack, trkType>()
                .ForMember(d => d.trkseg, opt => opt.MapFrom(s => s.Segments))
                .AfterMap((src, dst) =>
                {
                    if (dst.extensions == null)
                    {
                        dst.extensions = new extensionsType();
                    }
                    dst.extensions.Any = src.Extensions;
                });

            Mapper.CreateMap<wptType, GpxWaypoint>()
                .ForMember(d => d.Extensions, opt => opt.MapFrom(s => s.extensions.Any))
                .AfterMap((src, dst) =>
                {
                    if (src.eleSpecified)
                    {
                        dst.Coordinate = new GeoCoordinate(Convert.ToDouble(src.lat), Convert.ToDouble(src.lon), Convert.ToDouble(src.ele));
                    }
                    else
                    {
                        dst.Coordinate = new GeoCoordinate(Convert.ToDouble(src.lat), Convert.ToDouble(src.lon));
                    }
                });
            Mapper.CreateMap<GpxWaypoint, wptType>()
                .ForMember(d => d.lon, opt => opt.MapFrom(s => s.Coordinate.Longitude))
                .ForMember(d => d.ele, opt => opt.MapFrom(s => s.Coordinate.Altitude))
                .ForMember(d => d.lat, opt =>
                {
                    opt.PreCondition((GpxWaypoint x) => !double.NaN.Equals(x.Coordinate.Latitude));
                    opt.MapFrom(s => s.Coordinate.Latitude);
                })
                .AfterMap((src, dst) =>
                {
                    dst.timeSpecified = src.Time != DateTime.MinValue;
                    dst.eleSpecified = !src.Coordinate.Altitude.Equals(double.NaN);
                }); ;

            Mapper.CreateMap<trksegType, GpxTrackSegment>()
                .ForMember(d => d.TrackPoints, opt => opt.MapFrom(s => s.trkpt))
                .ForMember(d => d.Extensions, opt => opt.MapFrom(s => s.extensions.Any));
            Mapper.CreateMap<GpxTrackSegment, trksegType>()
                .ForMember(d => d.trkpt, opt => opt.MapFrom(s => s.TrackPoints));

            Mapper.CreateMap<metadataType, GpxMetadata>()
                .ForMember(d => d.Extensions, opt => opt.MapFrom(s => s.extensions.Any));
            Mapper.CreateMap<GpxMetadata, metadataType>()
                .AfterMap((src, dst) =>
                {
                    dst.timeSpecified = src.Time != DateTime.MinValue;
                });

            Mapper.CreateMap<personType, GpxPerson>();
            Mapper.CreateMap<GpxPerson, personType>();

            Mapper.CreateMap<emailType, GpxEmail>();
            Mapper.CreateMap<GpxEmail, emailType>();

            Mapper.CreateMap<linkType, GpxLink>();
            Mapper.CreateMap<GpxLink, linkType>();
        }

        public GpxDataAccess()
        {
            _serializer = new XmlSerializer(typeof(gpxType));
        }

        public GpxFile Read(string gpxContent)
        {
            var gpx = (gpxType)_serializer.Deserialize(new StringReader(gpxContent));

            return ReadToDto(gpx);
        }

        public string Write(GpxFile content)
        {
            var writer = new StringWriter();

            _serializer.Serialize(writer, WriteFromDto(content));

            return writer.ToString();
        }

        private gpxType WriteFromDto(GpxFile data)
        {
            var result = Mapper.Map<GpxFile, gpxType>(data);

            return result;
        }

        private GpxFile ReadToDto(gpxType data)
        {
            var dto = new GpxFile();

            Mapper.Map<gpxType, GpxFile>(data, dto);

            return dto;
        }


        public GradMinutesSeconds CalculateGradMinutesSeconds(double koordinate)
        {
            var grad = Convert.ToInt32(Math.Floor(koordinate));
            var minutesComplete = (koordinate - grad) * 60;
            var minutes = Convert.ToInt32(Math.Floor(minutesComplete));
            var seconds = (minutesComplete - minutes) * 60;

            return new GradMinutesSeconds
            {
                Grad = grad,
                Minutes = minutes,
                Seconds = seconds
            };
        }
    }
}
