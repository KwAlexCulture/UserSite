using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using BOL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KACOStudentCardWebPortal.Controllers
{
    [Route("[controller]/[action]")]
    [EnableCors("AllowCors")]
    public class ImageController : Controller
    {
        private readonly ImageService _imageService;

        public ImageController(ImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(Guid id)
        {
            try
            {
                Image image = await _imageService.FindAsync(id);
                if (image.ImageData != null)
                {
                    return File(image.ImageData, image.ImageType);
                }
                else if (image.StatusCode != true)
                {
                    return new JsonResult(new { status = "inactive image status" });
                }
                else
                {
                    return new JsonResult(new { status = "not found" });
                }
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                return new JsonResult(new { status = "failed" });
            }
        }

        [RequestSizeLimit(100_000_000)]
        [HttpPost]
        public async Task<Guid?> PostImage(IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    if (file.Length > 0)
                    {
                        byte[] buffer = new byte[file.Length];
                        file.OpenReadStream().Read(buffer, 0, (int)file.Length - 1);

                        Image image = new Image();
                        image.ImageData = buffer;
                        image.ImageType = file.ContentType;
                        image.StatusCode = true;
                        await _imageService.SaveAsync(image).ConfigureAwait(true);

                        // return null;
                        return image.ImageId;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeactivateImage(Image image)
        {
            Image thisImage = await _imageService.FindAsync(image.ImageId);
            image.StatusCode = false;
            return new JsonResult(new { status = "deactivate success" });
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
