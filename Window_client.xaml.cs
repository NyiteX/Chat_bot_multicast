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
        public string name = "Danil";
        string multiIp = "224.5.5.5";

        UdpClient udpClient = new UdpClient();
        public Window_client()
        {
            InitializeComponent();

            tb_chat.DataContext = MainTextStr;

            Thread t1 = new Thread(async() =>
            {
                IPAddress multicastAddress = IPAddress.Parse(multiIp);
                while (true)
                {
                    using (var udpClient = new UdpClient())
                    {                      
                        udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                        udpClient.JoinMulticastGroup(multicastAddress);
                        udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, multicastPort));

                        IPEndPoint remoteEndPoint = new IPEndPoint(multicastAddress, multicastPort);

                        var response = await udpClient.ReceiveAsync();
                        string responseString = Encoding.UTF8.GetString(response.Buffer);

                        if (responseString.Length > 0)
                        {
                            MainTextStr.MyString += responseString + "\n";
                        }
                    }
                }
            });
            t1.SetApartmentState(ApartmentState.STA);
            t1.Start();
        }

        async void SendMessageToServer(string message)
        {        
            IPAddress ip = IPAddress.Parse(multiIp);
            byte[] requestData = Encoding.UTF8.GetBytes(message);
            await udpClient.SendAsync(requestData, requestData.Length, new IPEndPoint(ip, multicastPort));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tb_send.Text.Length > 0 && tb_send.Text != "Enter message here...")
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

        private void tb_chat_TextChanged(object sender, TextChangedEventArgs e)
        {
            tb_chat.ScrollToEnd();
        }

        private void tb_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
    }
}
