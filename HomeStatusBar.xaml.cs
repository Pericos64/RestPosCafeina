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

namespace RestPOS
{
    /// <summary>
    /// Interaction logic for HomeStatusBar.xaml
    /// </summary>
    public partial class HomeStatusBar : UserControl
    {
        public HomeStatusBar()
        {
            InitializeComponent();
        }

       

        private void btnHomeMenuLink_Click_1(object sender, RoutedEventArgs e)
        {
           //s.Visibility = Visibility.Hidden;  
            Home go = new Home();
            go.Show();
        
            // this.Hide();


        }

      

    }
}
