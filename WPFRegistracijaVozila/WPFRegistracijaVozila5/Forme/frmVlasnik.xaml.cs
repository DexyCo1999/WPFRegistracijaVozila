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
    /// Interaction logic for frmVlasnik.xaml
    /// </summary>
    public partial class frmVlasnik : Window
    {
        public SqlConnection konekcija = Konekcija.KreirajKonekciju();
        public frmVlasnik()
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

                    string upit = @"Update tblVlasnik 
                            Set ImeVlas='" + txtImeVlasnika.Text + "', PrezimeVlasnika='" + txtPrezimeVlasnika.Text + "', JMBGVlasnika='" + txtJMBGVlasnika.Text + "', AdresaVlasnika='" + txtAdresaVlasnika.Text + "', GradVlasnika='" + txtGradVlasnika.Text + "', KontaktVlasnika='" + txtKontaktVlasnika.Text + "' Where VlasnikID=" + red["ID"];

                    SqlCommand komanda = new SqlCommand(upit, konekcija);
                    komanda.ExecuteNonQuery();
                    MainWindow.selektovan = null;
                    this.Close();
                }

                else { 

                string insert = @"insert into tblVlasnik values('" + txtImeVlasnika.Text + "','"  
                    + txtPrezimeVlasnika.Text + "','" + txtJMBGVlasnika.Text + "','" 
                    + txtAdresaVlasnika.Text + "','"+txtGradVlasnika.Text+"','" + txtKontaktVlasnika.Text + "');";
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
