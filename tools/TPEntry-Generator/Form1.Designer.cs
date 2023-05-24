namespace TPEntry_Generator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            tbFilename = new TextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(49, 40);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(210, 31);
            button1.TabIndex = 0;
            button1.Text = "Create Entry File";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // tbFilename
            // 
            tbFilename.Location = new Point(49, 78);
            tbFilename.Name = "tbFilename";
            tbFilename.Size = new Size(407, 27);
            tbFilename.TabIndex = 1;
            tbFilename.Text = "c:\\tmp\\entry.tp";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 600);
            Controls.Add(tbFilename);
            Controls.Add(button1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox tbFilename;
    }
}