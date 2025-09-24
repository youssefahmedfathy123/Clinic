using Application.Helpers;
using Application.Interfaces;
using Application.Validators;
using Domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MyProject.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services,IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("Clinic"));
            });


            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


           

           //.AddFacebook(facebookOptions =>
           //{
           //    facebookOptions.AppId = config["Authentication:Facebook:AppId"];
           //    facebookOptions.AppSecret = config["Authentication:Facebook:AppSecret"];
           //});


            services.AddScoped<ICloudinaryService,CloudinaryService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IOAuthService, OAuthService>();

            services.AddScoped<IJwt, Jwt>();

            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));


            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();

            services.AddFluentValidationAutoValidation(config =>
            {
                config.DisableDataAnnotationsValidation = true;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
          .AddJwtBearer(options =>
          {
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                  ValidIssuer = config["Jwt:Issuer"],
                  ValidAudience = config["Jwt:Audience"],
                  IssuerSigningKey = new SymmetricSecurityKey(
                      Encoding.UTF8.GetBytes(config["Jwt:Key"]))
              };
          })
          .AddGoogle("Google", googleOptions =>
          {
              googleOptions.ClientId = config["Authentication:Google:ClientId"];
              googleOptions.ClientSecret = config["Authentication:Google:ClientSecret"];

              googleOptions.Scope.Add("openid");
              googleOptions.Scope.Add("profile");
              googleOptions.Scope.Add("email");

              googleOptions.SaveTokens = true;
          });



            return services;
        }
    }
}


