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
    /// Логика взаимодействия для FilmDetailsEdit.xaml
    /// </summary>
    public partial class FilmDetailsEdit : Window
    {
        OpenFileDialog dlg = new OpenFileDialog();
        FileStream fs;
        SqlConnection EU = new SqlConnection(@"Data Source=LOVENANDLER\SQLEXPRESS;Initial Catalog=Cinema;Integrated Security=True");
        string connectionString;
        SqlCommand ChooseUser;
        SqlDataReader UsersEdit;
        byte[] data;
        ImageSourceConverter imgs;
        public byte[] UserIcon { get; set; }
        public FilmDetailsEdit(object user1)
        {
            InitializeComponent();
            DataContext = user1;
        }
        public FilmDetailsEdit()
        {
            InitializeComponent();
        }

        private void EditFilm_Click(object sender, RoutedEventArgs e)
        {
            string[] DataAge = AgeFilm.Text.Split(' ');
            SqlConnection FilmData = new SqlConnection(@"Data Source=LOVENANDLER\SQLEXPRESS;Initial Catalog=Cinema;Integrated Security=True");
            FilmData.Open();
            SqlCommand command = new SqlCommand("Update Films Set Name_Film = @Name, Description_Film = @Description, Age_restrictions = @Age where ID_Film='"+IDFilmTextBlock.Text+"'", FilmData);
            SqlParameter NameParam = new SqlParameter("@Name", SqlDbType.NVarChar, 120);
            SqlParameter DescriptionParam = new SqlParameter("@Description", SqlDbType.NVarChar, 400);
            SqlParameter AgeParam = new SqlParameter("@Age", SqlDbType.Int);
            NameParam.Value = FilmName.Text;
            DescriptionParam.Value = DescriptionFilmTextBox.Text;
            AgeParam.Value = DataAge[0];
            command.Parameters.Add(NameParam);
            command.Parameters.Add(DescriptionParam);
            command.Parameters.Add(AgeParam);

            command.Prepare();

            
            SqlCommand comimg = new SqlCommand(null, FilmData); 
            comimg.CommandText =
    "UPDATE Films SET Cover = @Cover WHERE ID_Film = '" + IDFilmTextBlock.Text + "'";
            comimg.Parameters.AddWithValue("@Cover", data);
            command.ExecuteNonQuery();
            comimg.ExecuteNonQuery();
            FilmData.Close();
            MessageBox.Show("Изменения сохранены");

        }//редактирование фильма
        private void deleteFilm()
        {
            SqlConnection connectionDel = new SqlConnection(@"Data Source=LOVENANDLER\SQLEXPRESS;Initial Catalog=Cinema;Integrated Security=True");
            connectionDel.Open();
            SqlCommand sqlCommandFilm = new SqlCommand("Delete From Films Where ID_Film='"+IDFilmTextBlock.Text+"'", connectionDel);
            SqlCommand sqlCommandCountry = new SqlCommand("Delete From Country_Films Where ID_Film='" + IDFilmTextBlock.Text + "'", connectionDel);
            SqlCommand sqlCommandProducer = new SqlCommand("Delete From Films_Producers Where ID_Film='" + IDFilmTextBlock.Text + "'", connectionDel);
            SqlCommand sqlCommandGenre = new SqlCommand("Delete From Genres_Films Where ID_Film='" + IDFilmTextBlock.Text + "'", connectionDel);
            SqlCommand sqlCommandLanguage = new SqlCommand("Delete From Languages_Films Where ID_Film='" + IDFilmTextBlock.Text + "'", connectionDel);
            SqlCommand sqlCommandSchedule = new SqlCommand("Delete From Schedule Where ID_Film='" + IDFilmTextBlock.Text + "'", connectionDel);
            SqlCommand sqlCommandActor = new SqlCommand("Delete From Actors_Films Where ID_Film='" + IDFilmTextBlock.Text + "'", connectionDel);
            sqlCommandActor.ExecuteNonQuery();
            sqlCommandGenre.ExecuteNonQuery();
            sqlCommandLanguage.ExecuteNonQuery();
            sqlCommandProducer.ExecuteNonQuery();
            sqlCommandCountry.ExecuteNonQuery();
            sqlCommandFilm.ExecuteNonQuery();
            sqlCommandSchedule.ExecuteNonQuery();
            connectionDel.Close();

            MessageBox.Show("Фильм удален");
        }//удаление фильма
        private void DelFilm_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить фильм?", "Удалить", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    deleteFilm();
                    break;
                case MessageBoxResult.No:
                    this.Close();
                    break;
            }

        }//подтверждение удаления

        private void ReturnFilm_Click(object sender, RoutedEventArgs e)
        {
            editFilm editFilm = new editFilm();
            editFilm.Show();
            this.Close();
        }

        private void ChangeImgFilm_Click(object sender, RoutedEventArgs e)
        {
            dlg.ShowDialog();

            fs = new FileStream(dlg.FileName, FileMode.Open,
            FileAccess.Read);

            data = new byte[fs.Length];
            fs.Read(data, 0, System.Convert.ToInt32(fs.Length));

            fs.Close();

            imgs = new ImageSourceConverter();
            CoverChange.SetValue(Image.SourceProperty, imgs.ConvertFromString(dlg.FileName.ToString()));
        }
    }
}
