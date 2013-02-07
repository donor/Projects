using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;



namespace grafa
{
    public partial class MainWindow : Form
    {
        private Graphics g;
        
        

        public MainWindow()
        {
            InitializeComponent();
            imgobrazek.Image = bitmapa;
            g = Graphics.FromImage(imgobrazek.Image);

        }
       
        
        private int x1;
        private int y1;
        private int x2;
        private int y2;



        public Bitmap bitmapa = new Bitmap(400, 400);
        
        private void okrag(int x, int y, int r, bool status)
        {
            int xc = 0, yc = r, x0 = x, y0 = y, ix = x, iy = y;
            int aa, aa2, d, dx, dy;
            status = false;
            aa = r * r;
            aa2 = aa + aa;
            d = aa - aa * r + (aa / 4);
            dx = 0;
            dy = aa2 * r;
            while (dx <= dy)
            {
               if ((ix + r >= 400) || (ix - r <= 0) || (iy + r >= 400) || (iy - r <= 0))
                {
                    status = true;
                    return;
                }


                bitmapa.SetPixel(x + yc, y - xc, lblKolor.BackColor);
                bitmapa.SetPixel(x + xc, y - yc, lblKolor.BackColor);
                bitmapa.SetPixel(x - xc, y - yc, lblKolor.BackColor);
                bitmapa.SetPixel(x - yc, y - xc, lblKolor.BackColor);
                bitmapa.SetPixel(x - yc, y + xc, lblKolor.BackColor);
                bitmapa.SetPixel(x - xc, y + yc, lblKolor.BackColor);
                bitmapa.SetPixel(x + xc, y + yc, lblKolor.BackColor);
                bitmapa.SetPixel(x + yc, y + xc, lblKolor.BackColor);
                if (d > 0)
                {
                    yc--;
                    dy -= aa2;
                    d -= dy;
                }
                xc++;
                dx += aa2;
                d += aa + dx;

            }
            status = false;

          

            for (int i = 1; i < r; i++) //wypelnienie okręgu pokrzez rysowanie okregow o tym saym srodku i 
            {                           //promieniach z zakresu od 1 do r
                okrag(x, y, i, status);
            }
        }

        private void Kwadrat(int x, int y, int a,bool status)
        {
            int xp = x,yp = y;
            status = false;

            if ((yp-a/2<=0)||(xp-a/2<=0)||(yp+a/2>=400)||(xp+a/2>=400))
            {
                status = true;
                return;
            }

            for (int j=yp-a/2; j <= yp+a/2; j++)                //rysowanie kwadratu o srodku w punkce x,y poprzez
            {                                                   //wstawianie pixeli tworzac a-linii o dlugosci a
                for (int i = xp - a / 2; i <= xp + a / 2; i++)
                    bitmapa.SetPixel(i, j, lblKolor.BackColor);

            }
            status = false;

          }


        private void Rysyj_odcinek(int x1, int y1, int x2, int y2,int rodzaj)
        {
            int x = 0, tempx = 0, tempy = 0;
            float dy = 0, dx = 0, y = 0, m = 0;
            bool status = false;

            
                if ((x2 <= 0) || (x2 >= 400) || (y2 <= 0) || (y2 >= 400))
                    return;

                dy = y2 - y1;
                dx = x2 - x1;

                m = dy / dx;


                if ((m > 1) || (m < -1)) // rysowanie dla m>1 lub m<-1
                {
                    m = dx / dy;
                    float X = 0; X = x1;
                    int Y;
                    if ((y1 > y2)) //warunek na rysownie gdy y2 jest mniejsze od y1
                    {                   //punkt P1 jest zamieniany z punktem P2
                        tempx = x2;
                        x2 = x1;
                        x1 = tempx;
                        tempy = y2;
                        y2 = y1;
                        y1 = tempy;
                    }
                    X = x1;
                    for (Y = y1; Y <= y2; Y++)
                    {
bitmapa.SetPixel((int)(X + 0.5), Y, lblKolor.BackColor);

                        switch (rodzaj)
                        {
                            case 1:   okrag((int)(X + 0.5), Y, 2, status); break; 
                            case 2:  okrag((int)(X + 0.5), Y, 4, status); break; 
                            case 3: Kwadrat((int)(X + 0.5), Y, 10,status); break;
                            default:   break;
                        }
                        if (status == true)
                            return;
                        X += m;

                    }

                }
                else                      //rysowanie dla -1<=m<=1
                {
                    if ((x1 - x2) > 0)    //warunek rysowanie gdy x2 jest mniejsze od x1
                    {                     //punkt P1 zamieniamy z punktem P2
                        tempx = x2;
                        x2 = x1;
                        x1 = tempx;
                        tempy = y2;
                        y2 = y1;
                        y1 = tempy;
                    }
                    y = y1;
                    for (x = x1; x <= x2; x++)
                    {

                        bitmapa.SetPixel(x,(int)(y + 0.5), lblKolor.BackColor);
                        switch (rodzaj)
                        {
                            case 1: {bitmapa.SetPixel(x,(int)(y + 0.5), lblKolor.BackColor);okrag(x,(int)(y + 0.5),  2, status); break;}
                            case 2: { bitmapa.SetPixel(x, (int)(y + 0.5), lblKolor.BackColor); okrag(x, (int)(y + 0.5), 4, status); break; }
                            case 3: Kwadrat(x,(int)(y + 0.5), 10,status); break;
                            default:      break;
                        }
                        if (status == true) return;
                        y += m;

                    }
                }
                     
        }



        private void imgobrazek_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x2 = e.X;
                y2 = e.Y;
                   int rodzaj=0;


                txtBox1.Text = string.Format("{0}, {1} ", x2, y2);

                if (radioButtonMaleKolko.Checked)
                   rodzaj=1;
                
                if (radioButtonDuzeKolko.Checked)
                    rodzaj = 2;

                if (radioButtonKwadrat.Checked)
                    rodzaj = 3;

                if (radioButtonBrakPogrobienia.Checked)
                    rodzaj = 0;
                    
                    Rysyj_odcinek(x1, y1, x2, y2,rodzaj);
                
                imgobrazek.Refresh();
            }
        }

        private void imgobrazek_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x1 = e.X;
                y1 = e.Y;
                txtBox.Text = string.Format("{0}, {1} ", e.X, e.Y);
            }
        }

        private void btnCzysc_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            imgobrazek.Refresh();
        }

        public void btnKolor_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = lblKolor.BackColor;
            dialog.ShowDialog();
            lblKolor.BackColor = dialog.Color;

        }

        private void zToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void zToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Autorzy a = new Autorzy();
            a.ShowDialog();
        }

        private void iToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InstrukcjaObslugi io1 = new InstrukcjaObslugi();
            io1.ShowDialog();
        }

        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Algorytm a = new Algorytm();
            a.ShowDialog();
        }

                     
    }
}

 