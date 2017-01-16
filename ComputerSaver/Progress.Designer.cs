namespace ComputerSaver
{
    partial class Progress
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Progress));
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.ExitImmediately = new DevComponents.DotNetBar.ButtonX();
            this.CancelExit = new DevComponents.DotNetBar.ButtonX();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("AlternateGothic2 BT", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Cornsilk;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(281, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "The computer shuts down after 15 seconds";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 36);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(288, 41);
            this.progressBar1.TabIndex = 3;
            // 
            // ExitImmediately
            // 
            this.ExitImmediately.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ExitImmediately.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ExitImmediately.Font = new System.Drawing.Font("AlternateGothic2 BT", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitImmediately.Location = new System.Drawing.Point(12, 83);
            this.ExitImmediately.Name = "ExitImmediately";
            this.ExitImmediately.Size = new System.Drawing.Size(135, 44);
            this.ExitImmediately.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ExitImmediately.TabIndex = 4;
            this.ExitImmediately.Text = "Exit immediatly";
            this.ExitImmediately.Click += new System.EventHandler(this.ExitImmediately_Click_1);
            // 
            // CancelExit
            // 
            this.CancelExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.CancelExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.CancelExit.Font = new System.Drawing.Font("AlternateGothic2 BT", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelExit.Location = new System.Drawing.Point(169, 83);
            this.CancelExit.Name = "CancelExit";
            this.CancelExit.Size = new System.Drawing.Size(131, 44);
            this.CancelExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.CancelExit.TabIndex = 5;
            this.CancelExit.Text = "Cancel Exit";
            this.CancelExit.Click += new System.EventHandler(this.CancelExit_Click_1);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick_1);
            // 
            // Progress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RoyalBlue;
            this.ClientSize = new System.Drawing.Size(319, 148);
            this.Controls.Add(this.CancelExit);
            this.Controls.Add(this.ExitImmediately);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Progress";
            this.Text = "Progress";
            this.Load += new System.EventHandler(this.Progress_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private DevComponents.DotNetBar.ButtonX ExitImmediately;
        private DevComponents.DotNetBar.ButtonX CancelExit;
        private System.Windows.Forms.Timer timer1;
    }
}