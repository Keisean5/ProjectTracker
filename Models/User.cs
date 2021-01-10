using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ProjectTracker.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please Provide Your First Name")]
        [MinLength(2, ErrorMessage = "First Name must be 2 characters or longer!")]
        public string FirstName {get;set;}

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please Provide Your Last Name")]
        [MinLength(2, ErrorMessage = "Last Name must be 2 characters or longer!")]
        public string LastName {get;set;}


        [EmailAddress]
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Please Provide An Email Address")]
        public string Email {get;set;}

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please Provide A Password")]
        [MinLength(8, ErrorMessage = "Password must be 8 characters or longer!")]
        public string Password {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now.Date;
        public DateTime UpdatedAt {get;set;} = DateTime.Now.Date;

        [NotMapped]
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Please Confirm Your Password")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string Confirm {get;set;}

        public string Admin {get;set;} = "User";

        public List<Project> PostedProjects {get;set;}
        public List<Ticket> PostedTickets {get;set;}

        //add info for assigned Tickets/Projects
    }
}