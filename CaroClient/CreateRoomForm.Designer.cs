namespace CaroClient
{
    partial class CreateRoomForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateRoomForm));
            this.lblRoomName = new System.Windows.Forms.Label();
            this.txtRoomName = new System.Windows.Forms.TextBox();
            this.lblRoomTimer = new System.Windows.Forms.Label();
            this.nudRoomTimer = new System.Windows.Forms.NumericUpDown();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudRoomTimer)).BeginInit();
            this.SuspendLayout();
            // 
            // lblRoomName
            // 
            this.lblRoomName.AutoSize = true;
            this.lblRoomName.Location = new System.Drawing.Point(23, 20);
            this.lblRoomName.Name = "lblRoomName";
            this.lblRoomName.Size = new System.Drawing.Size(106, 23);
            this.lblRoomName.TabIndex = 0;
            this.lblRoomName.Text = "Room Name";
            // 
            // txtRoomName
            // 
            this.txtRoomName.Location = new System.Drawing.Point(175, 20);
            this.txtRoomName.MaxLength = 64;
            this.txtRoomName.Name = "txtRoomName";
            this.txtRoomName.Size = new System.Drawing.Size(413, 30);
            this.txtRoomName.TabIndex = 1;
            // 
            // lblRoomTimer
            // 
            this.lblRoomTimer.AutoSize = true;
            this.lblRoomTimer.Location = new System.Drawing.Point(23, 89);
            this.lblRoomTimer.Name = "lblRoomTimer";
            this.lblRoomTimer.Size = new System.Drawing.Size(123, 23);
            this.lblRoomTimer.TabIndex = 2;
            this.lblRoomTimer.Text = "Time Per Move";
            // 
            // nudRoomTimer
            // 
            this.nudRoomTimer.Location = new System.Drawing.Point(175, 82);
            this.nudRoomTimer.Margin = new System.Windows.Forms.Padding(40);
            this.nudRoomTimer.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.nudRoomTimer.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudRoomTimer.Name = "nudRoomTimer";
            this.nudRoomTimer.Size = new System.Drawing.Size(120, 30);
            this.nudRoomTimer.TabIndex = 3;
            this.nudRoomTimer.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // btnOK
            // 
            this.btnOK.AutoSize = true;
            this.btnOK.Location = new System.Drawing.Point(27, 151);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(270, 33);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "Tạo phòng";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.Location = new System.Drawing.Point(308, 151);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(280, 33);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Hủy bỏ";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // CreateRoomForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 212);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.nudRoomTimer);
            this.Controls.Add(this.lblRoomTimer);
            this.Controls.Add(this.txtRoomName);
            this.Controls.Add(this.lblRoomName);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "CreateRoomForm";
            this.Padding = new System.Windows.Forms.Padding(20);
            this.Text = "Enter Room Information";
            ((System.ComponentModel.ISupportInitialize)(this.nudRoomTimer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRoomName;
        private System.Windows.Forms.TextBox txtRoomName;
        private System.Windows.Forms.Label lblRoomTimer;
        private System.Windows.Forms.NumericUpDown nudRoomTimer;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}