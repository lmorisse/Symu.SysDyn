using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using System.Windows.Forms;
using SymuSysDynApp.Properties;

namespace SymuSysDynApp
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Get you own SyncFusionKey
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Settings.Default.SyncFusionKey);
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Home());
        }
    }
}
