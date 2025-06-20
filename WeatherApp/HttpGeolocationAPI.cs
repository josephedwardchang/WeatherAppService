using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Text.Json;
using WeatherApp.Models;
using System.Net.Http.Headers;
using System.Runtime.Remoting;
using Newtonsoft.Json;

namespace WeatherApp
{
    public class HttpGeolocationAPI
    {
        public JsonGeoLocationApi m_jsonGeocodeApi;
        public XmlGeoLocationApi m_xmlGeocodeApi;

        public HttpGeolocationAPI() 
        {
            m_jsonGeocodeApi = null;
            m_xmlGeocodeApi = null;
        }

        private async Task<string> GetGeoLocationApiAsync(ResultType resultType, string cityname, string statecode = null, string countrycode = null)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var bRet = resultType == ResultType.json ? "application/json" : "text/xml";

                    var strUrl = ConfigurationManager.AppSettings["paramGeocodeApi"];
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["GeocodeApiUrl"]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(bRet));

                    strUrl = strUrl.Replace("{city}", cityname).Replace("{statecode}", statecode ?? "").Replace("{countrycode}", countrycode ?? "");
                    strUrl = strUrl.Replace("{mode}", resultType == ResultType.json ? "" : "mode=xml");
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

        public async Task<List<XmlGeoLocationApi>> GetXmlGeolocationFromName(string cityname, string statecode = null, string countrycode = null)
        {
            List<XmlGeoLocationApi> objRet = null;
            try
            {
                var xmlString = await GetGeoLocationApiAsync(ResultType.xml, cityname, statecode, countrycode);

                XmlSerializer serializer = new XmlSerializer(typeof(List<XmlGeoLocationApi>));
                StringReader rdr = new StringReader(xmlString);
                objRet = (List<XmlGeoLocationApi>)serializer.Deserialize(rdr);

            }
            catch (Exception ex)
            {
            }

            return objRet;
        }

        public async Task<List<JsonGeoLocationApi>> GetJsonGeolocationFromName(string cityname, string statecode = null, string countrycode = null)
        {
            List<JsonGeoLocationApi> objRet = null;
            try
            {
                var jsonString = await GetGeoLocationApiAsync(ResultType.json, cityname, statecode, countrycode);

                objRet = JsonConvert.DeserializeObject<List<JsonGeoLocationApi>>(jsonString);

            }
            catch (Exception ex)
            { 
            }

            return objRet;
        }
    }
}
