using OnlineJobPortal.Models;
using OnlineJobPortal.Repository;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineJobPortal.Controllers
{
    public class HomeController : Controller

    {
        public HomeController()
        {
        }
        private readonly IPublicRepository publicRepository;
        private readonly IEmployerRepository employerRepository;
        public HomeController(IPublicRepository publicRepository,IEmployerRepository employerRepository)
        {
            this.publicRepository = publicRepository;
            this.employerRepository = employerRepository;
        }
        /// <summary>
        /// Index page controler
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Controller view for Job seeker registration
        /// </summary>
        /// <returns></returns>
        public ActionResult JobSeekerRegister()
        {
            ModelState.Clear();
            return View();
        }

        /// <summary>
        /// Controller for job seeker registration 
        /// </summary>
        /// <param name="jobSeeker">Model instance</param>
        /// <param name="imageUpload">Uploaded profile picture</param>
        /// <param name="resumeUpload">Uploaded resume</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JobSeekerRegister(JobSeeker jobSeeker, HttpPostedFileBase imageUpload, HttpPostedFileBase resumeUpload)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    JobSeekerRepository jobSeekerRepository = new JobSeekerRepository();
                    if (jobSeekerRepository.JobSeekerRegister(jobSeeker, imageUpload, resumeUpload))
                    {
                        TempData["Message"] = "Registration successful ";
                        return RedirectToAction("Login");
                    }
                }
                return View();
            }
            catch (Exception)
            {
                TempData["Message"] = "Email alredy registred ";
                return View(jobSeeker);
            }

        }

        /// <summary>
        /// Login form for - Admin,JobSeeker,Employer
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Login obj)
        {
            try
            {
                PublicRepository publicRepository = new PublicRepository();
                string result = publicRepository.Login(obj);
                if (result == "JobSeeker")
                {
                    JobSeekerRepository jobSeekerRepository = new JobSeekerRepository();
                    var details = jobSeekerRepository.JobSeekers().Find(model => model.Username == obj.Username);
                    Session["SeekerId"] = details.SeekerId;
                    Session["SeekerImage"] = Convert.ToBase64String(details.Image);
                    Session["SeekerUsername"] = details.Username;
                    return RedirectToAction("Index", "JobSeeker");
                }
                else if (result == "Employer")
                {
                    EmployerRepository employerRepository = new EmployerRepository();
                    var details = employerRepository.Employers().Find(model => model.Username == obj.Username);
                    Session["EmployerId"] = details.EmployerID;
                    Session["CompanyLogo"] = Convert.ToBase64String(details.CompanyLogo);
                    Session["EmployerUsername"] = details.Username;
                    return RedirectToAction("Index", "Employer");
                }
                else
                {
                    TempData["Message"] = result;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }
        /// <summary>
        /// About page
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Employer registration view
        /// </summary>
        /// <returns></returns>
        public ActionResult EmployerRegister()
        {
            ModelState.Clear();
            return View();
        }

        /// <summary>
        /// Employer registration process
        /// </summary>
        /// <param name="emp">Employer model instance</param>
        /// <param name="logoUpload">Company logo</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EmployerRegister(Employer emp, HttpPostedFileBase logoUpload)
        {
            try
            {
                EmployerRepository employerRepository = new EmployerRepository();
                if (employerRepository.EmployerRegister(emp, logoUpload))
                {
                    TempData["Message"] = "Registred Successfully";
                    return RedirectToAction("Login");
                }
                return View();
            }
            catch (Exception)
            {
                TempData["Message"] = "Email is alredy registred";
                return View();
            }
        }

        public ActionResult Jobs()
        {
            try
            {
                PublicRepository publicRepository = new PublicRepository();
                DateTime currentDate = DateTime.Now;
                var jobs = publicRepository.GetJobDetails().Where(job => job.ApplicationDeadline >= currentDate && job.IsPublished).ToList();
                return View(jobs);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
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
                PublicRepository publicRepository = new PublicRepository();
                var jobs = publicRepository.GetJobDetails();
                if (!string.IsNullOrEmpty(search))
                {
                    jobs = jobs.Where(job => job.JobTitle.Contains(search) || job.CategoryName.Contains(search) || job.Location.Contains(search) && job.ApplicationDeadline > DateTime.Now && job.IsPublished).ToList();
                }

                return View(jobs);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
        /// <summary>
        /// Check username is alredy existed or not
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CheckUsername(string username)
        {
            try
            {
                PublicRepository publicRepository = new PublicRepository();

                if (!publicRepository.CheckUsername(username))
                {
                    return new HttpStatusCodeResult(200);
                }
                return new HttpStatusCodeResult(202);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }
    
}
}