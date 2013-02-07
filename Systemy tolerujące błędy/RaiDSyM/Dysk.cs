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
    class Dysk
    {
        private  readonly int szerokoscDysku = 160;
        private  readonly int wysokoscDysku = 480;
        
        public  void rysujDysk( Graphics g,Point p, int numerDysku)
        {
            StringFormat format=new StringFormat();
         
            Point p1 = new Point();
            p1.X = p.X;
            p1.Y = p.Y;           
            Rectangle ramka = new Rectangle(p.X,p.Y,szerokoscDysku,wysokoscDysku);            
            g.DrawRectangle(new Pen(Color.Black,2), ramka);

            g.DrawLine(new Pen(Color.Silver,3),p.X+80,p.Y,p.X+80,p.Y-150);

            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    g.DrawRectangle(new Pen(Color.Silver), p1.X, p1.Y, 40, 30);
                    p1.X += 40;
                }
                p1.X = p.X;
                p1.Y += 30;
            }   
            g.DrawString( "Dysk "+ numerDysku.ToString(),new Font(new FontFamily("Times new roman"),12F,FontStyle.Bold),new SolidBrush(Color.Black),p.X+50,p.Y+485);
        }
        public void rysujRaid(int typRaidu, Graphics g, Point p )
        {
            int nDysk = 0;
            switch (typRaidu)
            {
                case 0:
                    g.DrawLine(new Pen(Color.Silver, 3), 0, 100, 702, 100);
                    for (int i = 0; i < 2; i++)
                    {
                        rysujDysk(g, p,nDysk);
                        p.X += 220;
                        nDysk++;
                    }
                    break;
                case 1:
                    g.DrawLine(new Pen(Color.Silver, 3), 0, 100, 702, 100);
                    for (int i = 0; i < 2; i++)
                    {
                        rysujDysk(g, p,nDysk);
                        p.X += 220;
                        nDysk++;
                    }
                    break;
                case 3:
                    g.DrawLine(new Pen(Color.Silver, 3), 0, 100, 1142, 100);
                    for (int i = 0; i < 4; i++)
                    {
                        rysujDysk(g, p,nDysk);
                        p.X += 220;
                        nDysk++;
                    }
                    g.DrawRectangle(new Pen(Color.Red), 1060, 250, 160, 480);
                    g.DrawString("Dysk Parzystości", new Font(new FontFamily("Times new roman"), 12F, FontStyle.Bold), new SolidBrush(Color.Black), 1070, 750);
                    g.DrawString("Kontroler Parzystości", new Font(new FontFamily("Times new roman"), 10F, FontStyle.Bold), new SolidBrush(Color.Black),1070,38);
                    g.FillEllipse(new SolidBrush(Color.Black), 1096, 62, 80, 80);
                    break;
                default:

                    break;
            }
        }
    }
}
