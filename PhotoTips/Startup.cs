using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PhotoTips.Backoffice.Controllers;
using PhotoTips.Core.Repositories;
using PhotoTips.Frontoffice.Controllers;
using PhotoTips.Infrastructure.Repositories;

namespace PhotoTips
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                                    .AllowAnyMethod()
                                                                     .AllowAnyHeader())); 
            

            services.AddControllers()
                .AddJsonOptions(opts => { opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

            services.AddSwaggerGen();

            services.AddMediatR(typeof(Startup).Assembly, typeof(FrontofficeBaseController).Assembly
                , typeof(BackofficeBaseController).Assembly);

            services.AddScoped<PhotoTipsDbContext>();       
            services.AddScoped<IUserRepository, UserEfRepository>();
            services.AddScoped<IPhotoRepository, PhotoEfRepository>();
            services.AddScoped<ICityRepository, CityEfRepository>();
            services.AddScoped<ILectureContentRepository, LectureContentEfRepository>();
            services.AddScoped<IModuleEntryRepository, ModuleEntryEfRepository>();
            services.AddScoped<IModuleRepository, ModuleEfRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseCors("AllowAll");


            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"));

            app.UseRouting();
            app.UseFileServer();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
