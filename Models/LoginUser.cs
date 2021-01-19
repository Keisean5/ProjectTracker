using System.ComponentModel.DataAnnotations;

namespace ProjectTracker.Models
{
    public class LoginUser
    {
        [Display(Name = "Login Email")]
        public string LoginEmail {get;set;}

        [Display(Name = "Login Password")]
        [DataType(DataType.Password)]
        public string LoginPassword {get;set;}
    }
}