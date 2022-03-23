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
    public class CollegeController : Controller
    {
        private readonly CollegeService _collegeService;

        public CollegeController(CollegeService collegeService)
        {
            _collegeService = collegeService;
        }
        public IActionResult Index()
        {
            return View();
        }

        // Admin Site Methods //
        [HttpGet]
        public IActionResult GetCollegesList()
        {
            try
            {
                IEnumerable<College> collegesList = _collegeService.List().ToList().Where(m => m.StatusCode == true).ToList();
                if (collegesList != null)
                {
                    return Json(collegesList);
                }
                else
                {
                    return new JsonResult(new { status = "collegesList list is null" });
                }
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                return new JsonResult(new { status = "failed" });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCollegeDetails(Guid id)
        {
            try
            {
                var collegeDetails = _collegeService.Where(m => m.StatusCode == true).Where(s => s.CollegeId == id).SingleOrDefault();
                if (collegeDetails != null)
                {
                    return Json(collegeDetails);
                }
                else
                {
                    return new JsonResult(new { status = "college is null" });
                }
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                return new JsonResult(new { status = "failed" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostCollegeDetails([FromBody] College College)
        {
            try
            {
                College.StatusCode = true;
                await _collegeService.SaveAsync(College);
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
