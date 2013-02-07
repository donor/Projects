using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ASO
{
    public partial class MainWindow : Form
    {
        private int idPracownika;
        private int nrFaktury;
        private int idZlecenia;
        private int cenaZlecenia;
        private int idCzesci;
        private int cenaUslugi;
        
        public MainWindow()
        {
            
            InitializeComponent();
               
            menuStrip1.Visible = false;
            gbSZ.Visible = false;
            lblLogin.Visible = true;

                textBox1.Text = DateTime.Now.ToShortDateString();
                ukryjKontrolkiZlecenia();
                ukryjKontrolkiCzesci();
                ukryjKontrolkiPracownicy();
                ukryjKontrolkiUslugi();
                ukryjKontrolkiKlienci();
                ukryjKontrolkiHarmonogram();

                Harmonogram h = new Harmonogram();
                h.usunByleDni();

                for (int i = 1; i <= 10; i++)
                {
                    h.czyDzienJestwHarmonogramie(i);
                    if (!h.jestDzien)
                        h.dodajDniDoHarmonogramu(i);
                    else
                    { ;}

                }
         }
        private void ukryjKontrolkiUslugi()
        {
            lVUslugi.Visible = false;
            gBSzczegolyUslugi.Visible = false;
            btnAnulujDodajUsluge.Visible = false;
            btnOkDodajUsluge.Visible = false;
            button2.Visible = false;
            button1.Visible = false;
            label2.Visible = false;
            label1.Visible = false; ;
            txtCena.Visible = false; ;
            txtOpisUslugi.Visible = false;

            
            gbUsunUsluge.Visible = false;
            label4.Visible = false;
            txtIdUslugiUsun.Visible = false;
            btnOkUsunUsluge.Visible = false;
            btnAnulujUsunUsluge.Visible = false;

            
            gBModyfikujUsluge.Visible = false;
            lblOpisUslugiModyfikuj.Visible = false;
            lblIdUslugiModyfikuj.Visible = false;
            txtIdUslugiModyfikuj.Visible = false;
            txtOpisUslugiModyfikuj.Visible = false;
            lblCenaModyfikuj.Visible = false;
            txtCenaModyfikuj.Visible = false;
            btnOkModyfikuj.Visible = false;
            btnAnulujModyfikuj.Visible = false;

        }

        private void wyswietlListeUslug()
        {
            lVUslugi.Visible = true;
            lVUslugi.View = View.Details;
            lVUslugi.LabelEdit = true;
            lVUslugi.AllowColumnReorder = true;
            lVUslugi.FullRowSelect = true;
            lVUslugi.GridLines = true;
            lVUslugi.Clear();
            lVUslugi.Columns.Add("idUsługi", 100, HorizontalAlignment.Left);
            lVUslugi.Columns.Add("Opis Usługi", 250, HorizontalAlignment.Left);
            lVUslugi.Columns.Add("cena", 70, HorizontalAlignment.Left);
            
            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Uslugi", connection);

                SqlDataReader dr = cmd.ExecuteReader();

                lVUslugi.Items.Clear();

                while (dr.Read())
                {

                    ListViewItem lvi = new ListViewItem(dr[0].ToString());
                    for (int i = 1; i < dr.FieldCount; i++)
                    {
                        lvi.SubItems.Add(dr[1].ToString());
                        lvi.SubItems.Add(dr[2].ToString());
                    }
                    lVUslugi.Items.Add(lvi);
                }

                dr.Close();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void wyświetlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ukryjKontrolkiZlecenia();
            ukryjKontrolkiCzesci();
            ukryjKontrolkiPracownicy();
            ukryjKontrolkiKlienci();
            ukryjKontrolkiHarmonogram();
            wyswietlListeUslug();

        }
    
             

        private void btnZaloguj_Click(object sender, EventArgs e)
        {
            Logowanie l = new Logowanie();
            l.Zaloguj(txtLogin.Text, txtHaslo.Text);
            idPracownika = l.idPracownika;
            
            if (l.blad)
            {
                txtLogin.Visible = false; txtHaslo.Visible = false; btnZaloguj.Visible = false; lblLogin.Visible = false; lblHaslo.Visible = false;
                menuStrip1.Visible = true;
                gbLogowanie.Visible = false;
            }
            if (idPracownika != 0)
                pracownicyToolStripMenuItem.Visible = false;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'asoDataSet.Uslugi' table. You can move, or remove it, as needed.
           

        }

        private void txtCena_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.KeyChar = (char)0;
        }

//funkcja do zamiany zawartości textboxa na int
        private int txtBoxtoInt(string textbox)
        {
            string text = textbox;
            int inter = 0;

            try
            {
                inter = int.Parse(text);
               
            }
            catch (Exception)
            {
                MessageBox.Show("Błąd przy wprowadzaniu liczby!");
            }
            return inter;
        }

        private void btnOkDodajUsluge_Click(object sender, EventArgs e)
        {
            Usluga u = new Usluga();

            int cena = txtBoxtoInt(txtCena.Text);

            u.dodajUsluge(txtOpisUslugi.Text, cena, 1);
            wyswietlListeUslug();
            txtCena.Text = "";
            txtOpisUslugi.Text = "";

        }

        private void btnAnulujDodajUsluge_Click(object sender, EventArgs e)
        {
            gBSzczegolyUslugi.Visible = false; btnAnulujDodajUsluge.Visible = false; btnOkDodajUsluge.Visible = false;
            button2.Visible = false; button1.Visible = false; label2.Visible = false; label1.Visible = false;
            txtCena.Visible = false; txtOpisUslugi.Visible = false;

        }
        

        private void dodajUsługeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ukryjKontrolkiZlecenia();
            ukryjKontrolkiCzesci();
            ukryjKontrolkiPracownicy();
            ukryjKontrolkiKlienci();
            ukryjKontrolkiHarmonogram();
            wyswietlListeUslug();
             gBSzczegolyUslugi.Visible = true; btnAnulujDodajUsluge.Visible = true; btnOkDodajUsluge.Visible = true;
            button2.Visible = true; button1.Visible = true; label2.Visible = true; label1.Visible = true; txtCena.Visible = true; txtOpisUslugi.Visible = true;
            gbUsunUsluge.Visible = false; label4.Visible = false; txtIdUslugiUsun.Visible = false; btnOkUsunUsluge.Visible = false; btnAnulujUsunUsluge.Visible = false;
            gBModyfikujUsluge.Visible = false; lblOpisUslugiModyfikuj.Visible = false; lblIdUslugiModyfikuj.Visible = false; txtIdUslugiModyfikuj.Visible = false;
            txtOpisUslugiModyfikuj.Visible = false; lblCenaModyfikuj.Visible = false; txtCenaModyfikuj.Visible = false; btnOkModyfikuj.Visible = false; btnAnulujModyfikuj.Visible = false;
        }
    

        private void btnUsunUsluge_Click(object sender, EventArgs e)
        {
            ukryjKontrolkiZlecenia();
            ukryjKontrolkiCzesci();
            ukryjKontrolkiPracownicy();
            wyswietlListeUslug();
            ukryjKontrolkiKlienci();
            ukryjKontrolkiHarmonogram();
            gBSzczegolyUslugi.Visible = false; btnAnulujDodajUsluge.Visible = false; btnOkDodajUsluge.Visible = false;
            button2.Visible = false; button1.Visible = false; label2.Visible = false; label1.Visible = false;
            txtCena.Visible = false; txtOpisUslugi.Visible = false; 
            gbUsunUsluge.Visible = true; label4.Visible = true; txtIdUslugiUsun.Visible = true; btnOkUsunUsluge.Visible = true; btnAnulujUsunUsluge.Visible = true;
            gBModyfikujUsluge.Visible = false; lblOpisUslugiModyfikuj.Visible = false; lblIdUslugiModyfikuj.Visible = false; txtIdUslugiModyfikuj.Visible = false;
            txtOpisUslugiModyfikuj.Visible = false; lblCenaModyfikuj.Visible = false; txtCenaModyfikuj.Visible = false; btnOkModyfikuj.Visible = false; btnAnulujModyfikuj.Visible = false;
        }

        private void btnOkUsunUsluge_Click(object sender, EventArgs e)
        {
            Usluga u = new Usluga();
            
        u.usunUsluge(txtBoxtoInt(txtIdUslugiUsun.Text));
        wyswietlListeUslug();
            
        }

        private void btnAnulujUsunUsluge_Click(object sender, EventArgs e)
        {
            gbUsunUsluge.Visible = false; label4.Visible = false; txtIdUslugiUsun.Visible = false; btnOkUsunUsluge.Visible = false; btnAnulujUsunUsluge.Visible = false;
        }

        private void usuńUsługeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ukryjKontrolkiZlecenia();
            ukryjKontrolkiCzesci();
            ukryjKontrolkiPracownicy();
            ukryjKontrolkiKlienci();
            ukryjKontrolkiHarmonogram();
            wyswietlListeUslug();
            gBSzczegolyUslugi.Visible = false; btnAnulujDodajUsluge.Visible = false; btnOkDodajUsluge.Visible = false;
            button2.Visible = false; button1.Visible = false; label2.Visible = false; label1.Visible = false;
            txtCena.Visible = false; txtOpisUslugi.Visible = false;
            gbUsunUsluge.Visible = true; label4.Visible = true; txtIdUslugiUsun.Visible = true; btnOkUsunUsluge.Visible = true; btnAnulujUsunUsluge.Visible = true;
            
            gBModyfikujUsluge.Visible = false; lblOpisUslugiModyfikuj.Visible = false; lblIdUslugiModyfikuj.Visible = false; txtIdUslugiModyfikuj.Visible = false;
            txtOpisUslugiModyfikuj.Visible = false; lblCenaModyfikuj.Visible = false; txtCenaModyfikuj.Visible = false; btnOkModyfikuj.Visible = false;  btnAnulujModyfikuj.Visible = false;
        }

        private void modyfikujUsługeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ukryjKontrolkiZlecenia();
            ukryjKontrolkiCzesci();
            ukryjKontrolkiPracownicy();
            ukryjKontrolkiKlienci();
            ukryjKontrolkiHarmonogram();
            wyswietlListeUslug();
            gbUsunUsluge.Visible = false; label4.Visible = false; txtIdUslugiUsun.Visible = false; btnOkUsunUsluge.Visible = false; btnAnulujUsunUsluge.Visible = false;
            gBSzczegolyUslugi.Visible = false; btnAnulujDodajUsluge.Visible = false; btnOkDodajUsluge.Visible = false; button2.Visible = false;
            button1.Visible = false; label2.Visible = false;  label1.Visible = false; txtCena.Visible = false; txtOpisUslugi.Visible = false;
            
            gBModyfikujUsluge.Visible = true; lblOpisUslugiModyfikuj.Visible = true; lblIdUslugiModyfikuj.Visible = true; txtIdUslugiModyfikuj.Visible = true; txtOpisUslugiModyfikuj.Visible = true; lblCenaModyfikuj.Visible = true;
            txtCenaModyfikuj.Visible = true; btnOkModyfikuj.Visible = true; btnAnulujModyfikuj.Visible = true;
        }

        private void btnAnulujModyfikujUsluge_Click(object sender, EventArgs e)
        {
            gBModyfikujUsluge.Visible = false; lblOpisUslugiModyfikuj.Visible = false; lblIdUslugiModyfikuj.Visible = false; txtIdUslugiModyfikuj.Visible = false; txtOpisUslugiModyfikuj.Visible = false; lblCenaModyfikuj.Visible = false;
            txtCenaModyfikuj.Visible = false; btnOkModyfikuj.Visible = false;  btnAnulujModyfikuj.Visible = false;
        }

        private void btnOkModyfikujUsluge_Click(object sender, EventArgs e)
        {
            if (txtCenaModyfikuj.Text == "")
                txtCenaModyfikuj.Text ="-1";

            Usluga u = new Usluga();
            u.modyfikujUsluge(txtBoxtoInt(txtIdUslugiModyfikuj.Text), txtOpisUslugiModyfikuj.Text, txtBoxtoInt(txtCenaModyfikuj.Text), 1);

            txtCenaModyfikuj.Text = "";
            txtOpisUslugiModyfikuj.Text = "";
            txtIdUslugiModyfikuj.Text = "";

            wyswietlListeUslug();
        }

        private void txtIdUslugiModyfikuj_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.KeyChar = (char)0;
        }

        private void txtIdUslugiUsun_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
               e.KeyChar = (char)0;
        }
        private void wyswietlListeUslugWZleceniu(int idZlecenia)
        {
            listViewUWZ.Visible = true;
            listViewUWZ.View = View.Details;
            listViewUWZ.LabelEdit = true;
            listViewUWZ.AllowColumnReorder = true;
            listViewUWZ.FullRowSelect = true;
            listViewUWZ.GridLines = true;
            listViewUWZ.Clear();
            listViewUWZ.Columns.Add("idUslug", 50, HorizontalAlignment.Left);
            listViewUWZ.Columns.Add("opis Uslugi", 223, HorizontalAlignment.Left);

            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                string zapytanie = string.Format("SELECT dbo.Uslugi.idUslugi, dbo.Uslugi.opisUslugi FROM dbo.Uslugi_Zlecenia INNER JOIN dbo.Uslugi ON dbo.Uslugi_Zlecenia.idUslugi = dbo.Uslugi.idUslugi INNER JOIN dbo.Zlecenia ON dbo.Uslugi_Zlecenia.idZlecenia = dbo.Zlecenia.idZlecenia WHERE (dbo.Zlecenia.idZlecenia = {0})", idZlecenia);

                SqlCommand cmd = new SqlCommand(zapytanie, connection);

                SqlDataReader dr = cmd.ExecuteReader();

                listViewUWZ.Items.Clear();

                while (dr.Read())
                {

                    ListViewItem lvi = new ListViewItem(dr[0].ToString());
                    for (int i = 1; i < dr.FieldCount; i++)
                    {
                        lvi.SubItems.Add(dr[1].ToString());
                    }
                    listViewUWZ.Items.Add(lvi);
                }

                dr.Close();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void wyswietlListeZlecen()
        {

            listViewZlecenia.Visible = true;
            listViewZlecenia.View = View.Details;
            listViewZlecenia.LabelEdit = true;
            listViewZlecenia.AllowColumnReorder = true;
            listViewZlecenia.FullRowSelect = true;
            listViewZlecenia.GridLines = true;
            listViewZlecenia.Clear();
            listViewZlecenia.Columns.Add("idZlecenia", 75, HorizontalAlignment.Left);
            listViewZlecenia.Columns.Add("data", 70, HorizontalAlignment.Left);
            listViewZlecenia.Columns.Add("Cena", 60, HorizontalAlignment.Left);
            listViewZlecenia.Columns.Add("nr Faktury", 78, HorizontalAlignment.Left);

            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                SqlCommand cmd = new SqlCommand("SELECT idZlecenia, data, cena, nrFaktury FROM Zlecenia", connection);

                SqlDataReader dr = cmd.ExecuteReader();

                listViewZlecenia.Items.Clear();

                while (dr.Read())
                {

                    ListViewItem lvi = new ListViewItem(dr[0].ToString());
                    for (int i = 1; i < dr.FieldCount; i++)
                    {
                        lvi.SubItems.Add(dr[1].ToString().Remove(11));
                        lvi.SubItems.Add(dr[2].ToString());
                        lvi.SubItems.Add(dr[3].ToString());
                    }
                    listViewZlecenia.Items.Add(lvi);
                }

                dr.Close();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void wszystkieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listViewZlecenia.Visible = true;
            lblWyborZleceniaSzczegoly.Visible = false;
            txtIdZleceniaSzczegoly.Visible = false;
            btnWyswietlSzczegolyZlecenia.Visible = false;
            gbDodajZlecenia.Visible = false;
            gbSZ.Visible = false;
            gbPdoDZ.Visible = false;
            ukryjKontrolkiUslugi();
            ukryjKontrolkiCzesci();
            ukryjKontrolkiPracownicy();
            ukryjKontrolkiKlienci();
            ukryjKontrolkiHarmonogram();

            wyswietlListeZlecen();
        }

        private void szczegółyWybranegoZleceniaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listViewZlecenia.Visible = true;
            lblWyborZleceniaSzczegoly.Visible = true;
            txtIdZleceniaSzczegoly.Visible = true;
            btnWyswietlSzczegolyZlecenia.Visible = true;
            gbDodajZlecenia.Visible = false;
            gbSZ.Visible = true;
            gbPdoDZ.Visible = false;

            ukryjKontrolkiUslugi();
            ukryjKontrolkiCzesci();
            ukryjKontrolkiPracownicy();
            ukryjKontrolkiKlienci();
            ukryjKontrolkiHarmonogram();
           
            wyswietlListeZlecen();
            
        }

        private void btnWyswietlSzczegolyZlecenia_Click(object sender, EventArgs e)
        {
            gbSZ.Visible = true;
            
            ukryjKontrolkiUslugi();
            ukryjKontrolkiCzesci();
            ukryjKontrolkiPracownicy();
            ukryjKontrolkiKlienci();
            ukryjKontrolkiHarmonogram();
            
            Zlecenie z = new Zlecenie();
            z.pracownikWykonujacyIKlientSzczegoly(txtBoxtoInt(txtIdZleceniaSzczegoly.Text));
            txtSPZ.Text = z.pracownikDane;
            txtSKZ.Text = z.klientDane;
            wyswietlListeUslugWZleceniu(txtBoxtoInt(txtIdZleceniaSzczegoly.Text));
        }

        private void wyswietlListePomocniczaUslugWZleceniu()
        {
                        
            lVUWZ.Visible = true;
            lVUWZ.View = View.Details;
            lVUWZ.LabelEdit = true;
            lVUWZ.AllowColumnReorder = true;
            lVUWZ.FullRowSelect = true;
            lVUWZ.GridLines = true;
            lVUWZ.Clear();
            lVUWZ.Columns.Add("idUsługi", 100, HorizontalAlignment.Left);
            lVUWZ.Columns.Add("Opis Usługi", 250, HorizontalAlignment.Left);
            lVUWZ.Columns.Add("cena", 70, HorizontalAlignment.Left);
            
            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Uslugi", connection);

                SqlDataReader dr = cmd.ExecuteReader();

                lVUWZ.Items.Clear();

                while (dr.Read())
                {

                    ListViewItem lvi = new ListViewItem(dr[0].ToString());
                    for (int i = 1; i < dr.FieldCount; i++)
                    {
                        lvi.SubItems.Add(dr[1].ToString());
                        lvi.SubItems.Add(dr[2].ToString());
                    }
                    lVUWZ.Items.Add(lvi);
                }

                dr.Close();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            lVKWZ.Visible = true;
            lVKWZ.View = View.Details;
            lVKWZ.LabelEdit = true;
            lVKWZ.AllowColumnReorder = true;
            lVKWZ.FullRowSelect = true;
            lVKWZ.GridLines = true;
            lVKWZ.Clear();
            
            lVKWZ.Columns.Add("idKlienta", 77, HorizontalAlignment.Left);
            lVKWZ.Columns.Add("Imie", 110, HorizontalAlignment.Left);
            lVKWZ.Columns.Add("Nazwisko", 110, HorizontalAlignment.Left);
            lVKWZ.Columns.Add("Telefon", 130, HorizontalAlignment.Left);
            
            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                SqlCommand cmd = new SqlCommand("SELECT Klienci.idKlienta, Osoby.imie, Osoby.nazwisko, Osoby.telefon FROM Klienci INNER JOIN Osoby ON Klienci.idOsoby = Osoby.idOsoby", connection);

                SqlDataReader dr = cmd.ExecuteReader();

                lVKWZ.Items.Clear();

                while (dr.Read())
                {

                    ListViewItem lvi = new ListViewItem(dr[0].ToString());
                    for (int i = 1; i < dr.FieldCount; i++)
                    {
                        lvi.SubItems.Add(dr[1].ToString());
                        lvi.SubItems.Add(dr[2].ToString());
                        lvi.SubItems.Add(dr[3].ToString());
                    }
                    lVKWZ.Items.Add(lvi);
                }

                dr.Close();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ukryjKontrolkiZlecenia()
        {
            listViewZlecenia.Visible = false;
            lblWyborZleceniaSzczegoly.Visible = false;
            txtIdZleceniaSzczegoly.Visible = false;
            btnWyswietlSzczegolyZlecenia.Visible = false;
            gbDodajZlecenia.Visible = false;
            gbSZ.Visible = false;
            gbPdoDZ.Visible = false;

        }

        private void dodajZlecenieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ukryjKontrolkiUslugi();
            ukryjKontrolkiCzesci();
            ukryjKontrolkiPracownicy();
            ukryjKontrolkiKlienci();
            ukryjKontrolkiHarmonogram();
            listViewZlecenia.Visible = true;
            wyswietlListeZlecen();
            gbSZ.Visible = false;
            gbDodajZlecenia.Visible = true;
            gbPdoDZ.Visible = true;
            lblWyborZleceniaSzczegoly.Visible = false;
            txtIdZleceniaSzczegoly.Visible = false;
            btnWyswietlSzczegolyZlecenia.Visible = false;
            wyswietlListePomocniczaUslugWZleceniu();
        }
               
        private void btnDKZ_Click(object sender, EventArgs e)
        {
           // Logowanie l=new Logowanie();
            
            
            Zlecenie z = new Zlecenie();
            idZlecenia = z.kolejnyIdZlecenia();
            Faktura f=new Faktura();
            nrFaktury = f.dodajFakture();
            z.dodajZlecenie(idZlecenia,idPracownika, txtBoxtoInt(txtKWZ.Text), nrFaktury);
        }

        private void btnDUWZ_Click(object sender, EventArgs e)
        {
            Zlecenie z = new Zlecenie();
            z.dodajUslugeDoZlecenia(txtBoxtoInt(txtUWZ.Text),idZlecenia);
            ZmianaIlosci zi = new ZmianaIlosci();
            zi.wskazCzesci(txtBoxtoInt(txtUWZ.Text));
            cenaZlecenia=z.wyznaczCeneZlecenia(idZlecenia);
            z.wstawCeneZlecenia(cenaZlecenia, idZlecenia);
            txtUWZ.Text = "";
        }

        public void ukryjKontrolkiCzesci()
        {
            listViewCzesci.Visible=false;
            gbDodajCzesc.Visible = false;
            listViewPUC.Visible = false;
            gbASC.Visible = false;
        }

        private void btnZDOK_Click(object sender, EventArgs e)
        {
            wyswietlListeZlecen();
        }

        private void wyswietlListeCzesci()
        {
            listViewCzesci.Visible = true;
            listViewCzesci.View = View.Details;
            listViewCzesci.LabelEdit = true;
            listViewCzesci.AllowColumnReorder = true;
            listViewCzesci.FullRowSelect = true;
            listViewCzesci.GridLines = true;
            listViewCzesci.Clear();
            
            listViewCzesci.Columns.Add("idCzesci", 60, HorizontalAlignment.Left);
            listViewCzesci.Columns.Add("nazwa", 140, HorizontalAlignment.Left);
            listViewCzesci.Columns.Add("cena", 60, HorizontalAlignment.Left);
            listViewCzesci.Columns.Add("ilosc", 58, HorizontalAlignment.Left);
            
            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Czesci", connection);

                SqlDataReader dr = cmd.ExecuteReader();

                listViewCzesci.Items.Clear();

                while (dr.Read())
                {

                    ListViewItem lvi = new ListViewItem(dr[0].ToString());
                    for (int i = 1; i < dr.FieldCount; i++)
                    {
                        lvi.SubItems.Add(dr[1].ToString());
                        lvi.SubItems.Add(dr[2].ToString());
                        lvi.SubItems.Add(dr[3].ToString());
                    }
                    listViewCzesci.Items.Add(lvi);
                }

                dr.Close();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void wyswietlListePomocniczaUslugiDlaCzesci()
        {
            listViewPUC.Visible = true;
            listViewPUC.View = View.Details;
            listViewPUC.LabelEdit = true;
            listViewPUC.AllowColumnReorder = true;
            listViewPUC.FullRowSelect = true;
            listViewPUC.GridLines = true;
            listViewPUC.Clear();
            
            listViewPUC.Columns.Add("idUsługi", 70, HorizontalAlignment.Left);
            listViewPUC.Columns.Add("Opis Usługi", 250, HorizontalAlignment.Left);
            listViewPUC.Columns.Add("cena", 70, HorizontalAlignment.Left);
            
            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Uslugi", connection);

                SqlDataReader dr = cmd.ExecuteReader();

                listViewPUC.Items.Clear();

                while (dr.Read())
                {

                    ListViewItem lvi = new ListViewItem(dr[0].ToString());
                    for (int i = 1; i < dr.FieldCount; i++)
                    {
                        lvi.SubItems.Add(dr[1].ToString());
                        lvi.SubItems.Add(dr[2].ToString());
            
                    }
                    listViewPUC.Items.Add(lvi);
                }

                dr.Close();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void wyświetlToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ukryjKontrolkiZlecenia();
            
            ukryjKontrolkiPracownicy();
            ukryjKontrolkiUslugi();
            ukryjKontrolkiKlienci();
            ukryjKontrolkiHarmonogram();
            wyswietlListeCzesci();
        }

        private void dodajCzesciToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ukryjKontrolkiZlecenia();
            
            ukryjKontrolkiPracownicy();
            ukryjKontrolkiUslugi();
            ukryjKontrolkiKlienci();
            ukryjKontrolkiHarmonogram();
            wyswietlListeCzesci();
            gbDodajCzesc.Visible = true;
            gbASC.Visible = false;
            listViewPUC.Visible = true;
            
            wyswietlListePomocniczaUslugiDlaCzesci();
        }

        private void uzupelnijStanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ukryjKontrolkiZlecenia();
            
            ukryjKontrolkiPracownicy();
            ukryjKontrolkiUslugi();
            ukryjKontrolkiKlienci();
            ukryjKontrolkiHarmonogram();
            
            listViewPUC.Visible = false;
            gbDodajCzesc.Visible = false;
            gbASC.Visible = true;
            wyswietlListeCzesci();
            
        }

        private void btnDCDodajCzesc_Click(object sender, EventArgs e)
        {
            Czesc c = new Czesc();
            c.dodajCzesc(txtDCNazwa.Text,txtBoxtoInt(txtDCcena.Text),txtBoxtoInt(txtDCIloscSztuk.Text));
           idCzesci=c.idCzesci;
            wyswietlListeCzesci();
        }

        private void btnDCdoU_Click(object sender, EventArgs e)
        {
            Czesc c=new Czesc();

            c.dodajCzescDoUslugi(txtBoxtoInt(txtDCdoU.Text),idCzesci);
            cenaUslugi=c.wyznaczCeneUslugi(txtBoxtoInt(txtDCdoU.Text));

            c.wstawCeneUslugi(cenaUslugi,txtBoxtoInt(txtDCdoU.Text));
            wyswietlListePomocniczaUslugiDlaCzesci();
            txtDCdoU.Text = "";
        }

        private void btnASCOK_Click(object sender, EventArgs e)
        {
            Czesc c = new Czesc();
            c.uzupelnijStan(txtBoxtoInt(txtASIdCzesci.Text), txtBoxtoInt(txtASCLiczbasztuk.Text));

            txtASCLiczbasztuk.Text = "";
            txtASIdCzesci.Text = "";
            wyswietlListeCzesci();

        }

        private void btnASCAnuluj_Click(object sender, EventArgs e)
        {
            txtASCLiczbasztuk.Text = "";
            txtASIdCzesci.Text = "";
        }

        private void wyswietlListePracownikow()
        {
            listViewPracownicy.Visible = true;
            listViewPracownicy.View = View.Details;
            listViewPracownicy.LabelEdit = true;
            listViewPracownicy.AllowColumnReorder = true;
            listViewPracownicy.FullRowSelect = true;
            listViewPracownicy.GridLines = true;
            listViewPracownicy.Clear();
            listViewPracownicy.Columns.Add("idPracownika", 70, HorizontalAlignment.Left);
            listViewPracownicy.Columns.Add("Imie", 60, HorizontalAlignment.Left);
            listViewPracownicy.Columns.Add("Nazwisko", 75, HorizontalAlignment.Left);
            listViewPracownicy.Columns.Add("Stanowisko", 74, HorizontalAlignment.Left);
            listViewPracownicy.Columns.Add("Wynagrodzenie", 70, HorizontalAlignment.Left);

            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                SqlCommand cmd = new SqlCommand("SELECT Pracownicy.idPracownika, Osoby.imie, Osoby.nazwisko, Pracownicy.stanowisko, Pracownicy.wynagrodzenie FROM Pracownicy INNER JOIN Osoby ON Pracownicy.idOsoby = Osoby.idOsoby", connection);

                SqlDataReader dr = cmd.ExecuteReader();

                listViewPracownicy.Items.Clear();

                while (dr.Read())
                {

                    ListViewItem lvi = new ListViewItem(dr[0].ToString());
                    for (int i = 1; i < dr.FieldCount; i++)
                    {
                        lvi.SubItems.Add(dr[1].ToString());
                        lvi.SubItems.Add(dr[2].ToString());
                        lvi.SubItems.Add(dr[3].ToString());
                        lvi.SubItems.Add(dr[4].ToString());
                    }
                    listViewPracownicy.Items.Add(lvi);
                }

                dr.Close();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void ukryjKontrolkiPracownicy()
        {
            listViewPracownicy.Visible = false;
            gbDPracownika.Visible = false;
            gbUP.Visible = false;
            gbPZStanowiska.Visible = false;
            gbPZWynagrodzenia.Visible = false;
                     
        }

        private void wyświetlPracownikówToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ukryjKontrolkiZlecenia();
            ukryjKontrolkiCzesci();
            ukryjKontrolkiUslugi();
            ukryjKontrolkiKlienci();
            ukryjKontrolkiHarmonogram();

            wyswietlListePracownikow();
            
        }

        private void dodajPracownikaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wyswietlListePracownikow();
            gbDPracownika.Visible = true;
            gbUP.Visible = false;
            gbPZStanowiska.Visible = false;
            gbPZWynagrodzenia.Visible = false;
            ukryjKontrolkiZlecenia();
            ukryjKontrolkiCzesci();
            ukryjKontrolkiUslugi();
            ukryjKontrolkiKlienci();
            ukryjKontrolkiHarmonogram();
                     
        }

        private void usuńPracownikaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gbUP.Visible = true;
            gbDPracownika.Visible = false;
            gbPZStanowiska.Visible = false;
            gbPZWynagrodzenia.Visible = false;
            ukryjKontrolkiZlecenia();
            ukryjKontrolkiCzesci();
            ukryjKontrolkiUslugi();
            ukryjKontrolkiKlienci();
            ukryjKontrolkiHarmonogram();
        }

        private void stanowiskoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gbPZStanowiska.Visible = true;
            gbDPracownika.Visible = false;
            gbUP.Visible = false;
            gbPZWynagrodzenia.Visible = false;
            listViewPracownicy.Visible = true;
            ukryjKontrolkiZlecenia();
            ukryjKontrolkiCzesci();
            ukryjKontrolkiUslugi();
            ukryjKontrolkiKlienci();
            ukryjKontrolkiHarmonogram();
            wyswietlListePracownikow();
        }

        private void wynagrodzenieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gbPZWynagrodzenia.Visible = true;
            gbPZStanowiska.Visible = false;
            gbDPracownika.Visible = false;
            gbUP.Visible = false;
            listViewPracownicy.Visible = true;
            ukryjKontrolkiZlecenia();
            ukryjKontrolkiCzesci();
            ukryjKontrolkiUslugi();
            ukryjKontrolkiKlienci();
            ukryjKontrolkiHarmonogram();
            wyswietlListePracownikow();
        }

        private void btnDPOk_Click(object sender, EventArgs e)
        {
            Pracownik p = new Pracownik();
            p.dodajDaneOsobowePracownika(txtDPImie.Text, txtDPNazwisko.Text, txtDPStanowisko.Text, txtWynagrodzenie.Text);

            wyswietlListePracownikow();
            txtDPImie.Text = "";
            txtDPNazwisko.Text = "";
            txtDPStanowisko.Text = "";
            txtWynagrodzenie.Text = "";


        }

        private void btnDPAnuluj_Click(object sender, EventArgs e)
        {
            txtDPImie.Text = "";
            txtDPNazwisko.Text = "";
            txtDPStanowisko.Text = "";
            txtWynagrodzenie.Text = "";
        }

        private void btnUPUsun_Click(object sender, EventArgs e)
        {
            Pracownik p = new Pracownik();
            p.usunPracownika(txtBoxtoInt(txtUPIdPracownika.Text));
            wyswietlListePracownikow();
        }

        private void btnUPAnuluj_Click(object sender, EventArgs e)
        {
            txtUPIdPracownika.Text = "";
        }

        private void btnPZStanowiska_Click(object sender, EventArgs e)
        {
            Pracownik p = new Pracownik();
            p.modyfikujStanowiskoPracownika(txtBoxtoInt(txtPZSIdPracownika.Text), txtZSNNStanowiska.Text);
            wyswietlListePracownikow();
            txtZSNNStanowiska.Text = "";
            txtPZSIdPracownika.Text = "";
        }

        private void btnZPAnuluj_Click(object sender, EventArgs e)
        {
            txtZSNNStanowiska.Text = "";
            txtPZSIdPracownika.Text = "";
        }

        private void btnPZWOk_Click(object sender, EventArgs e)
        {
            Pracownik p = new Pracownik();
            p.modyfikujWynagrodzeniePracownika(txtBoxtoInt(txtPZWIdPracownika.Text), txtBoxtoInt(txtPZWNWwynagrodzenia.Text));
            wyswietlListePracownikow();
            txtPZWIdPracownika.Text = "";
            txtPZWNWwynagrodzenia.Text = "";
        }

        private void btnPZWAnuluj_Click(object sender, EventArgs e)
        {
            txtPZWIdPracownika.Text = "";
            txtPZWNWwynagrodzenia.Text = "";
        }

        private void wyswietlListeKlientow()
        {
            listViewKlienci.Visible = true;
            listViewKlienci.View = View.Details;
            listViewKlienci.LabelEdit = true;
            listViewKlienci.AllowColumnReorder = true;
            listViewKlienci.FullRowSelect = true;
            listViewKlienci.GridLines = true;
            listViewKlienci.Clear();
            
            listViewKlienci.Columns.Add("idKlienta", 35, HorizontalAlignment.Left);
            listViewKlienci.Columns.Add("Imie", 55, HorizontalAlignment.Left);
            listViewKlienci.Columns.Add("Nazwisko", 54, HorizontalAlignment.Left);
            listViewKlienci.Columns.Add("Telefon", 75, HorizontalAlignment.Left);
            listViewKlienci.Columns.Add("Miasto", 55, HorizontalAlignment.Left);
            listViewKlienci.Columns.Add("Ulica", 55, HorizontalAlignment.Left);
            listViewKlienci.Columns.Add("Nr Lokalu", 45, HorizontalAlignment.Left);


            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                SqlCommand cmd = new SqlCommand("SELECT Klienci.idKlienta, Osoby.imie, Osoby.nazwisko, Osoby.telefon, Osoby.miasto, Osoby.ulica, Osoby.nrLokalu FROM Klienci INNER JOIN Osoby ON Klienci.idOsoby = Osoby.idOsoby", connection);

                SqlDataReader dr = cmd.ExecuteReader();

                listViewKlienci.Items.Clear();

                while (dr.Read())
                {

                    ListViewItem lvi = new ListViewItem(dr[0].ToString());
                    for (int i = 1; i < dr.FieldCount; i++)
                    {
                        lvi.SubItems.Add(dr[1].ToString());
                        lvi.SubItems.Add(dr[2].ToString());
                        lvi.SubItems.Add(dr[3].ToString());
                        lvi.SubItems.Add(dr[4].ToString());
                        lvi.SubItems.Add(dr[5].ToString());
                        lvi.SubItems.Add(dr[6].ToString());
                    }
                    listViewKlienci.Items.Add(lvi);
                }

                dr.Close();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void ukryjKontrolkiKlienci()
        {
            listViewKlienci.Visible = false;
            gbDodajKlienta.Visible = false;
            gbUK.Visible = false;
            gbZK.Visible = false;
        }

        private void wyswietlZleceniaKlienta(int idKlienta)
        {
            listViewZK.Visible = true;
            listViewZK.View = View.Details;
            listViewZK.LabelEdit = true;
            listViewZK.AllowColumnReorder = true;
            listViewZK.FullRowSelect = true;
            listViewZK.GridLines = true;
            listViewZK.Clear();
            
            listViewZK.Columns.Add("idZlecenia", 70, HorizontalAlignment.Left);
            listViewZK.Columns.Add("data", 71, HorizontalAlignment.Left);
            listViewZK.Columns.Add("nrFaktury", 72, HorizontalAlignment.Left);
                        
            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                string zapytanie = string.Format("SELECT dbo.Zlecenia.idZlecenia, dbo.Zlecenia.data, dbo.Zlecenia.nrFaktury FROM dbo.Zlecenia, dbo.Klienci WHERE (dbo.Zlecenia.idKlienta=dbo.Klienci.idKlienta)and(dbo.Klienci.idKlienta = {0})", idKlienta);

                SqlCommand cmd = new SqlCommand(zapytanie, connection);

                SqlDataReader dr = cmd.ExecuteReader();

                listViewZK.Items.Clear();

                while (dr.Read())
                {

                    ListViewItem lvi = new ListViewItem(dr[0].ToString());
                    for (int i = 1; i < dr.FieldCount; i++)
                    {
                        lvi.SubItems.Add(dr[1].ToString().Remove(11));
                        lvi.SubItems.Add(dr[2].ToString());
                    }
                    listViewZK.Items.Add(lvi);
                }

                dr.Close();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dodajKlientaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ukryjKontrolkiZlecenia();
            ukryjKontrolkiCzesci();
            ukryjKontrolkiUslugi();
            ukryjKontrolkiPracownicy();
            ukryjKontrolkiHarmonogram();
            wyswietlListeKlientow();
            gbDodajKlienta.Visible = true;
            gbUK.Visible = false;
            gbZK.Visible = false;
            
        }

        private void usunKlientaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gbUK.Visible = true;
            ukryjKontrolkiZlecenia();
            ukryjKontrolkiCzesci();
            ukryjKontrolkiUslugi();
            ukryjKontrolkiPracownicy();
            ukryjKontrolkiHarmonogram();
            wyswietlListeKlientow();
            gbDodajKlienta.Visible = false;
            gbZK.Visible = false;
            
        }

        private void wyswietlKlientówToolStripMenuItem(object sender, EventArgs e)
        {
            ukryjKontrolkiZlecenia();
            ukryjKontrolkiCzesci();
            ukryjKontrolkiUslugi();
            ukryjKontrolkiPracownicy();
            ukryjKontrolkiHarmonogram();
            wyswietlListeKlientow();
            gbDodajKlienta.Visible = false;
            gbUK.Visible = false;
            gbZK.Visible = false;
        }

        private void wyswietlZleceniaKlientaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ukryjKontrolkiZlecenia();
            ukryjKontrolkiCzesci();
            ukryjKontrolkiUslugi();
            ukryjKontrolkiPracownicy();
            ukryjKontrolkiHarmonogram();
            wyswietlListeKlientow();
            gbDodajKlienta.Visible = false;
            gbUK.Visible = false; 
            gbZK.Visible = true;
        }

        private void btnDKOk_Click(object sender, EventArgs e)
        {
            Klient k = new Klient();

            k.dodajKlienta(txtDKImie.Text, txtDKNazwisko.Text, txtDKTelefon.Text, txtDKMiasto.Text, txtDKUlica.Text, txtBoxtoInt(txtDKNrLokalu.Text));
            wyswietlListeKlientow();
            txtDKImie.Text = ""; txtDKNazwisko.Text = ""; txtDKTelefon.Text = ""; txtDKMiasto.Text = ""; txtDKUlica.Text = ""; txtDKNrLokalu.Text = "";
           
        }

        private void btnDKAnuluj_Click(object sender, EventArgs e)
        {
            txtDKImie.Text = ""; txtDKNazwisko.Text = ""; txtDKTelefon.Text = ""; txtDKMiasto.Text = ""; txtDKUlica.Text = ""; txtDKNrLokalu.Text = "";
        }

        private void btnUKOk_Click(object sender, EventArgs e)
        {
            Klient k = new Klient();
            k.usunKlienta(txtBoxtoInt(txtUKidKlienta.Text));
            wyswietlListeKlientow();
            txtUKidKlienta.Text = "";
        }

        private void btnUKAnuluj_Click(object sender, EventArgs e)
        {
            txtUKidKlienta.Text = "";
        }

        private void btnKWyswietl_Click(object sender, EventArgs e)
        {
            wyswietlZleceniaKlienta(txtBoxtoInt(txtZKIdKlienta.Text));
        }

        private void wyswietlHarmonogram()
        {
            listViewHarmonogram.Visible = true;
            listViewHarmonogram.View = View.Details;
            listViewHarmonogram.LabelEdit = true;
            listViewHarmonogram.AllowColumnReorder = true;
            listViewHarmonogram.FullRowSelect = true;
            listViewHarmonogram.GridLines = true;
            listViewHarmonogram.Clear();
            
            listViewHarmonogram.Columns.Add("Id dnia", 70, HorizontalAlignment.Left);
            listViewHarmonogram.Columns.Add("data", 71, HorizontalAlignment.Left);
            listViewHarmonogram.Columns.Add("Uzupelniony", 72, HorizontalAlignment.Left);

            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                string zapytanie = string.Format("SELECT *  FROM dbo.Harmonogram");

                SqlCommand cmd = new SqlCommand(zapytanie, connection);

                SqlDataReader dr = cmd.ExecuteReader();

                listViewHarmonogram.Items.Clear();

                while (dr.Read())
                {

                    ListViewItem lvi = new ListViewItem(dr[0].ToString());
                    for (int i = 1; i < dr.FieldCount; i++)
                    {
                        lvi.SubItems.Add(dr[1].ToString().Remove(11));
                       
                        if ((dr[2].ToString())=="True") 
                            lvi.SubItems.Add("Tak");
                        else
                            lvi.SubItems.Add("Nie");

                    }
                    listViewHarmonogram.Items.Add(lvi);
                }

                dr.Close();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void wyswietlListePracownikowWHarmonogramie()
        {
            listViewHarmonogramPracownicy.Visible = true;
            listViewHarmonogramPracownicy.View = View.Details;
            listViewHarmonogramPracownicy.LabelEdit = true;
            listViewHarmonogramPracownicy.AllowColumnReorder = true;
            listViewHarmonogramPracownicy.FullRowSelect = true;
            listViewHarmonogramPracownicy.GridLines = true;
            listViewHarmonogramPracownicy.Clear();
            
            listViewHarmonogramPracownicy.Columns.Add("idPracownika", 80, HorizontalAlignment.Left);
            listViewHarmonogramPracownicy.Columns.Add("Imie", 69, HorizontalAlignment.Left);
            listViewHarmonogramPracownicy.Columns.Add("Nazwisko", 75, HorizontalAlignment.Left);
            listViewHarmonogramPracownicy.Columns.Add("Stanowisko", 77, HorizontalAlignment.Left);
           
            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                SqlCommand cmd = new SqlCommand("SELECT Pracownicy.idPracownika, Osoby.imie, Osoby.nazwisko, Pracownicy.stanowisko FROM Pracownicy INNER JOIN Osoby ON Pracownicy.idOsoby = Osoby.idOsoby", connection);

                SqlDataReader dr = cmd.ExecuteReader();

                listViewHarmonogramPracownicy.Items.Clear();

                while (dr.Read())
                {

                    ListViewItem lvi = new ListViewItem(dr[0].ToString());
                    for (int i = 1; i < dr.FieldCount; i++)
                    {
                        lvi.SubItems.Add(dr[1].ToString());
                        lvi.SubItems.Add(dr[2].ToString());
                        lvi.SubItems.Add(dr[3].ToString());
           
                    }
                    listViewHarmonogramPracownicy.Items.Add(lvi);
                }

                dr.Close();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void wyswietlListePracownikowWDniu(int idDnia)
        {
            listViewHWD.Visible = true;
            listViewHWD.View = View.Details;
            listViewHWD.LabelEdit = true;
            listViewHWD.AllowColumnReorder = true;
            listViewHWD.FullRowSelect = true;
            listViewHWD.GridLines = true;
            listViewHWD.Clear();
            
            listViewHWD.Columns.Add("idPracownika", 34, HorizontalAlignment.Left);
            listViewHWD.Columns.Add("Imie", 66, HorizontalAlignment.Left);
            listViewHWD.Columns.Add("Nazwisko", 66, HorizontalAlignment.Left);
            listViewHWD.Columns.Add("Stanowisko", 67, HorizontalAlignment.Left);
            
            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                string zapytanie = string.Format("SELECT Pracownicy.idPracownika, Osoby.nazwisko, Osoby.imie, Pracownicy.stanowisko FROM Harmonogram_Pracownicy INNER JOIN Pracownicy ON Harmonogram_Pracownicy.idPracownika = Pracownicy.idPracownika INNER JOIN Harmonogram ON Harmonogram_Pracownicy.idDnia = Harmonogram.idDnia INNER JOIN Osoby ON Pracownicy.idOsoby = Osoby.idOsoby WHERE (Harmonogram.idDnia = {0})",idDnia);

                SqlCommand cmd = new SqlCommand(zapytanie, connection);

                SqlDataReader dr = cmd.ExecuteReader();

                listViewHWD.Items.Clear();

                while (dr.Read())
                {

                    ListViewItem lvi = new ListViewItem(dr[0].ToString());
                    for (int i = 1; i < dr.FieldCount; i++)
                    {
                        lvi.SubItems.Add(dr[1].ToString());
                        lvi.SubItems.Add(dr[2].ToString());
                        lvi.SubItems.Add(dr[3].ToString());
                
                    }
                    listViewHWD.Items.Add(lvi);
                }

                dr.Close();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        public void ukryjKontrolkiHarmonogram()
        {
            listViewHarmonogram.Visible = false;
            listViewHarmonogramPracownicy.Visible = false;
            gbHWD.Visible = false;
        }

        private void wyswietlHarmonogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ukryjKontrolkiZlecenia();
            ukryjKontrolkiCzesci();
            ukryjKontrolkiPracownicy();
            ukryjKontrolkiUslugi();
            ukryjKontrolkiKlienci();
            wyswietlHarmonogram();
        }

        private void uzupelnijHarmonogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ukryjKontrolkiZlecenia();
            ukryjKontrolkiCzesci();
            ukryjKontrolkiPracownicy();
            ukryjKontrolkiUslugi();
            ukryjKontrolkiKlienci();
            gbHWD.Visible = true;
            wyswietlHarmonogram();
            wyswietlListePracownikowWHarmonogramie();
            listViewHarmonogramPracownicy.Visible =true;
        }

        private void btnHWDWyswietl_Click(object sender, EventArgs e)
        {
            wyswietlListePracownikowWDniu(txtBoxtoInt(txtHWD.Text));
        }

        private void btnHDP_Click(object sender, EventArgs e)
        {
            Harmonogram h = new Harmonogram();

            if (h.czyJest4PracownikowWDniu(txtBoxtoInt(txtHWD.Text))>=4)
                MessageBox.Show("Dla tego Dnia Sklad jest ustalony");
            else
                h.dodajPracownikaDoDnia(txtBoxtoInt(txtHDP.Text),txtBoxtoInt(txtHWD.Text));

            h.czyJest4PracownikowWDniu(txtBoxtoInt(txtHWD.Text));

            wyswietlHarmonogram();
            wyswietlListePracownikowWHarmonogramie();
            wyswietlListePracownikowWDniu(txtBoxtoInt(txtHWD.Text));
            txtHDP.Text = "";
        }
                 
    }
}
