using MahApps.Metro.Controls;

namespace WPF_MiniProject
{
    /// <summary>
    /// map.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class map : MetroWindow
    {
        public map()
        {
            InitializeComponent();
        }
        public map(string location_id) : this()
        {
            BrsLoc.Address = $"https://map.naver.com/p/search/{location_id}";
        }
    }
}
