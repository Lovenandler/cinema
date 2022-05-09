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
    /// Логика взаимодействия для AddProducer.xaml
    /// </summary>
    public partial class AddProducer : Window
    {
        public AddProducer()
        {
            InitializeComponent();
        }

        private void InsertProducerName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AddNewProducer_Click(object sender, RoutedEventArgs e)
        {
            if (InsertProducerName.Text.Length > 0) 
            { 
                if (InsertProducerSurname.Text.Length > 0) { 
                SqlConnection con = new SqlConnection(@"Data Source=LOVENANDLER\SQLEXPRESS;Initial Catalog=Cinema;Integrated Security=True");

                con.Open();
                int i = 0;
                SqlCommand checkLogAuth = new SqlCommand("Select * from Producers where Name_producer ='" + InsertProducerName.Text + "' and Surname_producer='" + InsertProducerSurname.Text + "'", con);
                checkLogAuth.ExecuteNonQuery();
                DataTable dataTableAuth = new DataTable();
                SqlDataAdapter adapterAuth = new SqlDataAdapter(checkLogAuth);
                adapterAuth.Fill(dataTableAuth);
                i = Convert.ToInt32(dataTableAuth.Rows.Count.ToString());
                if (i == 0)
                {

                        SqlCommand sql_cmnd = new SqlCommand("AddProducer", con);
                        sql_cmnd.CommandType = CommandType.StoredProcedure;
                        sql_cmnd.Parameters.AddWithValue("@Name_producer", SqlDbType.NVarChar).Value = InsertProducerName.Text;
                        sql_cmnd.Parameters.AddWithValue("@Surname_producer", SqlDbType.NVarChar).Value = InsertProducerSurname.Text;
                        

                        int rowsAffected = sql_cmnd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Продюсер успешно добавлен");
                }
                else
                {
                    MessageBox.Show("Ошибка добавления");
                }
            }
            else
            {
                MessageBox.Show("Такой режиссер уже зарегистрирован");
            }
                


            con.Close();
                } else
                {
                    MessageBox.Show("Введите фамилию режиссера");
                }
            } else
            {
                MessageBox.Show("Введите имя режиссера");
            }
            
        } 
    }
}
