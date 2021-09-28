﻿using Lidgren.Network;
using System;
using System.Threading;

namespace TMS_Server
{
    class Program
    {
        static LidgrenWorkThread workerObject;
        static Thread workerThread;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            bool startServer = true;
            int ScreenConfiguration = 2;

            if (args.Length > 0)
            {
                if (args[0] == "1" || args[0] == "3")
                {
                    startServer = false;
                    ScreenConfiguration = Convert.ToInt32(args[0]);
                }
            }

            if (startServer)
            {
                //Create Thread
                workerObject = new LidgrenWorkThread();
                workerThread = new Thread(workerObject.SyncComps);
                //Start Thread
                workerThread.Start();             
            }


            #region use elsewhere
            //Loop until worker thread active
            //while (!workerThread.IsAlive) ;

            //put the main thread to sleep to allow worker thread to do some work
            //Thread.Sleep(1);

            //stop worker thread
            //workerObject.RequestStop();

            //use join method
            //workerThread.Join();
            #endregion
        }

        static void game_Exiting(object sender, EventArgs e)
        {
            if (workerObject != null)
            {
                workerObject.RequestStop();
                workerThread.Abort();
            }
        }
    }
}
