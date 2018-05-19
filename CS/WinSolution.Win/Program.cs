using System;
using System.Configuration;
using System.Windows.Forms;
using DevExpress.ExpressApp.Security;
using WinSolution.Module;

namespace WinSolution.Win {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;
            WinSolutionWindowsFormsApplication _application = new WinSolutionWindowsFormsApplication();
            _application.ConnectionString = CodeCentralExampleDataStoreProvider.ConnectionString;
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null) {
                _application.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
            try {
                DevExpress.ExpressApp.InMemoryDataStoreProvider.Register();
                                _application.ConnectionString = DevExpress.ExpressApp.InMemoryDataStoreProvider.ConnectionString;
                _application.Setup();
                _application.Start();
            } catch (Exception e) {
                _application.HandleException(e);
            }
        }
    }
}
