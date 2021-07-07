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
    /// Interaction logic for frmVozilo.xaml
    /// </summary>
    public partial class frmVozilo : Window
    {
        SqlConnection konekcija = Konekcija.KreirajKonekciju();
        public frmVozilo()
        {
            InitializeComponent();


            try
            {
                konekcija.Open();

                string vratiMarke = "Select IDMarkaVozila, NazivMarke from tblMarka";
                DataTable dtMarka = new DataTable();
                SqlDataAdapter daMarka = new SqlDataAdapter(vratiMarke, konekcija);
                daMarka.Fill(dtMarka);
                cbxMarka.ItemsSource = dtMarka.DefaultView;


                string vratiModele = "Select ModelID, NazivModela from tblModel";
                DataTable dtModel = new DataTable();
                SqlDataAdapter daModel = new SqlDataAdapter(vratiModele, konekcija);
                daModel.Fill(dtModel);
                cbxModel.ItemsSource = dtModel.DefaultView;

                string vratiTipove = "Select TipID, NazivTipa from tblTip";
                DataTable dtTip = new DataTable();
                SqlDataAdapter daTip = new SqlDataAdapter(vratiTipove, konekcija);
                daTip.Fill(dtTip);
                cbxTip.ItemsSource = dtTip.DefaultView;

                string vratiVlasnike = "Select VlasnikID, ImeVlasnika+' '+Prezime as Vlasnik from tblVlasnik";
                DataTable dtVlasnik = new DataTable();
                SqlDataAdapter daVlasnik = new SqlDataAdapter(vratiVlasnike, konekcija);
                daVlasnik.Fill(dtVlasnik);
                cbxVlasnik.ItemsSource = dtVlasnik.DefaultView;
                                                          
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

                    string upit = @"Update tblVozilo
                        Set BrojSasije='" + txtBrojSasije.Text + "', Kubikaza=" + txtKubikaze.Text + " , KonjskaSnaga=" + txtKonjskaSnaga.Text + " , MarkaID =" + cbxMarka.SelectedValue + " ,ModelID= " + cbxModel.SelectedValue + ", TipVozilaID=" + cbxTip.SelectedValue + " , VlasnikID=" + cbxVlasnik.SelectedValue + " Where VoziloID=" + red["ID"];
                    SqlCommand komanda = new SqlCommand(upit, konekcija);
                    komanda.ExecuteNonQuery();
                    MainWindow.selektovan = null;
                    this.Close();
                }
                else {
                    
                string insert = @"insert into tblVozilo(BrojSasije, Kubikaze, KonjskaSnaga, VlasnikID, ModelID, MarkaID, TipVozilaID)
                        values('"+txtBrojSasije.Text+"', "+txtKubikaze.Text+", "+txtKonjskaSnaga.Text+", "+cbxVlasnik.SelectedValue+", "+cbxModel.SelectedValue+", "+cbxMarka.SelectedValue+", "+cbxTip.SelectedValue+")";
                                                                      
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
