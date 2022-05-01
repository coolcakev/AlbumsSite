using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Helper
{
    public static class FileManagerHelper
    {
        public async static Task CreateFile( IFormFile uploadedFile,string path) 
        {
            var fileBytes = new byte[2];
            using (var ms = new MemoryStream())
            {

               await uploadedFile.CopyToAsync(ms);
                fileBytes = ms.ToArray();
            }
            using FileStream fs = File.Create(path);
            await fs.WriteAsync(fileBytes.AsMemory(0, fileBytes.Length));

        }
        public async static Task CreateFile(byte[] photoBytes, string pathTo)
        {

            using FileStream fs = File.Create(pathTo);
            await fs.WriteAsync(photoBytes.AsMemory(0, photoBytes.Length));

        }
    }
}
