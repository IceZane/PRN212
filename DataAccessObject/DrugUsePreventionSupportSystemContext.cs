using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BusinessObjects;

namespace DataAccessObject;

public partial class DrugUsePreventionSupportSystemContext : DbContext
{
    public DrugUsePreventionSupportSystemContext()
    {
    }

    public DrugUsePreventionSupportSystemContext(DbContextOptions<DrugUsePreventionSupportSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<BlogPost> BlogPosts { get; set; }

    public virtual DbSet<CommunityProgram> CommunityPrograms { get; set; }

    public virtual DbSet<Consultant> Consultants { get; set; }

    public virtual DbSet<ConsultantSchedule> ConsultantSchedules { get; set; }

    public virtual DbSet<DashboardReport> DashboardReports { get; set; }

    public virtual DbSet<ProgramFeedbackSurvey> ProgramFeedbackSurveys { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Survey> Surveys { get; set; }

    public virtual DbSet<SurveyAnswer> SurveyAnswers { get; set; }

    public virtual DbSet<SurveyQuestion> SurveyQuestions { get; set; }

    public virtual DbSet<TrainingCourse> TrainingCourses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCourse> UserCourses { get; set; }

    public virtual DbSet<UserProgramParticipation> UserProgramParticipations { get; set; }

    public virtual DbSet<UserSurveyResult> UserSurveyResults { get; set; }

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
        var strConn = config["ConnectionStrings:DefaultConnection"];

        return strConn;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(GetConnectionString());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCA2D2260E04");

            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.ConsultantId).HasColumnName("ConsultantID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Consultant).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ConsultantId)
                .HasConstraintName("FK__Appointme__Consu__5CD6CB2B");

            entity.HasOne(d => d.User).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Appointme__UserI__5BE2A6F2");
        });

        modelBuilder.Entity<BlogPost>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__BlogPost__AA1260388BF8B1B7");

            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.PostedByNavigation).WithMany(p => p.BlogPosts)
                .HasForeignKey(d => d.PostedBy)
                .HasConstraintName("FK__BlogPosts__Poste__6C190EBB");
        });

        modelBuilder.Entity<CommunityProgram>(entity =>
        {
            entity.HasKey(e => e.ProgramId).HasName("PK__Communit__75256038595DBAFB");

            entity.Property(e => e.ProgramId).HasColumnName("ProgramID");
            entity.Property(e => e.ProgramTitle).HasMaxLength(200);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CommunityPrograms)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Community__Creat__60A75C0F");
        });

        modelBuilder.Entity<Consultant>(entity =>
        {
            entity.HasKey(e => e.ConsultantId).HasName("PK__Consulta__E5B83F39BDD4E19F");

            entity.HasIndex(e => e.UserId, "UQ__Consulta__1788CCADABC1A080").IsUnique();

            entity.Property(e => e.ConsultantId)
                .ValueGeneratedNever()
                .HasColumnName("ConsultantID");
            entity.Property(e => e.Qualification).HasMaxLength(255);
            entity.Property(e => e.Specialization).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithOne(p => p.Consultant)
                .HasForeignKey<Consultant>(d => d.UserId)
                .HasConstraintName("FK__Consultan__UserI__5629CD9C");
        });

        modelBuilder.Entity<ConsultantSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Consulta__9C8A5B69893EBBEC");

            entity.ToTable("ConsultantSchedule");

            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.ConsultantId).HasColumnName("ConsultantID");

            entity.HasOne(d => d.Consultant).WithMany(p => p.ConsultantSchedules)
                .HasForeignKey(d => d.ConsultantId)
                .HasConstraintName("FK__Consultan__Consu__59063A47");
        });

        modelBuilder.Entity<DashboardReport>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Dashboar__D5BD48E5A8EFC56C");

            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.GeneratedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReportType).HasMaxLength(100);

            entity.HasOne(d => d.GeneratedByNavigation).WithMany(p => p.DashboardReports)
                .HasForeignKey(d => d.GeneratedBy)
                .HasConstraintName("FK__Dashboard__Gener__6FE99F9F");
        });

        modelBuilder.Entity<ProgramFeedbackSurvey>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__ProgramF__6A4BEDF6400AFDB5");

            entity.Property(e => e.FeedbackId).HasColumnName("FeedbackID");
            entity.Property(e => e.ProgramId).HasColumnName("ProgramID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Program).WithMany(p => p.ProgramFeedbackSurveys)
                .HasForeignKey(d => d.ProgramId)
                .HasConstraintName("FK__ProgramFe__Progr__693CA210");

            entity.HasOne(d => d.User).WithMany(p => p.ProgramFeedbackSurveys)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ProgramFe__UserI__68487DD7");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A0209A29D");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B61607E8C4B0A").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Survey>(entity =>
        {
            entity.HasKey(e => e.SurveyId).HasName("PK__Surveys__A5481F9D62606CD3");

            entity.Property(e => e.SurveyId).HasColumnName("SurveyID");
            entity.Property(e => e.SurveyName).HasMaxLength(100);
        });

        modelBuilder.Entity<SurveyAnswer>(entity =>
        {
            entity.HasKey(e => e.AnswerId).HasName("PK__SurveyAn__D482502433A2C85E");

            entity.Property(e => e.AnswerId).HasColumnName("AnswerID");
            entity.Property(e => e.AnswerText).HasMaxLength(200);
            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

            entity.HasOne(d => d.Question).WithMany(p => p.SurveyAnswers)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK__SurveyAns__Quest__4D94879B");
        });

        modelBuilder.Entity<SurveyQuestion>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__SurveyQu__0DC06F8C7364D5F3");

            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.SurveyId).HasColumnName("SurveyID");

            entity.HasOne(d => d.Survey).WithMany(p => p.SurveyQuestions)
                .HasForeignKey(d => d.SurveyId)
                .HasConstraintName("FK__SurveyQue__Surve__4AB81AF0");
        });

        modelBuilder.Entity<TrainingCourse>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Training__C92D71872483D525");

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TargetAudience).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TrainingCourses)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__TrainingC__Creat__3F466844");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACF60177FB");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534051EB5DE").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Users__RoleID__3B75D760");
        });

        modelBuilder.Entity<UserCourse>(entity =>
        {
            entity.HasKey(e => e.UserCourseId).HasName("PK__UserCour__58886EF40DD5C3B2");

            entity.Property(e => e.UserCourseId).HasColumnName("UserCourseID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.ProgressPercent).HasDefaultValue(0);
            entity.Property(e => e.RegisteredAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Course).WithMany(p => p.UserCourses)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__UserCours__Cours__440B1D61");

            entity.HasOne(d => d.User).WithMany(p => p.UserCourses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserCours__UserI__4316F928");
        });

        modelBuilder.Entity<UserProgramParticipation>(entity =>
        {
            entity.HasKey(e => e.ParticipationId).HasName("PK__UserProg__4EA2708072C68D8F");

            entity.ToTable("UserProgramParticipation");

            entity.Property(e => e.ParticipationId).HasColumnName("ParticipationID");
            entity.Property(e => e.JoinedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProgramId).HasColumnName("ProgramID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Program).WithMany(p => p.UserProgramParticipations)
                .HasForeignKey(d => d.ProgramId)
                .HasConstraintName("FK__UserProgr__Progr__6477ECF3");

            entity.HasOne(d => d.User).WithMany(p => p.UserProgramParticipations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserProgr__UserI__6383C8BA");
        });

        modelBuilder.Entity<UserSurveyResult>(entity =>
        {
            entity.HasKey(e => e.ResultId).HasName("PK__UserSurv__976902289C8BD874");

            entity.Property(e => e.ResultId).HasColumnName("ResultID");
            entity.Property(e => e.SurveyId).HasColumnName("SurveyID");
            entity.Property(e => e.TakenAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Survey).WithMany(p => p.UserSurveyResults)
                .HasForeignKey(d => d.SurveyId)
                .HasConstraintName("FK__UserSurve__Surve__5165187F");

            entity.HasOne(d => d.User).WithMany(p => p.UserSurveyResults)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserSurve__UserI__5070F446");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
