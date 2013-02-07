using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Xml.XPath;
using System.Text.RegularExpressions;
using System.IO;

namespace RaiDSyM
{
     class Load
    {

        public  List<byte[]> pobierzPaskiZPliku(string fileName)
        {
            List<byte[]> paskiWejsciowe = new List<byte[]>();

            //for (int i=0; i<200;i++)
            //    paskiWejsciowe.Add(new byte[32]);
          // ArrayList paskiWejsciowe1 = new ArrayList();
             
            XmlDocument xml =new XmlDocument();
            xml.Load(fileName);
            string[] words=new string[34];
           // byte[] byteOut = new byte[32];
            int count = 0;
            

            XmlNode wezelGlowne = xml.DocumentElement.FirstChild;
         //   XmlNodeList xmlList = xml.SelectNodes("root/");

            do
            {
                  byte[] byteOut = new byte[32];
                words = Regex.Split(wezelGlowne.InnerText.Trim(),@"");
                for (int i = 0; i < 32; i++)
                {
                    byteOut[i] = Convert.ToByte(words[i + 1]);
                }
                
                
                paskiWejsciowe.Add(byteOut);
                count++;
                                wezelGlowne = wezelGlowne.NextSibling;
            }
            while (wezelGlowne!=null);


            return paskiWejsciowe ;
        }

        public  void zapis(String komunikat)
        {
            StreamWriter SW;
            SW = File.AppendText("Wyjscie.txt");
            String text = string.Format("{0}  {1}", DateTime.Now,komunikat);
            SW.WriteLine(text);
            SW.Close();
         }
            }
    
}
