namespace AutoAssembler
{
    partial class frmEditor
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
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lstWorkList = new System.Windows.Forms.ListView();
            this.lstAddList = new System.Windows.Forms.ListView();
            this.btnAddWork = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnCameraSetting = new System.Windows.Forms.Button();
            this.btnLightingSetting = new System.Windows.Forms.Button();
            this.btnMotionSetting = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnDIOSetting = new System.Windows.Forms.Button();
            this.btnRecognizeSet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDown
            // 
            this.btnDown.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDown.Location = new System.Drawing.Point(540, 598);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(95, 40);
            this.btnDown.TabIndex = 60;
            this.btnDown.Text = "아래로";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUp.Location = new System.Drawing.Point(439, 598);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(95, 40);
            this.btnUp.TabIndex = 59;
            this.btnUp.Text = "위로";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCancel.Location = new System.Drawing.Point(757, 644);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 40);
            this.btnCancel.TabIndex = 61;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lstWorkList
            // 
            this.lstWorkList.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lstWorkList.FullRowSelect = true;
            this.lstWorkList.HideSelection = false;
            this.lstWorkList.Location = new System.Drawing.Point(12, 12);
            this.lstWorkList.MultiSelect = false;
            this.lstWorkList.Name = "lstWorkList";
            this.lstWorkList.Size = new System.Drawing.Size(320, 580);
            this.lstWorkList.TabIndex = 62;
            this.lstWorkList.UseCompatibleStateImageBehavior = false;
            this.lstWorkList.View = System.Windows.Forms.View.Details;
            // 
            // lstAddList
            // 
            this.lstAddList.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lstAddList.FullRowSelect = true;
            this.lstAddList.HideSelection = false;
            this.lstAddList.Location = new System.Drawing.Point(439, 12);
            this.lstAddList.MultiSelect = false;
            this.lstAddList.Name = "lstAddList";
            this.lstAddList.Size = new System.Drawing.Size(398, 580);
            this.lstAddList.TabIndex = 63;
            this.lstAddList.UseCompatibleStateImageBehavior = false;
            this.lstAddList.View = System.Windows.Forms.View.Details;
            this.lstAddList.SelectedIndexChanged += new System.EventHandler(this.lstAddList_SelectedIndexChanged);
            this.lstAddList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstAddList_MouseDoubleClick);
            // 
            // btnAddWork
            // 
            this.btnAddWork.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAddWork.Location = new System.Drawing.Point(338, 205);
            this.btnAddWork.Name = "btnAddWork";
            this.btnAddWork.Size = new System.Drawing.Size(95, 95);
            this.btnAddWork.TabIndex = 64;
            this.btnAddWork.Text = "추가";
            this.btnAddWork.UseVisualStyleBackColor = true;
            this.btnAddWork.Click += new System.EventHandler(this.btnAddWork_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDelete.Location = new System.Drawing.Point(641, 598);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(95, 40);
            this.btnDelete.TabIndex = 65;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnEdit.Location = new System.Drawing.Point(742, 598);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(95, 40);
            this.btnEdit.TabIndex = 66;
            this.btnEdit.Text = "편집 ...";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnCameraSetting
            // 
            this.btnCameraSetting.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCameraSetting.Location = new System.Drawing.Point(12, 598);
            this.btnCameraSetting.Name = "btnCameraSetting";
            this.btnCameraSetting.Size = new System.Drawing.Size(156, 40);
            this.btnCameraSetting.TabIndex = 67;
            this.btnCameraSetting.Text = "카메라 설정";
            this.btnCameraSetting.UseVisualStyleBackColor = true;
            this.btnCameraSetting.Click += new System.EventHandler(this.btnCameraSetting_Click);
            // 
            // btnLightingSetting
            // 
            this.btnLightingSetting.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLightingSetting.Location = new System.Drawing.Point(176, 598);
            this.btnLightingSetting.Name = "btnLightingSetting";
            this.btnLightingSetting.Size = new System.Drawing.Size(156, 40);
            this.btnLightingSetting.TabIndex = 68;
            this.btnLightingSetting.Text = "조명 설정";
            this.btnLightingSetting.UseVisualStyleBackColor = true;
            this.btnLightingSetting.Click += new System.EventHandler(this.btnLightingSetting_Click);
            // 
            // btnMotionSetting
            // 
            this.btnMotionSetting.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMotionSetting.Location = new System.Drawing.Point(12, 644);
            this.btnMotionSetting.Name = "btnMotionSetting";
            this.btnMotionSetting.Size = new System.Drawing.Size(156, 40);
            this.btnMotionSetting.TabIndex = 69;
            this.btnMotionSetting.Text = "모션 설정";
            this.btnMotionSetting.UseVisualStyleBackColor = true;
            this.btnMotionSetting.Click += new System.EventHandler(this.btnMotionSetting_Click);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOk.Location = new System.Drawing.Point(671, 644);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 40);
            this.btnOk.TabIndex = 70;
            this.btnOk.Text = "확인";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnDIOSetting
            // 
            this.btnDIOSetting.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDIOSetting.Location = new System.Drawing.Point(176, 644);
            this.btnDIOSetting.Name = "btnDIOSetting";
            this.btnDIOSetting.Size = new System.Drawing.Size(156, 40);
            this.btnDIOSetting.TabIndex = 71;
            this.btnDIOSetting.Text = "DIO 설정";
            this.btnDIOSetting.UseVisualStyleBackColor = true;
            this.btnDIOSetting.Click += new System.EventHandler(this.btnDIOSetting_Click);
            // 
            // btnRecognizeSet
            // 
            this.btnRecognizeSet.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRecognizeSet.Location = new System.Drawing.Point(338, 306);
            this.btnRecognizeSet.Name = "btnRecognizeSet";
            this.btnRecognizeSet.Size = new System.Drawing.Size(95, 95);
            this.btnRecognizeSet.TabIndex = 72;
            this.btnRecognizeSet.Text = "영상 인식 세트 추가";
            this.btnRecognizeSet.UseVisualStyleBackColor = true;
            this.btnRecognizeSet.Click += new System.EventHandler(this.btnRecognizeSet_Click);
            // 
            // frmEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 697);
            this.Controls.Add(this.btnRecognizeSet);
            this.Controls.Add(this.btnDIOSetting);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnMotionSetting);
            this.Controls.Add(this.btnLightingSetting);
            this.Controls.Add(this.btnCameraSetting);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAddWork);
            this.Controls.Add(this.lstAddList);
            this.Controls.Add(this.lstWorkList);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "시나리오 편집기";
            this.Load += new System.EventHandler(this.frmEditor_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListView lstWorkList;
        private System.Windows.Forms.ListView lstAddList;
        private System.Windows.Forms.Button btnAddWork;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnCameraSetting;
        private System.Windows.Forms.Button btnLightingSetting;
        private System.Windows.Forms.Button btnMotionSetting;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnDIOSetting;
        private System.Windows.Forms.Button btnRecognizeSet;
    }
}