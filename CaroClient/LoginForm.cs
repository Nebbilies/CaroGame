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
        }

        private void ListenForServerMsg()
        {
            var reader = SocketManager.Instance.Reader;
            var _isListening = true;

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
                            _isListening = false;
                            LobbyForm lobbyForm = new LobbyForm();
                            lobbyForm.Show();
                            this.Hide();
                        }));
                        break;
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
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                SocketManager.Instance.Connect("127.0.0.1", 8888);
                Task.Run(() => ListenForServerMsg());
                SocketManager.Instance.Send("LOGIN|" + txtBoxName.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể kết nối tới máy chủ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoginForm_Paint(object sender, PaintEventArgs e)
        {
            Color colorStart = Color.FromArgb(255, 235, 235);
            Color colorEnd = Color.FromArgb(235, 250, 255);

            using (LinearGradientBrush brush = new LinearGradientBrush(
                this.ClientRectangle,
                colorStart,
                colorEnd,
                LinearGradientMode.ForwardDiagonal))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }
    }
}
