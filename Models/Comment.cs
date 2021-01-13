using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProjectTracker.Models
{
    public class Comment
    {
        [Key]
        public int CommentId {get;set;}

        public string CommentText {get;set;}


        public DateTime CreatedAt {get;set;} = DateTime.Now.Date;
        public DateTime UpdatedAt {get;set;} = DateTime.Now.Date;

        //User who Commented
        public int UserId {get;set;}
        public User CommentBy {get;set;}

        //Comment for Ticket
        public int TicketId {get;set;}
        public Ticket TicketComment {get;set;}

    }
}