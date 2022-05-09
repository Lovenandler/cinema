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
    /// Логика взаимодействия для adminWindow.xaml
    /// </summary>
    public partial class adminWindow : Window
    {
        public adminWindow()
        {
            InitializeComponent();
            
        }
        
        private void AddFilm_Click(object sender, RoutedEventArgs e)
        {
            administration administration = new administration();
            administration.Show();
            this.Close();
        }

        private void EditFilm_Click(object sender, RoutedEventArgs e)
        {
            editFilm editFilm = new editFilm();
            editFilm.Show();
            this.Close();
        }

        private void AppFilm_Click(object sender, RoutedEventArgs e)
        {
            Window2 window2 = new Window2();
            window2.Show();
            this.Close();
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            userPage userPage = new userPage();
            userPage.Show();
            this.Close();
        }
    }
}
