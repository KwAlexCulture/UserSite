using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace KACOStudentCardWebPortal.Controllers
{
    [Route("[controller]/[action]")]
    [EnableCors("AllowCors")]
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetNewsList()
        {
            return View();
        }

        public IActionResult LatestDevelopments()
        {
            return View();
        }

        public IActionResult FourSeasonsOffer()
        {
            return View();
        }

        public IActionResult TulipOffer()
        {
            return View();
        }

        public IActionResult SocialReward()
        {
            return View();
        }
    }
}
