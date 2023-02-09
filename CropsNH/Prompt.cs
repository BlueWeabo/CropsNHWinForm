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
        private static bool cancelled = false;

        public static bool Cancelled { get => cancelled; set => cancelled = value; }

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
            Cancelled = false;
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Cancelled = true;
            Close();
        }
    }
}
