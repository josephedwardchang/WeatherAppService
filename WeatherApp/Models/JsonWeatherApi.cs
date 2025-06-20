using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherApp.Models
{
    public class JsonWeatherApi
    {// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Clouds
        {
            public int all;
        }

        public class Coord
        {
            public double lon;
            public double lat;
        }

        public class Main
        {
            public double temp;
            public double feels_like;
            public double temp_min;
            public double temp_max;
            public int pressure;
            public int humidity;
            public int sea_level;
            public int grnd_level;
        }

        public class Rain
        {
            [JsonPropertyName("1h")]
            public double _1h;
        }

        public class Root
        {
            public Coord coord;
            public List<Weather> weather;
            public string @base;
            public Main main;
            public int visibility;
            public Wind wind;
            public Rain rain;
            public Clouds clouds;
            public int dt;
            public Sys sys;
            public int timezone;
            public int id;
            public string name;
            public int cod;
        }

        public class Sys
        {
            public int type;
            public int id;
            public string country;
            public int sunrise;
            public int sunset;
        }

        public class Weather
        {
            public int id;
            public string main;
            public string description;
            public string icon;
        }

        public class Wind
        {
            public double speed;
            public int deg;
            public double gust;
        }


    }
}
