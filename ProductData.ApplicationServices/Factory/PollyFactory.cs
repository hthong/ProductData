using System;
using System.Linq;
using Polly;
using RestSharp;

namespace ProductData.ApplicationServices.Factory
{
    public static class PollyFactory
    {
        private const int Retry = 3;

        public static ISyncPolicy<IRestResponse<T>> CreateGetPolicy<T>() =>
            Policy
                .HandleResult<IRestResponse<T>>(res =>
                    new[] {0, 408, 500, 502, 503, 504}.Contains((int) res.StatusCode))
                .WaitAndRetry(Retry, i => TimeSpan.FromSeconds(Math.Pow(2, i)));
    }
}
