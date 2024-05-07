using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;


namespace ex10_MovieFinder2024
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
        private async void SearchMovie(string movieName)
        {
            string tmdb_apiKey = "f4b0efb9e0cf84286dd0e1f284d79fc5"; // TMDB  사이트에서 제공받은 API  키
            string encoding_movieName = HttpUtility.UrlEncode(movieName,Encoding.UTF8);
            string openApiUri = $"https://api.themoviedb.org/3/search/movie?api_key={tmdb_apiKey}" +
                                $"&language=ko-KR&page=1&include_adult=false&query={encoding_movieName}";

            //Debug.WriteLine(openApiUri);

            string result = string.Empty; // 결과값을 담을 공간

            // OpenApi 실행 객체
            WebRequest req = null;
            WebResponse res = null;
            StreamReader reader = null;

            try
            {
                //tmdb api 요청
                req = WebRequest.Create(openApiUri); // URL을 넣어서 객체를 생성
                res = await req.GetResponseAsync(); // 요청한 URL의 결과를 비동기 응답으로 받는다
                reader = new StreamReader(res.GetResponseStream()); // 
                result = reader.ReadToEnd();  //json 결과를 문자열로 저장

                Debug.WriteLine(result);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message}");
                // TODO : 메시지 박스로 출력
            }
            finally
            {
                reader.Close();
                res.Close();
            }
        }

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtMovieName.Text))
            {
                await this.ShowMessageAsync("검색", "검색할 영화명을 입력하세요");
                return;
            }
            SearchMovie(TxtMovieName.Text);
        }


        #region 'Button'
        private async void BtnAddFavorite_Click(object sender, RoutedEventArgs e)
        {
            await this.ShowMessageAsync("즐겨찾기", "즐겨찾기 추가합니다!!");

        }

        private async void BtnViewFavorite_Click(object sender, RoutedEventArgs e)
        {
            await this.ShowMessageAsync("즐겨찾기", "즐겨찾기 확인합니다!!");

        }

        private async void BtnDelFavorite_Click(object sender, RoutedEventArgs e)
        {
            await this.ShowMessageAsync("즐겨찾기", "즐겨찾기 삭제합니다!!");

        }

        private async void BtnWatchTrailer_Click(object sender, RoutedEventArgs e)
        {
            await this.ShowMessageAsync("유투브 예고편", "예고편 확인합니다.!!");

        }

        private async void BtnNaverMovie_Click(object sender, RoutedEventArgs e)
        {
            await this.ShowMessageAsync("죄송 ㅜㅜ", "서비스가 종료되었습니다.ㅜㅜㅜ");
        }
        #endregion

        private void TxtMovieName_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private async void GrdResult_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            await this.ShowMessageAsync("포스터", "포스터 처리합니다");

        }
    }
}