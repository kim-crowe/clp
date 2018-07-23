using System;
using System.Threading.Tasks;
using CogsLite.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cogslite.Pages
{
    public class ImageModel : PageModel
    {
        private IImageStore _imageStore;

        public ImageModel(IImageStore imageStore)
        {
            _imageStore = imageStore ?? throw new ArgumentNullException(nameof(imageStore));
        }

        public async Task<IActionResult> OnGet(Guid imageId)
        {
            var storedImage = await _imageStore.Get(imageId);
            if (storedImage == null)
                return File("~/images/cogs.png", "application/octet-stream");
            else
                return new FileContentResult(storedImage.Data, "application/octet-stream");
        }
    }
}