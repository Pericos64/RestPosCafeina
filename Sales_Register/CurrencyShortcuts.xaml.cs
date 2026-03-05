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

namespace RestPOS.Sales_Register
{
    /// <summary>
    /// Interaction logic for CurrencyShortcuts.xaml
    /// </summary>
    public partial class CurrencyShortcuts : UserControl
    {
        public CurrencyShortcuts()
        {
            InitializeComponent();
        }
        public Delegate CoinandNotesFunctionPointer;
        public Delegate NumaricKeypad;        

        #region  //Shortcut Keypad Event

        private void btnNum1_Click(object sender, RoutedEventArgs e)
        {
           
            //if (btnNum1.Background != Brushes.Blue )
            //{
                
            //    btnNum1.Background = Brushes.Blue;
            //}
            //else
            //{
            //    btnNum1.Background = Brushes.Red;
               
            //}

            NumaricKeypad.DynamicInvoke("1");
        }

        private void btnNum2_Click(object sender, RoutedEventArgs e)
        {
            NumaricKeypad.DynamicInvoke("2");
        }

        private void btnNum3_Click(object sender, RoutedEventArgs e)
        {
            NumaricKeypad.DynamicInvoke("3");
        }

        private void btnNum4_Click(object sender, RoutedEventArgs e)
        {
            NumaricKeypad.DynamicInvoke("4");
        }

        private void btnNum5_Click(object sender, RoutedEventArgs e)
        {
            NumaricKeypad.DynamicInvoke("5");
        }

        private void btnNum6_Click(object sender, RoutedEventArgs e)
        {
            NumaricKeypad.DynamicInvoke("6");
        }

        private void btnNum7_Click(object sender, RoutedEventArgs e)
        {
            NumaricKeypad.DynamicInvoke("7");
        }

        private void btnNum8_Click(object sender, RoutedEventArgs e)
        {
            NumaricKeypad.DynamicInvoke("8");
        }

        private void btnNum9_Click(object sender, RoutedEventArgs e)
        {
            NumaricKeypad.DynamicInvoke("9");
        }

        private void btnDecimal_Click(object sender, RoutedEventArgs e)
        {
            NumaricKeypad.DynamicInvoke(".");
        }

        private void btnNum0_Click(object sender, RoutedEventArgs e)
        {
            NumaricKeypad.DynamicInvoke("0");
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            CoinandNotesFunctionPointer.DynamicInvoke("XX"); 
        }

        private void btnBackSpace_Click(object sender, RoutedEventArgs e)
        {
            CoinandNotesFunctionPointer.DynamicInvoke("BXC");
        }
        // Coin and Notes
        private void btnCoin1_Click(object sender, RoutedEventArgs e)
        {
            CoinandNotesFunctionPointer.DynamicInvoke("10");

        }

        private void btnCoin2_Click(object sender, RoutedEventArgs e)
        {
            CoinandNotesFunctionPointer.DynamicInvoke("20");
        }

        private void btnNote5_Click(object sender, RoutedEventArgs e)
        {
            CoinandNotesFunctionPointer.DynamicInvoke("50");
        }

        private void btnNote10_Click(object sender, RoutedEventArgs e)
        {
            CoinandNotesFunctionPointer.DynamicInvoke("100");
        }

        private void btnNote20_Click(object sender, RoutedEventArgs e)
        {
            CoinandNotesFunctionPointer.DynamicInvoke("200");
        }

        private void btnNote50_Click(object sender, RoutedEventArgs e)
        {
            CoinandNotesFunctionPointer.DynamicInvoke("500");
        }

        private void btnNote100_Click(object sender, RoutedEventArgs e)
        {
            CoinandNotesFunctionPointer.DynamicInvoke("1000");
        }

        #endregion

    }
}
