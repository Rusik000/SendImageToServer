using Clients.Command;
using Microsoft.Win32;
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
using System.Windows.Media.Imaging;


namespace Clients.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainWindow MainView { get; set; }

        public RelayCommand DownloadCommand { get; set; }

        public RelayCommand SendCommand { get; set; }


        private string _imagePath { get; set; }


        private Image _image;
        public Image Image
        {
            get { return _image; }
            set { _image = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            DownloadCommand = new RelayCommand((sender) =>
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "PNG (*.png)|*.png|JPEG (*.jpeg)|*.jpeg|JPG (*.jpg)|*.jpg";
                open.FilterIndex = 1;
                open.Multiselect = false;
                if (Convert.ToBoolean(open.ShowDialog()) == true)
                {
                    MainView.MyImage.Source = new BitmapImage(new Uri(open.FileName));
                    _imagePath = open.FileName;
                }
            });

            Image = new Image();

            SendCommand = new RelayCommand((sender) =>
            {
                Thread thread = new Thread(() =>
                {
                    Send();
                });
                thread.Start();
            },(predicate) =>
            {
                if (MainView.MyImage.Source != null)
                {
                    return true;
                }
                return false;
            });
        }
        private async void Send()
        {
            var ipadress = IPAddress.Parse("192.168.1.109");
            var port = 2501;
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                var ep = new IPEndPoint(ipadress, port);
                try
                {
                    socket.Connect(ep);
                    if (socket.Connected)
                    {
                        var bytes = Encoding.UTF8.GetBytes(_imagePath);
                        socket.Send(bytes);
                        MessageBox.Show("Image is send succesfully");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
    }
}
