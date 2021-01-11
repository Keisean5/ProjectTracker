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

        [HttpGet("home")]
        public IActionResult UserProfile()//Main Page
        {

            // Console.WriteLine(HttpContext.Session.GetInt32("UserId"));
            int? userId = HttpContext.Session.GetInt32("UserId");
            //protects the page from non-logged in users
            if(userId == null) // no user present
            {
                return RedirectToAction("LoginReg", "Users");
            }

            ViewBag.User = _context.Users
                .Find(userId);

            ViewBag.AllProjects = _context.Projects
                .Include(proj => proj.PostBy)
                .ToList();

            return View();
        }

//-----------------Users---------------------

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

        [HttpGet("profile/{id}")]
        public IActionResult MyProfile(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            //protects the page from non-logged in users
            if(userId == null) // no user present
            {
                return RedirectToAction("LoginReg", "Users");
            }

            ViewBag.User = _context.Users
                .Find(id);
        
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

//----------------End of Users--------------------


//--------------Projects-----------------------
        [HttpGet("project/new")] //Will be for Admins Only
        public IActionResult ProjectNew()
        {
            //Protects page info
            int? userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null)
            {
                return RedirectToAction("LoginReg", "Users");
            }

            //Allows to show the name on the View Page if Needed
            ViewBag.User = _context.Users
                .Find(userId);
        
        return View();
        }

        [HttpPost("project/create")]
        public IActionResult CreateProject(Project projectToCreate)
        {
            if(!ModelState.IsValid)
            {
                return View("NewProject");
            }

            //Project Already in DB
            var existingProject = _context.Projects
                .FirstOrDefault(proj => proj.ProjectTitle == projectToCreate.ProjectTitle);
            if(existingProject != null)
            {
                ModelState.AddModelError("ProjectTitle", "This Project is Already Listed");
                return View("ProjectNew");
            }

            projectToCreate.UserId = HttpContext.Session.GetInt32("UserId").GetValueOrDefault();

            _context.Add(projectToCreate);
            _context.SaveChanges();
        
            return Redirect($"/project/{projectToCreate.ProjectId}");
        }

        [HttpGet("project/{id}")]
        public IActionResult ProjectPage(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            //protects the page from non-logged in users
            if(userId == null) // no user present
            {
                return RedirectToAction("LoginReg", "Users");
            }
            
            ViewBag.Project = _context.Projects
                .Include(proj => proj.PostBy)
                .FirstOrDefault(proj => proj.ProjectId == id);

            ViewBag.User = _context.Users
                .Find(HttpContext.Session.GetInt32("UserId"));

            //List of Tickets
            ViewBag.ProjectTickets = _context.Tickets
                .Include(user => user.MadeBy)
                .ToList();
        
        return View();
        }
//------------End of Projects----------------------


//---------------Tickets---------------------------

        [HttpGet("project/{id}/ticket/new")]
        public IActionResult TicketNew(int id)
        {
            //Protects page info
            int? userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null)
            {
                return RedirectToAction("LoginReg", "Users");
            }

            var projectId = _context.Projects
                .Find(id);

            ViewBag.User = _context.Users
                .Find(userId);
        
            ViewBag.Project = projectId;

        return View();
        }

        [HttpPost("project/{id}/ticket/create")]
        public IActionResult CreateTicket(Ticket ticketToCreate, int id)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Project = _context.Projects
                    .Find(id);
                ticketToCreate.ProjectId = id;
                return View("TicketNew", ticketToCreate);
            }

            //Assign User who Posted the Ticket
            ticketToCreate.UserId = HttpContext.Session.GetInt32("UserId").GetValueOrDefault();

            //Grab project id
            var projectId = id;
            //Assign ticket id to Project
            ticketToCreate.ProjectId = id;

            _context.Add(ticketToCreate);
            _context.SaveChanges();
        
            //Go Back to the Project Details
            return Redirect($"/project/{ticketToCreate.ProjectId}");
        }

        [HttpGet("ticket/{id}")]
        public IActionResult TicketPage(int id)
        {
            //Protects page info
            int? userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null)
            {
                return RedirectToAction("LoginReg", "Users");
            }

            ViewBag.Ticket = _context.Tickets
                .Include(tick => tick.TicketFor)
                .FirstOrDefault(tick => tick.TicketId == id);

            ViewBag.User = _context.Users
                .Find(HttpContext.Session.GetInt32("UserId"));
        
        return View();
        }


        [HttpPost("ticket/{id}/complete")]
        public IActionResult TicketComplete(Ticket ticketToComplete, int id)
        {
            var ticket = _context.Tickets
                .Find(id);

            ticketToComplete.TicketTitle = ticket.TicketTitle;
            ticketToComplete.TicketDescription = ticket.TicketDescription;
            ticketToComplete.TicketPriority = ticket.TicketPriority;
            ticketToComplete.UserId = ticket.UserId;
            ticketToComplete.ProjectId = ticket.ProjectId;

            ticket.TicketStatus = "Closed";

            ticket.UpdatedAt = DateTime.Now;

            _context.SaveChanges();


            return RedirectToAction("TicketPage");
        }

        [HttpPost("ticket/{id}/incomplete")]
        public IActionResult TicketIncomplete(Ticket ticketToIncomplete, int id)
        {
            var ticket = _context.Tickets
                .Find(id);

            ticketToIncomplete.TicketTitle = ticket.TicketTitle;
            ticketToIncomplete.TicketDescription = ticket.TicketDescription;
            ticketToIncomplete.TicketPriority = ticket.TicketPriority;
            ticketToIncomplete.UserId = ticket.UserId;
            ticketToIncomplete.ProjectId = ticket.ProjectId;

            ticket.TicketStatus = "Open";

            ticket.UpdatedAt = DateTime.Now;

            _context.SaveChanges();


            return RedirectToAction("TicketPage");
        }


        [HttpGet("ticket/{id}/edit")]
        public IActionResult TicketEdit(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null)
            {
                return RedirectToAction("LoginReg", "Users");
            }

            var TicketToEdit = _context.Tickets
                .Find(id);

            ViewBag.TicketToEdit = TicketToEdit;
        
        return View(TicketToEdit);
        }

        [HttpPost("ticket/update/{id}")]
        public IActionResult UpdateTicket(Ticket ticketToUpdate, int id)
        {
            var ticket = _context.Tickets
                .Find(id);

            ticket.TicketTitle = ticketToUpdate.TicketTitle;
            ticket.TicketDescription = ticketToUpdate.TicketDescription;
            ticket.TicketPriority = ticketToUpdate.TicketPriority;
            
            ticketToUpdate.TicketStatus = ticket.TicketStatus;
            ticketToUpdate.UserId = ticket.UserId;
            ticketToUpdate.ProjectId = ticket.ProjectId;

            ticket.UpdatedAt = DateTime.Now;

            _context.SaveChanges();

            return RedirectToAction("TicketPage");
        }



//--------------End of Tickets---------------------


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
