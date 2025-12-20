using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaroClient
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void txtBoxName_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            SocketManager.Instance.Connect("127.0.0.1", 9999);
            SocketManager.Instance.Send("NAME|" + txtBoxName.Text);
            LobbyForm lobbyForm = new LobbyForm();
            lobbyForm.Show();
            this.Hide();
        }
    }
}
