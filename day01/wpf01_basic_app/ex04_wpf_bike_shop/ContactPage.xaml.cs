using ex04_wpf_bike_shop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ex04_wpf_bike_shop
{
    /// <summary>
    /// ContactPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ContactPage : Page
    {
        public ContactPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // 코드에서 속성값을 지정할때 사용방법
            Bike MyBike = new Bike();
            MyBike.Speed = 60;
            MyBike.Color = Colors.Black;

            TextBox text1 = new TextBox();
            StpBike.DataContext = MyBike;
            //MessageBox.Show(DgBike.Speed.ToString());

        }

       
    }
}
