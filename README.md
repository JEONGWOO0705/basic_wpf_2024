# basic_wpf_2024
IoT 개발자 WPF 학습 리포지토리

## 1일차
- WPF (Window Presentation Foundation)기본 학습 
    - Winforms 확장한 WPF
        - 이전의 Winforms는 이미지 비트맵 방식(2D)
        - WPF 이미지 벡터 방식()
        - XAML 화면 디자인 - 안드로이드 개발시 Java XML로 화면 디자인과 PyQt 디자인과동일
    
    - XAML(엑스에이엠엘, 재믈)
        - 여는 태크 <Window>, 닫는 태그 </Window>
        - 합치면 <Window /> - Window 태그 안에 다른 객체가 없다는 뜻
        - 여는 태그와 닫는 태그 사이에 다른 태그(객체)를 넣어서 디자인1

    - WPF 기본 사용법
        - Winforms와는 다르게 코딩으로 디자인을 함
    - 레이아웃  
        - Grid : WPF에서 가장 많이 사용되는 대표적인  레이아웃
        - StackPanel : 스택으로 컨트롤을 쌓는 레이아웃
        - Canvas : 미술에서 캔버스와 유사
        - DockPanel : 컨트롤을 방향에 따라서 도킹시키는 레이아웃
        - Margin : 여백 기능, 앵커링 같이함(중요!!)


    - 디자인, C#코드 완전분리 개발 - MVVM 디자인 패턴


## 2일차
- WPF 기본학습
    - 데이터 바인딩 : 데이터 소스(DB, 엑셀, txt, 클라우드에 보관된 데이터 원본)에 
                     데이터를 쉽게 가져다 쓰기 위한 데이터 핸들링 방법
        - xaml에서 사용하는 방법
            - {Binding Path = 속성, ElementName = 객체, Mode(OneWay|TwoWay), StringFormat={}{0:#,#}}
        - dataContext : 데이터를 담아서 전달하는 이름
        - 전통적인 윈폼 코드비하인드에서 데이터를 처리하는 것을 지양 = 디자인, 개발 부분 분리

## 3일차
- WPF에서 중요한 개념(윈폼과 다른점)
    1. 데이터 바인딩 : 바인딩 키워드로 코드와 분리
    2. 옵저버 패턴 : 값이 변경된 사실을 사용자에게 공지 OnPropertyChanged 이벤트 핸들러
    3. 디자인 리소스 : 각 컨트롤마다 디자인(x), 리소스로 디자인 공유
        - 각 화면당 Resources : 자기 화면에만 적용되는 디자인
        - App.xaml Resources : Application 전체에 적용되는 디자인
        - 리소스 사전 : 공유할 디자인 내용이 많을때 파일로 따로 지정 

- WPF MVVM
    - MVC(Model View Controller)
        - 웹개발(Spring, ASP.NET MVC, dJango, etc...) 현재도 사용되고 있음
        - Model : Data 입출력 처리를 담당
        - View : 디스플레이 화면을 담당, 순수 xaml로만 구성
        - Controller : View 를 제어 , Model 처리 

    - MVVM(Model View ViewModel)
        - Model : Data 입출력 (DB side)
        - View : 화면, 순수 xaml로만 구성
        - ViewModel : 뷰에 대한 메서드, 액션, INotifyPropertyChanged 를 구현

        ![MVVM패턴](https://raw.githubusercontent.com/JEONGWOO0705/basic_wpf_2024/main/image/wpf001.png)

    - 권장 구현 방법
        - ViewModel 생성, 알림 속성 구현
        - View에 ViewModel을 데이터 바인딩
        - Model DB작업 독립적으로 구현

    - MVVM 구현 도와주는 프레임 워크
        - **Caliburn.Micro** : 3rd Party 개발. MVVM이 아주 간단. 강력. 디버깅이 조금 어려움
        - AvaloniaUI : 3rd Party 개발, 크로스 플랫폼, 디자인은 최고!
        - Prism : Microsoft 개발, 무지막지하게 어렵다. 대규모 프로젝트 활용
        - Mvvmlight.ToolKit : 3rd Party 개발, 더이상 개발이나 지원이 없음

- Caliburn.Micro
    1. 프로젝트 생성 후, MainWindow.xaml 삭제
    2. Models, View, ViewModels 폴더(네임스페이스) 생성
    3. 종속성 NuGet패키지 Caliburn.Micro 설치
    4. 루트 폴더에 Bootstrapper.cs 클래스 생성
    5. App.xaml에서 StartupUri 삭제
    6. App.xaml에 Bootstrapper 클래스를 리소스 사전에 등록
    7. App.xaml.cs에 App() 생성자 추가
    8. ViewModels 폴더에 MainViewModel.cs 클래스 생성
    9. Bootstrapper.cs에 OnStartup에 내용을 변경
    10. Views 폴더에 MainView.xaml 생성

    - 작업(3명) 분리
        - DB 개발자 : DBMS 테이블 생성, Models에 클래스 작업
        - Xaml 디자이너 : Views 폴더에 있는 xaml 파일을 디자인 작업


## 4일차
- Caliburn.Micro
    - 작업 분리 
        - Xaml 디자이너 : xaml 파일만 디자인
        - ViewModel 개발자  : Model에 있는 DB 관련 정보와 View와 연계 전체 구현 작업

    - Caliburn.Micro 특징
        - Xaml 디자인시 {Binding ...} 잘 사용하지 않음
        - 대신 x:Name을 사용

    - MVVM 특징
        - 예외 발생시 예외 메시지 표시 없이 프로그램 종료
        - ViewModel 에서 디버깅 시작
        - View.xaml 바인딩, 버튼 클릭 이름(ViewModel 속성, 메서드) 지정 주의
        - Model은 DB 테이블 컬럼 이름 일치, CRUD 쿼리문 오타 주의
        - ViewModel 부분
            - 변수, 속성으로 분리
            - 속성이 Model내의 속성과 이름이 일치
            - List 사용 불가 -> BindableCollection으로 변경
            - 메서드와 이름이 동일한 Can... 프로퍼티 지정, 버튼 활성/비활성화
            - 모든 속성에 NotifyOfPropertyChange() 메서드 존재 해야한다 --> 값 변경을 알려줌 (Observer)



    ![실행화면](https://raw.githubusercontent.com/JEONGWOO0705/basic_wpf_2024/main/image/wpf002.png)


## 5일차
- MahApps.Metro(https//mahapps.com/)
    - Metro(Moden UI) 디자인 접목

    ![실행화면](https://raw.githubusercontent.com/JEONGWOO0705/basic_wpf_2024/main/image/wpf003.png)


    ![저장화면](https://raw.githubusercontent.com/JEONGWOO0705/basic_wpf_2024/main/image/wpf004.png)

- Movie API 연동 앱 (movieFinder)
    - 좋아하는 영화 즐겨찾기 앱
    - DB(SQL Server) 연동
    - MahApps.Metro UI
    - CefSharp WebBrowser 패키지
    - Google.Apis 패키지 
    - OpenAPI 두가지 사용
    - MVVM 사용안함
    - [TMDB](https://www.themoviedb.org/) OpenAPI 활용
        - 회원가입 후 API 신청
    - [Youtube API](https://console.cloud.google.com/)활용
        - 새 프로젝트 생성
        - API 및 서비스, 라이브러리 선택
        - YouTube Data API v3 선택
        - 사용자 인증 정보 만들기 클릭
            1. 사용자 데이터 라디오 버튼 클릭
            2. OAutho 동의화면, 기본내용 입력후 다움
            3. 범위는 저장후 계속
            4. OAuth Client ID, 앱 유형을 데스크톱 앱으로 설정, 이름 입력 후 다음 클릭


## 6일차
- MovieFinder 2024 남은것
    - 즐겨찾기 후 다시 선택 즐겨찾기 막아야함
    - 즐겨찾기 삭제
    - 그리드뷰 영화를 더블클랙하면 영화소개 팝업


## 7일차
- MovieFinder 2024 남은것
- 데이터포털 API 연동앱 예제
    - 5월 13일 개인 프로젝트 참조 소스


https://github.com/JEONGWOO0705/basic_wpf_2024/assets/84116251/103b16b4-1926-4455-b3bb-d7d2fb172f25




## 8일차
- WPF 개인 프로젝트 포트폴리오 작업
- 재난문자방송 발령현황 조회 앱 구성
- 오픈API를 통한 데이터 불러오기
- DB에 연동하여 불러오기, 저장 및 삭제 쿼리 기능 작동 구현
- CefSharpWpf를 통해 재난문자 지역을 네이버 맵을 통해 지도 나타내기 구현
- !!!! 재난문자가 자주 발생하니 json파일이 자주 업데이트 되면서 불러오기 오류가 자주 발생하는듯.... 


https://github.com/JEONGWOO0705/basic_wpf_2024/assets/84116251/90cce31d-e78b-44d6-8542-c47a2d6420a5



## 프로젝트 및 이 수업을하면서 배운점!! 

- 요약
    - OpenApi를 사용하여 데이터를 읽고 화면에 출력하는 방법을 배움
    - SQL을 활용하여 데이터를 저장,읽기,삭제 등의 기능을 구현하면서 기존의 배웠던 내용들을 학습함
    - 데이터에 들어가있는 값을 추출하여 네이버 지도를 실행시키는 기능을 구현할 수 있게 하였다.
    - Binding 짱!!


```cs
string openApiUri = "";
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
```

```cs
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
            //......
        });
    }

    // WarningList를 WPF 프로젝트의 DataContext로 설정하여 화면에 바인딩
    this.DataContext = WarningList;
```

- 특히 binding을 통해 화면에 출력할 수 있는 것은 새로웠던것 같았다.

```cs
<DataGrid x:Name="GrdResult" Grid.Row="1" Margin="10"
      IsReadOnly="True" ItemsSource="{Binding}"/>
      // xaml.cs에 DataContext에 들어간 리스트들을 표현해준다!!
```

- 데이터 클릭시 지도 확인 기능 구현
    - 또다른 xaml 창을 만들어 데이터 더블 클릭시 데이터에 알맞는 값을 보내주어 지도를 확인할 수 있는 기능을 구현하였다.
    - 부모, 자식 클래스, 오버로딩에 대해 다시 한번 복습하게 되었다.


```cs
// main xaml
private void GrdResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
{
    var curItem = GrdResult.SelectedItem as WarningMessage;
    // map.xaml.cs에 있는 오버로딩된 함수 map(string location_id)를 불러온다.
    var mapWindow = new map(curItem.location_name); 
    mapWindow.Owner = this;
}

// map xaml
 public map()
 {
     InitializeComponent();
 }
 public map(string location_id) : this() // 함수가 실행됬을때 부모의 함수(기존 생성자) 먼저 수행하게 된다.
 {
     BrsLoc.Address = $"https://map.naver.com/p/search/{location_id}";
 }

```