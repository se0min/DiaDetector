
namespace DiaDetector.SubForm
{
    partial class SmallLog
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonLogSave = new System.Windows.Forms.Button();
            this.buttonLogDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 85);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(776, 353);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "No.";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "로그 내용";
            this.columnHeader2.Width = 700;
            // 
            // buttonLogSave
            // 
            this.buttonLogSave.Font = new System.Drawing.Font("굴림", 22F);
            this.buttonLogSave.Location = new System.Drawing.Point(462, 12);
            this.buttonLogSave.Name = "buttonLogSave";
            this.buttonLogSave.Size = new System.Drawing.Size(160, 67);
            this.buttonLogSave.TabIndex = 1;
            this.buttonLogSave.Text = "로그 저장";
            this.buttonLogSave.UseVisualStyleBackColor = true;
            this.buttonLogSave.Click += new System.EventHandler(this.buttonLogSave_Click);
            // 
            // buttonLogDelete
            // 
            this.buttonLogDelete.Font = new System.Drawing.Font("굴림", 22F);
            this.buttonLogDelete.Location = new System.Drawing.Point(628, 12);
            this.buttonLogDelete.Name = "buttonLogDelete";
            this.buttonLogDelete.Size = new System.Drawing.Size(160, 67);
            this.buttonLogDelete.TabIndex = 2;
            this.buttonLogDelete.Text = "로그 삭제";
            this.buttonLogDelete.UseVisualStyleBackColor = true;
            this.buttonLogDelete.Click += new System.EventHandler(this.buttonLogDelete_Click);
            // 
            // SmallLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonLogDelete);
            this.Controls.Add(this.buttonLogSave);
            this.Controls.Add(this.listView1);
            this.Name = "SmallLog";
            this.Text = "SmallLog";
            this.Load += new System.EventHandler(this.SmallLog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button buttonLogSave;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button buttonLogDelete;
    }
}