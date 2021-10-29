using System;

namespace Models
{
    /// <summary>
    /// Lớp User.
    /// </summary>
    public class User : DbObject, IEquatable<User>
    {
        public User()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool Equals(User other) =>
            Name == other.Name &&
            Email == other.Email;
    }
}