using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace dojoPrep.Models

{
    public class User: BaseEntity
    {
        [Key]
        public int UserId { get; set; }
        //-----------------------------------------------------
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        //-----------------------------------------------------

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        //-----------------------------------------------------
        [Display(Name = "Password")]
        public string Password { get; set; }
        //-----------------------------------------------------
        [Display(Name = "Email")]
        public string Email { get; set; }
        //-----------------------------------------------------
        public DateTime UpdatedAt { get; set; }
        //-----------------------------------------------------
        public DateTime CreatedAt { get; set; }
        //-----------------------------------------------------
        public List<Activity> Created {get; set;} //******************
        //-----------------------------------------------------
        //-----------------------------------------------------
        public List<Joins> GoingTo {get; set;}
        public User(){
            Created = new List<Activity>();
            GoingTo = new List<Joins>();
            
        }
    }
}