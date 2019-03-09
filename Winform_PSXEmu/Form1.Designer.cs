namespace Winform_PSXEmu
{
    partial class Form1
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btn_Open = new System.Windows.Forms.Button();
            this.lsbx_BIOS = new System.Windows.Forms.ListBox();
            this.lbl_desc_display = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lsbx_RAM = new System.Windows.Forms.ListBox();
            this.btn_Reset = new System.Windows.Forms.Button();
            this.btn_formbios = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbl_H = new System.Windows.Forms.Label();
            this.lbl_D = new System.Windows.Forms.Label();
            this.lbl_B = new System.Windows.Forms.Label();
            this.lbl_A = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbl_L = new System.Windows.Forms.Label();
            this.lbl_E = new System.Windows.Forms.Label();
            this.lbl_C = new System.Windows.Forms.Label();
            this.lbl_F = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(228, 106);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(160, 144);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btn_Open
            // 
            this.btn_Open.Location = new System.Drawing.Point(671, 588);
            this.btn_Open.Name = "btn_Open";
            this.btn_Open.Size = new System.Drawing.Size(75, 23);
            this.btn_Open.TabIndex = 1;
            this.btn_Open.Text = "Open";
            this.btn_Open.UseVisualStyleBackColor = true;
            this.btn_Open.Click += new System.EventHandler(this.btn_Open_Click);
            // 
            // lsbx_BIOS
            // 
            this.lsbx_BIOS.FormattingEnabled = true;
            this.lsbx_BIOS.Location = new System.Drawing.Point(37, 386);
            this.lsbx_BIOS.Name = "lsbx_BIOS";
            this.lsbx_BIOS.Size = new System.Drawing.Size(82, 225);
            this.lsbx_BIOS.TabIndex = 2;
            // 
            // lbl_desc_display
            // 
            this.lbl_desc_display.AutoSize = true;
            this.lbl_desc_display.Location = new System.Drawing.Point(225, 90);
            this.lbl_desc_display.Name = "lbl_desc_display";
            this.lbl_desc_display.Size = new System.Drawing.Size(44, 13);
            this.lbl_desc_display.TabIndex = 3;
            this.lbl_desc_display.Text = "Display:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 370);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Byte List of Bios:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(432, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "RAM:";
            // 
            // lsbx_RAM
            // 
            this.lsbx_RAM.FormattingEnabled = true;
            this.lsbx_RAM.Location = new System.Drawing.Point(435, 106);
            this.lsbx_RAM.Name = "lsbx_RAM";
            this.lsbx_RAM.Size = new System.Drawing.Size(211, 225);
            this.lsbx_RAM.TabIndex = 6;
            // 
            // btn_Reset
            // 
            this.btn_Reset.Location = new System.Drawing.Point(37, 271);
            this.btn_Reset.Name = "btn_Reset";
            this.btn_Reset.Size = new System.Drawing.Size(75, 23);
            this.btn_Reset.TabIndex = 7;
            this.btn_Reset.Text = "Reset";
            this.btn_Reset.UseVisualStyleBackColor = true;
            this.btn_Reset.Click += new System.EventHandler(this.btn_Reset_Click);
            // 
            // btn_formbios
            // 
            this.btn_formbios.Location = new System.Drawing.Point(37, 337);
            this.btn_formbios.Name = "btn_formbios";
            this.btn_formbios.Size = new System.Drawing.Size(75, 23);
            this.btn_formbios.TabIndex = 8;
            this.btn_formbios.Text = "BIOS list";
            this.btn_formbios.UseVisualStyleBackColor = true;
            this.btn_formbios.Click += new System.EventHandler(this.btn_formbios_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(19, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(166, 48);
            this.button1.TabIndex = 9;
            this.button1.Text = "TEST async methods aka startemu instead of read bios";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbl_H);
            this.groupBox1.Controls.Add(this.lbl_D);
            this.groupBox1.Controls.Add(this.lbl_B);
            this.groupBox1.Controls.Add(this.lbl_A);
            this.groupBox1.Location = new System.Drawing.Point(37, 79);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(71, 177);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "HIGH";
            // 
            // lbl_H
            // 
            this.lbl_H.AutoSize = true;
            this.lbl_H.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_H.Location = new System.Drawing.Point(6, 137);
            this.lbl_H.Name = "lbl_H";
            this.lbl_H.Size = new System.Drawing.Size(21, 20);
            this.lbl_H.TabIndex = 14;
            this.lbl_H.Text = "H";
            // 
            // lbl_D
            // 
            this.lbl_D.AutoSize = true;
            this.lbl_D.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_D.Location = new System.Drawing.Point(6, 99);
            this.lbl_D.Name = "lbl_D";
            this.lbl_D.Size = new System.Drawing.Size(21, 20);
            this.lbl_D.TabIndex = 13;
            this.lbl_D.Text = "D";
            // 
            // lbl_B
            // 
            this.lbl_B.AutoSize = true;
            this.lbl_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_B.Location = new System.Drawing.Point(6, 65);
            this.lbl_B.Name = "lbl_B";
            this.lbl_B.Size = new System.Drawing.Size(20, 20);
            this.lbl_B.TabIndex = 12;
            this.lbl_B.Text = "B";
            // 
            // lbl_A
            // 
            this.lbl_A.AutoSize = true;
            this.lbl_A.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_A.Location = new System.Drawing.Point(6, 30);
            this.lbl_A.Name = "lbl_A";
            this.lbl_A.Size = new System.Drawing.Size(20, 20);
            this.lbl_A.TabIndex = 11;
            this.lbl_A.Text = "A";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbl_L);
            this.groupBox2.Controls.Add(this.lbl_E);
            this.groupBox2.Controls.Add(this.lbl_C);
            this.groupBox2.Controls.Add(this.lbl_F);
            this.groupBox2.Location = new System.Drawing.Point(114, 81);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(71, 175);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "LOW";
            // 
            // lbl_L
            // 
            this.lbl_L.AutoSize = true;
            this.lbl_L.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_L.Location = new System.Drawing.Point(6, 137);
            this.lbl_L.Name = "lbl_L";
            this.lbl_L.Size = new System.Drawing.Size(18, 20);
            this.lbl_L.TabIndex = 14;
            this.lbl_L.Text = "L";
            // 
            // lbl_E
            // 
            this.lbl_E.AutoSize = true;
            this.lbl_E.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_E.Location = new System.Drawing.Point(6, 99);
            this.lbl_E.Name = "lbl_E";
            this.lbl_E.Size = new System.Drawing.Size(20, 20);
            this.lbl_E.TabIndex = 13;
            this.lbl_E.Text = "E";
            // 
            // lbl_C
            // 
            this.lbl_C.AutoSize = true;
            this.lbl_C.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_C.Location = new System.Drawing.Point(6, 65);
            this.lbl_C.Name = "lbl_C";
            this.lbl_C.Size = new System.Drawing.Size(20, 20);
            this.lbl_C.TabIndex = 12;
            this.lbl_C.Text = "C";
            // 
            // lbl_F
            // 
            this.lbl_F.AutoSize = true;
            this.lbl_F.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_F.Location = new System.Drawing.Point(6, 30);
            this.lbl_F.Name = "lbl_F";
            this.lbl_F.Size = new System.Drawing.Size(19, 20);
            this.lbl_F.TabIndex = 11;
            this.lbl_F.Text = "F";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 623);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_formbios);
            this.Controls.Add(this.btn_Reset);
            this.Controls.Add(this.lsbx_RAM);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_desc_display);
            this.Controls.Add(this.lsbx_BIOS);
            this.Controls.Add(this.btn_Open);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btn_Open;
        private System.Windows.Forms.ListBox lsbx_BIOS;
        private System.Windows.Forms.Label lbl_desc_display;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lsbx_RAM;
        private System.Windows.Forms.Button btn_Reset;
        private System.Windows.Forms.Button btn_formbios;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbl_H;
        private System.Windows.Forms.Label lbl_D;
        private System.Windows.Forms.Label lbl_B;
        private System.Windows.Forms.Label lbl_A;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbl_L;
        private System.Windows.Forms.Label lbl_E;
        private System.Windows.Forms.Label lbl_C;
        private System.Windows.Forms.Label lbl_F;
    }
}

