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
    public class TicketController : Controller
    {
        private readonly MyContext _context;

        public TicketController(MyContext myContext)
        {
            _context = myContext;
        }


        [HttpGet("project/{name}/ticket/new")]
        public IActionResult TicketNew(string name)
        {
            //Protects page info
            int? userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null)
            {
                return RedirectToAction("LoginReg", "Users");
            }

            var projectId = _context.Projects
                .FirstOrDefault(proj => proj.ProjectTitle == name);

            ViewBag.User = _context.Users
                .Find(userId);
        
            ViewBag.Project = projectId;

        return View();
        }

        [HttpPost("project/{name}/ticket/create")]
        public IActionResult CreateTicket(Ticket ticketToCreate, string name)
        {
            // if(!ModelState.IsValid)
            // {
            //     ViewBag.Project = _context.Projects
            //         .Find(id);
            //     ticketToCreate.ProjectId = id;
            //     return View("TicketNew", ticketToCreate);
            // }

            //Assign User who Posted the Ticket
            ticketToCreate.UserId = HttpContext.Session.GetInt32("UserId").GetValueOrDefault();

            //Grab project id
            var project = _context.Projects
                .FirstOrDefault(proj => proj.ProjectTitle == name);
            //Assign ticket to Project
            ticketToCreate.ProjectId = project.ProjectId;

            _context.Add(ticketToCreate);
            _context.SaveChanges();
            Console.WriteLine("A Ticket was Created");
            //Go Back to the Project Details
            return Redirect($"/project/{project.ProjectTitle}");
        }

        [HttpGet("project/{name}/ticket/{id}")]
        public IActionResult TicketPage(string name,int id)
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

            ViewBag.Project = _context.Projects
                .FirstOrDefault(proj => proj.ProjectTitle == name);

            ViewBag.User = _context.Users
                .Find(HttpContext.Session.GetInt32("UserId"));
        
        return View();
        }


        [HttpPost("project/{name}/ticket/{id}/complete")]
        public IActionResult TicketComplete(Ticket ticketToComplete, string name, int id)
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

        [HttpPost("project/{name}/ticket/{id}/incomplete")]
        public IActionResult TicketIncomplete(Ticket ticketToIncomplete, string name, int id)
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


        [HttpGet("project/{name}/ticket/{id}/edit")]
        public IActionResult TicketEdit(string name, int id)
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

        [HttpPost("project/{name}/ticket/update/{id}")]
        public IActionResult UpdateTicket(Ticket ticketToUpdate,string name, int id)
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

        [HttpPost("project/{name}/ticket/{id}/delete")]
        public IActionResult DeleteTicket(string name, int id)
        {
            var ticketToDelete = _context.Tickets
                .Find(id);
            var projectId = ticketToDelete.ProjectId;
            _context.Tickets.Remove(ticketToDelete);
            _context.SaveChanges();
        
            Console.WriteLine(projectId);
            Console.WriteLine("A Ticket has been Deleted");
            return RedirectToAction("ProjectPage", "Project");
        }



    }
}