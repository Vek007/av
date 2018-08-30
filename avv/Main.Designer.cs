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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newAlbumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddFilesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeAlbums = new System.Windows.Forms.TreeView();
            this.contextMenuAlbum = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addPhToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tbMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.AlCal = new CalendarControl.CalendarControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.imgList = new ListImgControl.ListImgCtrl();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.contextMenuAlbum.SuspendLayout();
            this.tbMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(937, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newAlbumToolStripMenuItem,
            this.AddFilesMenuItem,
            this.saveMenuItem,
            this.toolStripMenuItem2,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newAlbumToolStripMenuItem
            // 
            this.newAlbumToolStripMenuItem.Name = "newAlbumToolStripMenuItem";
            this.newAlbumToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newAlbumToolStripMenuItem.Text = "&New Album";
            this.newAlbumToolStripMenuItem.Click += new System.EventHandler(this.OnNewAlbum);
            // 
            // AddFilesMenuItem
            // 
            this.AddFilesMenuItem.Name = "AddFilesMenuItem";
            this.AddFilesMenuItem.Size = new System.Drawing.Size(180, 22);
            this.AddFilesMenuItem.Text = "A&dd Files";
            this.AddFilesMenuItem.Click += new System.EventHandler(this.AddFilesMenuItem_Click);
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveMenuItem.Text = "Sa&ve";
            this.saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.OnExit);
            // 
            // treeAlbums
            // 
            this.treeAlbums.ContextMenuStrip = this.contextMenuAlbum;
            this.treeAlbums.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeAlbums.Location = new System.Drawing.Point(0, 24);
            this.treeAlbums.Name = "treeAlbums";
            this.treeAlbums.Size = new System.Drawing.Size(225, 599);
            this.treeAlbums.TabIndex = 1;
            this.treeAlbums.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.AfterSelect);
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
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.OnDelete);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Image files|*.gif;*.jpg;*.jpeg;*png";
            this.openFileDialog1.Multiselect = true;
            // 
            // tbMain
            // 
            this.tbMain.Controls.Add(this.tabPage1);
            this.tbMain.Controls.Add(this.tabPage2);
            this.tbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMain.Location = new System.Drawing.Point(225, 24);
            this.tbMain.Name = "tbMain";
            this.tbMain.SelectedIndex = 0;
            this.tbMain.Size = new System.Drawing.Size(712, 599);
            this.tbMain.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.AlCal);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(704, 573);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Calendar";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // AlCal
            // 
            this.AlCal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AlCal.Location = new System.Drawing.Point(3, 3);
            this.AlCal.Name = "AlCal";
            this.AlCal.Size = new System.Drawing.Size(698, 567);
            this.AlCal.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.imgList);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(704, 573);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Viewer";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // imgList
            // 
            this.imgList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgList.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.imgList.Location = new System.Drawing.Point(3, 3);
            this.imgList.Name = "imgList";
            this.imgList.Size = new System.Drawing.Size(698, 567);
            this.imgList.TabIndex = 0;
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem2.Text = "&Clear Viewer";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(937, 623);
            this.Controls.Add(this.tbMain);
            this.Controls.Add(this.treeAlbums);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "al";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuAlbum.ResumeLayout(false);
            this.tbMain.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newAlbumToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TreeView treeAlbums;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuAlbum;
        private System.Windows.Forms.ToolStripMenuItem addPhToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.TabControl tbMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStripMenuItem AddFilesMenuItem;
        private ListImgControl.ListImgCtrl imgList;
        private CalendarControl.CalendarControl AlCal;
        private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    }
}

