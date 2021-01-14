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

        public string TicketTitle {get;set;}

        public string TicketDescription {get;set;}

        public string TicketPriority {get;set;}

        public string TicketStatus {get;set;} = "Open";

        public DateTime CreatedAt {get;set;} = DateTime.Now.Date;
        public DateTime UpdatedAt {get;set;} = DateTime.Now.Date;

        public int UserId {get;set;}
        public User MadeBy {get;set;}
        public int ProjectId {get;set;}
        public Project TicketFor {get;set;}

        public List<Comment> PostedComments {get;set;}

        public List<Assign> AssignedTickets {get;set;}
    }
}