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
    /// Логика взаимодействия для addactor.xaml
    /// </summary>
    public partial class addactor : Window
    {
        public addactor()
        {
            InitializeComponent();
        }

        private void AddNewActor_Click(object sender, RoutedEventArgs e)
        {
            if (InsertActorName.Text.Length > 0)
            {
                if (InsertActorSurname.Text.Length > 0)
                {
                    SqlConnection con = new SqlConnection(@"Data Source=LOVENANDLER\SQLEXPRESS;Initial Catalog=Cinema;Integrated Security=True");

                    con.Open();
                    int i = 0;
                    SqlCommand checkActor = new SqlCommand("Select * from Actors where Name_actor ='" + InsertActorName.Text + "' and Surname_actor='" + InsertActorSurname.Text + "'", con);
                    checkActor.ExecuteNonQuery();
                    DataTable dataTableActor = new DataTable();
                    SqlDataAdapter adapterActor = new SqlDataAdapter(checkActor);
                    adapterActor.Fill(dataTableActor);
                    i = Convert.ToInt32(dataTableActor.Rows.Count.ToString());
                    if (i == 0)
                    {
                      
                        SqlCommand sql_cmnd = new SqlCommand("addActor", con);
                        sql_cmnd.CommandType = CommandType.StoredProcedure;
                        sql_cmnd.Parameters.AddWithValue("@Name_actor", SqlDbType.NVarChar).Value = InsertActorName.Text;
                        sql_cmnd.Parameters.AddWithValue("@Surname_actor", SqlDbType.NVarChar).Value = InsertActorSurname.Text;
                        int rowsAffected = sql_cmnd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Актер успешно добавлен");
                        }
                        else
                        {
                            MessageBox.Show("Ошибка добавления");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Такой актер уже зарегистрирован");
                    }



                    con.Close();
                }
                else
                {
                    MessageBox.Show("Введите фамилию актера");
                }
            }
            else
            {
                MessageBox.Show("Введите имя актера");
            }

        }
    
    }
}
