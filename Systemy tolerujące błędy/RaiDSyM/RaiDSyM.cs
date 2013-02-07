using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RaiDSyM
{
    public partial class RaiDSyM : Form
    {
       
        public static readonly int sleepTime=2;
        
        private Dysk d;
        private Przeplyw p;
        private Pasek pasek;
        private Pasek pasek1;
        private Load l;   
        
        public static int typRaida=-1;
        public static bool pracuj = false;
        public static int nrPasku = -1;
        public static int nrDysku = -1;
        public static bool bladWielokrotny = false;
     
        public RaiDSyM()
        {
           

             InitializeComponent();

            gBoxBledy.Visible = false;
            rbPojedyncze.Checked=true;
          
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        private   void RaiDSyM_Paint(object sender, PaintEventArgs e)
        {
            if (pracuj)
            {
                
                d.rysujRaid(typRaida, e.Graphics, new Point(400, 250));
               
                    try
                    {
                        foreach (Pasek pas in Przeplyw.paski)
                            pas.rysujPasek(e.Graphics, new Point(pas.x, pas.y), pas.nrPasku.ToString(), pas.kolor);
                    }
                    catch (Exception exp) { }

               
                if (!Przeplyw.blad)
                pasek.rysujPasek(e.Graphics, new Point(p.x, p.y), Przeplyw.numerPaska.ToString(), Color.Black);
                else
                    pasek1.rysujPasek(e.Graphics, new Point(p.x, p.y), Przeplyw.numerPaska.ToString(), Color.Black);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void btnPoziomRaid_Click(object sender, EventArgs e)
        {
            bool dobrze = false;

            try
            {
                typRaida = System.Int32.Parse(txtBoxPoziomRaid.Text);

            }
            catch (Exception exp)
            {
                MessageBox.Show("Wprowadzono Błąd przy wprowadzaniu Poziomu RAid");
                dobrze = true;
            }

            if ((typRaida == 1) || (typRaida == 0) || (typRaida == 3))
            {
                pracuj = true;
                txtBoxPoziomRaid.Visible = false;
                btnPoziomRaid.Visible = false;
                lblPoziomRaid.Visible = false;
                gBoxBledy.Visible = true;
       
                d = new Dysk();
                p = new Przeplyw();
                pasek = new Pasek();
                pasek1 = new Pasek();
                l = new Load();
                l.zapis("Program Włączono");

                if (typRaida == 0)
                {
                    rbWielokrotne.Visible = false;
                    rbPojedyncze.Visible = false;
                }
                if (typRaida == 1)
                {
                    lblDlaRaid0.Text = "Dla Raid1 można wstrzykiwać błędy tolerowane przez układ, \n"+
                        " lub błędy przekraczające zakres tolerancji ";
                }
                if (typRaida == 3)
                {
                    lblDlaRaid0.Text = "Dla Raid3 można wstrzykiwać błędy tolerowane przez układ, \n" +
                        " lub błędy przekraczające zakres tolerancji ";
                }
                
            }
            else if (!dobrze)
                MessageBox.Show("Wprowadzono Błąd przy wprowadzaniu Poziomu RAid");
        }

        private void rbPojedyncze_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPojedyncze.Checked)
            {
       
            }
            if (rbWielokrotne.Checked)
            {
                bladWielokrotny = true;
            }
        }

        private void btnWstrzyknij1_Click(object sender, EventArgs e)
        {
            nrPasku = System.Int32.Parse(txtNrPaska1.Text);
            nrDysku = System.Int32.Parse(txtNrDysku1.Text);
            Blad.sprawdzIstnieniePaska(System.Int32.Parse(txtNrDysku1.Text), System.Int32.Parse(txtNrPaska1.Text), typRaida);

        }
    }
}
