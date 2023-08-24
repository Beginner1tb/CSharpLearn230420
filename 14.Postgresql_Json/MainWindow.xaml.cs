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
using Dapper;
using Npgsql;
using Newtonsoft.Json;

namespace _14.Postgresql_Json
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Host=localhost;Username=postgres;password=613;Database=postgres";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //string commandString = $"SELECT DATA->'hobbies'->>0 FROM json_data WHERE id=3";
                    string commandString = $"SELECT DATA FROM json_data WHERE id=3";

                    //using (NpgsqlCommand command = new NpgsqlCommand(commandString))
                    //{
                    //using (var reader = command.ExecuteReader())
                    //{
                    //    while (reader.Read())
                    //    {
                    //        MessageBox.Show(reader.GetString(0));
                    //    }
                    //}

                    var objectread = connection.ExecuteScalar(commandString);
                    MessageBox.Show(Convert.ToString(objectread));
                    //}
                    Myclass myclass = new Myclass();

                    var json = JsonConvert.DeserializeObject<Myclass>(Convert.ToString(objectread));
                    MessageBox.Show(json.hobbies[1]);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }

    public class Myclass
    {
        public string[] hobbies { get; set; }
    }
}
