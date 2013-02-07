using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace RaiDSyM
{
[Serializable]
    class Pasek
    {
        public  int x;
        public int y;
        public int nrPasku;
        public Color kolor;
    //{
    //    get { return kolor; }
    //       //  set {kolor =value; }
    //}


        public Pasek()
        {
        }
        //public RaiDSyM rds;
        public Pasek(int x,int y, int nrPasku, Color kolor)
        {
            this.kolor=kolor;
            this.x = x;
            this.y = y;
            this.nrPasku = nrPasku;          
        }

        public void rysujPasek(Graphics g,Point p,string nrPasku,Color k)
        {
            g.DrawRectangle(new Pen( Color.DarkGray), p.X, p.Y, 40, 30);
            g.FillRectangle(new SolidBrush(k), p.X+1, p.Y+1, 39, 29);
            g.DrawString( nrPasku, new Font(new FontFamily("Times new roman"), 15F, FontStyle.Bold), new SolidBrush(Color.White), (float)(p.X+0.2) , p.Y + 5);
        }
    }


}
