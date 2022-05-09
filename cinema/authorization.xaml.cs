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
using System.Configuration;

namespace cinema
{


    public partial class authorization : Window
    {
        string connectionString;
        SqlCommand chooseRole;
        SqlDataReader Roles;
        public authorization()
        {
            InitializeComponent();
            
        }
        
        private void Button_Click(object sender, RoutedEventArgs e) //кнопка регистрации
        {
            if (!string.IsNullOrEmpty(LoginAuth.Text))
            {
                string[] dataLogin = LoginAuth.Text.Split('@');
                if (dataLogin.Length == 2)
                {
                    string[] data2Login = dataLogin[1].Split('.');
                    if (data2Login.Length == 2)
                    {

                        if (!string.IsNullOrEmpty(PasswordAuth.Text) && PasswordAuth.Text.Length >= 6)
                        {

                            bool en = true;
                            bool symbol = false;
                            bool number = false;
                            bool letter = false;

                            for (int i = 0; i < PasswordAuth.Text.Length; i++) 
                            {
                                if (PasswordAuth.Text[i] >= 'А' && PasswordAuth.Text[i] <= 'Я' || PasswordAuth.Text[i] >= 'а' && PasswordAuth.Text[i] <= 'я') en = false;
                                if (PasswordAuth.Text[i] >= '0' && PasswordAuth.Text[i] <= '9') number = true;
                                if (PasswordAuth.Text[i] >= 'A' && PasswordAuth.Text[i] <= 'Z' || PasswordAuth.Text[i] >= 'a' && PasswordAuth.Text[i] <= 'z') letter = true;
                                if (PasswordAuth.Text[i] == '$' || PasswordAuth.Text[i] == '#' || PasswordAuth.Text[i] == '!') symbol = true;
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
                                SqlConnection con = new SqlConnection(@"Data Source=LOVENANDLER\SQLEXPRESS;Initial Catalog=Cinema;Integrated Security=True");
                                
                                con.Open();
                                int i = 0;
                                SqlCommand checkLogAuth = new SqlCommand("Select * from Users where Email ='" + LoginAuth.Text + "' and Password='" + PasswordAuth.Text + "'", con);
                                checkLogAuth.ExecuteNonQuery();
                                DataTable dataTableAuth = new DataTable();
                                SqlDataAdapter adapterAuth = new SqlDataAdapter(checkLogAuth);
                                adapterAuth.Fill(dataTableAuth);
                                i = Convert.ToInt32(dataTableAuth.Rows.Count.ToString());
                                if (i == 0)
                                {
                                    SqlCommand command = new SqlCommand(null, con); //вставка в таблицу

                                    command.CommandText =
                            "INSERT INTO Users (Email, Password, ID_role)" +
                            "VALUES (@Email, @Password,  @ID_role)";
                                    SqlParameter EmailParam = new SqlParameter("@Email", SqlDbType.NVarChar, 64);
                                    SqlParameter PasswordParam =
                                        new SqlParameter("@Password", SqlDbType.NVarChar, 10);
                                    SqlParameter RoleParam = new SqlParameter("@ID_role", SqlDbType.Int);

                                    EmailParam.Value = LoginAuth.Text;
                                    PasswordParam.Value = PasswordAuth.Text;
                                    RoleParam.Value = 2;
                                    command.Parameters.Add(EmailParam);
                                    command.Parameters.Add(PasswordParam);
                                    command.Parameters.Add(RoleParam);

                                    command.Prepare();

                                    command.ExecuteNonQuery();


                                    if (pressed.IsChecked == true)
                                    {
                                        Window2 window2 = new Window2();
                                        window2.Show();

                                    }
                                    else
                                    {
                                        Window1 window1 = new Window1();
                                        window1.Show();

                                    }
                                    this.Close();
                                    
                                }
                                else if (i != 0)
                                {

                                    MessageBox.Show("Пользователь уже зарегистрирован");

                                }
                            }
                        }
                            else MessageBox.Show("пароль слишком короткий, минимум 6 символов");
                    }
                        else MessageBox.Show("Укажите логин в форме название@почта.com");
                }
                    else MessageBox.Show("Укажите логин в форме название@почта.com");
            }
                else MessageBox.Show("Укажите логин");

        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void Button_Click_1(object sender, RoutedEventArgs e) //кнопка возврата к окну входа
        {

            Window1 window1 = new Window1();
            window1.Show();
            this.Close();
        }
        private void Role_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
        private void pressed_MouseEnter(object sender, MouseEventArgs e)
        {
            pressed.IsChecked = false;
        }
    }
}

