using Caliburn.Micro;
using ex07_EmployeeMngApp.Helpers;
using ex07_EmployeeMngApp.Models;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Data.SqlClient;
using System.Windows;

namespace ex07_EmployeeMngApp.ViewModels
{
    public class MainViewModel : Conductor<object>
    {
        private IDialogCoordinator _dialogCoordinator;

        // 멤버 변수  --> private 으로 설정할때는 소문자로 설정한다!!
        private int id;
        private string empName;
        private decimal salary;
        private string deptName;
        private string addr;


        // List는 정적인 컬렉션, BindableCollection은 동적인 컬렉션.
        // MVVM 처럼 List 사용 못함
        private BindableCollection<Employees> listEmployees;

        private Employees selectedEmployee;

        // 속성
        // --> 이 값이 xaml에 Binding  되있는 값으로 들어감
        public int Id
        {
            get => id; set
            {
                id = value;
                NotifyOfPropertyChange(() => id);
                NotifyOfPropertyChange(() => CanDelEmployee); // 삭제 여부 속성도 변경했다고 알려줘야함
            }
        }
        public string EmpName
        {
            get => empName; set
            {
                empName = value;
                NotifyOfPropertyChange(() => empName);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            }
        }
        public decimal Salary
        {
            get => salary; set
            {
                salary = value;
                NotifyOfPropertyChange(() => salary);
                NotifyOfPropertyChange(() => CanDelEmployee);
                NotifyOfPropertyChange(() => CanSaveEmployee);


            }
        }
        public string DeptName
        {
            get => deptName; set
            {
                deptName = value;
                NotifyOfPropertyChange(() => deptName);
                NotifyOfPropertyChange(() => CanDelEmployee);
                NotifyOfPropertyChange(() => CanSaveEmployee);


            }
        }
        public string Addr
        {
            get => addr; set
            {
                addr = value;
                NotifyOfPropertyChange(() => addr);
                NotifyOfPropertyChange(() => CanDelEmployee);

            }
        }



        // DataGrid에 뿌릴 EMployees 테이블 데이터
        public BindableCollection<Employees> ListEmployees
        {
           get => listEmployees;
            set
            { 
                listEmployees = value;
                // 값이 변경된 것을 시스템에 알려줌
                NotifyOfPropertyChange(() => listEmployees);
            }
        }

        public Employees SelectedEmployee
        {
            get => selectedEmployee;
            set
            {
                selectedEmployee = value;
                // 데이터를 TextBox들에 전달
                if(selectedEmployee != null)
                {
                    Id = value.Id;
                    EmpName = value.EmpName;
                    Salary = value.Salary;
                    DeptName = value.DeptName;  
                    Addr = value.Addr;

                    NotifyOfPropertyChange(() => SelectedEmployee);
                    // View에 데이터가 표시될려면 필수
                    NotifyOfPropertyChange(() => Id);
                    NotifyOfPropertyChange(() => EmpName);
                    NotifyOfPropertyChange(() => Salary);
                    NotifyOfPropertyChange(() => DeptName);
                    NotifyOfPropertyChange(() => Addr);

                }
            }
        }



        public MainViewModel()
        {
            DisplayName = "직원관리 시스템";

            // 조회실행
            GetEmployees();
        }

        // 저장버튼 활성화 여부 속성
        public bool CanSaveEmployee
        {
            get
            {
                if(string.IsNullOrEmpty(EmpName) || Salary == 0 || string.IsNullOrEmpty(DeptName))
                    return false;
                else return true;
            }
        }

        /// <summary>
        /// Caliburn.Micro  가 Xaml의 버튼 x:Name 과 동일한 이름의 메서드로 메핑
        /// </summary>
        public async void SaveEmployee()
        {
            if(Common.DialogCoordinator != null)
            {
                this._dialogCoordinator = Common.DialogCoordinator;
            }
            using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))    // Common에 적힌 SQL 주소(CONNSTRING)로 통해 접속 연결 해준다
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                if(Id == 0)
                {
                    cmd.CommandText = Models.Employees.INSERT_QUERY;
                }
                else
                {
                    cmd.CommandText = Models.Employees.UPDATE_QUERY;
                }

                SqlParameter prmEmpName = new SqlParameter("@EmpName",EmpName);
                cmd.Parameters.Add(prmEmpName);

                /*
                 SqlParameter 객체 생성: 첫 번째 줄은 SqlParameter 객체를 생성합니다.
                이 객체는 SQL 쿼리에 전달될 매개변수를 나타냅니다. 여기서 "Salary"는 매개변수의 이름을 지정하고,
                Salary는 해당 매개변수의 값입니다.

                cmd.Parameters에 매개변수 추가: 두 번째 줄은 SqlCommand의 Parameters 컬렉션에 SqlParameter를 추가합니다. 
                이렇게 하면 SQL 쿼리에 매개변수를 전달할 수 있게 됩니다. 
                이렇게 함으로써 쿼리의 내용과는 독립적으로 매개변수 값을 설정할 수 있으며,
                SQL Injection과 같은 공격을 방지할 수 있습니다.
                 */


                SqlParameter prmSalary = new SqlParameter("@Salary", Salary);
                cmd.Parameters.Add(prmSalary);

                SqlParameter prmDeptName = new SqlParameter("@DeptName", DeptName);
                cmd.Parameters.Add(prmDeptName);

                SqlParameter prmAddr = new SqlParameter("@Addr", Addr??(object)DBNull.Value); // 주소가 빈값일때 컬럼에 null값 을 입력
                cmd.Parameters.Add(prmAddr);

                if(Id != 0) // 업데이트면 Id 파라미터 필요
                {
                    SqlParameter prmId = new SqlParameter("@id", Id);
                    cmd.Parameters.Add(prmId);
                }

                var result = cmd.ExecuteNonQuery();
                if(result > 0)
                {
                    //MessageBox.Show("저장 성공!");
                    await this._dialogCoordinator.ShowMessageAsync(this, "저장성공!!", "저장");

                }
                else
                {
                    //MessageBox.Show("저장 실패!");
                    await this._dialogCoordinator.ShowMessageAsync(this, "저장 실패!!", "저장실패");
                }
                GetEmployees();
                NewEmployee(); // 모든 입력 컨트롤 초기화
            }
        }

        public void GetEmployees()
        {
            using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(Models.Employees.SELECT_QUERY,conn);
                SqlDataReader reader = cmd.ExecuteReader();
                ListEmployees = new BindableCollection<Employees>();
                while (reader.Read())
                {
                    ListEmployees.Add(new Employees()
                    {
                        Id = (int)reader["Id"],
                        EmpName = reader["EmpName"].ToString(),
                        Salary = (decimal)reader["Salary"],
                        DeptName = reader["DeptName"].ToString(),
                        Addr = reader["Addr"].ToString()
                    });
                }
            }
        }


        // 삭제 버튼을 누를 수 있는지 여부 확인
        public bool CanDelEmployee
        {
            get { return Id != 0; } // TextBox Id 속성의 값이 0이면 false, 0이 아니면 true
        }
        public async void DelEmployee()
        {
            if (Common.DialogCoordinator != null)
            {
                this._dialogCoordinator = Common.DialogCoordinator;
            }
            if (Id == 0)
            {
                await this._dialogCoordinator.ShowMessageAsync(this, "삭제불가!!", "삭제");

                return;
            }

            var val = await this._dialogCoordinator.ShowMessageAsync(this, "삭제하시겠습니까?", "삭제 여부", MessageDialogStyle.AffirmativeAndNegative);
            if (val == MessageDialogResult.Negative)
            {
                return;
            }

            using(SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(Models.Employees.DELETE_QUERY,conn);
                SqlParameter prmId = new SqlParameter("Id", Id);
                cmd.Parameters.Add(prmId);

                var res = cmd.ExecuteNonQuery();
                //ExecuteNonQuery() 메서드는 SQL 쿼리를 실행하고, 영향을 받은 행의 수를 반환합니다.
                //주로 INSERT, UPDATE 또는 DELETE와 같은 데이터 변경 작업에 사용됩니다.
                //예를 들어, INSERT INTO, UPDATE, DELETE FROM와 같은 SQL 문을 실행할 때
                //ExecuteNonQuery() 메서드를 사용하여 데이터베이스에 변경을 가합니다.
                //그런 다음 res 변수에는 실행된 SQL 쿼리에 의해 변경된 행의 수가 포함됩니다.
                if (res > 0)
                {
                    //MessageBox.Show("삭제 성공");
                    await this._dialogCoordinator.ShowMessageAsync(this, "삭제성공!", "삭제");

                }
                else
                {
                    await this._dialogCoordinator.ShowMessageAsync(this, "삭제 실패!", "삭제");

                    //MessageBox.Show("삭제 실패");
                }
                GetEmployees();
                NewEmployee(); // 모든 입력 컨트롤 초기화

            }
        }

        public void NewEmployee()
        {
            Id = 0;
            Salary = 0;
            EmpName = DeptName = Addr = "";
        }
    }
}
