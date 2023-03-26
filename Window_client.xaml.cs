using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chat_bot_multicast
{
    /// <summary>
    /// Логика взаимодействия для Window_client.xaml
    /// </summary>
    public partial class Window_client : Window
    {
        stringVM MainTextStr = new stringVM();
        int multicastPort = 8080;
        string name = "Danil";

        public Window_client()
        {
            InitializeComponent();

            tb_chat.DataContext = MainTextStr;

            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        MainTextStr.MyString += Encoding.UTF8.GetString(Listener()) + "\n";
                    }
                    catch { }
                }
            });
            thread.Start();
        }

        void SendMessageToServer(string message)
        {
            Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPAddress ip = IPAddress.Parse("224.5.5.5");
            IPEndPoint ipep = new IPEndPoint(ip, 8080);

            byte[] data = Encoding.UTF8.GetBytes(message);
            soc.SendTo(data, ipep);

            soc.Close();
        }
        byte[] Listener()
        {
            Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPAddress ip = IPAddress.Parse("224.5.5.5");

            soc.Bind(new IPEndPoint(IPAddress.Any, multicastPort));
            soc.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(ip, IPAddress.Any));

            byte[] buff = new byte[4096];

            soc.Receive(buff);
            soc.Close();

            return buff;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tb_send.Text.Length > 0)
            {
                SendMessageToServer(name + ": " + tb_send.Text);

                tb_send.Text = "Enter message here...";
            }
        }

        private void tb_send_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tb_send.Text == "Enter message here...") tb_send.Text = "";
        }

        private void tb_send_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tb_send.Text == "") tb_send.Text = "Enter message here...";
        }
    }
}
