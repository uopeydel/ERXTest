using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ERXTest.Server.DataAccess;
using ERXTest.Server.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using AutoMapper;
using ERXTest.Server.Mapper;

namespace ERXTest.Server
{
    public class Startup
    {
        public const string OriginName = "ERXTestTEST";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var baseConStr = Configuration["ConnectionStrings:ERXTestSystemDbConnection"];
            var basePassword = Configuration["ConnectionStrings:ERXTestSystemPassword"];
            var constr = string.Format(baseConStr, basePassword);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(constr + "constr");
            Console.ResetColor();
            services.AddDbContext<ERXTestContext>(
                options =>
                     options.UseSqlServer(constr));

            
            services.AddCors(options =>
            {
                options.AddPolicy(OriginName,
                    builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            }); 
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddIoc(Configuration);
            services.AddControllersWithViews().AddNewtonsoftJson();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ERXTest.Server", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ERXTest.Server v1"));
            }

            //app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseCors(OriginName);
            app.UseRouting();

            //app.UseAuthorization();
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
