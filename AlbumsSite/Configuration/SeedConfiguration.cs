using AlbumsSite.Entities;
using AlbumsSite.Helper;
using AlbumsSite.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Configuration
{
    public static class SeedConfiguration
    {
        public static async Task SetDefaultUsers(ApplicationContext repository)
        {
            var users = repository.Users.SingleOrDefault(x => x.Role == RoleName.Administrator);

            if (users != null)
                return;

            var administratorUsers = new User()
            {
                Email = "admin@gmail.com",
                Password = SecurityHelper.ComputeSha256Hash("admin"),
                Role = RoleName.Administrator,
                UserLogin = "admin"
            };

            await repository.Users.AddAsync(administratorUsers);
            await repository.SaveChangesAsync();
        }
        public static void SetDefaultFolders(IWebHostEnvironment env)
        {
            string pathToPhoto = Path.Combine(env.WebRootPath, "Photo");
            Directory.CreateDirectory(pathToPhoto);

            string pathToAlbumThumbail = Path.Combine(pathToPhoto, "AlbumThumbail");
            Directory.CreateDirectory(pathToAlbumThumbail);

            string pathToMedium = Path.Combine(pathToPhoto, "Medium");
            Directory.CreateDirectory(pathToMedium);

            string pathToOriginal = Path.Combine(pathToPhoto, "Original");
            Directory.CreateDirectory(pathToOriginal);

            string pathToPlug = Path.Combine(pathToPhoto, "Plug");
            Directory.CreateDirectory(pathToPlug);

            string pathToThumbail = Path.Combine(pathToPhoto, "Thumbail");
            Directory.CreateDirectory(pathToThumbail);

        }
        public static async Task SetDefaultJobs(ApplicationContext repository)
        {
            var users = repository.JobMessages.Where(_ => true);

            if (users.Count() > 0)
                return;

            var job = new JobMessage()
            {
                JobName=JobName.DeletePhotoWhithoutAlbum,
                NextRun=DateTime.Now,
            };
            await repository.JobMessages.AddAsync(job);
            await repository.SaveChangesAsync();

        }

    }
}
