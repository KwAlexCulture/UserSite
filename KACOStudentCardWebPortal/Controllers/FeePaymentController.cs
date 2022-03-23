using System.Linq;
using System.Threading.Tasks;
using BLL;
using BOL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KACOStudentCardWebPortal.Models;
using System;
using System.Collections.Generic;

namespace KACOStudentCardWebPortal.Controllers
{
    [Route("[controller]/[action]")]
    [EnableCors("AllowCors")]
    public class FeePaymentController : Controller
    {
        public readonly FeePaymentService _feePaymentService;
        public readonly SameCollegeSeblingService _sameCollegeSeblingService;
        private readonly CollegeService _collegeService;
        private readonly UniversityService _universityService;
        public FeePaymentController(FeePaymentService feePaymentService, SameCollegeSeblingService sameCollegeSeblingService, CollegeService collegeService, UniversityService universityService)
        {
            _feePaymentService = feePaymentService;
            _sameCollegeSeblingService = sameCollegeSeblingService;
            _universityService = universityService;
            _collegeService = collegeService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PostFeePaymentRequest()
        {
            var getCollegesList = _collegeService.Where(c => c.StatusCode == true).Where(c => c.FeePaymentEntity == true).ToList();
            ViewBag.collegesDetails = getCollegesList;
            var getUniversitiesList = _universityService.Where(u => u.StatusCode == true).Where(u => u.FeePaymentEntity == true).ToList();
            ViewBag.universityDetails = getUniversitiesList;
            return View();
        }

        [HttpPost]
        public async Task<Guid> PostFeePaymentId(Guid feePaymentId)
        {
            return feePaymentId;
        }

        [HttpPost]
        public async Task<IActionResult> PostSameCollegeSeblings(SameCollegeSebling SameCollegeSebling, Guid feePaymentId, FeePayment FeePayment, string IsHavingSiblingsSameCollege)
        {
            try
            {
                if (IsHavingSiblingsSameCollege == "True")
                {
                    if (ModelState.IsValid)
                    {
                        SameCollegeSebling sameCollegeSebling = new SameCollegeSebling()
                        {
                            SeblingFullName = SameCollegeSebling.SeblingFullName,
                            SeblingNationalIDNo = SameCollegeSebling.SeblingNationalIDNo,
                            FeePaymentId = FeePayment.FeePaymentId,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now,
                            StatusCode = true
                        };

                        await _sameCollegeSeblingService.SaveAsync(sameCollegeSebling);
                        ViewBag.Success = "true";
                        return Json(new { err = "" });
                    }
                    else
                    {
                        ViewBag.Success = "true";
                        return Json(new { err = "" });
                    }
                }
                else
                {
                    return null;
                }
                
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                return Json(new { err = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> PostFeePaymentRequest(FeePayment FeePayment, Guid feePaymentId, SameCollegeSebling SameCollegeSebling, string IsHavingSiblingsSameCollege)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    FeePayment feePayment = new FeePayment()
                    {
                        FeePaymentId = await this.PostFeePaymentId(feePaymentId),
                        FullName = FeePayment.FullName,
                        NationalIDNo = FeePayment.NationalIDNo,
                        CollegeId = FeePayment.CollegeId,
                        UniversityId = FeePayment.UniversityId,
                        EducationLevel = FeePayment.EducationLevel,
                        IsHavingSiblingsSameCollege = FeePayment.IsHavingSiblingsSameCollege,
                        RequestStatus = "تحت المعاينة",
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        StatusCode = true
                    };
                    //await this.PostSameCollegeSeblings(SameCollegeSebling, FeePayment.FeePaymentId, FeePayment, FeePayment.IsHavingSiblingsSameCollege);
                    await _feePaymentService.SaveAsync(feePayment);
                    ViewBag.Success = "true";
                    return Json(new { err = "" });
                }
                else
                {
                    ViewBag.Success = "true";
                    return Json(new { err = "" });
                }
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                return Json(new { err = ex.Message });
            }
        }


        //Admin Site Methods
        [HttpGet]
        public IActionResult GetAllFeePaymentsList()
        {
            try
            {
                IEnumerable<FeePayment> feePaymentsList = _feePaymentService.List().ToList().Where(m => m.StatusCode == true).OrderByDescending(s => s.CreatedOn);
                if (feePaymentsList != null)
                {
                    return Json(feePaymentsList);
                }
                else
                {
                    return new JsonResult(new { status = "feePayment list is null" });
                }
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                return new JsonResult(new { status = "failed" });
            }
        }

        [HttpGet]
        public IActionResult GetUnderPreviewFeePaymentsList()
        {
            try
            {
                IEnumerable<FeePayment> feePaymentsList = _feePaymentService.List().ToList().Where(m => m.StatusCode == true).Where(m => m.RequestStatus == "تحت المعاينة").OrderByDescending(s => s.CreatedOn);
                if (feePaymentsList != null)
                {
                    return Json(feePaymentsList);
                }
                else
                {
                    return new JsonResult(new { status = "feePayment list is null" });
                }
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                return new JsonResult(new { status = "failed" });
            }
        }

        [HttpGet]
        public IActionResult GetFinishedFeePaymentsList()
        {
            try
            {
                IEnumerable<FeePayment> feePaymentsList = _feePaymentService.List().ToList().Where(m => m.StatusCode == true).Where(m => m.RequestStatus == "تم السداد").OrderByDescending(s => s.CreatedOn);
                if (feePaymentsList != null)
                {
                    return Json(feePaymentsList);
                }
                else
                {
                    return new JsonResult(new { status = "feePayment list is null" });
                }
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                return new JsonResult(new { status = "failed" });
            }
        }

        [HttpGet]
        public IActionResult GetRefusedFeePaymentsList()
        {
            try
            {
                IEnumerable<FeePayment> feePaymentsList = _feePaymentService.List().ToList().Where(m => m.StatusCode == true).Where(m => m.RequestStatus == "مرفوض").OrderByDescending(s => s.CreatedOn);
                if (feePaymentsList != null)
                {
                    return Json(feePaymentsList);
                }
                else
                {
                    return new JsonResult(new { status = "feePayment list is null" });
                }
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                return new JsonResult(new { status = "failed" });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetFeePaymentDetails(Guid id)
        {
            try
            {
                var feePaymentDetails = _feePaymentService.Where(m => m.StatusCode == true).Where(s => s.FeePaymentId == id).SingleOrDefault();
                if (feePaymentDetails != null)
                {
                    return Json(feePaymentDetails);
                }
                else
                {
                    return new JsonResult(new { status = "feepayment is null" });
                }
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                return new JsonResult(new { status = "failed" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostFeePaymentDetails([FromBody] FeePayment FeePayment)
        {
            try
            {
                FeePayment.StatusCode = true;
                await _feePaymentService.SaveAsync(FeePayment);

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
