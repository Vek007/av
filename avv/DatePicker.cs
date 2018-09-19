using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AV
{
    public partial class DatePicker : Form
    {
        private bool bMax = false;
        public DatePicker()
        {
            InitializeComponent();
        }

        public DateTime StartDate
        {
            get
            {
                if (!this.bMax)
                    return dtStart.Value;
                else
                    return DateTime.MinValue;
            }
        }
        public DateTime EndDate
        {
            get {
                if (!this.bMax)
                    return dtEnd.Value;
                else
                    return DateTime.MaxValue;
               }
        }

        public bool ResetDate
        {
            get { return bMax; }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            bMax = false;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cmdMax_Click(object sender, EventArgs e)
        {
            bMax = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
