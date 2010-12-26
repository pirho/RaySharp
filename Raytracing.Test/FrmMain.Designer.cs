namespace Raytracing.Test {
    partial class FrmMain {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbTileSize = new System.Windows.Forms.TrackBar();
            this.txtTileSize = new System.Windows.Forms.MaskedTextBox();
            this.tbSamples = new System.Windows.Forms.TrackBar();
            this.txtSamples = new System.Windows.Forms.MaskedTextBox();
            this.tbTraceDepth = new System.Windows.Forms.TrackBar();
            this.txtTraceDepth = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRender = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.chkReflections = new System.Windows.Forms.CheckBox();
            this.chkRefraction = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbTileSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSamples)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTraceDepth)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkRefraction);
            this.groupBox1.Controls.Add(this.chkReflections);
            this.groupBox1.Controls.Add(this.tbTileSize);
            this.groupBox1.Controls.Add(this.txtTileSize);
            this.groupBox1.Controls.Add(this.tbSamples);
            this.groupBox1.Controls.Add(this.txtSamples);
            this.groupBox1.Controls.Add(this.tbTraceDepth);
            this.groupBox1.Controls.Add(this.txtTraceDepth);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(342, 239);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parameters";
            // 
            // tbTileSize
            // 
            this.tbTileSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTileSize.LargeChange = 16;
            this.tbTileSize.Location = new System.Drawing.Point(167, 121);
            this.tbTileSize.Maximum = 64;
            this.tbTileSize.Minimum = 16;
            this.tbTileSize.Name = "tbTileSize";
            this.tbTileSize.Size = new System.Drawing.Size(169, 45);
            this.tbTileSize.SmallChange = 16;
            this.tbTileSize.TabIndex = 4;
            this.tbTileSize.TickFrequency = 16;
            this.tbTileSize.Value = 32;
            this.tbTileSize.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
            // 
            // txtTileSize
            // 
            this.txtTileSize.Location = new System.Drawing.Point(124, 121);
            this.txtTileSize.Mask = "00";
            this.txtTileSize.Name = "txtTileSize";
            this.txtTileSize.Size = new System.Drawing.Size(37, 20);
            this.txtTileSize.TabIndex = 3;
            this.txtTileSize.Text = "32";
            this.txtTileSize.TextChanged += new System.EventHandler(this.TileSize_TextChanged);
            // 
            // tbSamples
            // 
            this.tbSamples.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSamples.LargeChange = 64;
            this.tbSamples.Location = new System.Drawing.Point(167, 70);
            this.tbSamples.Maximum = 512;
            this.tbSamples.Minimum = 16;
            this.tbSamples.Name = "tbSamples";
            this.tbSamples.Size = new System.Drawing.Size(169, 45);
            this.tbSamples.SmallChange = 16;
            this.tbSamples.TabIndex = 4;
            this.tbSamples.TickFrequency = 32;
            this.tbSamples.Value = 32;
            this.tbSamples.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
            // 
            // txtSamples
            // 
            this.txtSamples.Location = new System.Drawing.Point(124, 70);
            this.txtSamples.Mask = "900";
            this.txtSamples.Name = "txtSamples";
            this.txtSamples.Size = new System.Drawing.Size(37, 20);
            this.txtSamples.TabIndex = 3;
            this.txtSamples.Text = "32";
            this.txtSamples.TextChanged += new System.EventHandler(this.Samples_TextChanged);
            // 
            // tbTraceDepth
            // 
            this.tbTraceDepth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTraceDepth.LargeChange = 4;
            this.tbTraceDepth.Location = new System.Drawing.Point(167, 19);
            this.tbTraceDepth.Maximum = 64;
            this.tbTraceDepth.Minimum = 1;
            this.tbTraceDepth.Name = "tbTraceDepth";
            this.tbTraceDepth.Size = new System.Drawing.Size(169, 45);
            this.tbTraceDepth.TabIndex = 4;
            this.tbTraceDepth.TickFrequency = 4;
            this.tbTraceDepth.Value = 4;
            this.tbTraceDepth.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
            // 
            // txtTraceDepth
            // 
            this.txtTraceDepth.Location = new System.Drawing.Point(124, 19);
            this.txtTraceDepth.Mask = "90";
            this.txtTraceDepth.Name = "txtTraceDepth";
            this.txtTraceDepth.Size = new System.Drawing.Size(37, 20);
            this.txtTraceDepth.TabIndex = 3;
            this.txtTraceDepth.Text = "4";
            this.txtTraceDepth.TextChanged += new System.EventHandler(this.TraceDepth_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Maximal ray depth:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Size of rendertiles:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Soft shadow samples:";
            // 
            // btnRender
            // 
            this.btnRender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRender.Location = new System.Drawing.Point(12, 257);
            this.btnRender.Name = "btnRender";
            this.btnRender.Size = new System.Drawing.Size(75, 23);
            this.btnRender.TabIndex = 1;
            this.btnRender.Text = "&Render!";
            this.btnRender.UseVisualStyleBackColor = true;
            this.btnRender.Click += new System.EventHandler(this.RenderButtonClick);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(279, 257);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "&Exit";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.CloseButtonClick);
            // 
            // chkReflections
            // 
            this.chkReflections.AutoSize = true;
            this.chkReflections.Checked = true;
            this.chkReflections.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkReflections.Location = new System.Drawing.Point(9, 166);
            this.chkReflections.Name = "chkReflections";
            this.chkReflections.Size = new System.Drawing.Size(127, 17);
            this.chkReflections.TabIndex = 5;
            this.chkReflections.Text = "Enable reflection rays";
            this.chkReflections.UseVisualStyleBackColor = true;
            // 
            // chkRefraction
            // 
            this.chkRefraction.AutoSize = true;
            this.chkRefraction.Checked = true;
            this.chkRefraction.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRefraction.Location = new System.Drawing.Point(9, 189);
            this.chkRefraction.Name = "chkRefraction";
            this.chkRefraction.Size = new System.Drawing.Size(128, 17);
            this.chkRefraction.TabIndex = 6;
            this.chkRefraction.Text = "Enable refraction rays";
            this.chkRefraction.UseVisualStyleBackColor = true;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 292);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRender);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Raytracer";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbTileSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSamples)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTraceDepth)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TrackBar tbTileSize;
        private System.Windows.Forms.MaskedTextBox txtTileSize;
        private System.Windows.Forms.TrackBar tbSamples;
        private System.Windows.Forms.MaskedTextBox txtSamples;
        private System.Windows.Forms.TrackBar tbTraceDepth;
        private System.Windows.Forms.MaskedTextBox txtTraceDepth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRender;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox chkRefraction;
        private System.Windows.Forms.CheckBox chkReflections;

    }
}

