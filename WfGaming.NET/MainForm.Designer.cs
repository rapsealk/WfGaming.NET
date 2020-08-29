namespace WfGaming
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.BuildLabel = new System.Windows.Forms.Label();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.WorkerButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.ModFileOpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.ModLabel = new System.Windows.Forms.Label();
            this.ModFilePathTextBox = new System.Windows.Forms.TextBox();
            this.ModSelectButton = new System.Windows.Forms.Button();
            this.ModInstallButton = new System.Windows.Forms.Button();
            this.ModPanel = new System.Windows.Forms.Panel();
            this.ModPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // BuildLabel
            // 
            this.BuildLabel.AutoSize = true;
            this.BuildLabel.Location = new System.Drawing.Point(366, 150);
            this.BuildLabel.Name = "BuildLabel";
            this.BuildLabel.Size = new System.Drawing.Size(33, 12);
            this.BuildLabel.TabIndex = 0;
            this.BuildLabel.Text = "Build";
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Location = new System.Drawing.Point(366, 176);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(48, 12);
            this.VersionLabel.TabIndex = 1;
            this.VersionLabel.Text = "Version";
            // 
            // WorkerButton
            // 
            this.WorkerButton.Location = new System.Drawing.Point(53, 165);
            this.WorkerButton.Name = "WorkerButton";
            this.WorkerButton.Size = new System.Drawing.Size(75, 23);
            this.WorkerButton.TabIndex = 2;
            this.WorkerButton.Text = "실행";
            this.WorkerButton.UseVisualStyleBackColor = true;
            this.WorkerButton.Click += new System.EventHandler(this.WorkerButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(181, 165);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.Text = "취소";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ModFileOpenDialog
            // 
            this.ModFileOpenDialog.FileName = "openFileDialog1";
            // 
            // ModLabel
            // 
            this.ModLabel.AutoSize = true;
            this.ModLabel.Location = new System.Drawing.Point(3, 25);
            this.ModLabel.Name = "ModLabel";
            this.ModLabel.Size = new System.Drawing.Size(30, 12);
            this.ModLabel.TabIndex = 4;
            this.ModLabel.Text = "Mod";
            // 
            // ModFilePathTextBox
            // 
            this.ModFilePathTextBox.Enabled = false;
            this.ModFilePathTextBox.Location = new System.Drawing.Point(39, 22);
            this.ModFilePathTextBox.Name = "ModFilePathTextBox";
            this.ModFilePathTextBox.Size = new System.Drawing.Size(558, 21);
            this.ModFilePathTextBox.TabIndex = 5;
            // 
            // ModSelectButton
            // 
            this.ModSelectButton.Location = new System.Drawing.Point(50, 65);
            this.ModSelectButton.Name = "ModSelectButton";
            this.ModSelectButton.Size = new System.Drawing.Size(75, 23);
            this.ModSelectButton.TabIndex = 6;
            this.ModSelectButton.Text = "모드 선택";
            this.ModSelectButton.UseVisualStyleBackColor = true;
            this.ModSelectButton.Click += new System.EventHandler(this.ModSelectButton_Click);
            // 
            // ModInstallButton
            // 
            this.ModInstallButton.Location = new System.Drawing.Point(147, 65);
            this.ModInstallButton.Name = "ModInstallButton";
            this.ModInstallButton.Size = new System.Drawing.Size(75, 23);
            this.ModInstallButton.TabIndex = 7;
            this.ModInstallButton.Text = "설치";
            this.ModInstallButton.UseVisualStyleBackColor = true;
            this.ModInstallButton.Click += new System.EventHandler(this.ModInstallButton_Click);
            // 
            // ModPanel
            // 
            this.ModPanel.Controls.Add(this.ModLabel);
            this.ModPanel.Controls.Add(this.ModInstallButton);
            this.ModPanel.Controls.Add(this.ModFilePathTextBox);
            this.ModPanel.Controls.Add(this.ModSelectButton);
            this.ModPanel.Location = new System.Drawing.Point(12, 12);
            this.ModPanel.Name = "ModPanel";
            this.ModPanel.Size = new System.Drawing.Size(600, 100);
            this.ModPanel.TabIndex = 8;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 281);
            this.Controls.Add(this.ModPanel);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.WorkerButton);
            this.Controls.Add(this.VersionLabel);
            this.Controls.Add(this.BuildLabel);
            this.Name = "MainForm";
            this.Text = "WfGaming.NET";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ModPanel.ResumeLayout(false);
            this.ModPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label BuildLabel;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.Button WorkerButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.OpenFileDialog ModFileOpenDialog;
        private System.Windows.Forms.Label ModLabel;
        private System.Windows.Forms.TextBox ModFilePathTextBox;
        private System.Windows.Forms.Button ModSelectButton;
        private System.Windows.Forms.Button ModInstallButton;
        private System.Windows.Forms.Panel ModPanel;
    }
}

