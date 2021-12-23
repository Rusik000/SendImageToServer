using Clients.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Clients.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainWindow MainView { get; set; }

        public RelayCommand DownloadCommand { get; set; }

        public RelayCommand SendCommand { get; set; }
        public MainViewModel()
        {
            DownloadCommand = new RelayCommand((sender) =>
            {
                //// open file dialog   
                //OpenFileDialog open = new OpenFileDialog();
                //// image filters  
                //open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
                //if (open.ShowDialog() == DialogResult.OK)
                //{
                //    // display image in picture box  
                //    pictureBox1.Image = new Bitmap(open.FileName);
                //    // image file path  
                //    textBox1.Text = open.FileName;
                //}

                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.DefaultExt = "png";
                openFileDialog1.ShowDialog();

            });

            SendCommand = new RelayCommand((sender) =>
            {

            });
        }
    }
}
