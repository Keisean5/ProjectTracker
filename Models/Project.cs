using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProjectTracker.Models
{
    public class Project
    {
        [Key]
        public int ProjectId {get;set;}

        [Display(Name = "Project Title")]
        [Required(ErrorMessage="Title Must Be Provided")]
        [MinLength(2, ErrorMessage = "Title must be 2 characters or longer!")]
        public string ProjectTitle {get;set;}

        [Display(Name = "Project Description")]
        [Required(ErrorMessage="Description Must Be Provided")]
        [MinLength(10, ErrorMessage = "Description must be 10 characters or longer!")]
        public string ProjectDescription {get;set;}

        [Display(Name = "Admin Assigned")]
        public string AdminAssigned {get;set;}

        [Display(Name = "Project Status")]
        public string ProjectStatus {get;set;} = "Incomplete";

        public DateTime CreatedAt {get;set;} = DateTime.Now.Date;
        public DateTime UpdatedAt {get;set;} = DateTime.Now.Date;

        //Admin who submitted Project
        // public int UserId {get;set;}
        // public User PostBy {get;set;}

        public List<Ticket> PostedTicket {get;set;}
    }
}