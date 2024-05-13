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

            try
            {
                var jsonResult = JObject.Parse(result);

                var firstDisasterMsg = jsonResult["DisasterMsg"][0];
                var resultObject = firstDisasterMsg["head"][2]["RESULT"];
                var resultCode = (string)resultObject["resultCode"];



                if (resultCode == "INFO-0")
                {
                    var data = jsonResult["DisasterMsg"][1]["row"];
                    var jsonArray = data as JArray; // json 자체에서 []안에 들어간 배열데이터만 JArray 변환가능

                    var WarningList = new List<WarningMessage>();
                    foreach (var item in jsonArray)
                    {
                        WarningList.Add(new WarningMessage()
                        {
                            create_date = Convert.ToDateTime(item["create_date"]),
                            location_id = Convert.ToString(item["location_id"]),
                            location_name = Convert.ToString(item["location_name"]),
                            md101_sn = Convert.ToInt32(item["md101_sn"]),
                            msg = Convert.ToString(item["msg"]),

                        });
                    }

                    this.DataContext = WarningList;
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
                await this.ShowMessageAsync("목록 추가", "추가할 메세지를 선택하세요");
                return;
            }


            var addMessageItems = new List<WarningMessage>();
            foreach (WarningMessage item in GrdResult.SelectedItems)
            {
                addMessageItems.Add(item);
            }

            try
            {
                var insRes = 0;
                using (SqlConnection conn = new SqlConnection(helpers.Common.CONNSTRING))
                {
                    conn.Open();
                    foreach (WarningMessage item in addMessageItems)
                    {
                        SqlCommand cmd = new SqlCommand(Models.WarningMessage.INSERT_QUERY, conn);
                        //cmd.Parameters.AddWithValue("@Id", item.Id);
                        cmd.Parameters.AddWithValue("@create_date", item.create_date);
                        cmd.Parameters.AddWithValue("@location_id", item.location_id);
                        cmd.Parameters.AddWithValue("@location_name", item.location_name);
                        cmd.Parameters.AddWithValue("@md101_sn", item.md101_sn);
                        cmd.Parameters.AddWithValue("@msg", item.msg);

                        insRes += cmd.ExecuteNonQuery();
                    }
                }

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

                    var cmd = new SqlCommand(Models.WarningMessage.SELECT_QUERY, conn);
                    var adapter = new SqlDataAdapter(cmd);
                    var dSet = new DataSet();
                    adapter.Fill(dSet, "WarningMessage");

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