﻿// <auto-generated />
using System;
using DeemZ.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DeemZ.Data.Migrations
{
    [DbContext(typeof(DeemZDbContext))]
    [Migration("20210704135148_AddedIsPublicForExam")]
    partial class AddedIsPublicForExam
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DeemZ.Data.Models.Answer", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit");

                    b.Property<string>("QuestionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Area", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("CountryId")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("DeemZ.Data.Models.City", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("AreaId")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AreaId");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Comment", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ForumId")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("АnswerТоId")
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.HasIndex("ForumId");

                    b.HasIndex("АnswerТоId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Country", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Course", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<int>("Credits")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("SignUpEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SignUpStartDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Description", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("LectureId")
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LectureId");

                    b.ToTable("Descriptions");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Exam", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("CourseId")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<int>("MaxPoints")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("UserId");

                    b.ToTable("Exams");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Forum", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Forums");
                });

            modelBuilder.Entity("DeemZ.Data.Models.InformativeMessage", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ShowFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ShowTo")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("InformativeMessages");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Lecture", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("CourseId")
                        .HasColumnType("nvarchar(40)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Lectures");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Question", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("ExamId")
                        .HasColumnType("nvarchar(40)");

                    b.Property<bool>("IsMultipleChoice")
                        .HasColumnType("bit");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExamId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Report", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("IssueDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Resource", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("LectureId")
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResourceTypeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.HasIndex("LectureId");

                    b.HasIndex("ResourceTypeId");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("DeemZ.Data.Models.ResourceType", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ResourceTypes");
                });

            modelBuilder.Entity("DeemZ.Data.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("CityId")
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("LastName")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DeemZ.Data.Models.UserCourse", b =>
                {
                    b.Property<string>("CourseId")
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(40)");

                    b.Property<int>("EarnedCredits")
                        .HasColumnType("int");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("PaidOn")
                        .HasColumnType("datetime2");

                    b.HasKey("CourseId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserCourses");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Answer", b =>
                {
                    b.HasOne("DeemZ.Data.Models.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Area", b =>
                {
                    b.HasOne("DeemZ.Data.Models.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("DeemZ.Data.Models.City", b =>
                {
                    b.HasOne("DeemZ.Data.Models.Area", "Area")
                        .WithMany()
                        .HasForeignKey("AreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Area");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Comment", b =>
                {
                    b.HasOne("DeemZ.Data.Models.Forum", "Forum")
                        .WithMany("Comments")
                        .HasForeignKey("ForumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeemZ.Data.Models.Comment", "АnswerТо")
                        .WithMany()
                        .HasForeignKey("АnswerТоId");

                    b.Navigation("Forum");

                    b.Navigation("АnswerТо");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Description", b =>
                {
                    b.HasOne("DeemZ.Data.Models.Lecture", null)
                        .WithMany("Descriptions")
                        .HasForeignKey("LectureId");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Exam", b =>
                {
                    b.HasOne("DeemZ.Data.Models.Course", "Course")
                        .WithMany("Exams")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeemZ.Data.Models.User", null)
                        .WithMany("Exams")
                        .HasForeignKey("UserId");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Forum", b =>
                {
                    b.HasOne("DeemZ.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Lecture", b =>
                {
                    b.HasOne("DeemZ.Data.Models.Course", null)
                        .WithMany("Lectures")
                        .HasForeignKey("CourseId");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Question", b =>
                {
                    b.HasOne("DeemZ.Data.Models.Exam", "Exam")
                        .WithMany("Questions")
                        .HasForeignKey("ExamId");

                    b.Navigation("Exam");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Report", b =>
                {
                    b.HasOne("DeemZ.Data.Models.User", "User")
                        .WithMany("Reports")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Resource", b =>
                {
                    b.HasOne("DeemZ.Data.Models.Lecture", null)
                        .WithMany("Resources")
                        .HasForeignKey("LectureId");

                    b.HasOne("DeemZ.Data.Models.ResourceType", "ResourceType")
                        .WithMany()
                        .HasForeignKey("ResourceTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ResourceType");
                });

            modelBuilder.Entity("DeemZ.Data.Models.User", b =>
                {
                    b.HasOne("DeemZ.Data.Models.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId");

                    b.Navigation("City");
                });

            modelBuilder.Entity("DeemZ.Data.Models.UserCourse", b =>
                {
                    b.HasOne("DeemZ.Data.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeemZ.Data.Models.User", "User")
                        .WithMany("UserCourses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Course", b =>
                {
                    b.Navigation("Exams");

                    b.Navigation("Lectures");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Exam", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Forum", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Lecture", b =>
                {
                    b.Navigation("Descriptions");

                    b.Navigation("Resources");
                });

            modelBuilder.Entity("DeemZ.Data.Models.Question", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("DeemZ.Data.Models.User", b =>
                {
                    b.Navigation("Exams");

                    b.Navigation("Reports");

                    b.Navigation("UserCourses");
                });
#pragma warning restore 612, 618
        }
    }
}
