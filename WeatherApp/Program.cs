using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var callApi = new HttpGeolocationAPI();
            var callWeatherApi = new HttpWeatherAPI();

            const string City = "Manila";

            // JSON
            Task<List<JsonGeoLocationApi>> test = Task.Run(async () =>
            {
                return await callApi.GetJsonGeolocationFromName(cityname: City);
            });

            if (test.Result != null && test.Result.Count > 0)
            {
                var lon = test.Result[0].lon;
                var lat = test.Result[0].lat;
                var test1 = Task.Run(async () =>
                {
                    // error: bad request when doing lat/lon coords from Geocode
                    //return await callWeatherApi.GetJsonWeatherFromCoords(lat, lon);
                    var obj = await callWeatherApi.GetJsonWeatherFromCityName(cityname: City);
                    return obj;
                });
                Console.WriteLine(test1.Result.ToString());
            }

            // XML 
            // Error: Geocode XML result not available from API
            //Task<List<XmlGeoLocationApi>> test2 = Task.Run(async () =>
            //{
            //    return await callApi.GetXmlGeolocationFromName(cityname: City);
            //});

            if (test.Result != null && test.Result.Count > 0)
            {
                var lon = test.Result[0].lon;
                var lat = test.Result[0].lat;

                var test3 = Task.Run(async () =>
                {
                    var obj = await callWeatherApi.GetXmlWeatherFromCityName(cityname: City);
                    return obj;
                });
                Console.WriteLine(test3.Result.ToString());

                if(test3.Result != null)
                {
                    // can't seem to get the values after deserialization is ok from weather api
                    // (both json/xml is ok from api) but in the object after deserialization,
                    // I can't see the values
                    var n = test3.Result;
                }
            }
        }
    }
}
