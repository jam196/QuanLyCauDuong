using System;

namespace Models
{
    /// <summary>
    /// Lớp cầu.
    /// </summary>
    public class Bridge : DbObject, IEquatable<Bridge>
    {
        public string Name { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }

        // Chủ đầu tư
        public string Investor { get; set; }

        // Tổng mức đầu tư
        public float TotalInvestment { get; set; }

        public float MaintenanceCost { get; set; }

        // Tải trọng thiết kế
        public float DesignLoad { get; set; }

        // Đơn vị thiết kế
        public string Designer { get; set; }

        // Đơn vị thi công
        public string Builder { get; set; }

        // Đơn vị giám sát
        public string Supervisor { get; set; }

        // Đơn vị quản lý giám sát
        public string Manager { get; set; }

        // Quốc lộ
        public string Location { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        // Trạng thái
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the id of the customer placing the order.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Lấy tên cầu + chủ đầu tư.
        /// </summary>
        public override string ToString() => $"{Name} {Investor}";

        public Bridge()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool Equals(Bridge other) =>
            Name == other.Name &&
            Investor == other.Investor;

        /*        public static implicit operator Bridge(Customer v)
                {
                    throw new NotImplementedException();
                }*/
    }
}