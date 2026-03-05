using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for PrintTicket.xaml
    /// </summary>
    public partial class PrintTicket : Window
    {
        ResourceManager res_man;
        CultureInfo cul; 
        public PrintTicket()
        {
            InitializeComponent();
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void printticketForm_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string sqlinfo =    " select sales_id,  salesdate, ordertime, tableno, tokenno " +                                 
                                    "  from tbl_hold_sales_item  where sales_id = '" + parameter.holdsalesid + "' ";
                DataAccess.ExecuteSQL(sqlinfo);
                DataTable dtinfo = DataAccess.GetDataTable(sqlinfo);

                lblTicketno.Text   = dtinfo.Rows[0].ItemArray[0].ToString();
                lblDateTicket.Text = dtinfo.Rows[0].ItemArray[1].ToString();
                lblTimeTicket.Text = dtinfo.Rows[0].ItemArray[2].ToString();
                lblTableNo.Text    = dtinfo.Rows[0].ItemArray[3].ToString();
                lblTokenno.Text    = dtinfo.Rows[0].ItemArray[4].ToString();

                /*
                string sqlTikitem = "  select  '- ' || o.qty || '  ' || o.itemname  || '\nSc:' || o.options  as 'Items' " +
                                    "   from tbl_hold_sales_item o inner join purchase p  on o.product_id = p.product_id " +
                                    "   where o.sales_id = '" + parameter.holdsalesid + "'  and  p.grupo = 'Comidas'";

                DataAccess.ExecuteSQL(sqlTikitem);
                DataTable dtTikitem = DataAccess.GetDataTable(sqlTikitem);
                dtgrdticketitem.ItemsSource = dtTikitem.DefaultView;

                switch_language();
                */
            }
            catch
            {

            }
        }


        //***************************

        private void btnPrintTicket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ImprimeTicket("Bebidas", "ImpresoraBebidas", "Ticket Bebidas");
                ImprimeTicket("Comidas", "ImpresoraCocina", "Ticket Comidas");

                this.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void ImprimeTicket(string grupo, string nombreImpresora, string titulo)
        {
            try
            {
                string sqlTikitem = " select  '- ' || o.qty || '  ' || o.itemname  || '\nSc:' || o.options  as 'Items' " +
                                    " from tbl_hold_sales_item o inner join purchase p  on o.product_id = p.product_id " +
                                    " where o.sales_id = '" + parameter.holdsalesid + "' and p.grupo = '" + grupo + "'";

                DataTable dtTikitem = DataAccess.GetDataTable(sqlTikitem);

                if (dtTikitem.Rows.Count == 0)
                    return;

                dtgrdticketitem.ItemsSource = dtTikitem.DefaultView;

                lblticketPNTtitle.Text = titulo;

                dtgrdticketitem.Items.Refresh();
                grdTicketPrintPanel.UpdateLayout();

                // FORZAR RENDER DEL DATAGRID
                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render,
                    new Action(() => { }));


                PrintDialog printDlg = new PrintDialog();

                using (LocalPrintServer ps = new LocalPrintServer())
                {
                    PrintQueue queue = ps.GetPrintQueue(nombreImpresora);
                    printDlg.PrintQueue = queue;
                }

                Size pageSize = new Size(printDlg.PrintableAreaWidth, printDlg.PrintableAreaHeight);

                grdTicketPrintPanel.Measure(pageSize);
                grdTicketPrintPanel.Arrange(new Rect(0, 0, pageSize.Width, pageSize.Height));
                grdTicketPrintPanel.UpdateLayout();

                printDlg.PrintVisual(grdTicketPrintPanel, "Ticket_" + grupo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al imprimir: " + ex.Message);
            }
        }


        private void PrintVisualAsBitmap(PrintDialog printDialog, Visual visual)
        {
            double width = printDialog.PrintableAreaWidth;
            double height = printDialog.PrintableAreaHeight;

            Size size = new Size(width, height);

            if (visual is FrameworkElement element)
            {
                element.Measure(size);
                element.Arrange(new Rect(size));
                element.UpdateLayout();
            }

            RenderTargetBitmap rtb = new RenderTargetBitmap(
                (int)width,
                (int)height,
                96,
                96,
                PixelFormats.Pbgra32);

            rtb.Render(visual);

            Image img = new Image();
            img.Source = rtb;

            printDialog.PrintVisual(img, "Ticket ");
        }

        // *****************************************



        public void updatetablereservada()  // Actuliza producto de la orden como ya ordenado y/o impreso
        {
            string sqlUpdateCmd = " update tbl_hold_sales_item set ordenado=1 where sales_id = '" + parameter.holdsalesid + "' ";
            DataAccess.ExecuteSQL(sqlUpdateCmd);
        }


        private void switch_language()
        {
            res_man = new ResourceManager("RestPOS.Resource.Res", typeof(Home).Assembly);
            if (language.ID == "1")
            {
                cul = CultureInfo.CreateSpecificCulture(language.languagecode);
                lblticketPNTtitle.Text = res_man.GetString("lblticketPNTtitle", cul);
                lbltimePNTtitle.Text = res_man.GetString("lbltimePNTtitle", cul);
                lbltablePNTtitle.Text = res_man.GetString("lbltablePNTtitle", cul);
                lblinvoiceNoPNTtitle.Text = res_man.GetString("lblinvoiceNoPNTtitle", cul);

                lbldatePNTtitle.Text = res_man.GetString("lbldatetitle", cul);
                lbltakennoPNTtitle.Text = res_man.GetString("lbltokonnoSRtitle", cul);
                btnPrintTicket.Content = res_man.GetString("btnPrint", cul);
            }
            else
            {
                // englishToolStripMenuItem.Checked = true;
            }
        }


    }
}
