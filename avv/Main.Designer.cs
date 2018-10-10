namespace AV
{
    partial class Main
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
            if(disposing && (components != null))
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.treeAlbums = new System.Windows.Forms.TreeView();
            this.contextMenuAlbum = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addPhToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.stBar = new System.Windows.Forms.StatusStrip();
            this.sbLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.sbPictSizeMode = new System.Windows.Forms.ToolStripStatusLabel();
            this.sbTimerIncr = new System.Windows.Forms.ToolStripStatusLabel();
            this.sbSlideShow = new System.Windows.Forms.ToolStripStatusLabel();
            this.sbTimerDecr = new System.Windows.Forms.ToolStripStatusLabel();
            this.sbAddFiles = new System.Windows.Forms.ToolStripStatusLabel();
            this.stpgFiles = new System.Windows.Forms.ToolStripProgressBar();
            this.tmrSlideShow = new System.Windows.Forms.Timer(this.components);
            this.tbMainPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tbMain = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.pictImage = new System.Windows.Forms.PictureBox();
            this.contextMenuAlbum.SuspendLayout();
            this.stBar.SuspendLayout();
            this.tbMainPanel.SuspendLayout();
            this.tbMain.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictImage)).BeginInit();
            this.SuspendLayout();
            // 
            // treeAlbums
            // 
            this.treeAlbums.ContextMenuStrip = this.contextMenuAlbum;
            this.treeAlbums.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeAlbums.Location = new System.Drawing.Point(3, 3);
            this.treeAlbums.Name = "treeAlbums";
            this.treeAlbums.Size = new System.Drawing.Size(222, 597);
            this.treeAlbums.TabIndex = 1;
            this.treeAlbums.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.AfterSelect);
            this.treeAlbums.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeAlbums_KeyDown);
            this.treeAlbums.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            // 
            // contextMenuAlbum
            // 
            this.contextMenuAlbum.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPhToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuAlbum.Name = "contextMenuAlbum";
            this.contextMenuAlbum.Size = new System.Drawing.Size(114, 48);
            // 
            // addPhToolStripMenuItem
            // 
            this.addPhToolStripMenuItem.Name = "addPhToolStripMenuItem";
            this.addPhToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.addPhToolStripMenuItem.Text = "Add Ph";
            this.addPhToolStripMenuItem.Click += new System.EventHandler(this.OnAddPh);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Image files|*.gif;*.jpg;*.jpeg;*png";
            this.openFileDialog1.Multiselect = true;
            // 
            // stBar
            // 
            this.tbMainPanel.SetColumnSpan(this.stBar, 2);
            this.stBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sbLabel,
            this.sbPictSizeMode,
            this.sbTimerIncr,
            this.sbSlideShow,
            this.sbTimerDecr,
            this.sbAddFiles,
            this.stpgFiles});
            this.stBar.Location = new System.Drawing.Point(0, 603);
            this.stBar.Name = "stBar";
            this.stBar.Size = new System.Drawing.Size(937, 20);
            this.stBar.TabIndex = 9;
            this.stBar.Text = "sb";
            // 
            // sbLabel
            // 
            this.sbLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.sbLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.sbLabel.Margin = new System.Windows.Forms.Padding(0, 3, 10, 2);
            this.sbLabel.Name = "sbLabel";
            this.sbLabel.Size = new System.Drawing.Size(64, 15);
            this.sbLabel.Text = "File Name";
            // 
            // sbPictSizeMode
            // 
            this.sbPictSizeMode.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.sbPictSizeMode.Margin = new System.Windows.Forms.Padding(0, 3, 10, 2);
            this.sbPictSizeMode.Name = "sbPictSizeMode";
            this.sbPictSizeMode.Size = new System.Drawing.Size(65, 15);
            this.sbPictSizeMode.Text = "Pict Mode";
            // 
            // sbTimerIncr
            // 
            this.sbTimerIncr.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.sbTimerIncr.Name = "sbTimerIncr";
            this.sbTimerIncr.Size = new System.Drawing.Size(19, 15);
            this.sbTimerIncr.Text = "+";
            this.sbTimerIncr.Visible = false;
            this.sbTimerIncr.Click += new System.EventHandler(this.sbTimerIncr_Click);
            // 
            // sbSlideShow
            // 
            this.sbSlideShow.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.sbSlideShow.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.sbSlideShow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.sbSlideShow.Name = "sbSlideShow";
            this.sbSlideShow.Size = new System.Drawing.Size(68, 15);
            this.sbSlideShow.Text = "Slide Show";
            this.sbSlideShow.Click += new System.EventHandler(this.sbSlideShow_Click);
            // 
            // sbTimerDecr
            // 
            this.sbTimerDecr.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.sbTimerDecr.Name = "sbTimerDecr";
            this.sbTimerDecr.Size = new System.Drawing.Size(22, 15);
            this.sbTimerDecr.Text = " - ";
            this.sbTimerDecr.Visible = false;
            this.sbTimerDecr.Click += new System.EventHandler(this.sbTimerDecr_Click);
            // 
            // sbAddFiles
            // 
            this.sbAddFiles.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.sbAddFiles.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.sbAddFiles.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
            this.sbAddFiles.Name = "sbAddFiles";
            this.sbAddFiles.Size = new System.Drawing.Size(59, 15);
            this.sbAddFiles.Text = "Add Files";
            this.sbAddFiles.Click += new System.EventHandler(this.sbAddFiles_Click);
            // 
            // stpgFiles
            // 
            this.stpgFiles.Name = "stpgFiles";
            this.stpgFiles.Size = new System.Drawing.Size(100, 14);
            this.stpgFiles.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.stpgFiles.ToolTipText = "Adding Files";
            this.stpgFiles.Visible = false;
            // 
            // tmrSlideShow
            // 
            this.tmrSlideShow.Interval = 3000;
            this.tmrSlideShow.Tick += new System.EventHandler(this.tmrSlideShow_Tick);
            // 
            // tbMainPanel
            // 
            this.tbMainPanel.ColumnCount = 2;
            this.tbMainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbMainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbMainPanel.Controls.Add(this.treeAlbums, 0, 0);
            this.tbMainPanel.Controls.Add(this.tbMain, 1, 0);
            this.tbMainPanel.Controls.Add(this.stBar, 0, 1);
            this.tbMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMainPanel.Location = new System.Drawing.Point(0, 0);
            this.tbMainPanel.Name = "tbMainPanel";
            this.tbMainPanel.RowCount = 2;
            this.tbMainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbMainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tbMainPanel.Size = new System.Drawing.Size(937, 623);
            this.tbMainPanel.TabIndex = 11;
            // 
            // tbMain
            // 
            this.tbMain.Controls.Add(this.tabPage4);
            this.tbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMain.Location = new System.Drawing.Point(231, 3);
            this.tbMain.Name = "tbMain";
            this.tbMain.SelectedIndex = 0;
            this.tbMain.Size = new System.Drawing.Size(703, 597);
            this.tbMain.TabIndex = 8;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.pictImage);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(695, 571);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Picture";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // pictImage
            // 
            this.pictImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictImage.Location = new System.Drawing.Point(3, 3);
            this.pictImage.Name = "pictImage";
            this.pictImage.Size = new System.Drawing.Size(689, 565);
            this.pictImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictImage.TabIndex = 0;
            this.pictImage.TabStop = false;
            this.pictImage.Click += new System.EventHandler(this.pictImage_Click);
            this.pictImage.Paint += new System.Windows.Forms.PaintEventHandler(this.pictImage_Paint);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(937, 623);
            this.Controls.Add(this.tbMainPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Main";
            this.Text = "al";
            this.Load += new System.EventHandler(this.Main_Load);
            this.contextMenuAlbum.ResumeLayout(false);
            this.stBar.ResumeLayout(false);
            this.stBar.PerformLayout();
            this.tbMainPanel.ResumeLayout(false);
            this.tbMainPanel.PerformLayout();
            this.tbMain.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TreeView treeAlbums;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuAlbum;
        private System.Windows.Forms.ToolStripMenuItem addPhToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.StatusStrip stBar;
        private System.Windows.Forms.ToolStripStatusLabel sbLabel;
        private System.Windows.Forms.ToolStripStatusLabel sbPictSizeMode;
        private System.Windows.Forms.Timer tmrSlideShow;
        private System.Windows.Forms.ToolStripStatusLabel sbSlideShow;
        private System.Windows.Forms.ToolStripStatusLabel sbTimerIncr;
        private System.Windows.Forms.ToolStripStatusLabel sbTimerDecr;
        private System.Windows.Forms.TableLayoutPanel tbMainPanel;
        private System.Windows.Forms.ToolStripProgressBar stpgFiles;
        private System.Windows.Forms.ToolStripStatusLabel sbAddFiles;
        private System.Windows.Forms.TabControl tbMain;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.PictureBox pictImage;
    }
}

