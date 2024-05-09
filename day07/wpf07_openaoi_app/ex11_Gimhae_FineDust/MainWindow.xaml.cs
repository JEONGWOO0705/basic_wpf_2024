using System.IO;
using System.Net;
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
using ex11_Gimhae_FineDust.Models;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;

namespace ex11_Gimhae_FineDust
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        // 실시간조회 버튼 클릭
        private async void BtnReqRealtime_Click(object sender, RoutedEventArgs e)
        {
            string openApiUri = "https://smartcity.gimhae.go.kr/smart_tour/dashboard/api/publicData/dustSensor";
            string result = string.Empty;

            // WebRequest, WebResponse 객체
            WebRequest req = null;
            WebResponse res = null;
            StreamReader reader = null;

            try
            {
                req = WebRequest.Create(openApiUri);
                res = await req.GetResponseAsync();
                reader = new StreamReader(res.GetResponseStream());
                result = await reader.ReadToEndAsync();

                //await this.ShowMessageAsync("결과", result);
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", $"OpenAPI 조회오류 {ex.Message}");
            }

            var jsonResult = JObject.Parse(result);
            var status = Convert.ToInt32(jsonResult["status"]);

            if (status == 200)
            {
                var data = jsonResult["data"];
                var jsonArray = data as JArray; // json 자체에서 []안에 들어간 배열데이터만 JArray 변환가능

                var dustSensors = new List<DustSensro>();
                foreach (var item in jsonArray)
                {
                    dustSensors.Add(new DustSensro()
                    {
                        Id = 0,
                        Dev_id = Convert.ToString(item["dev_id"]),
                        Name = Convert.ToString(item["name"]),
                        Loc = Convert.ToString(item["loc"]),
                        Company_id = Convert.ToString(item["company_id"]),
                        Company_Name = Convert.ToString(item["company_name"]),
                        Coordx = Convert.ToDouble(item["coordx"]),
                        Coordy = Convert.ToDouble(item["coordy"]),
                        Pm10_after = Convert.ToInt32(item["pm10_after"]),
                        State = Convert.ToInt32(item["state"]),
                        Ison = Convert.ToBoolean(item["ison"]),
                        Timestamp = Convert.ToDateTime(item["timestamp"]),
                    });
                }

                this.DataContext = dustSensors;
                StsResult.Content = $"OpenAPI {dustSensors.Count} 건 조회 완료!!";
            }
        }

        private void BtnSaveData_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CboReqDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void GrdResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}