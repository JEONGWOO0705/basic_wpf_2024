using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json.Linq;
using WPF_MiniProject.Models;
using Microsoft.Data.SqlClient;
using System.Data;


namespace WPF_MiniProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }


        private async void BtnReqRealtime_Click(object sender, RoutedEventArgs e)
        {
            string openApiUri = "http://apis.data.go.kr/1741000/DisasterMsg3/getDisasterMsg1List?serviceKey=racrqCX%2F1FixzUeq7J1HnKHDBb4wH1V7etLp2aOW0yW3fR3uHxPuvyp7qrW6FyhX8R1SDllWGu9qT7Il9%2FA7%2Fg%3D%3D&pageNo=1&numOfRows=10&type=json";
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

                var firstDisasterMsg = jsonResult["DisasterMsg"][0];
                var resultObject = firstDisasterMsg["head"][2]["RESULT"];
                var resultCode = (string)resultObject["resultCode"];

                

                if (resultCode == "INFO-0") // json파일안에 있는 resultCode 가 INFO-0 일때 실행해라
                {
                    var data = jsonResult["DisasterMsg"][1]["row"]; // // JSON 데이터에서 필요한 배열 데이터를 가져옴
                    var jsonArray = data as JArray; // json 자체에서 []안에 들어간 배열데이터만 JArray 변환가능,  // 배열 데이터를 JArray 형식으로 변환

                    var WarningList = new List<WarningMessage>();   // WarningMessage 객체를 담을 리스트 생성
                    foreach (var item in jsonArray)
                    {
                        // 각 배열 요소에서 필요한 정보를 추출하여 WarningMessage 객체를 생성하여 리스트에 추가
                        WarningList.Add(new WarningMessage()
                        {
                            create_date = Convert.ToDateTime(item["create_date"]),
                            location_id = Convert.ToString(item["location_id"]),
                            location_name = Convert.ToString(item["location_name"]),
                            md101_sn = Convert.ToInt32(item["md101_sn"]),
                            msg = Convert.ToString(item["msg"]),

                        });
                    }

                    // WarningList를 WPF 프로젝트의 DataContext로 설정하여 화면에 바인딩
                    this.DataContext = WarningList;

                    // 화면에 조회 결과 메시지 표시
                    StsResult.Content = $"OpenAPI {WarningList.Count} 건 조회 완료!!";
                }
            }
            catch (Exception ex)
            {

                await this.ShowMessageAsync("오류", $"오류발생{ex.Message}");
            }

            

        }

        private async void BtnSaveData_Click(object sender, RoutedEventArgs e)
        {
            if (GrdResult.SelectedItems.Count == 0)
            {   
                // 선택된 항목이 없을 때 메시지를 표시하고 메서드를 종료합니다.
                await this.ShowMessageAsync("목록 추가", "추가할 메세지를 선택하세요");
                return;
            }


            // 선택된 항목들을 담을 리스트를 생성합니다.
            var addMessageItems = new List<WarningMessage>();

            // 데이터 그리드에서 선택된 각 항목을 리스트에 추가합니다.
            foreach (WarningMessage item in GrdResult.SelectedItems)
            {
                addMessageItems.Add(item);
            }

            try
            {
                var insRes = 0; // 삽입 결과를 저장할 변수
                using (SqlConnection conn = new SqlConnection(helpers.Common.CONNSTRING))       // 데이터베이스 연결 문자열을 사용하여 SqlConnection 객체 생성
                {
                    conn.Open();


                    // addMessageItems 리스트에 있는 각 WarningMessage 객체에 대해 반복문 실행
                    foreach (WarningMessage item in addMessageItems)
                    {
                        SqlCommand cmd = new SqlCommand(Models.WarningMessage.INSERT_QUERY, conn);
                        //cmd.Parameters.AddWithValue("@Id", item.Id);

                        // 각 파라미터에 해당하는 속성 값을 설정
                        cmd.Parameters.AddWithValue("@create_date", item.create_date);
                        cmd.Parameters.AddWithValue("@location_id", item.location_id);
                        cmd.Parameters.AddWithValue("@location_name", item.location_name);
                        cmd.Parameters.AddWithValue("@md101_sn", item.md101_sn);
                        cmd.Parameters.AddWithValue("@msg", item.msg);


                        // 삽입 결과를 반환하는 ExecuteNonQuery 메서드 실행
                        /*
                         
                        이 코드 라인인 insRes += cmd.ExecuteNonQuery();는 
                        데이터베이스에 SQL 쿼리를 실행하여 영향을 받는 행의 수를 반환하는 
                        ExecuteNonQuery 메서드를 사용합니다. 
                        이 메서드는 INSERT, UPDATE, DELETE와 같이 데이터를 변경하는 
                        SQL 쿼리를 실행할 때 사용됩니다.
                         */ 
                        insRes += cmd.ExecuteNonQuery();
                    }
                }


                // 삽입된 행의 수와 리스트의 개수를 비교하여 결과 메시지를 표시
                if (insRes == addMessageItems.Count)
                {
                    await this.ShowMessageAsync("즐겨찾기", "즐겨찾기 성공!");

                }
                else
                {
                    await this.ShowMessageAsync("즐겨찾기", $"즐겨찾기 {addMessageItems.Count}건 중 {insRes}건 저장성공!");
                }
            }
            catch (Exception ex)
            {

                await this.ShowMessageAsync("오류", $"즐겨찾기 오류{ex.Message}");
            }
        }

        private async void BtnCallData_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = null; // 데이터그래드에 보낸 데이터를 모두 삭제

            List<WarningMessage> viewMessage = new List<WarningMessage>();

            try
            {
                using (SqlConnection conn = new SqlConnection(helpers.Common.CONNSTRING))
                {
                    conn.Open();


                    // SqlDataAdapter를 사용하여 데이터를 DataSet에 채웁니다.
                    var cmd = new SqlCommand(Models.WarningMessage.SELECT_QUERY, conn);
                    var adapter = new SqlDataAdapter(cmd);
                    var dSet = new DataSet();
                    adapter.Fill(dSet, "WarningMessage");
                    // SqlDataAdapter를 사용하여 SQL 쿼리를 실행하고
                    // , 그 결과를 DataSet에 채웁니다. "WarningMessage"라는 이름의 테이블에 결과가 저장됩니다.


                    // 조회된 데이터를 WarningMessage 객체로 변환하여 viewMessage 리스트에 추가합니다.
                    foreach (DataRow row in dSet.Tables["WarningMessage"].Rows)
                    {
                        var calledMessage = new WarningMessage()
                        {
                            //Id = Convert.ToInt32(row["ID"]),
                            create_date = Convert.ToDateTime(row["create_date"]),
                            location_id = Convert.ToString(row["location_id"]),
                            location_name = Convert.ToString(row["location_name"]),
                            md101_sn = Convert.ToInt32(row["md101_sn"]),
                            msg = Convert.ToString(row["msg"])
                        };
                        viewMessage.Add(calledMessage);
                    }

                    // 조회된 데이터를 화면에 바인딩합니다.
                    this.DataContext = viewMessage;
                    StsResult.Content = $"즐겨찾기 {viewMessage.Count}건 조회완료";


                }
            }
            catch (Exception ex)
            {

                await this.ShowMessageAsync("즐겨찾기", $"즐겨찾기 조회오류 {ex.Message}");
            }
        }

        private async void BtnDelData_Click(object sender, RoutedEventArgs e)
        {
            if (GrdResult.SelectedItems.Count == 0)
            {
                await this.ShowMessageAsync("삭제", "삭제할 영화를 선택하세요.");
                return;
            }

            // 삭제시작!
            try
            {
                using (SqlConnection conn = new SqlConnection(helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    var delRes = 0;

                    foreach (WarningMessage item in GrdResult.SelectedItems)
                    { 
                        SqlCommand cmd = new SqlCommand(Models.WarningMessage.DELETE_QUERY, conn);
                        cmd.Parameters.AddWithValue("@md101_sn", item.md101_sn);

                        delRes += cmd.ExecuteNonQuery();
                    }

                    if (delRes == GrdResult.SelectedItems.Count)
                    {
                        await this.ShowMessageAsync("삭제", $"즐겨찾기 {delRes}건 삭제");
                    }
                    else
                    {
                        await this.ShowMessageAsync("삭제", $"즐겨찾기 {GrdResult.SelectedItems.Count}건중 {delRes} 건 삭제");
                    }
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", $"즐겨찾기 조회오류 {ex.Message}");
            }

            BtnCallData_Click(sender, e);
        }

        private void GrdResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var curItem = GrdResult.SelectedItem as WarningMessage;
            var mapWindow = new map(curItem.location_name);
            mapWindow.Owner = this;
            mapWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mapWindow.ShowDialog();
        }

        private void CboReqDate_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
        }
    }
}