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
using Microsoft.Win32;
using System.IO;



namespace cinema
{
    
    public partial class administration : Window
    {
        string IDLanguage;
        string IDGenre;
        string IDCountry;
        string IDProducer;
        string IDFilm;
       
        OpenFileDialog imgaddfilm = new OpenFileDialog();
        ImageSourceConverter imgs = new ImageSourceConverter();
        FileStream nfs;
        byte[] data;
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public administration()
        {
            InitializeComponent();
            ChooseCountryList();
            ChooseProducerList();
            LoadActors();
            LanguageSelect();
            ChooseGenre();

        }
        public void ChooseGenre() //выбор жанра
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand WhichGenre = new SqlCommand("Select * from Genre", con);
            SqlDataReader GenresFilm;
            con.Open();

            GenresFilm = WhichGenre.ExecuteReader();
            while (GenresFilm.Read())
            {
                Genres.Items.Add(GenresFilm["Name_Genre"]);

            }
            GenresFilm.Close();
            con.Close();
        }
        public void ChooseCountryList() //выбор страны
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand whichCountry = new SqlCommand("Select * from Country", con);
            SqlDataReader Countries;
            con.Open();

            Countries = whichCountry.ExecuteReader();
            while (Countries.Read())
            {
                ChooseCountry.Items.Add(Countries["Name_country"]);

            }
            Countries.Close();
            con.Close();
        }
        public void ChooseProducerList() //выбор продюсера
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand whichProducer = new SqlCommand("Select Name_producer+' '+Surname_producer as 'Fullname' from Producers", con);
            whichProducer.ExecuteNonQuery();
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(whichProducer);
            adapter.Fill(table);
            foreach (DataRow row in table.Rows)
            {
                ChooseProducer.Items.Add(row["Fullname"].ToString());

            }
            con.Close();
        }
        private void NewFilmYear_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) //год выпуска фильма
        {


            var s = sender as Slider;
            int value = (Int32)s.Value;
            if (YearChange != null)
            {
                this.YearChange.Text = value.ToString();
            }


        }
        private void Button_Click(object sender, RoutedEventArgs e) //кнопка возврата
        {
            adminWindow adminWindow = new adminWindow();
            adminWindow.Show();
            this.Close();
        }
        private void RatingChange_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) //рейтинг фильма
        {
            var r = sender as Slider;
            int rate = (Int32)r.Value;
            if (RatingTextChange != null)
            {
                this.RatingTextChange.Text = rate.ToString();
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e) //добавление
        {
            if (NewFilmName.Text.Length > 0)
            {
                if (ChooseCountry.SelectedItems.Count > 0)
                {
                    if (NewFilmDescription.Text.Length > 0)
                    {
                        if (NewFilmDuration.Text.Length > 0)
                        {
                            if (Actors.SelectedItems.Count > 0)
                            {
                                if (ChooseProducer.SelectedItems.Count > 0)
                                {
                                    if (YearChange.Text.Length > 0)
                                    {
                                        if (zeroValue.IsChecked == true || sixValue.IsChecked == true || twelveValue.IsChecked == true || sixteenValue.IsChecked == true || eighteenValue.IsChecked == true)
                                        {
                                            if (LanguagesView.SelectedItems.Count > 0)
                                            {
                                                if (SubType.IsChecked == true || VoiceType.IsChecked == true)
                                                {

                                                    if (RatingTextChange.Text.Length > 0)
                                                    {
                                                        SqlConnection con = new SqlConnection(@"Data Source=LOVENANDLER\SQLEXPRESS;Initial Catalog=Cinema;Integrated Security=True");
                                                        int ageRestriction = 0;
                                                        string TypeLang = " ";
                                                        con.Open();
                                                        int i = 0;
                                                        SqlCommand AddFilmName = new SqlCommand("Select * from Films where Name_Film ='" + NewFilmName.Text + "'", con);
                                                        AddFilmName.ExecuteNonQuery();
                                                        DataTable AddFilm = new DataTable();
                                                        SqlDataAdapter adapterFilmName = new SqlDataAdapter(AddFilmName);
                                                        adapterFilmName.Fill(AddFilm);
                                                        i = Convert.ToInt32(AddFilm.Rows.Count.ToString());
                                                        if (i == 0)
                                                        {
                                                            #region Age Values
                                                            if (zeroValue.IsChecked == true)
                                                            {
                                                                ageRestriction = 0;
                                                            }
                                                            else if (sixValue.IsChecked == true)
                                                            {
                                                                ageRestriction = 6;
                                                            }
                                                            else if (twelveValue.IsChecked == true)
                                                            {
                                                                ageRestriction = 12;
                                                            }
                                                            else if (sixteenValue.IsChecked == true)
                                                            {
                                                                ageRestriction = 16;
                                                            }
                                                            else if (eighteenValue.IsChecked == true)
                                                            {
                                                                ageRestriction = 18;
                                                            }
                                                            #endregion

                                                            #region Types Languages
                                                            if (SubType.IsChecked == true)
                                                            {
                                                                TypeLang = "Субтитры";
                                                            }
                                                            else if (VoiceType.IsChecked == true)
                                                            {
                                                                TypeLang = "Озвучка";
                                                            }
                                                            #endregion

                                                            #region Insert NameFilm
                                                            String queryInsert = "INSERT INTO Films (Name_Film, Description_Film, Age_Restrictions, Year_Of_Release, Duration, Rating_Of_The_Film, Cover) VALUES (@Name_Film, @DescriptionFilm, @AgeRestrictions, @YearOfRelease, @Duration, @Rating, @Cover)";
                                                            SqlCommand commandInsert = new SqlCommand(queryInsert, con);
                                                            commandInsert.Parameters.AddWithValue("@Name_Film", NewFilmName.Text);
                                                            commandInsert.Parameters.AddWithValue("@DescriptionFilm", NewFilmDescription.Text);
                                                            commandInsert.Parameters.AddWithValue("@AgeRestrictions", ageRestriction);
                                                            commandInsert.Parameters.AddWithValue("@YearOfRelease", YearChange.Text);
                                                            commandInsert.Parameters.AddWithValue("@Duration", NewFilmDuration.Text);
                                                            commandInsert.Parameters.AddWithValue("@Rating", RatingTextChange.Text);
                                                            commandInsert.Parameters.AddWithValue("@Cover", data);
                                                            #endregion

                                                            int rowsAffectedInsert = commandInsert.ExecuteNonQuery();


                                                            if (rowsAffectedInsert > 0)
                                                            {

                                                                #region Select IDLanguage
                                                                SqlCommand Search_IDLanguage = new SqlCommand("Select ID_Language from Languages where Name_Language = @NameLanguage and Type_Language = @TypeLanguage", con);

                                                                foreach (string language in LanguagesView.SelectedItems)
                                                                {

                                                                    Search_IDLanguage.Parameters.Clear();
                                                                    Search_IDLanguage.Parameters.AddWithValue("@NameLanguage", language.ToString());
                                                                    Search_IDLanguage.Parameters.AddWithValue("@TypeLanguage", TypeLang);
                                                                    SqlDataReader sqlDataReaderLang = Search_IDLanguage.ExecuteReader();
                                                                    while (sqlDataReaderLang.Read())
                                                                    {

                                                                        IDLanguage = sqlDataReaderLang.GetValue(0).ToString();

                                                                    }
                                                                    sqlDataReaderLang.Close();

                                                                    SqlCommand Search_IDFilm = new SqlCommand("Select ID_Film from Films where Name_Film = @NameFilm", con);
                                                                    Search_IDFilm.Parameters.AddWithValue("@NameFilm", NewFilmName.Text);

                                                                    SqlDataReader sqlDataReaderFilm = Search_IDFilm.ExecuteReader();
                                                                    while (sqlDataReaderFilm.Read())
                                                                    {
                                                                        IDFilm = sqlDataReaderFilm.GetValue(0).ToString();

                                                                    }
                                                                    sqlDataReaderFilm.Close();

                                                                    String languageQuery = "INSERT INTO Languages_Films (ID_Film, ID_Language) VALUES (@ID_Film, @ID_Language)";
                                                                    SqlCommand languageCommand = new SqlCommand(languageQuery, con);
                                                                    languageCommand.Parameters.AddWithValue("@ID_Film", IDFilm);
                                                                    languageCommand.Parameters.AddWithValue("@ID_Language", IDLanguage);
                                                                    languageCommand.ExecuteNonQuery();

                                                                }
                                                                #endregion

                                                                #region Select IDCountry
                                                                SqlCommand Search_IDCountry = new SqlCommand("Select ID_Country from Country where Name_country = @NameCountry", con);

                                                                foreach (string country in ChooseCountry.SelectedItems)
                                                                {
                                                                    Search_IDCountry.Parameters.Clear();
                                                                    Search_IDCountry.Parameters.AddWithValue("@NameCountry", country.ToString());
                                                                    SqlDataReader sqlDataReaderCountry = Search_IDCountry.ExecuteReader();
                                                                    while (sqlDataReaderCountry.Read())
                                                                    {

                                                                        IDCountry = sqlDataReaderCountry.GetValue(0).ToString();

                                                                    }
                                                                    sqlDataReaderCountry.Close();

                                                                    String countryQuery = "INSERT INTO Country_Films (ID_Film, ID_Country) VALUES (@ID_Film, @ID_Country)";
                                                                    SqlCommand countryCommand = new SqlCommand(countryQuery, con);
                                                                    countryCommand.Parameters.AddWithValue("@ID_Film", IDFilm);
                                                                    countryCommand.Parameters.AddWithValue("@ID_Country", IDCountry);
                                                                    countryCommand.ExecuteNonQuery();
                                                                }
                                                                #endregion

                                                                #region Select IDGenre
                                                                SqlCommand Search_IDGenre = new SqlCommand("Select ID_Genre from Genre where Name_Genre = @NameGenre", con);

                                                                foreach (string genre in Genres.SelectedItems)
                                                                {
                                                                    Search_IDGenre.Parameters.Clear();
                                                                    Search_IDGenre.Parameters.AddWithValue("@NameGenre", genre.ToString());
                                                                    SqlDataReader sqlDataReaderGenre = Search_IDGenre.ExecuteReader();
                                                                    while (sqlDataReaderGenre.Read())
                                                                    {

                                                                        IDGenre = sqlDataReaderGenre.GetValue(0).ToString();

                                                                    }
                                                                    sqlDataReaderGenre.Close();

                                                                    String genreQuery = "INSERT INTO Genres_Films (ID_Film, ID_Genre) VALUES (@ID_Film, @ID_Genre)";
                                                                    SqlCommand genreCommand = new SqlCommand(genreQuery, con);
                                                                    genreCommand.Parameters.AddWithValue("@ID_Film", IDFilm);
                                                                    genreCommand.Parameters.AddWithValue("@ID_Genre", IDGenre);
                                                                    genreCommand.ExecuteNonQuery();

                                                                }
                                                                #endregion

                                                                #region Select IDProducer
                                                                SqlCommand Search_IDProducer = new SqlCommand("Select ID_Producer from Producers where Name_producer = @NameProducer", con);
                                                                foreach (string producer in ChooseProducer.SelectedItems)
                                                                {
                                                                    string[] ProducerName = producer.Split(' ');
                                                                    Search_IDProducer.Parameters.Clear();
                                                                    Search_IDProducer.Parameters.AddWithValue("@NameProducer", ProducerName[0]);
                                                                    SqlDataReader sqlDataReaderProducer = Search_IDProducer.ExecuteReader();
                                                                    while (sqlDataReaderProducer.Read())
                                                                    {

                                                                        IDProducer = sqlDataReaderProducer.GetValue(0).ToString();

                                                                    }
                                                                    sqlDataReaderProducer.Close();
                                                                    String producerQuery = "INSERT INTO Films_Producers (ID_Film, ID_Producer) VALUES (@ID_Film, @ID_Producer)";
                                                                    SqlCommand producerCommand = new SqlCommand(producerQuery, con);
                                                                    producerCommand.Parameters.AddWithValue("@ID_Film", IDFilm);
                                                                    producerCommand.Parameters.AddWithValue("@ID_Producer", IDProducer);
                                                                    producerCommand.ExecuteNonQuery();
                                                                }
                                                                #endregion


                                                            }
                                                            else MessageBox.Show("Ошибка добавления названия фильма");

                                                        }
                                                        else if (i != 0)
                                                        {

                                                            MessageBox.Show("Такой фильм уже есть в базе");

                                                        }
                                                        con.Close();
                                                    }
                                                    else MessageBox.Show("Добавьте рейтинг к фильму");
                                                }
                                                else MessageBox.Show("Выберите озвучку или субтитры как параметр");
                                            }
                                            else MessageBox.Show("Добавьте язык фильма");
                                        }
                                        else MessageBox.Show("Добавьте возрастные ограничения к фильму");
                                    }
                                    else MessageBox.Show("Добавьте год выпуска фильма");
                                }
                                else MessageBox.Show("Добавьте продюссера фильма");
                            }
                            else MessageBox.Show("Добавьте актеров фильма");
                        }
                        else MessageBox.Show("Введите длительность фильма");
                    }
                    else MessageBox.Show("Заполните описание к фильму");
                }
                else MessageBox.Show("Укажите страну производителя");

            }
            else MessageBox.Show("Введите название фильма");

        }
        public void LoadActors() //загрузка актеров
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand whichActor = new SqlCommand("Select Name_actor+' '+Surname_actor as 'Fullname_actor' from Actors", con);
            whichActor.ExecuteNonQuery();
            DataTable tableAct = new DataTable();
            SqlDataAdapter adapterAct = new SqlDataAdapter(whichActor);
            adapterAct.Fill(tableAct);
            foreach (DataRow row in tableAct.Rows)
            {
                Actors.Items.Add(row["Fullname_actor"].ToString());

            }
            con.Close();
        }
        private void LoadAddImg_Click(object sender, RoutedEventArgs e) //загрузка обложки фильма
        {
            imgaddfilm.ShowDialog();

            nfs = new FileStream(imgaddfilm.FileName, FileMode.Open,
            FileAccess.Read);

            data = new byte[nfs.Length];
            nfs.Read(data, 0, System.Convert.ToInt32(nfs.Length));

            nfs.Close();
            FilmAddImg.SetValue(Image.SourceProperty, imgs.ConvertFromString(imgaddfilm.FileName.ToString()));

        }
        public void LanguageSelect() //загрузка языков
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand whichLang = new SqlCommand("Select Distinct Name_Language as 'LanguageCombo' from Languages", con);
            whichLang.ExecuteNonQuery();
            DataTable tableLang = new DataTable();
            SqlDataAdapter adapterLang = new SqlDataAdapter(whichLang);
            adapterLang.Fill(tableLang);
            foreach (DataRow row in tableLang.Rows)
            {
                LanguagesView.Items.Add(row["LanguageCombo"].ToString());

            }
            con.Close();
        }
        private void AddProducer_Click(object sender, RoutedEventArgs e) //добавление продюссера
        {
            AddProducer addProducer = new AddProducer();
            addProducer.Show();
        }
        private void AddActor_Click(object sender, RoutedEventArgs e) //добавление актера
        {
            addactor addactor = new addactor();
            addactor.Show();
        }
        private void SearchCountryListView_Click(object sender, RoutedEventArgs e)
        {
            ChooseCountry.Items.Clear();
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand whichCountry = new SqlCommand("Select * from Country where Name_country='"+SearchCountryText.Text+"'", con);
            SqlDataReader Countries;
            con.Open();

            Countries = whichCountry.ExecuteReader();
            while (Countries.Read())
            {
                ChooseCountry.Items.Add(Countries["Name_country"]);

            }
            Countries.Close();
            con.Close();
        } //поиск страны
        private void SearchGenre_Click(object sender, RoutedEventArgs e)
        {
            Genres.Items.Clear();
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand WhichGenre = new SqlCommand("Select * from Genre where Name_Genre='"+SearchGenreText.Text+"'", con);
            SqlDataReader GenresFilm;
            con.Open();

            GenresFilm = WhichGenre.ExecuteReader();
            while (GenresFilm.Read())
            {
                Genres.Items.Add(GenresFilm["Name_Genre"]);

            }
            GenresFilm.Close();
            con.Close();
        }//поиск жанра
        private void SearchActor_Click(object sender, RoutedEventArgs e)
        {
            Actors.Items.Clear();
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand whichActor = new SqlCommand("Select Name_actor, Surname_actor, Name_actor+' '+Surname_actor as 'Fullname_actor' from Actors where Name_actor='" + SearchActorText.Text+"' or Surname_actor='"+ SearchActorText.Text + "'", con);
            whichActor.ExecuteNonQuery();
            DataTable tableAct = new DataTable();
            SqlDataAdapter adapterAct = new SqlDataAdapter(whichActor);
            adapterAct.Fill(tableAct);
            foreach (DataRow row in tableAct.Rows)
            {
                Actors.Items.Add(row["Fullname_actor"].ToString());

            }
            con.Close();
        }//поиск актера
        private void SearchProducer_Click(object sender, RoutedEventArgs e)
        {
            ChooseProducer.Items.Clear();
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand whichProducer = new SqlCommand("Select Name_producer, Surname_producer, Name_producer+' '+Surname_producer as 'Fullname' from Producers where Name_producer ='"+SearchProducerText.Text+ "' or Surname_producer='" + SearchProducerText.Text + "'", con);
            whichProducer.ExecuteNonQuery();
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(whichProducer);
            adapter.Fill(table);
            foreach (DataRow row in table.Rows)
            {
                ChooseProducer.Items.Add(row["Fullname"].ToString());

            }
            con.Close();
        }//поиск продюссера
        private void SearchLang_Click(object sender, RoutedEventArgs e)
        {
            LanguagesView.Items.Clear();
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand whichLang = new SqlCommand("Select Distinct Name_Language from Languages where Name_Language='"+SearchLanguageText.Text+"'", con);
            whichLang.ExecuteNonQuery();
            DataTable tableLang = new DataTable();
            SqlDataAdapter adapterLang = new SqlDataAdapter(whichLang);
            adapterLang.Fill(tableLang);
            foreach (DataRow row in tableLang.Rows)
            {
                LanguagesView.Items.Add(row["Name_Language"].ToString());

            }
            con.Close();
        }//поиск языка
    }
}
