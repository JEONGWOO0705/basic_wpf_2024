using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ex05_wpf_bikeshop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //  객체 생성시
            Bike yourBike = new Bike() { Speed = 50 , Color = Colors.Aqua}; // 방법 1

            Bike mybike = new Bike(); // 방법 2
            mybike.Speed = 100;
            mybike.Color = Colors.AliceBlue;

            StsBike.DataContext = yourBike; // WPF방식

            TxtMyBikeSpeed.Text = mybike.Speed.ToString(); // 전통적인 윈폼 방식
        }

      
        private void TxtMyBikeSpeed_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TxtCopySpeed.Text = TxtMyBikeSpeed.Text;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("버튼 클릭!!!");

        }
    }
}