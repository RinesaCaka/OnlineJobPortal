using OnlineJobPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineJobPortal.Repository
{
    public interface IPublicRepository
    {
        string Login(Login login);
        List<Skills> DisplaySkills();
        List<JobSeekerSkills> JobSeekerSkills(int seekerId);
        List<JobVacancy> GetJobVacancies();
        List<Category> DisplayCategories();
        List<JobDetails> GetJobDetails();
        bool CheckUsername(string username);
    }
}