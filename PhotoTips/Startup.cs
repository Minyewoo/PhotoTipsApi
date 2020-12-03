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
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(opts => { opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

            services.AddSwaggerGen();

            services.AddMediatR(typeof(Startup).Assembly, typeof(FrontofficeBaseController).Assembly
                , typeof(BackofficeBaseController).Assembly);

            services.AddDbContext<PhotoTipsDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("PhotoTipsDb")));
            
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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"));

            app.UseRouting();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}