using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SendImageToServer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<ListBoxItem> Items { get; set; }

        public MainWindow MainView { get; set; }

        private Image _serverImage;

        public Image ServerImage
        {
            get { return _serverImage; }
            set { _serverImage = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Image> _allImages;

        public ObservableCollection<Image> AllImages
        {
            get { return _allImages; }
            set { _allImages = value; OnPropertyChanged(); }
        }

        static public object obj = new object();

        public MainViewModel()
        {
            Items = new ObservableCollection<ListBoxItem>();
            ServerImage = new Image();

            Thread thread = new Thread(() =>
            {
                Receive();
            });
            thread.ApartmentState = ApartmentState.STA;
            thread.Start();
        }

        private void Receive()
        {

            var ipadress = IPAddress.Parse("192.168.1.109");
            var port = 1804;

            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {

                var ep = new IPEndPoint(ipadress, port);
                lock (obj)
                {
                    socket.Bind(ep);
                    socket.Listen(10);

                }


                while (true)
                {
                    var client = socket.Accept();

                    Task.Run(() =>
                    {

                        var bytes = new byte[1024];
                        int lenght = 0;
                        lenght = client.Receive(bytes);
                        var data = new byte[lenght];
                        Array.Copy(bytes, 0, data, 0, lenght);
                        bytes = new byte[lenght];
                        Array.Copy(data, 0, bytes, 0, lenght);
                        var image = ByteArrayToImage(bytes);
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            if (image.Source != null)
                            {
                                image.Width = 300;
                                image.Height = 300;
                                image.Stretch = System.Windows.Media.Stretch.Fill;

                                StackPanel stckpanel = new StackPanel()
                                {
                                    Orientation = Orientation.Horizontal,
                                    Width = 500,
                                    Height = 500
                                };

                                TextBlock txtblck = new TextBlock()
                                {
                                    Text = client.RemoteEndPoint.ToString(),
                                    FontSize = 22
                                };
                                stckpanel.Children.Add(image);
                                stckpanel.Children.Add(txtblck);

                                ListBoxItem item = new ListBoxItem();
                                item.Content = stckpanel;

                                Items.Add(item);
                            }
                        });

                    });


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
