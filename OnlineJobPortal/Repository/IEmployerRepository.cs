using OnlineJobPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineJobPortal.Repository
{
   
        public interface IEmployerRepository
        {
            bool EmployerRegister(Employer emp, HttpPostedFileBase logoUpload);
            bool UpdateEmployer(Employer emp, HttpPostedFileBase uploadedLogo, int employerId);
            List<Employer> Employers();
            bool AddJobVacancy(JobVacancy obj, int employerId);
            bool UpdateJobVacancy(JobVacancy obj);
            bool JobApplicationApprove(int id); 
            bool JobApplicationReject(int id);
            bool DeleteJobApplication(int id);
            bool JobApplicationRead(int id);
            List<JobApplication> GetJobApplications(int jobId);
            bool ChangePassword(string oldPassword, string newPassword, int employerId);
            bool AddCategory(Category cat);
            bool UpdateCategory(Category cat);
            bool DeleteCategory(int id);
        }
    }
