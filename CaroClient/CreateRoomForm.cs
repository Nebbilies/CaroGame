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
    public partial class CreateRoomForm : Form
    {
        string roomName;
        int timePerMove;
        public CreateRoomForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        { 
            roomName = txtRoomName.Text.Trim();
            timePerMove = (int)nudRoomTimer.Value;
            string createRoomMsg = $"CREATE_ROOM|{roomName}|{timePerMove}";
            SocketManager.Instance.Send(createRoomMsg);
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
