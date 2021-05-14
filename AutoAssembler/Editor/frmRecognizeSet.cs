using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AutoAssembler
{
    public partial class frmRecognizeSet : Form
    {
        public int SelectedButton = -1;

        public frmRecognizeSet()
        {
            InitializeComponent();
        }

        private void btnFixBefore_MoveBefore_Click(object sender, EventArgs e)
        {
            // 고정축 전방, 이동축 전방
            // --------------------------------------------------
            SelectedButton = 0;

            this.DialogResult = DialogResult.OK;
        }

        private void btnFixBefore_MoveAfter_Click(object sender, EventArgs e)
        {
            // 고정축 전방, 이동축 후방
            // ----------
            SelectedButton = 1;

            this.DialogResult = DialogResult.OK;
        }

        private void btnFixBefore_MoveTop_Click(object sender, EventArgs e)
        {
            // 고정축 전방, 이동축 상방
            // ----------
            SelectedButton = 2;

            this.DialogResult = DialogResult.OK;
        }

        private void btnFixAfter_MoveBefoe_Click(object sender, EventArgs e)
        {
            // 고정축 후방, 이동축 전방
            // ----------
            SelectedButton = 3;

            this.DialogResult = DialogResult.OK;
        }

        private void btnFixAfter_MoveAfter_Click(object sender, EventArgs e)
        {
            // 고정축 후방, 이동축 후방
            // ----------
            SelectedButton = 4;

            this.DialogResult = DialogResult.OK;
        }

        private void btnFixAfter_MoveTop_Click(object sender, EventArgs e)
        {
            // 고정축 후방, 이동축 상방
            // ----------
            SelectedButton = 5;

            this.DialogResult = DialogResult.OK;
        }
    }
}
