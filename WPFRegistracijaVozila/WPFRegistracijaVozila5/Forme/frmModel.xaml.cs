using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace WPFRegistracijaVozila5.Forme
{
    /// <summary>
    /// Interaction logic for frmModel.xaml
    /// </summary>
    public partial class frmModel : Window
    {
        public SqlConnection konekcija = Konekcija.KreirajKonekciju();
        public frmModel()
        {
            InitializeComponent();
        }

        private void BtnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();

                if (MainWindow.azuriraj)
                {
                    DataRowView red = (DataRowView)MainWindow.selektovan;
                    //OBRATI PAZNJU!!!!!!! 
                    string upit = @"Updata tblModel Set NazivModela='" + txtNazivModela.Text + "' Where ModelID=" + red["ID"];
                    SqlCommand komanda = new SqlCommand(upit, konekcija);
                    komanda.ExecuteNonQuery();
                    MainWindow.selektovan = null;
                    this.Close();
                }
                else
                {                   
                    string insert = @"insert into tblModel values('" + txtNazivModela.Text + "');";
                    SqlCommand cmd = new SqlCommand(insert, konekcija);
                    cmd.ExecuteNonQuery();
                    this.Close();
                }                                                                                                  

            }
            catch (SqlException) 
            {
                MessageBox.Show("Unos određenih podataka nije validan!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }

        }

        private void BtnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
