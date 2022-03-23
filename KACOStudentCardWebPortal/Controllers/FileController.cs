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
    public class FileController : Controller
    {
        private readonly FileService _fileService;
        public FileController(FileService fileService)
        {
            _fileService = fileService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile(Guid id)
        {
            try
            {
                File file = await _fileService.FindAsync(id);
                if (file.FileData != null)
                {
                    return File(file.FileData, file.FileType);
                }
                else if (file.StatusCode != true)
                {
                    return new JsonResult(new { status = "inactive file status" });
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
        public async Task<Guid?> PostFile(IFormFile currentFile)
        {
            try
            {
                if (currentFile != null)
                {
                    if (currentFile.Length > 0)
                    {
                        byte[] buffer = new byte[currentFile.Length];
                        currentFile.OpenReadStream().Read(buffer, 0, (int)currentFile.Length - 1);

                        File file = new File();
                        file.FileData = buffer;
                        file.FileType = currentFile.ContentType;
                        file.StatusCode = true;
                        await _fileService.SaveAsync(file).ConfigureAwait(true);

                        // return null;
                        return file.FileId;
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
        public async Task<JsonResult> DeactivateFile(File file)
        {
            File thisFile = await _fileService.FindAsync(file.FileId);
            file.StatusCode = false;
            return new JsonResult(new { status = "deactivate success" });
        }
    }
}
