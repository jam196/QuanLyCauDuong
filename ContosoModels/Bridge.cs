using System;

namespace Models
{
    /// <summary>
    /// Represents a customer.
    /// </summary>
    public class Bridge : DbObject, IEquatable<Bridge>
    {
        public string Name { get; set; }

        // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public string StartTime { get; set; }

        // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public string EndTime { get; set; }

        // Chủ đầu tư
        public string Investor { get; set; }

        // Tổng mức đầu tư
        public float TotalInvestment { get; set; }

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
        // [Required(ErrorMessage = "Địa điểm không được bỏ trống")]
        public string Location { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        // Trạng thái
        public string Status { get; set; }

        /// <summary>
        /// Returns the customer's name.
        /// </summary>
        public override string ToString() => $"{Name} {Investor}";

        public bool Equals(Bridge other) =>
            Name == other.Name &&
            Investor == other.Investor;

        public static implicit operator Bridge(Customer v)
        {
            throw new NotImplementedException();
        }
    }
}