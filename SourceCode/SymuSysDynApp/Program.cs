using System;
using System.Configuration;
using System.Windows.Forms;
using Syncfusion.Licensing;

namespace Symu.SysDyn.App
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Get you own SyncFusionKey
            SyncfusionLicenseProvider.RegisterLicense(ConfigurationManager.AppSettings.Get("SyncFusionKey"));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Home());
        }
    }
}
