using AlbumsSite.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace AlbumsSite.Command.Image
{
    [DataContract]
    public class Address
    {
        [DataMember]
        public string road { get; set; }
        [DataMember]
        public string suburb { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string state_district { get; set; }
        [DataMember]
        public string state { get; set; }
        [DataMember]
        public string postcode { get; set; }
        [DataMember]
        public string country { get; set; }
        [DataMember]
        public string country_code { get; set; }
    }

    [DataContract]
    public class RootObject
    {
        [DataMember]
        public string place_id { get; set; }
        [DataMember]
        public string licence { get; set; }
        [DataMember]
        public string osm_type { get; set; }
        [DataMember]
        public string osm_id { get; set; }
        [DataMember]
        public string lat { get; set; }
        [DataMember]
        public string lon { get; set; }
        [DataMember]
        public string display_name { get; set; }
        [DataMember]
        public Address address { get; set; }
    }
    public interface IGetPlaceByGPSCommand
    {
        string ExecuteAsync(double lat, double lon);
    }
    public class GetPlaceByGPSCommand : IGetPlaceByGPSCommand
    {
        
        public string ExecuteAsync(double lat, double lon)
        {
            string latConverted=lat.ToString().Replace(',', '.');
            string lonConwerted=lon.ToString().Replace(',', '.');            
            WebClient webClient = new WebClient();
            webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            webClient.Headers.Add("Referer", "http://www.microsoft.com");
            string url = "https://nominatim.openstreetmap.org/reverse?format=json&lat="+ latConverted + "&lon="+ lonConwerted;
            var jsonData = webClient.DownloadData(url);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RootObject));
            RootObject rootObject = (RootObject)ser.ReadObject(new MemoryStream(jsonData));
            string place = rootObject.address.country + " " + rootObject.address.city;
            return place;
        }
    }
}
