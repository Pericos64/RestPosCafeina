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
using System.Windows.Threading;


using System.Globalization;
using System.Resources;
namespace RestPOS
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// // Code Design By Tuaha - DynamicSoft
    /// citkar@live.com
    /// </summary>
    public partial class Home : Window
    {
        ResourceManager res_man;
        CultureInfo cul; 
        public Home()
        {
            InitializeComponent();
           // lblUsername.Text = UserInfo.UserName;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            { 

                if (UserInfo.usertype == "2")
                {
                   // btnStock.IsEnabled = false;
                   // btnReports.IsEnabled = false;
                    btnUsers.IsEnabled = false;
                    btnSettings.IsEnabled = false;
                    btnKitdisply.IsEnabled = false;
                    
                }
                if (UserInfo.usertype == "3")
                {
                    btnStock.IsEnabled = false;
                    btnReports.IsEnabled = false;
                    btnUsers.IsEnabled = false;
                    btnSettings.IsEnabled = false;
                    btnKitdisply.IsEnabled = false;
                    
                }                
                switch_language();                              
            }
            catch
            {
            }
        } 

        private void switch_language()
        {
            res_man = new ResourceManager("RestPOS.Resource.Res", typeof(Home).Assembly);
            if (language.ID == "1")
            {
                cul = CultureInfo.CreateSpecificCulture(language.languagecode);
                txtsrlink.Text = res_man.GetString("txtsrlink", cul);
                txtKitdisply.Text = res_man.GetString("txtKitdisply", cul);
                txtCustomers.Text = res_man.GetString("txtCustomers", cul);
                txtStock.Text = res_man.GetString("txtStock", cul);
                txtUsers.Text = res_man.GetString("txtUsers", cul);
                txtReports.Text = res_man.GetString("txtReports", cul);
                txtReturn.Text = res_man.GetString("txtReturn", cul);
                txtSettings.Text = res_man.GetString("txtSettings", cul);
                txtLogout.Text = res_man.GetString("txtLogout", cul);

                lbllanguagecode.Text = language.languagecodevalue;
            }
            else
            {
                // englishToolStripMenuItem.Checked = true;
            }

        }

        private void btnSalesRegister_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            //if (UserInfo.DefaulfSRWindow == "2")
            //{
                //Sales_Register.SalesRegisterPink go = new Sales_Register.SalesRegisterPink();
                //go.Show();
            //}
            //else
            //{
                Sales_Register.SalesRegister go = new Sales_Register.SalesRegister();
                go.Show();
            //}      
        }

        private void btnStock_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            Items.Stock_List go = new Items.Stock_List();
            go.Show();         
        }

        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            Reports.SalesReportsDetails go = new Reports.SalesReportsDetails();
            go.Show();
        }

        private void btnKitdisply_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden; 
            Items.Kitchen_Display go = new Items.Kitchen_Display();
            go.ShowDialog();
            
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden; 
            User_mgt.User_list go = new User_mgt.User_list();
            go.Show();
        }

        private void btnCustomers_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden; 
            Customer.CustomerList go = new Customer.CustomerList();
            go.Show();
          //  this.Hide();

        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;  
            Settings.Config go = new Settings.Config();
            go.Show();
           // this.Hide();

        }

        private void Others_Click(object sender, RoutedEventArgs e) //Return
        {
            this.Visibility = Visibility.Hidden;
            Sales_Register.Return_product go = new Sales_Register.Return_product();
            go.Show();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            workRecords();
            this.Visibility = Visibility.Hidden;
            //Login go = new Login();
            //go.Show();

            Logout go = new Logout();
            go.AllowsTransparency = false;
            //go.WindowStyle = WindowStyle.SingleBorderWindow;
            go.Topmost = true;
            go.Show();

            // System.Windows.Application.Current.Shutdown();
        }

        public void workRecords()
        {
            string logdate = DateTime.Now.ToString("yyyy-MM-dd");
            string logtime = DateTime.Now.ToString("HH:mm:ss");
            string logdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            UserInfo.logDateTimeOUT = logdatetime;
            string sventas = vatdisvalue.ventas;
            double Ventas = string.IsNullOrEmpty(sventas) ? 0.00 : Convert.ToDouble(sventas); 

            string sqlLogIn = " insert into tbl_workrecords (Username, datatype, logdate, logtime, logdatetime, ventas, descripcion) " +
                                 " values ('" + UserInfo.UserName + "' , 'OUT' , '" + logdate + "' , " +
                                  " '" + logtime + "' , '" + logdatetime + "', " + Ventas + ", 'Fondo Final' )";
            DataAccess.ExecuteSQL(sqlLogIn);
        }

    }
}
