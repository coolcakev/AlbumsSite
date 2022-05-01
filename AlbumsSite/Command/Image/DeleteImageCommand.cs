using AlbumsSite.Models;
using AlbumsSite.Models.Image;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Command.Image
{
    public interface IDeleteImageCommand
    {
        Task ExecuteAsync(Picture picture);
    }
    public class DeleteImageCommand : IDeleteImageCommand
    {
        private readonly ApplicationContext _repository;

        public DeleteImageCommand(ApplicationContext repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(Picture picture)
        {           
            _repository.Pictures.Remove(picture);
            File.Delete(picture.Path);
            File.Delete(picture.PathThubail);
            File.Delete(picture.PathMedium);
            await _repository.SaveChangesAsync();
        }

    }
}
