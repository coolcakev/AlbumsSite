using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Helper
{
    public static class PhotoManagerHelper
    {
        public async static Task<byte[]> ResizePhoto(IFormFile file, int width, int height)
        {
            byte[] photoBytes = new byte[2];
            using (var ms = new MemoryStream())
            {

                await file.CopyToAsync(ms);
                photoBytes = ms.ToArray();
            }

            // Format is automatically detected though can be changed.
            ISupportedImageFormat format = new JpegFormat { Quality = 1000 };
            Size size = new Size(width, height);
            var fileBytes = new byte[2];
            using (MemoryStream inStream = new MemoryStream(photoBytes))
            {
                
                using (MemoryStream outStream = new MemoryStream())
                {
                    // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        // Load, resize, set the format and quality and save an image.
                        imageFactory.Load(inStream)
                                    .Resize(size)
                                    .Format(format)
                                    .Save(outStream);
                    };
                    fileBytes = outStream.ToArray();
                }

            }
            return fileBytes;
        }
    }
}
