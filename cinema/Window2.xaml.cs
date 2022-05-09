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
using System.Configuration;

namespace cinema
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
      

        public Window2()
        {
            InitializeComponent();

        }
        

        private void ListboxCinemas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void ListboxCinemas_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Show();
            this.Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ChooseFilm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            confirmAdmin confirmAdmin = new confirmAdmin();
            confirmAdmin.Show();
            this.Close();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            
            this.frameFilms.Source = new Uri("searchFilms.xaml", UriKind.Relative);

        }

        private void frameFilms_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }

        private void Films_Click(object sender, RoutedEventArgs e)
        {
            this.frameFilms.Source = new Uri("films.xaml", UriKind.Relative);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.frameFilms.Source = new Uri("schedule.xaml", UriKind.Relative);
        }


        private void Ticket_Click(object sender, RoutedEventArgs e)
        {
            chooseseat chooseseat = new chooseseat();
            chooseseat.Show();
            this.Close();
        }
    }
}
