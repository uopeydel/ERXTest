using AutoMapper;

using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ERXTest.Server.Facade;
using ERXTest.Server.Repositories;
using ERXTest.Server.Services;

namespace ERXTest.Server.Helper
{

    public static class ServiceExtensions
    {
        public static void AddIoc(this IServiceCollection services, IConfiguration Configuration)
        {
            #region Transient

            //services.AddTransient<IValidator<DocumentCreateContract>, DocumentCreateContractValidation>();


            services.AddSingleton<IMemoryCache, MemoryCache>();
            //services.AddSingleton<MemoryCacheWithPolicy>();
            services.AddMemoryCache();

            #endregion


            #region Scoped 
             
            services.AddScoped<QuestionFacade>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();

            services.AddScoped<RespondentFacade>();
            services.AddScoped<IRespondentService, RespondentService>();
            services.AddScoped<IRespondentRepository, RespondentRepository>();

            services.AddScoped<DropDownFacade>();
            services.AddScoped<IDropDownService, DropDownService>();
            services.AddScoped<IDropDownRepository, DropDownRepository>();



            services.AddScoped<ILoggerService, LoggerService>(); 


            #endregion


            #region Singleton

            //var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile(Configuration)); });
            //IMapper mapper = mappingConfig.CreateMapper();

            //services.AddSingleton(mapper);
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #endregion

            services.AddHttpClient();
        }
    }
}
