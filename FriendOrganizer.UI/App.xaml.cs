using Autofac;
using FriendOrganizer.UI.Startup;
using System.Windows;
using System.Configuration;
using System;

namespace FriendOrganizer.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();
            var MainWindow = container.Resolve<MainWindow>();
            MainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unexpected error occured." + Environment.NewLine + e.Exception.Message);
            e.Handled = true;
        }
    }
}
