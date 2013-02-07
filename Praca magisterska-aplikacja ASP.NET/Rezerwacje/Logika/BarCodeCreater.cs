using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Text;

namespace Rezerwacje.Logika
{
    public class BarCodeCreater
    {


        public Bitmap generateBarcode(string data)
        {
            Bitmap b = new Bitmap(1, 1);
            Font font = new Font("Free 3 of 9", 60, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);

            Graphics g = Graphics.FromImage(b);

            SizeF dSize = g.MeasureString(data, font);
            b = new Bitmap(b, dSize.ToSize());
            g = Graphics.FromImage(b);

            g.Clear(Color.White);

            g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;

            g.DrawString(data, font, new SolidBrush(Color.Black), 0, 0);

            g.DrawString(data, new Font("Times new Romanow", 9, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point), new SolidBrush(Color.Black), 65, 58);

            g.Flush();
            font.Dispose();
            g.Dispose();

            return b;

        }


    }
}