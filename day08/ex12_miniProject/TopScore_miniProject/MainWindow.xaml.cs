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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.IO;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json.Linq;
using TopScore_miniProject.Models;


namespace TopScore_miniProject
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

        private async void BtnReqRealtime_Click(object sender, RoutedEventArgs e)
        {
            string openApiUri = "https://api.sportmonks.com/v3/football/rivals?api_token=MjNbtYxw4psAGTc6MsJp0HUrrVBLo7rq3tTWFvpkkxtlRz51IhNTbSP1FPaz";
            string result = string.Empty;

            // WebRequest, WebResponse 객체
            WebRequest req = null;   // 웹 리소스를 나타내는 개체를 만드는데 사용(WebRequest) , 
            WebResponse res = null; // WebResponse는 웹 리소스에 대한 응답을 나타내는 개체
            StreamReader reader = null; // 웹 응답을 읽기위한 스티림 리더

            /*
             --> req를 사용하여 openApiUri에 대한 HTTP Get 요청을 생성 
            --> res 를 사용하여 서버에 대한 응답을 받고, reader를 통해 응답을 읽어온다.
             */



            try
            {
                req = WebRequest.Create(openApiUri);    // 지정된 URL에 대한 WebRequest 생성
                res = await req.GetResponseAsync();     // 비동기 방식으로 서버 응답을 받음
                reader = new StreamReader(res.GetResponseStream());      // 응답 스트림을 읽을 수 있는 StreamReader 생성
                result = await reader.ReadToEndAsync();     // 비동기 방식으로 응답 데이터를 모두 읽어옴

                //await this.ShowMessageAsync("결과", result);
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", $"OpenAPI 조회오류 {ex.Message}");
            }

            try
            {
                // --> json 파일 읽고 분류하는 작업
                var jsonResult = JObject.Parse(result);

                var resultCode = jsonResult["timezone"].ToString();




                if (resultCode == "UTC") // json파일안에 있는 resultCode 가 INFO-0 일때 실행해라
                {
                    var data = jsonResult["data"] as JArray; // // JSON 데이터에서 필요한 배열 데이터를 가져옴
                    var jsonArray = data as JArray; // json 자체에서 []안에 들어간 배열데이터만 JArray 변환가능,  // 배열 데이터를 JArray 형식으로 변환

                    var TopScoreitems = new List<Topscore>();   // WarningMessage 객체를 담을 리스트 생성
                    foreach (var item in jsonArray)
                    {
                        // 각 배열 요소에서 필요한 정보를 추출하여 WarningMessage 객체를 생성하여 리스트에 추가
                        TopScoreitems.Add(new Topscore()
                        {

                            //sport_id = Convert.ToInt32(item["sport_id"]),
                            team_id = Convert.ToInt32(item["team_id"]),
                            rival_id = Convert.ToInt32(item["rival_id"]),
                            //id = Convert.ToInt32(item["id"]),

                        });
                    }

                    // WarningList를 WPF 프로젝트의 DataContext로 설정하여 화면에 바인딩
                    this.DataContext = TopScoreitems;

                    // 화면에 조회 결과 메시지 표시
                    //StsResult.Content = $"OpenAPI {WarningList.Count} 건 조회 완료!!";
                }
            }
            catch (Exception ex)
            {

                await this.ShowMessageAsync("오류", $"오류발생{ex.Message}");
            }


        }
    }
}