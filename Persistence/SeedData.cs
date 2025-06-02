using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Persistence
{
    public static class SeedData
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var courses = LoadCoursesFromJson();

            var courseEntities = new List<Course>();
            var courseDateEntities = new List<CourseDate>();

            var hardcodedGuids = new List<Guid>
            {
                Guid.Parse("49ed75aa-0a08-40c8-92ed-cd88a68f564d"),
                Guid.Parse("5cd91938-abc4-440f-b3c1-52371516bf8d"),
                Guid.Parse("8f516d6a-e1b3-4d66-b0d7-f9b40cdcdb04"),
                Guid.Parse("587cdc3e-40a8-43f1-b67e-251292d94f3e"),
                Guid.Parse("f0ca2d3e-554f-459e-9045-dce2d5ab616b"),
                Guid.Parse("ea3cf94a-7ab6-4347-a0a2-b8f32d2ba51b"),
                Guid.Parse("2b7d2ae2-0214-42a0-be07-f6f7bcdb8064"),
                Guid.Parse("8de1a562-ca5c-4dd3-9961-4b03650a6e47"),
                Guid.Parse("7cbf70c1-5cd9-435d-b39d-cd6bcd3bd7ee"),
                Guid.Parse("361b541c-c1af-46c6-b28f-7576d87bd51c"),
                Guid.Parse("48e2f6e5-e7a5-4861-9563-4823774a6b5c"),
                Guid.Parse("c694a722-4015-45ed-8f79-628b6050e664"),
                Guid.Parse("84dc067d-b953-42a2-afd2-fbbf4ef781f8")
            };

            int guidIndex = 0;

            foreach (var course in courses)
            {
                var courseEntity = new Course
                {
                    Id = course.Id,
                    Name = course.Name
                };
                courseEntities.Add(courseEntity);

                foreach (var date in course.Dates)
                {
                    // Use hard coded GUIDs for each CourseDate entity
                    var uniqueId = hardcodedGuids[guidIndex++];
                    courseDateEntities.Add(new CourseDate
                    {
                        Id = uniqueId,
                        Date = date,
                        CourseId = course.Id
                    });
                }
            }

            foreach (var course in courseEntities)
            {
                modelBuilder.Entity<Course>().HasData(course);
            }

            foreach (var courseDate in courseDateEntities)
            {
                modelBuilder.Entity<CourseDate>().HasData(courseDate);
            }
        }

        private static List<CourseJsonModel> LoadCoursesFromJson()
        {
            // Define the explicit path to the JSON file
            var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "courses.json");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file '{filePath}' was not found.");
            }

            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<CourseJsonModel>>(json);
        }

        private class CourseJsonModel
        {
            public int Id { get; set; }
            public required string Name { get; set; }
            public required List<DateTime> Dates { get; set; }
        }
    }
}