using System.Collections.Generic;
using System.Threading.Tasks;
using App.Common.Models;

namespace App.Common.Abstractions.Utilities
{
    public interface IHttpAPICallService
    {
        Task<TModel> PostFormUrlEncodedContentAndReadFromJsonAsync<TModel>
        (
            string url,
            List<KeyValuePair<string, string>> formUrlEncodedData,
            TModel defaultValue = null
        ) where TModel : class;

        Task<TModel> PostFormUrlEncodedContentAndReadAsStringAsync<TModel>
        (
            string url,
            List<KeyValuePair<string, string>> formUrlEncodedData,
            TModel defaultValue = null
        ) where TModel : class;

        Task<(bool isSuccess, string data)> PostMultipartFormDataContentAndReadAsStringAsync
        (
            string url,
            List<HttpContentDto> contents
        );
    }
}
