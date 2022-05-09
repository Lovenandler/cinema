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
    /// <summary>
    /// Логика взаимодействия для search.xaml
    /// </summary>
    public partial class search : Page
    {
        public search()
        {
            InitializeComponent();
            
        }
        
            
        private void FilmsSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (searchText.Text.Length == 0)
            {
                MessageBox.Show("Введите название фильма");
            } else { 
            SqlConnection conSearch = new SqlConnection(@"Data Source=LOVENANDLER\SQLEXPRESS;Initial Catalog=Cinema;Integrated Security=True");
            conSearch.Open();
            int i = 0;
            SqlCommand checkSearch = new SqlCommand("Select Cover, Name_Film, Age_Restrictions, Year(Year_Of_Release) as year, Description_Film from Films where Name_Film ='" + searchText.Text+"'", conSearch);
            checkSearch.ExecuteNonQuery();
            DataTable SearchDT = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(checkSearch);
            adapter.Fill(SearchDT);
            FilmsSearch.ItemsSource = SearchDT.DefaultView;
            i = Convert.ToInt32(SearchDT.Rows.Count.ToString());
            if (i == 0)
            {
                MessageBox.Show("Фильм в списке отсутствует");
            }
            /*
            else
            {
                string searchFilms = "SELECT Name_Film, Description_Film,Cover FROM Films"; // Из какой таблицы нужен вывод 
                SqlCommand CommandSearchGrid = new SqlCommand(searchFilms, conSearch);
                CommandSearchGrid.ExecuteNonQuery();

                SqlDataAdapter adSearch = new SqlDataAdapter(CommandSearchGrid);
                DataTable tableSearchFilms = new DataTable("Films");
                adSearch.Fill(tableSearchFilms);
                FilmsSearch.ItemsSource = tableSearchFilms.DefaultView;
                adSearch.Update(tableSearchFilms);

            }
            */
            conSearch.Close();
            }
        }

        

        private void DataGridRow_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var row = e.Source as DataGridRow;

                FilmDetails filmDetails = new FilmDetails(row.Item);
                filmDetails.Show();

            }
        }
    }
}
