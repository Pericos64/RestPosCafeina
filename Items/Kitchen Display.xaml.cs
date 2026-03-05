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
using System.Windows.Threading; 

namespace RestPOS.Items
{
    /// <summary>
    /// Interaction logic for Kitchen_Display.xaml
    /// </summary>
    public partial class Kitchen_Display : Window
    {
        private const double topOffset = 180;
        private const double leftOffset = 780;
        readonly GrowlNotifiactions growlNotifications = new GrowlNotifiactions();
         
        public Kitchen_Display()
        {
            InitializeComponent();
            growlNotifications.Top = SystemParameters.WorkArea.Top + topOffset;
            growlNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - leftOffset;
           
        }

        private void KitchenDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            try 
            {
                DispatcherTimer autoupdate = new DispatcherTimer();
                autoupdate.Tick += new EventHandler(autoupdate_Tick);
                autoupdate.Interval = new TimeSpan(0, 0, 5);
                autoupdate.Start();

                ItemList_with_images(); 
            }
            catch
            {

            }
  
        }
         
          //// KD  Synchronization 
        private void autoupdate_Tick(object sender, EventArgs e)
        {
            try
            {
                ItemList_with_images();
            }
            catch
            {

            }
        }

        public void  ItemList_with_images()
        {
            var uriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\ITEMIMAGE\\", UriKind.RelativeOrAbsolute);

            ///// SQLite   -- Enable/uncomment/open below section if you use SQLite Database and Block Mysql section
            string sql = " SELECT  si.item_id as id ,   si.sales_id as 'receiptno' ,    si.itemname as 'itemname' ,   si.note as 'note', si.qty ,   si.total , " +
                        "   si.sales_time   as 'date', si.itemcode,   '" + uriSource + "'   || p.imagename as  imagename, 'Srv: '|| sp.emp_id as 'emp_id' , tokenno, " +
                         "  CASE   " +
                         "  WHEN si.status = 3 THEN 'Pending' " +
                         "  WHEN si.status = 1 THEN 'Served'  " +
                         "  END 'status', 'table: ' || sp.tableno as 'tableno', 'Time: ' || sp.ordertime as 'time'  " +
                         "  FROM  sales_item si " +
                         "  left join  sales_payment sp " +
                         "  ON si.sales_id = sp.sales_id " +
                         "  left join purchase p " +
                         " ON p.product_id = si.itemcode " +
                         "  where si.status = 3   and  si.qty != 0 " +
                         "  order by si.item_id asc ";

            // /// MySQL  -- Enable/uncomment/open below section if you use MySQL Database and Block Above section 
            //string sql = " SELECT  si.item_id as id ,   si.sales_id as 'receiptno' ,    si.itemname as 'itemname' ,   si.note as 'note', si.qty ,   si.total , " +
            //       "   si.sales_time   as 'date', si.itemcode, CONCAT('" + uriSource + "', p.imagename)  as  imagename,  CONCAT('Srv: ',  sp.emp_id) as 'emp_id', tokenno, " +
            //        "  CASE   " +
            //        "  WHEN si.status = 3 THEN 'Pending' " +
            //        "  WHEN si.status = 1 THEN 'Served'  " +
            //        "  END 'status', CONCAT('Table: ', sp.tableno) as 'tableno',  CONCAT('Time: ', sp.ordertime) as 'time'  " +
            //        "  FROM  sales_item si " +
            //        "  left join  sales_payment sp " +
            //        "  ON si.sales_id = sp.sales_id " +
            //        "  left join purchase p " +
            //        " ON p.product_id = si.itemcode " +
            //        "  where si.status = 3   and  si.qty != 0 " +
            //        "  order by si.item_id asc ";

            DataAccess.ExecuteSQL(sql);
            DataTable dt1 = DataAccess.GetDataTable(sql);
            lstvwKitchenitem.ItemsSource = dt1.DefaultView;
        }

        private void btnOrderReady_Click(object sender, RoutedEventArgs e)
        {

            //string OrderNO = "";
            //foreach (DataRowView row in lstvwKitchenitem.SelectedItems)
            //{
            //    OrderNO = row.Row[1].ToString();
            //    //DataRowView dataRow = (DataRowView)lstvwKitchenitem.SelectedItem;
            //    //OrderNO = dataRow.Row.ItemArray[1].ToString();
            //}
            // MessageBox.Show(OrderNO.ToString());

            var btn = sender as Button;
            var lvi = FindAncestor<ListViewItem>(btn);
            if (lvi != null)
            {
                lvi.IsSelected = false; //forece re-select if already selected
                lvi.IsSelected = true;
            }
            growlNotifications.AddNotification(new Notification { Title = "Message", ImageUrl = "pack://application:,,,/Notifications/notification-icon.png", Message = "Order has beed Served... " });
        
        }

        //// trigger  SelectionChanged  buttonclick event is trigger for row within Listview 
        private static T FindAncestor<T>(FrameworkElement fe) where T : FrameworkElement
        {
            if (fe == null)
            {
                return null;
            }
            var p = fe.Parent as FrameworkElement ?? fe.TemplatedParent as FrameworkElement;
            if (p == null)
            {
                return null;
            }
            var match = p as T;
            return match ?? FindAncestor<T>(p);
        }

        private void lstvwKitchenitem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string OrderNO = "";
            foreach (DataRowView row in lstvwKitchenitem.SelectedItems)
            {
                OrderNO = row.Row[0].ToString();
            }
           
            string sql = " update sales_item set " +
                           " status = 1 " +
                           " where item_id  = '" + OrderNO + "' ";
            DataAccess.ExecuteSQL(sql);
            DataTable dt1 = DataAccess.GetDataTable(sql);

            ItemList_with_images();
           
          //  MessageBox.Show("Order has beed Served");
        }

        private void btnHomeMenuLink_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            Home go = new Home();
            go.Show();
        }

        private void btnopenKD_Click(object sender, RoutedEventArgs e)
        {
           // this.AllowsTransparency = false;
           // this.WindowStyle = WindowStyle.SingleBorderWindow;
            Items.Kitchen_Display go = new Items.Kitchen_Display();            
            go.AllowsTransparency = false;
            go.WindowStyle = WindowStyle.SingleBorderWindow;
            go.Topmost = true;
            go.Show();
        }

    }
}
