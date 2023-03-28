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
using System.Windows.Shapes;
using System.Threading;

namespace Chat_bot_multicast
{
    /// <summary>
    /// Логика взаимодействия для Window_Server.xaml
    /// </summary>
    public partial class Window_Server : Window
    {
        stringVM MainTextStr = new stringVM();
        int multicastPort = 8080;

        public Window_Server()
        {
            InitializeComponent();

            MainTextStr.MyString = "Server started.\n\n";
            tb_main.DataContext = MainTextStr;

            Thread t = new Thread(async() => { await udp_(); });
            t.Start();
        }

        async Task udp_()
        {
            IPAddress multicastAddress = IPAddress.Parse("224.5.5.5");
            while (true)
            {
                using (var udpServer = new UdpClient())
                {
                    udpServer.JoinMulticastGroup(multicastAddress);
                    udpServer.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    udpServer.Client.Bind(new IPEndPoint(IPAddress.Any, multicastPort));

                    var data = await udpServer.ReceiveAsync();
                    string request = Encoding.UTF8.GetString(data.Buffer);

                    if (request.Count() > 0)
                    {
                        MainTextStr.MyString += request + "\n";
                        byte[] responseData = Encoding.UTF8.GetBytes(request);
                        await udpServer.SendAsync(responseData, responseData.Length, new IPEndPoint(multicastAddress, multicastPort));
                    }
                }
            }
        }

        private void tb_main_TextChanged(object sender, TextChangedEventArgs e)
        {
            tb_main.ScrollToEnd();
        }
    }
}
