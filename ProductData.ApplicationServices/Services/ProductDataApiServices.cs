using System;
using System.Collections.Generic;
using System.Net;
using ProductData.ApplicationServices.Entity;
using ProductData.ApplicationServices.Factory;
using ProductData.ApplicationServices.Interface;
using RestSharp;

namespace ProductData.ApplicationServices.Services
{
    public class ProductDataApiServices : IProductDataApiServices
    {
        private readonly IRestClient _client;
        private readonly string _baseUri;
      
        public ProductDataApiServices(IRestClient client, string baseUri)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client), "Rest Client is null.");
            _baseUri = string.IsNullOrEmpty(baseUri) 
                ? throw new ArgumentNullException(nameof(baseUri), "Base Uri is empty.")
                : baseUri;
            
            _client.BaseUrl = new Uri(baseUri);
        }

        public IList<MaxPriceItemByName> GetHighestCostItems()
        {
            const string uri = "/items/group/name/agg/max(price)";
            var response = PollyFactory
                .CreateGetPolicy<IList<MaxPriceItemByName>>()
                .Execute(() => _client.Execute<IList<MaxPriceItemByName>>(new RestRequest(uri, Method.GET)));

            return response.IsSuccessful ? response.Data : throw response.ErrorException;

        }

        public MaxPriceItemByName GetHighestCostItemByName(string name)
        {
            var uri = $"/items/group/name/agg/max(price)?name={name}";
            var response = PollyFactory
                .CreateGetPolicy<MaxPriceItemByName>()
                .Execute(() => _client.Execute<MaxPriceItemByName>(new RestRequest(uri, Method.GET)));

            return response.IsSuccessful || response.StatusCode == HttpStatusCode.NotFound 
                ? response.Data 
                : throw response.ErrorException;
        }
    }
}
