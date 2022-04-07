using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using BOL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KACOStudentCardWebPortal.Models;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Web;
using System.Net;
using Syncfusion.Pdf.Grid;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text.html.simpleparser;
using System.IO;
using Document = iTextSharp.text.Document;
using Font = iTextSharp.text.Font;
using System.Text.RegularExpressions;
//using System.Web.Mvc;

namespace KACOStudentCardWebPortal.Controllers
{
    [Route("[controller]/[action]")]
    [EnableCors("AllowCors")]
    public class ServiceController : Controller
    {
        private readonly ImageService _imageService;
        private readonly CertifiedRecruitmentService _certificateRecruitmentService;
        private readonly ClinicalAllowanceService _clinicalAllowanceService;
        private readonly CollegeService _collegeService;
        private readonly UniversityService _universityService;
        private readonly TicketExchangeService _ticketExchangeService;
        private readonly AnnualAllowanceExchangeService _annualAllowancesExchangeService;
        public ServiceController(CertifiedRecruitmentService certificateRecruitmentService, ClinicalAllowanceService clinicalAllowanceService,
            ImageService imageService, CollegeService collegeService, UniversityService universityService, TicketExchangeService ticketExchangeService,
            AnnualAllowanceExchangeService annualAllowancesExchangeService)
        {
            _certificateRecruitmentService = certificateRecruitmentService;
            _clinicalAllowanceService = clinicalAllowanceService;
            _imageService = imageService;
            _collegeService = collegeService;
            _universityService = universityService;
            _ticketExchangeService = ticketExchangeService;
            _annualAllowancesExchangeService = annualAllowancesExchangeService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetServicesList()
        {
            return View();
        }

        public IActionResult PostCertifiedRecruitmentRequest()
        {
            return View();
        }
        public IActionResult UpdatingStudentInfo()
        {
            return View();
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

        public IActionResult PostAnnualAllowancesExchange()
        {
            var getCollegesList = _collegeService.Where(c => c.StatusCode == true).Where(c => c.AnnualAllowancesExchangeEntity == true).ToList();
            ViewBag.collegesDetails = getCollegesList;
            var getUniversitiesList = _universityService.Where(u => u.StatusCode == true).Where(u => u.AnnualAllowancesExchangeEntity == true).ToList();
            ViewBag.universityDetails = getUniversitiesList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostAnnualAllowancesExchange(AnnualAllowancesExchange AnnualAllowancesExchange, IFormFile ImageId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AnnualAllowancesExchange annualAllowancesExchange = new AnnualAllowancesExchange()
                    {
                        FullName = AnnualAllowancesExchange.FullName,
                        NationalIDNo = AnnualAllowancesExchange.NationalIDNo,
                        CollegeId = AnnualAllowancesExchange.CollegeId,
                        UniversityId = AnnualAllowancesExchange.UniversityId,
                        EducationLevel = AnnualAllowancesExchange.EducationLevel,
                        ImageId = await SaveImage(ImageId),
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        StatusCode = true
                    };
                    await _annualAllowancesExchangeService.SaveAsync(annualAllowancesExchange);
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

        public IActionResult PostTicketExchange()
        {
            var getCollegesList = _collegeService.Where(c => c.StatusCode == true).Where(c => c.TicketExchangeEntity == true).ToList();
            ViewBag.collegesDetails = getCollegesList;
            var getUniversitiesList = _universityService.Where(u => u.StatusCode == true).Where(u => u.TicketExchangeEntity == true).ToList();
            ViewBag.universityDetails = getUniversitiesList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostTicketExchange(TicketExchange TicketExchange, IFormFile ImageId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TicketExchange ticketExchange = new TicketExchange()
                    {
                        FullName = TicketExchange.FullName,
                        NationalIDNo = TicketExchange.NationalIDNo,
                        CollegeId = TicketExchange.CollegeId,
                        UniversityId = TicketExchange.UniversityId,
                        EducationLevel = TicketExchange.EducationLevel,
                        ImageId = await SaveImage(ImageId),
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        StatusCode = true
                    };
                    await _ticketExchangeService.SaveAsync(ticketExchange);
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

        public IActionResult PostClinicalAllowanceExchange()
        {
            var getCollegesList = _collegeService.Where(c => c.StatusCode == true).Where(c => c.ClinicalAllowanceEntity == true).ToList();
            ViewBag.collegesDetails = getCollegesList;
            var getUniversitiesList = _universityService.Where(u => u.StatusCode == true).Where(u => u.ClinicalAllowanceEntity == true).ToList();
            ViewBag.universityDetails = getUniversitiesList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostClinicalAllowanceExchange(ClinicalAllowance ClinicalAllowance, IFormFile ImageId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ClinicalAllowance clinicalAllowance = new ClinicalAllowance()
                    {
                        FullName = ClinicalAllowance.FullName,
                        NationalIDNo = ClinicalAllowance.NationalIDNo,
                        CollegeId = ClinicalAllowance.CollegeId,
                        UniversityId = ClinicalAllowance.UniversityId,
                        EducationLevel = ClinicalAllowance.EducationLevel,
                        ImageId = await SaveImage(ImageId),
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        StatusCode = true
                    };
                    await _clinicalAllowanceService.SaveAsync(clinicalAllowance);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> PrintStudentCertifiedRecruitment(Guid id, string ExportData)
        {
            var studentCertifiedRecruitment = _certificateRecruitmentService.Where(c => c.CertifiedRecruitmentId == id).SingleOrDefault();
            CertifiedRecruitment certifiedRecruitment = new CertifiedRecruitment()
            {
                RequestStatus = "مطبوع",
            };
            return View(studentCertifiedRecruitment);
           

            ////Create an instance of PdfDocument.
            //using (PdfDocument document = new PdfDocument())
            //{
            //    //Create a new PDF document.
            //    PdfDocument doc = new PdfDocument();
            //    //Add a page.
            //    PdfPage page = doc.Pages.Add();
            //    //Create a PdfGrid.
            //    PdfGrid pdfGrid = new PdfGrid();
            //    //Create a DataTable.
            //    DataTable dataTable = new DataTable();
            //    //Add columns to the DataTable
            //    dataTable.Columns.Add("ID");
            //    dataTable.Columns.Add("Name");
            //    //Add rows to the DataTable.
            //    dataTable.Rows.Add(new object[] { "E01", "Clay" });
            //    dataTable.Rows.Add(new object[] { "E02", "Thomas" });
            //    dataTable.Rows.Add(new object[] { "E03", "Andrew" });
            //    dataTable.Rows.Add(new object[] { "E04", "Paul" });
            //    dataTable.Rows.Add(new object[] { "E05", "Gary" });
            //    //Assign data source.
            //    pdfGrid.DataSource = dataTable;
            //    //Draw grid to the page of PDF document.
            //    pdfGrid.Draw(page, new PointF(10, 10));
            //    // Open the document in browser after saving it
            //    //doc.Save("Output.pdf", HttpContext.ApplicationInstance.Response, HttpReadType.Save);
            //    //close the document
            //    doc.Close(true);
            //}
            //return View(studentCertifiedRecruitment);
        }


        [HttpPost]
        public async Task<IActionResult> PostCertifiedRecruitmentRequest(CertifiedRecruitment CertifiedRecruitment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CertifiedRecruitment certifiedRecruitment = new CertifiedRecruitment()
                    {
                        FullName = CertifiedRecruitment.FullName,
                        NationalIDNo = CertifiedRecruitment.NationalIDNo,
                        CellularNoEgypt = CertifiedRecruitment.CellularNoEgypt,
                        State = CertifiedRecruitment.State,
                        DOB = CertifiedRecruitment.DOB,
                        PlaceOfBirth = CertifiedRecruitment.PlaceOfBirth,
                        KuwaitAddress = CertifiedRecruitment.KuwaitAddress,
                        HostCountryAddress = CertifiedRecruitment.HostCountryAddress,
                        EducationStage = CertifiedRecruitment.EducationStage,
                        EducationLevel = CertifiedRecruitment.EducationLevel,
                        EducationDiscontinuityReason = CertifiedRecruitment.EducationDiscontinuityReason,
                        ExpetctedCertificate = CertifiedRecruitment.ExpetctedCertificate,
                        IsStillEducating = CertifiedRecruitment.IsStillEducating,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        RequestStatus = "تحت المعاينة",
                        StatusCode = true
                    };
                    await _certificateRecruitmentService.SaveAsync(certifiedRecruitment);
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

        [HttpPost]
        //[ValidateInput(false)]
        public FileResult Export(string ExportData, string fullName, string state, string dob, string placeOfBirth, string kuwaitAddress, string educationStage,
          string expetctedCertificate, string isStillEducating, string hostCountryAddress, string educationLevel)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                var printDate = DateTime.Now.ToString();
                DateTime datevalue = (Convert.ToDateTime(printDate.ToString()));
                String dy = datevalue.Day.ToString("00");
                String mn = datevalue.Month.ToString("00");
                String yy = datevalue.Year.ToString();


                string dobAndPlaceOfBirth = dob + " - " + placeOfBirth;

                string fullEducationInfo = educationStage + " - " + educationLevel;

                String fullDeliveryDate = yy + "   " + mn + "   " + dy;

                StringReader reader = new StringReader(ExportData);
                Document PdfFile = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(PdfFile, stream);
                PdfFile.Open();

                //iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(@"http:\\dev.kwalexculture.org\certifiedRecruitment\certifiedRecruitmentLayout.png");
                string backgroundImageUrl = "http://dev.kwalexculture.org/certifiedRecruitment/certifiedRecruitmentLayout.png";
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(new Uri (backgroundImageUrl));
                image.ScaleAbsolute(PageSize.A4);
                image.SetAbsolutePosition(0, 0);
                PdfFile.Add(image);

                string certificateMainFontUrl = "http://dev.kwalexculture.org/fonts/forms/arialbd.ttf";
                //BaseFont bfArialUniCode = BaseFont.CreateFont(@"http:\\dev.kwalexculture.org\fonts\forms\arialbd.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                BaseFont bfArialUniCode = BaseFont.CreateFont(new Uri(certificateMainFontUrl).ToString(), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                //Create a font from the base font 
                Font font = new Font(bfArialUniCode, 16);

                //Use a table so that we can set the text direction 
                PdfPTable fullNameTable = new PdfPTable(1);
                PdfPTable stateTable = new PdfPTable(1);
                PdfPTable dobAndplaceOfBirthTable = new PdfPTable(1);
                PdfPTable kuwaitAddressTable = new PdfPTable(1);
                PdfPTable educationInfoTable = new PdfPTable(1);
                PdfPTable expetctedCertificateTable = new PdfPTable(1);
                PdfPTable isStillEducatingTable = new PdfPTable(1);
                PdfPTable hostCountryAddressTable = new PdfPTable(1);
                PdfPTable deliveryDateTable = new PdfPTable(1);

                //Ensure that wrapping is on, otherwise Right to Left text will not display 
                fullNameTable.DefaultCell.NoWrap = false; fullNameTable.DefaultCell.Border = 0;
                stateTable.DefaultCell.NoWrap = false; stateTable.DefaultCell.Border = 0;
                dobAndplaceOfBirthTable.DefaultCell.NoWrap = false; dobAndplaceOfBirthTable.DefaultCell.Border = 0;
                kuwaitAddressTable.DefaultCell.NoWrap = false; kuwaitAddressTable.DefaultCell.Border = 0;
                educationInfoTable.DefaultCell.NoWrap = false; educationInfoTable.DefaultCell.Border = 0;
                expetctedCertificateTable.DefaultCell.NoWrap = false; expetctedCertificateTable.DefaultCell.Border = 0;
                isStillEducatingTable.DefaultCell.NoWrap = false; isStillEducatingTable.DefaultCell.Border = 0;
                hostCountryAddressTable.DefaultCell.NoWrap = false; hostCountryAddressTable.DefaultCell.Border = 0;
                deliveryDateTable.DefaultCell.NoWrap = false; deliveryDateTable.DefaultCell.Border = 0;/* deliveryDateTable.TotalWidth = 600f;*/

                //Create a regex expression to detect hebrew or arabic code points 
                const string regex_match_arabic_hebrew_fullName = @"[\u0600-\u06FF,\u0590-\u05FF]+";
                if (Regex.IsMatch(fullName, regex_match_arabic_hebrew_fullName, RegexOptions.IgnoreCase))
                {
                    fullNameTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                }
                const string regex_match_arabic_hebrew_state = @"[\u0600-\u06FF,\u0590-\u05FF]+";
                if (Regex.IsMatch(state, regex_match_arabic_hebrew_state, RegexOptions.IgnoreCase))
                {
                    stateTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                }
                const string regex_match_arabic_hebrew_dobAndPlaceOfBirth = @"[\u0600-\u06FF,\u0590-\u05FF]+";
                if (Regex.IsMatch(dobAndPlaceOfBirth, regex_match_arabic_hebrew_dobAndPlaceOfBirth, RegexOptions.IgnoreCase))
                {
                    dobAndplaceOfBirthTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                }
                const string regex_match_arabic_hebrew_kuwaitAddress = @"[\u0600-\u06FF,\u0590-\u05FF]+";
                if (Regex.IsMatch(kuwaitAddress, regex_match_arabic_hebrew_kuwaitAddress, RegexOptions.IgnoreCase))
                {
                    kuwaitAddressTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                }
                const string regex_match_arabic_hebrew_educationInfo = @"[\u0600-\u06FF,\u0590-\u05FF]+";
                if (Regex.IsMatch(fullEducationInfo, regex_match_arabic_hebrew_educationInfo, RegexOptions.IgnoreCase))
                {
                    educationInfoTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                }
                const string regex_match_arabic_hebrew_expetctedCertificate = @"[\u0600-\u06FF,\u0590-\u05FF]+";
                if (Regex.IsMatch(expetctedCertificate, regex_match_arabic_hebrew_expetctedCertificate, RegexOptions.IgnoreCase))
                {
                    expetctedCertificateTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                }
                const string regex_match_arabic_hebrew_isStillEducating = @"[\u0600-\u06FF,\u0590-\u05FF]+";
                if (Regex.IsMatch(isStillEducating, regex_match_arabic_hebrew_isStillEducating, RegexOptions.IgnoreCase))
                {
                    isStillEducatingTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                }
                const string regex_match_arabic_hebrew_hostCountryAddress = @"[\u0600-\u06FF,\u0590-\u05FF]+";
                if (Regex.IsMatch(hostCountryAddress, regex_match_arabic_hebrew_hostCountryAddress, RegexOptions.IgnoreCase))
                {
                    hostCountryAddressTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                }
                const string regex_match_arabic_hebrew_fullDeliveryDate = @"[\u0600-\u06FF,\u0590-\u05FF]+";
                if (Regex.IsMatch(fullDeliveryDate, regex_match_arabic_hebrew_fullDeliveryDate, RegexOptions.IgnoreCase))
                {
                    deliveryDateTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                }

                ////Create a regex expression to detect hebrew or arabic code points 
                //const string regex_match_arabic_hebrew_state = @"[\u0600-\u06FF,\u0590-\u05FF]+";
                //if (Regex.IsMatch(state, regex_match_arabic_hebrew_state, RegexOptions.IgnoreCase))
                //{
                //    stateTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                //}

                //Create a cell and add fullName to it 
                PdfPCell fullNameText = new PdfPCell(new Phrase(fullName, font));
                PdfPCell stateText = new PdfPCell(new Phrase(state, font));
                PdfPCell dobAndplaceOfBirthText = new PdfPCell(new Phrase(dobAndPlaceOfBirth, font));
                PdfPCell kuwaitAddressText = new PdfPCell(new Phrase(kuwaitAddress, font));
                PdfPCell educationInfoText = new PdfPCell(new Phrase(fullEducationInfo, font));
                PdfPCell expetctedCertificateText = new PdfPCell(new Phrase(expetctedCertificate, font));
                PdfPCell isStillEducatingText = new PdfPCell(new Phrase(isStillEducating, font));
                PdfPCell hostCountryAddressText = new PdfPCell(new Phrase(hostCountryAddress, font));
                PdfPCell fullDeliveryDateText = new PdfPCell(new Phrase(fullDeliveryDate, font));

                //Ensure that wrapping is on, otherwise Right to Left text will not display 
                fullNameText.NoWrap = false;
                fullNameText.Border = 0;
                fullNameText.PaddingTop = 229;
                fullNameText.PaddingRight = 90;

                stateText.NoWrap = false;
                stateText.Border = 0;
                stateText.PaddingTop = 14;
                stateText.PaddingRight = 40;

                dobAndplaceOfBirthText.NoWrap = false;
                dobAndplaceOfBirthText.Border = 0;
                dobAndplaceOfBirthText.PaddingTop = 12;
                dobAndplaceOfBirthText.PaddingRight = 90;

                kuwaitAddressText.NoWrap = false;
                kuwaitAddressText.Border = 0;
                kuwaitAddressText.PaddingTop = 17;
                kuwaitAddressText.PaddingRight = 80;

                educationInfoText.NoWrap = false;
                educationInfoText.Border = 0;
                educationInfoText.PaddingTop = 15;
                educationInfoText.PaddingRight = 80;

                expetctedCertificateText.NoWrap = false;
                expetctedCertificateText.Border = 0;
                expetctedCertificateText.PaddingTop = 14;
                expetctedCertificateText.PaddingRight = 125;

                isStillEducatingText.NoWrap = false;
                isStillEducatingText.Border = 0;
                isStillEducatingText.PaddingTop = 10;
                isStillEducatingText.PaddingLeft = 60;

                hostCountryAddressText.NoWrap = false;
                hostCountryAddressText.Border = 0;
                hostCountryAddressText.PaddingTop = 16;
                hostCountryAddressText.PaddingRight = 95;

                fullDeliveryDateText.NoWrap = false;
                fullDeliveryDateText.Border = 0;
                fullDeliveryDateText.PaddingTop = 48f;
                //fullDeliveryDateText.PaddingBottom = -60;
                fullDeliveryDateText.PaddingLeft = 302;

                //Add the cell to the table 
                fullNameTable.AddCell(fullNameText);
                stateTable.AddCell(stateText);
                dobAndplaceOfBirthTable.AddCell(dobAndplaceOfBirthText);
                kuwaitAddressTable.AddCell(kuwaitAddressText);
                educationInfoTable.AddCell(educationInfoText);
                expetctedCertificateTable.AddCell(expetctedCertificateText);
                isStillEducatingTable.AddCell(isStillEducatingText);
                hostCountryAddressTable.AddCell(hostCountryAddressText);
                deliveryDateTable.AddCell(fullDeliveryDateText);

                PdfFile.Add(fullNameTable);
                PdfFile.Add(stateTable);
                PdfFile.Add(dobAndplaceOfBirthTable);
                PdfFile.Add(kuwaitAddressTable);
                PdfFile.Add(educationInfoTable);
                PdfFile.Add(expetctedCertificateTable);
                PdfFile.Add(isStillEducatingTable);
                PdfFile.Add(hostCountryAddressTable);
                PdfFile.Add(deliveryDateTable);

                ////Create a cell and add State to it 
                //PdfPCell stateText = new PdfPCell(new Phrase(state, font));
                ////Ensure that wrapping is on, otherwise Right to Left text will not display 
                //stateText.NoWrap = false;
                //stateText.Border = 0;
                //stateText.PaddingTop = 260;
                //stateText.PaddingLeft = 90;
                ////Add the cell to the table 
                //stateTable.AddCell(stateText);

                //Chunk c1 = new Chunk(fullName, font);

                //c1.ScaleAbsolute(PageSize.A4);
                //c1.SetAbsolutePosition(0, 0);

                //PdfFile.Add(stateText);

                XMLWorkerHelper.GetInstance().ParseXHtml(writer, PdfFile, reader);
                PdfFile.Close();
                return File(stream.ToArray(), "application/pdf", fullName + "_" + dob + ".pdf");
            }
        }

        //Admin Site Methods
        static void RouteToWhatAppLink(string whatsAppLink)
        {
            WebRequest req = WebRequest.Create(whatsAppLink);
            WebResponse resp = req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            sr.ReadToEnd();
            sr.Close();
        }

        static void SendCertifiedRecruitmentWhatsAppMessage(CertifiedRecruitment certifiedRecruitment)
        {
            if (certifiedRecruitment.RequestStatus == "تم إرسال رسالة برفض الطلب")
            {
             
            }
            else if (certifiedRecruitment.RequestStatus == "مرفوض")
            {
                //string url = "http://kwalexculture.org/service/PostCertifiedRecruitmentRequest";
                //string refuseMessage = " عزيزى الطالب. نحيطكم علما بأن تم رفض طلب إستخراج مصدقة التجنيد من المكتب الثقافى للدواعى الأتية: " +
                //certifiedRecruitment.RefuseReason + " ." + "برجاء إرسال الطلب مرة أخرى من خلال الرابط: " + url + "\r\n" + " شكرا. ";
                //System.Diagnostics.Process.Start(@"https://api.whatsapp.com/send?phone=" + certifiedRecruitment.CellularNoEgypt + "&text=" + refuseMessage);
                //System.Diagnostics.Process.Start(@"C:\\Users\\IT-1\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\whatsapp.exe");
                certifiedRecruitment.RequestStatus = "تم إرسال رسالة برفض الطلب";
            }
            else if (certifiedRecruitment.RequestStatus == "جاهز للإستلام و مختوم")
            {
                //string notificationMessage = " عزيزى الطالب. يرجى التوجه لمقر المكتب الثقافى بالأسكندرية لإستلام مصدقة التجنيد الخاصة بكم. شكرا ";
                //System.Diagnostics.Process.Start(@"https://api.whatsapp.com/send?phone=" + certifiedRecruitment.CellularNoEgypt + "&text=" + notificationMessage);
                certifiedRecruitment.RequestStatus = "تم إرسال رسالة بالحضور للإستلام";
            }
            else if (certifiedRecruitment.RequestStatus == "تم الإستلام")
            {
                //string deliveryText = "تم إستلام مصدقة التجنيد من المكتب الثقافى بالأسكندرية.";
                //System.Diagnostics.Process.Start(@"https://api.whatsapp.com/send?phone=" + certifiedRecruitment.CellularNoEgypt + "&text=" + deliveryText);
            }
        }

        // Clinical Allowances
        [HttpGet]
        public IActionResult GetClinicalAllowancesList()
        {
            try
            {
                IEnumerable<ClinicalAllowance> clinicalAllowances = _clinicalAllowanceService.List().ToList().Where(m => m.StatusCode == true).OrderByDescending(s => s.CreatedOn);
                if (clinicalAllowances != null)
                {
                    return Json(clinicalAllowances);
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
        public IActionResult GetClinicalAllowanceDetails(Guid id)
        {
            try
            {
                var clinicalAllowanceDetails = _clinicalAllowanceService.Where(m => m.StatusCode == true).Where(s => s.ClinicalAllowanceId == id).SingleOrDefault();
                if (clinicalAllowanceDetails != null)
                {
                    return Json(clinicalAllowanceDetails);
                }
                else
                {
                    return new JsonResult(new { status = "clinical allowance is null" });
                }
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                return new JsonResult(new { status = "failed" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostClinicalAllowanceDetails([FromBody] ClinicalAllowance ClinicalAllowance)
        {
            try
            {
                ClinicalAllowance.StatusCode = true;
                await _clinicalAllowanceService.SaveAsync(ClinicalAllowance);
            
                return new JsonResult(new { status = "saving succeeded" });
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                return new JsonResult(new { status = "failed" });
            }
        }

        // Certified Recruitments
        [HttpGet]
        public IActionResult GetCertifiedRecruitmentsList()
        {
            try
            {
                IEnumerable<CertifiedRecruitment> certifiedRecruitmentsList = _certificateRecruitmentService.List().ToList().Where(m => m.StatusCode == true).OrderByDescending(s => s.CreatedOn);
                if (certifiedRecruitmentsList != null)
                {
                    return Json(certifiedRecruitmentsList);
                }
                else
                {
                    return new JsonResult(new { status = "certified recruitment list is null" });
                }
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                return new JsonResult(new { status = "failed" });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCertifiedRecruitmentDetails(Guid id)
        {
            try
            {
                var certifiedRecruitmentDetails = _certificateRecruitmentService.Where(m => m.StatusCode == true).Where(s => s.CertifiedRecruitmentId == id).SingleOrDefault();
                if (certifiedRecruitmentDetails != null)
                {
                    return Json(certifiedRecruitmentDetails);
                }
                else
                {
                    return new JsonResult(new { status = "certified recruitment details is null" });
                }
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                return new JsonResult(new { status = "failed" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostCertifiedRecruitmentDetails([FromBody] CertifiedRecruitment CertifiedRecruitment)
        {
            try
            {
                SendCertifiedRecruitmentWhatsAppMessage(CertifiedRecruitment);
                CertifiedRecruitment.StatusCode = true;
                await _certificateRecruitmentService.SaveAsync(CertifiedRecruitment);
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
