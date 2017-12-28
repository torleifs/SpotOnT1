using System;
using System.Collections.Generic;
using Refit;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace SpotOnT1
{
    public class Image {
       /* [JsonProperty(PropertyName = "height")]
        public int height { get; set; }
        [JsonProperty(PropertyName = "width")]
        public int width { get; set; }*/
        [JsonProperty(PropertyName = "url")]
        public string url { get; set; }
                      
    }
    public class Tracks {
        [JsonProperty(PropertyName = "href")]
        public string href { get; set; }

        [JsonProperty(PropertyName = "total")]
        public int total { get; set; }
    }

   public class PlayList {
        [JsonProperty(PropertyName = "collaborative")]
        public bool collaborative { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string name { get; set; }

        [JsonProperty(PropertyName = "tracks")]
        public Tracks tracks { get; set; }
    }

    public class PlayLists {
        [JsonProperty(PropertyName = "href")]
        public string href { get; set; }

        [JsonProperty(PropertyName = "items")]
        public List<PlayList> items { get; set; }
    }
    public class User {
        [JsonProperty(PropertyName = "country")]
        public string country { get; set; }

        [JsonProperty(PropertyName = "display_name")]
        public string displayName { get; set; }

        [JsonProperty(PropertyName = "birthdate")]
        public string birthDate { get; set; }

       
        [JsonProperty(PropertyName = "product")]
        public string product { get; set; }

        [JsonProperty(PropertyName = "images")]
        public List<Image> images { get; set; }
      
    }
    public interface ISpotifyClient
    {
        [Get("/v1/me")]
        Task<User> GetMe([Header("Authorization")] string auth);

        [Get("/v1/me/playlists")]
        Task<PlayLists> GetPlayLists([Header("Authorization")] string auth);
    }
}
