using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ITI.PrimarySchool.WebApp.Services
{
    public class GitHubClient
    {
        public async Task<IEnumerable<string>> GetFollowedUsers( string githubAccessToken )
        {
            using( HttpClient client = new HttpClient() )
            {
                HttpRequestHeaders headers = client.DefaultRequestHeaders;
                headers.Add( "Authorization", string.Format( "token {0}", githubAccessToken ) );
                headers.Add( "User-Agent", "PrimarySchool" );
                HttpResponseMessage response = await client.GetAsync( "https://api.github.com/user/following" );

                using( TextReader tr = new StreamReader( await response.Content.ReadAsStreamAsync() ) )
                using( JsonTextReader jsonReader = new JsonTextReader( tr ) )
                {
                    JToken json = JToken.Load( jsonReader );
                    return json.Select( u => ( string )u[ "login" ] ).ToList();
                }
            }
        }

        public async void FollowStudent(string githubAccessToken, string nameOfStudent)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestHeaders headers = client.DefaultRequestHeaders;
                headers.Add("Authorization", string.Format("token {0}", githubAccessToken));
                headers.Add("User-Agent", "PrimarySchool");
                var jsonString = "{\"appid\":1,\"platformid\":1,\"rating\":3}";
                HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("https://api.github.com/user/following/" + nameOfStudent, httpContent);
                
            }
        }

        public async Task<IEnumerable<string>> FindInFollowedList(string githubAccessToken, string nameOfStudent)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestHeaders headers = client.DefaultRequestHeaders;
                headers.Add("Authorization", string.Format("token {0}", githubAccessToken));
                headers.Add("User-Agent", "PrimarySchool");
                HttpResponseMessage response = await client.GetAsync("https://api.github.com/user/following/" + nameOfStudent);

                using (TextReader tr = new StreamReader(await response.Content.ReadAsStreamAsync()))
                using (JsonTextReader jsonReader = new JsonTextReader(tr))
                {
                    JToken json = JToken.Load(jsonReader);
                    return json.Select(u => (string)u["login"]).ToList();
                }
            }
        }
    }
}