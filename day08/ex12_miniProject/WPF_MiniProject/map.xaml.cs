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
        public map(string location_id) : this()     // 함수가 실행됬을때 부모의 함수(기존 생성자) 먼저 수행하게 된다.
        {
            BrsLoc.Address = $"https://map.naver.com/p/search/{location_id}";
        }
    }
}
