using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using MySqlX.XDevAPI.CRUD;
using sakila.Models;
using System;
using System.Data;
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

namespace sakila
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

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            List<SqlText> languages = new List<SqlText>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    // SQL 쿼리 실행
                    MySqlCommand cmd = new MySqlCommand(Models.SqlText.selectQuery, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataSet dSet = new DataSet();
                    adapter.Fill(dSet, "language");

                    foreach (DataRow row in dSet.Tables["language"].Rows)
                    {
                        var languageData = new SqlText()
                        {
                            language_id = Convert.ToInt32(row["language_id"]), // 필드명과 변수명이 일치해야 함
                            name = Convert.ToString(row["name"]),
                            last_update = Convert.ToDateTime(row["last_update"]),
                        };

                        languages.Add(languageData);
                    }

                    // DataGrid에 데이터 바인딩
                    GrdResults.ItemsSource = languages;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {


            try
            {
                using (MySqlConnection conn = new MySqlConnection(Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    if (GrdResults.SelectedItems.Count > 0)
                    {
                        foreach (SqlText item in GrdResults.SelectedItems)
                        {
                            try
                            {
                              

                                using (MySqlCommand cmd = new MySqlCommand(Models.SqlText.deleteQuery, conn))
                                {
                                    cmd.Parameters.AddWithValue("@language_id", item.language_id);
                                    cmd.ExecuteNonQuery(); // 쿼리 실행
                                }
                            }
                            catch (Exception ex)
                            {
                                
                                MessageBox.Show($"삭제 작업 중 오류가 발생했습니다: {ex.Message}");
                                
                            }
                        }

                        MessageBox.Show("선택된 항목이 삭제되었습니다.");
                    }
                    else
                    {
                        // 선택된 항목이 없는 경우 처리
                        MessageBox.Show("선택된 항목이 없습니다.");
                    }
                }
            }
            catch (Exception ex)
            {
                // 데이터베이스 연결 문제 또는 기타 예외 처리
                MessageBox.Show("오류 발생: " + ex.Message);
            }
        }

        private void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    var insRes = 0;

                    MySqlCommand cmd = new MySqlCommand(Models.SqlText.insertQuery, conn);
                    MySqlParameter prmProduct = new MySqlParameter("@name", TxtName.Text);
                    cmd.Parameters.AddWithValue("@name", TxtName.Text);
                    insRes += cmd.ExecuteNonQuery();

                    if (insRes > 0)
                    {
                        MessageBox.Show("새로운 데이터가 저장되었습니다.");
                    }
                    else
                    {
                        MessageBox.Show("저장을 실패했습니다.");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {


            try
            {
                using (MySqlConnection conn = new MySqlConnection(Helpers.Common.CONNSTRING))
                {
                    conn.Open();


                    using (MySqlCommand cmd = new MySqlCommand(Models.SqlText.updateQuery, conn))
                    {
                        // 파라미터 추가
                        cmd.Parameters.AddWithValue("@name", TxtName.Text);
                        cmd.Parameters.AddWithValue("@language_id", TxtId.Text);

                        // 쿼리 실행
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("정보가 수정되었습니다.");
                        }
                        else
                        {
                            MessageBox.Show("수정할 항목을 찾을 수 없습니다.");
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}