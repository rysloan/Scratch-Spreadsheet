
namespace SpreadsheetGUI
{
    partial class SpreadsheetGUI
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
            this.SpreadsheetCells = new SS.SpreadsheetPanel();
            this.LabelCellName = new System.Windows.Forms.Label();
            this.TextCellName = new System.Windows.Forms.TextBox();
            this.LabelCellContent = new System.Windows.Forms.Label();
            this.LabelCellValue = new System.Windows.Forms.Label();
            this.TextCellValue = new System.Windows.Forms.TextBox();
            this.TextCellContent = new System.Windows.Forms.TextBox();
            this.SetValueButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GCDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GCDtext1 = new System.Windows.Forms.TextBox();
            this.GCDlabel = new System.Windows.Forms.Label();
            this.GCDtext2 = new System.Windows.Forms.TextBox();
            this.GCDbutton = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SpreadsheetCells
            // 
            this.SpreadsheetCells.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SpreadsheetCells.Location = new System.Drawing.Point(12, 108);
            this.SpreadsheetCells.Name = "SpreadsheetCells";
            this.SpreadsheetCells.Size = new System.Drawing.Size(1059, 404);
            this.SpreadsheetCells.TabIndex = 0;
            // 
            // LabelCellName
            // 
            this.LabelCellName.AutoSize = true;
            this.LabelCellName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCellName.Location = new System.Drawing.Point(12, 67);
            this.LabelCellName.Name = "LabelCellName";
            this.LabelCellName.Size = new System.Drawing.Size(64, 29);
            this.LabelCellName.TabIndex = 1;
            this.LabelCellName.Text = "Cell:";
            // 
            // TextCellName
            // 
            this.TextCellName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextCellName.Location = new System.Drawing.Point(82, 64);
            this.TextCellName.Name = "TextCellName";
            this.TextCellName.ReadOnly = true;
            this.TextCellName.Size = new System.Drawing.Size(100, 36);
            this.TextCellName.TabIndex = 2;
            // 
            // LabelCellContent
            // 
            this.LabelCellContent.AutoSize = true;
            this.LabelCellContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCellContent.Location = new System.Drawing.Point(242, 67);
            this.LabelCellContent.Name = "LabelCellContent";
            this.LabelCellContent.Size = new System.Drawing.Size(108, 29);
            this.LabelCellContent.TabIndex = 3;
            this.LabelCellContent.Text = "Content:";
            // 
            // LabelCellValue
            // 
            this.LabelCellValue.AutoSize = true;
            this.LabelCellValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCellValue.Location = new System.Drawing.Point(581, 67);
            this.LabelCellValue.Name = "LabelCellValue";
            this.LabelCellValue.Size = new System.Drawing.Size(85, 29);
            this.LabelCellValue.TabIndex = 4;
            this.LabelCellValue.Text = "Value:";
            // 
            // TextCellValue
            // 
            this.TextCellValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextCellValue.Location = new System.Drawing.Point(672, 63);
            this.TextCellValue.Name = "TextCellValue";
            this.TextCellValue.ReadOnly = true;
            this.TextCellValue.Size = new System.Drawing.Size(149, 36);
            this.TextCellValue.TabIndex = 5;
            // 
            // TextCellContent
            // 
            this.TextCellContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextCellContent.Location = new System.Drawing.Point(356, 64);
            this.TextCellContent.Name = "TextCellContent";
            this.TextCellContent.Size = new System.Drawing.Size(149, 36);
            this.TextCellContent.TabIndex = 6;
            // 
            // SetValueButton
            // 
            this.SetValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SetValueButton.Location = new System.Drawing.Point(833, 60);
            this.SetValueButton.Name = "SetValueButton";
            this.SetValueButton.Size = new System.Drawing.Size(167, 40);
            this.SetValueButton.TabIndex = 7;
            this.SetValueButton.Text = "Set Value";
            this.SetValueButton.UseVisualStyleBackColor = true;
            this.SetValueButton.Click += new System.EventHandler(this.SetValueButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.GCDToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1083, 28);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.newToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(50, 26);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(58, 24);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // GCDToolStripMenuItem
            // 
            this.GCDToolStripMenuItem.Name = "GCDToolStripMenuItem";
            this.GCDToolStripMenuItem.Size = new System.Drawing.Size(61, 24);
            this.GCDToolStripMenuItem.Text = "GCD";
            this.GCDToolStripMenuItem.Click += new System.EventHandler(this.GCDToolStripMenuItem_Click);
            // 
            // GCDtext1
            // 
            this.GCDtext1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GCDtext1.Location = new System.Drawing.Point(384, 9);
            this.GCDtext1.Name = "GCDtext1";
            this.GCDtext1.Size = new System.Drawing.Size(100, 27);
            this.GCDtext1.TabIndex = 9;
            this.GCDtext1.Visible = false;
            // 
            // GCDlabel
            // 
            this.GCDlabel.AutoSize = true;
            this.GCDlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GCDlabel.Location = new System.Drawing.Point(179, 12);
            this.GCDlabel.Name = "GCDlabel";
            this.GCDlabel.Size = new System.Drawing.Size(171, 20);
            this.GCDlabel.TabIndex = 10;
            this.GCDlabel.Text = "Enter two cell names:";
            this.GCDlabel.Visible = false;
            // 
            // GCDtext2
            // 
            this.GCDtext2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GCDtext2.Location = new System.Drawing.Point(490, 9);
            this.GCDtext2.Name = "GCDtext2";
            this.GCDtext2.Size = new System.Drawing.Size(100, 27);
            this.GCDtext2.TabIndex = 11;
            this.GCDtext2.Visible = false;
            // 
            // GCDbutton
            // 
            this.GCDbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GCDbutton.Location = new System.Drawing.Point(614, 8);
            this.GCDbutton.Name = "GCDbutton";
            this.GCDbutton.Size = new System.Drawing.Size(146, 29);
            this.GCDbutton.TabIndex = 12;
            this.GCDbutton.Text = "Calculate GCD";
            this.GCDbutton.UseVisualStyleBackColor = true;
            this.GCDbutton.Visible = false;
            this.GCDbutton.Click += new System.EventHandler(this.GCDbutton_Click);
            // 
            // SpreadsheetGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1083, 510);
            this.Controls.Add(this.GCDbutton);
            this.Controls.Add(this.GCDtext2);
            this.Controls.Add(this.GCDlabel);
            this.Controls.Add(this.GCDtext1);
            this.Controls.Add(this.SetValueButton);
            this.Controls.Add(this.TextCellContent);
            this.Controls.Add(this.TextCellValue);
            this.Controls.Add(this.LabelCellValue);
            this.Controls.Add(this.LabelCellContent);
            this.Controls.Add(this.TextCellName);
            this.Controls.Add(this.LabelCellName);
            this.Controls.Add(this.SpreadsheetCells);
            this.Controls.Add(this.menuStrip1);
            this.Name = "SpreadsheetGUI";
            this.Text = "Spreadsheet";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpreadsheetGUI_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SS.SpreadsheetPanel SpreadsheetCells;
        private System.Windows.Forms.Label LabelCellName;
        private System.Windows.Forms.TextBox TextCellName;
        private System.Windows.Forms.Label LabelCellContent;
        private System.Windows.Forms.Label LabelCellValue;
        private System.Windows.Forms.TextBox TextCellValue;
        private System.Windows.Forms.TextBox TextCellContent;
        private System.Windows.Forms.Button SetValueButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem GCDToolStripMenuItem;
        private System.Windows.Forms.TextBox GCDtext1;
        private System.Windows.Forms.Label GCDlabel;
        private System.Windows.Forms.TextBox GCDtext2;
        private System.Windows.Forms.Button GCDbutton;
    }
}

