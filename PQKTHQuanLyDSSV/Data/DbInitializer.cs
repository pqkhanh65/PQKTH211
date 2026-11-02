using Microsoft.EntityFrameworkCore;
using PQKTHQuanLyDSSV.Models;

namespace PQKTHQuanLyDSSV.Data
{
    public class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new SchoolContext(
                serviceProvider.GetRequiredService<DbContextOptions<SchoolContext>>());

            context.Database.EnsureCreated();

            if (!context.Majors.Any())
            {
                context.Majors.AddRange(
                    new Major { MajorName = "IT" },
                    new Major { MajorName = "Economics" },
                    new Major { MajorName = "Mathematics" });
                context.SaveChanges();
            }

            if (context.Learners.Count() < 8)
            {
                var samples = new[]
                {
                    new Learner { FirstMidName = "Nam", LastName = "Le", EnrollmentDate = new DateTime(2023, 10, 4), MajorID = 3 },
                    new Learner { FirstMidName = "Minh", LastName = "Tran", EnrollmentDate = new DateTime(2023, 10, 4), MajorID = 2 },
                    new Learner { FirstMidName = "Bich", LastName = "Vu", EnrollmentDate = new DateTime(2023, 10, 23), MajorID = 2 },
                    new Learner { FirstMidName = "Tuan Nam", LastName = "Tran", EnrollmentDate = new DateTime(2023, 10, 5), MajorID = 1 },
                    new Learner { FirstMidName = "Ha", LastName = "Nguyen", EnrollmentDate = new DateTime(2023, 10, 4), MajorID = 2 },
                    new Learner { FirstMidName = "Hoang", LastName = "Pham", EnrollmentDate = new DateTime(2024, 1, 15), MajorID = 1 }
                };

                foreach (var learner in samples)
                {
                    bool exists = context.Learners.Any(l =>
                        l.FirstMidName == learner.FirstMidName &&
                        l.LastName == learner.LastName);
                    if (!exists)
                        context.Learners.Add(learner);
                }
                context.SaveChanges();
            }

            if (!context.Courses.Any())
            {
                context.Courses.AddRange(
                    new Course { CourseID = 1050, Title = "Chemistry", Credits = 3 },
                    new Course { CourseID = 4022, Title = "Microeconomics", Credits = 3 },
                    new Course { CourseID = 4041, Title = "Macroeconomics", Credits = 3 });
                context.SaveChanges();
            }

            if (!context.Enrollments.Any())
            {
                var firstLearner = context.Learners.OrderBy(l => l.LearnerID).FirstOrDefault();
                var secondLearner = context.Learners.OrderBy(l => l.LearnerID).Skip(1).FirstOrDefault();

                if (firstLearner != null && secondLearner != null)
                {
                    context.Enrollments.AddRange(
                        new Enrollment { LearnerID = firstLearner.LearnerID, CourseID = 1050, Grade = 5.5f },
                        new Enrollment { LearnerID = firstLearner.LearnerID, CourseID = 4022, Grade = 7.5f },
                        new Enrollment { LearnerID = secondLearner.LearnerID, CourseID = 1050, Grade = 3.5f },
                        new Enrollment { LearnerID = secondLearner.LearnerID, CourseID = 4041, Grade = 7.0f });
                    context.SaveChanges();
                }
            }
        }
    }
}