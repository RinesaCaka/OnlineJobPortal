
using OnlineJobPortal.Models;
using System.Collections.Generic;
using System.Web;

namespace OnlineJobPortal.Services
{
    public interface IEmployerService
    {
        bool EmployerRegister(Employer emp, HttpPostedFileBase logoUpload);
        bool UpdateEmployer(Employer emp, HttpPostedFileBase uploadedLogo, int employerId);
        bool ChangePassword(string oldPassword, string newPassword, int employerId);
        List<Employer>Employers();
        bool AddJobVacancy(JobVacancy vacancy, int employerId);
        bool UpdateJobVacancy(JobVacancy vacancy);
        List<JobApplication> GetJobApplications(int jobId);
        bool JobApplicationApprove(int applicationId);
        bool JobApplicationReject(int applicationId);
        bool DeleteJobApplication(int applicationId);
        bool JobApplicationRead(int applicationId);
        bool AddCategory(Category cat);
        bool UpdateCategory(Category cat);
        bool DeleteCategory(int categoryId);
    }
}