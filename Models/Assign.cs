using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ProjectTracker.Models
{
    public class Assign
    {
        [Key]
        public int AssignId {get;set;}

        public int UserId {get;set;}

        public int TicketId {get;set;}

        public User AssignedUser {get;set;}

        public Ticket AssignedTicket {get;set;}

        
        public DateTime CreatedAt {get;set;} = DateTime.Now.Date;
        public DateTime UpdatedAt {get;set;} = DateTime.Now.Date;
    }
}