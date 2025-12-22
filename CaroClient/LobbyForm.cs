using CaroLibrary.CaroLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
            using (var reader = new StreamReader(SocketManager.Instance.Stream, Encoding.UTF8, true, 1024, true))
            {
                while (SocketManager.Instance.Client.Connected)
                {
                    try
                    {
                        string msg = reader.ReadLine();
                        if (msg.StartsWith("LOBBY_DATA|"))
                        {
                            string roomList = msg.Substring("LOBBY_DATA|".Length);
                            this.Invoke(new UpdateLobbyDelegate(UpdateLobby), roomList);
                        }
                        else if (msg.StartsWith("ROOM_CREATED|"))
                        {
                            string roomId = msg.Substring("ROOM_CREATED|".Length);
                            MessageBox.Show("Phòng được tạo thành công với ID: " + roomId);
                        }
                        else if (msg.StartsWith("ERROR|"))
                        {
                            string errorMsg = msg.Substring("ERROR|".Length);
                            MessageBox.Show("Error: " + errorMsg);
                        }
                        else if (msg.StartsWith("GAME_START|"))
                        {
                            string[] parts = msg.Split('|');
                            if (parts.Length >= 5)
                            {
                                string playerOName = parts[1];
                                string playerXName = parts[2];
                                int.TryParse(parts[3], out int timePerMove);
                                int.TryParse(parts[4], out int playerSymbol);
                                this.Invoke(new Action(() => {
                                    // GameForm gameForm = new GameForm(playerOName, playerXName, playerSymbol, timePerMove);
                                    // gameForm.Show();
                                    this.Hide();
                                    MessageBox.Show($"Trận đấu bắt đầu! Bạn là quân {(playerSymbol == 1 ? "O" : "X")}");
                                }));
                            }
                        }
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
            string roomId = selectedItem.SubItems[0].Text;
            string joinRoomMsg = $"JOIN_ROOM|{roomId}";
            SocketManager.Instance.Send(joinRoomMsg);
        }

        private void LobbyForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (SocketManager.Instance.Client.Connected)
            {
                SocketManager.Instance.Send("LOGOUT|");
                SocketManager.Instance.Client.Close();
            }

            Application.Exit();
        }
    }
}
