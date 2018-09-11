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
                    yearNode.Nodes.Add(mnNode);
                    List<ph> phs = Data.GetPhByMonthsAndYear(Convert.ToInt32(year), DateTime.ParseExact(mn, "MMMM", CultureInfo.CurrentCulture).Month);

                    foreach (ph p in phs)
                    {
                        TreeNode pNode = new TreeNode(p.id+"("+p.time_stamp.Value.Day.ToString()+")");
                        pNode.Tag = p;

                        mnNode.Nodes.Add(pNode);
                    }
                }
                treeAlbums.Nodes.Add(yearNode);
            }
        }

        private void LoadCal()
        {
            List<ph> allPhs = Data.GetPhs();

            Dictionary<string, ItemInfo> hashPh = new Dictionary<string, ItemInfo>();
            Dictionary<string, ItemInfo> hashAh = new Dictionary<string, ItemInfo>();

            foreach (ph pho in allPhs)
            {
                ItemInfo phInfo = new ItemInfo
                {
                    StartTime = pho.time_stamp.Value.DateTime
                };
                TimeSpan ts = new TimeSpan(10, 30, 0);
                phInfo.StartTime = phInfo.StartTime.Date + ts;
                phInfo.EndTime = phInfo.StartTime.AddMinutes(30);
                phInfo.Text = "pic";
                phInfo.R = 150;
                phInfo.G = 100;
                phInfo.B = 50;

                if (!hashPh.ContainsKey(phInfo.StartTime.ToShortDateString()))
                {
                    hashPh.Add(phInfo.StartTime.ToShortDateString(), phInfo);
                    int i = 2;
                    foreach(al alm in pho.als)
                    {
                        if (!hashAh.ContainsKey(alm.name))
                        {
                            ItemInfo alInfo = new ItemInfo();
                            alInfo.Text = alm.name;
                            alInfo.StartTime = phInfo.StartTime.AddHours(i); i++;
                            alInfo.EndTime = alInfo.StartTime.AddMinutes(30);
                            alInfo.R = alm.r ?? 0;
                            alInfo.G = alm.g ?? 0;
                            alInfo.B = alm.b ?? 0;
                            hashAh.Add(alm.name+alInfo.StartTime.ToShortDateString(), alInfo);
                        }
                    }
                }
            }

            //foreach (ItemInfo itm in hashPh.Values.ToList())
            //{
            //    this.AlCal.AddItem(itm);
            //}

            //foreach (ItemInfo itm in hashAh.Values.ToList())
            //{
            //    this.AlCal.AddItem(itm);
            //}

        }

        #region Menu Events Handlers

        /// <summary>
        /// Exit menu handler
        /// </summary>
        private void OnExit(object sender, System.EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewAlbum(object sender, EventArgs e)
        {
            Al album = Data.AddAl();

            TreeNode node = treeAlbums.Nodes.Add(album.Name);
            node.Tag = album;
        }

        #endregion

        #region TreeView Context Menu Handlers 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            // We are only interested in right mouse clicks
            if(e.Button == MouseButtons.Right)
            {
                // Attempt to get the node the mouse clicked on
                TreeNode node = treeAlbums.GetNodeAt(e.X, e.Y);
                if(node != null)
                {
                    // Select the tree item
                    treeAlbums.SelectedNode = node;

                    // Check what type of node was clicked and edit
                    // context menu
                    if(node.Tag is Ph)
                    {
                        contextMenuAlbum.Items[0].Visible = false;
                    }
                    else
                    {
                        contextMenuAlbum.Items[0].Visible = true;
                    }

                    if (node.Tag != null)
                    {
                        ph ph1 = (ph)node.Tag;
                        imgList.Select();
                        imgList.SelectImage(ph1.path);
                    }
                }
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDelete(object sender, EventArgs e)
        {
            // Determine the type of node
            if(treeAlbums.SelectedNode.Tag is Al)
            {
                Data.DeleteAl(((Al)treeAlbums.SelectedNode.Tag).Id);
            }
            else if(treeAlbums.SelectedNode.Tag is Ph)
            {
                Data.DeletePh(((Ph)treeAlbums.SelectedNode.Tag).Id);
            }

            // Remove the node from the tree
            treeAlbums.SelectedNode.Remove();
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
                if (alm.Trim().ToLower() == "months")
                {
                    LoadImageList(treeAlbums.SelectedNode);
                }
                else
                {
                    if(treeAlbums.SelectedNode.Nodes.Count > 0)
                        LoadImageList(treeAlbums.SelectedNode.Nodes[0]);
                }

                treeAlbums.SelectedNode.Expand();
                tbMain.SelectedIndex = 2;
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

            if (treeAlbums.SelectedNode.Nodes.Count > 0)
            {
                treeAlbums.SelectedNode = treeAlbums.SelectedNode.Nodes[0];
            }
//            pictImage.ImageLocation = imgViewer.GetCurrentImage().path;
  //          pictImage.Refresh();

            tbMain.SelectedIndex = 2;
        }

        public void SaveTags()
        {
            foreach (ImageListViewItem itm in imgList.GetImgs())
            {
                if (itm.Tag != null)
                {
                    ph img = (ph)itm.Tag;

                    if (img.infoTags != itm.InfoTags && itm.InfoTags.Trim().Length > 0)
                    {
                        img.infoTags = itm.InfoTags;
                        img.UpdatePh();
                        Data.RefreshDatabase(img);
                    }
                }
            }
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

        private void AddFilesMenuItem_Click(object sender, EventArgs e)
        {
            int i = 0;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = "c:\\";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string filename = fbd.SelectedPath;
                Thread thread = new Thread(() => AV.PhIterator.IterateAndSave(filename));
                thread.Start();
            }
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            SaveTags();
        }

        private void RefreshTree()
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
            imgList.ClearImages();
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
                }
                else if ((int)m.WParam == (int)Keys.Right)
                {
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
                    treeAlbums.Visible = false;
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
                else if ((int)m.WParam == (int)Keys.F3)
                {
                    treeAlbums.Visible = true;
                    tbMain.Width -= treeAlbums.Width;
                    tbMain.Left = treeAlbums.Width + 5;

                    imgViewer.HidePanels(false);
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
                }
                else if ((int)m.WParam == (int)Keys.F3)
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

                }


            }
            return base.ProcessKeyPreview(ref m);
        }

        private void UpdateStatusBar(string infoTags)
        {
            if (selectedNode != null && selectedNode.Tag != null)
            {
                ph phh = (ph)selectedNode.Tag;
                string info = phh.infoTags ?? string.Empty;
                this.sbLabel.Text = phh.id + " (" + info.Trim() + ")";
            }

            this.sbPictSizeMode.Text = pictImage.SizeMode.ToString();
        }

        private void pictImage_Click(object sender, EventArgs e)
        {
            if (pictImage.Parent == this)
                pictImage.Parent = tbMain.TabPages[2];
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
                        treeAlbums.SelectedNode = treeAlbums.SelectedNode.Parent.Nodes[selectedNode.Index + 1];
                        selectedNode = treeAlbums.SelectedNode;
                        treeAlbums.EndUpdate();
                        treeAlbums.Refresh();
                        Debug.WriteLine("<>" + treeAlbums.SelectedNode.Text);
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
    }
}
