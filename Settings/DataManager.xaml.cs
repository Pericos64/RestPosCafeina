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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

using System.Globalization;
using System.Resources;

namespace RestPOS.Settings
{
    /// <summary>
    /// Interaction logic for DataManager.xaml
    /// </summary>
    public partial class DataManager : UserControl
    {
        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        OpenFileDialog openFileDialog1 = new OpenFileDialog();

        ResourceManager res_man;
        CultureInfo cul; 
        public DataManager()
        {
            InitializeComponent();
            switch_language();
        }

        //// Save Databackup As
        private void btnDataSaveas_Click(object sender, RoutedEventArgs e)
        {
            try
            {            
              
                string sourceFileName = "RestPOS.db";
                string destFileFileName = "RestPOSBackup_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".db";
                saveFileDialog1.FileName = destFileFileName;  
                saveFileDialog1.ShowDialog();
                string sourcePath = System.AppDomain.CurrentDomain.BaseDirectory ;

                // Get file name and dest path             
                string targetPath = saveFileDialog1.FileName;

                //   string targetPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);  

                //// Use Path class to manipulate file and directory paths. 
                string sourceFile = System.IO.Path.Combine(sourcePath, sourceFileName);
                string destFile = System.IO.Path.Combine(targetPath, destFileFileName);



                // To copy a folder's contents to a new location: 
                // Create a new target folder, if necessary. 
                if (!System.IO.Directory.Exists(targetPath))
                {
                    System.IO.Directory.CreateDirectory(targetPath);

                }

                System.IO.File.Copy(sourceFile, destFile, true);


                //File.Copy(sourceFile, destFileFileName);

            }
            catch
            {
            }
        }

        //// Reset system
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                if (MessageBox.Show("Realmente desea limpiar la base de datos ? \n Se perderan todos los datos de Operación", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    if(rdbtnSqlite.IsChecked == true)
                    {
                        string sql1 =   " DELETE FROM return_item; " +
                                        " UPDATE tbl_tablezone SET fillcolor = 'Green', ordertime = ''; " +
                                        " DELETE FROM tbl_expense; " +
                                        " DELETE FROM tbl_saleinfo; " +
                                        " DELETE FROM tbl_custcredit; " +
                                        " DELETE FROM tbl_tokenno; " +
                                        " UPDATE SQLITE_SEQUENCE SET seq = 0 WHERE name = 'tbl_tokenno'; " +
                                        " DELETE FROM tbl_workrecords; " +
                                        " UPDATE SQLITE_SEQUENCE SET seq = 0 WHERE name = 'tbl_workrecords'; " +
                                        " DELETE FROM sales_item; " +
                                        " UPDATE SQLITE_SEQUENCE SET seq = 0 WHERE name = 'sales_item'; " +
                                        " DELETE FROM sales_payment; " +
                                        " UPDATE SQLITE_SEQUENCE SET seq = 0 WHERE name = 'sales_payment'; " +
                                        " DELETE FROM tbl_hold_sales_item; " +
                                        " UPDATE SQLITE_SEQUENCE SET seq = 0 WHERE name = 'tbl_hold_sales_item'; " +
                                        " DELETE FROM tbl_duepayment; " +
                                        " UPDATE SQLITE_SEQUENCE SET seq = 0 WHERE name = 'tbl_duepayment'; ";
                        //DataTable dt1 = DataAccess.GetDataTable(sql1);
                        DataAccess.ExecuteSQL(sql1);
                    } else {
                        string sql1 = " truncate table  return_item; " + 
                                  " DELETE FROM usermgt where usertype != '1'; " +
                                  "  truncate table  sales_item; " + 
                                  "  truncate table  sales_payment; " + 
                                  "  truncate table  purchase; " + 
                                  "  truncate table  tbl_hold_sales_item; " + 
                                  "  truncate table  tbl_duepayment;  ";
                        DataTable dt1 = DataAccess.GetDataTable(sql1);
                    }
                  
                    MessageBox.Show("BD ha sido limpiada Exitosamente!!! ", "Limpieza BD", MessageBoxButton.OK, MessageBoxImage.Warning);              
                    
                }
            }
            catch (Exception exp) {
                MessageBox.Show(exp.Message);
            }
        }
        
        // Select Database file ( .db )
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog1.Title = "Archivos de BD";

            //openFileDialog1.CheckFileExists = true;
            //openFileDialog1.CheckPathExists = true;

            openFileDialog1.DefaultExt = "db";
            openFileDialog1.Filter = "DB files (*.DB)|*.DB| db files (*.db)|*.db";
            //openFileDialog1.Filter = "All files (*.*)|*.* | jpg files (*.jpg)|*.jpg";



            if (openFileDialog1.ShowDialog() == true)
            {
                // textBox1.Text = openFileDialog1.FileName;
                txtfilepath.Text = openFileDialog1.FileName;
            }
        }

        private void btnTruncate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtfilepath.Text == string.Empty)
                {
                    MessageBox.Show("Seleccione un archivo de BD", "Restauración de BD", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtfilepath.Focus();
                }
                else
                { 

                    if (MessageBox.Show("Realmente desea restaurar esta BD ? \n Después de confirmar, se perderan los datos actuales", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        string Newfile = "RestPOS.db";
                        string targetPath = System.AppDomain.CurrentDomain.BaseDirectory;

                        if (!System.IO.File.Exists(targetPath + @"\" + Newfile)) //if not File Exists
                        {
                            System.IO.File.Copy(txtfilepath.Text, targetPath + @"\" + Newfile);
                        }
                        else
                        {
                            // Delete a file by using File class static method...                    
                            System.IO.File.Delete(targetPath + @"\" + Newfile);
                            System.IO.File.Copy(txtfilepath.Text, targetPath + @"\" + Newfile);
                        }

                        MessageBox.Show("La BD ha sido restaurada con éxito", "Restauración Exitosa", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }

                }

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

                lblresettitle.Text = res_man.GetString("lblresettitle", cul);
                lbltruncatedeslabel.Text = res_man.GetString("lbltruncatedeslabel", cul);
                lblrestoretitle.Text = res_man.GetString("lblrestoretitle", cul);
                lblrestoredescription.Text = res_man.GetString("lblrestoredescription", cul);             

                btnDataSaveas.Content = res_man.GetString("btnDataSaveas", cul);
                btnReset.Content = res_man.GetString("btnReset", cul);
                btnTruncate.Content = res_man.GetString("btnTruncate", cul);
                btnBrowse.Content = res_man.GetString("btnBrowse", cul);
            }
            else
            {
                // englishToolStripMenuItem.Checked = true;
            }

        }
    }
}
