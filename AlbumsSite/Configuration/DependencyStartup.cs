using AlbumsSite.Command;
using AlbumsSite.Services;
using Microsoft.AspNetCore.Http;
using AlbumsSite.Services.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlbumsSite.Command.Image;
using AlbumsSite.Command.AlbumCommand;
using AlbumsSite.Jobs;

namespace AlbumsSite.Configuration
{
    public class DependencyStartup
    {
        public static void Configure(IServiceCollection services)
        {
            AddJobs(services);
            addInfrastructure(services);
            AddServices(services);
            AddCommands(services);
        }

        private static void AddJobs(IServiceCollection services)
        {
            services.AddScoped<IDeletePhotoWhithoutAlbum, DeletePhotoWhithoutAlbum>();
        }

        private static void addInfrastructure(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IServiceFactory, ServiceFactory>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
            });
        }

        private static void AddCommands(IServiceCollection services)
        {
            services.AddScoped<IAccountAuthenticateCommand, AccountAuthenticateCommand>();
            services.AddScoped<IParseMetaDataCommand, ParseMetaDataCommand>();
            services.AddScoped<IGetPlaceByGPSCommand, GetPlaceByGPSCommand>();
            services.AddScoped<IDeleteImageCommand, DeleteImageCommand>();
            services.AddScoped<IDeleteAlbumCommand, DeleteAlbumCommand>();
        }
        private static void AddServices(IServiceCollection services)
        {
            services.AddHostedService<ScopedBackgroundService>();
            services.AddScoped<IDeletePhotoJobService, DeletePhotoJobService>();

            services.AddScoped<IAccountAuthenticateService, AccountAuthenticateService>();

            services.AddScoped<IAdminService, BaseAdminService>();
            services.AddScoped<IAdminService, AuthentificateUserAdminService>();
            services.AddScoped<IAdminService, AdministratorAdminService>();

            services.AddScoped<IBaseAlbumService, BaseAlbumService>();
            services.AddScoped<IBaseAlbumService, AuthentificateUserAlbumService>();
            services.AddScoped<IBaseAlbumService, AdministratorAlbumService>();

            services.AddScoped<IBaseHomeService, BaseHomeService>();
            services.AddScoped<IBaseHomeService, AuthentificateUserHomeService>();
            services.AddScoped<IBaseHomeService, AdministratorHomeService>();

            services.AddScoped<IBaseImageService, BaseImageService>();
            services.AddScoped<IBaseImageService, NoneAuthentificateUserImageService>();
            services.AddScoped<IBaseImageService, AdministratorImageService>();

            services.AddScoped<IBaseUserService, BaseUserService>();
            services.AddScoped<IBaseUserService, AuthentificateUserUserService>();
            services.AddScoped<IBaseUserService, AdministratorUserService>();


        }
    }
}
