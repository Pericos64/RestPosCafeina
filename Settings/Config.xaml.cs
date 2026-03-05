using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Globalization;
using System.Resources;

namespace RestPOS.Settings
{
    /// <summary>
    /// Interaction logic for Config.xaml
    /// </summary>
    public partial class Config : Window
    {
        OpenFileDialog openFileDialog1 = new OpenFileDialog();

        ResourceManager res_man;
        CultureInfo cul; 
        public Config()
        {
            InitializeComponent();
        }

        #region data_bind terminal
        public void terminallist()
        {
            string sqlterminallist = "select shopid as 'ID', branchname, location, phone ,  " +
                                     " vat as 'TAX %', dis as 'Discount %'    from tbl_terminallocation";
            DataAccess.ExecuteSQL(sqlterminallist);
            DataTable dtterminallist = DataAccess.GetDataTable(sqlterminallist);
            dtgridTerminallist.ItemsSource = dtterminallist.DefaultView;
            dtgridTerminallist.Columns[0].Width = 100; // dtgridTerminallist.ActualWidth - 10; // ID
            dtgridTerminallist.Columns[1].Width = 200; //Branch
            dtgridTerminallist.Columns[3].Width = 160; //Phone
            dtgridTerminallist.Columns[4].Width = 100; // VAT
            dtgridTerminallist.Columns[5].Width = 100; //Discount
        }

        public void terminaldetails()
        {
            string sqlterminaldetails = " Select shopid, branchname, location, phone, email, web, vat, dis, vatregino, " +
                                      "   footermsg, companyname, imagename, languagecode, currencysign, contrasenia from tbl_terminalLocation " +
                                      "   where shopid = '" + lblShopID.Text + "' ";
            DataAccess.ExecuteSQL(sqlterminaldetails);
            DataTable dtterminaldetails = DataAccess.GetDataTable(sqlterminaldetails);

            lblShopID.Text          = dtterminaldetails.Rows[0].ItemArray[0].ToString();
            txtterminalname.Text    = dtterminaldetails.Rows[0].ItemArray[1].ToString();
            txtTerminaladdress.Text = dtterminaldetails.Rows[0].ItemArray[2].ToString();
            txtTerminalPhone.Text   = dtterminaldetails.Rows[0].ItemArray[3].ToString();
            txtTremail.Text         = dtterminaldetails.Rows[0].ItemArray[4].ToString();
            txtTrweb.Text           = dtterminaldetails.Rows[0].ItemArray[5].ToString();
            txtTrVAT.Text           = dtterminaldetails.Rows[0].ItemArray[6].ToString();
            txtTrDis.Text           = dtterminaldetails.Rows[0].ItemArray[7].ToString();
            txtTrVATregino.Text     = dtterminaldetails.Rows[0].ItemArray[8].ToString();
            txtTrFootermsg.Text     = dtterminaldetails.Rows[0].ItemArray[9].ToString();
            txtCompanyName.Text     = dtterminaldetails.Rows[0].ItemArray[10].ToString();
            lblimagename.Text       = dtterminaldetails.Rows[0].ItemArray[11].ToString();
            Cmbolanguagecode.Text   = dtterminaldetails.Rows[0].ItemArray[12].ToString();
            txtcurrencysign.Text    = dtterminaldetails.Rows[0].ItemArray[13].ToString();
            txtContrasenia.Text     = dtterminaldetails.Rows[0].ItemArray[14].ToString();
            // tabcontrolpanel.SelectedItem = tabTerminallist;
            btnAddnew.Visibility = Visibility.Visible;
            btndelete.Visibility = Visibility.Visible;
          //  lbltrmsg.Visibility = Visibility.Visible;
         //   btnSave.Content = "Update";


            string path = AppDomain.CurrentDomain.BaseDirectory + "\\STORELOGO\\" + dtterminaldetails.Rows[0].ItemArray[11].ToString();
            if (!File.Exists(path))
            {
                piclogo.Source = null;
            }
            else
            {
                piclogo.Source = BitmapFromUri(new Uri(path));
            }
        }

        public static ImageSource BitmapFromUri(Uri source)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = source;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }

        private void ConfigForm_Loaded(object sender, RoutedEventArgs e)
        {
              try
              {
                  terminallist();
                  btnAddnew.Visibility = Visibility.Hidden;
                  btndelete.Visibility = Visibility.Hidden;
                  lbltrmsg.Visibility = Visibility.Hidden;
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
                tabTerminallist.Header          = res_man.GetString("tabTerminallist", cul);
                tabTerminalDetailsview.Header   = res_man.GetString("tabTerminalDetailsview", cul);
                tabTablezone.Header             = res_man.GetString("tabTablezone", cul);
                tabDataManager.Header           = res_man.GetString("tabDataManager", cul);

                lblcomnametile.Text         = res_man.GetString("lblcomnametile", cul);
                lblbrchnametile.Text        = res_man.GetString("lblbrchnametile", cul);
                lblemailtile.Text           = res_man.GetString("lblemailtitle", cul);
                lbllocationaddresstile.Text = res_man.GetString("lbllocationaddresstile", cul);
                lblphonetile.Text           = res_man.GetString("lblcontacttitle", cul);
                lblwebtitle.Text            = res_man.GetString("lblwebtitle", cul);
                lbltaxreginotile.Text       = res_man.GetString("lbltaxreginotile", cul);
                lbltaxtile.Text             = res_man.GetString("lbltaxtile", cul);
                lbldistile.Text             = res_man.GetString("lbldistile", cul);
                lblfootertile.Text          = res_man.GetString("lblfootertile", cul);
                lbllanguagecodetitle.Text   = res_man.GetString("lbllanguagecodetitle", cul);
                lblcurrencysigntitle.Text   = res_man.GetString("lblcurrencysigntitle", cul); 

                btnSave.Content = res_man.GetString("btnSave", cul);
                btnAddnew.Content = res_man.GetString("btnAddnew", cul);
                btndelete.Content = res_man.GetString("btnDelete", cul);
                btnBrowse.Content = res_man.GetString("btnBrowse", cul);
            }
            else
            {
                // englishToolStripMenuItem.Checked = true;
            }
        }

        private void dtgridTerminallist_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
               
                DataRowView dataRow = (DataRowView)dtgridTerminallist.SelectedItem;
                int index = dtgridTerminallist.CurrentCell.Column.DisplayIndex;
                lblShopID.Text = dataRow.Row.ItemArray[0].ToString();
                tabcontrolpanel.SelectedItem = tabTerminalDetailsview;
                //tabTerminalDetailsview.Header = "Details View";
                terminaldetails();
                lbltrmsg.Visibility = Visibility.Hidden;
            }
            catch
            {

            }

        }
        #endregion

        #region Save Edit delete Terminal data
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {

            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;

            openFileDialog1.DefaultExt = ".png";
            // openFileDialog1.Filter = "GIF files (*.gif)|*.gif| jpg files (*.jpg)|*.jpg| PNG files (*.png)|*.png| All files (*.*)|*.*";
            openFileDialog1.Filter = "PNG files (*.png)|*.png| PNG files (*.PNG)|*.PNG";

            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;



            if (openFileDialog1.ShowDialog() == true)
            { 
                piclogo.Source = new BitmapImage(new Uri(openFileDialog1.FileName));
                // //// textBox1.Text = openFileDialog1.FileName;
                // //picItemimage.Source = openFileDialog1.FileName;
                lblFileExtension.Text = System.IO.Path.GetExtension(openFileDialog1.FileName);
            }


        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtterminalname.Text == "" || txtTerminaladdress.Text == "" || 
                    txtTerminalPhone.Text == "" || txtTrVAT.Text == "" || txtTrDis.Text == "" || txtContrasenia.Text == "")
                {
                    MessageBox.Show("Please fill Terminal info", "Fill", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    String guid = Guid.NewGuid().ToString();
                    //Add new Terminal Info
                    if (lblShopID.Text == "-")
                    {
                        string imageName = guid + lblFileExtension.Text;
                        string Shopid = txtterminalname.Text.Substring(0, 2) + txtTrVATregino.Text.Substring(0, 2);
                        string sqlinsert =  " insert into tbl_terminallocation  (shopid, companyname, branchname, location, " +
                                            " phone, email, web, vat, dis, vatregino, footermsg, imagename,languagecode, currencysign, contrasenia ) " +
                                            " values ('" + Shopid + "', '" + txtCompanyName.Text + "', '" + txtterminalname.Text + "', " +
                                            " '" + txtTerminaladdress.Text + "', '" + txtTerminalPhone.Text + "', '" + txtTremail.Text + "', " +
                                            " '" + txtTrweb.Text + "', '" + txtTrVAT.Text + "', '" + txtTrDis.Text + "', '" + txtTrVATregino.Text + "', " +
                                            " '" + txtTrFootermsg.Text + "', '" + imageName + "', '" + Cmbolanguagecode.Text + "', '" + 
                                            txtcurrencysign.Text.Trim() + "', '" + txtContrasenia.Text.Trim() +  "' )";
                        DataAccess.ExecuteSQL(sqlinsert);

                        ///// picture  upload - insert  /////////////////      
                        if (openFileDialog1.FileName != string.Empty )
                        {
                            string destinatiopath = System.AppDomain.CurrentDomain.BaseDirectory + @"\STORELOGO\";
                            string iName = openFileDialog1.SafeFileName;  // only file name
                            string filepath = openFileDialog1.FileName;   // Filename with Source path
                            System.IO.File.Copy(filepath, destinatiopath + imageName);
                        }
                        lbltrmsg.Visibility = Visibility.Visible;
                        lbltrmsg.Content = "Submitted a new Terminal";
                        
                        terminallist();
                        tabcontrolpanel.SelectedItem = tabTerminallist;
                    }
                    else // Update selected 
                    {
                        string imageName;
                        if (lblFileExtension.Text == "logo.png")
                        {
                            imageName = lblimagename.Text;
                        }
                        else
                        {
                            imageName = guid + lblFileExtension.Text;
                        }

                        string sql = "update tbl_terminallocation set branchname = '" + txtterminalname.Text + "', location = '" + txtTerminaladdress.Text + "', " +
                        " email = '" + txtTremail.Text + "', phone = '" + txtTerminalPhone.Text + "', vat = '" + txtTrVAT.Text + "', web = '" + txtTrweb.Text + "', " +
                        " dis = '" + txtTrDis.Text + "', vatregiNo = '" + txtTrVATregino.Text + "', footermsg = '" + txtTrFootermsg.Text + "',   " +
                        " companyName = '" + txtCompanyName.Text + "', imagename = '" + imageName + "' , languagecode = '" + Cmbolanguagecode.Text + "', " +
                        " currencysign = '" + txtcurrencysign.Text.Trim() + "',  contrasenia = '" + txtContrasenia.Text.Trim() + "' " +
                        " where shopid = '" + lblShopID.Text + "' ";
                        DataAccess.ExecuteSQL(sql);


                        ///// picture upload - update /////////////////      
                        if (openFileDialog1.FileName != string.Empty)
                        {
                            string destinatiopath = System.AppDomain.CurrentDomain.BaseDirectory + @"\STORELOGO\";
                            string iName = openFileDialog1.SafeFileName;
                            string filepath = openFileDialog1.FileName;
                            System.IO.File.Copy(filepath, destinatiopath + imageName);
                        }

                        lbltrmsg.Visibility = Visibility.Visible;
                        lbltrmsg.Content = "Terminal info has been Saved";                        
                        terminallist();
                        tabcontrolpanel.SelectedItem = tabTerminallist;
                    }
                }

            }
            catch
            {
            }
        }

        private void btnAddnew_Click(object sender, RoutedEventArgs e)
        {
            txtCompanyName.Text = string.Empty;
            txtterminalname.Text = string.Empty;
            txtTerminaladdress.Text = string.Empty;
            txtTerminalPhone.Text = string.Empty;
            txtTremail.Text = string.Empty;
            txtTrVATregino.Text = string.Empty;
            txtTrweb.Text       = string.Empty;
            txtTrDis.Text = string.Empty;
            txtTrVAT.Text = string.Empty;
            txtTrFootermsg.Text = string.Empty;
            lblShopID.Text = "-";
            piclogo.Source = null;
            btnSave.Content = "Add new";
            tabTerminalDetailsview.Header = "Add new";
        }
        
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

            if (lblShopID.Text == "-")
            {
                // MessageBox.Show("You are Not able to Update");
                MessageBox.Show("You are Not able to Delete", "Button3 Title", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                try
                {
                    if (MessageBox.Show("Do you want to Delete?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        string sql = "delete from tbl_terminalLocation where shopid ='" + lblShopID.Text + "'";
                        DataAccess.ExecuteSQL(sql);
                        piclogo.Source = null;
                        MessageBox.Show("Successfully Data Delete !", "Successful", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        terminallist();
                        tabcontrolpanel.SelectedItem = tabTerminallist;
                    }
      


                }
                catch (Exception exp)
                {
                    MessageBox.Show("Sorry\r\n You have to Check the Data" + exp.Message);
                }
            }
        }
        #endregion

        #region texbox validation
        private void txtTrVAT_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9.-]+");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch
            { }
        }

        private void txtTrDis_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9.-]+");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch
            { }
        }
        #endregion
        private void btnHomeMenuLink_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            Home go = new Home();
            go.Show();

        }

        private void DataManager_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
