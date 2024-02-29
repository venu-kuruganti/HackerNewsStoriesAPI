using HackerNewsStories.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Web;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HackerNewsStories.Controllers
{  

    [Route("api/[controller]")]
    [ApiController]
    public class HackerNewsOperations : ControllerBase
    {
        protected const string bestStoriesURL = "https://hacker-news.firebaseio.com/v0/beststories.json";
        protected const string getStoryDetailsURL = "https://hacker-news.firebaseio.com/v0/item/";
        protected static List<Story> Stories;
        // GET: api/<HackerNewsOperations>
        [HttpGet]
        public List<Story> GetBestNStories(int n)
        {
            if (Stories == null)
            {
                Stories = new List<Story>();

                //Call API to get the best story Ids.
                List<string> ids = GetBestStoryIDs();

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(getStoryDetailsURL);
                foreach (string storyID in ids)
                {
                    //Create proper parameter
                    string urlParameters = storyID + ".json";

                    //Get story details via API call
                    HttpResponseMessage message = client.GetAsync(urlParameters).Result;
                    if (message.IsSuccessStatusCode)
                    {
                        //Create story object from json
                        Story story = message.Content.ReadFromJsonAsync<Story>().Result;

                        //Add to list
                        Stories.Add(story);
                    }
                }

                //Sort stories according to score in descending order.
                Stories = Stories.OrderByDescending(s => s.score).ToList();
            }

            //Slice array to get 'n' elements and return that as a list.
            return Stories.Take(n).ToList();
        }

        /// <summary>
        /// This function calls the HackerNews API to get the best story IDs.
        /// </summary>
        /// <returns>Story IDs as a List of strings</returns>
        private List<string> GetBestStoryIDs()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(bestStoriesURL);

            List<String> storyIds = new List<string>();

            HttpResponseMessage response = client.GetAsync("").Result;
            if (response.IsSuccessStatusCode)
            {
                string ids = response.Content.ReadAsStringAsync().Result;

                ids = ids.Replace("[", "");
                ids = ids.Replace("]", "");

                storyIds = ids.Split(',').ToList();
            }

            client.Dispose();

            return storyIds;
            
        }

    }
}
