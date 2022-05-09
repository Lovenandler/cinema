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
using System.Data.SqlClient;
using System.Data;

namespace cinema
{
    
    public partial class Window1 : Window
    {
        string IDRole;
        public Window1()
        {
            InitializeComponent();
            
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            authorization authorization = new authorization();
            authorization.Show();
            this.Close();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Login1.Text))
            {
                string[] dataLogin = Login1.Text.Split('@');
                if (dataLogin.Length == 2)
                {
                    string[] data2Login = dataLogin[1].Split('.');
                    if (data2Login.Length == 2)
                    {
                        if (!string.IsNullOrEmpty(PasswordEnter.Password) && PasswordEnter.Password.Length >= 6)
                        {

                            bool en = true;
                            bool symbol = false;
                            bool number = false;
                            bool letter = false;

                            for (int i = 0; i < PasswordEnter.Password.Length; i++) // перебираем символы
                            {
                                if (PasswordEnter.Password[i] >= 'А' && PasswordEnter.Password[i] <= 'Я' || PasswordEnter.Password[i] >= 'а' && PasswordEnter.Password[i] <= 'я') en = false;
                                if (PasswordEnter.Password[i] >= '0' && PasswordEnter.Password[i] <= '9') number = true;
                                if (PasswordEnter.Password[i] >= 'A' && PasswordEnter.Password[i] <= 'Z' || PasswordEnter.Password[i] >= 'a' && PasswordEnter.Password[i] <= 'z') letter = true;
                                if (PasswordEnter.Password[i] == '$' || PasswordEnter.Password[i] == '#' || PasswordEnter.Password[i] == '!') symbol = true;
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
                                SqlCommand checkLog = new SqlCommand("Select * from Users where Email ='" + Login1.Text + "' and Password='" + PasswordEnter.Password + "'", connectionLog);
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
                                    Search_IDRole.Parameters.AddWithValue("@Email", Login1.Text);
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
                                        Window2 window2 = new Window2();
                                        window2.Show();

                                        this.Close();
                                    }
                                    
                                        
                                    
                                }

                                connectionLog.Close();
                            }
                        }
                        else if (PasswordEnter.Password.Length == 0)
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

        private void Login1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void eyes_PreviewMouseDown(object sender, MouseButtonEventArgs e) => ShowPassword();
        private void eyes_PreviewMouseUp(object sender, MouseButtonEventArgs e) => HidePassword();
        private void eyes_MouseEnter(object sender, MouseEventArgs e) => HidePassword();

        private void ShowPassword()
        {
            PasswordUnmask.Visibility = Visibility.Visible;
            PasswordEnter.Visibility = Visibility.Hidden;
            PasswordUnmask.Text = PasswordEnter.Password;
        }

        private void HidePassword()
        {
            PasswordUnmask.Visibility = Visibility.Hidden;
            PasswordEnter.Visibility = Visibility.Visible;
            PasswordEnter.Focus();
        }
        private void PasswordEnter_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordEnter.Password.Length > 0)
            {
                eyes.Visibility = Visibility.Visible;
            }
            else
                eyes.Visibility = Visibility.Hidden;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Login1.Clear();
            PasswordEnter.Clear();
        }
    }

        
       
            
        
    
}
