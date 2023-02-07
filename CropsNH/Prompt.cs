using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CropsNH
{
    public partial class Prompt : Form
    {
        public static bool cancelled = false;
        public Prompt(bool isConfirm)
        {
            InitializeComponent();
            if (!isConfirm)
            {
                button1.Hide();
                button2.Hide();
            }
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            cancelled = false;
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            cancelled = true;
            Close();
        }
    }
}
