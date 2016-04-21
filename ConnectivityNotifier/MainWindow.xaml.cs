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
using System.Timers;

namespace ConnectivityNotifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static bool isInternet = false;
        static bool prevState = false;

        //static System.Timers.Timer checkForTime = new System.Timers.Timer(500);
        static System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.sin);
        //static Thread statusUpdatingThread = new Thread(new ThreadStart(StatusUpdater));
        Thread statusUpdatingThread;

        public MainWindow()
        {
            //checkForTime.Enabled = true;
            //checkForTime.Elapsed += new ElapsedEventHandler(checkForTime_Elapsed);

            
            InitializeComponent();

            statusUpdatingThread = new Thread(new ThreadStart(StatusUpdater));

            statusUpdatingThread.IsBackground = true;
            statusUpdatingThread.Start();
        }



        void StatusUpdater()
        {
            while(true)
            {
                
                    if (PingStatus("8.8.8.8") || PingStatus("77.88.8.8"))
                        isInternet = true;
                    else
                        isInternet = false;

                    if (isInternet)
                    {
                        if (prevState == false)
                            player.PlayLooping();

                        prevState = true;

                        Dispatcher.BeginInvoke(new Action(delegate ()
                        {
                            Label_State.Content = "=)";
                        }));
                    }
                    else
                    {
                        player.Stop();

                        prevState = false;

                        Dispatcher.BeginInvoke(new Action(delegate ()
                        {
                            Label_State.Content = "=(";
                        }));
                    }
                

                Thread.Sleep(1000);
            }
        }




        //private void checkForTime_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    Task.Factory.StartNew(StatusUpdater);
        //}




        static bool PingStatus(string host)
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            statusUpdatingThread.Abort();
        }
    }
}
