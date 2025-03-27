﻿using Azure.Core;
using Company.G04.DAL.Moudel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using Company.G04.PL.Dtos.Auth;

public class AccountController : Controller
{
    
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #region SignUp
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    if (user == null)//There is no user with this name
                    {
                        user = await _userManager.FindByEmailAsync(model.Email);
                        if (user == null)
                        {
                            user = new AppUser()
                            {
                                UserName = model.UserName,
                                FirstName = model.FirstName,
                                LastName = model.LastName,
                                Email = model.Email,
                                IsAgreed = model.IsAgreed

                            };
                            var result = await _userManager.CreateAsync(user, model.Password);

                            if (result.Succeeded)
                            {
                                return RedirectToAction("SignIn");
                            }
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                        ModelState.AddModelError(string.Empty, "Email is already exist !!");
                        return View(model);
                    }
                    ModelState.AddModelError(string.Empty, "UserName is already exist !!");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }
        #endregion

        #region SignIn
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //check email
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user is not null)
                    {
                        //check password
                        var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                        if (flag)
                        {
                            //SignIn

                            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                            if (result.Succeeded)
                                return RedirectToAction("Index", "Home");
                        }
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Login !!");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(model);
        }
        #endregion

        #region SignOut

        public async Task<IActionResult> SignOut()
        {   
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
    }



        #endregion



