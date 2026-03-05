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
using System.Windows.Shapes;

namespace RestPOS
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class MyInputBox : Window
    {

        public string UserPassword
        {
            get
            {
                if (txtPassword == null) return string.Empty;
                return txtPassword.Password.ToString();
            }
        }

        public MyInputBox()
        {
            InitializeComponent();
        }

        private void BtnInputBox1_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
