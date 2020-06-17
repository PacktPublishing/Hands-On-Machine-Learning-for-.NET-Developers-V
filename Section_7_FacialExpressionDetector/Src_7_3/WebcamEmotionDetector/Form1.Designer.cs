namespace WebcamEmotionDetector
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.picFear = new System.Windows.Forms.PictureBox();
            this.picFrown = new System.Windows.Forms.PictureBox();
            this.picSurprise = new System.Windows.Forms.PictureBox();
            this.picSad = new System.Windows.Forms.PictureBox();
            this.picNeutral = new System.Windows.Forms.PictureBox();
            this.picHappy = new System.Windows.Forms.PictureBox();
            this.picAngry = new System.Windows.Forms.PictureBox();
            this.btnCamera = new System.Windows.Forms.Button();
            this.updateEmotionTimer = new System.Windows.Forms.Timer(this.components);
            this.panel4 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.picEmotion = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.picDisplay = new System.Windows.Forms.PictureBox();
            this.lnkLicense = new System.Windows.Forms.LinkLabel();
            this.lnkCredit = new System.Windows.Forms.LinkLabel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFrown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSurprise)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNeutral)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHappy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAngry)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEmotion)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lnkLicense);
            this.panel1.Controls.Add(this.lnkCredit);
            this.panel1.Controls.Add(this.picFear);
            this.panel1.Controls.Add(this.picFrown);
            this.panel1.Controls.Add(this.picSurprise);
            this.panel1.Controls.Add(this.picSad);
            this.panel1.Controls.Add(this.picNeutral);
            this.panel1.Controls.Add(this.picHappy);
            this.panel1.Controls.Add(this.picAngry);
            this.panel1.Controls.Add(this.btnCamera);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 438);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(695, 100);
            this.panel1.TabIndex = 0;
            // 
            // picFear
            // 
            this.picFear.Image = ((System.Drawing.Image)(resources.GetObject("picFear.Image")));
            this.picFear.Location = new System.Drawing.Point(911, 6);
            this.picFear.Name = "picFear";
            this.picFear.Size = new System.Drawing.Size(80, 80);
            this.picFear.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picFear.TabIndex = 10;
            this.picFear.TabStop = false;
            this.picFear.Visible = false;
            // 
            // picFrown
            // 
            this.picFrown.Image = ((System.Drawing.Image)(resources.GetObject("picFrown.Image")));
            this.picFrown.Location = new System.Drawing.Point(820, 8);
            this.picFrown.Name = "picFrown";
            this.picFrown.Size = new System.Drawing.Size(80, 80);
            this.picFrown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picFrown.TabIndex = 9;
            this.picFrown.TabStop = false;
            this.picFrown.Visible = false;
            // 
            // picSurprise
            // 
            this.picSurprise.Image = ((System.Drawing.Image)(resources.GetObject("picSurprise.Image")));
            this.picSurprise.Location = new System.Drawing.Point(722, 6);
            this.picSurprise.Name = "picSurprise";
            this.picSurprise.Size = new System.Drawing.Size(80, 80);
            this.picSurprise.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSurprise.TabIndex = 8;
            this.picSurprise.TabStop = false;
            this.picSurprise.Visible = false;
            // 
            // picSad
            // 
            this.picSad.Image = ((System.Drawing.Image)(resources.GetObject("picSad.Image")));
            this.picSad.InitialImage = ((System.Drawing.Image)(resources.GetObject("picSad.InitialImage")));
            this.picSad.Location = new System.Drawing.Point(616, 6);
            this.picSad.Name = "picSad";
            this.picSad.Size = new System.Drawing.Size(80, 80);
            this.picSad.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSad.TabIndex = 7;
            this.picSad.TabStop = false;
            this.picSad.Visible = false;
            // 
            // picNeutral
            // 
            this.picNeutral.Image = ((System.Drawing.Image)(resources.GetObject("picNeutral.Image")));
            this.picNeutral.Location = new System.Drawing.Point(507, 7);
            this.picNeutral.Name = "picNeutral";
            this.picNeutral.Size = new System.Drawing.Size(80, 80);
            this.picNeutral.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picNeutral.TabIndex = 6;
            this.picNeutral.TabStop = false;
            this.picNeutral.Visible = false;
            // 
            // picHappy
            // 
            this.picHappy.Image = ((System.Drawing.Image)(resources.GetObject("picHappy.Image")));
            this.picHappy.InitialImage = ((System.Drawing.Image)(resources.GetObject("picHappy.InitialImage")));
            this.picHappy.Location = new System.Drawing.Point(400, 8);
            this.picHappy.Name = "picHappy";
            this.picHappy.Size = new System.Drawing.Size(80, 80);
            this.picHappy.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picHappy.TabIndex = 5;
            this.picHappy.TabStop = false;
            this.picHappy.Visible = false;
            // 
            // picAngry
            // 
            this.picAngry.Image = ((System.Drawing.Image)(resources.GetObject("picAngry.Image")));
            this.picAngry.Location = new System.Drawing.Point(263, 7);
            this.picAngry.Name = "picAngry";
            this.picAngry.Size = new System.Drawing.Size(80, 80);
            this.picAngry.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAngry.TabIndex = 4;
            this.picAngry.TabStop = false;
            this.picAngry.Visible = false;
            // 
            // btnCamera
            // 
            this.btnCamera.Font = new System.Drawing.Font("Calibri", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCamera.Location = new System.Drawing.Point(39, 18);
            this.btnCamera.Name = "btnCamera";
            this.btnCamera.Size = new System.Drawing.Size(172, 69);
            this.btnCamera.TabIndex = 0;
            this.btnCamera.Text = "Start";
            this.btnCamera.UseVisualStyleBackColor = true;
            this.btnCamera.Click += new System.EventHandler(this.btnCamera_Click);
            // 
            // updateEmotionTimer
            // 
            this.updateEmotionTimer.Enabled = true;
            this.updateEmotionTimer.Interval = 750;
            this.updateEmotionTimer.Tick += new System.EventHandler(this.updateEmotionTimer_Tick);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.flowLayoutPanel1);
            this.panel4.Controls.Add(this.panel2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(443, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(252, 438);
            this.panel4.TabIndex = 5;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 169);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(10);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(252, 269);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.picEmotion);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(252, 169);
            this.panel2.TabIndex = 5;
            // 
            // picEmotion
            // 
            this.picEmotion.Image = ((System.Drawing.Image)(resources.GetObject("picEmotion.Image")));
            this.picEmotion.InitialImage = ((System.Drawing.Image)(resources.GetObject("picEmotion.InitialImage")));
            this.picEmotion.Location = new System.Drawing.Point(58, 12);
            this.picEmotion.Name = "picEmotion";
            this.picEmotion.Size = new System.Drawing.Size(150, 144);
            this.picEmotion.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picEmotion.TabIndex = 2;
            this.picEmotion.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.picDisplay);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(443, 438);
            this.panel3.TabIndex = 6;
            // 
            // picDisplay
            // 
            this.picDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picDisplay.Location = new System.Drawing.Point(0, 0);
            this.picDisplay.Name = "picDisplay";
            this.picDisplay.Size = new System.Drawing.Size(443, 438);
            this.picDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picDisplay.TabIndex = 2;
            this.picDisplay.TabStop = false;
            // 
            // lnkLicense
            // 
            this.lnkLicense.AutoSize = true;
            this.lnkLicense.LinkArea = new System.Windows.Forms.LinkArea(9, 18);
            this.lnkLicense.Location = new System.Drawing.Point(545, 68);
            this.lnkLicense.Name = "lnkLicense";
            this.lnkLicense.Size = new System.Drawing.Size(123, 20);
            this.lnkLicense.TabIndex = 12;
            this.lnkLicense.TabStop = true;
            this.lnkLicense.Text = "Licence: CC-BY-4.0";
            this.lnkLicense.UseCompatibleTextRendering = true;
            this.lnkLicense.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLicense_LinkClicked);
            // 
            // lnkCredit
            // 
            this.lnkCredit.AutoSize = true;
            this.lnkCredit.LinkArea = new System.Windows.Forms.LinkArea(12, 21);
            this.lnkCredit.Location = new System.Drawing.Point(353, 68);
            this.lnkCredit.Name = "lnkCredit";
            this.lnkCredit.Size = new System.Drawing.Size(168, 20);
            this.lnkCredit.TabIndex = 11;
            this.lnkCredit.TabStop = true;
            this.lnkCredit.Text = "Images from FontAwesome";
            this.lnkCredit.UseCompatibleTextRendering = true;
            this.lnkCredit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCredit_LinkClicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 538);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Emotion detector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFrown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSurprise)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNeutral)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHappy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAngry)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picEmotion)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCamera;

        private System.Windows.Forms.Timer updateEmotionTimer;
        private System.Windows.Forms.PictureBox picAngry;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox picEmotion;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox picDisplay;
        private System.Windows.Forms.PictureBox picHappy;
        private System.Windows.Forms.PictureBox picSurprise;
        private System.Windows.Forms.PictureBox picSad;
        private System.Windows.Forms.PictureBox picNeutral;
        private System.Windows.Forms.PictureBox picFrown;
        private System.Windows.Forms.PictureBox picFear;
        private System.Windows.Forms.LinkLabel lnkLicense;
        private System.Windows.Forms.LinkLabel lnkCredit;

        #endregion
    }
}

