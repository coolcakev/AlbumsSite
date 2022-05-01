using AlbumsSite.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Jobs
{
    public interface IDeletePhotoWhithoutAlbum : IJob
    {
    }
    public class DeletePhotoWhithoutAlbum : IDeletePhotoWhithoutAlbum
    {
        private readonly ApplicationContext _repository;

        public DeletePhotoWhithoutAlbum(ApplicationContext reposetory, IServiceProvider serviceProvider)
        {
            _repository = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ApplicationContext>();
        }
        public async Task ExecuteAsync()
        {
            var photos = _repository.Pictures.Include(x => x.PictureAlbums).Where(x => x.PictureAlbums.Count == 0);

            foreach (var item in photos)
            {
                _repository.Pictures.Remove(item);
                File.Delete(item.Path);
                File.Delete(item.PathThubail);
                File.Delete(item.PathMedium);
            }
            _repository.RemoveRange(photos);
            await _repository.SaveChangesAsync();


        }
    }
}
