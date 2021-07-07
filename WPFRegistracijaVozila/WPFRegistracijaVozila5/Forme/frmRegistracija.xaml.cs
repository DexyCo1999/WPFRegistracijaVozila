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
    /// Interaction logic for frmRegistracija.xaml
    /// </summary>
    public partial class frmRegistracija : Window
    {
        SqlConnection konekcija = Konekcija.KreirajKonekciju();
        public frmRegistracija()
        {
            InitializeComponent();

            try
            {
                konekcija.Open();

                string vratiVozila = "Select VoziloID, BrojSasije from tblVozilo";
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(vratiVozila, konekcija);
                da.Fill(dt);
                cbxVozilo.ItemsSource = dt.DefaultView;

            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
            } 

        }

        private void BtnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                if (MainWindow.azuriraj)
                {
                    DataRowView red = (DataRowView)MainWindow.selektovan;
                    
                    string upit = @"Update tblRegistracija Set BrojTablice='" + txtBrojTablice.Text + "', Cena='" + txtCena.Text + "', Datum ='" + dpDatum.SelectedDate + "', VoziloID=" + cbxVozilo.SelectedValue + ", PosebnaTablica=" + Convert.ToInt32(cbPosebnaTablica.IsChecked) + " Where RegistracijaID=" + red["ID"];
                    SqlCommand komanda = new SqlCommand(upit, konekcija);
                    komanda.ExecuteNonQuery();
                    MainWindow.selektovan = null;
                    this.Close();
                }

                else {
                    DateTime date = (DateTime)dpDatum.SelectedDate;

                  
                    string insert = @"insert into tblRegistracija values(Datum, Cena, BrojTablice, PosebnaTablica, VoziloID)
                    values('" + dpDatum.SelectedDate +
                    "', " + txtCena.Text + 
                    ", '" + txtBrojTablice.Text + 
                    "'," + Convert.ToInt32(cbPosebnaTablica.IsChecked) +
                    ", " + cbxVozilo.SelectedValue + ");";


                    SqlCommand cmd = new SqlCommand(insert, konekcija);
                    cmd.ExecuteNonQuery();
                    this.Close();
                }               

            }
            catch (SqlException)
            {
                MessageBox.Show("Unos odredjenih vrednosti nije validan!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Odaberite datum!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
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
