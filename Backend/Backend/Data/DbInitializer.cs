
using BCrypt.Net;
using ProfkomBackend.Models;

namespace ProfkomBackend.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext db)
        {
            // Admins
            if (!db.Admins.Any())
            {
                var hash = BCrypt.Net.BCrypt.HashPassword("admin123");
                db.Admins.Add(new Admin { Username = "admin", PasswordHash = hash, Role = "admin" });
            }

            // News
            if (!db.News.Any())
            {
                db.News.Add(new News { Title = "Вітаємо у профкомі", Summary = "Перший запис", Content = "Ласкаво просимо на сайт профкому.", ImageUrl = "https://example.com/image.jpg", PublishedAt = DateTime.UtcNow });
                db.News.Add(new News { Title = "Новина 2", Summary = "Другий запис", Content = "Інформація про подію.", ImageUrl = "https://example.com/image.jpg", PublishedAt = DateTime.UtcNow.AddHours(-5) });
            }

            // Events
            if (!db.Events.Any())
            {
                db.Events.Add(new Event { Title = "Засідання профкому", Description = "Щомісячне засідання", StartsAt = DateTime.UtcNow.AddDays(3), EndsAt = DateTime.UtcNow.AddDays(3).AddHours(2), Location = "Актова зала" });
                db.Events.Add(new Event { Title = "Тренінг", Description = "Тренінг для волонтерів", StartsAt = DateTime.UtcNow.AddDays(10), EndsAt = DateTime.UtcNow.AddDays(10).AddHours(3), Location = "Конференц-зал" });
            }

            // Profile members (профбюро)
            if (!db.ProfileMembers.Any())
            {
                db.ProfileMembers.Add(new ProfileMember { Position = "Голова", Name = "Іван Іваненко", Phone = "+380501112233", Email = "ivan@example.com" });
                db.ProfileMembers.Add(new ProfileMember { Position = "Заступник", Name = "Олена Петрівна", Phone = "+380631112233", Email = "olena@example.com" });
            }

            db.SaveChanges();
        }
    }
}
