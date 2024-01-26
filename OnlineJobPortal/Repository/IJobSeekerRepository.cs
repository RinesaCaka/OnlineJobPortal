using OnlineJobPortal.Models;
using System.Collections.Generic;
using System.Web;

namespace OnlineJobPortal.Repository
{
    public interface IJobSeekerRepository
    {
        bool JobSeekerRegister(JobSeeker seeker, HttpPostedFileBase imageUpload, HttpPostedFileBase resumeUpload);

        bool JobSeekerUpdate(JobSeeker seeker, HttpPostedFileBase imageUpload, int seekerId);

        bool UpdateResume(HttpPostedFileBase resume, int seekerId);

        List<JobSeeker> JobSeekers();

        bool AddEducationDetails(EducationDetails obj, int id);

        bool UpdateEducationDetails(EducationDetails obj);

        bool DeleteEducationDetails(int id);

        bool DeleteJobSeekerSkill(int id);

        List<EducationDetails> GetEducationDetails(int seekerId);

        bool CreateJobApplication(JobApplication application);

        List<JobApplication> GetJobApplications(int seekerId);

        List<Bookmark> GetBookmarks(int seekerId);

        bool VisitJob(ViewJob obj);

        bool Bookmark(Bookmark obj);

        bool ChangePassword(string oldPassword, string newPassword, int seekerId);

        bool AddSkill(int skillId, int seekerId);

    }
}