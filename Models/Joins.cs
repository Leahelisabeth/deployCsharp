using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace dojoPrep.Models

{
    public class Joins : BaseEntity
    {
        [Key]
        public int JoinId { get; set; }
        //-----------------------------------------
        [RequiredAttribute]
        [ForeignKey("ActivityId")]
        public int ActId {get; set;}
        public Activity Activty { get; set; }
        //----------------------------------------------------
        public User User {get; set;}

        //---------------------------------------------
        [RequiredAttribute]
        [ForeignKey("UserId")]
        public int UserId {get; set;}
    }
}