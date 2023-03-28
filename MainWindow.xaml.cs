using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
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
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Data.Sqlite;

namespace Chat_bot_multicast
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isServerActive_my = false;
        string database_name = "D:/Database25.db";
        public MainWindow()
        {
            InitializeComponent();
            btn_client.IsEnabled = false;
            
            Ini();
        }      

       /* private void btn_server_Click(object sender, RoutedEventArgs e)
        {
            if (!isServerActive_my)
            {
                isServerActive_my = true;
                btn_server.IsEnabled = false;

                Thread thread = new Thread(() =>
                {
                    Window_Server form = new Window_Server();
                    form.Closed += delegate
                    {
                        Dispatcher.Invoke(() =>
                        {
                            isServerActive_my = false;
                            btn_server.IsEnabled = true;
                        });
                    };

                    form.ShowDialog();
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
        }*/

        private void btn_client_Click(object sender, RoutedEventArgs e)
        {
            string tmp = Login_TB.Text;
            Thread thread = new Thread(() =>
            {
                Window_client form = new Window_client();
                form.name = tmp;
                form.ShowDialog();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            btn_client.IsEnabled = false;
            Pass_TB.Text = "Password";
            login_maybe.IsChecked = false;
            Login_TB.Text = "Login";
            Name_TB.Text = "FirstName";
        }


        void Ini()
        {
            if (!File.Exists(database_name))
                using (var connection = new SqliteConnection("Data Source = " + database_name))
                {
                    connection.Open();

                    SqliteCommand command = new SqliteCommand("Data Source = " + database_name);
                    command.Connection = connection;

                    command.CommandText = "CREATE TABLE Client" +
                "(ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, " +
                "Name TEXT NOT NULL, " +
                "Login TEXT NOT NULL UNIQUE, " +
                "Pass TEXT NOT NULL)";
                    command.ExecuteNonQuery();
                }
        }

        void Save()
        {
            if (File.Exists(database_name))
                using (var connection = new SqliteConnection("Data Source = " + database_name))
                {
                    connection.Open();

                    SqliteCommand command = new SqliteCommand("Data Source = " + database_name);
                    command.Connection = connection;

                    command.CommandText = "INSERT INTO Client(Name,Login,Pass) " +
                        "VALUES('" + Name_TB.Text + "','" + Login_TB.Text + "','" + Pass_TB.Text + "')";
                    command.ExecuteNonQuery();
                }
        }
        bool Login_check()
        {
            if (File.Exists(database_name))
                using (var connection = new SqliteConnection("Data Source = " + database_name))
                {
                    connection.Open();

                    SqliteCommand command = new SqliteCommand("Data Source = " + database_name);
                    command.Connection = connection;

                    command.CommandText = "SELECT * FROM Client";
                    SqliteDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            bool f = false;
                            if (Login_TB.Text == Convert.ToString(reader.GetValue(2))) f = true;
                            else f = false;
                            if (Pass_TB.Text == Convert.ToString(reader.GetValue(3))) f = true;
                            else f = false;
                            if (f) return true;
                        }
                    }
                }
            return false;
        }

        string Proverka_probel(string str)
        {
            str = str.Replace(" ", ""); ;

            return str;
        }
        bool Proverka_kol(string str)
        {
            bool b = false;
            for (int i = 0; i < str.Length; i++)
                if (str[i] != ' ') b = true;

            return b;
        }

        private void Name_TB_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Name_TB.Text == "FirstName")
                Name_TB.Text = "";
        }

        private void Name_TB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Name_TB.Text == "" || !Proverka_kol(Name_TB.Text))
                Name_TB.Text = "FirstName";
            Name_TB.Text = Proverka_probel(Name_TB.Text);
        }

        private void Login_TB_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Login_TB.Text == "Login")
                Login_TB.Text = "";
        }

        private void Login_TB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Login_TB.Text == "" || !Proverka_kol(Login_TB.Text))
                Login_TB.Text = "Login";
            Login_TB.Text = Proverka_probel(Login_TB.Text);
        }

        private void Pass_TB_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Pass_TB.Text == "Password")
                Pass_TB.Text = "";
        }

        private void Pass_TB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Pass_TB.Text == "" || !Proverka_kol(Pass_TB.Text))
                Pass_TB.Text = "Password";
            Pass_TB.Text = Proverka_probel(Pass_TB.Text);
        }

        private void BTN_Login_Click(object sender, RoutedEventArgs e)
        {
            if (login_maybe.IsChecked == false)
            {
                if (Login_TB.Text.Length > 0 && Name_TB.Text.Length > 0 && Pass_TB.Text.Length > 0
                    && Login_TB.Text != "Login" && Name_TB.Text != "FirstName" && Pass_TB.Text != "Password")
                {
                    Save();
                    MessageBox.Show("Добавлено");
                }
                else MessageBox.Show("Кривой ввод. Пустые строки.");
            }
            else
            {
                try
                {
                    if (Login_TB.Text.Length > 0 && Pass_TB.Text.Length > 0
                        && Login_TB.Text != "Login" && Pass_TB.Text != "Password")
                    {
                        if (!Login_check()) MessageBox.Show("ti durak");
                        else
                        {
                            btn_client.IsEnabled = true;
                        }
                    }
                    else MessageBox.Show("Кривой ввод. Пустые строки.");
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }

        }
        private void login_maybe_Unchecked(object sender, RoutedEventArgs e)
        {
            line_name.Visibility = Visibility.Visible;
            Name_TB.Visibility = Visibility.Visible;

            BTN_Login.Content = "Sign up";
        }

        private void login_maybe_Checked(object sender, RoutedEventArgs e)
        {
            line_name.Visibility = Visibility.Hidden;
            Name_TB.Visibility = Visibility.Hidden;

            BTN_Login.Content = "Log in";
        }

        private void btn_x_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
