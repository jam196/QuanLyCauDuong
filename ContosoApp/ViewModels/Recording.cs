using System;
using System.Collections.ObjectModel;

namespace QuanLyCauDuong.ViewModels
{
    public class Recording
    {
        public string Value { get; set; }
        public string Title { get; set; }
        public string Glyph { get; set; }
        public DateTime ReleaseDateTime { get; set; }
        public Recording()
        {
            this.Value = "Wolfgang Amadeus Mozart";
            this.Title = "Andante in C for Piano";
            this.Glyph = "&#xE74E;";
            this.ReleaseDateTime = new DateTime(1761, 1, 1);
        }
        public string OneLineSummary
        {
            get
            {
                return $"{this.Title} by {this.Value}, released: "
                    + this.ReleaseDateTime.ToString("d");
            }
        }
    }

    public class Model
    {
        public string Month { get; set; }

        public double Target { get; set; }

        public Model(string xValue, double yValue)
        {
            Month = xValue;
            Target = yValue;
        }
    }


    public class RecordingViewModel
    {
        private Recording defaultRecording = new Recording();
        public Recording DefaultRecording { get { return this.defaultRecording; } }

        private ObservableCollection<Recording> recordings = new ObservableCollection<Recording>();
        public ObservableCollection<Recording> Recordings { get { return this.recordings; } }

        public ObservableCollection<Model> Data { get; set; }

        public RecordingViewModel()
        {
            this.recordings.Add(new Recording()
            {
                Value = "39",
                Title = "Tổng số cầu",
                Glyph = ((char)0xF49A).ToString(),
                ReleaseDateTime = new DateTime(1748, 7, 8)
            });
            this.recordings.Add(new Recording()
            {
                Value = "80",
                Title = "Tổng số người dùng",
                Glyph = ((char)0xE7EE).ToString(),
                ReleaseDateTime = new DateTime(1805, 2, 11)
            });
            this.recordings.Add(new Recording()
            {
                Value = "191",
                Title = "Tổng số hoạt động",
                Glyph = ((char)0xF404).ToString(),
                ReleaseDateTime = new DateTime(1737, 12, 3)
            });

            Data = new ObservableCollection<Model>()
        {
            new Model("Jan", 50),
            new Model("Feb", 70),
            new Model("Mar", 65),
            new Model("Apr", 57),
            new Model("May", 48),
        };
        }
    }
}