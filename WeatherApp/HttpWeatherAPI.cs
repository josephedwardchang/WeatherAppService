﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using WeatherApp.Models;
using static WeatherApp.Models.JsonWeatherApi;

namespace WeatherApp
{
    public class HttpWeatherAPI
    {
        public JsonWeatherApi m_jsonWeatherApi;
        public XmlWeatherApi m_xmlWeatherApi;

        public HttpWeatherAPI()
        {
            m_jsonWeatherApi = new JsonWeatherApi();
            m_xmlWeatherApi = new XmlWeatherApi();
        }

        private async Task<string> GetWeatherApiAsync(ResultType resultType, double lat, double lon,string cityname = null, string statecode = null, string countrycode = null)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var bRet = resultType == ResultType.json ? "application/json" : "text/xml";

                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WeatherApiUrl"]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(bRet));

                    var strUrl = ConfigurationManager.AppSettings["paramWeatherGeocodeApi"];
                    if (string.IsNullOrEmpty(cityname))
                    {
                        strUrl = strUrl.Replace("{city}", "");
                    }
                    else
                    {
                        strUrl = strUrl.Replace("{city}", "q=" + cityname ?? "");
                    }
                    if (lat > 0.0 && lon > 0.0)
                    {
                        strUrl = strUrl.Replace("{lat}", "lat=" + lat.ToString()).Replace("{lon}", "&lon=" + lon.ToString());
                    }
                    else
                    {
                        strUrl = strUrl.Replace("{lat}", "").Replace("{lon}", "");
                    }
                    strUrl = strUrl.Replace("{mode}", resultType == ResultType.json ? "" : "&mode=xml");
                    strUrl = strUrl.Replace("{APIkey}", "&appid=" + ConfigurationManager.AppSettings["WeatherApiKey"]);

                    //GET Method
                    HttpResponseMessage response = await client.GetAsync(strUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var ans = response.Content.ReadAsStringAsync();
                        Console.WriteLine("\r\nraw {1}: {0}\r\n", ans.Result, bRet);
                        return ans.Result;
                    }
                    else
                    {
                        Console.WriteLine("404");
                    }
                }
            }
            catch 
            { 
            }

            return "";
        }
        public T DeserializeXml<T>(string xmlStringName, XmlRootAttribute root)
        {
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T), root);

            using(var stringReader = new StringReader(xmlStringName)){
                var xmlReader = new XmlTextReader(stringReader);
                return (T)xmlSerializer.Deserialize(xmlReader);
            }
        }

        public async Task<XmlWeatherApi> GetXmlWeatherFromCityName(string cityname, string statecode = null, string countrycode = null)
        {
            try
            {
                var xmlString = await GetWeatherApiAsync(ResultType.xml, 0, 0, cityname, statecode, countrycode);

                var xmlHdr = xmlString.Substring(0, xmlString.IndexOf("\n")+1);
                var xmlBody = xmlString.Substring(xmlString.IndexOf("<current>")+9);

                // insert default xmlns
                var xmlStringTmp = xmlHdr + "<current xmlns=\"https://openweathermap.org\">" + xmlBody;

                XmlRootAttribute xRoot = new XmlRootAttribute();
                xRoot.ElementName = "current";
                xRoot.Namespace = "https://openweathermap.org";
                xRoot.IsNullable = true;

                m_xmlWeatherApi = DeserializeXml<XmlWeatherApi>(xmlStringTmp, xRoot);
            }
            catch (Exception ex)
            {
            }

            return m_xmlWeatherApi;
        }

        public async Task<JsonWeatherApi> GetJsonWeatherFromCityName(string cityname, string statecode = null, string countrycode = null)
        {
            JsonWeatherApi jsonWeatherApi = new JsonWeatherApi();
            try
            {
                var jsonString = await GetWeatherApiAsync(ResultType.json, 0, 0, cityname, statecode, countrycode);

                jsonWeatherApi = JsonConvert.DeserializeObject<JsonWeatherApi>(jsonString);

            }
            catch (Exception ex)
            {
            }

            return jsonWeatherApi;
        }

        public async Task<XmlWeatherApi> GetXmlWeatherFromCoords(double lon, double lat)
        {
            try
            {
                var xmlString = await GetWeatherApiAsync(ResultType.xml, lat, lon, "", "", "");

                m_xmlWeatherApi = DeserializeXml<XmlWeatherApi>(xmlString, null);

            }
            catch (Exception ex)
            {
            }

            return m_xmlWeatherApi;
        }

        public async Task<JsonWeatherApi> GetJsonWeatherFromCoords(double lon, double lat)
        {
            try
            {
                var jsonString = await GetWeatherApiAsync(ResultType.json, lat, lon, "", "", "");

                m_jsonWeatherApi = JsonConvert.DeserializeObject<JsonWeatherApi>(jsonString);

            }
            catch (Exception ex)
            {
            }

            return m_jsonWeatherApi;
        }
    }
}
