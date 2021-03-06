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

    public class ProjectController : Controller
    {
        private readonly MyContext _context;

        public ProjectController(MyContext myContext)
        {
            _context = myContext;
        }

        [HttpGet("projects")]
        public IActionResult ProjectsAll()//Main Page
        {

            // Console.WriteLine(HttpContext.Session.GetInt32("UserId"));
            int? userId = HttpContext.Session.GetInt32("UserId");
            //protects the page from non-logged in users
            if(userId == null) // no user present
            {
                return RedirectToAction("LoginForm", "Users");
            }

            ViewBag.User = _context.Users
                .Find(userId);

            ViewBag.AllProjects = _context.Projects
                .Include(ticket => ticket.PostedTicket)
                .ToList();

            return View();
        }

        [HttpGet("project/{name}")]
                public IActionResult ProjectPage(string name)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            //protects the page from non-logged in users
            if(userId == null) // no user present
            {
                return RedirectToAction("LoginForm", "Users");
            }

            ViewBag.AllUsers = _context.Users
                .ToList();
            
            ViewBag.Project = _context.Projects
                .FirstOrDefault(proj => proj.ProjectTitle == name);

            ViewBag.User = _context.Users
                .Find(HttpContext.Session.GetInt32("UserId"));

            //List of Tickets
            ViewBag.ProjectTickets = _context.Tickets
                .ToList();
        
        return View();
        }

        [HttpGet("project/new")] //Will be for Admins Only
        public IActionResult ProjectNew()
        {
            //Protects page info
            int? userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null)
            {
                return RedirectToAction("LoginForm", "Users");
            }

            //Allows to show the name on the View Page if Needed
            ViewBag.User = _context.Users
                .Find(userId);

            ViewBag.AdminNames = _context.Users
                .ToList();

            var user = _context.Users
                .Find(userId);

            if(user.Admin != "Admin")
            {
                return RedirectToAction("LoginForm", "Users");
            }
        
        return View();
        }

        [HttpPost("project/create")]
        public IActionResult CreateProject(Project projectToCreate)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.User = _context.Users
                .Find(HttpContext.Session.GetInt32("UserId"));
                return View("ProjectNew");
            }

            //Project Already in DB
            var existingProject = _context.Projects
                .FirstOrDefault(proj => proj.ProjectTitle == projectToCreate.ProjectTitle);
            if(existingProject != null)
            {
                ModelState.AddModelError("ProjectTitle", "This Project is Already Listed");
                return View("ProjectNew");
            }


            _context.Add(projectToCreate);
            _context.SaveChanges();
        
            return Redirect($"/project/{projectToCreate.ProjectTitle}");
        }
        


        [HttpGet("project/{name}/edit")]
        public IActionResult ProjectEdit(string name)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            //protects the page from non-logged in users
            if(userId == null) // no user present
            {
                return RedirectToAction("LoginForm", "Users");
            }

            var ProjectToEdit = _context.Projects
                .FirstOrDefault(proj => proj.ProjectTitle == name);

            ViewBag.User = _context.Users
                .Find(userId);

            ViewBag.ProjectToEdit = ProjectToEdit;

            ViewBag.AdminNames = _context.Users
                .ToList();
                
            var user = _context.Users
                .Find(userId);

            if(user.Admin != "Admin")
            {
                return RedirectToAction("LoginForm", "Users");
            }
        
        return View(ProjectToEdit);
        }

        [HttpPost("project/{name}/update")]
        public IActionResult UpdateProject(Project projectToUpdate, string name)
        {
            var project = _context.Projects
                .FirstOrDefault(proj => proj.ProjectTitle == name);

            project.ProjectTitle = projectToUpdate.ProjectTitle;
            project.ProjectDescription = projectToUpdate.ProjectDescription;
            project.AdminAssigned = projectToUpdate.AdminAssigned;
            project.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
        
            return Redirect($"/project/{projectToUpdate.ProjectTitle}");
        }

        [HttpPost("project/{name}/delete")]
        public IActionResult DeleteProject(string name)
        {
            var projectToDelete = _context.Projects
                .FirstOrDefault(proj => proj.ProjectTitle == name);

            _context.Projects.Remove(projectToDelete);
            _context.SaveChanges();
        
            return RedirectToAction("ProjectsAll");
        }

        [HttpPost("project/{name}/complete")]
        public IActionResult ProjectComplete(Project projectToComplete, string name)
        {
            var project = _context.Projects
                .FirstOrDefault(proj => proj.ProjectTitle == name);

            projectToComplete.ProjectTitle = project.ProjectTitle;
            projectToComplete.ProjectDescription = project.ProjectDescription;
            projectToComplete.AdminAssigned = project.AdminAssigned;
            
            project.ProjectStatus = "Complete";
            
            project.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
            Console.WriteLine("A project has been completed");
            return RedirectToAction("ProjectPage");
        }


        [HttpPost("project/{name}/incomplete")]
        public IActionResult ProjectIncomplete(Project projectToIncomplete, string name)
        {
            var project = _context.Projects
                .FirstOrDefault(proj => proj.ProjectTitle == name);

            projectToIncomplete.ProjectTitle = project.ProjectTitle;
            projectToIncomplete.ProjectDescription = project.ProjectDescription;
            projectToIncomplete.AdminAssigned = project.AdminAssigned;
            
            project.ProjectStatus = "Incomplete";
            
            project.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
        
            return RedirectToAction("ProjectPage");
        }

        [HttpPost("project/{name}/assign")]
        public IActionResult AssignAdmin(Project projectToAssign, string name)
        {
            var project = _context.Projects
                .FirstOrDefault(proj => proj.ProjectTitle == name);

            projectToAssign.ProjectTitle = project.ProjectTitle;
            projectToAssign.ProjectDescription = project.ProjectDescription;
            projectToAssign.ProjectStatus = project.ProjectStatus;

            project.AdminAssigned = projectToAssign.AdminAssigned;
            project.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
            Console.WriteLine("Admin was Assigned");
            return RedirectToAction("ProjectPage");
        }

    }
}