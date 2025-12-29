namespace CaroClient
{
    partial class GameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.pbxGameGrid = new System.Windows.Forms.PictureBox();
            this.panelRightInfo = new System.Windows.Forms.Panel();
            this.lblPlayer1 = new System.Windows.Forms.Label();
            this.lblPlayer2 = new System.Windows.Forms.Label();
            this.lblTimer = new System.Windows.Forms.Label();
            this.lblScore = new System.Windows.Forms.Label();
            this.turnTimer = new System.Windows.Forms.Timer(this.components);
            this.mainLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxGameGrid)).BeginInit();
            this.panelRightInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayout
            // 
            this.mainLayout.ColumnCount = 2;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 667F));
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Controls.Add(this.pbxGameGrid, 0, 0);
            this.mainLayout.Controls.Add(this.panelRightInfo, 1, 0);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.RowCount = 1;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Size = new System.Drawing.Size(1200, 923);
            this.mainLayout.TabIndex = 0;
            // 
            // pbxGameGrid
            // 
            this.pbxGameGrid.BackColor = System.Drawing.Color.White;
            this.pbxGameGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbxGameGrid.Location = new System.Drawing.Point(3, 4);
            this.pbxGameGrid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pbxGameGrid.Name = "pbxGameGrid";
            this.pbxGameGrid.Size = new System.Drawing.Size(661, 915);
            this.pbxGameGrid.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbxGameGrid.TabIndex = 0;
            this.pbxGameGrid.TabStop = false;
            this.pbxGameGrid.Paint += new System.Windows.Forms.PaintEventHandler(this.pbxGameGrid_Paint);
            this.pbxGameGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbxGameGrid_MouseClick);
            // 
            // panelRightInfo
            // 
            this.panelRightInfo.AutoSize = true;
            this.panelRightInfo.Controls.Add(this.lblPlayer1);
            this.panelRightInfo.Controls.Add(this.lblPlayer2);
            this.panelRightInfo.Controls.Add(this.lblTimer);
            this.panelRightInfo.Controls.Add(this.lblScore);
            this.panelRightInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRightInfo.Location = new System.Drawing.Point(670, 4);
            this.panelRightInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelRightInfo.Name = "panelRightInfo";
            this.panelRightInfo.Padding = new System.Windows.Forms.Padding(27, 25, 27, 25);
            this.panelRightInfo.Size = new System.Drawing.Size(527, 915);
            this.panelRightInfo.TabIndex = 1;
            // 
            // lblPlayer1
            // 
            this.lblPlayer1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblPlayer1.ForeColor = System.Drawing.Color.Red;
            this.lblPlayer1.Location = new System.Drawing.Point(27, 25);
            this.lblPlayer1.Name = "lblPlayer1";
            this.lblPlayer1.Size = new System.Drawing.Size(160, 32);
            this.lblPlayer1.TabIndex = 0;
            this.lblPlayer1.Text = "Player 1 (O)";
            // 
            // lblPlayer2
            // 
            this.lblPlayer2.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblPlayer2.ForeColor = System.Drawing.Color.Blue;
            this.lblPlayer2.Location = new System.Drawing.Point(27, 189);
            this.lblPlayer2.Name = "lblPlayer2";
            this.lblPlayer2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.lblPlayer2.Size = new System.Drawing.Size(160, 44);
            this.lblPlayer2.TabIndex = 1;
            this.lblPlayer2.Text = "Player 2 (X)";
            this.lblPlayer2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblTimer.ForeColor = System.Drawing.Color.Black;
            this.lblTimer.Location = new System.Drawing.Point(125, 92);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(137, 60);
            this.lblTimer.TabIndex = 2;
            this.lblTimer.Text = "00:00";
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblScore.ForeColor = System.Drawing.Color.Gray;
            this.lblScore.Location = new System.Drawing.Point(30, 99);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(58, 50);
            this.lblScore.TabIndex = 3;
            this.lblScore.Text = "vs";
            // 
            // turnTimer
            // 
            this.turnTimer.Interval = 1000;
            this.turnTimer.Tick += new System.EventHandler(this.turnTimer_Tick);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1200, 923);
            this.Controls.Add(this.mainLayout);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "GameForm";
            this.Text = "Caro Online";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GameForm_FormClosed);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GameForm_Paint);
            this.mainLayout.ResumeLayout(false);
            this.mainLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxGameGrid)).EndInit();
            this.panelRightInfo.ResumeLayout(false);
            this.panelRightInfo.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.PictureBox pbxGameGrid;
        private System.Windows.Forms.Panel panelRightInfo;
        private System.Windows.Forms.Label lblPlayer1;
        private System.Windows.Forms.Label lblPlayer2;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Timer turnTimer;
    }
}