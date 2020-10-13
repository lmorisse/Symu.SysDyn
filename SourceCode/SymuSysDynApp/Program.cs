#region Licence

// Description: SymuSysDyn - SymuSysDynApp
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Windows.Forms;
using SymuSysDynApp.Properties;
using Syncfusion.Licensing;

#endregion

namespace SymuSysDynApp
{
    internal static class Program
    {
        /// <summary>
        ///     Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // Get you own SyncFusionKey
            SyncfusionLicenseProvider.RegisterLicense(Settings.Default.SyncFusionKey);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Home());
        }
    }
}