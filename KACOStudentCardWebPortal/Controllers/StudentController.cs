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
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text.html.simpleparser;
using System.IO;
using Document = iTextSharp.text.Document;
using Font = iTextSharp.text.Font;
using System.Text.RegularExpressions;
using System.Drawing;
using Rectangle = iTextSharp.text.Rectangle;
using System.Net.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace KACOStudentCardWebPortal.Controllers
{
    [Route("[controller]/[action]")]
    [EnableCors("AllowCors")]
    public class StudentController : Controller
    {
        private readonly StudentService _studentService;
        private readonly ImageService _imageService;
        private readonly FileService _fileService;
        private readonly CollegeService _collegeService;
        private readonly UniversityService _universityService;
        private readonly StudentViewService _studentViewService;
        private readonly StudentInfoService _studentInfoService;
        private readonly SeasonService _seasonService;
        public StudentController(StudentService studentService, ImageService imageService, FileService fileService, CollegeService collegeService, 
            UniversityService universityService, StudentViewService studentViewService, StudentInfoService studentInfoService, SeasonService seasonService)
        {
            _studentService = studentService;
            _imageService = imageService;
            _fileService = fileService;
            _universityService = universityService;
            _collegeService = collegeService;
            _studentViewService = studentViewService;
            _studentInfoService = studentInfoService;
            _seasonService = seasonService;
        }

        public async Task<Guid> GetCurrentSeasonYear(Guid thisSeasonId)
        {
            thisSeasonId = _seasonService.Where(s => s.IsCurrentYear == true).Where(s => s.StatusCode == true).Select(s => s.SeasonId).SingleOrDefault();
            //var startYear = _seasonService.Where(s => s.SeasonId == thisSeasonId).Select(s => s.StartYear).SingleOrDefault();
            //var endYear = _seasonService.Where(s => s.SeasonId == thisSeasonId).Select(s => s.EndYear).SingleOrDefault();
            //string fullSeasonYear = startYear + "/" + endYear;
            return thisSeasonId;

        }

        [RequestSizeLimit(100_000_000)]
        [HttpPost]
        private async Task<Guid?> SaveImage(IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    if (file.Length > 0)
                    {
                        byte[] buffer = new byte[file.Length];
                        file.OpenReadStream().Read(buffer, 0, (int)file.Length - 1);
                        string contentType = file.ContentType;
                        BOL.Image image = new BOL.Image()
                        {
                            ImageData = buffer,
                            ImageType = contentType,
                            StatusCode = true
                        };
                        await _imageService.SaveAsync(image);
                        return image.ImageId;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                return null;
            }
        }

        [RequestSizeLimit(100_000_000)]
        [HttpPost]
        private async Task<Guid?> SaveFile(IFormFile uploadedFile)
        {
            try
            {
                if (uploadedFile != null)
                {
                    if (uploadedFile.Length > 0)
                    {
                        byte[] buffer = new byte[uploadedFile.Length];
                        uploadedFile.OpenReadStream().Read(buffer, 0, (int)uploadedFile.Length - 1);
                        string contentType = uploadedFile.ContentType;
                        BOL.File file = new BOL.File()
                        {
                            FileData = buffer,
                            FileType = contentType,
                            StatusCode = true
                        };
                        await _fileService.SaveAsync(file);
                        return file.FileId;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                return null;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(Guid id)
        {
            try
            {
                BOL.Image image = await _imageService.FindAsync(id);
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

        //[HttpGet("{id}")]
        //public FileResult GetStudentImage(Guid id)
        //{
        //    var image = _imageService.FindAsync(id);
        //    return FileResult(image);
        //}

        [HttpGet("{id}")]
        public IActionResult PrintStudentIdCard(Guid id)
        {
            var studentDetails = _studentViewService.Where(s => s.StudentId == id).Where(s => s.StatusCode == true).SingleOrDefault();
            return View(studentDetails);
        }

        public FileResult ExportStudentIdCard(string ExportData, string fullName, string educationLevel, string collegeName, string universityName,
            string nationalIDNo, string passportNo, DateTime createdOn,string registrationNo, Guid imageId)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {

                //Fetching today's date details
                var printDate = DateTime.Now.ToString();
                DateTime datevalue = (Convert.ToDateTime(printDate.ToString()));
                String dy = datevalue.Day.ToString("00");
                String mn = datevalue.Month.ToString("00");
                String yy = datevalue.Year.ToString();
                String fullDeliverDate = dy + " - " + mn + " - " + yy;

                string collegeAndUniversity =  collegeName + " - " + universityName;

                string passportNoToUpperText = passportNo.ToUpper();

                //BaseFont custFont = BaseFont.CreateFont(@"C:\Users\Softglory\source\repos\KACOStudentCardWebPortal\KACOStudentCardWebPortal\wwwroot\fonts\forms\arialbd.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                BaseFont custFont = BaseFont.CreateFont(@"http://dev.kwalexculture.org/fonts/forms/arialbd.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                //Create a font from the base font 
                Font font = new Font(custFont, 36);
                Font registrationNoFont = new Font(custFont, 33);
                registrationNoFont.SetColor(225, 3, 11);
                Font deliveryDateFont = new Font(custFont, 31);

                StringReader reader = new StringReader(ExportData);
                Document PdfFile = new Document(new Rectangle(850f, 500f), 35f, 35f, 0f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(PdfFile, stream);
                //PdfWriter.GetInstance(PdfFile, stream);
                PdfFile.Open();

                iTextSharp.text.Image thisStudentImage = iTextSharp.text.Image.GetInstance(@"http://dev.kwalexculture.org/image/getimage/" + imageId);
                thisStudentImage.SetAbsolutePosition(100, 270);
                thisStudentImage.WidthPercentage = 100f;
                thisStudentImage.Alignment = iTextSharp.text.Image.ALIGN_LEFT;
                thisStudentImage.PaddingTop = 250f;
                thisStudentImage.ScaleToFit(310f, 210f);
                PdfFile.Add(thisStudentImage);

                iTextSharp.text.Image cardBg = iTextSharp.text.Image.GetInstance(@"http://dev.kwalexculture.org/id-cards/cardBgForWebsiteFinal.PNG");
                cardBg.SetAbsolutePosition(0, 0);
                PdfFile.Add(cardBg);

                //RegistrationNo Table
                PdfPTable registrationNocardTable = new PdfPTable(1);
                registrationNocardTable.WidthPercentage = 840f;
                registrationNocardTable.DefaultCell.NoWrap = false; registrationNocardTable.DefaultCell.Border = 0;
                const string regex_match_arabic_hebrew_registrationNo = @"[\u0600-\u06FF,\u0590-\u05FF]+";
                if (Regex.IsMatch(registrationNo, regex_match_arabic_hebrew_registrationNo, RegexOptions.IgnoreCase))
                {
                    registrationNocardTable.RunDirection = PdfWriter.RUN_DIRECTION_DEFAULT;
                }
                PdfPCell registrationNoText = new PdfPCell(new Phrase(registrationNo, registrationNoFont));
                //Ensure that wrapping is on, otherwise Right to Left text will not display 
                registrationNoText.NoWrap = false;
                registrationNoText.PaddingRight = -400f;
                registrationNoText.PaddingTop = 98f;
                //registrationNoText.PaddingBottom = 10f;
                //registrationNoText.PaddingLeft = 100f;
                //fullNameText.Colspan = 2;
                registrationNoText.Border = 0;
                registrationNoText.HorizontalAlignment = 1;
                registrationNocardTable.AddCell(registrationNoText);

                //FullName Table
                PdfPTable fullNamecardTable = new PdfPTable(2);
                fullNamecardTable.WidthPercentage = 840f;
                fullNamecardTable.DefaultCell.NoWrap = false; fullNamecardTable.DefaultCell.Border = 0;
                const string regex_match_arabic_hebrew_fullName = @"[\u0600-\u06FF,\u0590-\u05FF]+";
                if (Regex.IsMatch(fullName, regex_match_arabic_hebrew_fullName, RegexOptions.IgnoreCase))
                {
                    fullNamecardTable.RunDirection = PdfWriter.RUN_DIRECTION_DEFAULT;
                }
                //PdfPCell emptyCellText = new PdfPCell(new Phrase(""));
                PdfPCell fullNameText = new PdfPCell(new Phrase(fullName, font));
                //emptyCellText.PaddingTop = 10f;
                //emptyCellText.PaddingRight = 20f;
                //emptyCellText.Colspan = 0;
                //emptyCellText.Border = 0;
                //emptyCellText.HorizontalAlignment = 1;

                //fullNameText.NoWrap = false;
                fullNameText.PaddingRight = -220f;
                fullNameText.PaddingTop = 10f;
                //fullNameText.PaddingBottom = 10f;
                //fullNameText.PaddingLeft = 260f;
                fullNameText.Colspan = 2;
                fullNameText.Border = 0;
                fullNameText.HorizontalAlignment = 1;
                //fullNamecardTable.AddCell(emptyCellText);
                fullNamecardTable.AddCell(fullNameText);

                //College and university Table
                PdfPTable collegeAndUniversitycardTable = new PdfPTable(1);
                collegeAndUniversitycardTable.WidthPercentage = 840f;
                collegeAndUniversitycardTable.DefaultCell.NoWrap = false; collegeAndUniversitycardTable.DefaultCell.Border = 0;
                const string regex_match_arabic_hebrew_collegeAndUniversity = @"[\u0600-\u06FF,\u0590-\u05FF]+";
                if (Regex.IsMatch(collegeAndUniversity, regex_match_arabic_hebrew_collegeAndUniversity, RegexOptions.IgnoreCase))
                {
                    collegeAndUniversitycardTable.RunDirection = PdfWriter.RUN_DIRECTION_DEFAULT;
                }
                PdfPCell collegeAndUniversityText = new PdfPCell(new Phrase(collegeAndUniversity, font));
                //collegeAndUniversityText.NoWrap = false;
                collegeAndUniversityText.PaddingRight = -350f;
                collegeAndUniversityText.PaddingTop = 11f;
                collegeAndUniversityText.PaddingBottom = 10f;
                //collegeAndUniversityText.PaddingLeft = 370f;
                //collegeAndUniversityText.Colspan = 2;
                collegeAndUniversityText.Border = 0;
                collegeAndUniversityText.HorizontalAlignment = 1;
                collegeAndUniversitycardTable.AddCell(collegeAndUniversityText);

                //Education Table
                PdfPTable educationLevelcardTable = new PdfPTable(1);
                educationLevelcardTable.WidthPercentage = 840f;
                educationLevelcardTable.DefaultCell.NoWrap = false; educationLevelcardTable.DefaultCell.Border = 0;
                const string regex_match_arabic_hebrew_educationLevel = @"[\u0600-\u06FF,\u0590-\u05FF]+";
                if (Regex.IsMatch(educationLevel, regex_match_arabic_hebrew_educationLevel, RegexOptions.IgnoreCase))
                {
                    educationLevelcardTable.RunDirection = PdfWriter.RUN_DIRECTION_DEFAULT;
                }
                PdfPCell educationLevelText = new PdfPCell(new Phrase(educationLevel, font));
                educationLevelText.NoWrap = false;
                educationLevelText.PaddingRight = -55f;
                educationLevelText.PaddingTop = 8f;
                educationLevelText.PaddingBottom = 10f;
                educationLevelText.PaddingLeft = 60f;
                //educationLevelText.Colspan = 2;
                educationLevelText.Border = 0;
                educationLevelText.HorizontalAlignment = 1;
                educationLevelcardTable.AddCell(educationLevelText);

                //NationalID Table
                PdfPTable nationalIDcardTable = new PdfPTable(1);
                nationalIDcardTable.WidthPercentage = 840f;
                nationalIDcardTable.DefaultCell.NoWrap = false; nationalIDcardTable.DefaultCell.Border = 0;
                const string regex_match_arabic_hebrew_nationalIDNo = @"[\u0600-\u06FF,\u0590-\u05FF]+";
                if (Regex.IsMatch(nationalIDNo, regex_match_arabic_hebrew_nationalIDNo, RegexOptions.IgnoreCase))
                {
                    nationalIDcardTable.RunDirection = PdfWriter.RUN_DIRECTION_DEFAULT;
                }
                PdfPCell nationalIDNoText = new PdfPCell(new Phrase(nationalIDNo, font));
                nationalIDNoText.NoWrap = false;
                nationalIDNoText.PaddingRight = -128f;
                nationalIDNoText.PaddingTop = 4f;
                nationalIDNoText.PaddingBottom = 10f;
                //nationalIDNoText.PaddingLeft = 120f;
                //collegeAndUniversityText.Colspan = 2;
                nationalIDNoText.Border = 0;
                nationalIDNoText.HorizontalAlignment = 1;
                nationalIDcardTable.AddCell(nationalIDNoText);

                //Passport No Table
                PdfPTable passportNocardTable = new PdfPTable(1);
                passportNocardTable.WidthPercentage = 840f;
                passportNocardTable.DefaultCell.NoWrap = false; passportNocardTable.DefaultCell.Border = 0;
                const string regex_match_arabic_hebrew_passportDNo = @"[\u0600-\u06FF,\u0590-\u05FF]+";
                if (Regex.IsMatch(passportNo, regex_match_arabic_hebrew_passportDNo, RegexOptions.IgnoreCase))
                {
                    passportNocardTable.RunDirection = PdfWriter.RUN_DIRECTION_DEFAULT;
                }
                PdfPCell passportNoText = new PdfPCell(new Phrase(passportNoToUpperText, font));
                passportNoText.NoWrap = false;
                passportNoText.PaddingRight = -100f;
                passportNoText.PaddingTop = 3f;
                passportNoText.PaddingBottom = 10f;
                //passportNoText.PaddingLeft = 25f;
                //collegeAndUniversityText.Colspan = 2;
                passportNoText.Border = 0;
                passportNoText.HorizontalAlignment = 1;
                passportNocardTable.AddCell(passportNoText);

                //DeliveryDate Table
                PdfPTable deliveryDateTable = new PdfPTable(1);
                deliveryDateTable.WidthPercentage = 840f;
                deliveryDateTable.DefaultCell.NoWrap = false; deliveryDateTable.DefaultCell.Border = 0;
                const string regex_match_arabic_hebrew_deliveryDate = @"[\u0600-\u06FF,\u0590-\u05FF]+";
                if (Regex.IsMatch(fullDeliverDate, regex_match_arabic_hebrew_deliveryDate, RegexOptions.IgnoreCase))
                {
                    deliveryDateTable.RunDirection = PdfWriter.RUN_DIRECTION_DEFAULT;
                }
                PdfPCell deliverDateText = new PdfPCell(new Phrase(fullDeliverDate, deliveryDateFont));
                deliverDateText.NoWrap = false;
                deliverDateText.PaddingRight = -40f;
                deliverDateText.PaddingTop = 12f;
                deliverDateText.PaddingBottom = 10f;
                deliverDateText.PaddingLeft = 170f;
                //collegeAndUniversityText.Colspan = 2;
                deliverDateText.Border = 0;
                deliverDateText.HorizontalAlignment = 1;
                deliveryDateTable.AddCell(deliverDateText);

                PdfFile.Add(registrationNocardTable);
                PdfFile.Add(fullNamecardTable);
                PdfFile.Add(collegeAndUniversitycardTable);
                PdfFile.Add(educationLevelcardTable);
                PdfFile.Add(nationalIDcardTable);
                PdfFile.Add(passportNocardTable);
                PdfFile.Add(deliveryDateTable);
               
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, PdfFile, reader);
                PdfFile.Close();
                return File(stream.ToArray(), "application/pdf", fullName + "-" + registrationNo + ".pdf");
            }
        }

        //public IActionResult GetCard(Guid id)
        //{
        //   var studentDetails = _studentService.Where(s => s.StudentId == id).Where(s => s.StatusCode == true).SingleOrDefault();
        //    return View(studentDetails);
        //}

        //[HttpPost]
        //public async Task<IActionResult> PostIdCardRequests(Student Student, IFormFile ImageId, IFormFile FileId)
        //{
        //    try
        //    {
        //        var checkingEmaialAvailability = _studentService.Where(s => s.Email == Student.Email).SingleOrDefault();
        //        var checkingNationalIDNoAvailability = _studentService.Where(s => s.NationalIDNo == Student.NationalIDNo).SingleOrDefault();
        //        int getMaxRegistrationNo = _studentService.Where(s => s.StatusCode == true).Max(s => s.RegistrationNo);
        //        //int getHighestRegistrationNo = _studentService.Where(s => s.RegistrationNo == Student.RegistrationNo).Select(s =>s.RegistrationNo).Max() ;
        //        if (checkingEmaialAvailability != null)
        //        {
        //            ModelState.AddModelError("Email", "هذا البريد الإلكترونى مسجل من قبل. من فضلك أدخل بريد إلكترونى أخر");
        //            ViewData["thisPageMessage"] = "هذا البريد الإلكترونى مسجل من قبل. من فضلك أدخل بريد إلكترونى أخر";
        //            return View("Failed", ViewData["thisPageMessage"]);
        //        }
        //        else if (checkingNationalIDNoAvailability != null)
        //        {

        //            ModelState.AddModelError("NationalIDNo", "الرقم المدنى المدخل مسجل سابقا. من فضلك أختر رقم مدنى أخر.");
        //            ViewData["thisPageMessage"] = "الرقم المدنى المدخل مسجل سابقا. من فضلك أختر رقم مدنى أخر.";
        //            return View("Failed", ViewData["thisPageMessage"]);
        //        }
        //        else if (ModelState.IsValid)
        //        {
        //            Student student = new Student()
        //            {
        //                FileId = await SaveFile(FileId),
        //                FullName = Student.FullName,
        //                NationalIDNo = Student.NationalIDNo,
        //                CollegeName = Student.CollegeName,
        //                EducationLevel = Student.EducationLevel,
        //                PassportNo = Student.PassportNo,
        //                RegistrationNo = getMaxRegistrationNo + 1,
        //                CellularNoEgypt = Student.CellularNoEgypt,
        //                CellularNoKuwait = Student.CellularNoKuwait,
        //                Email = Student.Email,
        //                ImageId = await SaveImage(ImageId),
        //                CreatedOn = DateTime.Now,
        //                ModifiedOn = DateTime.Now,
        //                StatusCode = true
        //            };
        //            if (Student.NationalIDNo != null)
        //            {
        //                ModelState.AddModelError("NationalIDNo", "من فضلك أدخل رقم مدنى أخر");
        //            }
        //            else if (Student.Email != null)
        //            {
        //                ModelState.AddModelError("Email", "من فضلك أدخل بريد إلكترونى أخر");
        //            }
        //            await _studentService.SaveAsync(student);
        //            return View("Success");
        //        }
        //        else
        //        {
        //            return View("Success");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var innerEx = ex.InnerException;
        //        return View("Failed");
        //    }
        //}

        public IActionResult StudentCardView(Guid id)
        {

            ViewBag.getStudentDetails = _studentService.Where(s => s.StudentId == id).SingleOrDefault();
            return View(ViewBag.getStudentDetails);
        }

        public IActionResult landPage(Student Student, string Email, string NationalIDNo)
        {
            //var checkingEmaialAvailability = _studentService.Where(s => s.Email == Student.Email).SingleOrDefault();
            //var checkingNationalIDNoAvailability = _studentService.Where(s => s.NationalIDNo == Student.NationalIDNo).SingleOrDefault();
            //var nationalId = _studentService.Where(s => s.StatusCode == true).Where(s => s.NationalIDNo == NationalIDNo).SingleOrDefault();
            //var email = _studentService.Where(s => s.StatusCode == true).Where(s => s.Email == Email).SingleOrDefault();
            //if (nationalId != null)
            //{
            //    ModelState.AddModelError("NationalIDNo", "من فضلك أدخل رقم مدنى أخر");
            //    ViewBag.Success = "false";
            //    return Json(new { err = "الرقم المدنى مسجل سابقا. من فضلك أدخل رقم مدنى صحيح" });
            //}
            //else
            //{
            //    ViewBag.Success = "ture";
            //}
            //if (email != null)
            //{
            //    ModelState.AddModelError("Email", "من فضلك أدخل بريد إلكترونى أخر");
            //    ViewBag.mailSuccess = "false";
            //    return Json(new { err = "البريد الإلكترونى مسجل سابقا. من فضلك أدخل بريد إلكترونى أخر" });
            //}
            //else
            //{
            //    ViewBag.mailSuccess = "true";
            //}
            return View();
        }

        public async Task<int> GetMaximumRegistrationNo(int maxRecord)
        {
            int getMaxRegistrationNo = _studentService.Where(s => s.StatusCode == true).Select(s => s.RegistrationNo).Max();
            ViewBag.getMaxRegistrationNo = getMaxRegistrationNo + 1;
            maxRecord = getMaxRegistrationNo + 1;
            return maxRecord;
        }

        public IActionResult PostIdCardRequest(int RegistrationNo)
        {
            //int getMaxRegNo = _studentService.Where(s => s.StatusCode == true).Select(s => s.RegistrationNo).Max();
            //int regNo;
            //if (getMaxRegNo != null)
            //{
            //    regNo = getMaxRegNo + 1;
            //}
            //else
            //{
            //    regNo = getMaxRegNo;
            //};

            //ViewBag.finalRegRecord = getMaxRegNo;
            //int getMaxRegistrationNo = _studentService.Where(s => s.StatusCode == true).Select(s => s.RegistrationNo).Max();
            //ViewBag.getMaxRegistrationNo = getMaxRegistrationNo + 1;
            var getCollegesList = _collegeService.Where(c => c.StatusCode == true).ToList();
            ViewBag.collegesDetails = getCollegesList;
            var getUniversitiesList = _universityService.Where(u => u.StatusCode == true).ToList();
            ViewBag.universityDetails = getUniversitiesList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostIdCardRequest(Student Student, IFormFile ImageId, IFormFile FileId, string Email, string NationalIDNo, int RegistrationNo)
        {
            try
            {
                var checkingEmaialAvailability = _studentService.Where(s => s.Email == Student.Email).SingleOrDefault();
                var checkingNationalIDNoAvailability = _studentService.Where(s => s.NationalIDNo == Student.NationalIDNo).SingleOrDefault();
                int getMaxRegNo = await GetMaximumRegistrationNo(RegistrationNo);
                int regNo;
                //if (getMaxRegNo != null)
                //{
                //    regNo = RegistrationNo + 1;
                //}
                //else
                //{
                //    regNo = RegistrationNo;
                //};

                //ViewBag.finalRegRecord = regNo;
                //int getHighestRegistrationNo = _studentService.Where(s => s.RegistrationNo == Student.RegistrationNo).Select(s =>s.RegistrationNo).Max() ;
                //if (checkingEmaialAvailability != null)
                //{
                //    ModelState.AddModelError("Email", "هذا البريد الإلكترونى مسجل من قبل. من فضلك أدخل بريد إلكترونى أخر");
                //    ViewData["thisPageMessage"] = "هذا البريد الإلكترونى مسجل من قبل. من فضلك أدخل بريد إلكترونى أخر";
                //    return View("Failed", ViewData["thisPageMessage"]);
                //}
                //else if (checkingNationalIDNoAvailability != null)
                //{

                //    ModelState.AddModelError("NationalIDNo", "الرقم المدنى المدخل مسجل سابقا. من فضلك أختر رقم مدنى أخر.");
                //    ViewData["thisPageMessage"] = "الرقم المدنى المدخل مسجل سابقا. من فضلك أختر رقم مدنى أخر.";
                //    return View("Failed", ViewData["thisPageMessage"]);
                //}
                //else if (ModelState.IsValid)

                if (ModelState.IsValid)
                {
                    Student student = new Student()
                    {
                        FileId = await SaveFile(FileId),
                        FullName = Student.FullName,
                        NationalIDNo = Student.NationalIDNo,
                        //CollegeName = Student.CollegeName,
                        EducationLevel = Student.EducationLevel,
                        PassportNo = Student.PassportNo,
                        RegistrationNo = 5000,
                        CellularNoEgypt = Student.CellularNoEgypt,
                        CellularNoKuwait = Student.CellularNoKuwait,
                        Email = Student.Email,
                        CollegeId = Student.CollegeId,
                        UniversityId = Student.UniversityId,
                        ImageId = await SaveImage(ImageId),
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        RequestStatus = "تحت المعاينة",
                        StatusCode = true
                    };
         
                    await _studentService.SaveAsync(student);
                    //RegistrationNo = RegistrationNo + 1;
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

        [HttpGet]
        public async Task<IActionResult> UpdateStudentInfo(Guid SeasonId)
        {
            //var thisSeasonId = await GetCurrentSeasonYear(SeasonId);
            var getCollegesList = _collegeService.Where(c => c.StatusCode == true).ToList();
            ViewBag.collegesDetails = getCollegesList;
            var getUniversitiesList = _universityService.Where(u => u.StatusCode == true).ToList();
            ViewBag.universityDetails = getUniversitiesList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStudentInfo(StudentInfo StudentInfo, IFormFile ImageId, Guid SeasonId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    StudentInfo studentInfo = new StudentInfo()
                    {
                        StudentInfoFullName = StudentInfo.StudentInfoFullName,
                        StudentInfoNationalIdNo = StudentInfo.StudentInfoNationalIdNo,
                        StudentPassportNo = StudentInfo.StudentPassportNo,
                        StudentInfoNationality = StudentInfo.StudentInfoNationality,
                        ImageId = await SaveImage(ImageId),
                        StudentInfoEgCellNo = StudentInfo.StudentInfoEgCellNo,
                        StudentInfoKwCellNo = StudentInfo.StudentInfoKwCellNo,
                        StudentInfoUrgentEgCellNo = StudentInfo.StudentInfoUrgentEgCellNo,
                        StudentInfoEmail = StudentInfo.StudentInfoEmail,
                        StudentInfoEgAddress = StudentInfo.StudentInfoEgAddress,
                        UniversityId = StudentInfo.UniversityId,
                        CollegeId = StudentInfo.CollegeId,
                        EducationLevel = StudentInfo.EducationLevel,
                        EducationCategory = StudentInfo.EducationCategory,
                        SeasonId = await GetCurrentSeasonYear(SeasonId),
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        StatusCode = true
                    };

                    await _studentInfoService.SaveAsync(studentInfo);
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


        public IActionResult Success(int regNo)
        {
            ViewBag.regNo = regNo;
            return View(ViewBag.regNo);
        }

        public IActionResult Failed()
        {
            return View();
        }



        //public IActionResult ViewCard(Guid id)
        //{
        //    ViewBag.getStudentDetails = _studentService.Where(s => s.StudentId == id).SingleOrDefault();
        //    Card image = new Card();
        //    image.Image1 = GenerateImage.GetDefaultBase64Image(ViewBag.getStudentDetails.FullName, new Font(FontFamily.GenericSansSerif, 50, FontStyle.Bold), Color.Black, Color.Transparent, 300, 200);
        //    return View(image);
        //}

        public IActionResult Index()
        {
            return View();
        }




        // Admin Site Methods //
        [HttpGet]
        public IActionResult GetStudentsList()
        {
            try
            {
                IEnumerable<Student> studentsList = _studentService.List().ToList().Where(m => m.StatusCode == true).OrderBy(s => s.RegistrationNo);
                if (studentsList != null)
                {
                    return Json(studentsList);
                }
                else
                {
                    return new JsonResult(new { status = "studens list is null" });
                }
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                return new JsonResult(new { status = "failed" });
            }
        }

        [HttpGet]
        public IActionResult GetRefusedStudentsCards()
        {
            try
            {
                IEnumerable<Student> refulsedStudentsCardsList = _studentService.List().ToList().Where(m => m.StatusCode == true). Where(c => c.RegistrationNo == 8000).OrderBy(s => s.RegistrationNo);
                if (refulsedStudentsCardsList != null)
                {
                    return Json(refulsedStudentsCardsList);
                }
                else
                {
                    return new JsonResult(new { status = "refused students cards list is null" });
                }
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                return new JsonResult(new { status = "failed" });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetStudentDetails(Guid id)
        {
            try
            {
                var studentDetails = _studentService.Where(m => m.StatusCode == true).Where(s => s.StudentId == id).SingleOrDefault();
                if (studentDetails != null)
                {
                    return Json(studentDetails);
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

        static void SendWhatsAppMessage(string studentPhoneNo)
        {
            var accountSid = "AC72a70695bef375c2efe4565e82f17369";
            var authToken = "6f7c77c41cf51b94ca5d55d1488f01de";
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(new PhoneNumber("whatsapp:" + studentPhoneNo));
            messageOptions.From = new PhoneNumber("whatsapp:+14155238886");
            messageOptions.Body = "طلب جديد مرفوض";

            var message = MessageResource.Create(messageOptions);
            Console.WriteLine(message.Body);
        }

        [HttpPost]
        public async Task<IActionResult> PostStudentCardDetails([FromBody] Student Student, IFormFile studentImage)
        {
            try
            {
                var studentMail = Student.Email;
                if (Student.RequestStatus == "تم إرسال رسالة برفض الطلب")
                {
                    
                }
                else if(Student.RequestStatus == "مرفوض")
                {
                    string refuseMessage = "يرجى إرسال الطلب مرة أخرى على الرابط http://kwalexculture.org/student/postidcardrequest. شكرا .عزيزى الطالب. نحيطكم علما بأن تم رفض طلب إستخراج الهوية";
                    System.Diagnostics.Process.Start("http://api.whatsapp.com/send?phone=" + Student.CellularNoEgypt + "&text=" + refuseMessage);
                    Student.RequestStatus = "تم إرسال رسالة برفض الطلب";
                    //SendWhatsAppMessage(Student.CellularNoEgypt);
                }
                else if(Student.RequestStatus == "تم الإستلام")
                {
                    System.Diagnostics.Process.Start("http://api.whatsapp.com/send?phone=" + Student.CellularNoEgypt + "&text=" + "تم إستلام بطاقة الهوية");
                }
                //if (Student.RequestStatus == "مرفوض")
                //{
                //    MailAddress ccKate = new MailAddress(studentMail, "إفادة بحالة الطلب الخاص بإستخراج هوية الطالب");
                //    MailMessage myMail = new System.Net.Mail.MailMessage("no.reply@kwalexculture.org", studentMail);
                //    SmtpClient companySmtpClient = new SmtpClient("mail.kwalexculture.org");
                //    companySmtpClient.UseDefaultCredentials = true;

                //    myMail.CC.Add("shady.nessim@kwalexculture.org");

                //    myMail.Subject = "إفادة بحالة الطلب الخاص بإستخراج هوية الطالب";
                //    myMail.SubjectEncoding = System.Text.Encoding.UTF8;

                //    myMail.Body = " .عزيزى الطالب. نحيطكم علما بأن تم رفض طلب إستخراج الهوية لعدم إستيفاء البيانات الصحيحة أو الصورة الشخصية الملحقة غير مطابقة لمواصفات الطلب" +
                //    "يرجى إرسال الطلب مرة أخرى على الرابط http://kwalexculture.org/student/postidcardrequest. شكرا";

                //    myMail.BodyEncoding = System.Text.Encoding.UTF8;
                //    myMail.IsBodyHtml = true;

                //    //myMail.Attachments.Add(new Attachment(@"PathToAttachment"));

                //    companySmtpClient.Send(myMail);
                //}

                Student.StatusCode = true;
                //Client.ImageId = await SaveImage(clientImage).ConfigureAwait(true);
                await _studentService.SaveAsync(Student);

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
