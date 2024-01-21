using ImageResizerApi.Data;
using ImageResizerApi.Model;
using Microsoft.EntityFrameworkCore;

namespace ImageResizerApi.Service
{
    public class ImageStore
    {
        public async Task<IEnumerable<ImageSizings>> GetImageSizings()
        {
            throw new NotImplementedException();
            //return await DB.Find<ImageSizings>().ExecuteAsync();
        }

        public async Task<IEnumerable<Image>> GetImages(Context _ctx)
        {
            var Images =  await _ctx.Images.Include(e => e.Sizes.OrderBy(e => e.Width)).ToListAsync();
            Images.ForEach(image =>
            {
                foreach (var e in image.Sizes)
                {
                    e.Image = null;
                }
            });
            return Images;

        }
    }
}
