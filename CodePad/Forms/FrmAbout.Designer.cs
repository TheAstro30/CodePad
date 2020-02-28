﻿namespace CodePad.Forms
{
    partial class FrmAbout
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
            this.pnlIcon = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.lblContributors = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pnlIcon
            // 
            this.pnlIcon.BackColor = System.Drawing.Color.Transparent;
            this.pnlIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlIcon.Location = new System.Drawing.Point(12, 12);
            this.pnlIcon.Name = "pnlIcon";
            this.pnlIcon.Size = new System.Drawing.Size(84, 83);
            this.pnlIcon.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(268, 262);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTitle.Location = new System.Drawing.Point(102, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(153, 45);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "CodePad";
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblSubTitle.Location = new System.Drawing.Point(107, 59);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(139, 15);
            this.lblSubTitle.TabIndex = 3;
            this.lblSubTitle.Text = "The Ultimate Code Editor";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.Location = new System.Drawing.Point(107, 80);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(48, 15);
            this.lblVersion.TabIndex = 4;
            this.lblVersion.Text = "Version:";
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.BackColor = System.Drawing.Color.Transparent;
            this.lblAuthor.Location = new System.Drawing.Point(107, 111);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(182, 15);
            this.lblAuthor.TabIndex = 5;
            this.lblAuthor.Text = "Written by: Jason James Newland";
            // 
            // lblContributors
            // 
            this.lblContributors.BackColor = System.Drawing.Color.Transparent;
            this.lblContributors.Location = new System.Drawing.Point(107, 152);
            this.lblContributors.Name = "lblContributors";
            this.lblContributors.Size = new System.Drawing.Size(236, 59);
            this.lblContributors.TabIndex = 6;
            this.lblContributors.Text = "Contributors: Adam Oldham, beep, Pavel Torgashov, Ryan Alexander";
            // 
            // lblCopyright
            // 
            this.lblCopyright.BackColor = System.Drawing.Color.Transparent;
            this.lblCopyright.Location = new System.Drawing.Point(107, 211);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(236, 37);
            this.lblCopyright.TabIndex = 7;
            this.lblCopyright.Text = "Copyright © 2020 KangaSoft Software, All Rights Reserved.";
            // 
            // FrmAbout
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 297);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.lblContributors);
            this.Controls.Add(this.lblAuthor);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblSubTitle);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnlIcon);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAbout";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About CodePad";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlIcon;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.Label lblContributors;
        private System.Windows.Forms.Label lblCopyright;
    }
}