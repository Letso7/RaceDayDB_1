using Microsoft.EntityFrameworkCore;
using RaceDayDB1.Models;
using RaceDayDB1.Data;

namespace Project1UnitTest
{
    public class UnitTest1
    {
        private RaceDayDBContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<RaceDayDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            return new RaceDayDBContext(options);
        }

        [Fact]
        public async Task Test_User_PasswordHash_With_BCrypt()
        {
            var context = GetInMemoryContext();
            string plain = "password123";
            string hashed = BCrypt.Net.BCrypt.HashPassword(plain);

            var user = new User
            {
                Username = "testuser",
                Email = "test@test.com",
                PasswordHash = hashed,
                Role = "Participant",
                FullName = "Test User"
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var saved = await context.Users.FirstAsync();
            Assert.True(BCrypt.Net.BCrypt.Verify(plain, saved.PasswordHash));
        }

        [Fact]
        public async Task Test_Add_Category_Should_Save()
        {
            var context = GetInMemoryContext();
            var cat = new Category
            {
                CategoryName = "5km",
                Description = "5km run"
            };

            context.Categories.Add(cat);
            await context.SaveChangesAsync();

            Assert.Equal(1, await context.Categories.CountAsync());
        }

        [Fact]
        public async Task Test_Add_Event_Should_Save()
        {
            var context = GetInMemoryContext();
            var ev = new Event
            {
                EventName = "10km Race",
                Description = "Test event",
                EventDate = DateTime.Now,
                Location = "Joburg",
                Distance = 10.0,
                EventType = "run",
                CategoryId = 1,
                OrganiserId = 1
            };

            context.Events.Add(ev);
            await context.SaveChangesAsync();

            Assert.Equal(1, await context.Events.CountAsync());
        }

        [Fact]
        public async Task Test_Add_Enrolment_Should_Save()
        {
            var context = GetInMemoryContext();
            var enrol = new Enrolment
            {
                UserId = 1,
                EventId = 1,
                CategoryId = 1,
                EnrolmentDate = DateTime.Now
            };

            context.Enrolments.Add(enrol);
            await context.SaveChangesAsync();

            Assert.Single(await context.Enrolments.ToListAsync());
        }

        [Fact]
        public async Task Test_Add_Result_Should_Save()
        {
            var context = GetInMemoryContext();
            var result = new Result
            {
                EnrolmentId = 1,
                EventId = 1,
                UserId = 1,
                FinishTime = TimeSpan.FromMinutes(25),
                Position = 1,
                RecordedDate = DateTime.Now
            };

            context.Results.Add(result);
            await context.SaveChangesAsync();

            var saved = await context.Results.FirstAsync();
            Assert.Equal(1, saved.Position);
        }
    }
}