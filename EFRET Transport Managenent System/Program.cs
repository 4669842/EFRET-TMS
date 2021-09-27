using System;
using System.Linq;
using System.Windows.Forms;

namespace efretTMS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //We do a check to see if this is the most recent version running in a channel.
            // Subscribed to Dev channel will have development builds/experiemential most recent versions.
            // Subscribed to Live channel have long term support and is the most extenstively tested.
            versionCheck();
            Application.Run(new RadForm1());
        }
        static void versionCheck() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Updater());
        }
    }
}