using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace dojoPrep.Models

{
    public class ActivityViewModel : BaseEntity
    {
        [Required]
        [MinLengthAttribute(2)]
        public string Title {get; set;}
        [RequiredAttribute]
        [InTheFuture]
        public DateTime DateStart {get; set;}
        //Add(TimeSpan) --returns new date time that add value of timespane
        [RequiredAttribute]
        public TimeSpan TimeStart {get; set;}
        [RequiredAttribute]
        public TimeSpan TimeEnd {get; set;}
        [RequiredAttribute]
        public string Units {get; set;}
        [RequiredAttribute]
        public Double Duration {get; set;}
        //Add(TimeSpan)
        //Compare(TimeSpan, TimeSpan) --see if first value is short than equal to or longer than second value
        //Equality(TimeSpan, TimeSpan) --returns a value indicatinf whther two time spans are equal
        //subtract(TimeSpan) 
        //Addtion(TimeSpan, TimeSpan)
        [RequiredAttribute]
        [MinLengthAttribute(10)]
        public string Desc {get;set;}



    }
}