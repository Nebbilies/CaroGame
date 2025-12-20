using CaroLibrary.CaroLibrary;
using Newtonsoft.Json;
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
    public partial class LobbyForm : Form
    {
        private delegate void UpdateLobbyDelegate(string roomList);
        public LobbyForm()
        {
            InitializeComponent();
            Task.Run(() => ListenForServerMsg());
        }

        private void ListenForServerMsg()
        {
            while (SocketManager.Instance.Client.Connected)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = SocketManager.Instance.Stream.Read(buffer, 0, buffer.Length);
                    string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                    if (msg.StartsWith("LOBBY_UPDATE|"))
                    {
                        string roomList = msg.Substring("LOBBY_UPDATE|".Length);
                        this.Invoke(new UpdateLobbyDelegate(UpdateLobby), roomList);
                    }
                    /*else if ()
                    {

                    }*/
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Connection lost: " + ex.Message);
                    LoginForm loginForm = new LoginForm();
                    loginForm.Show();
                    this.Close();
                    break;
                }
            }
        }

        private void UpdateLobby(string roomList)
        {
            lvRooms.Items.Clear();
            List<RoomSummary> rooms = JsonConvert.DeserializeObject<List<RoomSummary>>(roomList);
            foreach (var room in rooms)
            {
                ListViewItem item = new ListViewItem(room.RoomId);
                item.SubItems.Add(room.RoomName);
                item.SubItems.Add($"{room.CurrentPlayerCount}/2");
                if (room.IsFull || room.IsGameStarted)
                {
                    item.ForeColor = Color.Gray;
                    item.BackColor = Color.LightGray;
                    item.Tag = "DISABLED";
                }
                else
                {
                    item.ForeColor = Color.Black;
                    item.BackColor = Color.White;
                    item.Tag = "ENABLED";
                }
                lvRooms.Items.Add(item);
            }
        }

        private void btnCreateRoom_Click(object sender, EventArgs e)
        {
            CreateRoomForm createRoomForm = new CreateRoomForm();
            createRoomForm.ShowDialog();
        }

        private void btnJoinRoom_Click(object sender, EventArgs e)
        {
            if (lvRooms.SelectedItems.Count == 0)
            {
                MessageBox.Show("Bạn chưa chọn phòng.");
                return;
            }
            ListViewItem selectedItem = lvRooms.SelectedItems[0];
            if (selectedItem.Tag.ToString() == "DISABLED")
            {
                MessageBox.Show("Phòng đã đầy.");
                return;
            }
            string roomName = selectedItem.Text;
            string joinRoomMsg = $"JOIN_ROOM|{SocketManager.Instance.PlayerName}|{roomId}";
            SocketManager.Instance.Send(joinRoomMsg);
        }
    }
}
