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
using Microsoft.Win32;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;


namespace cinema
{
    /// <summary>
    /// Логика взаимодействия для userPage.xaml
    /// </summary>
    
    public partial class userPage : Window
    {
        
        SqlConnection EU = new SqlConnection(@"Data Source=LOVENANDLER\SQLEXPRESS;Initial Catalog=Cinema;Integrated Security=True");
        string connectionString;
        SqlCommand ChooseUser;
        SqlCommand ChooseRole;
        SqlDataReader UsersEdit;
        SqlDataReader RoleEdit;
        int ID_Role;
        public userPage()
        {
            InitializeComponent();
            EmailFill();
            FillRoles();
        }
        public void EmailFill()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            ChooseUser = new SqlCommand();
            con.Open();
            ChooseUser.Connection = con;
            ChooseUser.CommandText = "Select * from Users";
            UsersEdit = ChooseUser.ExecuteReader();
            while (UsersEdit.Read())
            {
                EmailChange.Items.Add(UsersEdit["Email"]);
                //memory stream for images 
            }
            UsersEdit.Close();
            con.Close();
        }//заполнение логина

        private void ExitButtonUserPage_Click(object sender, RoutedEventArgs e) //выход
        {
            adminWindow adminWindow = new adminWindow();
            adminWindow.Show();
            this.Close();
        }

        public void FillRoles() //заполнение ролей
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            ChooseRole = new SqlCommand();
            con.Open();
            ChooseRole.Connection = con;
            ChooseRole.CommandText = "Select * from Roles";
            RoleEdit = ChooseRole.ExecuteReader();
            while (RoleEdit.Read())
            {
                RoleUser.Items.Add(RoleEdit["Name_Role"]);
            }
            RoleEdit.Close();
            con.Close();
        }

        private void EditUser_Click(object sender, RoutedEventArgs e) //редактирование пользователя
        {
            if (!string.IsNullOrEmpty(PasswordChange.Text) && PasswordChange.Text.Length >= 6)
            {

                bool en = true;
                bool symbol = false;
                bool number = false;
                bool letter = false;

                for (int i = 0; i < PasswordChange.Text.Length; i++) 
                {
                    if (PasswordChange.Text[i] >= 'А' && PasswordChange.Text[i] <= 'Я' || PasswordChange.Text[i] >= 'а' && PasswordChange.Text[i] <= 'я') en = false;
                    if (PasswordChange.Text[i] >= '0' && PasswordChange.Text[i] <= '9') number = true;
                    if (PasswordChange.Text[i] >= 'A' && PasswordChange.Text[i] <= 'Z' || PasswordChange.Text[i] >= 'a' && PasswordChange.Text[i] <= 'z') letter = true;
                    if (PasswordChange.Text[i] == '$' || PasswordChange.Text[i] == '#' || PasswordChange.Text[i] == '!') symbol = true;
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
                    EU.Open();
                    SqlCommand command = new SqlCommand(null, EU);

                    command.CommandText = "UPDATE Users SET Password = @password, ID_role = @role WHERE Email = '" + EmailChange.Text + "'";


                    SqlParameter PasswordParam = new SqlParameter("@password", SqlDbType.NVarChar, 10);
                    SqlParameter RoleParam = new SqlParameter("@role", SqlDbType.Int);
                    PasswordParam.Value = PasswordChange.Text;
                    if(RoleUser.Text == "Администратор")
                    {
                        ID_Role = 1;
                    }else 
                    if (RoleUser.Text == "Клиент")
                     {
                        ID_Role = 2;
                     }
                    RoleParam.Value = ID_Role;

                    command.Parameters.Add(PasswordParam);
                    command.Parameters.Add(RoleParam);
                    command.Prepare();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Данные пользователя изменены");
                    EU.Close();
                }
            }else if (PasswordChange.Text.Length == 0)
            {
                MessageBox.Show("Укажите пароль");
            }
            else
                MessageBox.Show("пароль слишком короткий, минимум 6 символов");

        }
        
        private void EmailChange_DropDownClosed(object sender, EventArgs e) //показ пароля
        {
            EU.Open();
            SqlCommand ShowPassword = new SqlCommand("Select Password from Users where Email = '" + EmailChange.Text + "'", EU);

            SqlDataReader dataReader = ShowPassword.ExecuteReader();
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    string Password = (string)dataReader["Password"].ToString();
                    PasswordChange.Text = Password;


                }
            }


            EU.Close();
        }
    }
}
