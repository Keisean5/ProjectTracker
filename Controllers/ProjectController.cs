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
        
            return Redirect($"/project/{projectToCreate.ProjectTitle}");
        }
        [HttpGet("project/{name}")]
        public IActionResult ProjectPage(string name)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            //protects the page from non-logged in users
            if(userId == null) // no user present
            {
                return RedirectToAction("LoginReg", "Users");
            }
            
            ViewBag.Project = _context.Projects
                .Include(proj => proj.PostBy)
                .FirstOrDefault(proj => proj.ProjectTitle == name);

            ViewBag.User = _context.Users
                .Find(HttpContext.Session.GetInt32("UserId"));

            //List of Tickets
            ViewBag.ProjectTickets = _context.Tickets
                .Include(user => user.MadeBy)
                .ToList();
        
        return View();
        }

        [HttpGet("project/{name}/edit")]
        public IActionResult ProjectEdit(string name)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            //protects the page from non-logged in users
            if(userId == null) // no user present
            {
                return RedirectToAction("LoginReg", "Users");
            }

            var ProjectToEdit = _context.Projects
                .FirstOrDefault(proj => proj.ProjectTitle == name);

            ViewBag.ProjectToEdit = ProjectToEdit;
        
        return View(ProjectToEdit);
        }

        [HttpPost("project/{name}/update")]
        public IActionResult UpdateProject(Project projectToUpdate, string name)
        {
            var project = _context.Projects
                .FirstOrDefault(proj => proj.ProjectTitle == name);

            project.ProjectTitle = projectToUpdate.ProjectTitle;
            project.ProjectDescription = projectToUpdate.ProjectDescription;
            project.UpdatedAt = DateTime.Now;

            projectToUpdate.UserId = project.UserId;

            _context.SaveChanges();
        
            return Redirect($"/project/{projectToUpdate.ProjectTitle}");
        }




    }
}