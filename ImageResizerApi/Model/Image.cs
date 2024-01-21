
namespace ImageResizerApi.Model
{
    public class Image
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string DominantColorHex { get; set; }
        public IEnumerable<ImageSizings> Sizes { get; set; }
    }
}
