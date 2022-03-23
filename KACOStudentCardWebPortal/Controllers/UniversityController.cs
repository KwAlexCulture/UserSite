using System;
//using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using BOL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KACOStudentCardWebPortal.Models;

namespace KACOStudentCardWebPortal.Controllers
{
    [Route("[controller]/[action]")]
    [EnableCors("AllowCors")]
    public class UniversityController : Controller
    {
        private readonly UniversityService _universityService;

        public UniversityController(UniversityService universityService)
        {
            _universityService = universityService;
        }
        public IActionResult Index()
        {
            return View();
        }

        // Admin Site Methods //
        [HttpGet]
        public IActionResult GetUniversitiesList()
        {
            try
            {
                IEnumerable<University> universitiesList = _universityService.List().ToList().Where(m => m.StatusCode == true).ToList();
                if (universitiesList != null)
                {
                    return Json(universitiesList);
                }
                else
                {
                    return new JsonResult(new { status = "universitiesList list is null" });
                }
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                return new JsonResult(new { status = "failed" });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetUniversityDetails(Guid id)
        {
            try
            {
                var universityDetails = _universityService.Where(m => m.StatusCode == true).Where(s => s.UniversityId == id).SingleOrDefault();
                if (universityDetails != null)
                {
                    return Json(universityDetails);
                }
                else
                {
                    return new JsonResult(new { status = "studens is null" });
                }
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                return new JsonResult(new { status = "failed" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostUniversitydDetails([FromBody] University University)
        {
            try
            {
                University.StatusCode = true;
                await _universityService.SaveAsync(University);
                return new JsonResult(new { status = "saving succeeded" });
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                return new JsonResult(new { status = "failed" });
            }
        }
    }
}
