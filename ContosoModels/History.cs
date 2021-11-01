using System;

namespace Models
{
    public class History : DbObject, IEquatable<History>
    {
        public History()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public String Content { get; set; }

        public bool Equals(History other)
        {
            throw new NotImplementedException();
        }
    }
}
