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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFRegistracijaVozila5.Forme;

namespace WPFRegistracijaVozila5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static SqlConnection konekcija = Konekcija.KreirajKonekciju();
        public static string ucitanaTabela;
        public static bool azuriraj;
        public static object selektovan;




        #region Select upiti
        static string vlasniciSelect = @"Select VlasnikID as ID,ImeVlasnika as Ime, PrezimeVlasnika as Prezime, JBMGVlasnika as JBMG, AdresaVlasnika as Adresa, 
GradVlasnika as Grad, KontaktVlasnika as Kontakt From tblVlasnik;";

        static string vozilaSelect = @"Select VoziloID as ID, BrojSasije as 'Broj sasije', Kubikaza, KonjskaSnaga as 'KS',
ImeVlasnika+ ''+PrezimeVlasnika as Vlasnik, NazivMarke as Marka, NazivModela as Model, NazivTipaVozila as Tip
From tblVozilo inner join tblVlasnik on tblVozilo.VlasnikID = tblVlasnik.VlasnikID
				inner join tblMarka on tblVozilo.MarkaID = tblMarka.IDMarkaVozila
				inner join tblModel on tblVozilo.ModelID = tblModel.ModelID
				inner join tblTipVozila on tblVozilo.TipVozilaID = tblTipVozila.TipVozilaID;";
        static string registracijaSelect = @"Select RegistracijaID as ID, BrojTablice as 'Broj tablice', Datum, Cena, PosebnaTablica as 'Posebna tablica', BrojSasije as 'Broj sasije'
From tblRegistracija inner join tblVozilo on tblRegistracija.VoziloID = tblVozilo.VoziloID;";
        static string markaSelect = @"Select IDMarkaVozila as ID, NazivMarke as Naziv From tblMarka;";
        static string modelSelect = @"Select ModelID as ID, NazivModela as Naziv From tblModel;";
        static string tipSelect = @"Select TipVozilaID as ID, NazivTipaVozila as Naziv From tblTipVozila;";

        #endregion

        #region Select sa uslovom
        static string selectUslovVlasnici = @"Select * From tblVlasnik Where VlasnikID =";
        static string selectUslovVozila = @"Select * From tblVozilo Where VoziloID =";
        static string selectUslovRegistracije = @"Select * From tblRegistracija Where RegistracijaID =";
        static string selectUslovMarka = @"Select * From tblMarka Where IDMarkaVozila =";
        static string selectUslovModel = @"Select * From tblModel Where ModelID =";
        static string selectUslovTipVozila= @"Select * From tblTipVozila Where TipVozilaID =";
        #endregion
        #region Delete Upit
        static string vlasiniciDelete = @"Delete From tblVlasnik Where VlasnikID=";
        static string vozilaDelete = @"Delete From tblVozilo Where VoziloID =";
        static string registracijeDelete = @"Delete From tblRegistracija Where RegistracijaID =";
        static string markaDelete = @"Delete From tblMarka Where MarkaID =";
        static string modelDelete = @"Delete From tblModel Where ModelID =";
        static string tipDelete = @"Delete From tblTipVozila Where TipVozilaID =";
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            UcitajPodatke(dataGridCentralni, vlasniciSelect); //podaci se odmah izgenerisu pri otvaranju
        }

        static void UcitajPodatke(DataGrid grid, string selectUpit)
        {
            
            try
            {
                konekcija.Open();
                SqlDataAdapter da = new SqlDataAdapter(selectUpit, konekcija);
                DataTable dt = new DataTable();
                da.Fill(dt);
                grid.ItemsSource = dt.DefaultView;

                ucitanaTabela = selectUpit;



            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
            }

        }
        static void popuniFormu(DataGrid grid, string selectUslov)
        {
            try
            {
                azuriraj = true;
                konekcija.Open();
                               

                DataRowView red = (DataRowView)grid.SelectedItems[0];
                selektovan = red;
                

                string upit = selectUslov + red["ID"];

                SqlCommand komanda = new SqlCommand(upit, konekcija);

                SqlDataReader citac = komanda.ExecuteReader();

                while (citac.Read())
                {
                    if (ucitanaTabela.Equals(vlasniciSelect))
                    {
                        frmVlasnik prozorVlasnik = new frmVlasnik();
                        prozorVlasnik.txtImeVlasnika.Text = citac["ImeVlasnika"].ToString();
                        prozorVlasnik.txtPrezimeVlasnika.Text = citac["PrezimeVlasnika"].ToString();
                        prozorVlasnik.txtJMBGVlasnika.Text = citac["JMBGVlasnika"].ToString();
                        prozorVlasnik.txtAdresaVlasnika.Text = citac["AdresaVlasnika"].ToString();
                        prozorVlasnik.txtGradVlasnika.Text = citac["GradVlasnika"].ToString();
                        prozorVlasnik.txtKontaktVlasnika.Text = citac["KontaktVlasnika"].ToString();

                        prozorVlasnik.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(vozilaSelect))
                    {
                        frmVozilo prozorVozilo = new frmVozilo();

                        prozorVozilo.txtBrojSasije.Text = citac["BrojSasije"].ToString();
                        prozorVozilo.txtKubikaze.Text = citac["Kubikaza"].ToString();
                        prozorVozilo.txtKonjskaSnaga.Text = citac["KonjskaSnaga"].ToString();
                        prozorVozilo.cbxVlasnik.SelectedValue = citac["VlasnikID"].ToString();
                        prozorVozilo.cbxMarka.SelectedValue = citac["MarkaID"].ToString();
                        prozorVozilo.cbxModel.SelectedValue = citac["ModelID"].ToString();
                        prozorVozilo.cbxTip.SelectedValue = citac["TipVozilaID"].ToString();

                        prozorVozilo.ShowDialog();
                    }


                    else if (ucitanaTabela.Equals(registracijaSelect))
                    {
                        frmRegistracija prozorReg = new frmRegistracija();

                        prozorReg.txtBrojTablice.Text = citac["BrojTablice"].ToString();
                        prozorReg.txtCena.Text = citac["Cena"].ToString();
                        prozorReg.dpDatum.Text = citac["Datum"].ToString();
                        prozorReg.cbxVozilo.SelectedValue = citac["VoziloID"].ToString();
                        prozorReg.cbPosebnaTablica.IsChecked = (bool)citac["PosebnaTablica"];

                        prozorReg.ShowDialog();
                    }

                    else if (ucitanaTabela.Equals(markaSelect))
                    {
                        frmMarka prozorMarka = new frmMarka();

                        prozorMarka.txtNazivMarke.Text = citac["NazivMarke"].ToString();
                        prozorMarka.ShowDialog();

                    }

                    else if (ucitanaTabela.Equals(modelSelect))
                    {
                        frmModel prozorModel = new frmModel();

                        prozorModel.txtNazivModela.Text = citac["NazivModela"].ToString();
                        prozorModel.ShowDialog();

                    }
                    else if (ucitanaTabela.Equals(tipSelect))
                    {
                        frmTipVozila prozorTip = new frmTipVozila();

                        prozorTip.txtNazivTipa.Text = citac["NazivTipa"].ToString();
                        prozorTip.ShowDialog();

                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste selektovali red", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
                azuriraj = false;
            }
        }
        static void ObrisiZapis(DataGrid grid, string deleteUpit)
        {
            try
            {
                konekcija.Open();

                DataRowView red = (DataRowView)grid.SelectedItems[0];
                string upit = deleteUpit + red["ID"];

                MessageBoxResult rezultat = MessageBox.Show("Da li ste sigurni?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (rezultat == MessageBoxResult.Yes)
                {
                    SqlCommand komanda = new SqlCommand(upit, konekcija);
                    komanda.ExecuteNonQuery();
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste selektovali red!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SqlException)
            {
                MessageBox.Show("Postoje povezani podaci u drugim tabelama.", "Obavestenje", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
            }
        }


        //ISPIS
        private void BtnVlasnici_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, vlasniciSelect);

        }

        private void BtnVozila_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, vozilaSelect);
        }

        private void BtnRegistracija_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, registracijaSelect);
        }

        private void BtnMarke_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, markaSelect);
        }

        private void BtnModeli_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, modelSelect);
        }

        private void BtnTipovi_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, tipSelect);
        }
        //DODAJ
        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            Window prozor;

            if (ucitanaTabela.Equals(vlasniciSelect))
            {
                prozor = new frmVlasnik();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, vlasniciSelect); //automatski se desi

            }
           
                                          
           
            else if (ucitanaTabela.Equals(registracijaSelect))
            {
                prozor = new frmVozilo();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, registracijaSelect);
            }
            
           
            else if (ucitanaTabela.Equals(markaSelect))
            {
                prozor = new frmVozilo();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, markaSelect);
            }
                       
           
            else if (ucitanaTabela.Equals(modelSelect))
            {
                prozor = new frmVozilo();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, modelSelect);
            }
                       
            
            else if (ucitanaTabela.Equals(tipSelect))
            {
                prozor = new frmVozilo();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, tipSelect);
            }



        }







        //IZMENI
        private void BtnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (ucitanaTabela.Equals(vlasniciSelect))
            {
                popuniFormu(dataGridCentralni, selectUslovVlasnici);
                UcitajPodatke(dataGridCentralni, vlasniciSelect);
            }
            else if (ucitanaTabela.Equals(vozilaSelect))
            {
                popuniFormu(dataGridCentralni, selectUslovVozila);
                UcitajPodatke(dataGridCentralni, vozilaSelect);
            }
            else if (ucitanaTabela.Equals(registracijaSelect))
            {
                popuniFormu(dataGridCentralni, selectUslovRegistracije);
                UcitajPodatke(dataGridCentralni, registracijaSelect);
            }
            else if (ucitanaTabela.Equals(markaSelect))
            {
                popuniFormu(dataGridCentralni, selectUslovMarka);
                UcitajPodatke(dataGridCentralni, markaSelect);
            }
            else if (ucitanaTabela.Equals(modelSelect))
            {
                popuniFormu(dataGridCentralni, selectUslovModel);
                UcitajPodatke(dataGridCentralni, modelSelect);
            }
            else if (ucitanaTabela.Equals(tipSelect))
            {
                popuniFormu(dataGridCentralni, selectUslovTipVozila);
                UcitajPodatke(dataGridCentralni, tipSelect);
            }

        }

        private void BtnObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (ucitanaTabela.Equals(vlasniciSelect))
            {
                ObrisiZapis(dataGridCentralni, vlasiniciDelete);
                UcitajPodatke(dataGridCentralni, vlasniciSelect);
            }
            else if (ucitanaTabela.Equals(vozilaSelect))
            {
                ObrisiZapis(dataGridCentralni, vozilaDelete);
                UcitajPodatke(dataGridCentralni, vozilaSelect);
            }
            else if (ucitanaTabela.Equals(registracijaSelect))
            {
                ObrisiZapis(dataGridCentralni, registracijeDelete);
                UcitajPodatke(dataGridCentralni, registracijaSelect);
            }
            else if (ucitanaTabela.Equals(markaSelect))
            {
                ObrisiZapis(dataGridCentralni, markaDelete);
                UcitajPodatke(dataGridCentralni, markaSelect);
            }
            else if (ucitanaTabela.Equals(modelSelect))
            {
                ObrisiZapis(dataGridCentralni, modelDelete);
                UcitajPodatke(dataGridCentralni, modelSelect);
            }

            else if (ucitanaTabela.Equals(tipSelect))
            {
                ObrisiZapis(dataGridCentralni, tipDelete);
                UcitajPodatke(dataGridCentralni, tipSelect);
            }

        }
    }
}
