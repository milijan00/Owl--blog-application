using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.DataAccess;
using BlogApp.Implementations.UseCases.Ef;
using BlogApp.Application.UseCases.Commands;
using BlogApp.Application.UseCases.Queries;
using BlogApp.Domain;
using BlogApp.Implementations.Validators;
using BlogApp.API.Extensions;
using BlogApp.API.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BlogApp.Application.Emails;
using BlogApp.Implementations.Emails;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace BlogApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BlogApp.API", Version = "v1" });
            });

            services.AddTransient<BlogAppDbContext>();
            //services.AddDbContext<BlogAppDbContext>();
            services.AddTransient<TokenRequest>();

            var appSettings = new AppSettings();
            Configuration.Bind(appSettings);
            services.AddSingleton(appSettings);

            var settings = new JwtSettings();
            services.AddJwt(appSettings);
            services.AddActionUser();
            services.AddHttpContextAccessor();

            //services.AddTransient<IActionUser, ActionUser>();

            services.AddValidators();
            services.AddUseCases();
            services.AddAutoMapper(typeof(Startup));
            services.CreateMaps();

            services.AddTransient<IEmailSender>(x => new SmtpEmailSender(appSettings.EmailFrom, appSettings.EmailPassword));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlogApp.API v1"));
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            CorsPolicyBuilder c = new CorsPolicyBuilder();
            app.UseCors(c => {
                c.AllowAnyMethod();
                c.AllowAnyHeader();
                c.AllowAnyOrigin();
            });
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
