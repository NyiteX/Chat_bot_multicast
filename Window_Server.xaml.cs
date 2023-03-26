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
        int Interval = 500;
        public Window_Server()
        {
            InitializeComponent();

            MainTextStr.MyString = "Server started.\n\n";
            tb_main.DataContext = MainTextStr;

            Thread Sender = new Thread(new ThreadStart(multicastSend));
            Sender.Start();
        }

        void multicastSend()
        {
            /*Console.WriteLine("Server");*/
            while (true)
            {
                //Thread.Sleep(Interval);
                string message = "";
                while(message == "")
                {
                    message = Encoding.UTF8.GetString(Listener());
                }
                MainTextStr.MyString += message + "\n";

                Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                soc.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);

                IPAddress dast = IPAddress.Parse("224.5.5.5");

                soc.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(dast));

                IPEndPoint ipep = new IPEndPoint(dast, 8080);

                soc.Connect(ipep);
                soc.Send(Encoding.UTF8.GetBytes(message));
                soc.Close();
            }
        }
       byte[] Listener()
        {
            /*Console.WriteLine("\nClient");*/
            /*while (true)
            {*/
                Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 8080);

                soc.Bind(ipep);

                IPAddress ip = IPAddress.Parse("224.5.5.5");

                soc.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(ip, IPAddress.Any));

                byte[] buff = new byte[4096];

                soc.Receive(buff);
                /*if (Encoding.UTF8.GetString(buff))
                    Console.WriteLine(Encoding.UTF8.GetString(buff));*/
                soc.Close();
            return buff;
            /*}*/
        }
    }
}
