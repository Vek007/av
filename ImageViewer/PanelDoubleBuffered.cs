using System.Windows.Forms;

namespace KaiwaProjects
{
    public class PanelDoubleBuffered : System.Windows.Forms.Panel
    {

        KpImageViewer imgViewer = null;
        public PanelDoubleBuffered()
        {
            this.DoubleBuffered = true;
            this.UpdateStyles();
        }

        public void SetParent(KpImageViewer viewer)
        {
            imgViewer = viewer;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            MessageBox.Show("You press " + keyData.ToString());

            // dO operations here...

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
