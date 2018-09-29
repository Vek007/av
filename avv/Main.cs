using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Threading;
using CalendarControl;
using ImageListViewLib;
using System.Diagnostics;
using KaiwaProjects;
using System.IO;
using System.Globalization;
using AV;

namespace AV
{
    public partial class Main : Form
    {
        private TreeNode selectedNode = null;
        public Main()
        {
            InitializeComponent();
            this.KeyPreview = true;
            MouseWheel += new MouseEventHandler(OnMouseWheel);
            pictImage.KeyDown += treeAlbums_KeyDown;
            tbMain.KeyDown += treeAlbums_KeyDown;
        }

        bool bFileAdd = false;
        internal void ShowProgressBar(bool v)
        {
            bFileAdd = v;
            this.stpgFiles.Visible = v;
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            Debug.WriteLine(e.Delta);
            if (tbMain.SelectedIndex == 2)
            {
                if (e.Delta > 0)
                {
                    imgViewer.ZoomIn();
                }
                else
                {
                    imgViewer.ZoomOut();
                }
            }
        }

        private void LoadAls()
        {
            treeAlbums.Nodes.Clear();
            List<string> lstYears = Data.GetDistinctPhYears();
            foreach (string year in lstYears)
            {
                TreeNode yearNode = new TreeNode(year);
                yearNode.Tag = "years";
                List<string> lstMonths = Data.GetDistinctPhMonths(Convert.ToInt32(year));
                foreach (string mn in lstMonths)
                {
                    TreeNode mnNode = new TreeNode(mn);
                    mnNode.Tag = "months";

                    int month = Convert.ToDateTime(mn + " 01, 1900").Month;
                    List<int> days = Data.GetDistinctPhDays(Convert.ToInt32(year), month);
                    yearNode.Nodes.Add(mnNode);

                    foreach (int day in days.OrderBy(a=>a))
                    {

                        List<ph> phs = Data.GetPhByDayMonthYear(Convert.ToInt32(year), DateTime.ParseExact(mn, "MMMM", CultureInfo.CurrentCulture).Month, day).OrderBy(p=>p.time_stamp).ToList();
                        string dayNodeName = day.ToString() + "-" + Convert.ToDateTime(mn + " " + day + ", " + year).ToString("dddd") + "(" + phs.Count.ToString() + ")";
                        TreeNode dayNode = new TreeNode(dayNodeName);
                        dayNode.Tag = "days";

                        mnNode.Nodes.Add(dayNode);

                        foreach (ph p in phs)
                        {
//                            TreeNode pNode = new TreeNode(p.id + "(" + p.time_stamp.Value.Day.ToString() + ")");
                            TreeNode pNode = new TreeNode(p.id);
                            pNode.Tag = p;

                            dayNode.Nodes.Add(pNode);
                        }
                    }
                }
                treeAlbums.Nodes.Add(yearNode);
            }
        }

        private void LoadAls(DateTime stDate, DateTime endDate)
        {
            treeAlbums.Nodes.Clear();
            DateTime sDate = stDate;
            while (sDate <= endDate)
            {
                TreeNode dtNode = new TreeNode(sDate.ToString("dd-MMM-yyyy"));
                List<ph> phs = Data.GetPhByDate(sDate);

                foreach (ph p in phs)
                {
                    TreeNode pNode = new TreeNode(p.id + "(" + p.time_stamp.Value.Day.ToString() + ")");
                    pNode.Tag = p;

                    dtNode.Nodes.Add(pNode);
                }
                treeAlbums.Nodes.Add(dtNode);
                sDate=sDate.AddDays(1);
            }
        }

        #region TreeView Context Menu Handlers 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
        }

        #endregion

        #region Album context menu handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAddPh(object sender, EventArgs e)
        {
            if(DialogResult.OK == openFileDialog1.ShowDialog())
            {
                // Retrieve the Album to add Ph(s) to
                Al album = (Al)treeAlbums.SelectedNode.Tag;

                // We allow multiple selections so loop through each one
                foreach(string file in openFileDialog1.FileNames)
                {
                    // Create a new stream to load this Ph into
                    System.IO.FileStream stream = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    // Create a buffer to hold the stream bytes
                    byte[] buffer = new byte[stream.Length];
                    // Read the bytes from this stream
                    stream.Read(buffer, 0, (int)stream.Length);
                    // Now we can close the stream
                    stream.Close();

                    Ph Ph = new Ph()
                    {
                        // Extract out the name of the file an use it for the name
                        // of the Ph
                        Name = System.IO.Path.GetFileNameWithoutExtension(file),
                        Image = buffer
                    };

                    // Insert the image into the database and add it to the tree
                    Data.AddPh(album.Id, Ph);
                    buffer = null;

                    // Add the Ph to the album node
                    TreeNode node = treeAlbums.SelectedNode.Nodes.Add(Ph.Name);
                    node.Tag = Ph;
                }
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeAlbums.SelectedNode.Tag is string alm)
            {
                selectedNode = null;
                if (alm.Trim().ToLower() == "days")
                {
                    LoadImageList(treeAlbums.SelectedNode);
                }
                else if (alm.Trim().ToLower() == "months")
                {
                    if (treeAlbums.SelectedNode.Nodes.Count > 0)
                        LoadImageList(treeAlbums.SelectedNode.Nodes[0]);
                }

                tbMain.SelectedIndex = 1;
            }
            else if (treeAlbums.SelectedNode.Tag is ph phh)
            {
                selectedNode = treeAlbums.SelectedNode;
                if (File.Exists(phh.path))
                {
                    pictImage.ImageLocation = phh.path;
                    pictImage.Tag = phh;
                    UpdateStatusBar(phh.infoTags ?? phh.id + "(no tags)");
                    tbMain.SelectedIndex = 2;
                }
                else
                {
                    UpdateStatusBar("File: " + phh.path + "doesn't exists");
                }
            }
            else
            {
            }
        }

        private void LoadImageList(TreeNode nd)
        {
            List<Photo> phs = new List<Photo>();

            foreach (TreeNode node in nd.Nodes)
            {
                phs.Add((node.Tag as ph).GetPhoto());
            }

            imgViewer.LoadImageList(phs);

            if (imgViewer.GetImageList().Count > 0)
            {
                imgViewer.ApplyLeftRightArrowKey(true, out string msg);
                if (msg.Length > 0)
                {
                    UpdateStatusBar(msg);
                }
            }

            tbMain.SelectedIndex = 1;
        }

        public void SaveTags()
        {
            var allNodes = treeAlbums.Nodes
                        .Cast<TreeNode>()
                        .SelectMany(GetNodeBranch);

            foreach (TreeNode itm in allNodes)
            {
                if (itm.Tag != null)
                {
                    ph img = (ph)itm.Tag;

                    if (img.infoTags != null )
                    {
                        img.UpdatePh();
                        Data.RefreshDatabase(img);
                    }
                }
            }
        }

        private IEnumerable<TreeNode> GetNodeBranch(TreeNode node)
        {
            yield return node;

            foreach (TreeNode child in node.Nodes)
                foreach (var childChild in GetNodeBranch(child))
                    yield return childChild;
        }

        /// <summary>
        /// This function will determine the scale the image should be rendered
        /// at from the size of the picture box and image
        /// </summary>
        /// <param name="bmp">Image to scale and assign to picture box</param>
        private void DrawPictToScale(Image bmp)
        {
            // The client rectangle
            Rectangle rc = new Rectangle();//pictureBox.ClientRectangle;

            // From Programming Windows with C#, by Charles Petzold
            // Figure out the scaling necessary for the image
            SizeF size = new SizeF(bmp.Width / bmp.HorizontalResolution, bmp.Height / bmp.VerticalResolution);
            float fScale = Math.Min(rc.Width / size.Width, rc.Height / size.Height);

            size.Width *= fScale;
            size.Height *= fScale;

            // Create a new bitmap of the proper size for the existing bitmap
            // and assign it to the picture box
            //pictureBox.Image = new Bitmap(bmp, size.ToSize());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEdit(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSave(object sender, EventArgs e)
        {
            //Edit.Visible = true;
            //Save.Visible = false;
            //Cancel.Visible = false;

            //DisplayName.Text = EditName.Text;
            //DisplayDescription.Text = EditDescription.Text;

            //EditName.Visible = false;
            //EditDescription.Visible = false;

            //DisplayName.Visible = true;
            //DisplayDescription.Visible = true;

            //// Determine the type of node
            //if(treeAlbums.SelectedNode.Tag is Al)
            //{
            //    Al album = (Al)treeAlbums.SelectedNode.Tag;

            //    album.Name = EditName.Text;
            //    album.Description = EditDescription.Text;

            //    Data.UpdateAl(album);

            //    treeAlbums.SelectedNode.Tag = album;
            //}
            //else if(treeAlbums.SelectedNode.Tag is Ph)
            //{
            //    Ph Ph = (Ph)treeAlbums.SelectedNode.Tag;

            //    Ph.Name = EditName.Text;
            //    Ph.Description = EditDescription.Text;

            //    Data.UpdatePh(Ph);

            //    treeAlbums.SelectedNode.Tag = Ph;
            //}

            //treeAlbums.SelectedNode.Text = EditName.Text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancel(object sender, EventArgs e)
        {
            //Edit.Visible = true;
            //Save.Visible = false;
            //Cancel.Visible = false;

            //EditName.Visible = false;
            //EditDescription.Visible = false;

            //DisplayName.Visible = true;
            //DisplayDescription.Visible = true;
        }

        private void AddFilesMenuItem_Click1(object sender, EventArgs e)
        {
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            SaveTags();
        }

        public void RefreshTree()
        {
            treeAlbums.Nodes.Clear();
            LoadAls();
            //LoadCal();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            tbMain.SelectedIndex = 2;
            RefreshTree();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SaveTags();
        }

        protected override bool ProcessKeyPreview(ref Message m)
        {
            Debug.WriteLine(m.ToString());

            //if (m.Msg == WM_KEYUP)
            {
                int j = 0;
            }

            if (m.Msg == 257)
            {
                if ((int)m.WParam == (int)Keys.Left)
                {
                    //Debug.WriteLine("Sending left key to tree album.");
                    //KeyEventArgs e = new KeyEventArgs(Keys.Left);
                    //treeAlbums_KeyDown(null, e);
                }
                else if ((int)m.WParam == (int)Keys.Right)
                {
                    //Debug.WriteLine("Sending right key to tree album.");
                    //KeyEventArgs e = new KeyEventArgs(Keys.Right);
                    //treeAlbums_KeyDown(null, e);
                }
                else if ((int)m.WParam == (int)Keys.Add)
                {
                    imgViewer.ZoomIn();
                }
                else if ((int)m.WParam == (int)Keys.Subtract)
                {
                    imgViewer.ZoomOut();
                }
                else if ((int)m.WParam == (int)Keys.F2)
                {
                    this.WindowState = FormWindowState.Maximized;
                    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    treeAlbums.Visible = false;
                    stBar.Visible = false;
                    stpgFiles.Visible = false;
                    tbMain.Width += treeAlbums.Width;
                    tbMain.Left = 0;
                    imgViewer.HidePanels(true);
                }
                else if ((int)m.WParam == (int)Keys.F8)
                {
                    if (pictImage.SizeMode == PictureBoxSizeMode.Zoom)
                    {
                        pictImage.SizeMode = 0;
                    }
                    pictImage.SizeMode = pictImage.SizeMode + 1;

                    sbPictSizeMode.Text = pictImage.SizeMode.ToString();
                }
                else if ((int)m.WParam == (int)Keys.F1)
                {
                    sbSlideShow_Click(null, null);
                }
                else if ((int)m.WParam == (int)Keys.F3)
                {
                    stpgFiles.Visible = bFileAdd;
                    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                    stBar.Visible = true;
                    treeAlbums.Visible = true;
                    treeAlbums.Refresh();
                    tbMain.Width -= treeAlbums.Width;
                    tbMain.Left = treeAlbums.Width + 5;

                    imgViewer.HidePanels(false);
                }
                else if ((int)m.WParam == (int)Keys.F12)
                {
                    sbSlideShow_Click(null, null);
                }
                else if ((int)m.WParam == (int)Keys.F4)
                {
                    DatePicker dp = new DatePicker();
                    if (dp.ShowDialog() == DialogResult.OK)
                    {
                        DateTime stDt = dp.StartDate;
                        DateTime enDt = dp.EndDate;

                        if (!dp.ResetDate)
                            LoadAls(stDt, enDt);
                        else
                            LoadAls();
                    }
                }
                else if ((int)m.WParam == (int)Keys.F5)
                {
                    showIcons = !showIcons;
                    pictImage.Invalidate();
                }
                else if (char.IsLetter((char)m.WParam) || char.IsDigit((char)m.WParam))
                {
                    Debug.WriteLine(m.WParam.ToString());

                    Photo curImg =  imgViewer.GetCurrentImage();

                    ph curPhh = treeAlbums.SelectedNode.Tag as ph;//Data.alDb.phs.Where(a => a.path == curImg.path).FirstOrDefault();

                    curPhh.infoTags = curPhh.infoTags ?? "";

                    if (curPhh != null && !curPhh.infoTags.ToLower().Contains((char)m.WParam))
                    {
                        curPhh.infoTags += (char)m.WParam;
                    }

                    UpdateStatusBar(curPhh.infoTags);

                    curPhh.UpdatePh();
                    Data.RefreshDatabase(curPhh);
                    pictImage.Invalidate();
                }
                else if ((int)m.WParam == (int)Keys.Back)
                {
                    Photo curImg = imgViewer.GetCurrentImage();

                    ph curPhh = treeAlbums.SelectedNode.Tag as ph;//Data.alDb.phs.Where(a => a.path == curImg.path).FirstOrDefault();

                    curPhh.infoTags = curPhh.infoTags ?? "";

                    if (curPhh != null && curPhh.infoTags.Trim().Length>0)
                    {
                        curPhh.infoTags = curPhh.infoTags.Trim().Substring(0, curPhh.infoTags.Length - 1);
                    }

                    UpdateStatusBar(curPhh.infoTags);

                    curPhh.UpdatePh();
                    Data.RefreshDatabase(curPhh);
                    pictImage.Invalidate();
                }


            }
            return base.ProcessKeyPreview(ref m);
        }

        bool showIcons = false;

        private void UpdateStatusBar(string infoTags)
        {
            if (selectedNode != null && selectedNode.Tag != null)
            {
                ph phh = (ph)selectedNode.Tag;
                string info = phh.infoTags ?? string.Empty;
                this.sbLabel.Text = phh.id + " (" + info.Trim() + ")";
            }

            this.sbPictSizeMode.Text = pictImage.SizeMode.ToString();
            this.Text = this.sbLabel.Text + " - " + this.sbPictSizeMode.Text;
        }

        private void pictImage_Click(object sender, EventArgs e)
        {
            if (pictImage.Parent == this)
                pictImage.Parent = tbMain.TabPages[1];
            else
                pictImage.Parent = this;

            pictImage.BringToFront();
            pictImage.Refresh();
        }

        private void pictImage_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void treeAlbums_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left )
            {
                if (selectedNode != null)
                {
                    if (selectedNode != null && selectedNode.Parent.Nodes.Count >= 0 && selectedNode.Index > 0)
                    {
                        Debug.WriteLine("<>" + treeAlbums.SelectedNode.Text);
                        treeAlbums.BeginUpdate();
                        treeAlbums.SelectedNode = treeAlbums.SelectedNode.Parent.Nodes[selectedNode.Index - 1];
                        selectedNode = treeAlbums.SelectedNode;
                        treeAlbums.EndUpdate();
                        treeAlbums.Refresh();
                        Debug.WriteLine("<>" + treeAlbums.SelectedNode.Text);
                        e.Handled = true;
                    }
                }
            }

            if (e.KeyCode == Keys.Right)
            {
                if (selectedNode != null)
                {
                    if (selectedNode != null && selectedNode.Parent.Nodes.Count >= 0 && selectedNode.Index >= 0 && selectedNode.Index < selectedNode.Parent.Nodes.Count)
                    {
                        Debug.WriteLine("<>" + treeAlbums.SelectedNode.Text);
                        treeAlbums.BeginUpdate();
                        if (selectedNode.Index + 1 < selectedNode.Parent.Nodes.Count)
                        {
                            treeAlbums.SelectedNode = treeAlbums.SelectedNode.Parent.Nodes[selectedNode.Index + 1];
                            selectedNode = treeAlbums.SelectedNode;
                            treeAlbums.EndUpdate();
                            treeAlbums.Refresh();
                            Debug.WriteLine("<>" + treeAlbums.SelectedNode.Text);
                        }
                        e.Handled = true;

                    }
                }
            }

        }

        private void tmrSlideShow_Tick(object sender, EventArgs e)
        {
            KeyEventArgs e1 = new KeyEventArgs(Keys.Right);
            treeAlbums_KeyDown(null,  e1);
        }

        private void slideShowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tmrSlideShow.Enabled = !tmrSlideShow.Enabled;
        }

        private void sbSlideShow_Click(object sender, EventArgs e)
        {
            tmrSlideShow.Enabled = !tmrSlideShow.Enabled;
            {
                sbTimerDecr.Visible = tmrSlideShow.Enabled;
                sbTimerIncr.Visible = tmrSlideShow.Enabled;
            }
        }

        private void sbTimerIncr_Click(object sender, EventArgs e)
        {
            tmrSlideShow.Interval += 1000;
        }

        private void sbTimerDecr_Click(object sender, EventArgs e)
        {
            tmrSlideShow.Interval -= 1000;
        }

        private void pictImage_Paint(object sender, PaintEventArgs e)
        {
            if (treeAlbums.SelectedNode!=null && treeAlbums.SelectedNode.Tag!=null&&treeAlbums.SelectedNode.Tag is ph phh)
            {
                if (phh.infoTags != null && phh.infoTags.ToUpper().Contains("D") && showIcons)
                {
                    e.Graphics.DrawIcon(AV.Properties.Resources.delete, 0, 0);
                }
            }
        }

        private void pgFilesAdd_Click(object sender, EventArgs e)
        {
            stpgFiles.Visible = false;
        }

        private void sbAddFiles_Click(object sender, EventArgs e)
        {
            int i = 0;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = "c:\\";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string fldName = fbd.SelectedPath;
                Thread thread = new Thread(() => AV.PhIterator.IterateAndSave(fldName, this));
                thread.Start();
            }
        }
    }
}
