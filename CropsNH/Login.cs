using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CropsNH
{
    public partial class Login : Form
    {
        public int id;
        public Login()
        {
            InitializeComponent();
        }

        // Whenever the login button is clicked we will try to see if the one has logged with the correct username and password
        private void LoginButton_Click(object sender, EventArgs e)
        {
            MySqlCommand cmd = new()
            {
                Connection = Program.mConnection,
                CommandText = "Select id from Login where username=@user and password=@pwd"
            };
            cmd.Parameters.Add(new MySqlParameter("@user", usernameText.Text));
            cmd.Parameters.Add(new MySqlParameter("@pwd", passwordText.Text));
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                id = (int) rdr["id"];
            }
            rdr.Close();
            if (id <= 0)
            {
                Prompt prompt = new(false);
                prompt.promptText.Text = "Logged in with view-only access.";
                prompt.ShowDialog();
            }
            Close();
        }

        // if the close button was pressed we must also close the main program
        private void CloseButton_Click(object sender, EventArgs e)
        {
            Program.mMenu.Close();
            Close();
        }
    }
}
