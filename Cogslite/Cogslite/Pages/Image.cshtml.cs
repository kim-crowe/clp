using System;
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

        public IActionResult OnGet(Guid imageId)
        {
            var storedImage = _imageStore.Get(imageId);
            if (storedImage == null)
                return File("~/images/cogs.png", "application/octet-stream");
            else
                return new FileContentResult(storedImage.Data, "application/octet-stream");
        }
    }
}