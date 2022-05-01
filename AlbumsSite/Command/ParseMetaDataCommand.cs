using AlbumsSite.Models;
using ExifLibrary;
using MetadataExtractor;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Command
{
    public interface IParseMetaDataCommand
    {
        (double GPSLatitude, double GPSLongitude) ExecuteAsync(string path, Picture picture);
        
    }
    public class ParseMetaDataCommand : IParseMetaDataCommand
    {
        public (double GPSLatitude, double GPSLongitude) ExecuteAsync(string path, Picture picture)
        {
            var file = ImageFile.FromFile(path);
            List<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(path).ToList();
            picture.Width = int.Parse(directories[0].Tags.SingleOrDefault(x => x.Name == "Image Width").Description.Split(' ')[0]);
            picture.Height = int.Parse(directories[0].Tags.SingleOrDefault(x => x.Name == "Image Height").Description.Split(' ')[0]);
            picture.CameraModel = file.Properties.Get<ExifProperty>(ExifTag.Make)?.ToString() ?? string.Empty + " " + file.Properties.Get<ExifProperty>(ExifTag.Model)?.ToString() ?? string.Empty;
            picture.Date = file.Properties.Get<ExifDateTime>(ExifTag.DateTimeOriginal) ?? DateTime.Now;
            picture.ISO = file.Properties.Get<ExifProperty>(ExifTag.ISOSpeedRatings)?.ToString() ?? string.Empty;
            picture.Flash = file.Properties.Get<ExifEnumProperty<Flash>>(ExifTag.Flash)?.ToString() ?? string.Empty;
            var FocalLength = file.Properties.Get<ExifURational>(ExifTag.FocalLength);
            var result = string.Empty;
            if (FocalLength != null)
                result = (int)FocalLength + ".0mm";
            picture.FocalLength = result;
            picture.ShutterSpeed = file.Properties.Get<ExifURational>(ExifTag.ExposureTime)?.ToString() ?? string.Empty;
            picture.Name = file.Properties.Get<ExifProperty>(ExifTag.WindowsTitle)?.ToString() ?? string.Empty;
            var aperture = file.Properties.Get<ExifURational>(ExifTag.FNumber);
            result = string.Empty;
            if (aperture != null)
                result = "f/" + Math.Round((double)aperture, 1);
            picture.Aperture = result;
            var GPSLatitude = file.Properties.Get<GPSLatitudeLongitude>(ExifTag.GPSLatitude);
            double resultLat = double.NaN;
            if (GPSLatitude != null)
                resultLat = ConwertGPS(GPSLatitude);


            var GPSLongitude = file.Properties.Get<GPSLatitudeLongitude>(ExifTag.GPSLongitude);
            double resultLong = double.NaN;
            if (GPSLongitude != null)
                resultLong = ConwertGPS(GPSLongitude);
            return (GPSLatitude: resultLat, GPSLongitude: resultLong);
        }
        

        private double ConwertGPS(GPSLatitudeLongitude gPS)
        {

            var cordinate = Math.Round((double.Parse(gPS.ToString().Replace('"', ' ').Split(new char[] { '°', '\'' }).Select((x, counter) => Double.Parse(x) / Math.Pow(60, counter)).Sum().ToString())), 7);
            return cordinate;
        }
    }

}
