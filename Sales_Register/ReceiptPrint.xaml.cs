using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

using System.Globalization;
using System.Resources;
using System.Printing;

namespace RestPOS.Sales_Register
{
    /// <summary>
    /// Interaction logic for ReceiptPrint.xaml
    /// </summary>
    public partial class ReceiptPrint : Window
    {
        ResourceManager res_man;
        CultureInfo cul; 
        public ReceiptPrint(string ReceiptNo)
        {
            InitializeComponent();
            lblInvoNo.Text = ReceiptNo;
        }

       
        public void topheader()
        {
            string sql3 = "select * from tbl_terminallocation where shopid = '" + UserInfo.Shopid + "'"; //'MTQC02' "; // 
            DataAccess.ExecuteSQL(sql3);
            DataTable dt1 = DataAccess.GetDataTable(sql3);

            DateTime dt = DateTime.Now;          
            lblCompanyName.Text = dt1.Rows[0].ItemArray[1].ToString();
            //lblBranch.Text       = dt1.Rows[0].ItemArray[2].ToString();
            lblAddress.Text     = dt1.Rows[0].ItemArray[3].ToString();
            lblContact.Text     = dt1.Rows[0].ItemArray[4].ToString();
            //  lblEmail.Text       = dt1.Rows[0].ItemArray[5].ToString();
            lblWebsite.Text = "www.cafeteriacafeina.com"; //dt1.Rows[0].ItemArray[6].ToString();
            parameter.footermsg = dt1.Rows[0].ItemArray[11].ToString();

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\STORELOGO\\" + dt1.Rows[0].ItemArray[12].ToString();
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

        public void topinfoTicket()
        {   /*
            string sqlinfo = "select sales_id,  sales_time, ordertime, tableno, tokenno  from sales_payment  where  sales_id = '" + lblInvoNo.Text + "' ";
            DataAccess.ExecuteSQL(sqlinfo);
            DataTable dtinfo = DataAccess.GetDataTable(sqlinfo);

            lblTicketno.Text        = dtinfo.Rows[0].ItemArray[0].ToString();
            lblDateTicket.Text      = dtinfo.Rows[0].ItemArray[1].ToString();
            lblTimeTicket.Text      = dtinfo.Rows[0].ItemArray[2].ToString();
            lblTableNo.Text         = dtinfo.Rows[0].ItemArray[3].ToString();
            lblTokenno.Text         = dtinfo.Rows[0].ItemArray[4].ToString();

            ///// SQLite   -- Enable/uncomment/open below section if you use SQLite Database and Block Mysql section
           string sqlTikitem = "  select  '- ' || Qty || '  ' || itemName  || '\nSc:' || note  as 'Items'   from sales_item   where sales_id = '" + lblInvoNo.Text + "' ";

            DataAccess.ExecuteSQL(sqlTikitem);
            DataTable dtTikitem = DataAccess.GetDataTable(sqlTikitem);
            dtgrdticketitem.ItemsSource = dtTikitem.DefaultView;            
            */
        }

        private void ReceiptForm_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string currencysign = parameter.currencysign;
                topheader();

                string sql = " select   " +
                                " CASE     " +
                                " WHEN taxapply = 1 THEN  'TX ' || itemname " +
                                " ELSE   itemname  " +
                                " END 'Items'  " +
                                " , qty ,  retailsprice as Price ,  '" + currencysign + "' || total as Total  " +
                                "   from sales_item " +
                                " where (sales_id = '" + lblInvoNo.Text + "')";

                DataAccess.ExecuteSQL(sql);
                DataTable dt1 = DataAccess.GetDataTable(sql);
                dtgriditems.ItemsSource = dt1.DefaultView;

                string sql3 = "select SUM(total)   from sales_item  where sales_id = '" + lblInvoNo.Text + "'";
                DataAccess.ExecuteSQL(sql3);
                DataTable dt3 = DataAccess.GetDataTable(sql3);

                string sql6 = "select * from sales_payment  where (sales_id = '" + lblInvoNo.Text + "')";
                DataAccess.ExecuteSQL(sql6);
                DataTable dt6 = DataAccess.GetDataTable(sql6);


                DataRow dr = dt1.NewRow();
                dr[0] = "";                
                dt1.Rows.Add(dr);

                DataRow Total = dt1.NewRow();
                Total[0] = "Sub Total : ";
                Total[3] =  currencysign  + dt3.Rows[0].ItemArray[0].ToString();
                dt1.Rows.Add(Total);

                DataRow dis = dt1.NewRow();
                dis[0] = "Descuento : " + dt6.Rows[0].ItemArray[13].ToString()  + " %";
                dis[3] = currencysign + dt6.Rows[0].ItemArray[5].ToString();
                dt1.Rows.Add(dis);

                ////////////////////////  General TAX =  ONE TAX  ////////////////////////  START
                DataRow dr0 = dt1.NewRow();
                dr0[0] = "I.V.A : " + dt6.Rows[0].ItemArray[14].ToString() + " %";
                dr0[3] = currencysign + dt6.Rows[0].ItemArray[6].ToString();
                dt1.Rows.Add(dr0);
                ////////////////////////  General TAX =  ONE TAX  ////////////////////////  END

                DataRow dr2 = dt1.NewRow();
                dr2[0] = "TOTAL :  ";
                dr2[3] = currencysign + dt6.Rows[0].ItemArray[2].ToString();
                dt1.Rows.Add(dr2);

                DataRow drLine = dt1.NewRow();
                drLine[0] = "";
                dt1.Rows.Add(drLine);

                double paidamount = (Convert.ToDouble(dt6.Rows[0].ItemArray[2].ToString()) + Convert.ToDouble(dt6.Rows[0].ItemArray[3].ToString())) - Convert.ToDouble(dt6.Rows[0].ItemArray[4].ToString());
                DataRow dr4 = dt1.NewRow();
                dr4[0] =  dt6.Rows[0].ItemArray[1].ToString();
                dr4[3] = currencysign + paidamount.ToString();
                dt1.Rows.Add(dr4);

                DataRow dr5 = dt1.NewRow();
                dr5[0] = "Cambio : ";
                dr5[3] = currencysign + dt6.Rows[0].ItemArray[3].ToString();
                dt1.Rows.Add(dr5);

                DataRow due = dt1.NewRow();
                due[0] = "Debe : ";
                due[3] = currencysign + dt6.Rows[0].ItemArray[4].ToString();
                dt1.Rows.Add(due);

                DataRow dr6 = dt1.NewRow();
                dr6[0] = "";
                dt1.Rows.Add(dr6);

                lblInvoNo.Text = dt6.Rows[0].ItemArray[0].ToString();
                lbltoknNo.Text = dt6.Rows[0].ItemArray[17].ToString();
                lblservedby.Text = dt6.Rows[0].ItemArray[9].ToString();
                lblPrintDate.Text = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss tt");
                
                topinfoTicket();
                switch_language();
            }
            catch
            {
            }
        }


        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            Autoprint();
        }

        
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //// if 1 from SR it's back to SR
            if (parameter.autoprint == "1")
            {
                this.Visibility = Visibility.Hidden;
                    Sales_Register.SalesRegister go = new Sales_Register.SalesRegister();
                    go.Show();
                //}   
            }
            else // if open from Report page it's close 
            {
                this.Close();
            }
         
        }


        // ************************ IMPRESION POS INICIA
        public void Autoprint()
        {
            try
            {
                string printer = "ImpresoraCaja";  

                string ticket = BuildTicketText();

                RawPrinterHelper.SendStringToPrinter(printer, ticket);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string BuildTicketText()
        {
            StringBuilder ticket = new StringBuilder();

            ticket.AppendLine("      CAFETERIA CAFEINA");
            ticket.AppendLine(" www.cafeteriacafeina.com");
            ticket.AppendLine("------------------------------");

            ticket.AppendLine("Ticket: " + lblInvoNo.Text);
            ticket.AppendLine("Fecha : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

            ticket.AppendLine("------------------------------");

            foreach (DataRowView row in dtgriditems.ItemsSource as DataView)
            {
                string item = row["Items"].ToString();
                string qty = row["qty"].ToString();
                string total = row["Total"].ToString();

                ticket.AppendLine(qty + "  " + item);
                ticket.AppendLine("      " + total);
            }

            ticket.AppendLine("------------------------------");
            ticket.AppendLine("GRACIAS POR SU VISITA");
            ticket.AppendLine("\n\n\n");

            return ticket.ToString();
        }

        // ************************ IMPRESION POS TERMINA


        private void switch_language()
        {
            res_man = new ResourceManager("RestPOS.Resource.Res", typeof(Home).Assembly);
            if (language.ID == "1")
            {   /*
                cul = CultureInfo.CreateSpecificCulture(language.languagecode);
                btnPrint.Content = res_man.GetString("btnPrint", cul);
                lbltokenPNTtitle.Text = res_man.GetString("lbltokenPNTtitle", cul);
                lblinvoicePNTtile.Text = res_man.GetString("lblinvoicePNTtile", cul);
                lblsvrPNTtitle.Text = res_man.GetString("lblsvrPNTtitle", cul);
                lblticketPNTtitle.Text = res_man.GetString("lblticketPNTtitle", cul);
                lbltimePNTtitle.Text = res_man.GetString("lbltimePNTtitle", cul);
                lbltablePNTtitle.Text = res_man.GetString("lbltablePNTtitle", cul);
                lblinvoiceNoPNTtitle.Text = res_man.GetString("lblinvoiceNoPNTtitle", cul);

                lbldatePNTtitle.Text = res_man.GetString("lbldatetitle", cul);
                lbltakennoPNTtitle.Text = res_man.GetString("lbltokonnoSRtitle", cul);
                btnPrintTicket.Content = res_man.GetString("btnPrint", cul);
                btnClose.Content = res_man.GetString("btnHomeMenuLink", cul);
                lbltakennoPNTtitle.Text = res_man.GetString("lbltokonnoSRtitle", cul);
                */
            }
            else
            {
                // englishToolStripMenuItem.Checked = true;
            }
        }

        private void Dtgriditems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
