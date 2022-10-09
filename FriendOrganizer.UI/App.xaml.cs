using Autofac;
using FriendOrganizer.UI.Startup;
using System.Windows;
using System.Configuration;

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
    }
}
