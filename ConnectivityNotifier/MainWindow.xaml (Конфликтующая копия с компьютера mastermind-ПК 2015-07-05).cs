using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace ConnectivityNotifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isInternet = false;
        bool prevState = false;
        public MainWindow()
        {
            InitializeComponent();
            Update();
        }

        private async void Update()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.sin);
            

            await Dispatcher.BeginInvoke(new Action(async delegate()
            {
                //for (;; Thread.Sleep(1000))
                for (; ; await Task.Delay(1))
                {
                    if (PingStatus("8.8.8.8"))
                        isInternet = true;
                    else
                        isInternet = false;

                    if (isInternet)
                    {
                        if (prevState == false)
                            player.PlayLooping();

                        prevState = true;
                        Label_State.Content = "=)";
                    }                    
                    else
                    {
                        player.Stop();

                        prevState = false;
                        Label_State.Content = "=(";
                    }                    
                }
            }));
        }


        bool PingStatus(string host)
        {
            System.Net.NetworkInformation.Ping ping =
            new System.Net.NetworkInformation.Ping();
            
            try
            {
                if (ping.Send(host).Status == IPStatus.Success)
                    return true;
            }
            catch
            {
                return false;
            }

            return false;
        }
    }
}
