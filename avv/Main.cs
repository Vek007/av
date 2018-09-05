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

namespace AV
{
    public partial class Main : Form
    {

        public Main()
        {
            InitializeComponent();
            this.KeyPreview = true;
            tbMain.SelectedIndex = 2;
        }

        private void LoadAls()
        {
            List<al> lstAl = Data.GetAls();

            // Now iterate through them and add to treeview
            foreach(al alm in lstAl)
            {
                TreeNode albumNode = new TreeNode(alm.name);
                
                // Add the album struct to the Tag for later
                // retrieval of info without database call
                albumNode.Tag = alm;

                treeAlbums.Nodes.Add(albumNode);

                // Add each Ph in album to treenode for the album
                foreach(ph Ph in alm.phs)
                {
                    TreeNode PhNode = new TreeNode(Ph.name);
                    PhNode.Tag = Ph;

                    albumNode.Nodes.Add(PhNode);
                }                
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

            foreach (ItemInfo itm in hashPh.Values.ToList())
            {
                this.AlCal.AddItem(itm);
            }

            foreach (ItemInfo itm in hashAh.Values.ToList())
            {
                this.AlCal.AddItem(itm);
            }

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
            if (treeAlbums.SelectedNode.Tag is al alm)
            {
                SaveTags();

                imgList.ClearImages();

                foreach (ph phh in alm.phs)
                {
                    imgList.AddImage(phh.path, phh.infoTags, phh);
                }

                imgViewer.LoadImageList(alm.GetAllPhsWithFullPath());
                if(imgViewer.GetCurrentImage()!=null)
                    UpdateStatusBar(imgViewer.GetCurrentImage().infoTags);
                tbMain.SelectedIndex = 2;
            }
            else if (treeAlbums.SelectedNode.Tag is ph phh)
            {
                DrawPictToScale(new Bitmap(phh.path));
                imgList.Select();
                if (!imgList.SelectImage(phh.path))
                {
                    imgList.AddImage(phh.path, phh.infoTags, phh);
                    imgList.SelectImage(phh.path);
                    UpdateStatusBar(phh.infoTags);
                }
            }
            else
            {
            }
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
            LoadCal();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            tbMain.SelectedIndex = 1;
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

            if ((int)m.WParam == (int)Keys.Left)
            {
                imgViewer.ApplyLeftRightArrowKey(true);
                if (imgViewer.GetCurrentImage() != null)
                    UpdateStatusBar(imgViewer.GetCurrentImage().infoTags);

            }
            else if ((int)m.WParam == (int)Keys.Right)
            {
                imgViewer.ApplyLeftRightArrowKey(false);
                if (imgViewer.GetCurrentImage() != null)
                    UpdateStatusBar(imgViewer.GetCurrentImage().infoTags);

            }

            if (m.Msg == 258 && ( char.IsLetter((char)m.WParam) || char.IsDigit((char)m.WParam)))
            {
                Debug.WriteLine(m.WParam.ToString());

                Photo curImg = imgViewer.GetCurrentImage();

                ph curPhh = Data.alDb.phs.Where(a => a.path == curImg.path).FirstOrDefault();

                if (curPhh != null && !curPhh.infoTags.ToLower().Contains((char)m.WParam))
                {
                    curPhh.infoTags += (char)m.WParam;
                }

                UpdateStatusBar(curPhh.infoTags);

                curPhh.UpdatePh();
                Data.RefreshDatabase(curPhh);
            }

            return base.ProcessKeyPreview(ref m);
        }

        private void UpdateStatusBar(string infoTags)
        {
            this.sbLabel.Text = infoTags;
        }
    }
}
