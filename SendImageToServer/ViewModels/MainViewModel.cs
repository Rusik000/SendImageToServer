using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SendImageToServer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainWindow MainView { get; set; }

        private Image _serverImage;

        public Image ServerImage
        {
            get { return _serverImage; }
            set { _serverImage = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            Receive();
        }

        private void Receive()
        {
            var ipadress = IPAddress.Parse("192.168.1.109");
            var port = 2501;

            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                var ep = new IPEndPoint(ipadress, port);
                socket.Bind(ep);
                socket.Listen(10);
                ServerImage = new Image();
                while (true)
                {
                    var client = socket.Accept();

                    var bytes = new byte[1024];
                    int lenght = 0;
                    lenght = client.Receive(bytes);
                    var data = new byte[lenght];
                    Array.Copy(bytes, 0, data, 0, lenght);
                    bytes = new byte[lenght];
                    Array.Copy(data, 0, bytes, 0, lenght);
                    ServerImage = ByteArrayToImage(bytes);

                }
            }

        }


        public Image ByteArrayToImage(byte[] buffer)
        {
            Image returnImage = null;
            string path = Encoding.UTF8.GetString(buffer);

            App.Current.Dispatcher.Invoke(() =>
            {
                returnImage = new Image();
                returnImage.Source = new BitmapImage(new Uri(path));
            });

            return returnImage;
        }
    }
}
