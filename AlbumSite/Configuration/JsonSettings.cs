using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AlbumSite.Configuration
{
    [JsonObject()]
    public class JsonSettings
    {
        [JsonPropertyName("albumsUri")]
        public string AlbumsUri { get; set; }

        [JsonPropertyName("photosUri")]
        public string PhotosUri { get; set; }

        [JsonPropertyName("commentsUri")]
        public string CommentsUri { get; set; }
    }
}
