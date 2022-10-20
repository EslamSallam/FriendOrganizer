using FriendOrganizer.Model;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void ckabc_Checked_1(object sender, RoutedEventArgs e)
        {

        }



        private void cmb_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            cmb.Text = "";
            var list = cmb.ItemsSource;
            foreach (FriendProgrammingLanguages item in list)
            {
                if (cmb.Text.ToString().Length > 0 && item.IsChecked)
                {
                    cmb.Text += ", ";

                }
                if (item.IsChecked)
                {
                    cmb.Text += item.DisplayMember.ToString();
                }
            }
        }

      

        //private void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //txtLangs.Text = "";
        //    //foreach (FriendProgrammingLanguages item in cmb.ItemsSource)
        //    //{
        //    //    txtLangs.Text += item.DisplayMember.ToString(); 

        //    //}
        //}
    }
}
