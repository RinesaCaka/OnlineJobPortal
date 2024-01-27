using OnlineJobPortal.Models;
using OnlineJobPortal.Repository;
using System.Collections.Generic;
using System.Web.Mvc;

namespace OnlineJobPortal.Services
{
    public class PublicService : IPublicService
    {
        private readonly IPublicRepository _publicRepository;

        public PublicService(IPublicRepository publicRepository)
        {
            _publicRepository = publicRepository;
        }


        public string Login(Login login)
        {
            return _publicRepository.Login(login);
        }

        public List<Skills> DisplaySkills()
        {
            return _publicRepository.DisplaySkills();
        }

        public List<JobSeekerSkills> JobSeekerSkills(int seekerId)
        {
            return _publicRepository.JobSeekerSkills(seekerId);
        }

        public List<JobVacancy> GetJobVacancies()
        {
            return _publicRepository.GetJobVacancies();
        }

        public List<Category> DisplayCategories()
        {
            return _publicRepository.DisplayCategories();
        }

        public List<JobDetails> GetJobDetails()
        {
            return _publicRepository.GetJobDetails();
        }

        public bool CheckUsername(string username)
        {
            return _publicRepository.CheckUsername(username);
        }
    }
}