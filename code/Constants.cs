using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlideShow.TCP_IP.WinForm
{
    class Constants
    {
        internal const double SLIDE_SHOW__INTERVAL_TIME = 3000;

        internal const string SLIDE_SHOW__COMMAND__STOP = "2";
        internal const string SLIDE_SHOW__COMMAND__PAUSE_PLAY = "1";

        internal const int NETWORK_LISTENER_PORT = 8888;
        internal const string NETWORK_COMMUNICATION_TERMINATOR = "$";
    }
}
