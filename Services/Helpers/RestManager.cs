using Viq.AccessPoint.TestHarness.Services.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Viq.AccessPoint.TestHarness.Services.Helpers
{
    public class RestManager
    {
        /// <summary>Put the specified endpoint.</summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="body">The body.</param>
        /// <param name="headers">The headers.</param>
        public virtual async Task<TResult> Put<TResult>(string endpoint, object body, Dictionary<string, string> headers = null, ModelType modelOptions = 0)
        {
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(endpoint);

                if (headers != null && headers.Keys.Any())
                {
                    foreach (var header in headers)
                    {
                        if (header.Key == "Bearer")
                        {
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", header.Value);
                        }
                        else
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }
                }

                var httpResponse = await client.PutAsync(new Uri(endpoint), new JsonContent(body));

                var json = await httpResponse.Content.ReadAsStringAsync();
                //json = json.Replace("\\", "");
                //json = json.Substring(1, json.Length - 2);

                var result = JsonConvert.DeserializeObject<TResult>(json);
                return result;
            }
            catch (Exception ex)
            {
                //ex.LogError(  $"Endpoint '{endpoint}'");
                return  JsonConvert.DeserializeObject<TResult>(ex.Message);
            }
            finally
            {
                //logger.Log();
            }

        }

        /// <summary>Posts the specified endpoint.</summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="body">The body.</param>
        /// <param name="headers">The headers.</param>
        public virtual async Task<TResult> Post<TResult>(string endpoint, object body, Dictionary<string, string> headers = null, ModelType modelOptions = 0)
        {
            //var logger = new LogJsonRequest(endpoint, body);
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(endpoint);

                if (headers != null && headers.Keys.Any())
                {
                    foreach (var header in headers)
                    {
                        switch (header.Key)
                        {
                            case "Bearer":
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", header.Value);
                                break;
                            default:
                                client.DefaultRequestHeaders.Add(header.Key, header.Value);
                                break;
                        }
                    }
                }

                var httpResponse = await client.PostAsync(new Uri(endpoint), new JsonContent(body));

                var json = await httpResponse.Content.ReadAsStringAsync();

                TResult result = default;
                switch (modelOptions)
                {
                    case ModelType.WebPortalApiResponse:
                        json = json.Replace("\\", "");
                        json = json[1..^1];
                        result = JsonConvert.DeserializeObject<TResult>(json);
                        break;

                    case ModelType.None:
                    default:

                        result = JsonConvert.DeserializeObject<TResult>(json);
                        break;
                }
                return result;
                //logger.AddResponse(result);

            }
            catch (Exception ex)
            {
                //ex.LogError($"Endpoint '{endpoint}'");
                return default;
            }
            finally
            {
                //logger.Log();
            }

        }

        /// <summary>Gets the specified endpoint.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="requestPath">The request path.</param>
        public virtual async Task<T> Get<T>(string endpoint, string requestPath, Dictionary<string, string> headers = null)
        {
            //var logger = new LogJsonRequest(endpoint, requestPath);
            T data;
            try
            {
                using var client = new HttpClient();
                if (string.IsNullOrEmpty(requestPath))
                {
                    data = default;
                    return data;
                }

                if (headers != null && headers.Keys.Any())
                {
                    foreach (var header in headers)
                    {
                        switch (header.Key)
                        {
                            case "Bearer":
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", header.Value);
                                break;
                            default:
                                client.DefaultRequestHeaders.Add(header.Key, header.Value);
                                break;
                        }
                    }
                }

                client.BaseAddress = new Uri(endpoint);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(requestPath);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    //Logger.LogWarning($"Endpoint:{endpoint} respond with:{response.StatusCode}");
                    data = default;
                    return data;
                }
                data = JsonConvert.DeserializeObject<T>(json);
                //logger.AddResponse(data);
                return data;
            }
            catch (Exception ex)
            {
                //ex.LogError($"Endpoint '{endpoint}'");
                data = default;
                return data;
            }
            finally
            {
                //logger.Log();
            }
        }
    }
}
