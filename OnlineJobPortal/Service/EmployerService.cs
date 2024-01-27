
using OnlineJobPortal.Models;
using OnlineJobPortal.Repository;
using System.Collections.Generic;
using System.Web;
using Unity;
namespace OnlineJobPortal.Services
{
    public class EmployerService : IEmployerService
    {

        private readonly IEmployerRepository _employerRepository;

        public EmployerService(IEmployerRepository employerRepository)
        {
            _employerRepository = employerRepository;
        }

        public bool EmployerRegister(Employer emp, HttpPostedFileBase logoUpload)
        {
            return _employerRepository.EmployerRegister(emp, logoUpload);
        }

        public bool UpdateEmployer(Employer emp, HttpPostedFileBase uploadedLogo, int employerId)
        {
            return _employerRepository.UpdateEmployer(emp, uploadedLogo, employerId);
        }

        
        public bool ChangePassword(string oldPassword, string newPassword, int employerId)
        {
            return _employerRepository.ChangePassword(oldPassword, newPassword, employerId);
        }

        public List<Employer>Employers()
        {
            return _employerRepository.Employers();
        }

        public bool AddJobVacancy(JobVacancy vacancy, int employerId)
        {
            return _employerRepository.AddJobVacancy(vacancy, employerId);
        }

        public bool UpdateJobVacancy(JobVacancy vacancy)
        {
            return _employerRepository.UpdateJobVacancy(vacancy);
        }

        public List<JobApplication> GetJobApplications(int jobId)
        {
            return _employerRepository.GetJobApplications(jobId);
        }

        public bool JobApplicationApprove(int applicationId)
        {
            return _employerRepository.JobApplicationApprove(applicationId);
        }

        public bool JobApplicationReject(int applicationId)
        {
            return _employerRepository.JobApplicationReject(applicationId);
        }

        public bool DeleteJobApplication(int applicationId)
        {
            return _employerRepository.DeleteJobApplication(applicationId);
        }

        public bool JobApplicationRead(int applicationId)
        {
            return _employerRepository.JobApplicationRead(applicationId);
        }

        public bool AddCategory(Category cat)
        {
            return _employerRepository.AddCategory(cat);
        }

        public bool UpdateCategory(Category cat)
        {
            return _employerRepository.UpdateCategory(cat);
        }

        public bool DeleteCategory(int categoryId)
        {
            return _employerRepository.DeleteCategory(categoryId);
        }
    }
}