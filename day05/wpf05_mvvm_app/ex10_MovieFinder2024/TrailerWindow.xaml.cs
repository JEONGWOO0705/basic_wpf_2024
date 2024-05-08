using ex10_MovieFinder2024.Models;
using Google.Apis.YouTube.v3;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Google.Apis.Services;
using System.Diagnostics;

namespace ex10_MovieFinder2024
{
    /// <summary>
    /// TrailerWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TrailerWindow : MetroWindow
    {
        List<YoutubeItem> youtubeItems = null;
        public TrailerWindow()
        {
            InitializeComponent();
        }

        // MainWindow 그리드에서 선택된 영화제목 넘기면서 생성
        // 재정의 생성자
        public TrailerWindow(string movieName) : this()
        {
            // this() => TrailerWindow() 생성자를 먼저 실행한 뒤
            LblMovieName.Content = $"{movieName} 예고편";
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            youtubeItems = new List<YoutubeItem>(); // 초기화
            SearchYoutubeApi(); // 핵심 메서드 실행
        }

        private async void SearchYoutubeApi()
        {
            await LoadDataCollection(); // 비동기로 유튜브 API 실행
            LsvResult.ItemsSource = youtubeItems;
            LsvResult.SelectedIndex = 0; // 맨 첫번째 것이 선택
        }

        private async Task LoadDataCollection()
        {
            // Youtube Service 용 패키지 다운로드
            var service = new YouTubeService(
                new BaseClientService.Initializer()
                {
                    ApiKey = "AIzaSyBzQafmPC4b_iF_sa0rSN4FMXDViRIkbrE",
                    ApplicationName = this.GetType().ToString()
                });
            var req = service.Search.List("snippet");
            req.Q = LblMovieName.Content.ToString(); // 어벤져스 인피티니 예고편
            req.MaxResults = 10;

            var res = await req.ExecuteAsync(); // Youtube 서버에서 요청된 값 실행하고 결과를 리턴

            //await this.ShowMessageAsync("검색결과",res.Items.Count.ToString());

            foreach (var item in res.Items)
            {
                if (item.Id.Kind.Equals("youtube#video")) // 동영상 플레이 가능한 아이템
                {
                    var youtube = new YoutubeItem()
                    {
                        Title = item.Snippet.Title,
                        ChannelTitle = item.Snippet.ChannelTitle,
                        URL = $"https://www.youtube.com/watch?v={item.Id.VideoId}",// 유투브 플레이 링크
                        Author = item.Snippet.ChannelId
                    };

                    youtube.Thumbnail = new BitmapImage(new Uri(item.Snippet.Thumbnails.Default__.Url, UriKind.RelativeOrAbsolute));

                    youtubeItems.Add(youtube);
                }
            }


        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 한번씩 CefSharp 브라우저에서 메모리 릭발생
            BrsYoutube.Address = string.Empty; // string은 null 이 아니라 string.Empty 로 하기
            BrsYoutube.Dispose(); // 종종 앱 종료시 객체를 완전 해제해야 오류 발생을 안함
        }

        private void LsvResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(LsvResult.SelectedItem is YoutubeItem)   // is  => true/false
            {
                var video = LsvResult.SelectedItem as YoutubeItem;  // as => casting 실패하면 null
                Debug.WriteLine(video.URL);
                BrsYoutube.Address = video.URL;
            }
        }
    }
}
