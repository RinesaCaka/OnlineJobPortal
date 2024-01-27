
using OnlineJobPortal.Models;
using System.Collections.Generic;
using System.Web;

namespace OnlineJobPortal.Services
{
    public interface IJobSeekerService
    {
        bool JobSeekerRegister(JobSeeker seeker, HttpPostedFileBase imageUpload, HttpPostedFileBase resumeUpload);
        bool JobSeekerUpdate(JobSeeker seeker, HttpPostedFileBase imageUpload, int seekerId);
        bool UpdateResume(HttpPostedFileBase resume, int seekerId);
        List<JobSeeker>JobSeekers();
        bool AddEducationDetails(EducationDetails details, int seekerId);
        bool UpdateEducationDetails(EducationDetails details);
        List<EducationDetails> GetEducationDetails(int seekerId);
        bool DeleteEducationDetails(int educationId);
        bool CreateJobApplication(JobApplication application);
        List<JobApplication> GetJobApplications(int seekerId);
        List<Bookmark> GetBookmarks(int seekerId);
        bool VisitJob(ViewJob viewJob);
        bool Bookmark(Bookmark bookmark);
        bool ChangePassword(string oldPassword, string newPassword, int seekerId);
        bool AddSkill(int skillId, int seekerId);
    }
}