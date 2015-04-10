using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SlideShow.TCP_IP.WinForm.Business;
using SlideShow.TCP_IP.WinForm.Concrete;

namespace SlideShow.TCP_IP.WinForm
{
    public partial class Form1 : Form
    {
        private Thread _SlideShowThread;
        private SSlideShow _SlideShowForm;
        private readonly ImageBusiness _ImageBusiness;
        private TcpListener _Server;
        private TcpClient _Client;


        public Form1()
        {
            InitializeComponent();
            _ImageBusiness = new ImageBusiness();
            Init();
        }

        private void Init()
        {
            Thread networkListener = new Thread(new ThreadStart(StartNetworkListener));
            networkListener.IsBackground = true;
            networkListener.Start();
        }


        private void StartNetworkListener()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            IPAddress localHostIP = IPAddress.Parse(localIP);

            #region check to see if our port is in use or not.
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
            {
                if (tcpi.LocalEndPoint.Port == Constants.NETWORK_LISTENER_PORT)
                {
                    MessageBox.Show("can't listen for remote control commands.... the port is already in use by something else.");
                    return;
                }
            }
            #endregion

            _Server = new TcpListener(localHostIP, Constants.NETWORK_LISTENER_PORT);

            _Client = default(TcpClient);
            _Server.Start();

            while (true)
            {
                try
                {
                    _Client = _Server.AcceptTcpClient();
                    NetworkStream networkStream = _Client.GetStream();
                    byte[] bytesFromClient = new byte[10025];
                    networkStream.Read(bytesFromClient, 0, (int)_Client.ReceiveBufferSize);
                    string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFromClient);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf(Constants.NETWORK_COMMUNICATION_TERMINATOR));
                    switch (dataFromClient)
                    {

                        case Constants.SLIDE_SHOW__COMMAND__PAUSE_PLAY:
                            if (_SlideShowThread == null || _SlideShowThread.ThreadState == ThreadState.Stopped)
                            {
                                _SlideShowThread = new Thread(() => StartSlideShow());
                                _SlideShowThread.Start();
                            }
                            else
                            {
                                _SlideShowForm.PausePlaySlideShow();
                            }
                            break;
                        case Constants.SLIDE_SHOW__COMMAND__STOP:
                            _SlideShowForm.StopSlideShow();
                            break;
                    }
                    _Client.Close();
                }
                catch
                {
                    if (_Client != null)
                    {
                        _Client.Close();
                    }
                    if (_Server != null)
                    {
                        _Server.Stop();
                    }
                }
            }
        }


        private void StartSlideShow()
        {
            if (_SlideShowForm != null)
            {
                _SlideShowForm.Stop();
            }
            IList<Image> images = _ImageBusiness.GetImagesForSlideShow();
            Application.Run(_SlideShowForm = new SSlideShow(images));
        }
    }
}
