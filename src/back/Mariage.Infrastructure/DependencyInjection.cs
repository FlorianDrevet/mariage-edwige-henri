using System.Text;
using Mariage.Application.Common.Interfaces.Authentication;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Application.Common.Interfaces.Services;
using Mariage.Infrastructure.Authentication;
using Mariage.Infrastructure.Persistence;
using Mariage.Infrastructure.Persistence.Repositories;
using Mariage.Infrastructure.Services;
using Mariage.Infrastructure.Services.BlobService;
using Mariage.Infrastructure.Services.DiscordService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

namespace Mariage.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(ConfigurationManager builderConfiguration)
        {
            services
                .AddAuth(builderConfiguration)
                .AddBlob(builderConfiguration)
                .AddDiscordWebhook(builderConfiguration)
                .AddDbContext<MariageDbContext>((sp, options) =>
                    {
                        var dataSource = sp.GetServices<NpgsqlDataSource>();
                        options.UseSqlServer(dataSource.First().ConnectionString);
                    }
                )
                .AddRepositories();
        
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        
            return services;
        }

        private IServiceCollection AddRepositories()
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGiftRepository, GiftRepository>();
            services.AddScoped<IPictureRepository, PictureRepository>();
            return services;
        }

        private IServiceCollection AddBlob(ConfigurationManager builderConfiguration)
        {
            var blobSettings = new BlobSettings();
            builderConfiguration.Bind(BlobSettings.SectionName, blobSettings);

            services.AddSingleton(Options.Create(blobSettings));
            services.AddSingleton<IBlobService, BlobService>();
            return services;
        }

        private IServiceCollection AddDiscordWebhook(ConfigurationManager builderConfiguration)
        {
            var discordWebhookSettings = new DiscordWebhookSettings();
            builderConfiguration.Bind(DiscordWebhookSettings.SectionName, discordWebhookSettings);

            services.AddSingleton(Options.Create(discordWebhookSettings));
            services.AddSingleton<IDiscordWebhook, DiscordWebhook>();
            return services;
        }

        private IServiceCollection AddAuth(ConfigurationManager builderConfiguration)
        {
            var jwtSettings = new JwtSettings();
            builderConfiguration.Bind(JwtSettings.SectionName, jwtSettings);

            services.AddSingleton(Options.Create(jwtSettings));
            services.AddSingleton<IJwtGenerator, JwtGenerator>();
            services.AddSingleton<IHashPassword, HashPassword>();
            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Secret)
                    ),
                });
            return services;
        }
    }
}