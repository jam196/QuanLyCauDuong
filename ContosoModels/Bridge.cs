using System;

namespace Models
{
    /// <summary>
    /// Lớp cầu.
    /// </summary>
    public class Bridge : DbObject, IEquatable<Bridge>
    {
        public string Name { get; set; }
        public string StartTime { get; set; }
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
        public string Location { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        // Trạng thái
        public string Status { get; set; }

        /// <summary>
        /// Lấy tên cầu + chủ đầu tư.
        /// </summary>
        public override string ToString() => $"{Name} {Investor}";

        public bool Equals(Bridge other) =>
            Name == other.Name &&
            Investor == other.Investor;

        /*        public static implicit operator Bridge(Customer v)
                {
                    throw new NotImplementedException();
                }*/
    }
}