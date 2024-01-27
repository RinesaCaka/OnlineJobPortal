
using OnlineJobPortal.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace OnlineJobPortal.Services
{
    public interface IPublicService
    {
        string Login(Login login);
        List<Skills> DisplaySkills();
        List<JobSeekerSkills>JobSeekerSkills(int seekerId);
        List<JobVacancy> GetJobVacancies();
        List<Category> DisplayCategories();
        List<JobDetails> GetJobDetails();
        bool CheckUsername(string username);
    }
}