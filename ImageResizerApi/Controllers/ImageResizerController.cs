using ImageResizerApi.Data;
using ImageResizerApi.Factories;
using ImageResizerApi.Model;
using ImageResizerApi.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

using System.Drawing;
using System.Text.Json;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ImageResizerApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImageResizerController : ControllerBase
    {
        private readonly Context _ctx;

        private IWebHostEnvironment _hostingEnvironment { get; }
        public ImageResizerController(IWebHostEnvironment hostingEnvironment, Context ctx)
        {
            _hostingEnvironment = hostingEnvironment;
            _ctx = ctx;
        }



        [HttpPost("UploadImage")]
        public async Task<ActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string sizingsJson, [FromForm] FillMode fill, [FromForm] string extention)
        { 
            string uploads = Path.Combine(_hostingEnvironment.WebRootPath, "");
            var sizings = JsonSerializer.Deserialize<IEnumerable<ImageSizings>>(sizingsJson);
            //read image byte stream
            var imageBytes = new byte[file.Length];
            var byteStream = file.OpenReadStream();
            byteStream.Read(imageBytes, 0, (int)file.Length);

            foreach (var size in sizings)
            {
                ImageResizerService.resize(imageBytes, size.Width, size.Height, $"{uploads}/{ImageResizerService.RenameResizedFile(file.FileName, size.Width, size.Height)}", fill, extention);
            }

            using (var fileStream = new FileStream($"{uploads}/{file.FileName }", FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var image = new Image
            {
                Sizes = sizings,
                Path = $"{Config.Path}/{file.FileName}",
                DominantColorHex = ImageResizerService.GetDominantColor(imageBytes).ToHexString() ,
            };

            if (!string.IsNullOrEmpty(extention))
            {
                image.Path = CompressionFactory.ChangeExtention(image.Path, extention);
            }

            await _ctx.Images.AddAsync(image);
            await _ctx.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("FetchImages")]
        public async Task<ActionResult> GetImages()
        {

            var store = new ImageStore();
            var images = await store.GetImages(_ctx);
            return Ok(images);
        }
    }
}
