namespace CaroClient
{
    partial class LobbyForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.lvRooms = new System.Windows.Forms.ListView();
            this.btnCreateRoom = new System.Windows.Forms.Button();
            this.btnJoinRoom = new System.Windows.Forms.Button();
            this.roomName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.roomInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.roomId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(30, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(722, 38);
            this.label1.TabIndex = 0;
            this.label1.Text = "Lobby";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lvRooms
            // 
            this.lvRooms.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.roomId,
            this.roomName,
            this.roomInfo});
            this.lvRooms.FullRowSelect = true;
            this.lvRooms.HideSelection = false;
            this.lvRooms.Location = new System.Drawing.Point(33, 87);
            this.lvRooms.MultiSelect = false;
            this.lvRooms.Name = "lvRooms";
            this.lvRooms.Size = new System.Drawing.Size(716, 511);
            this.lvRooms.TabIndex = 1;
            this.lvRooms.UseCompatibleStateImageBehavior = false;
            this.lvRooms.View = System.Windows.Forms.View.Details;
            // 
            // btnCreateRoom
            // 
            this.btnCreateRoom.AutoSize = true;
            this.btnCreateRoom.Location = new System.Drawing.Point(33, 630);
            this.btnCreateRoom.Name = "btnCreateRoom";
            this.btnCreateRoom.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnCreateRoom.Size = new System.Drawing.Size(350, 40);
            this.btnCreateRoom.TabIndex = 2;
            this.btnCreateRoom.Text = "Tạo phòng";
            this.btnCreateRoom.UseVisualStyleBackColor = true;
            this.btnCreateRoom.Click += new System.EventHandler(this.btnCreateRoom_Click);
            // 
            // btnJoinRoom
            // 
            this.btnJoinRoom.AutoSize = true;
            this.btnJoinRoom.Location = new System.Drawing.Point(399, 630);
            this.btnJoinRoom.Name = "btnJoinRoom";
            this.btnJoinRoom.Size = new System.Drawing.Size(350, 40);
            this.btnJoinRoom.TabIndex = 3;
            this.btnJoinRoom.Text = "Vào phòng";
            this.btnJoinRoom.UseVisualStyleBackColor = true;
            this.btnJoinRoom.Click += new System.EventHandler(this.btnJoinRoom_Click);
            // 
            // roomName
            // 
            this.roomName.Text = "Tên phòng";
            this.roomName.Width = 466;
            // 
            // roomInfo
            // 
            this.roomInfo.Text = "Số người";
            this.roomInfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.roomInfo.Width = 200;
            // 
            // roomId
            // 
            this.roomId.Text = "#";
            // 
            // LobbyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 696);
            this.Controls.Add(this.btnJoinRoom);
            this.Controls.Add(this.btnCreateRoom);
            this.Controls.Add(this.lvRooms);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "LobbyForm";
            this.Padding = new System.Windows.Forms.Padding(30);
            this.Text = "LobbyForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvRooms;
        private System.Windows.Forms.Button btnCreateRoom;
        private System.Windows.Forms.Button btnJoinRoom;
        private System.Windows.Forms.ColumnHeader roomName;
        private System.Windows.Forms.ColumnHeader roomInfo;
        private System.Windows.Forms.ColumnHeader roomId;
    }
}