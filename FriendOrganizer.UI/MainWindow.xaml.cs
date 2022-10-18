using FriendOrganizer.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FriendOrganizer.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly MainViewModel MViewModel;

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            this.MViewModel = viewModel;
            this.DataContext = MViewModel;
            SetF1CommandBinding();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
           await MViewModel.LoadAsync();  
        }

        private void SetF1CommandBinding()
        {
            CommandBinding helpBinding = new CommandBinding(ApplicationCommands.Help);
            helpBinding.CanExecute += CanHelpExecute;
            helpBinding.Executed += HelpExecuted;
            CommandBindings.Add(helpBinding);
        }

        private void CanHelpExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // Here, you can set CanExecute to false if you want to prevent the command from executing.
             e.CanExecute = true;
        }
        private void HelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Look, it is not that difficult. Just type something!", "Help!");
        }
       
    }
}
