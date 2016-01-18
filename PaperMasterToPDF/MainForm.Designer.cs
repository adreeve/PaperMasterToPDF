namespace PaperMasterToPDF
{
    partial class MainForm
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
            this.OpenCabinetButton = new System.Windows.Forms.Button();
            this.DocumentsListBox = new System.Windows.Forms.CheckedListBox();
            this.SelectAllButton = new System.Windows.Forms.Button();
            this.DeselectAllButton = new System.Windows.Forms.Button();
            this.ExportButton = new System.Windows.Forms.Button();
            this.SelectPaperMasterCabinetDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SelectOutputFolderButton = new System.Windows.Forms.Button();
            this.CabinetFolderLabel = new System.Windows.Forms.Label();
            this.OutputFolderLabel = new System.Windows.Forms.Label();
            this.SelectOutputFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.PreviewCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // OpenCabinetButton
            // 
            this.OpenCabinetButton.Location = new System.Drawing.Point(12, 12);
            this.OpenCabinetButton.Name = "OpenCabinetButton";
            this.OpenCabinetButton.Size = new System.Drawing.Size(126, 23);
            this.OpenCabinetButton.TabIndex = 0;
            this.OpenCabinetButton.Text = "&Open Cabinet...";
            this.OpenCabinetButton.UseVisualStyleBackColor = true;
            this.OpenCabinetButton.Click += new System.EventHandler(this.OpenCabinetButton_Click);
            // 
            // DocumentsListBox
            // 
            this.DocumentsListBox.FormattingEnabled = true;
            this.DocumentsListBox.Location = new System.Drawing.Point(12, 99);
            this.DocumentsListBox.Name = "DocumentsListBox";
            this.DocumentsListBox.Size = new System.Drawing.Size(525, 259);
            this.DocumentsListBox.TabIndex = 1;
            // 
            // SelectAllButton
            // 
            this.SelectAllButton.Location = new System.Drawing.Point(117, 365);
            this.SelectAllButton.Name = "SelectAllButton";
            this.SelectAllButton.Size = new System.Drawing.Size(75, 23);
            this.SelectAllButton.TabIndex = 2;
            this.SelectAllButton.Text = "Select &All";
            this.SelectAllButton.UseVisualStyleBackColor = true;
            this.SelectAllButton.Click += new System.EventHandler(this.SelectAll_Click);
            // 
            // DeselectAllButton
            // 
            this.DeselectAllButton.Location = new System.Drawing.Point(198, 364);
            this.DeselectAllButton.Name = "DeselectAllButton";
            this.DeselectAllButton.Size = new System.Drawing.Size(105, 24);
            this.DeselectAllButton.TabIndex = 3;
            this.DeselectAllButton.Text = "&Deselect All";
            this.DeselectAllButton.UseVisualStyleBackColor = true;
            this.DeselectAllButton.Click += new System.EventHandler(this.DeselectAll_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(309, 365);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(123, 23);
            this.ExportButton.TabIndex = 4;
            this.ExportButton.Text = "&Export Selections...";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.Export_Click);
            // 
            // SelectPaperMasterCabinetDialog
            // 
            this.SelectPaperMasterCabinetDialog.Description = "Find the folder that contains your PaperMaster data.";
            this.SelectPaperMasterCabinetDialog.ShowNewFolderButton = false;
            // 
            // SelectOutputFolderButton
            // 
            this.SelectOutputFolderButton.Location = new System.Drawing.Point(12, 41);
            this.SelectOutputFolderButton.Name = "SelectOutputFolderButton";
            this.SelectOutputFolderButton.Size = new System.Drawing.Size(126, 23);
            this.SelectOutputFolderButton.TabIndex = 5;
            this.SelectOutputFolderButton.Text = "Select &Output Folder...";
            this.SelectOutputFolderButton.UseVisualStyleBackColor = true;
            this.SelectOutputFolderButton.Click += new System.EventHandler(this.SelectOutputFolderButton_Click);
            // 
            // CabinetFolderLabel
            // 
            this.CabinetFolderLabel.AutoSize = true;
            this.CabinetFolderLabel.Location = new System.Drawing.Point(144, 22);
            this.CabinetFolderLabel.Name = "CabinetFolderLabel";
            this.CabinetFolderLabel.Size = new System.Drawing.Size(98, 13);
            this.CabinetFolderLabel.TabIndex = 6;
            this.CabinetFolderLabel.Text = "CabinetFolderLabel";
            // 
            // OutputFolderLabel
            // 
            this.OutputFolderLabel.AutoSize = true;
            this.OutputFolderLabel.Location = new System.Drawing.Point(144, 51);
            this.OutputFolderLabel.Name = "OutputFolderLabel";
            this.OutputFolderLabel.Size = new System.Drawing.Size(94, 13);
            this.OutputFolderLabel.TabIndex = 7;
            this.OutputFolderLabel.Text = "OutputFolderLabel";
            // 
            // PreviewCheckBox
            // 
            this.PreviewCheckBox.AutoSize = true;
            this.PreviewCheckBox.Location = new System.Drawing.Point(171, 76);
            this.PreviewCheckBox.Name = "PreviewCheckBox";
            this.PreviewCheckBox.Size = new System.Drawing.Size(210, 17);
            this.PreviewCheckBox.TabIndex = 8;
            this.PreviewCheckBox.Text = "Show Converted Files After Conversion";
            this.PreviewCheckBox.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 400);
            this.Controls.Add(this.PreviewCheckBox);
            this.Controls.Add(this.OutputFolderLabel);
            this.Controls.Add(this.CabinetFolderLabel);
            this.Controls.Add(this.SelectOutputFolderButton);
            this.Controls.Add(this.ExportButton);
            this.Controls.Add(this.DeselectAllButton);
            this.Controls.Add(this.SelectAllButton);
            this.Controls.Add(this.DocumentsListBox);
            this.Controls.Add(this.OpenCabinetButton);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "PaperMaster to PDF Exporter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OpenCabinetButton;
        private System.Windows.Forms.CheckedListBox DocumentsListBox;
        private System.Windows.Forms.Button SelectAllButton;
        private System.Windows.Forms.Button DeselectAllButton;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.FolderBrowserDialog SelectPaperMasterCabinetDialog;
        private System.Windows.Forms.Button SelectOutputFolderButton;
        private System.Windows.Forms.Label CabinetFolderLabel;
        private System.Windows.Forms.Label OutputFolderLabel;
        private System.Windows.Forms.FolderBrowserDialog SelectOutputFolderDialog;
        private System.Windows.Forms.CheckBox PreviewCheckBox;
    }
}

