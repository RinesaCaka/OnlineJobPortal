using OnlineJobPortal.Models;
using OnlineJobPortal.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using OnlineJobPortal.Services;

namespace OnlineJobPortal.Controllers
{
    public class JobSeekerController : Controller
    {
        public JobSeekerController()
        {
        }

        private readonly IJobSeekerService jobSeekerService;
        private readonly IPublicService publicService;

        // Constructor for dependency injection
        public JobSeekerController( IJobSeekerService jobSeekerService, IPublicService publicService)
        {
          
            this.jobSeekerService = jobSeekerService;
            this.publicService = publicService;
        }
        private bool IsValid()
        {
            return Session["SeekerId"] != null;
        }
        // GET: JobSeeker
        public ActionResult Index()
        {
            bool isValid = IsValid();
            if (isValid)
            {
                PublicRepository publicRepository = new PublicRepository();
                var jobs = publicRepository.GetJobDetails();
                JobSeekerRepository repo = new JobSeekerRepository();
                int id = Convert.ToInt32(Session["SeekerId"]);
                var applications = repo.GetJobApplications(id);
                return View(new Index { JobApplications = applications, JobDetails = jobs });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Display profile of the job seeker
        /// </summary>
        /// <returns></returns>
        public ActionResult JobSeekerProfile()
        {
            try
            {
                
                PublicRepository repo = new PublicRepository();
                int seekerId = (int)Session["SeekerId"];
                var jobSeeker = jobSeekerService.JobSeekers().Find(model => model.SeekerId == seekerId);
                var edu = jobSeekerService.GetEducationDetails(seekerId);
                var userSkills = repo.JobSeekerSkills(seekerId).ToList();
                var userSkillsId = repo.JobSeekerSkills(seekerId).Select(js => js.SkillId).ToList();

                var skills = repo.DisplaySkills().Where(skil => !userSkillsId.Contains(skil.SkillId)).ToList();
                var viewModel = new JobSeekerProfile
                {
                    JobSeekerDetails = jobSeeker,
                    EducationDetails = edu,
                    Skills = userSkills,
                    AllSkills = skills
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }
        public ActionResult UpdateProfile()
        {
            ;
            var jobSeeker = jobSeekerService.JobSeekers().Find(model => model.SeekerId == (int)Session["SeekerId"]);
            return View(jobSeeker);

        }
        /// <summary>
        /// Update profile job seeker
        /// </summary>
        /// <param name="jobSeeker">Job seeker instance</param>
        /// <param name="imageUpload">Uploaded image</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateProfile(JobSeeker jobSeeker, HttpPostedFileBase imageUpload)
        {
            try
            {
                
                if (jobSeekerService.JobSeekerUpdate(jobSeeker, imageUpload, Convert.ToInt32(Session["SeekerId"])))
                {
                    TempData["Message"] = "Updated";
                }
                return RedirectToAction("JobSeekerProfile");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }

        }

        public ActionResult AddEducationDetails()
        {
            return View();
        }
        [HttpPost]
        /// <summary>
        /// To add eduction details
        /// </summary>
        /// <param name="educationList">Form contain list of all the eduction deatils of the user</param>
        /// <returns></returns>
        public ActionResult AddEducationDetails(EducationDetails educationList)
        {
            try
            {
                int id = (int)Session["SeekerId"];
                
                if (jobSeekerService.AddEducationDetails(educationList, id))
                {
                    TempData["Message"] = "Added successfully";
                    return RedirectToAction("JobSeekerProfile");
                }
                return View();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult UpdateResume(HttpPostedFileBase resumeFile)
        {
            try
            {
               
                if (jobSeekerService.UpdateResume(resumeFile, Convert.ToInt32(Session["SeekerId"])))
                {
                    TempData["Message"] = "Resume Updated";
                }
                return RedirectToAction("JobSeekerProfile");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }

        }
        public ActionResult UpdateEducationDetails(int id)
        {
           
            var educationDetails = jobSeekerService.GetEducationDetails(Convert.ToInt32(Session["SeekerId"])).Find(ed => ed.EducationId == id);
            return View(educationDetails);
        }
        /// <summary>
        /// Update Education details
        /// </summary>
        /// <param name="educationDetails"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateEducationDetails(EducationDetails educationDetails)
        {
            
            try
            {
                if (jobSeekerService.UpdateEducationDetails(educationDetails))
                {
                    TempData["Message"] = "Updated Successfully";
                }
                return RedirectToAction("JobSeekerProfile");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }
        /// <summary>
        /// Delete educational details
        /// </summary>
        /// <param name="id">Education id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteEducationDetails(int id)
        {
            
            try
            {
                if (jobSeekerService.DeleteEducationDetails(id))
                {
                    TempData["Message"] = "Deleted Successfully ";
                }
                return RedirectToAction("JobSeekerProfile");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }
     
        
        /// <summary>
        /// Display job vacancies posted by the employer
        /// </summary>
        /// <returns></returns>
        public ActionResult Jobs()
        {
            try
            {
                
                DateTime currentDate = DateTime.Now;
                var jobs = publicService.GetJobDetails().Where(job => job.ApplicationDeadline >= currentDate && job.IsPublished).ToList();
                return View(jobs);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }

        }
        /// <summary>
        /// Filter the job details based on the search string
        /// </summary>
        /// <param name="search">Search string</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Jobs(string search)
        {
            try
            {
               
                var jobs = publicService.GetJobDetails();
                if (!string.IsNullOrEmpty(search))
                {
                    jobs = jobs.Where(job => job.JobTitle.Contains(search) || job.CategoryName.Contains(search) || job.Location.Contains(search) && job.ApplicationDeadline > DateTime.Now && job.IsPublished).ToList();
                }

                return View(jobs);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }

        [HttpGet]
        /// <summary>
        /// Apply for the job 
        /// </summary>
        /// <param name="id">JobId</param>
        /// <returns></returns>
        public ActionResult ApplyJob(int id)
        {
            try
            {
                JobApplication application = new JobApplication
                {
                    SeekerId = Convert.ToInt32(Session["SeekerId"]),
                    JobApplicationID = id,
                    ApplicationDate = DateTime.Now
                };
                JobSeekerRepository repo = new JobSeekerRepository();
                if (repo.CreateJobApplication(application))
                {
                    TempData["Message"] = "Applied Successfull";
                }
                return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }
        /// <summary>
        /// Visit job to store who visisted the job
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViewJob(int id)
        {
            try
            {
                
                ViewJob obj = new ViewJob
                {
                    JobId = id,
                    SeekerId = Convert.ToInt32(Session["SeekerId"]),
                    ViewDate = DateTime.Now,
                };
                if (jobSeekerService.VisitJob(obj))
                {
                    return new HttpStatusCodeResult(200);
                }
                return new HttpStatusCodeResult(400);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(400);
            }
        }
        /// <summary>
        /// Create and delete bookmark
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Bookmark(int id)
        {
            try
            {
                
                Bookmark obj = new Bookmark
                {
                    JobId = id,
                    SeekerId = Convert.ToInt32(Session["SeekerId"]),
                };
                if (jobSeekerService.Bookmark(obj))
                {
                    return new HttpStatusCodeResult(200);
                }
                return new HttpStatusCodeResult(400);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }
        /// <summary>
        /// Display job details 
        /// </summary>
        /// <param name="id">Job id</param>
        /// <returns></returns>
        public ActionResult JobDetails(int id)
        {
            try
            {
                int seekerId = Convert.ToInt32(Session["SeekerId"]);
               
                
                var jobDetails = publicService.GetJobDetails().Find(model => model.JobID == id);
                if (jobDetails != null)
                {
                    var bookmarks = jobSeekerService.GetBookmarks(seekerId);
                    var appliedJobs = jobSeekerService.GetJobApplications(seekerId);
                    bool isSaved = bookmarks.Any(jobId => jobId.JobId == id);
                    bool isApplied = appliedJobs.Any(jobId => jobId.JobId == id);
                    ViewBag.isSaved = isSaved;
                    ViewBag.isApplied = isApplied;
                }

                return View(jobDetails);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }

        }
        /// <summary>
        /// View applied jobs and check the status
        /// </summary>
        /// <returns></returns>
        public ActionResult AppliedJobs()
        {
            
            int id = Convert.ToInt32(Session["SeekerId"]);
            return View(jobSeekerService.GetJobApplications(id));
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        /// <summary>
        /// Change password job seeker
        /// </summary>
        /// <param name="oldPassword">Old password</param>
        /// <param name="newPassword">New password</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePassword(string oldPassword, string newPassword)
        {
            try
            {
                
                if (jobSeekerService.ChangePassword(oldPassword, newPassword, Convert.ToInt32(Session["SeekerId"])))
                {
                    TempData["Message"] = "Password changed";
                }
                else
                {
                    TempData["Message"] = "Wrong password";
                    return View();
                }
                return RedirectToAction("JobSeekerProfile");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }
      
        [HttpPost]
        public ActionResult AddSkill(int[] SkillId)
        {
            
            try
            {
                foreach (int skillId in SkillId)
                {
                    if (jobSeekerService.AddSkill(skillId, Convert.ToInt32(Session["SeekerId"])))
                    {
                        TempData["Message"] = "Skills added";
                    }
                }
                return RedirectToAction("JobSeekerProfile");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }
        /// <summary>
        /// View saved jobs
        /// </summary>
        /// <returns></returns>
        public ActionResult Bookmarks()
        {
            try
            {
                
                var bookmarks = jobSeekerService.GetBookmarks(Convert.ToInt32(Session["SeekerId"]));
                return View(bookmarks);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }       
        /// <summary>
        /// Logout Employer
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Session["SeekerId"] = null;
            Session["SeekerImage"] = null;
            Session["SeekerUsername"] = null;
            TempData["Message"] = "Logouted";
            return RedirectToAction("Index", "Home");
        }
    }
}