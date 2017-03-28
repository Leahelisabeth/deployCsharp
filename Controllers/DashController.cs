using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dojoPrep.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace dojoPrep.Controllers
{
    public class DashController : Controller
    {
        private dojoPrepContext _context;

        public DashController(dojoPrepContext context)
        {
            _context = context;
        }
        // GET: /Home/
        [HttpGet]
        [Route("dashboard")]
        public IActionResult dashboard()
        {
            int? LogUserId = HttpContext.Session.GetInt32("users_id");
            List<Activity> AllActivity = _context.Activity
                .Include(act => act.Creator)
                .Include(act => act.Going)
                .ToList();
            foreach (var act in AllActivity)
            {
                if (act.CreatorId == LogUserId)
                {
                    System.Console.WriteLine("act.action == Delete");
                    System.Console.WriteLine(act.ActId);
                    act.Action = "Delete";
                    _context.SaveChanges();
                }
                else
                {
                    // List<Activity> notCreator =  _context.Activity
                    // .Where(act1 = act1.CreatorId != LogUserId )
                    // .Include(act1 => act1.Creator)
                    // .Include(act1 => act1.Going)
                    // .ToList();
                    foreach (var join in act.Going)
                    {
                        if (join.UserId == LogUserId)
                        {
                            System.Console.WriteLine("act.action = Leave");
                            System.Console.WriteLine(act.ActId);
                            act.Action = "Leave";
                            _context.SaveChanges();
                        }
                        else if (join.UserId != LogUserId)
                        {
                            System.Console.WriteLine("act.Action = Join");
                            System.Console.WriteLine(act.ActId);
                            act.Action = "Join";
                            _context.SaveChanges();
                        }
                    }
                }
            }
            ViewBag.Activities = AllActivity;
            ViewBag.NewError = "";
            ViewBag.UserId = HttpContext.Session.GetInt32("users_id");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View("Dash");
        }
        [HttpGet]
        [Route("New")]
        public IActionResult Create()
        {
            ViewBag.NewError = "";
            ViewBag.UserId = HttpContext.Session.GetInt32("users_id");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View("Create");
        }
        // CREATE A NEW ACTIVITY
        [HttpPostAttribute]
        [RouteAttribute("CreateNew")]
        public IActionResult CreateNew(ActivityViewModel model)
        {
            int? SessionUser = HttpContext.Session.GetInt32("users_id");
            if (ModelState.IsValid)
            {
                User Creator = _context.User
                .Where(user => user.UserId == SessionUser)
                .Include(user => user.GoingTo)
                .SingleOrDefault();
                //          ////////////////////////////////////////////////////////////////
                /// ////////////////////////////////////DAYS UNITS ADD ///////////////////////////////////////
                if (model.Units == "Days")
                {
                    System.Console.WriteLine("MADE IT HERE!!!");
                    DateTime TrueDateTimeStart = model.DateStart.Add(model.TimeStart);//what DateStart will equal 
                    TimeSpan SpanDays = TimeSpan.FromDays(model.Duration);

                    TimeSpan NewTimeEnd = model.TimeStart.Add(SpanDays);//what time end will be


                    DateTime NewDateEnd = model.DateStart.Add(NewTimeEnd);//whate Date End will equal
                    DateTime TestTimeBegin = DateTime.Today;
                    System.Console.WriteLine(TestTimeBegin);
                    //System.Console.WriteLine(DateTime.Compare(TestTimeBegin, NewDateEnd));
                    //-1 means first was earlier than second
                    //0 means its the same as the second
                    //greater than 0 means its after the second
                    //if activitystart or activity end is greater than timestart and less than time end: it is not valid.

                    System.Console.WriteLine("Diff between testtime begin and the end date of new event");
                    System.Console.WriteLine(DateTime.Compare(TestTimeBegin, TrueDateTimeStart));
                    System.Console.WriteLine("Diff between testtime begin and the start date of new event");
                    //DateTime.Compare(TestTimeBegin, TrueDateTimeStart);

                    double NewDuration = model.Duration;
                    List<Joins> CurUserJoinEventInfo = _context.Joins
                    .Where(join => join.UserId == Creator.UserId)
                    .Include(join => join.Activty)
                    .ToList();
                    foreach (var join in CurUserJoinEventInfo)
                    {
                        //if datestart greater and date end greater or if dtatestart less and and dateend less than
                        if ((DateTime.Compare(TrueDateTimeStart, join.Activty.DateEnd) > 0
                        && DateTime.Compare(TrueDateTimeStart, join.Activty.DateStart) > 0)
                        // if the time an existing event ends is before the time the new event begins it before then it checks out.
                        || (DateTime.Compare(NewDateEnd, join.Activty.DateEnd) < 0
                        && DateTime.Compare(NewDateEnd, join.Activty.DateStart) < 0))
                        {
                            // print stuff to console
                            System.Console.WriteLine(join.Activty.ActId);
                            System.Console.WriteLine("all the ids should appear here at some point");
                            System.Console.WriteLine(DateTime.Compare(TrueDateTimeStart, join.Activty.DateEnd));

                            System.Console.WriteLine(DateTime.Compare(TrueDateTimeStart, join.Activty.DateStart));
                            System.Console.WriteLine("both should be greater than 0");
                            System.Console.WriteLine(DateTime.Compare(NewDateEnd, join.Activty.DateEnd));
                            System.Console.WriteLine(DateTime.Compare(NewDateEnd, join.Activty.DateStart));
                            System.Console.WriteLine("or both should be less than 0");
                        }
                        else
                        {
                            ViewBag.NewError = "You already have an event at this time!";
                            return View("Create");
                        }
                    }

                    //what duration will be
                    // /////////////////////////////now create the object////////////////////////////////////////////////////
                    Activity newActivity = new Activity
                    {
                        Title = model.Title,
                        DateStart = TrueDateTimeStart,
                        TimeStart = model.TimeStart,
                        Duration = NewDuration,
                        Units = model.Units,
                        TimeEnd = NewTimeEnd,
                        DateEnd = NewDateEnd,
                        Desc = model.Desc,
                        CreatorId = Creator.UserId,
                        Creator = Creator,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    _context.Add(newActivity);
                    _context.SaveChanges();
                    // remeber to save activity after testing!!
                    System.Console.WriteLine(newActivity.Title);
                    System.Console.WriteLine("title");
                    System.Console.WriteLine(newActivity.DateStart);
                    System.Console.WriteLine("datestart");
                    System.Console.WriteLine(newActivity.TimeStart);
                    System.Console.WriteLine("timestart");

                    System.Console.WriteLine(newActivity.Duration);
                    System.Console.WriteLine("duartion");

                    System.Console.WriteLine(newActivity.TimeEnd);
                    System.Console.WriteLine("timeend--should be days and hours and mins");
                    System.Console.WriteLine(newActivity.DateEnd);
                    System.Console.WriteLine("date end");
                    System.Console.WriteLine(newActivity.Desc);
                    System.Console.WriteLine("desc");
                    System.Console.WriteLine(newActivity.CreatorId);
                    System.Console.WriteLine(newActivity.CreatedAt);

                    Activity thisactivity = _context.Activity.SingleOrDefault(act => act.Title == newActivity.Title && act.Desc == newActivity.Desc && act.CreatedAt == newActivity.CreatedAt);
                    Creator.Created.Add(thisactivity);
                    _context.SaveChanges();
                    // now ad creator and activity to a new join
                    Joins newJoin = new Joins
                    {
                        ActId = thisactivity.ActId,
                        Activty = thisactivity,
                        User = Creator,
                        UserId = Creator.UserId
                    };
                    // save changes for the new join
                    _context.Add(newJoin);
                    _context.SaveChanges();
                    Joins thisJoin = _context.Joins.Where(join => join.ActId == thisactivity.ActId && join.UserId == Creator.UserId)
                    .SingleOrDefault();
                    Creator.GoingTo.Add(thisJoin);
                    _context.SaveChanges();
                    thisactivity.Going.Add(thisJoin);
                    _context.SaveChanges();
                    //look for users all activities to make sure it exist
                    foreach (var act in Creator.Created)
                    {
                        System.Console.WriteLine(act.ActId);
                        System.Console.WriteLine(act.Title);
                    }
                    ViewBag.NewError = "";
                    return RedirectToAction("dashboard");

                }
                // /////////////////////////////////////////////////////////////
                // //////////////////////// HOURS UNITS ADD //////////////////////////////////////////////
                else if (model.Units == "Hours")
                {
                    System.Console.WriteLine("MADE IT HERE!!!");
                    DateTime TrueDateTimeStart = model.DateStart.Add(model.TimeStart);//what DateStart will equal 
                    TimeSpan SpanHours = TimeSpan.FromHours(model.Duration);

                    TimeSpan NewTimeEnd = model.TimeStart.Add(SpanHours);//what time end will be


                    DateTime NewDateEnd = model.DateStart.Add(NewTimeEnd);//whate Date End will equal
                    DateTime TestTimeBegin = DateTime.Today;
                    System.Console.WriteLine(TestTimeBegin);
                    System.Console.WriteLine(DateTime.Compare(TestTimeBegin, NewDateEnd));
                    //-1 means first was earlier than second
                    //0 means its the same as the second
                    //greater than 0 means its after the second
                    //if activitystart or activity end is greater than timestart and less than time end: it is not valid.

                    System.Console.WriteLine("Diff between testtime begin and the end date of new event");
                    System.Console.WriteLine(DateTime.Compare(TestTimeBegin, TrueDateTimeStart));
                    System.Console.WriteLine("Diff between testtime begin and the start date of new event");
                    //DateTime.Compare(TestTimeBegin, TrueDateTimeStart);

                    double NewDuration = model.Duration;

                    List<Joins> CurUserJoinEventInfo = _context.Joins
                    .Where(join => join.UserId == Creator.UserId)
                    .Include(join => join.Activty)
                    .ToList();
                    foreach (var join in CurUserJoinEventInfo)
                    {
                        //if datestart greater and date end greater or if dtatestart less and and dateend less than
                        if ((DateTime.Compare(TrueDateTimeStart, join.Activty.DateEnd) > 0
                        && DateTime.Compare(TrueDateTimeStart, join.Activty.DateStart) > 0)
                        // if the time an existing event ends is before the time the new event begins it before then it checks out.
                        || (DateTime.Compare(NewDateEnd, join.Activty.DateEnd) < 0
                        && DateTime.Compare(NewDateEnd, join.Activty.DateStart) < 0))
                        {
                            // print stuff to console
                            System.Console.WriteLine(join.Activty.ActId);
                            System.Console.WriteLine("all the ids should appear here at some point");
                            System.Console.WriteLine(DateTime.Compare(TrueDateTimeStart, join.Activty.DateEnd));

                            System.Console.WriteLine(DateTime.Compare(TrueDateTimeStart, join.Activty.DateStart));
                            System.Console.WriteLine("both should be greater than 0");
                            System.Console.WriteLine(DateTime.Compare(NewDateEnd, join.Activty.DateEnd));
                            System.Console.WriteLine(DateTime.Compare(NewDateEnd, join.Activty.DateStart));
                            System.Console.WriteLine("or both should be less than 0");
                        }
                        else
                        {
                            ViewBag.NewError = "You already have an event at this time!";
                            return View("Create");
                        }
                    }
                    //what duration will be
                    // /////////now create the object////////////////////////////////////////////////////
                    Activity newActivity = new Activity
                    {
                        Title = model.Title,
                        DateStart = TrueDateTimeStart,
                        TimeStart = model.TimeStart,
                        Duration = NewDuration,
                        Units = model.Units,
                        TimeEnd = NewTimeEnd,
                        DateEnd = NewDateEnd,
                        Desc = model.Desc,
                        CreatorId = Creator.UserId,
                        Creator = Creator,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    _context.Add(newActivity);
                    // remeber to save activity after testing!!
                    _context.SaveChanges();
                    System.Console.WriteLine(newActivity.Title);
                    System.Console.WriteLine("title");
                    System.Console.WriteLine(newActivity.DateStart);
                    System.Console.WriteLine("datestart");
                    System.Console.WriteLine(newActivity.TimeStart);
                    System.Console.WriteLine("timestart");

                    System.Console.WriteLine(newActivity.Duration);
                    System.Console.WriteLine("duartion");

                    System.Console.WriteLine(newActivity.TimeEnd);
                    System.Console.WriteLine("timeend--should be days and hours and mins");
                    System.Console.WriteLine(newActivity.DateEnd);
                    System.Console.WriteLine("date end");
                    System.Console.WriteLine(newActivity.Desc);
                    System.Console.WriteLine("desc");
                    System.Console.WriteLine(newActivity.CreatorId);
                    System.Console.WriteLine(newActivity.CreatedAt);
                    Activity thisactivity = _context.Activity.SingleOrDefault(act => act.Title == newActivity.Title && act.CreatedAt == newActivity.CreatedAt);
                    Creator.Created.Add(thisactivity);
                    _context.SaveChanges();
                    Joins newJoin = new Joins
                    {
                        ActId = thisactivity.ActId,
                        Activty = thisactivity,
                        User = Creator,
                        UserId = Creator.UserId
                    };
                    // save changes for the new join
                    _context.Add(newJoin);
                    _context.SaveChanges();
                    Joins thisJoin = _context.Joins.Where(join2 => join2.ActId == thisactivity.ActId && join2.UserId == Creator.UserId)
                    .SingleOrDefault();
                    Creator.GoingTo.Add(thisJoin);
                    _context.SaveChanges();
                    thisactivity.Going.Add(thisJoin);
                    _context.SaveChanges();
                    //look for users all activities to make sure it exist
                    foreach (var act in Creator.Created)
                    {
                        System.Console.WriteLine(act.ActId);
                        System.Console.WriteLine(act.Title);
                    }
                    ViewBag.NewError = "";
                    return RedirectToAction("dashboard");

                }
                // ///////////////////////////////////////////////////////////////
                // //////////////////////////MINUTES UNITS ADD ///////////////////////////
                else if (model.Units == "Minutes")
                {
                    System.Console.WriteLine("MADE IT HERE!!!");
                    DateTime TrueDateTimeStart = model.DateStart.Add(model.TimeStart);//what DateStart will equal 
                    TimeSpan SpanMins = TimeSpan.FromMinutes(model.Duration);

                    TimeSpan NewTimeEnd = model.TimeStart.Add(SpanMins);//what time end will be


                    DateTime NewDateEnd = model.DateStart.Add(NewTimeEnd);//whate Date End will equal
                    DateTime TestTimeBegin = DateTime.Today;
                    System.Console.WriteLine(TestTimeBegin);
                    System.Console.WriteLine(DateTime.Compare(TestTimeBegin, NewDateEnd));
                    //-1 means first was earlier than second
                    //0 means its the same as the second
                    //greater than 0 means its after the second
                    //if activitystart or activity end is greater than timestart and less than time end: it is not valid.

                    System.Console.WriteLine("Diff between testtime begin and the end date of new event");
                    System.Console.WriteLine(DateTime.Compare(TestTimeBegin, TrueDateTimeStart));
                    System.Console.WriteLine("Diff between testtime begin and the start date of new event");
                    //DateTime.Compare(TestTimeBegin, TrueDateTimeStart);

                    double NewDuration = model.Duration;//what duration will be
                    List<Joins> CurUserJoinEventInfo = _context.Joins
                    .Where(join => join.UserId == Creator.UserId)
                    .Include(join => join.Activty)
                    .ToList();
                    foreach (var join in CurUserJoinEventInfo)
                    {
                        //if datestart greater and date end greater or if dtatestart less and and dateend less than
                        if ((DateTime.Compare(TrueDateTimeStart, join.Activty.DateEnd) > 0
                        && DateTime.Compare(TrueDateTimeStart, join.Activty.DateStart) > 0)
                        // if the time an existing event ends is before the time the new event begins it before then it checks out.
                        || (DateTime.Compare(NewDateEnd, join.Activty.DateEnd) < 0
                        && DateTime.Compare(NewDateEnd, join.Activty.DateStart) < 0))
                        {
                            // print stuff to console
                            System.Console.WriteLine(join.Activty.ActId);
                            System.Console.WriteLine("all the ids should appear here at some point");
                            System.Console.WriteLine(DateTime.Compare(TrueDateTimeStart, join.Activty.DateEnd));

                            System.Console.WriteLine(DateTime.Compare(TrueDateTimeStart, join.Activty.DateStart));
                            System.Console.WriteLine("both should be greater than 0");
                            System.Console.WriteLine(DateTime.Compare(NewDateEnd, join.Activty.DateEnd));
                            System.Console.WriteLine(DateTime.Compare(NewDateEnd, join.Activty.DateStart));
                            System.Console.WriteLine("or both should be less than 0");
                        }
                        else
                        {
                            ViewBag.NewError = "You already have an event at this time!";
                            return View("Create");
                        }
                    }
                    // /////////////////////////////now create the object////////////////////////////////////////////////////

                    Activity newActivity = new Activity
                    {
                        Title = model.Title,
                        DateStart = TrueDateTimeStart,
                        TimeStart = model.TimeStart,
                        Duration = NewDuration,
                        Units = model.Units,
                        TimeEnd = NewTimeEnd,
                        DateEnd = NewDateEnd,
                        Desc = model.Desc,
                        CreatorId = Creator.UserId,
                        Creator = Creator,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    _context.Add(newActivity);
                    // remeber to save activity after testing!!
                    _context.SaveChanges();
                    //then filter to find it to make sure you have the correct one to add to user
                    //list of activities
                    System.Console.WriteLine(newActivity.Title);
                    System.Console.WriteLine("title");
                    System.Console.WriteLine(newActivity.DateStart);
                    System.Console.WriteLine("datestart");
                    System.Console.WriteLine(newActivity.TimeStart);
                    System.Console.WriteLine("timestart");

                    System.Console.WriteLine(newActivity.Duration);
                    System.Console.WriteLine("duartion");

                    System.Console.WriteLine(newActivity.TimeEnd);
                    System.Console.WriteLine("timeend--should be days and hours and mins");
                    System.Console.WriteLine(newActivity.DateEnd);
                    System.Console.WriteLine("date end");
                    System.Console.WriteLine(newActivity.Desc);
                    System.Console.WriteLine("desc");
                    System.Console.WriteLine(newActivity.CreatorId);
                    System.Console.WriteLine(newActivity.CreatedAt);
                    //find actiovity just created
                    Activity thisactivity = _context.Activity.SingleOrDefault(act => act.Title == newActivity.Title && act.Desc == newActivity.Desc && act.CreatedAt == newActivity.CreatedAt);
                    Creator.Created.Add(thisactivity);
                    _context.SaveChanges();
                    Joins newJoin = new Joins
                    {
                        ActId = thisactivity.ActId,
                        Activty = thisactivity,
                        User = Creator,
                        UserId = Creator.UserId
                    };
                    // save changes for the new join
                    _context.Add(newJoin);
                    _context.SaveChanges();
                    Joins thisJoin = _context.Joins.Where(join => join.ActId == thisactivity.ActId && join.UserId == Creator.UserId)
                    .SingleOrDefault();
                    Creator.GoingTo.Add(thisJoin);
                    _context.SaveChanges();
                    thisactivity.Going.Add(thisJoin);
                    _context.SaveChanges();
                    //look for users all activities to make sure it exist
                    foreach (var act in Creator.Created)
                    {
                        System.Console.WriteLine(act.ActId);
                        System.Console.WriteLine(act.Title);
                    }
                    ViewBag.NewError = "";
                    return RedirectToAction("dashboard");

                }
                //if model state IsValid but not units in days hours or mins
                ViewBag.NewError = "";
                ViewBag.UserId = HttpContext.Session.GetInt32("users_id");
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                return View("Create");

            }
            //model state WAS NOT VALID
            //redirect to show one later
            ViewBag.NewError = "";
            ViewBag.UserId = HttpContext.Session.GetInt32("users_id");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View("Create");
        }
        [HttpPostAttribute]
        [RouteAttribute("Delete/{ActionId}")]
        public IActionResult DeleteActivity(int ActionId)
        {
            int? LogUserId = HttpContext.Session.GetInt32("users_id");
            User LogUser = _context.User.SingleOrDefault(user => user.UserId == LogUserId);
            Activity CurActivity = _context.Activity.SingleOrDefault(act => act.ActId == ActionId);
            _context.Activity.Remove(CurActivity);
            _context.SaveChanges();
            System.Console.WriteLine("Deleting");
            //     System.Console.WriteLine(ActionId);
            //     List<Activity> AllActivity = _context.Activity
            // .Include(act => act.Creator)
            // .Include(act => act.Going)
            // .ToList();
            //             foreach (var act in AllActivity)
            //             {
            //                 if (act.CreatorId == LogUserId)
            //                 {
            //                     System.Console.WriteLine("act.action == Delete");
            //                     System.Console.WriteLine(act.ActId);
            //                     act.Action = "Delete";
            //                     _context.SaveChanges();
            //                 }
            //                 else
            //                 {
            //                     foreach (var join1 in act.Going)
            //                     {
            //                         if (join1.UserId == LogUserId)
            //                         {
            //                             System.Console.WriteLine("act.action = Leave");
            //                             System.Console.WriteLine(act.ActId);
            //                             act.Action = "Leave";
            //                             _context.SaveChanges();
            //                         }
            //                         else if (join1.UserId != LogUserId && act.Action != "Leave")
            //                         {
            //                             System.Console.WriteLine("act.Action = Join");
            //                             System.Console.WriteLine(act.ActId);
            //                             act.Action = "Join";
            //                             _context.SaveChanges();
            //                         }
            //                     }
            //                 }
            //             }
            ViewBag.NewError = "";
            //ViewBag.Activities = AllActivity;
            ViewBag.UserId = HttpContext.Session.GetInt32("users_id");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return RedirectToAction("dashboard");
        }
        [HttpPostAttribute]
        [RouteAttribute("Join/{ActionId}")]
        public IActionResult JoinActivity(int ActionId)
        {
            //get user 
            int? LogUserId = HttpContext.Session.GetInt32("users_id");
            User LogUser = _context.User.SingleOrDefault(user => user.UserId == LogUserId);

            //find current event
            Activity CurActivity = _context.Activity.SingleOrDefault(act => act.ActId == ActionId);

            // find that user's Joins
            List<Joins> CurUserJoinEventInfo = _context.Joins
                    .Where(join => join.UserId == LogUserId)
                    .Include(join => join.Activty)
                    .ToList();
            // loop through check if any match current event

            foreach (var join in CurUserJoinEventInfo)
            {
                //if datestart greater and date end greater or if dtatestart less and and dateend less than
                if ((DateTime.Compare(CurActivity.DateStart, join.Activty.DateEnd) > 0
                && DateTime.Compare(CurActivity.DateStart, join.Activty.DateStart) > 0)
                // if the time an existing event ends is before the time the new event begins it before then it checks out.
                || (DateTime.Compare(CurActivity.DateEnd, join.Activty.DateEnd) < 0
                && DateTime.Compare(CurActivity.DateEnd, join.Activty.DateStart) < 0))
                {
                    // print stuff to console
                    System.Console.WriteLine(join.Activty.ActId);
                    System.Console.WriteLine("all the ids should appear here at some point");
                    System.Console.WriteLine(DateTime.Compare(CurActivity.DateStart, join.Activty.DateEnd));

                    System.Console.WriteLine(DateTime.Compare(CurActivity.DateStart, join.Activty.DateStart));
                    System.Console.WriteLine("both should be greater than 0");
                    System.Console.WriteLine(DateTime.Compare(CurActivity.DateEnd, join.Activty.DateEnd));
                    System.Console.WriteLine(DateTime.Compare(CurActivity.DateEnd, join.Activty.DateStart));
                    System.Console.WriteLine("or both should be less than 0");

                }
                //event EXIST!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                else
                {
                    ViewBag.NewError = "You already have an event at this time!";
                    List<Activity> AllActivity = _context.Activity
        .Include(act => act.Creator)
        .Include(act => act.Going)
        .ToList();
                    foreach (var act in AllActivity)
                    {
                        if (act.CreatorId == LogUserId)
                        {
                            System.Console.WriteLine("act.action == Delete");
                            System.Console.WriteLine(act.ActId);
                            act.Action = "Delete";
                            _context.SaveChanges();
                        }
                        else
                        {
                            foreach (var join1 in act.Going)
                            {
                                if (join1.UserId == LogUserId)
                                {
                                    System.Console.WriteLine("act.action = Leave");
                                    System.Console.WriteLine(act.ActId);
                                    act.Action = "Leave";
                                    _context.SaveChanges();
                                }
                                else
                                {
                                    System.Console.WriteLine("act.Action = Join");
                                    System.Console.WriteLine(act.ActId);
                                    act.Action = "Join";
                                    _context.SaveChanges();
                                }
                            }
                        }
                    }
                    ViewBag.Activities = AllActivity;
                    ViewBag.UserId = HttpContext.Session.GetInt32("users_id");
                    ViewBag.UserName = HttpContext.Session.GetString("UserName");
                    return View("Dash");

                }
                /// EVENT NOT EXIST
            }// foreach end
            // all checkedout
            Joins newJoin = new Joins
            {
                ActId = CurActivity.ActId,
                Activty = CurActivity,
                User = LogUser,
                UserId = LogUser.UserId
            };
            
            // save changes for the new join
            _context.Add(newJoin);
            _context.SaveChanges();
            Joins thisJoin = _context.Joins.Where(join => join.ActId == CurActivity.ActId && join.UserId == LogUser.UserId)
            .SingleOrDefault();
            LogUser.GoingTo.Add(thisJoin);
            _context.SaveChanges();
            CurActivity.Going.Add(thisJoin);
            _context.SaveChanges();
            CurActivity.Action = "Leave";
            _context.SaveChanges();
            System.Console.WriteLine("Joining");
            System.Console.WriteLine(ActionId);
            return RedirectToAction("dashboard");
        }
        [HttpPostAttribute]
        [RouteAttribute("Leave/{ActionId}")]
        public IActionResult LeaveActivity(int ActionId)
        {
            int? LogUserId = HttpContext.Session.GetInt32("users_id");
            User LogUser = _context.User.SingleOrDefault(user => user.UserId == LogUserId);

            //find current event
            Activity CurActivity = _context.Activity.SingleOrDefault(act => act.ActId == ActionId);
            Joins LeaveJoin = _context.Joins.Where(join => join.UserId == LogUserId && join.ActId == ActionId)
            .SingleOrDefault();
            _context.Joins.Remove(LeaveJoin);
            _context.SaveChanges();
            CurActivity.Action = "Join";
            _context.SaveChanges();


            return RedirectToAction("dashboard");
        }
        [HttpGetAttribute]
        [RouteAttribute("activity/{ActionId}")]
        public IActionResult ShowOne(int ActionId)
        {
            int? LogUserId = HttpContext.Session.GetInt32("users_id");
            ViewBag.LogUser = _context.User.SingleOrDefault(user => user.UserId == LogUserId);

            //find current event
            Activity CurActivity = _context.Activity
            .Include(act => act.Creator)
            .SingleOrDefault(act => act.ActId == ActionId);
            List<Joins> JoinedThis = _context.Joins
            .Where(j => j.ActId == CurActivity.ActId && j.UserId != LogUserId)
            .Include(j => j.User)
            .ToList();
            ViewBag.Activity = CurActivity;
            ViewBag.Joins = JoinedThis;
            return View("SHowOne");
        }

    }
}
