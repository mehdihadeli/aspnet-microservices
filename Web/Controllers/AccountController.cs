﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountSvc _accSvc;

        public AccountController(IAccountSvc accSvc, ILogger<HomeController> logger)
        {
            _accSvc = accSvc;
            _logger = logger;
        }

        /// <summary>
        /// My Account
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            // todo :: get acct data from acctSvc
            return View();
        }

        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult SignIn()
        {
            return View();
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SignIn(SignIn request)
        {
            // todo :: get claims from accSvc
            var claims = new List<Claim>
            {
                new Claim("FullName", "Bruno Hildenbrand")
            };

            var authProperties = new AuthenticationProperties();

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Account");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
