using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq; //allows us to access query methods
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ProjectTracker.Models;


namespace ProjectTracker.Controllers
{
    public class UsersController : Controller
    {
        private readonly MyContext _context;

        public UsersController(MyContext myContext)
        {
            _context = myContext;
        }


        [HttpGet("")]
        public IActionResult LoginForm()
        {
            return View();
        }

        [HttpGet("register")]
        public IActionResult RegistrationForm()
        {
            return View();
        }

        [HttpPost("users")]
        public IActionResult Register(User userToCreate)
        {

            if(!ModelState.IsValid)
            {
                //invalid data coming in
                return View("LoginForm");
            }
            
            var existingUser = _context.Users
                .FirstOrDefault(user => user.Email == userToCreate.Email);

            if(existingUser != null)
            {
                //email already in the DB
                ModelState.AddModelError("Email", "Email unavailable.");
                return View("LoginForm");
            }

            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            userToCreate.Password = Hasher.HashPassword(userToCreate, userToCreate.Password);

            _context.Add(userToCreate);
            _context.SaveChanges();

            //save the user ID to session
            HttpContext.Session.SetInt32("UserId", userToCreate.UserId);

            //note that we're sending the user to a different controller
            return RedirectToAction("MyProfile", "Home");
        }

        [HttpPost("users/login")]
        public IActionResult Login(LoginUser userToLogin)
        {
            //check validation state(optional)
            if(!ModelState.IsValid)
            {
                return View("LoginForm");
            }
            //check the DB for the email
            var foundUser = _context.Users
                .FirstOrDefault(user => user.Email == userToLogin.LoginEmail);

            if(foundUser == null)
            {
                //not in DB
                Console.WriteLine("User wasn't found");
                ModelState.AddModelError("LoginPassword", "Please check your email/password.");
                return View("LoginForm");
            }

            //compare the plaintext password to what's stored in the DB
            var hasher = new PasswordHasher<LoginUser>();

            //verify provided password against hash stored in Db
            var result = hasher.VerifyHashedPassword(userToLogin, foundUser.Password, userToLogin.LoginPassword);

            if(result == 0)
            {
                //password doesn't match
                Console.WriteLine("password not matching");
                ModelState.AddModelError("LoginPassword", "Please check your email/password.");
                return View("LoginForm");
            }

            //put the user ID into session
            HttpContext.Session.SetInt32("UserId", foundUser.UserId);
            return RedirectToAction("MyProfile", "Home");
        }

        [HttpGet("users/logout")]
        public IActionResult Logout()
        {
            //clears the Session
            HttpContext.Session.Clear();
            
            return RedirectToAction("LoginForm");
        }
    }
}