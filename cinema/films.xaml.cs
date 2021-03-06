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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;

namespace cinema
{
    
    public partial class films : Page
    {
        public films()
        {
            InitializeComponent();
            LoadingData();
        }
        private void LoadingData()
        {
            SqlConnection connectionGridFilms = new SqlConnection(@"Data Source=LOVENANDLER\SQLEXPRESS;Initial Catalog=Cinema;Integrated Security=True");
            connectionGridFilms.Open();
            string selectFilms = "SELECT * FROM Films";  
            SqlCommand createCommandGridFilm = new SqlCommand(selectFilms, connectionGridFilms);
            createCommandGridFilm.ExecuteNonQuery();

            SqlDataAdapter adapterFilms = new SqlDataAdapter(createCommandGridFilm);
            DataTable tableFilms = new DataTable("Films");
            adapterFilms.Fill(tableFilms);
            ListFilms.ItemsSource = tableFilms.DefaultView;
            adapterFilms.Update(tableFilms);

            connectionGridFilms.Close();
        }//заполнение фильмов

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void DataGridTemplateColumn_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
               var row = e.Source as DataGridRow;
                var parentWin = Window.GetWindow(this);
                FilmDetails filmDetails = new FilmDetails(row.Item);
                filmDetails.Show();
                parentWin.Close();                
            }
        }
    }
}
