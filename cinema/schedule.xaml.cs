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
    /// Логика взаимодействия для schedule.xaml
    /// </summary>
    public partial class schedule : Page
    {
        public static string filmTitle { get; set; }
        public static string cinemaTitle { get; set; }
        public static string date { get; set; }
        public static string time { get; set; }
        public schedule()
        {
            InitializeComponent();
            scheduleFill();
        }
        private void scheduleFill()
        {
            SqlConnection conSearch = new SqlConnection(@"Data Source=LOVENANDLER\SQLEXPRESS;Initial Catalog=Cinema;Integrated Security=True");
            conSearch.Open();
            int i = 0;
            SqlCommand checkSearch = new SqlCommand("SELECT dbo.Films.Name_Film, dbo.Cinemas.Name_cinema, dbo.Schedule.Date_FilmSession, dbo.Schedule.Time_FilmSession FROM dbo.Schedule INNER JOIN dbo.Films ON dbo.Schedule.ID_Film = dbo.Films.ID_Film INNER JOIN dbo.Cinemas ON dbo.Schedule.ID_cinema = dbo.Cinemas.ID_cinema", conSearch);
            checkSearch.ExecuteNonQuery();
            DataTable SearchDT = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(checkSearch);
            adapter.Fill(SearchDT);
            scheduleList.ItemsSource = SearchDT.DefaultView;
            i = Convert.ToInt32(SearchDT.Rows.Count.ToString());
            if (i == 0)
            {
                MessageBox.Show("Расписание отсутствует");
            }

        }
        private void scheduleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dynamic scheduleItem = scheduleList.SelectedItem;
            if (scheduleItem != null)
            {
                filmTitle = scheduleItem[0];
                cinemaTitle = scheduleItem[1];
                string dateInfo = scheduleItem[2].ToString();
                string [] dateHelper = dateInfo.Split(' ');
                date = dateHelper[0];
                time = scheduleItem[3].ToString();
                chooseseat chooseseat = new chooseseat();
                var parentWin = Window.GetWindow(this);
                chooseseat.Show();
                parentWin.Close();
            }
        }
    }
}
