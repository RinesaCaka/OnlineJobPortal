
using OnlineJobPortal.Models;
using OnlineJobPortal.Repository;
using System.Collections.Generic;
using System.Web;

namespace OnlineJobPortal.Services
{
    public class JobSeekerService : IJobSeekerService
    {
        private readonly IJobSeekerRepository _jobSeekerRepository;

        public JobSeekerService(IJobSeekerRepository jobSeekerRepository)
        {
            _jobSeekerRepository = jobSeekerRepository;
        }

        public bool JobSeekerRegister(JobSeeker seeker, HttpPostedFileBase imageUpload, HttpPostedFileBase resumeUpload)
        {
            return _jobSeekerRepository.JobSeekerRegister(seeker, imageUpload, resumeUpload);
        }

        public bool JobSeekerUpdate(JobSeeker seeker, HttpPostedFileBase imageUpload, int seekerId)
        {
            return _jobSeekerRepository.JobSeekerUpdate(seeker, imageUpload, seekerId);
        }

        public bool UpdateResume(HttpPostedFileBase resume, int seekerId)
        {
            return _jobSeekerRepository.UpdateResume(resume, seekerId);
        }

        public List<JobSeeker> JobSeekers()
        {
            return _jobSeekerRepository.JobSeekers();
        }

        public bool AddEducationDetails(EducationDetails details, int seekerId)
        {
            return _jobSeekerRepository.AddEducationDetails(details, seekerId);
        }

        public bool UpdateEducationDetails(EducationDetails details)
        {
            return _jobSeekerRepository.UpdateEducationDetails(details);
        }

        public bool DeleteEducationDetails(int educationId)
        {
            return _jobSeekerRepository.DeleteEducationDetails(educationId);
        }

        public bool CreateJobApplication(JobApplication application)
        {
            return _jobSeekerRepository.CreateJobApplication(application);
        }

        public List<JobApplication> GetJobApplications(int seekerId)
        {
            return _jobSeekerRepository.GetJobApplications(seekerId);
        }

        public List<EducationDetails> GetEducationDetails(int seekerId)
        {
            return _jobSeekerRepository.GetEducationDetails(seekerId);
        }

        public List<Bookmark> GetBookmarks(int seekerId)
        {
            return _jobSeekerRepository.GetBookmarks(seekerId);
        }

        public bool VisitJob(ViewJob viewJob)
        {
            return _jobSeekerRepository.VisitJob(viewJob);
        }

        public bool Bookmark(Bookmark bookmark)
        {
            return _jobSeekerRepository.Bookmark(bookmark);
        }

        public bool ChangePassword(string oldPassword, string newPassword, int seekerId)
        {
            return _jobSeekerRepository.ChangePassword(oldPassword, newPassword, seekerId);
        }

        public bool AddSkill(int skillId, int seekerId)
        {
            return _jobSeekerRepository.AddSkill(skillId, seekerId);
        }
    }
}