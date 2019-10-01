using AutoMapper;
using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace GPX.Common
{
    public class GpxDataAccess : GPX.Common.IGpxDataAccess
    {
        private readonly XmlSerializer _serializer;
        private readonly IMapper _mapper;

        public GpxDataAccess()
        {
            _serializer = new XmlSerializer(typeof(gpxType));
            var configuration = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<XmlElement[], extensionsType>()
                    .ForMember(d => d.Any, opt => opt.MapFrom(src => src));

                cfg.CreateMap<double, decimal>()
                    .ConvertUsing(doub => double.NaN.Equals(doub) ? decimal.Zero : Convert.ToDecimal(doub));

                cfg.CreateMap<gpxType, GpxFile>()
                    .ForMember(d => d.Tracks, opt => opt.MapFrom(s => s.trk))
                    .ForMember(d => d.Extensions, opt => opt.MapFrom(s => s.extensions.Any));
                cfg.CreateMap<GpxFile, gpxType>()
                    .ForMember(d => d.trk, opt => opt.MapFrom(s => s.Tracks));

                cfg.CreateMap<trkType, GpxTrack>()
                    .ForMember(d => d.Segments, opt => opt.MapFrom(s => s.trkseg))
                    .ForMember(d => d.Extensions, opt => opt.MapFrom(s => s.extensions.Any));
                cfg.CreateMap<GpxTrack, trkType>()
                    .ForMember(d => d.trkseg, opt => opt.MapFrom(s => s.Segments))
                    .AfterMap((src, dst) =>
                    {
                        if (dst.extensions == null)
                        {
                            dst.extensions = new extensionsType();
                        }
                        dst.extensions.Any = src.Extensions;
                    });

                cfg.CreateMap<wptType, GpxWaypoint>()
                    .ForMember(d => d.Extensions, opt => opt.MapFrom(s => s.extensions.Any))
                    .AfterMap((src, dst) =>
                    {
                        if (src.eleSpecified)
                        {
                            dst.Coordinate = new GpxCoordinate()
                            {
                                Latitude = Convert.ToDouble(src.lat),
                                Longitude = Convert.ToDouble(src.lon),
                                Altitude = Convert.ToDouble(src.ele)
                            };
                        }
                        else
                        {
                            dst.Coordinate = new GpxCoordinate()
                            {
                                Latitude = Convert.ToDouble(src.lat),
                                Longitude = Convert.ToDouble(src.lon)
                            };
                        }
                    });
                cfg.CreateMap<GpxWaypoint, wptType>()
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

                cfg.CreateMap<trksegType, GpxTrackSegment>()
                    .ForMember(d => d.TrackPoints, opt => opt.MapFrom(s => s.trkpt))
                    .ForMember(d => d.Extensions, opt => opt.MapFrom(s => s.extensions.Any));
                cfg.CreateMap<GpxTrackSegment, trksegType>()
                    .ForMember(d => d.trkpt, opt => opt.MapFrom(s => s.TrackPoints));

                cfg.CreateMap<metadataType, GpxMetadata>()
                    .ForMember(d => d.Extensions, opt => opt.MapFrom(s => s.extensions.Any));
                cfg.CreateMap<GpxMetadata, metadataType>()
                    .AfterMap((src, dst) =>
                    {
                        dst.timeSpecified = src.Time != DateTime.MinValue;
                    });

                cfg.CreateMap<personType, GpxPerson>();
                cfg.CreateMap<GpxPerson, personType>();

                cfg.CreateMap<emailType, GpxEmail>();
                cfg.CreateMap<GpxEmail, emailType>();

                cfg.CreateMap<linkType, GpxLink>();
                cfg.CreateMap<GpxLink, linkType>();
            });

            _mapper = configuration.CreateMapper();
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
            var result = _mapper.Map<GpxFile, gpxType>(data);

            return result;
        }

        private GpxFile ReadToDto(gpxType data)
        {
            var dto = new GpxFile();

            _mapper.Map<gpxType, GpxFile>(data, dto);

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
