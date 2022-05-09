using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;

namespace cinema
{
    /// <summary>
    /// Логика взаимодействия для confirmAdmin.xaml
    /// </summary>
    public partial class confirmAdmin : Window
    {
        string IDRole;
        public confirmAdmin()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(LoginAdmin.Text))
            {
                string[] dataLogin = LoginAdmin.Text.Split('@');
                if (dataLogin.Length == 2)
                {
                    string[] data2Login = dataLogin[1].Split('.');
                    if (data2Login.Length == 2)
                    {
                        if (!string.IsNullOrEmpty(PasswordAdmin.Text) && PasswordAdmin.Text.Length >= 6)
                        {

                            bool en = true;
                            bool symbol = false;
                            bool number = false;
                            bool letter = false;

                            for (int i = 0; i < PasswordAdmin.Text.Length; i++) // перебираем символы
                            {
                                if (PasswordAdmin.Text[i] >= 'А' && PasswordAdmin.Text[i] <= 'Я' || PasswordAdmin.Text[i] >= 'а' && PasswordAdmin.Text[i] <= 'я') en = false;
                                if (PasswordAdmin.Text[i] >= '0' && PasswordAdmin.Text[i] <= '9') number = true;
                                if (PasswordAdmin.Text[i] >= 'A' && PasswordAdmin.Text[i] <= 'Z' || PasswordAdmin.Text[i] >= 'a' && PasswordAdmin.Text[i] <= 'z') letter = true;
                                if (PasswordAdmin.Text[i] == '$' || PasswordAdmin.Text[i] == '#' || PasswordAdmin.Text[i] == '!') symbol = true;
                            }

                            if (!en)
                                MessageBox.Show("Доступна только английская раскладка");
                            else if (!symbol)
                                MessageBox.Show("Добавьте один из следующих символов: $ # !");
                            else if (!number)
                                MessageBox.Show("Добавьте хотя бы одну цифру");
                            else if (!letter)
                                MessageBox.Show("Добавьте хотя бы одну букву");
                            if (en && symbol && number && letter)
                            {
                                SqlConnection connectionLog = new SqlConnection(@"Data Source=LOVENANDLER\SQLEXPRESS;Initial Catalog=Cinema;Integrated Security=True");
                                connectionLog.Open();
                                int i = 0;
                                SqlCommand checkLog = new SqlCommand("Select * from Users where Email ='" + LoginAdmin.Text + "' and Password='" + PasswordAdmin.Text + "'", connectionLog);
                                checkLog.ExecuteNonQuery();
                                DataTable dataTable = new DataTable();
                                SqlDataAdapter adapter = new SqlDataAdapter(checkLog);
                                adapter.Fill(dataTable);
                                i = Convert.ToInt32(dataTable.Rows.Count.ToString());
                                if (i == 0)
                                {
                                    MessageBox.Show("Неправильный логин или пароль");
                                }
                                else
                                {
                                    SqlCommand Search_IDRole = new SqlCommand("Select ID_role from Users where Email = @Email", connectionLog);
                                    Search_IDRole.Parameters.AddWithValue("@Email", LoginAdmin.Text);
                                    SqlDataReader sqlDataReaderCountry = Search_IDRole.ExecuteReader();
                                    while (sqlDataReaderCountry.Read())
                                    {

                                        IDRole = sqlDataReaderCountry.GetValue(0).ToString();

                                    }
                                    sqlDataReaderCountry.Close();
                                    if (IDRole == "1")
                                    {

                                        adminWindow adminWindow = new adminWindow();
                                        adminWindow.Show();
                                        this.Close();
                                    }
                                    else if (IDRole == "2")
                                    {
                                        MessageBox.Show("Вы не являетесь администратором");
                                        Window2 window2 = new Window2();
                                        window2.Show();

                                        this.Close();
                                    }



                                }

                                connectionLog.Close();
                            }
                        }
                        else if (PasswordAdmin.Text.Length == 0)
                        {
                            MessageBox.Show("Укажите пароль");
                        }
                        else
                            MessageBox.Show("пароль слишком короткий, минимум 6 символов");


                    }
                    else MessageBox.Show("Укажите логин в форме название@почта.com");
                }
                else MessageBox.Show("Укажите логин в форме название@почта.com");
            }
            else MessageBox.Show("Укажите логин");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window2 window2 = new Window2();
            window2.Show();
            this.Close();
        }
    }
}
