using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
 

namespace RestPOS.Sales_Register
{
    /// <summary>
    /// Interaction logic for Payment.xaml
    /// </summary>
    public partial class Payment : Window
    {
        private const double topOffset = 40;
        private const double leftOffset = 380;
        readonly GrowlNotifiactions growlNotifications = new GrowlNotifiactions();
        
        public Payment(object dataSource, string total, string subtotal, string TotalAmount, string discount, string vat,   string VatRate,  string totalitems)
        {
            InitializeComponent();
            growlNotifications.Top = SystemParameters.WorkArea.Top + topOffset;
            growlNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - leftOffset;

          dgrvSalesItemList.DataContext = dataSource;
          lblTotalPayable.Text = TotalAmount;
          lblTotal.Text = total;
          lblsubtotal.Text = subtotal;
          lblTotalPayable.Text = TotalAmount;
          lblTotalDisCount.Text = discount;
          lbloveralldiscount.Text = discount;
          lblTotalVAT.Text = vat;
        //  txtDiscountRate.Text = DiscountRate;
          txtVATRate.Text = VatRate;
          //txtPaidAmount.Text = "0.00";
         // txtInvoice.Text = invoiceNo;
          lblTotalItems.Text = totalitems;
          lbluser.Text = UserInfo.UserName;
          txtPaidAmount.Focus();
         // this.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(HandleEsc);

          numformFunctionPointer += new numvaluefunctioncall(NumaricKeypad);
          currency_ShortcutsContorl.NumaricKeypad = numformFunctionPointer;
        }
        public delegate void numvaluefunctioncall(string Numvalue);        
        private event numvaluefunctioncall numformFunctionPointer;


        private void NumaricKeypad(string Numvalue)
        {
            txtPaidAmount.Text += Numvalue;
            txtPaidAmount.Focus();
        }
        private void HandleEsc(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
        
        private void Payment1_Loaded(object sender, RoutedEventArgs e)
        {
          
         //   lbluser.Text = date; //UserInfo.UserName;
            try
            {
                //Customer Info
                string sqlCust = "select   DISTINCT  *   from tbl_customer where PeopleType = 'Customer'";
                DataAccess.ExecuteSQL(sqlCust);
                DataTable dtCust = DataAccess.GetDataTable(sqlCust);
                txtCustName.ItemsSource = dtCust.DefaultView;
                txtCustName.DisplayMemberPath = "Name";
                txtCustName.SelectedValuePath = "ID";
                txtCustName.Text = "Guest"; 

                DispatcherTimer invoiceautoupdate = new DispatcherTimer();
                invoiceautoupdate.Tick += new EventHandler(invoiceautoupdate_Tick);
                invoiceautoupdate.Interval = new TimeSpan(0, 0, 0);
                invoiceautoupdate.Start();

                CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
                ci.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
                Thread.CurrentThread.CurrentCulture = ci; 
                dtSalesDate.SelectedDate = DateTime.Today;
                tokennumberget();
            }
            catch
            {
            }
        }

        //// Invoice Synchronization from Database
        public void invoiceautoupdate_Tick(object sender, EventArgs e)
        {
            try
            {
                //Invoice id auto update
                string sql = "select  sales_id  from sales_payment order by sales_id desc";
                DataTable dt = DataAccess.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    double id = Convert.ToDouble(dt.Rows[0].ItemArray[0].ToString()) + 1;
                    txtInvoice.Text = Convert.ToString(Convert.ToInt32(id));
                }
                else
                {
                    double id = 1;
                    txtInvoice.Text = Convert.ToString(Convert.ToInt32(id));
                }
              
            }
            catch
            {

            }
        }

        public void tokennumberget()
        {
            string sqltk = "select  tokenno, sales_date  from tbl_tokenno order by id desc , tokenno desc ";
            DataTable dttk = DataAccess.GetDataTable(sqltk);
            if (dttk.Rows.Count > 0)
            {
                if (dttk.Rows[0].ItemArray[1].ToString() == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    double idtk = Convert.ToDouble(dttk.Rows[0].ItemArray[0].ToString()) + 1;
                    txttokenno.Text = Convert.ToString(Convert.ToInt32(idtk));
                }

            }
            else
            {
                double idtk = 1;
                txttokenno.Text = Convert.ToString(Convert.ToInt32(idtk));
            } 
        }
         
        /// //// Add sales item  ////////////Store into sales_item table //////////         
        public bool sales_item()
        {
            DataTable dt = new DataTable();
            dt = ((DataView)dgrvSalesItemList.ItemsSource).ToTable();
            int rows = dgrvSalesItemList.Items.Count;
            for (int i = 0; i < rows; i++)
            {
                string SalesDate        = dtSalesDate.Text;
                string trno             = txtInvoice.Text;
                string itemid           = dt.Rows[i].ItemArray[4].ToString();  //dgrvSalesItemList.Rows[i].Cells[4].Value.ToString();
                string itNam            = dt.Rows[i][0].ToString(); // dt.Rows[i].Cells[0].Value.ToString();
                double qty              = Convert.ToDouble(dt.Rows[i][2].ToString());
                double Rprice           = Convert.ToDouble(dt.Rows[i][1].ToString());
                double total            = Convert.ToDouble(dt.Rows[i][3].ToString());
                double dis              = Convert.ToDouble(dt.Rows[i][7].ToString()); //discount rate
                double taxapply         = Convert.ToDouble(dt.Rows[i][8].ToString());
                int kitchendisplay      = Convert.ToInt32(dt.Rows[i][9].ToString());
                string notes            = dt.Rows[i].ItemArray[10].ToString(); 
                

                // =================================Start=====  Profit calculation =============== Start ========= 
                // Discount_amount = (Retail_price * discount) / 100                    -- 49 * 3 / 100 = 1.47
                // Retail_priceAfterDiscount = Retail_price - Discount_amount           -- 49 - 1.47 = 47.53
                // Profit = (Retail_priceAfterDiscount * QTY )   - (cost_price * qty);  ---( 47.53 * 1 ) - ( 45 * 1) = 2.53

                string sqlprofit = "Select cost_price , discount  from  purchase  where product_id  = '" + itemid + "'";
                DataAccess.ExecuteSQL(sqlprofit);
                DataTable dt1 = DataAccess.GetDataTable(sqlprofit);

                double cost_price = Convert.ToDouble(dt1.Rows[0].ItemArray[0].ToString());
                double discount = Convert.ToDouble(dt1.Rows[0].ItemArray[1].ToString());

                double Discount_amount = (Rprice * discount) / 100.00;
                double Retail_priceAfterDiscount = Rprice - Discount_amount;
                double Profit = Math.Round((Retail_priceAfterDiscount - cost_price), 2); // old calculation (Retail_priceAfterDiscount * qty) - (cost_price * qty);
                // =================================Start=====  Profit calculation =============== Start =========  


                string sql1 = " insert into sales_item (sales_id,itemName,Qty,RetailsPrice,Total, profit,sales_time, itemcode , discount, taxapply, note, status) " +
                              " values ('" + trno + "', '" + itNam + "', '" + qty + "', '" + Rprice + "', '" + total + "', '" + Profit + "', " +
                              " '" + SalesDate + "','" + itemid + "','" + dis + "','" + taxapply + "','"+ notes +"','" + kitchendisplay + "')";
                DataAccess.ExecuteSQL(sql1);

                //update quantity Decrease from Stock Qty |  purchase Table
                if (txtInvoice.Text == "")
                {
                    MessageBox.Show("please check sales no ");
                }
                else
                {

                    string itemids = dt.Rows[i][4].ToString();  
                    double qtyupdate = Convert.ToDouble(dt.Rows[i][2].ToString());

                    // Update Quantity
                    string sqlupdateQty = "select product_quantity  from purchase where product_id = '" + itemids + "'";
                    DataAccess.ExecuteSQL(sqlupdateQty);
                    DataTable dtUqty = DataAccess.GetDataTable(sqlupdateQty);
                    double product_quantity = Convert.ToDouble(dtUqty.Rows[0].ItemArray[0].ToString()) - qtyupdate;

                    string sql = " update purchase set " +
                                    " product_quantity = '" + product_quantity + "' " +
                                    " where product_id = '" + itemids + "' ";
                    DataAccess.ExecuteSQL(sql);
                }

            }
            return true;

        }

        /// //// Payment items Add  ///////////Store into Sales_payment table //////// 
        public void payment_item()
        {
            string trno             = txtInvoice.Text;
            string payamount        = lblTotalPayable.Text;
            string changeamount     = txtChangeAmount.Text;
            string due              = txtDueAmount.Text;
            string vat              = lblTotalVAT.Text;
            string DiscountTotal    = lbloveralldiscount.Text; //lblTotalDisCount.Text;
            string Comment          = txtCustName.Text + " " + txtcomment.Text;
            string overalldisRate   = txtDiscountRate.Text;
            string vatRate          = txtVATRate.Text;
            string ordertime        = DateTime.Now.ToString("HH:mm");
            string tableno          = cmboTableNo.Text;
            string tokenno          = txttokenno.Text;

            string sql1 = " insert into sales_payment (sales_id, payment_type, payment_amount, change_amount, due_amount, dis, vat, " +
                            " sales_time, c_id, emp_id, comment, TrxType, Shopid , ovdisrate , vaterate, ordertime , tableno, tokenno ) " +
                            "  values ('" + txtInvoice.Text + "','" + CombPayby.Text + "', '" + payamount + "', '" + changeamount + "', " +
                            " '" + due + "', '" + DiscountTotal + "', '" + vat + "', '" + dtSalesDate.Text + "', '" + lblCustID.Text + "', " +
                            " '"+ lbluser.Text +"','" + Comment + "','POS','MTQC02' , '" + overalldisRate + "' , '" + vatRate + "', " + 
                            " '" + ordertime + "', '" + tableno + "', '" + tokenno + "' )";
            DataAccess.ExecuteSQL(sql1);
        }

        /// //// Token no Add  ///////////Store into tbl_tokenno table //////// 
        public void tokennoInsert()
        {
            string trno = txtInvoice.Text;
            string payamount = lblTotalPayable.Text;             
            string tokenno = txttokenno.Text;

            string sqltkn = " insert into tbl_tokenno (sales_id, tokenno, sales_date) " +
                            "  values ('" + txtInvoice.Text + "','" + tokenno + "',   '" + dtSalesDate.Text + "' )";
            DataAccess.ExecuteSQL(sqltkn);
        }

        private void btnPayment_Click(object sender, RoutedEventArgs e)
        {          
                if (txtPaidAmount.Text == "00" || txtPaidAmount.Text == "0" || txtPaidAmount.Text == string.Empty)
                {
                  //  MessageBox.Show("Please insert paid amount", "Yes or No", MessageBoxButton.OK, MessageBoxImage.Warning);
                    growlNotifications.AddNotification(new Notification { Title = "Alert Message", Message = "Please insert paid amount", ImageUrl = "pack://application:,,,/Notifications/Radiation_warning_symbol.png" });
                    txtPaidAmount.Focus();
                }
                //else if (Convert.ToInt32(txtInvoice.Text) >= 120)
                //{
                //    growlNotifications.AddNotification(new Notification { Title = "Alert Message", Message = "Sorry ! Demo version has limited transaction   Please buy it   contact at : citkar@live.com ", ImageUrl = "pack://application:,,,/Notifications/Radiation_warning_symbol.png" });                   
                //    MessageBox.Show("Sorry ! Demo version has limited transaction \n Please buy it \n contact at : citkar@live.com ", "Yes or No", MessageBoxButton.OK, MessageBoxImage.Warning);
                //}
                else
                {
                    try
                    {
                        if (MessageBox.Show("Do you want to Complete this transaction?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            //Save payment info into sales_payment table
                            payment_item();

                            ///// save single item 
                            sales_item();

                            // TokennoInsert
                            tokennoInsert();

                            this.Visibility = Visibility.Hidden;
                            btnCompleteSalesAndPrint.IsEnabled = false;
                            //btnSaveOnly.Content = "Done";
                            //btnSaveOnly.IsEnabled = false;

                            ///// // Open Print Invoice
                            parameter.autoprint = "1";
                            Sales_Register.ReceiptPrint go = new Sales_Register.ReceiptPrint(txtInvoice.Text);
                            go.ShowDialog();             

                        }
  
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show(exp.Message);
                    }
                 
                }  
        }


        private void txtPaidAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lblTotalPayable.Text == "")
            {
                // MessageBox.Show("please insert Amount ");
            }
            else
            {
                try
                {
                    if (Convert.ToDouble(txtPaidAmount.Text) >= Convert.ToDouble(lblTotalPayable.Text))
                    {
                        double changeAmt = Convert.ToDouble(txtPaidAmount.Text) - Convert.ToDouble(lblTotalPayable.Text);
                        changeAmt = Math.Round(changeAmt, 2);
                        txtChangeAmount.Text = changeAmt.ToString();
                        txtDueAmount.Text = "0";
                    }
                    if (Convert.ToDouble(txtPaidAmount.Text) <= Convert.ToDouble(lblTotalPayable.Text))
                    {
                        double changeAmt = Convert.ToDouble(lblTotalPayable.Text) - Convert.ToDouble(txtPaidAmount.Text);
                        changeAmt = Math.Round(changeAmt, 2);
                        txtDueAmount.Text = changeAmt.ToString();
                        txtChangeAmount.Text = "0";
                    }

                }
                catch //(Exception exp)
                {
                    txtPaidAmount.Text = "0";
                    // MessageBox.Show(exp.Message);
                }

            }
        }

        private void txtPaidAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {         
            try
            {
                Regex regex = new Regex("[^0-9.-]+");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch
            { }
           
        }

        private void txtCustName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
               // string sqlCmd = "Select ID from  tbl_customer  where Name  = '" + txtCustName.Text + "'";
              //  DataAccess.ExecuteSQL(sqlCmd);
               // DataTable dt1 = DataAccess.GetDataTable(sqlCmd);
             //   lblCustID.Text = dt1.Rows[0].ItemArray[0].ToString();
                lblCustID.Text = txtCustName.SelectedValue.ToString();
                
            }
            catch
            {
            }
        }

        private void txtDiscountRate_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (lblTotalPayable.Text == "")
                {
                    MessageBox.Show("Please Add at least One Item");
                }
                else
                {
                    double Discountvalue = Convert.ToDouble(txtDiscountRate.Text);
                    txtDiscountRate.Text = Discountvalue.ToString();
                    double subtotal = Convert.ToDouble(lblTotal.Text) - Convert.ToDouble(lblTotalDisCount.Text); // total - item discount  100 - 5 = 95        
                    double totaldiscount = (subtotal * Discountvalue) / 100;  //Counter discount  // 95 * 5 /100 = 4.75  
                    double disPlusOverallDiscount = totaldiscount + Convert.ToDouble(lblTotalDisCount.Text); // 4.75 + 5 = 9.75
                    disPlusOverallDiscount = Math.Round(disPlusOverallDiscount, 2);
                    lbloveralldiscount.Text = disPlusOverallDiscount.ToString();  // Overall discount 9.75

                    double subtotalafteroveralldiscount = subtotal - totaldiscount; // 95 - 4.75 = 90.25
                    subtotalafteroveralldiscount = Math.Round(subtotalafteroveralldiscount, 2);
                    lblsubtotal.Text = subtotalafteroveralldiscount.ToString();

                    double payable = subtotalafteroveralldiscount + Convert.ToDouble(lblTotalVAT.Text);
                    payable = Math.Round(payable, 2);
                    lblTotalPayable.Text = payable.ToString();

                    txtPaidAmount.Text = payable.ToString();

                }
            }
            catch
            {
                txtDiscountRate.Text = "0";
                //lbloveralldiscount.Text = lblTotalDisCount.Text;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            Sales_Register.SalesRegister go = new Sales_Register.SalesRegister();
            go.Show();
        }

 

        #region  //Shortcut Keypad Event
           
            private void btnNum1_Click(object sender, RoutedEventArgs e)
            {                
                txtPaidAmount.Text += "1";                
            }

            private void btnNum2_Click(object sender, RoutedEventArgs e)
            {
                txtPaidAmount.Text += "2";
            }

            private void btnNum3_Click(object sender, RoutedEventArgs e)
            {
                txtPaidAmount.Text += "3";
            }

            private void btnNum4_Click(object sender, RoutedEventArgs e)
            {
                txtPaidAmount.Text += "4";
            }

            private void btnNum5_Click(object sender, RoutedEventArgs e)
            {
                txtPaidAmount.Text += "5";
            }

            private void btnNum6_Click(object sender, RoutedEventArgs e)
            {
                txtPaidAmount.Text += "6";
            }

            private void btnNum7_Click(object sender, RoutedEventArgs e)
            {
                txtPaidAmount.Text += "7";
            }

            private void btnNum8_Click(object sender, RoutedEventArgs e)
            {
                txtPaidAmount.Text += "8";
            }

            private void btnNum9_Click(object sender, RoutedEventArgs e)
            {
                txtPaidAmount.Text += "9";
            }

            private void btnDecimal_Click(object sender, RoutedEventArgs e)
            {
                txtPaidAmount.Text += ".";
            }

            private void btnNum0_Click(object sender, RoutedEventArgs e)
            {
                txtPaidAmount.Text += "0";
            }

            private void btnClear_Click(object sender, RoutedEventArgs e)
            {
                txtPaidAmount.Text = "";
              //  txtPaidAmount.Text = txtPaidAmount.Text.Substring(0, txtPaidAmount.Text.Length - 1);
            }

        // Coin and Notes
            private void btnCoin1_Click(object sender, RoutedEventArgs e)
            {
                if (string.IsNullOrEmpty(txtPaidAmount.Text))
                {
                    txtPaidAmount.Text = "0.00";
                    txtPaidAmount.Text = (Convert.ToDouble(txtPaidAmount.Text) + Convert.ToDouble("10.00")).ToString();
                }
                else
                {
                    txtPaidAmount.Text = (Convert.ToDouble(txtPaidAmount.Text) + Convert.ToDouble("10.00")).ToString();
                }
               
            }

            private void btnCoin2_Click(object sender, RoutedEventArgs e)
            {
                if (string.IsNullOrEmpty(txtPaidAmount.Text))
                {
                    txtPaidAmount.Text = "0.00";
                    txtPaidAmount.Text = (Convert.ToDouble(txtPaidAmount.Text) + Convert.ToDouble("20.00")).ToString();
                }
                else
                {
                    txtPaidAmount.Text = (Convert.ToDouble(txtPaidAmount.Text) + Convert.ToDouble("20.00")).ToString();
                }
            }

            private void btnNote5_Click(object sender, RoutedEventArgs e)
            {
                if (string.IsNullOrEmpty(txtPaidAmount.Text))
                {
                    txtPaidAmount.Text = "0.00";
                    txtPaidAmount.Text = (Convert.ToDouble(txtPaidAmount.Text) + Convert.ToDouble("50.00")).ToString();
                }
                else
                {
                    txtPaidAmount.Text = (Convert.ToDouble(txtPaidAmount.Text) + Convert.ToDouble("50.00")).ToString();
                }
            }

            private void btnNote10_Click(object sender, RoutedEventArgs e)
            {
                if (string.IsNullOrEmpty(txtPaidAmount.Text))
                {
                    txtPaidAmount.Text = "0.00";
                    txtPaidAmount.Text = (Convert.ToDouble(txtPaidAmount.Text) + Convert.ToDouble("100.00")).ToString();
                }
                else
                {
                    txtPaidAmount.Text = (Convert.ToDouble(txtPaidAmount.Text) + Convert.ToDouble("100.00")).ToString();
                }
            }

            private void btnNote20_Click(object sender, RoutedEventArgs e)
            {
                if (string.IsNullOrEmpty(txtPaidAmount.Text))
                {
                    txtPaidAmount.Text = "0.00";
                    txtPaidAmount.Text = (Convert.ToDouble(txtPaidAmount.Text) + Convert.ToDouble("200.00")).ToString();
                }
                else
                {
                    txtPaidAmount.Text = (Convert.ToDouble(txtPaidAmount.Text) + Convert.ToDouble("200.00")).ToString();
                }
            }

            private void btnNote50_Click(object sender, RoutedEventArgs e)
            {
                if (string.IsNullOrEmpty(txtPaidAmount.Text))
                {
                    txtPaidAmount.Text = "0.00";
                    txtPaidAmount.Text = (Convert.ToDouble(txtPaidAmount.Text) + Convert.ToDouble("500.00")).ToString();
                }
                else
                {
                    txtPaidAmount.Text = (Convert.ToDouble(txtPaidAmount.Text) + Convert.ToDouble("500.00")).ToString();
                }
            }

            private void btnNote100_Click(object sender, RoutedEventArgs e)
            {
                if (string.IsNullOrEmpty(txtPaidAmount.Text))
                {
                    txtPaidAmount.Text = "0.00";
                    txtPaidAmount.Text = (Convert.ToDouble(txtPaidAmount.Text) + Convert.ToDouble("1000.00")).ToString();
                }
                else
                {
                    txtPaidAmount.Text = (Convert.ToDouble(txtPaidAmount.Text) + Convert.ToDouble("1000.00")).ToString();
                }
            }

        #endregion

 
    }
}
