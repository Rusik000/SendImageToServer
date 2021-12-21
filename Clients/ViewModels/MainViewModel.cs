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
                //OpenFileDialog openFile = new OpenFileDialog();
                //openFile.Filter = "Txt File (*.txt)|*.txt";
                //openFile.Title = "Select File";

                MessageBox.Show("Salam");
                
            });

            SendCommand = new RelayCommand((sender) =>
            {

            });
        }
    }
}
