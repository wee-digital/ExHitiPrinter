using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DemoCSharp
{
    public partial class SetPasswordDialog : Form
    {
        public string strCurrentPasswd
        {
            get { return this.textBoxCurrentPasswd.Text; }
        }

        public string strNewPasswd
        {
            get { return this.textBoxNewPasswd.Text; }
        }

        public SetPasswordDialog()
        {
            InitializeComponent();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {

        }
    }
}