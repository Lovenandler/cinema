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
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Syncfusion.DocIO.DLS;
using System.ComponentModel;
using Spire.Doc;
using Word = Microsoft.Office.Interop.Word;




namespace cinema
{
    /// <summary>
    /// Логика взаимодействия для chooseseat.xaml
    /// </summary>
    public partial class chooseseat : Window
    {

        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        int Price;
        string ID_seat;

        public chooseseat()
        {
            InitializeComponent();
            FillListStandart();
            FillCinemas(schedule.cinemaTitle, schedule.date, schedule.time);
            FillHalls();
            FillFilms(schedule.filmTitle);
        }

        public void FillFilms(string filmName) //заполнение списка фильмов
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand ChooseTypeHall = new SqlCommand("Select * from Films ", con);

            SqlDataReader TypeHallChange;
            TypeHallChange = ChooseTypeHall.ExecuteReader();
            while (TypeHallChange.Read())
            {
                ChooseFilmCombobox.Items.Add(TypeHallChange["Name_Film"]);

            }
            ChooseFilmCombobox.Text = filmName;
            TypeHallChange.Close();

            con.Close();
        }
        public void FillHalls() //заполнение списка залов
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand ChooseTypeHall = new SqlCommand("Select * from Halls", con);

            SqlDataReader TypeHallChange;
            TypeHallChange = ChooseTypeHall.ExecuteReader();
            while (TypeHallChange.Read())
            {
                ChooseHall.Items.Add(TypeHallChange["Number_Hall"]);

            }
            TypeHallChange.Close();

            con.Close();
        }
        public void FillCinemas(string cinemaName, string filmDate, string filmTime) //заполнение списка кинотеатров
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand ChooseCinemas = new SqlCommand("Select * from Cinemas", con);
            SqlDataReader CinemasChoose;
            CinemasChoose = ChooseCinemas.ExecuteReader();
            while (CinemasChoose.Read())
            {
                ChooseCinema.Items.Add(CinemasChoose["Name_cinema"]);

            }
            ChooseCinema.Text = cinemaName;
            date.Text = filmDate;
            Time.Value = Convert.ToDateTime(filmTime);
            CinemasChoose.Close();
            con.Close();
        }
        public void FillListStandart() //заполнение схемы стандартного зала
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand ChooseSeat = new SqlCommand("Select * from Seats_standart", con);
            con.Open();
            SqlDataReader SeatChoose;
            SeatChoose = ChooseSeat.ExecuteReader();
            while (SeatChoose.Read())
            {
                seats.Items.Add(SeatChoose["Number_Of_Seat"]);

            }
            SeatChoose.Close();
            con.Close();
        }
        public void FillListMuvic() //заполнение схемы детского зала
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand ChooseSeat = new SqlCommand("Select * from Seats_muvic", con);
            con.Open();
            SqlDataReader SeatChoose;
            SeatChoose = ChooseSeat.ExecuteReader();
            while (SeatChoose.Read())
            {

                seats.Items.Add(SeatChoose["Number_Of_Seat"]);

            }
            SeatChoose.Close();
            con.Close();
        }
        public void FillListPremium() //заполнение схемы премиум зала
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand ChooseSeat = new SqlCommand("Select * from Seats_premium", con);
            con.Open();
            SqlDataReader SeatChoose;
            SeatChoose = ChooseSeat.ExecuteReader();
            while (SeatChoose.Read())
            {
                
                seats.Items.Add(SeatChoose["Number_Of_Seat"]);

            }
            SeatChoose.Close();
            con.Close();
        }
        private void seats_SelectionChanged(object sender, SelectionChangedEventArgs e) //информация о местах
        {

            SelectedSeat.Clear();
            foreach (object seat in seats.SelectedItems)
            {
                SelectedSeat.Text += (SelectedSeat.Text == "" ? "" : ",") + seat.ToString();

            }

        }
        private void Return_Click(object sender, RoutedEventArgs e) //кнопка возврата
        {
            Window2 window2 = new Window2();
            window2.Show();
            this.Close();

        }

        private void ChooseCinema_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TypeHallCinema_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ChooseHall_DropDownClosed(object sender, EventArgs e) //категория в зависимости от зала
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("Select * from Halls where Number_Hall ='" + ChooseHall.Text + "'", connection);
            connection.Open();
            command.ExecuteNonQuery();
            SqlDataReader sqlData;
            sqlData = command.ExecuteReader();
            while (sqlData.Read())
            {
                string CategoryHall = (string)sqlData["Category_Hall"].ToString();
                TypeHallCinema.Text = CategoryHall;
                if (CategoryHall == "Премиум")
                {
                    seats.Items.Clear();
                    FillListPremium();
                }else if (CategoryHall == "Мувик")
                {
                    seats.Items.Clear();
                    FillListMuvic();
                }else if (CategoryHall == "Стандарт")
                {
                    seats.Items.Clear();
                    FillListStandart();
                }
            }

        }

        private void CreateWord() //создание документа с информацией о билете
        {
            Word.Application app = new Word.Application();
            Word.Document doc = app.Documents.Add(Visible: true);
            var pNameFilm = doc.Paragraphs.Add();
            pNameFilm.Format.SpaceAfter = 10f;
            pNameFilm.Range.Text = String.Format("Название фильма: " + ChooseFilmCombobox.Text);
            pNameFilm.Range.InsertParagraphAfter();
            var pSeat = doc.Paragraphs.Add();
            pSeat.Format.SpaceAfter = 10f;
            pSeat.Range.Text = String.Format("Места: " + SelectedSeat.Text);
            pSeat.Range.InsertParagraphAfter();
            var pHall = doc.Paragraphs.Add();
            pHall.Format.SpaceAfter = 10f;
            pHall.Range.Text = String.Format("Зал: " + ChooseHall.Text);
            pHall.Range.InsertParagraphAfter();
            var pCategory = doc.Paragraphs.Add();
            pCategory.Format.SpaceAfter = 10f;
            pCategory.Range.Text = String.Format("Категория: " + TypeHallCinema.Text);
            pCategory.Range.InsertParagraphAfter();
            var pDate = doc.Paragraphs.Add();
            pDate.Format.SpaceAfter = 10f;
            pDate.Range.Text = String.Format("Дата сеанса: " + date.Text);
            pDate.Range.InsertParagraphAfter();
            var pTime = doc.Paragraphs.Add();
            pTime.Format.SpaceAfter = 10f;
            pTime.Range.Text = String.Format("время сеанса: " + Time.Text);
            pTime.Range.InsertParagraphAfter();
            try
            {
                doc.Save();
                doc.Close();
                app.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Загрузка билета отменена");
            }
        }

        private void booked_Click(object sender, RoutedEventArgs e) //бронирование
        {
            if (ChooseFilmCombobox.Text.Length > 0)
            {
                if (ChooseCinema.Text.Length > 0)
                {
                    if (date.Text.Length > 0)
                    {
                        if (Time.Text.Length > 0)
                        {
                            if (ChooseHall.Text.Length > 0)
                            {
                                if (seats.SelectedItems.Count > 0)
                                {
                                    if (ChooseHall.Text == "1")
                                    {
                                        #region Бронирование стандартого зала
                                        Price = 250;
                                        string[] allTokens = SelectedSeat.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                        int[] allInts = Array.ConvertAll<string, int>(allTokens, int.Parse);
                                        foreach (int c in allInts)
                                        {

                                            int selected_row = seats.SelectedIndex / 18 + 1;//выбранный ряд

                                            SqlConnection con = new SqlConnection(@"Data Source=LOVENANDLER\SQLEXPRESS;Initial Catalog=Cinema;Integrated Security=True");

                                            con.Open();
                                            SqlCommand checkSeat = new SqlCommand("Select * From Seats_standart where Number_Of_Seat ='" + c + "' and Booking_Status = 'Свободно' and Number_Row = '" + selected_row + "'", con);
                                            checkSeat.ExecuteNonQuery();
                                            DataTable dataTableSeat = new DataTable();
                                            SqlDataAdapter adapterSeat = new SqlDataAdapter(checkSeat);
                                            adapterSeat.Fill(dataTableSeat);
                                            int i = Convert.ToInt32(dataTableSeat.Rows.Count.ToString());
                                            if (i != 0)
                                            {
                                                SqlCommand Search_IDSeat = new SqlCommand("SELECT ID_seat FROM Seats_standart WHERE Number_Of_Seat = @NumberSeat and Number_Row = @NumberRow and Number_Hall = @NumberHall", con);
                                                Search_IDSeat.Parameters.AddWithValue("@NumberSeat", c);
                                                Search_IDSeat.Parameters.AddWithValue("@NumberRow", selected_row);
                                                Search_IDSeat.Parameters.AddWithValue("@NumberHall", ChooseHall.Text);
                                                SqlDataReader sqlDataReaderSeat = Search_IDSeat.ExecuteReader();
                                                while (sqlDataReaderSeat.Read())
                                                {

                                                    ID_seat = sqlDataReaderSeat.GetValue(0).ToString();

                                                }
                                                sqlDataReaderSeat.Close();

                                                SqlCommand Search_IDFilm = new SqlCommand("Select ID_Film from Films where Name_Film = @NameFilm", con);
                                                Search_IDFilm.Parameters.AddWithValue("@NameFilm", ChooseFilmCombobox.Text);

                                                SqlDataReader sqlDataReaderFilm = Search_IDFilm.ExecuteReader();
                                                while (sqlDataReaderFilm.Read())
                                                {

                                                    Select_ID_Film.Text = sqlDataReaderFilm.GetValue(0).ToString();

                                                }
                                                sqlDataReaderFilm.Close();

                                                SqlCommand Search_IDCinema = new SqlCommand("Select ID_cinema from Cinemas where Name_cinema = @NameCinema", con);
                                                Search_IDCinema.Parameters.AddWithValue("@NameCinema", ChooseCinema.Text);

                                                SqlDataReader sqlDataReaderCinema = Search_IDCinema.ExecuteReader();
                                                while (sqlDataReaderCinema.Read())
                                                {

                                                    Select_ID_Cinema.Text = sqlDataReaderCinema.GetValue(0).ToString();

                                                }
                                                sqlDataReaderCinema.Close();

                                                SqlCommand Search_DateFilmSession = new SqlCommand("Select Date_FilmSession from Schedule where Date_FilmSession = @DateFilmSession", con);
                                                Search_DateFilmSession.Parameters.AddWithValue("@DateFilmSession", date.Text);

                                                SqlDataReader sqlDataReaderDateFilmSession = Search_DateFilmSession.ExecuteReader();
                                                while (sqlDataReaderDateFilmSession.Read())
                                                {

                                                    Select_DateFilmSession.Text = sqlDataReaderDateFilmSession.GetValue(0).ToString();

                                                }
                                                sqlDataReaderDateFilmSession.Close();

                                                SqlCommand Search_TimeFilmSession = new SqlCommand("Select Time_FilmSession from Schedule where Time_FilmSession = @TimeFilmSession", con);

                                                try
                                                {
                                                    Search_TimeFilmSession.Parameters.AddWithValue("@TimeFilmSession", Time.Text);
                                                    SqlDataReader sqlDataReaderTimeFilmSession = Search_TimeFilmSession.ExecuteReader();
                                                    while (sqlDataReaderTimeFilmSession.Read())
                                                    {

                                                        Select_TimeFilmSession.Text = sqlDataReaderTimeFilmSession.GetValue(0).ToString();

                                                    }
                                                    sqlDataReaderTimeFilmSession.Close();
                                                }
                                                catch (Exception ex)
                                                {
                                                    MessageBox.Show(ex.Message);
                                                }


                                                SqlCommand Search_IDFilmSession = new SqlCommand("Select ID_FilmSession from Schedule where ID_Film = @IDFilm and ID_cinema = @IDCinema and Date_FilmSession = @DateFilmSession and Time_FilmSession = @TimeFilmSession", con);
                                                Search_IDFilmSession.Parameters.AddWithValue("@IDFilm", Select_ID_Film.Text);
                                                Search_IDFilmSession.Parameters.AddWithValue("@IDCinema", Select_ID_Cinema.Text);
                                                Search_IDFilmSession.Parameters.AddWithValue("@DateFilmSession", Select_DateFilmSession.Text);
                                                Search_IDFilmSession.Parameters.AddWithValue("@TimeFilmSession", Select_TimeFilmSession.Text);
                                                SqlDataReader sqlDataReaderIDFilmSession = Search_IDFilmSession.ExecuteReader();
                                                while (sqlDataReaderIDFilmSession.Read())
                                                {

                                                    Select_IDFilmSession.Text = sqlDataReaderIDFilmSession.GetValue(0).ToString();

                                                }
                                                sqlDataReaderIDFilmSession.Close();

                                                
                                                SqlCommand booked = new SqlCommand(null, con);

                                                booked.CommandText = "UPDATE Seats_standart  SET Booking_Status = @Booking_Status WHERE Number_Of_Seat = @numberSeat and Number_Row = @numberRow";
                                                SqlParameter BookingParam = new SqlParameter("@Booking_Status", SqlDbType.NVarChar, 20);
                                                SqlParameter numberSeatParam = new SqlParameter("@numberSeat", SqlDbType.Int);
                                                SqlParameter numberRowParam = new SqlParameter("@numberRow", SqlDbType.Int);
                                                numberSeatParam.Value = c;
                                                numberRowParam.Value = selected_row;
                                                BookingParam.Value = "Забронированно";

                                                booked.Parameters.Add(BookingParam);
                                                booked.Parameters.Add(numberSeatParam);
                                                booked.Parameters.Add(numberRowParam);

                                                booked.Prepare();

                                                booked.ExecuteNonQuery();



                                                // " 8, 2, 8 "

                                                // i = количество выбранных мест
                                                // for (;;i < number;i++)




                                                String query = "INSERT INTO Tickets (ID_seat,Price,ID_FilmSession) VALUES (@ID_seat,@Price,@ID_FilmSession)";

                                                SqlCommand command = new SqlCommand(query, con);

                                                command.Parameters.AddWithValue("@ID_seat", Int32.Parse(Select_ID_seat.Text));
                                                command.Parameters.AddWithValue("@Price", Price);
                                                command.Parameters.AddWithValue("@ID_FilmSession", Int32.Parse(Select_IDFilmSession.Text));

                                                int rowsAffected = command.ExecuteNonQuery();
                                                if (rowsAffected > 0)
                                                {
                                                    MessageBox.Show("Место успешно забронированно");
                                                    CreateWord();
                                                }

                                                else
                                                {
                                                    MessageBox.Show("Ошибка бронирования");
                                                }










                                            }
                                            else
                                            {
                                                MessageBox.Show("Место уже забронировано");
                                            }



                                            con.Close();
                                        }
                                        #endregion

                                    } else if (ChooseHall.Text == "2")
                                    {
                                        #region Бронирование детского зала
                                        Price = 150;
                                        string[] allTokens = SelectedSeat.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                        int[] allInts = Array.ConvertAll<string, int>(allTokens, int.Parse);
                                        foreach (int c in allInts)
                                        {

                                            int selected_row = seats.SelectedIndex / 18 + 1;//выбранный ряд

                                            SqlConnection con = new SqlConnection(@"Data Source=LOVENANDLER\SQLEXPRESS;Initial Catalog=Cinema;Integrated Security=True");

                                            con.Open();
                                            SqlCommand checkSeat = new SqlCommand("Select * From Seats_muvic where Number_Of_Seat ='" + c + "' and Booking_Status = 'Свободно' and Number_Row = '" + selected_row + "'", con);
                                            checkSeat.ExecuteNonQuery();
                                            DataTable dataTableSeat = new DataTable();
                                            SqlDataAdapter adapterSeat = new SqlDataAdapter(checkSeat);
                                            adapterSeat.Fill(dataTableSeat);
                                            int i = Convert.ToInt32(dataTableSeat.Rows.Count.ToString());
                                            if (i != 0)
                                            {
                                                SqlCommand Search_IDSeat = new SqlCommand("SELECT ID_seat FROM Seats_muvic WHERE Number_Of_Seat = @NumberSeat and Number_Row = @NumberRow and Number_Hall = @NumberHall", con);
                                                Search_IDSeat.Parameters.AddWithValue("@NumberSeat", c);
                                                Search_IDSeat.Parameters.AddWithValue("@NumberRow", selected_row);
                                                Search_IDSeat.Parameters.AddWithValue("@NumberHall", ChooseHall.Text);
                                                SqlDataReader sqlDataReaderSeat = Search_IDSeat.ExecuteReader();
                                                while (sqlDataReaderSeat.Read())
                                                {

                                                    Select_ID_seat.Text = sqlDataReaderSeat.GetValue(0).ToString();

                                                }
                                                sqlDataReaderSeat.Close();

                                                SqlCommand Search_IDFilm = new SqlCommand("Select ID_Film from Films where Name_Film = @NameFilm", con);
                                                Search_IDFilm.Parameters.AddWithValue("@NameFilm", ChooseFilmCombobox.Text);

                                                SqlDataReader sqlDataReaderFilm = Search_IDFilm.ExecuteReader();
                                                while (sqlDataReaderFilm.Read())
                                                {

                                                    Select_ID_Film.Text = sqlDataReaderFilm.GetValue(0).ToString();

                                                }
                                                sqlDataReaderFilm.Close();

                                                SqlCommand Search_IDCinema = new SqlCommand("Select ID_cinema from Cinemas where Name_cinema = @NameCinema", con);
                                                Search_IDCinema.Parameters.AddWithValue("@NameCinema", ChooseCinema.Text);

                                                SqlDataReader sqlDataReaderCinema = Search_IDCinema.ExecuteReader();
                                                while (sqlDataReaderCinema.Read())
                                                {

                                                    Select_ID_Cinema.Text = sqlDataReaderCinema.GetValue(0).ToString();

                                                }
                                                sqlDataReaderCinema.Close();

                                                SqlCommand Search_DateFilmSession = new SqlCommand("Select Date_FilmSession from Schedule where Date_FilmSession = @DateFilmSession", con);
                                                Search_DateFilmSession.Parameters.AddWithValue("@DateFilmSession", date.Text);

                                                SqlDataReader sqlDataReaderDateFilmSession = Search_DateFilmSession.ExecuteReader();
                                                while (sqlDataReaderDateFilmSession.Read())
                                                {

                                                    Select_DateFilmSession.Text = sqlDataReaderDateFilmSession.GetValue(0).ToString();

                                                }
                                                sqlDataReaderDateFilmSession.Close();

                                                SqlCommand Search_TimeFilmSession = new SqlCommand("Select Time_FilmSession from Schedule where Time_FilmSession = @TimeFilmSession", con);

                                                try
                                                {
                                                    Search_TimeFilmSession.Parameters.AddWithValue("@TimeFilmSession", Time.Text);
                                                    SqlDataReader sqlDataReaderTimeFilmSession = Search_TimeFilmSession.ExecuteReader();
                                                    while (sqlDataReaderTimeFilmSession.Read())
                                                    {

                                                        Select_TimeFilmSession.Text = sqlDataReaderTimeFilmSession.GetValue(0).ToString();

                                                    }
                                                    sqlDataReaderTimeFilmSession.Close();
                                                }
                                                catch (Exception ex)
                                                {
                                                    MessageBox.Show(ex.Message);
                                                }


                                                SqlCommand Search_IDFilmSession = new SqlCommand("Select ID_FilmSession from Schedule where ID_Film = @IDFilm and ID_cinema = @IDCinema and Date_FilmSession = @DateFilmSession and Time_FilmSession = @TimeFilmSession", con);
                                                Search_IDFilmSession.Parameters.AddWithValue("@IDFilm", Select_ID_Film.Text);
                                                Search_IDFilmSession.Parameters.AddWithValue("@IDCinema", Select_ID_Cinema.Text);
                                                Search_IDFilmSession.Parameters.AddWithValue("@DateFilmSession", Select_DateFilmSession.Text);
                                                Search_IDFilmSession.Parameters.AddWithValue("@TimeFilmSession", Select_TimeFilmSession.Text);
                                                SqlDataReader sqlDataReaderIDFilmSession = Search_IDFilmSession.ExecuteReader();
                                                while (sqlDataReaderIDFilmSession.Read())
                                                {

                                                    Select_IDFilmSession.Text = sqlDataReaderIDFilmSession.GetValue(0).ToString();

                                                }
                                                sqlDataReaderIDFilmSession.Close();

                                                
                                                SqlCommand booked = new SqlCommand(null, con);

                                                booked.CommandText = "UPDATE Seats_muvic  SET Booking_Status = @Booking_Status WHERE Number_Of_Seat = @numberSeat and Number_Row = @numberRow";
                                                SqlParameter BookingParam = new SqlParameter("@Booking_Status", SqlDbType.NVarChar, 20);
                                                SqlParameter numberSeatParam = new SqlParameter("@numberSeat", SqlDbType.Int);
                                                SqlParameter numberRowParam = new SqlParameter("@numberRow", SqlDbType.Int);
                                                numberSeatParam.Value = c;
                                                BookingParam.Value = "Забронированно";
                                                numberRowParam.Value = selected_row;

                                                booked.Parameters.Add(BookingParam);
                                                booked.Parameters.Add(numberSeatParam);
                                                booked.Parameters.Add(numberRowParam);

                                                booked.Prepare();

                                                booked.ExecuteNonQuery();


                                                String query = "INSERT INTO Tickets (ID_seat,Price,ID_FilmSession) VALUES (@ID_seat,@Price,@ID_FilmSession)";

                                                SqlCommand command = new SqlCommand(query, con);

                                                command.Parameters.AddWithValue("@ID_seat", Int32.Parse(Select_ID_seat.Text));
                                                command.Parameters.AddWithValue("@Price", Price);
                                                command.Parameters.AddWithValue("@ID_FilmSession", Int32.Parse(Select_IDFilmSession.Text));

                                                int rowsAffected = command.ExecuteNonQuery();
                                                if (rowsAffected > 0)
                                                {
                                                    MessageBox.Show("Место успешно забронированно");
                                                    CreateWord();
                                                }

                                                else
                                                {
                                                    MessageBox.Show("Ошибка бронирования");
                                                }

                                            }


                                            else
                                            {
                                                MessageBox.Show("Место уже забронировано");
                                            }



                                            con.Close();
                                        }
                                        #endregion

                                    }
                                    else if (ChooseHall.Text == "3")
                                    {
                                        #region Бронирование зала типа Премиум
                                        Price = 700;
                                        string[] allTokens = SelectedSeat.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                        int[] allInts = Array.ConvertAll<string, int>(allTokens, int.Parse);
                                        foreach (int c in allInts)
                                        {

                                            int selected_row = seats.SelectedIndex / 18 + 1;//выбранный ряд

                                            SqlConnection con = new SqlConnection(@"Data Source=LOVENANDLER\SQLEXPRESS;Initial Catalog=Cinema;Integrated Security=True");

                                            con.Open();
                                            SqlCommand checkSeat = new SqlCommand("Select * From Seats_premium where Number_Of_Seat ='" + c + "' and Booking_Status = 'Свободно' and Number_Row = '" + selected_row + "'", con);
                                            checkSeat.ExecuteNonQuery();
                                            DataTable dataTableSeat = new DataTable();
                                            SqlDataAdapter adapterSeat = new SqlDataAdapter(checkSeat);
                                            adapterSeat.Fill(dataTableSeat);
                                            int i = Convert.ToInt32(dataTableSeat.Rows.Count.ToString());
                                            if (i != 0)
                                            {
                                                SqlCommand Search_IDSeat = new SqlCommand("SELECT ID_seat FROM Seats_premium WHERE Number_Of_Seat = @NumberSeat and Number_Row = @NumberRow and Number_Hall = @NumberHall", con);
                                                Search_IDSeat.Parameters.AddWithValue("@NumberSeat", c);
                                                Search_IDSeat.Parameters.AddWithValue("@NumberRow", selected_row);
                                                Search_IDSeat.Parameters.AddWithValue("@NumberHall", ChooseHall.Text);
                                                SqlDataReader sqlDataReaderSeat = Search_IDSeat.ExecuteReader();
                                                while (sqlDataReaderSeat.Read())
                                                {

                                                    Select_ID_seat.Text = sqlDataReaderSeat.GetValue(0).ToString();

                                                }
                                                sqlDataReaderSeat.Close();

                                                SqlCommand Search_IDFilm = new SqlCommand("Select ID_Film from Films where Name_Film = @NameFilm", con);
                                                Search_IDFilm.Parameters.AddWithValue("@NameFilm", ChooseFilmCombobox.Text);

                                                SqlDataReader sqlDataReaderFilm = Search_IDFilm.ExecuteReader();
                                                while (sqlDataReaderFilm.Read())
                                                {

                                                    Select_ID_Film.Text = sqlDataReaderFilm.GetValue(0).ToString();

                                                }
                                                sqlDataReaderFilm.Close();

                                                SqlCommand Search_IDCinema = new SqlCommand("Select ID_cinema from Cinemas where Name_cinema = @NameCinema", con);
                                                Search_IDCinema.Parameters.AddWithValue("@NameCinema", ChooseCinema.Text);

                                                SqlDataReader sqlDataReaderCinema = Search_IDCinema.ExecuteReader();
                                                while (sqlDataReaderCinema.Read())
                                                {

                                                    Select_ID_Cinema.Text = sqlDataReaderCinema.GetValue(0).ToString();

                                                }
                                                sqlDataReaderCinema.Close();

                                                SqlCommand Search_DateFilmSession = new SqlCommand("Select Date_FilmSession from Schedule where Date_FilmSession = @DateFilmSession", con);
                                                Search_DateFilmSession.Parameters.AddWithValue("@DateFilmSession", date.Text);

                                                SqlDataReader sqlDataReaderDateFilmSession = Search_DateFilmSession.ExecuteReader();
                                                while (sqlDataReaderDateFilmSession.Read())
                                                {

                                                    Select_DateFilmSession.Text = sqlDataReaderDateFilmSession.GetValue(0).ToString();

                                                }
                                                sqlDataReaderDateFilmSession.Close();

                                                SqlCommand Search_TimeFilmSession = new SqlCommand("Select Time_FilmSession from Schedule where Time_FilmSession = @TimeFilmSession", con);

                                                try
                                                {
                                                    Search_TimeFilmSession.Parameters.AddWithValue("@TimeFilmSession", Time.Text);
                                                    SqlDataReader sqlDataReaderTimeFilmSession = Search_TimeFilmSession.ExecuteReader();
                                                    while (sqlDataReaderTimeFilmSession.Read())
                                                    {

                                                        Select_TimeFilmSession.Text = sqlDataReaderTimeFilmSession.GetValue(0).ToString();

                                                    }
                                                    sqlDataReaderTimeFilmSession.Close();
                                                }
                                                catch (Exception ex)
                                                {
                                                    MessageBox.Show(ex.Message);
                                                }


                                                SqlCommand Search_IDFilmSession = new SqlCommand("Select ID_FilmSession from Schedule where ID_Film = @IDFilm and ID_cinema = @IDCinema and Date_FilmSession = @DateFilmSession and Time_FilmSession = @TimeFilmSession", con);
                                                Search_IDFilmSession.Parameters.AddWithValue("@IDFilm", Select_ID_Film.Text);
                                                Search_IDFilmSession.Parameters.AddWithValue("@IDCinema", Select_ID_Cinema.Text);
                                                Search_IDFilmSession.Parameters.AddWithValue("@DateFilmSession", Select_DateFilmSession.Text);
                                                Search_IDFilmSession.Parameters.AddWithValue("@TimeFilmSession", Select_TimeFilmSession.Text);
                                                SqlDataReader sqlDataReaderIDFilmSession = Search_IDFilmSession.ExecuteReader();
                                                while (sqlDataReaderIDFilmSession.Read())
                                                {

                                                    Select_IDFilmSession.Text = sqlDataReaderIDFilmSession.GetValue(0).ToString();

                                                }
                                                sqlDataReaderIDFilmSession.Close();

                                                
                                                SqlCommand booked = new SqlCommand(null, con);

                                                booked.CommandText = "UPDATE Seats_premium  SET Booking_Status = @Booking_Status WHERE Number_Of_Seat = @numberSeat and Number_Row = @numberRow";
                                                SqlParameter BookingParam = new SqlParameter("@Booking_Status", SqlDbType.NVarChar, 20);
                                                SqlParameter numberSeatParam = new SqlParameter("@numberSeat", SqlDbType.Int);
                                                SqlParameter numberRowParam = new SqlParameter("@numberRow", SqlDbType.Int);
                                                numberSeatParam.Value = c;
                                                BookingParam.Value = "Забронированно";
                                                numberRowParam.Value = selected_row;
                                                booked.Parameters.Add(BookingParam);
                                                booked.Parameters.Add(numberSeatParam);
                                                booked.Parameters.Add(numberRowParam);

                                                booked.Prepare();

                                                booked.ExecuteNonQuery();



                                                // " 8, 2, 8 "

                                                // i = количество выбранных мест
                                                // for (;;i < number;i++)




                                                String query = "INSERT INTO Tickets (ID_seat,Price,ID_FilmSession) VALUES (@ID_seat,@Price,@ID_FilmSession)";

                                                SqlCommand command = new SqlCommand(query, con);

                                                command.Parameters.AddWithValue("@ID_seat", Int32.Parse(Select_ID_seat.Text));
                                                command.Parameters.AddWithValue("@Price", Price);
                                                command.Parameters.AddWithValue("@ID_FilmSession", Int32.Parse(Select_IDFilmSession.Text));

                                                int rowsAffected = command.ExecuteNonQuery();
                                                if (rowsAffected > 0)
                                                {
                                                    MessageBox.Show("Место успешно забронированно");
                                                    CreateWord();
                                                }

                                                else
                                                {
                                                    MessageBox.Show("Сеанс отсутствует");
                                                }










                                            }


                                            else
                                            {
                                                MessageBox.Show("Место уже забронировано");
                                            }



                                            con.Close();
                                        }
                                        #endregion

                                    }
                                }
                                else MessageBox.Show("Выберите место(а)");
                            }
                            else MessageBox.Show("Выберите зал");
                        }
                        else MessageBox.Show("Выберите время сеанса");
                    }
                    else MessageBox.Show("Выберите дату сеанса");
                }
                else MessageBox.Show("Выберите кинотеатр");
            }
            else MessageBox.Show("Выберите фильм");

        }

        private void TextBlock_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }
    }
}
