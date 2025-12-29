using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CaroLibrary.CaroLibrary;
using Timer = System.Windows.Forms.Timer;

namespace CaroClient
{
    public partial class GameForm : Form
    {
        private string _roomId;
        private int _mySymbol; 
        private int _currentTurn;
        private int[,] _board;
        private bool _isGameActive;
        
        private int _timePerMove;
        private int _timeLeftSeconds;
        private const int CellSize = 20;
        private const int GridMargin = 20;
        private int GridPixelSize => CaroConfig.BOARD_SIZE * CellSize;
        
        
        public GameForm(string roomId, string playerOName, string playerXName, int mySymbol, int timePerMove)
        {
            InitializeComponent();
            
            _roomId = roomId;
            _mySymbol = mySymbol;
            _timePerMove = timePerMove;
            _board = new int[CaroConfig.BOARD_SIZE, CaroConfig.BOARD_SIZE];
            _isGameActive = true;
            _currentTurn = CaroConfig.PLAYER_O; 

            lblPlayer1.Text = playerOName + " (O)";
            lblPlayer2.Text = playerXName + " (X)";
    
            if (_mySymbol == CaroConfig.PLAYER_O) lblPlayer1.ForeColor = Color.Red;
            else lblPlayer2.ForeColor = Color.Blue;

            UpdateTurnUI();
            ResetTurnTimer();
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Task.Run(() => ListenForServerMsg());
        }
        private void ListenForServerMsg()
        {
            try
            {
                StreamReader reader = SocketManager.Instance.Reader;

                while (SocketManager.Instance.Client.Connected)
                {
                    string msg = reader.ReadLine();
                    if (msg == null) break;
                    
                    Console.WriteLine($"Client Received: {msg}");
                    ;

                    this.Invoke(new Action(() => ProcessServerMessage(msg)));
                }
            }
            catch (Exception ex)
            {
                if (!_isGameActive) return;
                MessageBox.Show("Lost connection: " + ex.Message);
                CloseGame();
            }
        }

        private void ProcessServerMessage(string msg)
        {
            string[] parts = msg.Split('|');
            string command = parts[0];

            switch (command)
            {
                case "MOVE_UPDATE":
                    if (parts.Length >= 5)
                    {
                        int x = int.Parse(parts[1]);
                        int y = int.Parse(parts[2]);
                        int playerWhoMoved = int.Parse(parts[3]);
                        int nextTurn = int.Parse(parts[4]);

                        _board[x, y] = playerWhoMoved;
                        _currentTurn = nextTurn;
                        
                        pbxGameGrid.Invalidate();
                        ResetTurnTimer();
                        UpdateTurnUI();
                    }
                    break;

                case "GAME_OVER":
                    HandleGameOver(parts);
                    break;
                
                case "REMATCH_REQUEST":
                    DialogResult result = MessageBox.Show(
                        "Đối thủ muốn chơi ván mới. Bạn có đồng ý không?", 
                        "Yêu cầu tái đấu", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        SocketManager.Instance.Send($"REMATCH_ACCEPT|{_roomId}");

                    }
                    else
                    {
                        LeaveRoom();
                    }
                    break;
                
                case "OPPONENT_LEFT":
                    MessageBox.Show("Đối thủ đã thoát hoặc từ chối tái đấu.", "Kết thúc");
                    CloseGame();
                    break;

                case "GAME_RESET":
                    ResetGame();
                    break;
            }
        }

        private void HandleGameOver(string[] parts)
        {
            _isGameActive = false;
            turnTimer.Stop();
            string subType = parts[1];
            bool iAmWinner = false;
            string message = "";

            if (subType == "WIN")
            {
                int winnerId = int.Parse(parts[2]);
                iAmWinner = (winnerId == _mySymbol);
                string winnerName = (winnerId == CaroConfig.PLAYER_O) ? lblPlayer1.Text : lblPlayer2.Text;
                if (iAmWinner) message = "Chúc mừng! Bạn đã chiến thắng!";
                else message = $"{winnerName} đã thắng!";;
            }
            else if (subType == "DRAW")
            {
                message = "Ván cờ Hòa!";
                iAmWinner = (_mySymbol == CaroConfig.PLAYER_O);
            }
            else if (subType == "OPPONENT_LEFT")
            {
                MessageBox.Show("Đối thủ đã thoát. Bạn thắng!", "Kết thúc");
                CloseGame(); 
                return;
            }

            if (iAmWinner)
            {
                DialogResult result = MessageBox.Show(
                    message + "\n\nBạn có muốn tái đấu không?",
                    "Kết thúc game",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    SocketManager.Instance.Send($"REMATCH_REQUEST|{_roomId}");
                    
                    result = MessageBox.Show("Đang chờ đối thủ...", "Đang chờ đối thủ...", MessageBoxButtons.OKCancel);

                    if (result == DialogResult.Cancel)
                    {
                        LeaveRoom();
                    }
                }
                if (result == DialogResult.Cancel)
                {
                    LeaveRoom();
                }
            }

            else
            {
                DialogResult result = MessageBox.Show(
                    message + "\n\nĐang chờ đối thủ...",
                    "Kết thúc game",
                    MessageBoxButtons.OKCancel);
                if (result == DialogResult.Cancel)
                {
                    LeaveRoom();
                }
            }
        }

        private void ResetGame()
        {

            _board = new int[CaroConfig.BOARD_SIZE, CaroConfig.BOARD_SIZE];
            _currentTurn = CaroConfig.PLAYER_O; 
            _isGameActive = true;

            pbxGameGrid.Invalidate();
            ResetTurnTimer();
            UpdateTurnUI();
            
            MessageBox.Show("Ván mới bắt đầu!");
        }

        private void LeaveRoom()
        {
            SocketManager.Instance.Send("LEAVE_ROOM|");
            CloseGame();
        }

        private void CloseGame()
        {
            _isGameActive = false;
            turnTimer.Stop();
            
            this.Hide();
            LobbyForm lobby = new LobbyForm(); 
            lobby.ShowDialog(); 
            this.Close();
        }
        private void pbxGameGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (!_isGameActive) return;
            
            if (_currentTurn != _mySymbol)
            {
                return; 
            }
            
            int relativeX = e.X - GridMargin;
            int relativeY = e.Y - GridMargin;
            if (relativeX < 0 || relativeY < 0 || relativeX >= GridPixelSize || relativeY >= GridPixelSize)
                return;

            int gridX = relativeX / CellSize;
            int gridY = relativeY / CellSize;
            
            if (_board[gridX, gridY] != CaroConfig.EMPTY)
                return;
            
            SocketManager.Instance.Send($"MOVE|{_roomId}|{gridX}|{gridY}");
        }
        

        private void ResetTurnTimer()
        {
            _timeLeftSeconds = _timePerMove;
            lblTimer.Text = _timeLeftSeconds.ToString("00");
            turnTimer.Start();
        }

        private void turnTimer_Tick(object sender, EventArgs e)
        {
            if (_timeLeftSeconds > 0)
            {
                _timeLeftSeconds--;
                lblTimer.Text = _timeLeftSeconds.ToString("00");
            }
        }

        private void UpdateTurnUI()
        {
            lblPlayer1.Font = new Font(lblPlayer1.Font, (_currentTurn == CaroConfig.PLAYER_O) ? FontStyle.Bold : FontStyle.Regular);
            lblPlayer2.Font = new Font(lblPlayer2.Font, (_currentTurn == CaroConfig.PLAYER_X) ? FontStyle.Bold : FontStyle.Regular);
            
            // Indicate if it is MY turn
            if (_currentTurn == _mySymbol)
                this.Text = "Caro Game - YOUR TURN";
            else
                this.Text = "Caro Game - Opponent's Turn";
        }

        private void pbxGameGrid_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TranslateTransform(GridMargin, GridMargin);

            Pen gridPen = new Pen(Color.LightGray, 1);
            Pen xPen = new Pen(Color.Red, 2);
            Pen oPen = new Pen(Color.Blue, 2);
            int padding = 4;
            
            for (int i = 0; i <= CaroConfig.BOARD_SIZE; i++)
            {
                g.DrawLine(gridPen, i * CellSize, 0, i * CellSize, GridPixelSize);
                g.DrawLine(gridPen, 0, i * CellSize, GridPixelSize, i * CellSize);
            }
            
            for (int i = 0; i < CaroConfig.BOARD_SIZE; i++)
            {
                for (int j = 0; j < CaroConfig.BOARD_SIZE; j++)
                {
                    int cell = _board[i, j];
                    if (cell == CaroConfig.EMPTY) continue;

                    int x = i * CellSize + padding;
                    int y = j * CellSize + padding;
                    int s = CellSize - (padding * 2);

                    if (cell == CaroConfig.PLAYER_O)
                        g.DrawEllipse(oPen, x, y, s, s);
                    else if (cell == CaroConfig.PLAYER_X)
                    {
                        g.DrawLine(xPen, x, y, x + s, y + s);
                        g.DrawLine(xPen, x + s, y, x, y + s);
                    }
                }
            }
        }

        private void GameForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (SocketManager.Instance.Client.Connected)
            {
                SocketManager.Instance.Send("LOGOUT|");
                SocketManager.Instance.Client.Close();
            }

            Application.Exit();
        }

        private void GameForm_Paint(object sender, PaintEventArgs e)
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

        private void panelRightInfo_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}