using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UniversalBoredGames
{
    public static class ServiceClient
    {
       

        //////////////////////////////////////////////SELECTS/////////////////////////////////////////////////////////////
        internal async static Task<clsPublisher> GetPublishersAsync(string prPublisherName)
        {
            using (HttpClient lcHttpClient = new HttpClient())
                return JsonConvert.DeserializeObject<clsPublisher>
                    (await lcHttpClient.GetStringAsync("http://localhost:60064/api/BoredGames/GetPublishers?Name=" + prPublisherName));
        }
        internal async static Task<List<string>> GetPublisherNamesAsync()
        {
            using (HttpClient lcHttpClient = new HttpClient())
                return JsonConvert.DeserializeObject<List<string>>
                        (await lcHttpClient.GetStringAsync("http://localhost:60064/api/BoredGames/GetPublisherName/"));
        }
        internal async static Task<object> GetOrderInfoAsync()
        {
            using (HttpClient lcHttpClient = new HttpClient())
                return JsonConvert.DeserializeObject<List<string>>
                        (await lcHttpClient.GetStringAsync("http://localhost:60064/api/BoredGames/GetOrderInfo/"));
        }

        internal async static Task<clsOrder> GetOrderAsync(string prOrderID)
        {
            using (HttpClient lcHttpClient = new HttpClient())
                return JsonConvert.DeserializeObject<clsOrder>
                    (await lcHttpClient.GetStringAsync("http://localhost:60064/api/BoredGames/GetOrder?OrderID=" + prOrderID));
        }

        //////////////////////////////////////////////INSERTS/////////////////////////////////////////////////////////////
        internal async static Task<string> InsertPublisherAsync(clsPublisher prPublisher)
        {
            return await InsertOrUpdateAsync(prPublisher, "http://localhost:60064/api/BoredGames/PostPublisher", "POST");
        }
        internal async static Task<string> InsertOrderAsync(clsOrder prOrder)
        {
            return await InsertOrUpdateAsync(prOrder, "http://localhost:60064/api/BoredGames/PostOrder", "POST");
        }
        internal async static Task<string> InsertGameAsync(clsAllGames prWork)
        {
            return await InsertOrUpdateAsync(prWork, "http://localhost:60064/api/BoredGames/PostGames", "POST");
        }
        private async static Task<string> InsertOrUpdateAsync<TItem>(TItem prItem, string prUrl, string prRequest)
        {
            using (HttpRequestMessage lcReqMessage = new HttpRequestMessage(new HttpMethod(prRequest), prUrl))
            using (lcReqMessage.Content = new StringContent(JsonConvert.SerializeObject(prItem), Encoding.UTF8, "application/json"))
            using (HttpClient lcHttpClient = new HttpClient())
            {
                HttpResponseMessage lcRespMessage = await lcHttpClient.SendAsync(lcReqMessage); return await lcRespMessage.Content.ReadAsStringAsync();
            }
        }
        //////////////////////////////////////////////UPDATES/////////////////////////////////////////////////////////////
        internal async static Task<string> UpdatePublisherAsync(clsPublisher prArtist)
        {
            return await InsertOrUpdateAsync(prArtist, "http://localhost:60064/api/BoredGames/PutPublisher", "PUT");
        }
        internal async static Task<string> UpdateGameAsync(clsAllGames prWork)
        {
            return await InsertOrUpdateAsync(prWork, "http://localhost:60064/api/BoredGames/PutGame", "PUT");
        }
        //////////////////////////////////////////////DELETES/////////////////////////////////////////////////////////////
        internal async static Task<string> DeletePublisherAsync(string prName)
        {
           using (HttpClient lcHttpClient = new HttpClient())
            {
                HttpResponseMessage lcRespMessage = await lcHttpClient.DeleteAsync
                    ($"http://localhost:60064/api/BoredGames/DeletePublisher?Name=" + prName);

                return await lcRespMessage.Content.ReadAsStringAsync();
            }
        }   
        internal async static Task<string> DeleteGamesAsync(clsAllGames prWork)
        {
            using (HttpClient lcHttpClient = new HttpClient())
            {
                HttpResponseMessage lcRespMessage = await lcHttpClient.DeleteAsync
                    ($"http://localhost:60064/api/BoredGames/DeleteArtWork?Name={prWork.Name}&PublisherName={prWork.PublisherName}");

                return await lcRespMessage.Content.ReadAsStringAsync();
            }
        }
        internal async static Task<string> DeleteOrderAsync(string prOrderID)
        {
            using (HttpClient lcHttpClient = new HttpClient())
            {
                HttpResponseMessage lcRespMessage = await lcHttpClient.DeleteAsync
                    ($"http://localhost:60064/api/BoredGames/DeleteOrder?ID=" + prOrderID);

                return await lcRespMessage.Content.ReadAsStringAsync();
            }
        }
    }

}

