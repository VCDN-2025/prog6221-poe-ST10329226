// FILE: QuizForm.Designer.cs

namespace AwarenessChatbot
{
    partial class QuizForm
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
            btnQuizSubmit = new Button();
            btnCloseQuiz = new Button();
            rtbQuizOutput = new RichTextBox();
            txtQuizInput = new TextBox();
            SuspendLayout();
            // 
            // btnQuizSubmit
            // 
            btnQuizSubmit.BackColor = Color.CornflowerBlue;
            btnQuizSubmit.Location = new Point(413, 375);
            btnQuizSubmit.Name = "btnQuizSubmit";
            btnQuizSubmit.Size = new Size(110, 23);
            btnQuizSubmit.TabIndex = 0;
            btnQuizSubmit.Text = "Submit";
            btnQuizSubmit.UseVisualStyleBackColor = false;
            btnQuizSubmit.Click += btnQuizSubmit_Click;
            // 
            // btnCloseQuiz
            // 
            btnCloseQuiz.BackColor = Color.LightCoral;
            btnCloseQuiz.Location = new Point(559, 375);
            btnCloseQuiz.Name = "btnCloseQuiz";
            btnCloseQuiz.Size = new Size(100, 23);
            btnCloseQuiz.TabIndex = 1;
            btnCloseQuiz.Text = "Close Quiz";
            btnCloseQuiz.UseVisualStyleBackColor = false;
            btnCloseQuiz.Click += btnCloseQuiz_Click;
            // 
            // rtbQuizOutput
            // 
            rtbQuizOutput.Location = new Point(29, 32);
            rtbQuizOutput.Name = "rtbQuizOutput";
            rtbQuizOutput.Size = new Size(1019, 262);
            rtbQuizOutput.TabIndex = 2;
            rtbQuizOutput.Text = "";
            // 
            // txtQuizInput
            // 
            txtQuizInput.Location = new Point(413, 312);
            txtQuizInput.Name = "txtQuizInput";
            txtQuizInput.Size = new Size(246, 23);
            txtQuizInput.TabIndex = 3;
            // 
            // QuizForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1074, 450);
            Controls.Add(txtQuizInput);
            Controls.Add(rtbQuizOutput);
            Controls.Add(btnCloseQuiz);
            Controls.Add(btnQuizSubmit);
            Name = "QuizForm";
            Text = "Quiz Game";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        // Private declarations of the controls with their new names
        private System.Windows.Forms.Button btnQuizSubmit;
        private System.Windows.Forms.Button btnCloseQuiz;
        private System.Windows.Forms.RichTextBox rtbQuizOutput;
        private System.Windows.Forms.TextBox txtQuizInput;
    }
}