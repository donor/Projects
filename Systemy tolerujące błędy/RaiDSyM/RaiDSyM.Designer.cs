namespace RaiDSyM
{
    partial class RaiDSyM
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
            this.components = new System.ComponentModel.Container();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.txtBoxPoziomRaid = new System.Windows.Forms.TextBox();
            this.btnPoziomRaid = new System.Windows.Forms.Button();
            this.lblPoziomRaid = new System.Windows.Forms.Label();
            this.gBoxBledy = new System.Windows.Forms.GroupBox();
            this.btnWstrzyknij1 = new System.Windows.Forms.Button();
            this.txtNrPaska1 = new System.Windows.Forms.TextBox();
            this.txtNrDysku1 = new System.Windows.Forms.TextBox();
            this.lbNumerPaska1 = new System.Windows.Forms.Label();
            this.lblNrDysku1 = new System.Windows.Forms.Label();
            this.rbWielokrotne = new System.Windows.Forms.RadioButton();
            this.rbPojedyncze = new System.Windows.Forms.RadioButton();
            this.lblDlaRaid0 = new System.Windows.Forms.Label();
            this.gBoxBledy.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 1;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // txtBoxPoziomRaid
            // 
            this.txtBoxPoziomRaid.Location = new System.Drawing.Point(597, 287);
            this.txtBoxPoziomRaid.Name = "txtBoxPoziomRaid";
            this.txtBoxPoziomRaid.Size = new System.Drawing.Size(65, 20);
            this.txtBoxPoziomRaid.TabIndex = 0;
            this.txtBoxPoziomRaid.Text = "0";
            // 
            // btnPoziomRaid
            // 
            this.btnPoziomRaid.Location = new System.Drawing.Point(686, 282);
            this.btnPoziomRaid.Name = "btnPoziomRaid";
            this.btnPoziomRaid.Size = new System.Drawing.Size(82, 28);
            this.btnPoziomRaid.TabIndex = 1;
            this.btnPoziomRaid.Text = "OK";
            this.btnPoziomRaid.UseVisualStyleBackColor = true;
            this.btnPoziomRaid.Click += new System.EventHandler(this.btnPoziomRaid_Click);
            // 
            // lblPoziomRaid
            // 
            this.lblPoziomRaid.AutoSize = true;
            this.lblPoziomRaid.Location = new System.Drawing.Point(393, 290);
            this.lblPoziomRaid.Name = "lblPoziomRaid";
            this.lblPoziomRaid.Size = new System.Drawing.Size(170, 13);
            this.lblPoziomRaid.TabIndex = 2;
            this.lblPoziomRaid.Text = "Poziom Raid (wprowadź 0, 1 lub 3)";
            // 
            // gBoxBledy
            // 
            this.gBoxBledy.Controls.Add(this.lblDlaRaid0);
            this.gBoxBledy.Controls.Add(this.btnWstrzyknij1);
            this.gBoxBledy.Controls.Add(this.txtNrPaska1);
            this.gBoxBledy.Controls.Add(this.txtNrDysku1);
            this.gBoxBledy.Controls.Add(this.lbNumerPaska1);
            this.gBoxBledy.Controls.Add(this.lblNrDysku1);
            this.gBoxBledy.Controls.Add(this.rbWielokrotne);
            this.gBoxBledy.Controls.Add(this.rbPojedyncze);
            this.gBoxBledy.Location = new System.Drawing.Point(12, 240);
            this.gBoxBledy.Name = "gBoxBledy";
            this.gBoxBledy.Size = new System.Drawing.Size(361, 481);
            this.gBoxBledy.TabIndex = 3;
            this.gBoxBledy.TabStop = false;
            this.gBoxBledy.Text = "Błędy";
            // 
            // btnWstrzyknij1
            // 
            this.btnWstrzyknij1.Location = new System.Drawing.Point(221, 97);
            this.btnWstrzyknij1.Name = "btnWstrzyknij1";
            this.btnWstrzyknij1.Size = new System.Drawing.Size(116, 20);
            this.btnWstrzyknij1.TabIndex = 3;
            this.btnWstrzyknij1.Text = "Wstrzyknij Błąd";
            this.btnWstrzyknij1.UseVisualStyleBackColor = true;
            this.btnWstrzyknij1.Click += new System.EventHandler(this.btnWstrzyknij1_Click);
            // 
            // txtNrPaska1
            // 
            this.txtNrPaska1.Location = new System.Drawing.Point(75, 97);
            this.txtNrPaska1.Name = "txtNrPaska1";
            this.txtNrPaska1.Size = new System.Drawing.Size(100, 20);
            this.txtNrPaska1.TabIndex = 2;
            // 
            // txtNrDysku1
            // 
            this.txtNrDysku1.Location = new System.Drawing.Point(75, 70);
            this.txtNrDysku1.Name = "txtNrDysku1";
            this.txtNrDysku1.Size = new System.Drawing.Size(100, 20);
            this.txtNrDysku1.TabIndex = 2;
            // 
            // lbNumerPaska1
            // 
            this.lbNumerPaska1.AutoSize = true;
            this.lbNumerPaska1.Location = new System.Drawing.Point(6, 100);
            this.lbNumerPaska1.Name = "lbNumerPaska1";
            this.lbNumerPaska1.Size = new System.Drawing.Size(51, 13);
            this.lbNumerPaska1.TabIndex = 1;
            this.lbNumerPaska1.Text = "Nr Paska";
            // 
            // lblNrDysku1
            // 
            this.lblNrDysku1.AutoSize = true;
            this.lblNrDysku1.Location = new System.Drawing.Point(6, 73);
            this.lblNrDysku1.Name = "lblNrDysku1";
            this.lblNrDysku1.Size = new System.Drawing.Size(51, 13);
            this.lblNrDysku1.TabIndex = 1;
            this.lblNrDysku1.Text = "Nr Dysku";
            // 
            // rbWielokrotne
            // 
            this.rbWielokrotne.AutoSize = true;
            this.rbWielokrotne.Location = new System.Drawing.Point(6, 42);
            this.rbWielokrotne.Name = "rbWielokrotne";
            this.rbWielokrotne.Size = new System.Drawing.Size(313, 17);
            this.rbWielokrotne.TabIndex = 0;
            this.rbWielokrotne.TabStop = true;
            this.rbWielokrotne.Text = "błąd  przekraczający zakres tolerancji symulowanego układu";
            this.rbWielokrotne.UseVisualStyleBackColor = true;
            // 
            // rbPojedyncze
            // 
            this.rbPojedyncze.AutoSize = true;
            this.rbPojedyncze.Location = new System.Drawing.Point(6, 19);
            this.rbPojedyncze.Name = "rbPojedyncze";
            this.rbPojedyncze.Size = new System.Drawing.Size(162, 17);
            this.rbPojedyncze.TabIndex = 0;
            this.rbPojedyncze.TabStop = true;
            this.rbPojedyncze.Text = "błąd tolerowalny przez układ";
            this.rbPojedyncze.UseVisualStyleBackColor = true;
            this.rbPojedyncze.CheckedChanged += new System.EventHandler(this.rbPojedyncze_CheckedChanged);
            // 
            // lblDlaRaid0
            // 
            this.lblDlaRaid0.AutoSize = true;
            this.lblDlaRaid0.Location = new System.Drawing.Point(11, 397);
            this.lblDlaRaid0.Name = "lblDlaRaid0";
            this.lblDlaRaid0.Size = new System.Drawing.Size(344, 13);
            this.lblDlaRaid0.TabIndex = 4;
            this.lblDlaRaid0.Text = "Dla Raid0 każdy wstrzyknięty błąd przekracza zakres tolerancji układu.";
            // 
            // RaiDSyM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1264, 812);
            this.Controls.Add(this.gBoxBledy);
            this.Controls.Add(this.lblPoziomRaid);
            this.Controls.Add(this.btnPoziomRaid);
            this.Controls.Add(this.txtBoxPoziomRaid);
            this.Name = "RaiDSyM";
            this.Text = "Form1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.RaiDSyM_Paint);
            this.gBoxBledy.ResumeLayout(false);
            this.gBoxBledy.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.TextBox txtBoxPoziomRaid;
        private System.Windows.Forms.Button btnPoziomRaid;
        private System.Windows.Forms.Label lblPoziomRaid;
        private System.Windows.Forms.GroupBox gBoxBledy;
        private System.Windows.Forms.RadioButton rbPojedyncze;
        private System.Windows.Forms.Button btnWstrzyknij1;
        private System.Windows.Forms.TextBox txtNrPaska1;
        private System.Windows.Forms.TextBox txtNrDysku1;
        private System.Windows.Forms.Label lbNumerPaska1;
        private System.Windows.Forms.Label lblNrDysku1;
        private System.Windows.Forms.Label lblDlaRaid0;
        public System.Windows.Forms.RadioButton rbWielokrotne;
    }
}

