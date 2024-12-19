namespace WinFormsApp1
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            label1 = new Label();
            tabPage2 = new TabPage();
            label4 = new Label();
            HideButton = new Button();
            MessageToHideTextBox = new TextBox();
            FinalFileButton = new Button();
            SourceFileButton = new Button();
            FinalFileTextBox = new TextBox();
            SourceFileTextBox = new TextBox();
            tabPage3 = new TabPage();
            ExtractedMessageLabel = new TextBox();
            label3 = new Label();
            ExtractButton = new Button();
            ExtractFileTextBox = new TextBox();
            imageList = new ImageList(components);
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Font = new Font("Ebrima", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tabControl1.ImageList = imageList;
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(968, 600);
            tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.Transparent;
            tabPage1.Controls.Add(pictureBox1);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(label1);
            tabPage1.ForeColor = Color.Black;
            tabPage1.ImageIndex = 0;
            tabPage1.Location = new Point(4, 34);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(960, 562);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Home page";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(457, 235);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(436, 291);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.Location = new Point(53, 503);
            label2.Name = "label2";
            label2.Size = new Size(201, 23);
            label2.TabIndex = 1;
            label2.Text = "Tymofii Khomych";
            // 
            // label1
            // 
            label1.Font = new Font("Ebrima", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(53, 18);
            label1.Name = "label1";
            label1.Size = new Size(697, 199);
            label1.TabIndex = 0;
            label1.Text = resources.GetString("label1.Text");
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(label4);
            tabPage2.Controls.Add(HideButton);
            tabPage2.Controls.Add(MessageToHideTextBox);
            tabPage2.Controls.Add(FinalFileButton);
            tabPage2.Controls.Add(SourceFileButton);
            tabPage2.Controls.Add(FinalFileTextBox);
            tabPage2.Controls.Add(SourceFileTextBox);
            tabPage2.ImageIndex = 1;
            tabPage2.Location = new Point(4, 34);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(960, 562);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Hide message";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.Location = new Point(33, 218);
            label4.Name = "label4";
            label4.Size = new Size(450, 35);
            label4.TabIndex = 10;
            label4.Text = "Type message to hide:";
            // 
            // HideButton
            // 
            HideButton.BackColor = Color.SteelBlue;
            HideButton.ForeColor = Color.Transparent;
            HideButton.Location = new Point(653, 470);
            HideButton.Name = "HideButton";
            HideButton.Size = new Size(134, 56);
            HideButton.TabIndex = 9;
            HideButton.Text = "Hide";
            HideButton.UseVisualStyleBackColor = false;
            HideButton.Click += HideButton_Click;
            // 
            // MessageToHideTextBox
            // 
            MessageToHideTextBox.Font = new Font("Cambria", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MessageToHideTextBox.Location = new Point(33, 268);
            MessageToHideTextBox.Multiline = true;
            MessageToHideTextBox.Name = "MessageToHideTextBox";
            MessageToHideTextBox.Size = new Size(566, 258);
            MessageToHideTextBox.TabIndex = 8;
            // 
            // FinalFileButton
            // 
            FinalFileButton.BackColor = Color.WhiteSmoke;
            FinalFileButton.ForeColor = Color.SteelBlue;
            FinalFileButton.Location = new Point(653, 123);
            FinalFileButton.Name = "FinalFileButton";
            FinalFileButton.Size = new Size(126, 40);
            FinalFileButton.TabIndex = 7;
            FinalFileButton.Text = "Final file";
            FinalFileButton.UseVisualStyleBackColor = false;
            FinalFileButton.Click += FinalFileButton_Click;
            // 
            // SourceFileButton
            // 
            SourceFileButton.BackColor = Color.WhiteSmoke;
            SourceFileButton.FlatAppearance.BorderColor = Color.SteelBlue;
            SourceFileButton.FlatAppearance.BorderSize = 5;
            SourceFileButton.ForeColor = Color.SteelBlue;
            SourceFileButton.Location = new Point(653, 60);
            SourceFileButton.Name = "SourceFileButton";
            SourceFileButton.Size = new Size(126, 39);
            SourceFileButton.TabIndex = 6;
            SourceFileButton.Text = "Source file";
            SourceFileButton.UseVisualStyleBackColor = false;
            SourceFileButton.Click += SourceFileButton_Click;
            // 
            // FinalFileTextBox
            // 
            FinalFileTextBox.Font = new Font("Cambria", 12F);
            FinalFileTextBox.Location = new Point(33, 130);
            FinalFileTextBox.Multiline = true;
            FinalFileTextBox.Name = "FinalFileTextBox";
            FinalFileTextBox.ReadOnly = true;
            FinalFileTextBox.Size = new Size(566, 33);
            FinalFileTextBox.TabIndex = 5;
            // 
            // SourceFileTextBox
            // 
            SourceFileTextBox.Font = new Font("Cambria", 12F);
            SourceFileTextBox.Location = new Point(33, 66);
            SourceFileTextBox.Multiline = true;
            SourceFileTextBox.Name = "SourceFileTextBox";
            SourceFileTextBox.ReadOnly = true;
            SourceFileTextBox.Size = new Size(566, 33);
            SourceFileTextBox.TabIndex = 4;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(ExtractedMessageLabel);
            tabPage3.Controls.Add(label3);
            tabPage3.Controls.Add(ExtractButton);
            tabPage3.Controls.Add(ExtractFileTextBox);
            tabPage3.ImageIndex = 2;
            tabPage3.Location = new Point(4, 34);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(960, 562);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Extract message";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // ExtractedMessageLabel
            // 
            ExtractedMessageLabel.Font = new Font("Cambria", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ExtractedMessageLabel.Location = new Point(55, 232);
            ExtractedMessageLabel.Multiline = true;
            ExtractedMessageLabel.Name = "ExtractedMessageLabel";
            ExtractedMessageLabel.Size = new Size(771, 264);
            ExtractedMessageLabel.TabIndex = 3;
            // 
            // label3
            // 
            label3.Location = new Point(55, 186);
            label3.Name = "label3";
            label3.Size = new Size(259, 24);
            label3.TabIndex = 2;
            label3.Text = "Obtained message:";
            // 
            // ExtractButton
            // 
            ExtractButton.BackColor = Color.SteelBlue;
            ExtractButton.ForeColor = Color.White;
            ExtractButton.Location = new Point(692, 65);
            ExtractButton.Name = "ExtractButton";
            ExtractButton.Size = new Size(134, 56);
            ExtractButton.TabIndex = 1;
            ExtractButton.Text = "Extract";
            ExtractButton.UseVisualStyleBackColor = false;
            ExtractButton.Click += ExtractButton_Click;
            // 
            // ExtractFileTextBox
            // 
            ExtractFileTextBox.BackColor = SystemColors.Control;
            ExtractFileTextBox.Font = new Font("Cambria", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ExtractFileTextBox.Location = new Point(55, 83);
            ExtractFileTextBox.Multiline = true;
            ExtractFileTextBox.Name = "ExtractFileTextBox";
            ExtractFileTextBox.Size = new Size(566, 33);
            ExtractFileTextBox.TabIndex = 0;
            // 
            // imageList
            // 
            imageList.ColorDepth = ColorDepth.Depth32Bit;
            imageList.ImageSize = new Size(16, 16);
            imageList.TransparentColor = Color.Transparent;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(992, 624);
            Controls.Add(tabControl1);
            Font = new Font("Ebrima", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "Form1";
            Text = "Steganography";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button FinalFileButton;
        private Button SourceFileButton;
        private TextBox FinalFileTextBox;
        private TextBox SourceFileTextBox;
        private Button HideButton;
        private TextBox MessageToHideTextBox;
        private TabPage tabPage3;
        private Label label1;
        private ImageList imageList;
        private Label label2;
        private Label label3;
        private Button ExtractButton;
        private TextBox ExtractFileTextBox;
        private PictureBox pictureBox1;
        private TextBox ExtractedMessageLabel;
        private Label label4;
    }
}
