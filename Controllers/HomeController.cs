using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ProjectTracker.Controllers
{

//User Logins
    public class HomeController : Controller
    {
        private readonly MyContext _context;

        public HomeController(MyContext myContext)
        {
            _context = myContext;
        }

        // [HttpGet("home")]
        // public IActionResult UserProfile()//Main Page
        // {

        //     // Console.WriteLine(HttpContext.Session.GetInt32("UserId"));
        //     int? userId = HttpContext.Session.GetInt32("UserId");
        //     //protects the page from non-logged in users
        //     if(userId == null) // no user present
        //     {
        //         return RedirectToAction("LoginReg", "Users");
        //     }

        //     ViewBag.User = _context.Users
        //         .Find(userId);

        //     ViewBag.AllProjects = _context.Projects
        //         .ToList();

        //     return View();
        // }



        [HttpGet("admin/users")]
        public IActionResult UserAll()
        {

            int? userId = HttpContext.Session.GetInt32("UserId");
            //protects the page from non-logged in users
            if(userId == null) // no user present
            {
                return RedirectToAction("LoginReg", "Users");
            }

            ViewBag.AllUsers = _context.Users
                .OrderBy(name => name.Admin)
                .ToList();
        
        return View();
        }

        [HttpGet("user/{id}")]
        public IActionResult UserPage(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            //protects the page from non-logged in users
            if(userId == null) // no user present
            {
                return RedirectToAction("LoginReg", "Users");
            }
            
            ViewBag.User = _context.Users
                .Find(HttpContext.Session.GetInt32("UserId"));

            ViewBag.SelectedUser = _context.Users
                .FirstOrDefault(user => user.UserId == id);

            ViewBag.AssignedTickets = _context.Tickets
                .Include(project => project.TicketFor)
                .ToList();
        
        return View();
        }

        [HttpGet("user/update/{id}")]
        public IActionResult UserProfileUpdate(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            //protects the page from non-logged in users
            if(userId == null) // no user present
            {
                return RedirectToAction("LoginReg", "Users");
            }

            var userToEdit = _context.Users
                .Find(id);

            ViewBag.UserToUpdate = userToEdit;
        
        return View(userToEdit);
        }

        [HttpGet("profile/")]
        public IActionResult MyProfile()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            //protects the page from non-logged in users
            if(userId == null) // no user present
            {
                return RedirectToAction("LoginReg", "Users");
            }

            ViewBag.User = _context.Users
                .Find(userId);

            ViewBag.AssignedTickets = _context.Tickets
                .Include(project => project.TicketFor)
                .ToList();

            ViewBag.AllProjects = _context.Projects
                .ToList();

            ViewBag.AllUsers = _context.Users
                .ToList();
        
        return View();
        }


        [HttpPost("profile/edit/{id}")]
        public IActionResult EditProfile(User profileToEdit, int id)
        {

            var profile = _context.Users
                .Find(id);

            profile.FirstName = profileToEdit.FirstName;
            profile.LastName = profileToEdit.LastName;
            profile.Email = profileToEdit.Email;

            profileToEdit.Password = profile.Password;
            profileToEdit.Confirm = profile.Confirm;
            profileToEdit.Admin = profile.Admin;

            _context.SaveChanges();
            return RedirectToAction("MyProfile");
        }

        [HttpPost("user/edit/{id}")]
        public IActionResult EditUser(User userToEdit, int id)
        {
            var user = _context.Users
                .Find(id);

            //sets previous information to updated form
            userToEdit.FirstName = user.FirstName;
            userToEdit.LastName = user.LastName;
            userToEdit.Email = user.Email;
            //Only changing the Admin
            user.Admin = userToEdit.Admin;
            //Show when It was updated
            user.UpdatedAt = DateTime.Now;
            
            _context.SaveChanges();
        
            return RedirectToAction("UserPage");
        }

        [HttpPost("user/{id}/delete")]
        public IActionResult DeleteUser(int id)
        {
            var userToDelete =_context.Users
                .Find(id);

            _context.Users.Remove(userToDelete);
            _context.SaveChanges();

            Console.WriteLine($"User: {userToDelete.FirstName}, {userToDelete.LastName} has Been Deleted");
            return RedirectToAction("UserAll");
        }




//----------------------------------------------------------
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
