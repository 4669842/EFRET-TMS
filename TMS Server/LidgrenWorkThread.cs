using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TMS_Server
{
    class LidgrenWorkThread
    {
        ServerNetworking Communication = new ServerNetworking();
        public void SyncComps()
        {
            while (true)
            {
                Communication.Receive();
                Thread.Sleep(1);
            }
        }
    }
}
