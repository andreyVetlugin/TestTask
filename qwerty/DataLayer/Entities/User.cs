using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataLayer.Entities
{
    public class User : IBenefitsEntity
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(255)]
        public string Login { get; set; }
        [MaxLength(255)]
        public string Password { get; set; }

        public string Name { get; set; }

        public string SecondName { get; set; }
        
    }

    public static class UserQuereableExtensions
    {
        public static IQueryable<User> ByLoginAndPassword(this IQueryable<User> users, string login, string password) =>
            users.Where(u => u.Login == login && u.Password == password);

        public static IQueryable<User> ByName(this IQueryable<User> users, string name) =>
            users.Where(u => u.Login==name);

        public static IQueryable<User> ById(this IQueryable<User> users, Guid userId) =>
            users.Where(u => u.Id == userId);
    }
}
