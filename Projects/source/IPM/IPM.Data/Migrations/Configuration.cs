using IPM.Model.Models;

namespace IPM.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<IPM.Data.IPMDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(IPM.Data.IPMDbContext context)
        {
            DataForRoom(context);
            DataForMeetingRequest(context);
            DataForSkill(context);
            DataForUser(context);
            DataForPosition(context);
            DataForCandidate(context);
            DataForQuestion(context);
            DataForCatalog(context);
            DataForCatalogQuestion(context);
            DataForGuideline(context);
            DataForGuidelineCatalog(context);
            DataForInterviewProcess(context);
            DataForInterviewRound(context);
            DataForRoundProcess(context);
            DataForInterview(context);
            DataForInterviewResult(context);
            DataForAnswerQuestion(context);
            DataForRole(context);
            DataForUserRole(context);
            DataForCertificates(context);
            DataForCandidateCertificates(context);
            DataForCandidateSkills(context);

        }

        private void DataForUser(IPM.Data.IPMDbContext context)
        {

            if (context.Users.Count() == 0)
            {
                List<User> listUsers = new List<User>()
                {
                    new User() { Account = "ThuongTT2", Active = true},
                    new User() { Account= "CuongPNB1", Active = true},
                    new User() { Account = "HoangNV16", Active = true},
                    new User() { Account= "DungPQ2", Active = true},
                    new User() { Account = "DuyHH1", Active = true},
                    new User() { Account= "NgheTK", Active = true},
                    new User() { Account = "DuyPA", Active = true},
                    new User() { Account= "DuyHK", Active = true},
                    new User() { Account = "TienTD1", Active = true},
                    new User() { Account = "PhuongPD2", Active = true},
                    new User() { Account= "HienHT2", Active = true},
                    new User() { Account= "BaoTC", Active = true},
                    new User() { Account= "SonPHH", Active = true},
                    new User() { Account= "QuocPT", Active = true},
                    new User() { Account= "QuyTM4", Active = true},
                    new User() { Account= "BaoNQ7", Active = true},
                    new User() { Account= "DuyetDT", Active = true},
                    new User() { Account= "ThiNL", Active = true},
                    new User() { Account= "NhutNH5", Active = true},
                    new User() { Account= "SangNT9", Active = true},

                };
                context.Users.AddRange(listUsers);
                context.SaveChanges();
            }
        }

        private void DataForSkill(IPM.Data.IPMDbContext context)
        {

            if (context.Skills.Count() == 0)
            {
                List<Skill> listSkill = new List<Skill>()
                {
                    new Skill() { Name="Java", Active = true},
                    new Skill() { Name="C#", Active = true},
                    new Skill() { Name="Python", Active = true},
                    new Skill() { Name="C++", Active = true}
                };
                context.Skills.AddRange(listSkill);
                context.SaveChanges();
            }
        }
        private void DataForRoom(IPM.Data.IPMDbContext context)
        {
            if (context.Rooms.Count() == 0)
            {
                List<Room> listRoom = new List<Room>()
                {
                    new Room() { Name="Yale"},
                    new Room() { Name="Hạ Long Bay"},
                    new Room() { Name="Oxford"},
                    new Room() { Name="Sơn Đoong"}
                };
                context.Rooms.AddRange(listRoom);
                context.SaveChanges();
            }
        }

        private void DataForMeetingRequest(IPM.Data.IPMDbContext context)
        {
            if (context.MeetingRequests.Count() == 0)
            {
                DateTime startTime = DateTime.Now;
                DateTime endTime = startTime.AddHours(2);
                List<MeetingRequest> listMeetingRequest = new List<MeetingRequest>()
                {
                    new MeetingRequest() {Subject = "Phỏng vấn Fresher đợt 1", RoomID = 1, EmailContent = "abc",
                        StartDate = startTime, EndDate = endTime, InterviewAdmin = "ThuongTT2", Active = true, InterviewerEmail = "thuongtt2@fsoft.fpt.vn"},
                    new MeetingRequest() {Subject = "Phỏng vấn Fresher đợt 2", RoomID = 1, EmailContent = "abc",
                        StartDate = startTime, EndDate = endTime, InterviewAdmin = "ThuongTT2", Active = true, InterviewerEmail = "thuongtt2@fsoft.fpt.vn"},
                    new MeetingRequest() {Subject = "Phỏng vấn Fresher đợt 3", RoomID = 1, EmailContent = "abc",
                        StartDate = startTime, EndDate = endTime, InterviewAdmin = "ThuongTT2", Active = true, InterviewerEmail = "thuongtt2@fsoft.fpt.vn"},
                    new MeetingRequest() {Subject = "Phỏng vấn Fresher đợt 4", RoomID = 1, EmailContent = "abc",
                        StartDate = startTime, EndDate = endTime, InterviewAdmin = "ThuongTT2", Active = true, InterviewerEmail = "thuongtt2@fsoft.fpt.vn"},
                    new MeetingRequest() {Subject = "Phỏng vấn Fresher đợt 5", RoomID = 1, EmailContent = "",
                        StartDate = startTime, EndDate = endTime, InterviewAdmin = "CUONGPNB1", Active = true, InterviewerEmail = "thuongtt2@fsoft.fpt.vn"},
                    new MeetingRequest() {Subject = "Phỏng vấn Fresher đợt 6", RoomID = 1, EmailContent = "abc",
                        StartDate = startTime, EndDate = endTime, InterviewAdmin = "ThuongTT2", Active = true, InterviewerEmail = "thuongtt2@fsoft.fpt.vn"},
                    new MeetingRequest() {Subject = "Phỏng vấn Fresher đợt 7", RoomID = 1, EmailContent = "abc",
                        StartDate = startTime, EndDate = endTime, InterviewAdmin = "CUONGPNB1", Active = true, InterviewerEmail = "thuongtt2@fsoft.fpt.vn"},
                    new MeetingRequest() {Subject = "Phỏng vấn Fresher đợt 8", RoomID = 1, EmailContent = "abc",
                        StartDate = startTime, EndDate = endTime, InterviewAdmin = "CUONGPNB1", Active = true, InterviewerEmail = "thuongtt2@fsoft.fpt.vn"},
                    new MeetingRequest() {Subject = "Phỏng vấn Fresher đợt 9", RoomID = 1, EmailContent = "abc",
                        StartDate = startTime, EndDate = endTime, InterviewAdmin = "CUONGPNB1", Active = true, InterviewerEmail = "thuongtt2@fsoft.fpt.vn"},
                    new MeetingRequest() {Subject = "Phỏng vấn Fresher đợt 10", RoomID = 1, EmailContent = "abc",
                        StartDate = startTime, EndDate = endTime, InterviewAdmin = "ThuongTT2", Active = true, InterviewerEmail = "thuongtt2@fsoft.fpt.vn"},
                    new MeetingRequest() {Subject = "Phỏng vấn Fresher đợt 11", RoomID = 1, EmailContent = "abc",
                        StartDate = startTime, EndDate = endTime, InterviewAdmin = "ThuongTT2", Active = true, InterviewerEmail = "thuongtt2@fsoft.fpt.vn"}
                };
                context.MeetingRequests.AddRange(listMeetingRequest);
                context.SaveChanges();
            }
        }

        private void DataForPosition(IPM.Data.IPMDbContext context)
        {
            if (context.Positions.Count() == 0)
            {
                List<Position> listPosition = new List<Position>()
                {
                    new Position() { Name="FRESHER", Code="Fr001", Active = true },
                    new Position() { Name="DEVELOPER", Code="De001", Active = true },
                    new Position() { Name="TESTER", Code="Tes001", Active = true },
                    new Position() { Name="PROJECT MANAGER", Code="PM001", Active = true },
                    new Position() { Name="TRAINER", Code="Tr001", Active = true },
                };
                context.Positions.AddRange(listPosition);
                context.SaveChanges();
            }
        }

        private void DataForQuestion(IPM.Data.IPMDbContext context)
        {
            if (context.Questions.Count() == 0)
            {
                List<Question> listQuestion = new List<Question>()
                {
                    new Question() {Content = "Câu hỏi 1", SkillID = 1 ,Answer = "Câu trả lời 1", Active = true},
                    new Question() {Content = "Câu hỏi 2", SkillID = 2 ,Answer = "Câu trả lời 2", Active = true },
                    new Question() {Content = "Câu hỏi 3", SkillID = 3 ,Answer = "Câu trả lời 3", Active = true },
                    new Question() {Content = "Câu hỏi 4", SkillID = 4 ,Answer = "Câu trả lời 4", Active = true },
                    new Question() {Content = "Câu hỏi 5", SkillID = 1 ,Answer = "Câu trả lời 5", Active = true },
                    new Question() {Content = "Câu hỏi 6", SkillID = 2 ,Answer = "Câu trả lời 6", Active = true },
                    new Question() {Content = "Câu hỏi 7", SkillID = 3 ,Answer = "Câu trả lời 7", Active = true },
                    new Question() {Content = "Câu hỏi 8", SkillID = 4 ,Answer = "Câu trả lời 8", Active = true },
                    new Question() {Content = "Câu hỏi 9", SkillID = 1 ,Answer = "Câu trả lời 9", Active = true },
                    new Question() {Content = "Câu hỏi 10", SkillID = 2 ,Answer = "Câu trả lời 10", Active = true },
                    new Question() {Content = "Câu hỏi 11", SkillID = 3 ,Answer = "Câu trả lời 11", Active = true }
                };
                context.Questions.AddRange(listQuestion);
                context.SaveChanges();
            }
        }

        private void DataForCatalog(IPM.Data.IPMDbContext context)
        {
            if (context.Catalogs.Count() == 0)
            {
                List<Catalog> listCatalog = new List<Catalog>()
                {
                    new Catalog() {Name = "Nhóm câu hỏi 1", MaxPoint = 5 , Active = true},
                    new Catalog() {Name = "Nhóm câu hỏi 2", MaxPoint = 5 , Active = true },
                    new Catalog() {Name = "Nhóm câu hỏi 3", MaxPoint = 5 , Active = true },
                    new Catalog() {Name = "Nhóm câu hỏi 4", MaxPoint = 5 , Active = true },
                    new Catalog() {Name = "Nhóm câu hỏi 5", MaxPoint = 5 , Active = true },
                    new Catalog() {Name = "Nhóm câu hỏi 6", MaxPoint = 5 , Active = true },
                    new Catalog() {Name = "Nhóm câu hỏi 7", MaxPoint = 5 , Active = true },
                    new Catalog() {Name = "Nhóm câu hỏi 8", MaxPoint = 5 , Active = true },
                    new Catalog() {Name = "Nhóm câu hỏi 9", MaxPoint = 5 , Active = true },
                    new Catalog() {Name = "Nhóm câu hỏi 10", MaxPoint = 5 , Active = true },
                    new Catalog() {Name = "Nhóm câu hỏi 11", MaxPoint = 5 , Active = true }
                };
                context.Catalogs.AddRange(listCatalog);
                context.SaveChanges();
            }
        }

        private void DataForGuideline(IPM.Data.IPMDbContext context)
        {
            if (context.Guidelines.Count() == 0)
            {
                List<Guideline> listGuideline = new List<Guideline>()
                {
                    new Guideline() {Name = "Guideline 1" , Active = true},
                    new Guideline() {Name = "Guideline 2" , Active = true },
                };
                context.Guidelines.AddRange(listGuideline);
                context.SaveChanges();
            }
        }

        private void DataForCatalogQuestion(IPM.Data.IPMDbContext context)
        {
            if (context.CatalogQuestions.Count() == 0)
            {
                List<CatalogQuestion> listCatalogQuestion = new List<CatalogQuestion>()
                {
                    new CatalogQuestion() {CatalogID =1, QuestionID = 1 },
                    new CatalogQuestion() {CatalogID =2, QuestionID = 2 },
                    new CatalogQuestion() {CatalogID =3, QuestionID = 3 },
                    new CatalogQuestion() {CatalogID =4, QuestionID = 4 },
                    new CatalogQuestion() {CatalogID =5, QuestionID = 5 },
                    new CatalogQuestion() {CatalogID =1, QuestionID = 6 },
                    new CatalogQuestion() {CatalogID =2, QuestionID = 7 },
                    new CatalogQuestion() {CatalogID =3, QuestionID = 8 },
                    new CatalogQuestion() {CatalogID =4, QuestionID = 9 },
                    new CatalogQuestion() {CatalogID =5, QuestionID = 10 },
                    new CatalogQuestion() {CatalogID =1, QuestionID = 11 },
                };
                context.CatalogQuestions.AddRange(listCatalogQuestion);
                context.SaveChanges();
            }
        }

        private void DataForGuidelineCatalog(IPM.Data.IPMDbContext context)
        {
            if (context.GuidelineCatalogs.Count() == 0)
            {
                List<GuidelineCatalog> listGuidelineCatalog = new List<GuidelineCatalog>()
                {
                    new GuidelineCatalog() {GuidelineID =1, CatalogID = 1 },
                    new GuidelineCatalog() {GuidelineID =2, CatalogID = 2 },
                    new GuidelineCatalog() {GuidelineID =1, CatalogID = 3 },
                    new GuidelineCatalog() {GuidelineID =2, CatalogID = 4 },
                    new GuidelineCatalog() {GuidelineID =1, CatalogID = 5 },
                    new GuidelineCatalog() {GuidelineID =2, CatalogID = 1 },
                    new GuidelineCatalog() {GuidelineID =1, CatalogID = 2 },
                    new GuidelineCatalog() {GuidelineID =2, CatalogID = 3 },
                    new GuidelineCatalog() {GuidelineID =1, CatalogID = 4 },
                    new GuidelineCatalog() {GuidelineID =2, CatalogID = 5 },
                };
                context.GuidelineCatalogs.AddRange(listGuidelineCatalog);
                context.SaveChanges();
            }
        }

        private void DataForInterviewProcess(IPM.Data.IPMDbContext context)
        {
            DateTime startTime1 = DateTime.Now;

            if (context.InterviewProcesses.Count() == 0)
            {
                List<InterviewProcess> listInterviewProcess = new List<InterviewProcess>()
                {
                    new InterviewProcess() { ProcessName = "NET 1", PositionID = 1, Active = true, StartDate = startTime1 },
                    new InterviewProcess() { ProcessName = "JAVA 1", PositionID = 2, Active = true, StartDate = startTime1 },
                };
                context.InterviewProcesses.AddRange(listInterviewProcess);
                context.SaveChanges();
            }
        }
        private void DataForRoundProcess(IPM.Data.IPMDbContext context)
        {
            if (context.RoundProcesses.Count() == 0)
            {
                List<RoundProcess> listRoundProcess = new List<RoundProcess>()
                {
                    new RoundProcess() { InterviewRoundID = 1, InterviewProcessID = 1 },
                    new RoundProcess() { InterviewRoundID = 1, InterviewProcessID = 2 },
                };
                context.RoundProcesses.AddRange(listRoundProcess);
                context.SaveChanges();
            }
        }

        private void DataForCandidate(IPM.Data.IPMDbContext context)
        {
            DateTime startTime1 = DateTime.Now;

            if (context.Candidates.Count() == 0)
            {
                List<Candidate> listCandidate = new List<Candidate>()
                {
                    new Candidate() { Name = "Phạm Hoàng Hải Sơn", Address ="TPHCM", GPA = "10", University="Sư phạm Kĩ thuật TPHCM", InterviewAdminID = 1, PositionID = 1, Birthdate = startTime1, ConcidentStatus = 1, Active = true},
                    new Candidate() { Name = "Phan Thiên Quốc", Address ="TPHCM", GPA = "10", University="Sư phạm Kĩ thuật TPHCM", InterviewAdminID = 1, PositionID = 1, Birthdate = startTime1, ConcidentStatus = 1, Active = true},
                };
                context.Candidates.AddRange(listCandidate);
                context.SaveChanges();
            }
        }

        private void DataForInterview(IPM.Data.IPMDbContext context)
        {
            if (context.Interviews.Count() == 0)
            {
                DateTime startTime1 = DateTime.Now;
                List<Interview> listInterview = new List<Interview>()
                {
                    new Interview() { CandidateID = 1, InterviewerID = 20, Record = "", RoundProcessID = 1, StartTime = startTime1, Result = true },
                    new Interview() { CandidateID = 1, InterviewerID = 20, Record = "", RoundProcessID = 2, StartTime = startTime1, Result = null },
                    new Interview() { CandidateID = 2, InterviewerID = 20, Record = "", RoundProcessID = 1, StartTime = startTime1, Result = null },
                };
                context.Interviews.AddRange(listInterview);
                context.SaveChanges();
            }
        }

        private void DataForInterviewRound(IPM.Data.IPMDbContext context)
        {
            if (context.InterviewRounds.Count() == 0)
            {
                List<InterviewRound> listInterviewRound = new List<InterviewRound>()
                {
                    new InterviewRound() { GuidelineID = 1, RoundName = ".NET Round 1", Description = "Vòng 1 .NET", Active = true },
                    new InterviewRound() { GuidelineID = 2, RoundName = ".NET Round 2", Description = "Vòng 2 .NET", Active = true },

                };
                context.InterviewRounds.AddRange(listInterviewRound);
                context.SaveChanges();
            }
        }
        private void DataForInterviewResult(IPM.Data.IPMDbContext context)
        {
            if (context.InterviewAnswers.Count() == 0)
            {
                List<InterviewAnswer> listInterviewResult = new List<InterviewAnswer>()
                {
                    new InterviewAnswer() { CatalogID = 1, Mark = 5, InterviewID = 1},
                    new InterviewAnswer() { CatalogID = 3, Mark = 5, InterviewID = 1},
                };
                context.InterviewAnswers.AddRange(listInterviewResult);
                context.SaveChanges();
            }
        }

        private void DataForAnswerQuestion(IPM.Data.IPMDbContext context)
        {
            if (context.AnswerQuestions.Count() == 0)
            {
                List<AnswerQuestion> listAnswerQuestion = new List<AnswerQuestion>()
                {
                    new AnswerQuestion() { QuestionID = 1, CandidateAnswer ="ABC", Comment = "ABC", InterviewAnswerID = 1},
                    new AnswerQuestion() { QuestionID = 3, CandidateAnswer ="DEF", Comment = "DEF", InterviewAnswerID = 1 },
                    new AnswerQuestion() { QuestionID = 6, CandidateAnswer ="123", Comment = "123" , InterviewAnswerID = 2},
                    new AnswerQuestion() { QuestionID = 8, CandidateAnswer ="456", Comment = "456" , InterviewAnswerID = 2},

                };
                context.AnswerQuestions.AddRange(listAnswerQuestion);
                context.SaveChanges();
            }
        }

        private void DataForRole(IPM.Data.IPMDbContext context)
        {
            if (context.Roles.Count() == 0)
            {
                List<Role> listRole = new List<Role>()
                {
                    new Role() { Name = "Admin", Active = true},
                    new Role() { Name = "Interview Admin", Active = true},
                    new Role() { Name = "Interviewer", Active = true },

                };
                context.Roles.AddRange(listRole);
                context.SaveChanges();
            }
        }

        private void DataForUserRole(IPM.Data.IPMDbContext context)
        {
            if (context.UserRoles.Count() == 0)
            {
                List<UserRole> listUserRole = new List<UserRole>()
                {
                    new UserRole() { Account = "ThuongTT2", RoleID = 1},
                    new UserRole() { Account= "CuongPNB1", RoleID = 1},
                    new UserRole() { Account = "HoangNV16", RoleID = 1},
                    new UserRole() { Account= "DungPQ2", RoleID = 1},
                    new UserRole() { Account = "DuyHH1", RoleID = 1},
                    new UserRole() { Account= "NgheTK", RoleID = 1},
                    new UserRole() { Account = "DuyPA", RoleID = 1},
                    new UserRole() { Account= "DuyHK", RoleID = 1},
                    new UserRole() { Account = "TienTD1", RoleID = 1},
                    new UserRole() { Account = "PhuongPD2", RoleID = 1},
                    new UserRole() { Account= "HienHT2", RoleID = 1},
                    new UserRole() { Account= "BaoTC", RoleID = 1},
                    new UserRole() { Account= "SonPHH", RoleID = 1},
                    new UserRole() { Account= "QuocPT", RoleID = 1},
                    new UserRole() { Account= "QuyTM4", RoleID = 1},
                    new UserRole() { Account= "BaoNQ7", RoleID = 1},
                    new UserRole() { Account= "DuyetDT", RoleID = 1},
                    new UserRole() { Account= "ThiNL", RoleID = 1},
                    new UserRole() { Account= "NhutNH5", RoleID = 1},
                    new UserRole() { Account= "SangNT9", RoleID = 1},

                };
                context.UserRoles.AddRange(listUserRole);
                context.SaveChanges();
            }
        }

        private void DataForCandidateSkills(IPM.Data.IPMDbContext context)
        {
            if (context.CandidateSkills.Count() == 0)
            {
                List<CandidateSkill> candidateSkills = new List<CandidateSkill>()
                {
                    new CandidateSkill() {SkillID = 1, Active = true, CandidateID = 1 },
                    new CandidateSkill() {SkillID = 1, Active = true, CandidateID = 2 }
                };
                context.CandidateSkills.AddRange(candidateSkills);
                context.SaveChanges();
            }
        }

        private void DataForCandidateCertificates(IPM.Data.IPMDbContext context)
        {
            if (context.CandidateCertificates.Count() == 0)
            {
                List<CandidateCertificate> listCadidateCertificates = new List<CandidateCertificate>()
                {
                    new CandidateCertificate() { CertificateID = 1, Active = true, CandidateID = 1 },
                    new CandidateCertificate() { CertificateID = 1, Active = true, CandidateID = 2 },
                    new CandidateCertificate() { CertificateID = 2, Active = true, CandidateID = 1 }
                };
                context.CandidateCertificates.AddRange(listCadidateCertificates);
                context.SaveChanges();
            }
        }

        private void DataForCertificates(IPM.Data.IPMDbContext context)
        {
            if (context.Certificates.Count() == 0)
            {
                List<Certificate> listCertificates = new List<Certificate>()
                {
                    new Certificate() { Name = "GST .NET", Active = true },
                    new Certificate() { Name = "GST .Java", Active = true },
                };
                context.Certificates.AddRange(listCertificates);
                context.SaveChanges();
            }
        }
    }
}
