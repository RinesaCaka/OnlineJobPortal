﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineJobPortal
{
    using OnlineJobPortal.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class JobPortalEntities : DbContext
    {
        public JobPortalEntities()
            : base("name=JobPortalEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Bookmark> Bookmarks { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<EducationDetails> EducationDetails { get; set; }
        public virtual DbSet<Employer> Employers { get; set; }
        public virtual DbSet<JobApplication> JobApplications { get; set; }
        public virtual DbSet<JobSeeker> JobSeeker { get; set; }
        public virtual DbSet<JobSeekerSkills> JobSeekerSkills { get; set; }
        public virtual DbSet<JobVacancy> JobVacancies { get; set; }
        public virtual DbSet<JobViewers> JobViews { get; set; }
      
        public virtual DbSet<Skills> Skills { get; set; }
    }
}
