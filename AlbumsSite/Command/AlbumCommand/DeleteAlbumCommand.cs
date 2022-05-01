using AlbumsSite.Models;
using AlbumsSite.Models.AlbumModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Command.AlbumCommand
{
    public interface IDeleteAlbumCommand
    {
        Task ExecuteAsync(Album album);
    }
    public class DeleteAlbumCommand : IDeleteAlbumCommand
    {
        private readonly ApplicationContext _repository;

        public DeleteAlbumCommand(ApplicationContext repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(Album album)
        {           
            if (album.SmallThumbailImagePath != "\\Photo\\Plug\\240x240.jpeg")
                File.Delete(album.ThumbailImagePath);

            _repository.Albums.Remove(album);
            await _repository.SaveChangesAsync();
        }

    }
}
