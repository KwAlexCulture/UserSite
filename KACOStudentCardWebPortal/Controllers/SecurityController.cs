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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace KACOStudentCardWebPortal.Controllers
{
    [Route("[controller]/[action]")]
    [EnableCors("AllowCors")]
    public class SecurityController : Controller
    {
        private readonly UserService _userService;
        private readonly UserRoleService _userRoleService;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public SecurityController(UserService userService, UserRoleService userRoleService)
        {
            _userService = userService;
            _userRoleService = userRoleService;
        }
        public IActionResult Index()
        {
            return View();
        }

        // Admin Site Methods //
        // POST: /Account/Login    
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(IdentityUser idUser, User user, string returnUrl, bool RememberMe, bool ShouldLockOn)
        {
            if (!ModelState.IsValid)
            {
                return View("Home","Index");
            }

            var userEmail = await userManager.FindByEmailAsync(user.Email);
            if (user != null)
            {
                ModelState.AddModelError("message", "Email is not verified");
                return View(user);

            }
            if (await userManager.CheckPasswordAsync(idUser, user.Password) == false)
            {
                ModelState.AddModelError("message", "Invalid credentials");
                return View(user);

            }

            // This doesn't count login failures towards account lockout    
            // To enable password failures to trigger account lockout, change to shouldLockout: true    
            var result = await signInManager.PasswordSignInAsync(user.Email, user.Password, true , true);

            if (result.Succeeded)
            {
                await userManager.AddClaimAsync(idUser, new Claim("UserRole", "FeePayment"));
                return RedirectToAction("Dashboard");
            }
            else if (result.IsLockedOut)
            {
                return View("AccountLocked");
            }
            else
            {
                ModelState.AddModelError("message", "Invalid login attempt");
                return View(user);
            }
    }
    }
}
