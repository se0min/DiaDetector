namespace AutoAssembler.Data
{
    partial class frmModelList
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
            this.btnEditModel = new System.Windows.Forms.Button();
            this.btnNewModel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSelectModel = new System.Windows.Forms.Button();
            this.lvModelList = new System.Windows.Forms.ListView();
            this.btnModelCopy = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnSharedFolder = new System.Windows.Forms.Button();
            this.JDirList = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDeleteModel = new System.Windows.Forms.Button();
            this.btnDeleteFolder = new System.Windows.Forms.Button();
            this.btnDownloadDXF = new System.Windows.Forms.Button();
            this.btnUploadDXF = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEditModel
            // 
            this.btnEditModel.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnEditModel.Location = new System.Drawing.Point(523, 496);
            this.btnEditModel.Margin = new System.Windows.Forms.Padding(4);
            this.btnEditModel.Name = "btnEditModel";
            this.btnEditModel.Size = new System.Drawing.Size(120, 40);
            this.btnEditModel.TabIndex = 29;
            this.btnEditModel.Text = "모델 편집";
            this.btnEditModel.UseVisualStyleBackColor = true;
            this.btnEditModel.Click += new System.EventHandler(this.btnEditModel_Click);
            // 
            // btnNewModel
            // 
            this.btnNewModel.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnNewModel.Location = new System.Drawing.Point(395, 448);
            this.btnNewModel.Margin = new System.Windows.Forms.Padding(4);
            this.btnNewModel.Name = "btnNewModel";
            this.btnNewModel.Size = new System.Drawing.Size(120, 40);
            this.btnNewModel.TabIndex = 28;
            this.btnNewModel.Text = "신규 등록";
            this.btnNewModel.UseVisualStyleBackColor = true;
            this.btnNewModel.Click += new System.EventHandler(this.btnNewModel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClose.Location = new System.Drawing.Point(651, 544);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 40);
            this.btnClose.TabIndex = 26;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSelectModel
            // 
            this.btnSelectModel.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSelectModel.Location = new System.Drawing.Point(651, 448);
            this.btnSelectModel.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectModel.Name = "btnSelectModel";
            this.btnSelectModel.Size = new System.Drawing.Size(120, 40);
            this.btnSelectModel.TabIndex = 25;
            this.btnSelectModel.Text = "모델 선택";
            this.btnSelectModel.UseVisualStyleBackColor = true;
            this.btnSelectModel.Click += new System.EventHandler(this.btnSelectModel_Click);
            // 
            // lvModelList
            // 
            this.lvModelList.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lvModelList.FullRowSelect = true;
            this.lvModelList.HideSelection = false;
            this.lvModelList.Location = new System.Drawing.Point(395, 40);
            this.lvModelList.Margin = new System.Windows.Forms.Padding(4);
            this.lvModelList.Name = "lvModelList";
            this.lvModelList.Size = new System.Drawing.Size(376, 400);
            this.lvModelList.TabIndex = 24;
            this.lvModelList.UseCompatibleStateImageBehavior = false;
            this.lvModelList.View = System.Windows.Forms.View.Details;
            // 
            // btnModelCopy
            // 
            this.btnModelCopy.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnModelCopy.Location = new System.Drawing.Point(523, 448);
            this.btnModelCopy.Margin = new System.Windows.Forms.Padding(4);
            this.btnModelCopy.Name = "btnModelCopy";
            this.btnModelCopy.Size = new System.Drawing.Size(120, 40);
            this.btnModelCopy.TabIndex = 30;
            this.btnModelCopy.Text = "모델 복사";
            this.btnModelCopy.UseVisualStyleBackColor = true;
            this.btnModelCopy.Click += new System.EventHandler(this.btnModelCopy_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUpload.Location = new System.Drawing.Point(283, 193);
            this.btnUpload.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(104, 80);
            this.btnUpload.TabIndex = 31;
            this.btnUpload.Text = "<< Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDownload.Location = new System.Drawing.Point(283, 281);
            this.btnDownload.Margin = new System.Windows.Forms.Padding(4);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(104, 80);
            this.btnDownload.TabIndex = 32;
            this.btnDownload.Text = "Download >>";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnSharedFolder
            // 
            this.btnSharedFolder.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSharedFolder.Location = new System.Drawing.Point(395, 544);
            this.btnSharedFolder.Margin = new System.Windows.Forms.Padding(4);
            this.btnSharedFolder.Name = "btnSharedFolder";
            this.btnSharedFolder.Size = new System.Drawing.Size(248, 40);
            this.btnSharedFolder.TabIndex = 33;
            this.btnSharedFolder.Text = "공유 폴더 계정";
            this.btnSharedFolder.UseVisualStyleBackColor = true;
            this.btnSharedFolder.Click += new System.EventHandler(this.btnSharedFolder_Click);
            // 
            // JDirList
            // 
            this.JDirList.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.JDirList.FullRowSelect = true;
            this.JDirList.HideSelection = false;
            this.JDirList.Location = new System.Drawing.Point(13, 40);
            this.JDirList.Margin = new System.Windows.Forms.Padding(4);
            this.JDirList.Name = "JDirList";
            this.JDirList.Size = new System.Drawing.Size(262, 400);
            this.JDirList.TabIndex = 34;
            this.JDirList.UseCompatibleStateImageBehavior = false;
            this.JDirList.View = System.Windows.Forms.View.Details;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(480, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 16);
            this.label1.TabIndex = 35;
            this.label1.Text = "C 또는 I 드라이브(편집 위치) :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 16);
            this.label2.TabIndex = 36;
            this.label2.Text = "J 드라이브(공유 폴더) :";
            // 
            // btnDeleteModel
            // 
            this.btnDeleteModel.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDeleteModel.Location = new System.Drawing.Point(395, 496);
            this.btnDeleteModel.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeleteModel.Name = "btnDeleteModel";
            this.btnDeleteModel.Size = new System.Drawing.Size(120, 40);
            this.btnDeleteModel.TabIndex = 37;
            this.btnDeleteModel.Text = "모델 삭제";
            this.btnDeleteModel.UseVisualStyleBackColor = true;
            this.btnDeleteModel.Click += new System.EventHandler(this.btnDeleteModel_Click);
            // 
            // btnDeleteFolder
            // 
            this.btnDeleteFolder.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDeleteFolder.Location = new System.Drawing.Point(13, 448);
            this.btnDeleteFolder.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeleteFolder.Name = "btnDeleteFolder";
            this.btnDeleteFolder.Size = new System.Drawing.Size(262, 40);
            this.btnDeleteFolder.TabIndex = 38;
            this.btnDeleteFolder.Text = "모델 삭제";
            this.btnDeleteFolder.UseVisualStyleBackColor = true;
            this.btnDeleteFolder.Click += new System.EventHandler(this.btnDeleteFolder_Click);
            // 
            // btnDownloadDXF
            // 
            this.btnDownloadDXF.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDownloadDXF.Location = new System.Drawing.Point(13, 496);
            this.btnDownloadDXF.Margin = new System.Windows.Forms.Padding(4);
            this.btnDownloadDXF.Name = "btnDownloadDXF";
            this.btnDownloadDXF.Size = new System.Drawing.Size(262, 40);
            this.btnDownloadDXF.TabIndex = 39;
            this.btnDownloadDXF.Text = "DXF 다운로드(J => C)";
            this.btnDownloadDXF.UseVisualStyleBackColor = true;
            this.btnDownloadDXF.Click += new System.EventHandler(this.btnDownloadDXF_Click);
            // 
            // btnUploadDXF
            // 
            this.btnUploadDXF.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUploadDXF.Location = new System.Drawing.Point(13, 544);
            this.btnUploadDXF.Margin = new System.Windows.Forms.Padding(4);
            this.btnUploadDXF.Name = "btnUploadDXF";
            this.btnUploadDXF.Size = new System.Drawing.Size(262, 40);
            this.btnUploadDXF.TabIndex = 40;
            this.btnUploadDXF.Text = "DXF 업로드(I => J)";
            this.btnUploadDXF.UseVisualStyleBackColor = true;
            this.btnUploadDXF.Click += new System.EventHandler(this.btnUploadDXF_Click);
            // 
            // frmModelList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 602);
            this.Controls.Add(this.btnUploadDXF);
            this.Controls.Add(this.btnDownloadDXF);
            this.Controls.Add(this.btnDeleteFolder);
            this.Controls.Add(this.btnDeleteModel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.JDirList);
            this.Controls.Add(this.btnSharedFolder);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.btnModelCopy);
            this.Controls.Add(this.lvModelList);
            this.Controls.Add(this.btnEditModel);
            this.Controls.Add(this.btnNewModel);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSelectModel);
            this.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmModelList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "모델 목록";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmModelList_FormClosing);
            this.Load += new System.EventHandler(this.frmModelList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEditModel;
        private System.Windows.Forms.Button btnNewModel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSelectModel;
        private System.Windows.Forms.ListView lvModelList;
        private System.Windows.Forms.Button btnModelCopy;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnSharedFolder;
        private System.Windows.Forms.ListView JDirList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDeleteModel;
        private System.Windows.Forms.Button btnDeleteFolder;
        private System.Windows.Forms.Button btnDownloadDXF;
        private System.Windows.Forms.Button btnUploadDXF;
    }
}