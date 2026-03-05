using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace RestPOS.Help
{
    /// <summary>
    /// Interaction logic for HelpIndex.xaml
    /// </summary>
    public partial class HelpIndex : Window
    {
        public HelpIndex()
        {
            InitializeComponent();
        }

        private void OnNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }


        private void btnHomeMenuLink_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
          //  Home go = new Home();
          //  go.Show();

        }

    }
}
