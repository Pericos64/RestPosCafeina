using System;
using System.Data;
using System.Windows;
using Activationconfig;
 
namespace RestPOS
{
    /// <summary>
    /// Interaction logic for Logout.xaml
    /// // Code Design By Pedro Rubio - VitalSoftware
    /// pedro.rubio@vital-software.com
    /// </summary>
    public partial class Logout : Window
    {
        private const double topOffset = 180;
        private const double leftOffset = 780;
        readonly GrowlNotifiactions growlNotifications = new GrowlNotifiactions();
         
        public Logout()
        {
            InitializeComponent();
            growlNotifications.Top = SystemParameters.WorkArea.Top + topOffset;
            growlNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - leftOffset;
            ResulationHW();
            workRecords();
        }

        //Dynamic Resulation
        public void ResulationHW()
        {
            if (System.Windows.SystemParameters.PrimaryScreenWidth == 1920)
            {
                double W = (System.Windows.SystemParameters.PrimaryScreenWidth * 12) / 100;
                double H = (System.Windows.SystemParameters.PrimaryScreenHeight * 16) / 100;
                MainGrid.Margin = new Thickness(W, H, 0, 0);
                Toolbargrid.Margin = new Thickness(0, 20, W, 0);
            }
            else
            {
                double W = (System.Windows.SystemParameters.PrimaryScreenWidth * 1.6) / 100;
                double H = (System.Windows.SystemParameters.PrimaryScreenHeight * 3) / 100;
                MainGrid.Margin = new Thickness(W, H, 0, 0);
            }
        }

        private void btnSignin_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {

                System.Windows.Controls.PrintDialog Printdlg = new System.Windows.Controls.PrintDialog();
                if ((bool)Printdlg.ShowDialog().GetValueOrDefault())
                {
                    Size pageSize = new Size(Printdlg.PrintableAreaWidth, Printdlg.PrintableAreaHeight);
                    // Size pageSize = new Size(340, 194);
                    // sizing of the element.
                    MainGrid.Measure(pageSize);
                    MainGrid.Arrange(new Rect(0, 0, pageSize.Width, pageSize.Height));
                    Printdlg.MaxPage = 10;
                    Printdlg.PrintVisual(MainGrid, "Ticket_Caja_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss"));
                }
                this.Visibility = Visibility.Hidden;
                Login go = new Login();
                go.Show();

            }
            catch //(Exception exe)
            {
                    
            }

        }
        
        public void workRecords()
        {
          
            string tkhan = "SELECT *, (date || ' ' || out) AS 'Turno' FROM vw_workrecords WHERE username =  '" + 
                            UserInfo.UserName + "' ORDER BY Turno DESC LIMIT 1";
            DataTable dt = DataAccess.GetDataTable(tkhan);

            txtUserName.Text = string.Empty;
            txtUserName.Focus();

            txtUserName.Text            = dt.Rows[0].ItemArray[0].ToString();
            txtUserTurno.Text           = dt.Rows[0].ItemArray[1].ToString() + " " + dt.Rows[0].ItemArray[2].ToString() + " " + dt.Rows[0].ItemArray[3].ToString();
            txtUserFondoInicial.Text    = dt.Rows[0].ItemArray[5].ToString();
            txtUserVentas.Text          = dt.Rows[0].ItemArray[6].ToString();
            txtUserFondoFinal.Text      = dt.Rows[0].ItemArray[9].ToString();
            
        }


        private void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            try 
            {
                //expodtclass();
            }
            catch
            {

            }  
        }

        #region not use cl
        public void expodtclass()
        {
            ///////////////// Ex date ////////////////////////////
            if (Convert.ToInt32(InvoicesManager.Activationvalue) > Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"))) // Exp date > Curnt date              
            {
                btnSignin.Visibility = Visibility.Visible;
            }
            else
            {
                lblUserTitle.Text = "Your license has been expired. Contact: prubio.sanchez@gmail.com";
                txtUserName.Visibility          = Visibility.Hidden;
            }
        }

        #endregion

        
    }
}
