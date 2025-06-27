// FILE: Form1.Designer.cs

namespace AwarenessChatbot
{
    partial class Form1
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
            rtbChatOutput = new RichTextBox();
            txtUserInput = new TextBox();
            btnSend = new Button();
            btnQuiz = new Button();
            btnMainMenu = new Button();
            btnActivityLog = new Button();
            SuspendLayout();
            // 
            // rtbChatOutput
            // 
            rtbChatOutput.Location = new Point(12, 12);
            rtbChatOutput.Name = "rtbChatOutput";
            rtbChatOutput.ReadOnly = true;
            rtbChatOutput.Size = new Size(1035, 350);
            rtbChatOutput.TabIndex = 0;
            rtbChatOutput.Text = "";
            // 
            // txtUserInput
            // 
            txtUserInput.Location = new Point(211, 368);
            txtUserInput.Name = "txtUserInput";
            txtUserInput.Size = new Size(600, 23);
            txtUserInput.TabIndex = 1;
            // 
            // btnSend
            // 
            btnSend.BackColor = Color.CornflowerBlue;
            btnSend.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSend.ForeColor = Color.White;
            btnSend.Location = new Point(172, 397);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(142, 30);
            btnSend.TabIndex = 2;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = false;
            // 
            // btnQuiz
            // 
            btnQuiz.BackColor = Color.MediumSeaGreen;
            btnQuiz.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnQuiz.ForeColor = Color.White;
            btnQuiz.Location = new Point(538, 397);
            btnQuiz.Name = "btnQuiz";
            btnQuiz.Size = new Size(142, 30);
            btnQuiz.TabIndex = 3;
            btnQuiz.Text = "Quiz";
            btnQuiz.UseVisualStyleBackColor = false;
            // 
            // btnMainMenu
            // 
            btnMainMenu.BackColor = Color.DarkOrange;
            btnMainMenu.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnMainMenu.ForeColor = Color.White;
            btnMainMenu.Location = new Point(320, 397);
            btnMainMenu.Name = "btnMainMenu";
            btnMainMenu.Size = new Size(142, 30);
            btnMainMenu.TabIndex = 4;
            btnMainMenu.Text = "Main Menu";
            btnMainMenu.UseVisualStyleBackColor = false;
            // 
            // btnActivityLog
            // 
            btnActivityLog.BackColor = Color.LightSteelBlue;
            btnActivityLog.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnActivityLog.ForeColor = Color.Black;
            btnActivityLog.Location = new Point(686, 397);
            btnActivityLog.Name = "btnActivityLog";
            btnActivityLog.Size = new Size(145, 30);
            btnActivityLog.TabIndex = 5;
            btnActivityLog.Text = "Activity Log";
            btnActivityLog.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1059, 461);
            Controls.Add(btnActivityLog);
            Controls.Add(btnMainMenu);
            Controls.Add(btnQuiz);
            Controls.Add(btnSend);
            Controls.Add(txtUserInput);
            Controls.Add(rtbChatOutput);
            Name = "Form1";
            Text = "Awareness Bot";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        // Declare the controls here with their correct names
        private System.Windows.Forms.RichTextBox rtbChatOutput;
        private System.Windows.Forms.TextBox txtUserInput;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnQuiz;
        private System.Windows.Forms.Button btnMainMenu; // Declare btnMainMenu here
        private Button btnActivityLog;
    }
}