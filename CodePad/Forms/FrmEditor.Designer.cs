namespace CodePad.Forms
{
    sealed partial class FrmEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEditor));
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.mdiTab = new corelib.Controls.MdiTab.MdiTabStrip();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.mnuMain.Size = new System.Drawing.Size(705, 24);
            this.mnuMain.TabIndex = 0;
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 388);
            this.statusBar.Name = "statusBar";
            this.statusBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusBar.Size = new System.Drawing.Size(705, 22);
            this.statusBar.TabIndex = 4;
            // 
            // toolBar
            // 
            this.toolBar.GripMargin = new System.Windows.Forms.Padding(5, 2, 2, 2);
            this.toolBar.Location = new System.Drawing.Point(0, 24);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(705, 25);
            this.toolBar.TabIndex = 6;
            this.toolBar.Text = "toolStrip1";
            // 
            // mdiTab
            // 
            this.mdiTab.GripMargin = new System.Windows.Forms.Padding(5, 2, 2, 2);
            this.mdiTab.Location = new System.Drawing.Point(0, 49);
            this.mdiTab.Name = "mdiTab";
            this.mdiTab.SelectedTab = null;
            this.mdiTab.Size = new System.Drawing.Size(705, 25);
            this.mdiTab.TabIndex = 8;
            this.mdiTab.Text = "mdiTabStrip1";
            // 
            // FrmEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 410);
            this.Controls.Add(this.mdiTab);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.mnuMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mnuMain;
            this.Name = "FrmEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "CodePad";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStrip toolBar;
        private corelib.Controls.MdiTab.MdiTabStrip mdiTab;


    }
}

