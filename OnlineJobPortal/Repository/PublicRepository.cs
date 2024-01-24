
using OnlineJobPortal.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace OnlineJobPortal.Repository
{
    public class PublicRepository
    {
        /// <summary>
        /// Connection veriable
        /// </summary>
        private SqlConnection con;

        /// <summary>
        /// To establish connection between server and the application
        /// </summary>
        private void connection()
        {
            string conn = ConfigurationManager.ConnectionStrings["mycon"].ToString();
            con = new SqlConnection(conn);

        }
        /// <summary>
        /// Login method for all users
        /// </summary>
        /// <param name="username">Username or Email</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public string Login([Bind(Include = "Username,Password")] Login login)
        {
            try
            {
                connection();
                con.Open();
                SqlCommand com = new SqlCommand("SP_JobSeekerLogin", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Username", login.Username);
                string res = Convert.ToString(com.ExecuteScalar());
                if (res == "0")
                {
                    com = new SqlCommand("SP_EmployerLogin", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Username", login.Username);
                    res = Convert.ToString(com.ExecuteScalar());
                    if (res == "0")
                    {
                        com = new SqlCommand("SP_AdminLogin", con);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@Username", login.Username);
                        res = Convert.ToString(com.ExecuteScalar());
                        if (res == "0")
                        {
                            return res = "Wrong username and  password ";
                        }
                        else
                        {
                            if (login.VerifyPassword(res))
                            {
                                return res = "Admin";
                            }
                            else
                            {
                                return res = "Wrong  password ";
                            }
                        }
                    }
                    else
                    {
                        if (login.VerifyPassword(res))
                        {
                            return res = "Employer";
                        }
                        else
                        {
                            return res = "Wrong  password ";
                        }
                    }
                }
                else
                {
                    if (login.VerifyPassword(res))
                    {
                        return res = "JobSeeker";
                    }
                    else
                    {
                        return res = "Wrong  password ";
                    }
                }
            }
            finally { con.Close(); }
        }
        /// <summary>
        /// Display all the skills
        /// </summary>
        public List<Skills> DisplaySkills()
        {
            List<Skills> skill = new List<Skills>();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("SP_ReadSkills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    skill.Add(new Skills()
                    {
                        SkillId = Convert.ToInt32(dr["SkillID"]),
                        SkillName = Convert.ToString(dr["SkillName"])
                    });
                }
                return skill;
            }
            finally { con.Close(); }
        }
        /// <summary>
        /// Display job seeker skills
        /// </summary>
        /// <returns></returns>
        public List<JobSeekerSkills> JobSeekerSkills(int seekerId)
        {
            List<JobSeekerSkills> skill = new List<JobSeekerSkills>();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("SP_ReadJobSeekerSkills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@JobSeekerId", seekerId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    skill.Add(new JobSeekerSkills()
                    {
                        JobSeekerSkillId = Convert.ToInt32(dr["JobSeekerSkillID"]),
                        SeekerId = Convert.ToInt32(dr["JobSeekerID"]),
                        SkillId = Convert.ToInt32(dr["SkillID"]),
                        SkillName = Convert.ToString(dr["SkillName"])
                    });

                }
                return skill;
            }
            finally { con.Close(); }
        }
        /// <summary>
        /// Display Job vacancy list
        /// </summary>
        /// <returns></returns>
        public List<JobVacancy> GetJobVacancies()
        {
            try
            {
                connection();
                SqlCommand com = new SqlCommand("SP_ReadJobVacancy", con);
                List<JobVacancy> jobVacancies = new List<JobVacancy>();
                com.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    jobVacancies.Add(new JobVacancy()
                    {
                        VacancyID = Convert.ToInt32(dr["VacancyID"]),
                        EmployerID = Convert.ToInt32(dr["EmployerID"]),
                        JobTitle = Convert.ToString(dr["JobTitle"]),
                        Description = Convert.ToString(dr["Description"]),
                        CategoryID = Convert.ToInt32(dr["CategoryID"]),
                        CategoryName = Convert.ToString(dr["CategoryName"]),
                        Location = Convert.ToString(dr["Location"]),
                        Salary = Convert.ToDecimal(dr["Salary"]),
                        EmploymentType = Convert.ToString(dr["EmploymentType"]),
                        ApplicationDeadline = Convert.ToDateTime(dr["ApplicationDeadline"]),
                        IsPublished = Convert.ToBoolean(dr["IsPublished"])
                    });
                }

                return jobVacancies;
            }
            finally { con.Close(); }
        }
        /// <summary>
        /// Display categories
        /// </summary>
        /// <returns></returns>
        public List<Category> DisplayCategories()
        {
            List<Category> skill = new List<Category>();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("SP_ReadCategories", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    skill.Add(new Category()
                    {
                        CategoryId = Convert.ToInt32(dr["CategoryID"]),
                        CategoryName = Convert.ToString(dr["CategoryName"])
                    });
                }
                return skill;
            }
            finally { con.Close(); }
        }

        public List<JobDetails> GetJobDetails()
        {
            try
            {
                connection();
                SqlCommand com = new SqlCommand("SP_ReadJobDetails", con);
                List<JobDetails> jobDetails = new List<JobDetails>();
                com.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    jobDetails.Add(
                    new JobDetails
                    {
                        JobID = Convert.ToInt32(dr["VacancyID"]),
                        EmployerID = Convert.ToInt32(dr["EmployerID"]),
                        JobTitle = Convert.ToString(dr["JobTitle"]),
                        Description = Convert.ToString(dr["Description"]),
                        CategoryID = Convert.ToInt32(dr["CategoryID"]),
                        CategoryName = Convert.ToString(dr["Category"]),
                        Location = Convert.ToString(dr["Location"]),
                        Salary = Convert.ToDecimal(dr["Salary"]),
                        EmploymentType = Convert.ToString(dr["EmploymentType"]),
                        ApplicationDeadline = Convert.ToDateTime(dr["ApplicationDeadline"]),
                        CompanyName = Convert.ToString(dr["CompanyName"]),
                        OfficialEmail = Convert.ToString(dr["OfficialEmail"]),
                        Email = Convert.ToString(dr["Email"]),
                        ContactPhone = Convert.ToString(dr["ContactPhone"]),
                        Website = Convert.ToString(dr["Website"]),
                        EmployerName = Convert.ToString(dr["EmployerName"]),
                        Designation = Convert.ToString(dr["Designation"]),
                        CompanyLogo = (byte[])dr["CompanyLogo"],
                        NumberOfApplications = Convert.ToInt32(dr["Applications"]),
                        NumberOfViews = Convert.ToInt32(dr["JobViews"]),
                        IsPublished = Convert.ToBoolean(dr["IsPublished"])
                    });
                }

                return jobDetails;
            }
            finally { con.Close(); }
        }
        /// <summary>
        /// Check username is alredy used or not
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns></returns>
        public bool CheckUsername(string username)
        {
            try
            {
                connection();
                SqlCommand com = new SqlCommand("SP_CheckUsername", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Username", username);
                con.Open();
                int i = (int)com.ExecuteScalar();
                return i > 0;
            }
            finally { con.Close(); }
        }
        
       
       


    }
}