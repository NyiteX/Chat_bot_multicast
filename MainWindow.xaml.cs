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

namespace Chat_bot_multicast
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isServerActive_my = false;
        public MainWindow()
        {
            InitializeComponent();
           
        }      

        private void btn_server_Click(object sender, RoutedEventArgs e)
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
        }

        private void btn_client_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                Window_client form = new Window_client();

                form.ShowDialog();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
}
