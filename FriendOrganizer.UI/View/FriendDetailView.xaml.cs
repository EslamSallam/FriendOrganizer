using FriendOrganizer.Model;
using System.Windows;
using System.Windows.Controls;

namespace FriendOrganizer.UI.View
{
    /// <summary>
    /// Interaction logic for FriendDetailView.xaml
    /// </summary>
    public partial class FriendDetailView : UserControl
    {
        public FriendDetailView()
        {
            InitializeComponent();
        }

        private void ChkProgrammingLanguages_Checked(object sender, RoutedEventArgs e)
        {

            PrepareProgrammingLanguages();
        }

        private void ChkProgrammingLanguages_Unchecked(object sender, RoutedEventArgs e)
        {
            var s = (CheckBox)sender;
            string languageName = s.Content.ToString();
            
            PrepareProgrammingLanguages();
        }

        public void PrepareProgrammingLanguages()
        {
            cmb.Text = "";

            var list = cmb.ItemsSource;
            foreach (FriendProgrammingLanguages item in list)
            {
                if (item.IsChecked)
                {
                    if (cmb.Text != "")
                    {
                        cmb.Text += ", ";
                    }
                    cmb.Text += item.DisplayMember.ToString();
                }
            }
        }
   
    }
}
