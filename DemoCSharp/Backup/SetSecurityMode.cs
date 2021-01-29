using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DemoCSharp
{
    public partial class SetSecurityModeDialog : Form
    {
        public string strCurrentPasswd
        {
            get { return this.textBoxCurrentPasswd.Text; }
        }

        public int nLockMode
        {
            get
            {
                if (radioButton2.Checked)
                    return 1;
                else
                    return 0;
            }
        }

        public SetSecurityModeDialog()
        {
            InitializeComponent();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {

        }

    }
}