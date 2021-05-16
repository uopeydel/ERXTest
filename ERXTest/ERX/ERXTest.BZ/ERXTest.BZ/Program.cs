using Blazored.Modal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ERXTest.BZ.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ERXTest.BZ
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services 
                .AddScoped<IQuestionService, QuestionService>()   
                .AddScoped<IRespondentService, RespondentService>() 
               .AddScoped<IDropDownService, DropDownService>()  
               .AddScoped<IHttpService, HttpService>()
                .AddScoped<ILocalStorageService, LocalStorageService>();

            Console.WriteLine("apiUrl " + builder.Configuration["apiUrl"]);

            builder.Services.AddScoped(x =>
            { 
                var apiUrl = new Uri(builder.Configuration["apiUrl"]); 
                return new HttpClient() { BaseAddress = apiUrl };
            });

               

            builder.Services.AddBlazoredModal();
            await builder.Build().RunAsync();
        }
    }
}
