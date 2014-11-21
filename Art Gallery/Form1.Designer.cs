namespace FormsPolygonGenerator
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
            this.tb_numPoints = new System.Windows.Forms.TextBox();
            this.btn_draw = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tb_populationCount = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_previousGuard = new System.Windows.Forms.Button();
            this.btn_NextGuard = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_showArea = new System.Windows.Forms.Button();
            this.tb_generationCount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rb_WOC = new System.Windows.Forms.RadioButton();
            this.rb_GA = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.lbl_guardCount = new System.Windows.Forms.Label();
            this.lbl_gatime = new System.Windows.Forms.Label();
            this.lbl_woctime = new System.Windows.Forms.Label();
            this.lbl_GAavg = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb_numPoints
            // 
            this.tb_numPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_numPoints.Location = new System.Drawing.Point(6, 42);
            this.tb_numPoints.Name = "tb_numPoints";
            this.tb_numPoints.Size = new System.Drawing.Size(77, 20);
            this.tb_numPoints.TabIndex = 1;
            // 
            // btn_draw
            // 
            this.btn_draw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_draw.Location = new System.Drawing.Point(4, 153);
            this.btn_draw.Name = "btn_draw";
            this.btn_draw.Size = new System.Drawing.Size(75, 23);
            this.btn_draw.TabIndex = 2;
            this.btn_draw.Text = "Create";
            this.btn_draw.UseVisualStyleBackColor = true;
            this.btn_draw.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1106, 603);
            this.panel1.TabIndex = 3;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Number of Points";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.tb_populationCount);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btn_previousGuard);
            this.groupBox1.Controls.Add(this.btn_NextGuard);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btn_showArea);
            this.groupBox1.Controls.Add(this.tb_generationCount);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tb_numPoints);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btn_draw);
            this.groupBox1.Location = new System.Drawing.Point(1157, 63);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(91, 347);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // tb_populationCount
            // 
            this.tb_populationCount.Location = new System.Drawing.Point(4, 130);
            this.tb_populationCount.Name = "tb_populationCount";
            this.tb_populationCount.Size = new System.Drawing.Size(78, 20);
            this.tb_populationCount.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Population Count";
            // 
            // btn_previousGuard
            // 
            this.btn_previousGuard.Location = new System.Drawing.Point(7, 319);
            this.btn_previousGuard.Name = "btn_previousGuard";
            this.btn_previousGuard.Size = new System.Drawing.Size(75, 23);
            this.btn_previousGuard.TabIndex = 11;
            this.btn_previousGuard.Text = "Last Guard";
            this.btn_previousGuard.UseVisualStyleBackColor = true;
            this.btn_previousGuard.Click += new System.EventHandler(this.btn_previousGuard_Click);
            // 
            // btn_NextGuard
            // 
            this.btn_NextGuard.Location = new System.Drawing.Point(7, 289);
            this.btn_NextGuard.Name = "btn_NextGuard";
            this.btn_NextGuard.Size = new System.Drawing.Size(75, 23);
            this.btn_NextGuard.TabIndex = 10;
            this.btn_NextGuard.Text = "Next Guard";
            this.btn_NextGuard.UseVisualStyleBackColor = true;
            this.btn_NextGuard.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(7, 233);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(72, 20);
            this.textBox1.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 216);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Guard";
            // 
            // btn_showArea
            // 
            this.btn_showArea.Location = new System.Drawing.Point(6, 259);
            this.btn_showArea.Name = "btn_showArea";
            this.btn_showArea.Size = new System.Drawing.Size(75, 23);
            this.btn_showArea.TabIndex = 7;
            this.btn_showArea.Text = "Show Area";
            this.btn_showArea.UseVisualStyleBackColor = true;
            this.btn_showArea.Click += new System.EventHandler(this.button1_Click);
            // 
            // tb_generationCount
            // 
            this.tb_generationCount.Location = new System.Drawing.Point(4, 86);
            this.tb_generationCount.Name = "tb_generationCount";
            this.tb_generationCount.Size = new System.Drawing.Size(79, 20);
            this.tb_generationCount.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Generation Count";
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(1060, 473);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(200, 141);
            this.dataGridView1.TabIndex = 6;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.rb_WOC);
            this.groupBox2.Controls.Add(this.rb_GA);
            this.groupBox2.Location = new System.Drawing.Point(1157, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(92, 55);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            // 
            // rb_WOC
            // 
            this.rb_WOC.AutoSize = true;
            this.rb_WOC.Location = new System.Drawing.Point(4, 32);
            this.rb_WOC.Name = "rb_WOC";
            this.rb_WOC.Size = new System.Drawing.Size(51, 17);
            this.rb_WOC.TabIndex = 1;
            this.rb_WOC.TabStop = true;
            this.rb_WOC.Text = "WOC";
            this.rb_WOC.UseVisualStyleBackColor = true;
            this.rb_WOC.CheckedChanged += new System.EventHandler(this.rb_WOC_CheckedChanged);
            // 
            // rb_GA
            // 
            this.rb_GA.AutoSize = true;
            this.rb_GA.Location = new System.Drawing.Point(4, 10);
            this.rb_GA.Name = "rb_GA";
            this.rb_GA.Size = new System.Drawing.Size(40, 17);
            this.rb_GA.TabIndex = 0;
            this.rb_GA.TabStop = true;
            this.rb_GA.Text = "GA";
            this.rb_GA.UseVisualStyleBackColor = true;
            this.rb_GA.CheckedChanged += new System.EventHandler(this.rb_GA_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(4, 183);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Redraw";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // lbl_guardCount
            // 
            this.lbl_guardCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_guardCount.AutoSize = true;
            this.lbl_guardCount.Location = new System.Drawing.Point(1124, 413);
            this.lbl_guardCount.Name = "lbl_guardCount";
            this.lbl_guardCount.Size = new System.Drawing.Size(73, 13);
            this.lbl_guardCount.TabIndex = 8;
            this.lbl_guardCount.Text = "Guard Count: ";
            // 
            // lbl_gatime
            // 
            this.lbl_gatime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_gatime.AutoSize = true;
            this.lbl_gatime.Location = new System.Drawing.Point(1124, 457);
            this.lbl_gatime.Name = "lbl_gatime";
            this.lbl_gatime.Size = new System.Drawing.Size(47, 13);
            this.lbl_gatime.TabIndex = 9;
            this.lbl_gatime.Text = "GA time:";
            // 
            // lbl_woctime
            // 
            this.lbl_woctime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_woctime.AutoSize = true;
            this.lbl_woctime.Location = new System.Drawing.Point(1124, 444);
            this.lbl_woctime.Name = "lbl_woctime";
            this.lbl_woctime.Size = new System.Drawing.Size(61, 13);
            this.lbl_woctime.TabIndex = 10;
            this.lbl_woctime.Text = "WOC time: ";
            // 
            // lbl_GAavg
            // 
            this.lbl_GAavg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_GAavg.AutoSize = true;
            this.lbl_GAavg.Location = new System.Drawing.Point(1124, 426);
            this.lbl_GAavg.Name = "lbl_GAavg";
            this.lbl_GAavg.Size = new System.Drawing.Size(71, 13);
            this.lbl_GAavg.TabIndex = 11;
            this.lbl_GAavg.Text = "GA Average: ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 617);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lbl_GAavg);
            this.Controls.Add(this.lbl_woctime);
            this.Controls.Add(this.lbl_gatime);
            this.Controls.Add(this.lbl_guardCount);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_numPoints;
        private System.Windows.Forms.Button btn_draw;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tb_generationCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn_showArea;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_NextGuard;
        private System.Windows.Forms.Button btn_previousGuard;
        private System.Windows.Forms.TextBox tb_populationCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rb_WOC;
        private System.Windows.Forms.RadioButton rb_GA;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lbl_guardCount;
        private System.Windows.Forms.Label lbl_gatime;
        private System.Windows.Forms.Label lbl_woctime;
        private System.Windows.Forms.Label lbl_GAavg;
    }
}

