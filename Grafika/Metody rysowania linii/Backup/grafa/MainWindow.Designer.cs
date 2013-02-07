using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;




namespace grafa
{
   
    partial class MainWindow
    {
       

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel = new System.Windows.Forms.Panel();
            this.imgobrazek = new System.Windows.Forms.PictureBox();
            this.txtBox = new System.Windows.Forms.TextBox();
            this.txtBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblKolor = new System.Windows.Forms.Label();
            this.btnKolor = new System.Windows.Forms.Button();
            this.btnCzysc = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.pToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.zToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.iToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButtonKwadrat = new System.Windows.Forms.RadioButton();
            this.radioButtonDuzeKolko = new System.Windows.Forms.RadioButton();
            this.radioButtonMaleKolko = new System.Windows.Forms.RadioButton();
            this.radioButtonBrakPogrobienia = new System.Windows.Forms.RadioButton();
            this.panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgobrazek)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.BackColor = System.Drawing.Color.White;
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel.Controls.Add(this.imgobrazek);
            this.panel.Location = new System.Drawing.Point(26, 48);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(400, 400);
            this.panel.TabIndex = 0;
            // 
            // imgobrazek
            // 
            this.imgobrazek.Location = new System.Drawing.Point(-1, -1);
            this.imgobrazek.Name = "imgobrazek";
            this.imgobrazek.Size = new System.Drawing.Size(400, 400);
            this.imgobrazek.TabIndex = 0;
            this.imgobrazek.TabStop = false;
            this.imgobrazek.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imgobrazek_MouseDown);
            this.imgobrazek.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imgobrazek_MouseUp);
            // 
            // txtBox
            // 
            this.txtBox.Location = new System.Drawing.Point(105, 27);
            this.txtBox.Name = "txtBox";
            this.txtBox.Size = new System.Drawing.Size(92, 20);
            this.txtBox.TabIndex = 1;
            // 
            // txtBox1
            // 
            this.txtBox1.Location = new System.Drawing.Point(105, 60);
            this.txtBox1.Name = "txtBox1";
            this.txtBox1.Size = new System.Drawing.Size(92, 20);
            this.txtBox1.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Poczatek odcinka";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Koniec odcinka";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtBox);
            this.groupBox1.Controls.Add(this.txtBox1);
            this.groupBox1.Location = new System.Drawing.Point(455, 335);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(213, 113);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Wspolrzedne";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblKolor);
            this.groupBox2.Controls.Add(this.btnKolor);
            this.groupBox2.Controls.Add(this.btnCzysc);
            this.groupBox2.Location = new System.Drawing.Point(455, 48);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(213, 113);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Kontrola";
            // 
            // lblKolor
            // 
            this.lblKolor.BackColor = System.Drawing.Color.Black;
            this.lblKolor.Location = new System.Drawing.Point(125, 63);
            this.lblKolor.Name = "lblKolor";
            this.lblKolor.Size = new System.Drawing.Size(27, 27);
            this.lblKolor.TabIndex = 1;
            // 
            // btnKolor
            // 
            this.btnKolor.Location = new System.Drawing.Point(6, 63);
            this.btnKolor.Name = "btnKolor";
            this.btnKolor.Size = new System.Drawing.Size(104, 28);
            this.btnKolor.TabIndex = 0;
            this.btnKolor.Text = "Kolor";
            this.btnKolor.UseVisualStyleBackColor = true;
            this.btnKolor.Click += new System.EventHandler(this.btnKolor_Click);
            // 
            // btnCzysc
            // 
            this.btnCzysc.Location = new System.Drawing.Point(6, 19);
            this.btnCzysc.Name = "btnCzysc";
            this.btnCzysc.Size = new System.Drawing.Size(104, 28);
            this.btnCzysc.TabIndex = 0;
            this.btnCzysc.Text = "Czysc";
            this.btnCzysc.UseVisualStyleBackColor = true;
            this.btnCzysc.Click += new System.EventHandler(this.btnCzysc_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pToolStripMenuItem,
            this.pToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(685, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // pToolStripMenuItem
            // 
            this.pToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zToolStripMenuItem});
            this.pToolStripMenuItem.Name = "pToolStripMenuItem";
            this.pToolStripMenuItem.Size = new System.Drawing.Size(34, 20);
            this.pToolStripMenuItem.Text = "Plik";
            // 
            // zToolStripMenuItem
            // 
            this.zToolStripMenuItem.Name = "zToolStripMenuItem";
            this.zToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.zToolStripMenuItem.Text = "Wyjscie";
            this.zToolStripMenuItem.Click += new System.EventHandler(this.zToolStripMenuItem_Click);
            // 
            // pToolStripMenuItem1
            // 
            this.pToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zToolStripMenuItem1,
            this.iToolStripMenuItem,
            this.aToolStripMenuItem});
            this.pToolStripMenuItem1.Name = "pToolStripMenuItem1";
            this.pToolStripMenuItem1.Size = new System.Drawing.Size(50, 20);
            this.pToolStripMenuItem1.Text = "Pomoc";
            // 
            // zToolStripMenuItem1
            // 
            this.zToolStripMenuItem1.Name = "zToolStripMenuItem1";
            this.zToolStripMenuItem1.Size = new System.Drawing.Size(171, 22);
            this.zToolStripMenuItem1.Text = "Autorzy";
            this.zToolStripMenuItem1.Click += new System.EventHandler(this.zToolStripMenuItem1_Click);
            // 
            // iToolStripMenuItem
            // 
            this.iToolStripMenuItem.Name = "iToolStripMenuItem";
            this.iToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.iToolStripMenuItem.Text = "Instrukcja Obslugi";
            this.iToolStripMenuItem.Click += new System.EventHandler(this.iToolStripMenuItem_Click);
            // 
            // aToolStripMenuItem
            // 
            this.aToolStripMenuItem.Name = "aToolStripMenuItem";
            this.aToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.aToolStripMenuItem.Text = "Algorytm";
            this.aToolStripMenuItem.Click += new System.EventHandler(this.aToolStripMenuItem_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButtonKwadrat);
            this.groupBox3.Controls.Add(this.radioButtonDuzeKolko);
            this.groupBox3.Controls.Add(this.radioButtonMaleKolko);
            this.groupBox3.Controls.Add(this.radioButtonBrakPogrobienia);
            this.groupBox3.Location = new System.Drawing.Point(455, 184);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(213, 145);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Pogrobienie";
            // 
            // radioButtonKwadrat
            // 
            this.radioButtonKwadrat.AutoSize = true;
            this.radioButtonKwadrat.Location = new System.Drawing.Point(22, 105);
            this.radioButtonKwadrat.Name = "radioButtonKwadrat";
            this.radioButtonKwadrat.Size = new System.Drawing.Size(126, 17);
            this.radioButtonKwadrat.TabIndex = 0;
            this.radioButtonKwadrat.Text = "kwadrat o boku a=10";
            this.radioButtonKwadrat.UseVisualStyleBackColor = true;
            // 
            // radioButtonDuzeKolko
            // 
            this.radioButtonDuzeKolko.AutoSize = true;
            this.radioButtonDuzeKolko.Location = new System.Drawing.Point(22, 82);
            this.radioButtonDuzeKolko.Name = "radioButtonDuzeKolko";
            this.radioButtonDuzeKolko.Size = new System.Drawing.Size(95, 17);
            this.radioButtonDuzeKolko.TabIndex = 0;
            this.radioButtonDuzeKolko.Text = "duze kolko r=4";
            this.radioButtonDuzeKolko.UseVisualStyleBackColor = true;
            // 
            // radioButtonMaleKolko
            // 
            this.radioButtonMaleKolko.AutoSize = true;
            this.radioButtonMaleKolko.Location = new System.Drawing.Point(22, 59);
            this.radioButtonMaleKolko.Name = "radioButtonMaleKolko";
            this.radioButtonMaleKolko.Size = new System.Drawing.Size(94, 17);
            this.radioButtonMaleKolko.TabIndex = 0;
            this.radioButtonMaleKolko.Text = "male kolko r=2";
            this.radioButtonMaleKolko.UseVisualStyleBackColor = true;
            // 
            // radioButtonBrakPogrobienia
            // 
            this.radioButtonBrakPogrobienia.AutoSize = true;
            this.radioButtonBrakPogrobienia.Checked = true;
            this.radioButtonBrakPogrobienia.Location = new System.Drawing.Point(22, 36);
            this.radioButtonBrakPogrobienia.Name = "radioButtonBrakPogrobienia";
            this.radioButtonBrakPogrobienia.Size = new System.Drawing.Size(104, 17);
            this.radioButtonBrakPogrobienia.TabIndex = 0;
            this.radioButtonBrakPogrobienia.TabStop = true;
            this.radioButtonBrakPogrobienia.Text = "brak pogrobienia";
            this.radioButtonBrakPogrobienia.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(685, 469);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.menuStrip1);
            this.Name = "MainWindow";
            this.Text = "Rysowanie odcinka";
            this.panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgobrazek)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.PictureBox imgobrazek;
        private TextBox txtBox;
        private TextBox txtBox1;
        private Label label4;
        private Label label2;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button btnCzysc;
        private Button btnKolor;
        private Label lblKolor;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem pToolStripMenuItem;
        private ToolStripMenuItem zToolStripMenuItem;
        private ToolStripMenuItem pToolStripMenuItem1;
        private ToolStripMenuItem zToolStripMenuItem1;
        private ToolStripMenuItem iToolStripMenuItem;
        private ToolStripMenuItem aToolStripMenuItem;
        private GroupBox groupBox3;
        private RadioButton radioButtonKwadrat;
        private RadioButton radioButtonDuzeKolko;
        private RadioButton radioButtonMaleKolko;
        private RadioButton radioButtonBrakPogrobienia;
        

      
    }
}

