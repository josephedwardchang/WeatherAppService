using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WeatherApp.Models
{
    public class XmlWeatherApi
    {
        // using System.Xml.Serialization;
        // XmlSerializer serializer = new XmlSerializer(typeof(Current));
        // using (StringReader reader = new StringReader(xml))
        // {
        //    var test = (Current)serializer.Deserialize(reader);
        // }

        public class Coord
        {
            public double Lon;
            public double Lat;
        }

        public class Sun
        {
            public DateTime Rise;
            public DateTime Set;
        }

        public class City
        {
            public Coord Coord;
            public string Country;
            public int Timezone;
            public Sun Sun;
            public int Id;
            public string Name;
            public string Text;
        }

        public class Temperature
        {
            public double Value;
            public double Min;
            public double Max;
            public string Unit;
        }

        public class FeelsLike
        {
            public DateTime Value;
            public string Unit;
        }

        public class Humidity
        {
            public int Value;
            public string Unit;
        }

        public class Pressure
        {
            public int Value;
            public string Unit;
        }

        public class Speed
        {
            public double Value;
            public string Unit;
            public string Name;
        }

        public class Direction
        {
            public int Value;
            public string Code;
            public string Name;
        }

        public class Wind
        {
            public Speed Speed;
            public object Gusts;
            public Direction Direction;
        }

        public class Clouds
        {
            public int Value;
            public string Name;
        }

        public class Visibility
        {
            public int Value;
        }

        public class Precipitation
        {
            public string Mode;
        }

        public class Weather
        {
            public int Number;
            public string Value;
            public string Icon;
        }

        public class Lastupdate
        {
            public DateTime Value;
        }

        public class Current
        {
            public City City;
            public Temperature Temperature;
            public FeelsLike FeelsLike;
            public Humidity Humidity;
            public Pressure Pressure;
            public Wind Wind;
            public Clouds Clouds;
            public Visibility Visibility;
            public Precipitation Precipitation;
            public Weather Weather;
            public Lastupdate Lastupdate;
        }
    }
}
