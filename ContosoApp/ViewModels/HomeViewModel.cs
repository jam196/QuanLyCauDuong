using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace QuanLyCauDuong.ViewModels
{
    public class ItemModel
    {
        public string Value { get; set; }
        public string Title { get; set; }
        public string Glyph { get; set; }
        public DateTime ReleaseDateTime { get; set; }
        public ItemModel()
        {
            this.Value = "Default Value";
            this.Title = "Default Title";
            this.Glyph = "&#xE74E;";
            this.ReleaseDateTime = new DateTime(1761, 1, 1);
        }
    }

    public class HomeViewModel
    {
        private ItemModel defaultRecording = new ItemModel();
        public ItemModel DefaultRecording { get { return this.defaultRecording; } }

        private ObservableCollection<ItemModel> items = new ObservableCollection<ItemModel>();
        public ObservableCollection<ItemModel> Items { get { return this.items; } }

        public MainViewModel ViewModel => App.ViewModel;

        public String ChartUrl = "";

        public HomeViewModel()
        {
            String goodBridges = ViewModel.CloneBridges.Where(bridge => bridge.Status == "Hoạt động tốt").ToList().Count.ToString();
            String buildingBridges = ViewModel.CloneBridges.Where(bridge => bridge.Status == "Đang xây dựng").ToList().Count.ToString();
            String upgradeBridges = ViewModel.CloneBridges.Where(bridge => bridge.Status == "Đang bảo trì").ToList().Count.ToString();

            this.ChartUrl = "https://quickchart.io/chart?c={type:'pie',data:{labels:['Cầu đang xây dựng','Cầu đang bảo trì','Cầu hoạt động tốt'],datasets:[{data:[" + buildingBridges + "," + upgradeBridges + "," + goodBridges + "], backgroundColor: [ 'rgb(0, 33, 113)', 'rgb(255, 171, 0)', 'rgb(0, 200, 80)']}]},options:{plugins:{datalabels:{display: true,align:'center',backgroundColor:'rgb(64, 64, 64)',color:'rgb(255, 255, 255)'}}}}";

            this.items.Add(new ItemModel()
            {
                Value = ViewModel.Bridges.Count.ToString(),
                Title = "Tổng số cầu",
                Glyph = ((char)0xF49A).ToString(),
                ReleaseDateTime = new DateTime(1748, 7, 8)
            });
            this.items.Add(new ItemModel()
            {
                Value = ViewModel.Users.Count.ToString(),
                Title = "Tổng số người dùng",
                Glyph = ((char)0xE7EE).ToString(),
                ReleaseDateTime = new DateTime(1805, 2, 11)
            });
            this.items.Add(new ItemModel()
            {
                Value = ViewModel.Histories.Count.ToString(),
                Title = "Tổng số hoạt động",
                Glyph = ((char)0xF404).ToString(),
                ReleaseDateTime = new DateTime(1737, 12, 3)
            });
            this.items.Add(new ItemModel()
            {
                Value = buildingBridges,
                Title = "Số cầu đang xây dựng",
                Glyph = ((char)0xE7C0).ToString(),
                ReleaseDateTime = new DateTime(1737, 12, 3)
            });
            this.items.Add(new ItemModel()
            {
                Value = goodBridges,
                Title = "Số cầu đang hoạt động",
                Glyph = ((char)0xE706).ToString(),
                ReleaseDateTime = new DateTime(1737, 12, 3)
            });
            this.items.Add(new ItemModel()
            {
                Value = upgradeBridges,
                Title = "Số cầu cần nâng cấp",
                Glyph = ((char)0xE791).ToString(),
                ReleaseDateTime = new DateTime(1737, 12, 3)
            });
        }
    }
}