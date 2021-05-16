using ERXTest.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ERXTest.Shared.Helpers;

namespace ERXTest.BZ.Services
{
    public interface IHttpService
    {
        Task<T> Get<T>(string uri, PagingParameters paging = null);
        Task<T> Post<T>(string uri, object value, PagingParameters paging = null);
    }

    public class HttpService : IHttpService
    {
        private HttpClient _httpClient;
        private NavigationManager _navigationManager;
        private ILocalStorageService _localStorageService;
        private IConfiguration _configuration;

        public HttpService(
            HttpClient httpClient,
            NavigationManager navigationManager,
            ILocalStorageService localStorageService,
            IConfiguration configuration
        )
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
            _configuration = configuration;
        }

        public async Task<T> Get<T>(string uri, PagingParameters paging = null)
        {
            var apiSubfix = _configuration["apiSubfix"];
            uri = apiSubfix + uri + ERXTestResponse.GetQueryString(paging);
            //Console.WriteLine(" uri " + uri);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return await sendRequest<T>(request);
        }

        public async Task<T> Post<T>(string uri, object value, PagingParameters paging = null)
        {
            var apiSubfix = _configuration["apiSubfix"];
            uri = apiSubfix + uri + ERXTestResponse.GetQueryString(paging);
            Console.WriteLine(" uri " + uri);
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            return await sendRequest<T>(request);
        }

        // helper methods

        private async Task<T> sendRequest<T>(HttpRequestMessage request)
        {

            using var response = await _httpClient.SendAsync(request);


            // throw exception on error response
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                throw new Exception(error["message"]);
            }
            //var str = await response.Content.ReadAsStringAsync();
            //Console.WriteLine("str > " + str);
            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}