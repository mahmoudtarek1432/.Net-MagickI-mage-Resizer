

using System.Text.Json.Serialization;

namespace ImageResizerApi.Model
{
    public class ImageSizings 
    {
        public int Id { get; set; }
        [JsonPropertyName("height")]
        public int Height { get; set; }
        [JsonPropertyName("width")]
        public int Width { get; set; }

        public int ImageId { get; set; }
        public Image Image { get; set; }
    }
}
