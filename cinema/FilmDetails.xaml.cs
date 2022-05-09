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

namespace cinema
{
    /// <summary>
    /// Логика взаимодействия для FilmDetails.xaml
    /// </summary>
    public partial class FilmDetails : Window
    {
        public FilmDetails(object user)
        { 
            InitializeComponent();
            DataContext = user;

        }

        private void BookFilm_Click(object sender, RoutedEventArgs e)
        {
            chooseseat chooseseat = new chooseseat();
            chooseseat.Show();
            this.Close();
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            Window2 window2 = new Window2();
            window2.Show();
            this.Close();
           
        }
    }
}
