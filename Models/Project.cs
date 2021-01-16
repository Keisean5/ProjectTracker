using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProjectTracker.Models
{
    public class Project
    {
        [Key]
        public int ProjectId {get;set;}

        [Required(ErrorMessage="Title Must Be Provided")]
        public string ProjectTitle {get;set;}

        [Required(ErrorMessage="Description Must Be Provided")]
        public string ProjectDescription {get;set;}

        public string AdminAssigned {get;set;}

        public string ProjectStatus {get;set;} //if there are bugs, show "Bugs" else show "Complete" - ADD LATER

        public DateTime CreatedAt {get;set;} = DateTime.Now.Date;
        public DateTime UpdatedAt {get;set;} = DateTime.Now.Date;

        //Admin who submitted Project
        // public int UserId {get;set;}
        // public User PostBy {get;set;}

        public List<Ticket> PostedTicket {get;set;}
    }
}