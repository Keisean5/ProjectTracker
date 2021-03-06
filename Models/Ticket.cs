using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ProjectTracker.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId {get;set;}

        [Display(Name = "Ticket Title")]
        [Required(ErrorMessage="Title Must Be Provided")]
        [MinLength(2, ErrorMessage = "Title must be 2 characters or longer!")]
        public string TicketTitle {get;set;}

        [Display(Name = "Ticket Description")]
        [Required(ErrorMessage="Describe Ticket")]
        [MinLength(10, ErrorMessage = "Description must be 10 characters or longer!")]
        public string TicketDescription {get;set;}

        [Display(Name = "Ticket Priority")]
        public string TicketPriority {get;set;}

        [Display(Name = "Ticket Status")]
        public string TicketStatus {get;set;} = "Open";

        [Display(Name = "Assign Ticket")]
        public string UserAssigned {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now.Date;
        public DateTime UpdatedAt {get;set;} = DateTime.Now.Date;

        public int ProjectId {get;set;}
        public Project TicketFor {get;set;}

        public List<Comment> PostedComments {get;set;}

    }
}