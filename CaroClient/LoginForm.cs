using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
            Task.Run(() => ListenForServerMsg());
        }

        private bool _isListening = true;

        private void ListenForServerMsg()
        {
            var reader = SocketManager.Instance.Reader;

            while (_isListening && SocketManager.Instance.Client.Connected)
            {
                try
                {
                    string msg = reader.ReadLine();
                    if (msg == null) break;

                    if (msg.StartsWith("LOGIN_SUCCESS"))
                    {
                        this.Invoke(new Action(() =>
                        {
                            LobbyForm lobbyForm = new LobbyForm();
                            lobbyForm.Show();
                            this.Hide();
                        }));
                    }
                    else if (msg.StartsWith("ERROR|"))
                    {
                        string errorMsg = msg.Substring("ERROR|".Length);
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show(errorMsg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }));
                    }
                }
                catch (Exception ex)
                {
                    break;
                }
            }
        }

        private void txtBoxName_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            SocketManager.Instance.Connect("127.0.0.1", 8888);
            SocketManager.Instance.Send("LOGIN|" + txtBoxName.Text);
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _isListening = false;
            Application.Exit();
        }

        private void LoginForm_Paint(object sender, PaintEventArgs e)
        {
            Color colorStart = Color.FromArgb(255, 235, 235); // Top-Left
            Color colorEnd = Color.FromArgb(235, 250, 255);   // Bottom-Right

            using (LinearGradientBrush brush = new LinearGradientBrush(
                this.ClientRectangle,
                colorStart,
                colorEnd,
                LinearGradientMode.ForwardDiagonal))
            {
                // Apply the gradient
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }
    }
}
