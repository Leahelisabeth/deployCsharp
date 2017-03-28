using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace dojoPrep.Models

{
    public class Activity : BaseEntity
    {
        [Key]
        public int ActId { get; set; }
        public string Title {get; set;}
        public DateTime DateStart {get; set;}//is both TimeStart and DateStart user selected in one.
        //Add(TimeSpan) --returns new date time that add value of timespane
        public TimeSpan TimeStart {get; set;}
        public Double Duration {get; set;}
        //Add(TimeSpan)
        //Compare(TimeSpan, TimeSpan) --see if first value is short than equal to or longer than second value
        //Equality(TimeSpan, TimeSpan) --returns a value indicatinf whther two time spans are equal
        //subtract(TimeSpan) 
        //Addtion(TimeSpan, TimeSpan)
        //FromDays(Double)
        //FromHours(Double)
        //FromMinutes(Double)
        //GreaterThanOrEqual(TimeSpan, TimeSpan) 
        //LessThanOrEqual(TimeSpan, TimeSpan)	!!!!! i think this one is what i want, time start comparent time end.
        [RequiredAttribute]
        public TimeSpan TimeEnd {get;set;}
        [RequiredAttribute]
        public DateTime DateEnd {get; set;}
        public string Units {get; set;}
        //subtract(TimeSpan)
        //CompareAttribute(datetime, datetime)
        //Addition(DateTime, TimeSpan) yielding a new Datetime!! that is time end yay ^_^
        //LessThanOrEqual(DateTime, DateTime) -- if one is less than or equal to another!
        public string Desc {get;set;}
        //---------------------------------------------
        public List<Joins> Going {get; set;}
        //------------------------------------
        [RequiredAttribute]
        public User Creator {get; set;}
        //------------------------------
        [RequiredAttribute]
        [ForeignKey("UserId")]
        public int CreatorId {get; set;}
        //-----------------------------------------Events------------
        [RequiredAttribute]
        public DateTime UpdatedAt { get; set; }
        //------------------------------------------------
        [RequiredAttribute]
        public DateTime CreatedAt { get; set; }

        public string Action {get; set;}
        public Activity(){
            Creator = new User();
            Going = new List<Joins>();
        }


    }
}