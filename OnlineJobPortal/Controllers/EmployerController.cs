using OnlineJobPortal.Models;
using OnlineJobPortal.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace OnlineJobPortal.Controllers

{
    public class EmployerController : Controller
    {
        public EmployerController()
        {
        } 
        private readonly IEmployerRepository employerRepository;
        private readonly IPublicRepository publicRepository;

        public EmployerController(IEmployerRepository employerRepository, IPublicRepository publicRepository)
        {
            this.employerRepository = employerRepository;
            this.publicRepository = publicRepository;
        }
        public ActionResult Index()
        {
            try
            {
                PublicRepository publicRespository = new PublicRepository();
                var vacency = publicRespository.GetJobDetails().Where(emp => emp.EmployerID == Convert.ToInt32(Session["EmployerId"]));
                return View(vacency);
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
                
                var jobs = publicRepository.GetJobDetails();
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

        /// <summary>
        /// Display Categories
        /// </summary>
        /// <returns></returns>
        public ActionResult Categories()
        {
            
            var category = publicRepository.DisplayCategories();
            return View(category);
        }

        public ActionResult UpdateCategory(int id)
        {
            try
            {
              
                return View(publicRepository.DisplayCategories().Find(cat => cat.CategoryId == id));
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }

        }

        /// <summary>
        /// Edit category
        /// </summary>
        /// <param name="obj">Category object</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateCategory(Category obj)
        {
            try
            {
              
                if (employerRepository.UpdateCategory(obj))
                {
                    TempData["Message"] = "Category Updated";
                }
                return RedirectToAction("Categories", "Employer");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }
        /// <summary>
        /// Delete category
        /// </summary>
        /// <param name="id">Skill id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteCategory(int id)
        {
            try
            {
                
                if (employerRepository.DeleteCategory(id))
                {
                    TempData["Message"] = "Category deleted";
                }
                return RedirectToAction("Categories");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }
        /// <summary>
        /// Add category
        /// </summary>
        /// <returns></returns>
        public ActionResult AddCategory()
        {
            return View();
        }
        /// <summary>
        /// Add Category
        /// </summary>
        /// <param name="cat">Category model instance </param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddCategory(Category cat)
        {
            try
            {
                if (employerRepository.AddCategory(cat))
                {
                    TempData["Message"] = "Category added ";
                    return RedirectToAction("AddCategory");

                }
                return View();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }
        /// <summary>
        /// Edit category
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns></returns>
        public ActionResult EditCategory(int id)
        {
            try
            {
                
                var category = publicRepository.DisplayCategories().Find(cat => cat.CategoryId == id);
                return View(category);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
        /// <summary>
        /// Add job vacancy
        /// </summary>
        /// <returns></returns>
        public ActionResult AddJobVacancy()
        {
            PublicRepository publicRespository = new PublicRepository();
            var categories = publicRespository.DisplayCategories();
            return View(categories);
        }
        [HttpPost]
        public ActionResult AddJobVacancy(JobVacancy obj)
        {
            try
            {
                int employerId = Convert.ToInt32(Session["EmployerId"]);
               
                if (employerRepository.AddJobVacancy(obj, employerId))
                {
                    TempData["Message"] = "Job vaccency published....";
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }
        /// <summary>
        /// View profile
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewProfile()
        {
            
            return View(employerRepository.Employers().Find(model => model.EmployerID == Convert.ToInt32(Session["EmployerId"])));
        }
        public ActionResult UpdateProfile()
        {
            
            return View(employerRepository.Employers().Find(model => model.EmployerID == Convert.ToInt32(Session["EmployerId"])));
        }

        [HttpPost]
        public ActionResult UpdateProfile(Employer employer, HttpPostedFileBase uploadedLogo)
        {
            try
            {
                
                if (employerRepository.UpdateEmployer(employer, uploadedLogo, Convert.ToInt32(Session["EmployerId"])))
                {
                    TempData["Message"] = "Updated";
                }
                return RedirectToAction("ViewProfile");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }
        /// <summary>
        /// Diplay vacancy created by the employer
        /// </summary>
        /// <returns></returns>
        public ActionResult Vacancies()
        {
            try
            {
              
                var vacency = publicRepository.GetJobDetails().Where(emp => emp.EmployerID == Convert.ToInt32(Session["EmployerId"]));
                return View(vacency);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }
        /// <summary>
        /// Get details of the jobs
        /// </summary>
        /// <param name="id">Job id</param>
        /// <returns></returns>
        public ActionResult JobDetails(int id)
        {
            
            var jobDetails = publicRepository.GetJobDetails().Find(model => model.JobID == id);
            return View(jobDetails);

        }
        /// <summary>
        /// Update vacancy details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UpdateVacancy(int id)
        {
            
            var jobDetails = publicRepository.GetJobDetails().Find(job => job.JobID == id);
            var categories = publicRepository.DisplayCategories();

            var viewModel = new VacancyViewModel
            {
                JobVacancies = jobDetails,
                Categories = categories
            };

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult UpdateVacancy(JobVacancy jobVacancy)
        {
            try
            {
                
                if (employerRepository.UpdateJobVacancy(jobVacancy))
                {
                    TempData["Message"] = "Updated";
                }
                return RedirectToAction("Vacancies");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }
        /// <summary>
        /// Retrive applications 
        /// </summary>
        /// <param name="id">Job id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Applications(int id)
        {
            try
            {
                
                return View(employerRepository.GetJobApplications(id));
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }
        /// <summary>
        /// Approve application
        /// </summary>
        /// <param name="id">Aplication id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ApplicationApprove(int id, int aid)
        {
            try
            {
               
                if (employerRepository.JobApplicationApprove(id))
                {
                    TempData["Message"] = "Application Approved";
                }
                return RedirectToAction("Applications", new { id = aid });
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }
        /// <summary>
        /// Reject application
        /// </summary>
        /// <param name="id">Application id</param>
        [HttpGet]
        public ActionResult ApplicationReject(int id, int aid)
        {
            try
            {
                
                if (employerRepository.JobApplicationReject(id))
                {
                    TempData["Message"] = "Application Rejected";
                }
                return RedirectToAction("Applications", new { id = aid });
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }

        /// <summary>
        /// Reject application
        /// </summary>
        /// <param name="id">Application id</param>
        public ActionResult DeleteApplication(int id)
        {
            try
            {
                
                if (employerRepository.DeleteJobApplication(id))
                {
                    TempData["Message"] = "Application Deleted";
                }

                // Redirect back to the applications page or update the view accordingly
                return RedirectToAction("Applications");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }

        /// <summary>
        /// Update status to read 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ApplicationRead(int id)
        {
            try
            {
                
                if (employerRepository.JobApplicationRead(id))
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
        /// View applicant details
        /// </summary>
        /// <param name="id">Job seeker id</param>
        /// <returns></returns>
        public ActionResult JobSeekerProfile(int id)
        {
            JobSeekerRepository jobSeekerRepository = new JobSeekerRepository();
            
            var jobSeeker = jobSeekerRepository.JobSeekers().Find(model => model.SeekerId == id);
            var edu = jobSeekerRepository.GetEducationDetails(id);
            var userSkills = publicRepository.JobSeekerSkills(id);
            var viewModel = new JobSeekerProfile
            {
                JobSeekerDetails = jobSeeker,
                EducationDetails = edu,
                Skills = userSkills
            };
            return View(viewModel);
        }
        /// <summary>
        /// Employer chnage password 
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(string oldPassword, string newPassword)
        {
            try
            {
                
                if (employerRepository.ChangePassword(oldPassword, newPassword, Convert.ToInt32(Session["EmployerId"])))
                {
                    TempData["Message"] = "Password changed";
                }
                else
                {
                    TempData["Message"] = "Wrong password";
                    return View();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View("Error");
            }
        }

        /// <summary>
        /// Logout employer
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Session["EmployrId"] = null;
            Session["CompanyLogo"] = null;
            Session["EmployerUsername"] = null;
            TempData["Message"] = "Logouted";
            return RedirectToAction("Index", "Home");
        }
    }
}