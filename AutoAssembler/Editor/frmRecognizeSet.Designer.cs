namespace AutoAssembler
{
    partial class frmRecognizeSet
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
            this.btnFixBefore_MoveBefore = new System.Windows.Forms.Button();
            this.btnFixBefore_MoveAfter = new System.Windows.Forms.Button();
            this.btnFixBefore_MoveTop = new System.Windows.Forms.Button();
            this.btnFixAfter_MoveBefoe = new System.Windows.Forms.Button();
            this.btnFixAfter_MoveAfter = new System.Windows.Forms.Button();
            this.btnFixAfter_MoveTop = new System.Windows.Forms.Button();
            this.cboUseVBlockZ = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnFixBefore_MoveBefore
            // 
            this.btnFixBefore_MoveBefore.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnFixBefore_MoveBefore.Location = new System.Drawing.Point(12, 12);
            this.btnFixBefore_MoveBefore.Name = "btnFixBefore_MoveBefore";
            this.btnFixBefore_MoveBefore.Size = new System.Drawing.Size(200, 200);
            this.btnFixBefore_MoveBefore.TabIndex = 0;
            this.btnFixBefore_MoveBefore.Text = "고정축 전방, 이동축 전방";
            this.btnFixBefore_MoveBefore.UseVisualStyleBackColor = true;
            this.btnFixBefore_MoveBefore.Click += new System.EventHandler(this.btnFixBefore_MoveBefore_Click);
            // 
            // btnFixBefore_MoveAfter
            // 
            this.btnFixBefore_MoveAfter.Location = new System.Drawing.Point(218, 12);
            this.btnFixBefore_MoveAfter.Name = "btnFixBefore_MoveAfter";
            this.btnFixBefore_MoveAfter.Size = new System.Drawing.Size(200, 200);
            this.btnFixBefore_MoveAfter.TabIndex = 1;
            this.btnFixBefore_MoveAfter.Text = "고정축 전방, 이동축 후방";
            this.btnFixBefore_MoveAfter.UseVisualStyleBackColor = true;
            this.btnFixBefore_MoveAfter.Click += new System.EventHandler(this.btnFixBefore_MoveAfter_Click);
            // 
            // btnFixBefore_MoveTop
            // 
            this.btnFixBefore_MoveTop.Location = new System.Drawing.Point(424, 12);
            this.btnFixBefore_MoveTop.Name = "btnFixBefore_MoveTop";
            this.btnFixBefore_MoveTop.Size = new System.Drawing.Size(200, 200);
            this.btnFixBefore_MoveTop.TabIndex = 2;
            this.btnFixBefore_MoveTop.Text = "고정축 전방, 이동축 상방";
            this.btnFixBefore_MoveTop.UseVisualStyleBackColor = true;
            this.btnFixBefore_MoveTop.Click += new System.EventHandler(this.btnFixBefore_MoveTop_Click);
            // 
            // btnFixAfter_MoveBefoe
            // 
            this.btnFixAfter_MoveBefoe.Location = new System.Drawing.Point(12, 218);
            this.btnFixAfter_MoveBefoe.Name = "btnFixAfter_MoveBefoe";
            this.btnFixAfter_MoveBefoe.Size = new System.Drawing.Size(200, 200);
            this.btnFixAfter_MoveBefoe.TabIndex = 3;
            this.btnFixAfter_MoveBefoe.Text = "고정축 후방, 이동축 전방";
            this.btnFixAfter_MoveBefoe.UseVisualStyleBackColor = true;
            this.btnFixAfter_MoveBefoe.Click += new System.EventHandler(this.btnFixAfter_MoveBefoe_Click);
            // 
            // btnFixAfter_MoveAfter
            // 
            this.btnFixAfter_MoveAfter.Location = new System.Drawing.Point(218, 218);
            this.btnFixAfter_MoveAfter.Name = "btnFixAfter_MoveAfter";
            this.btnFixAfter_MoveAfter.Size = new System.Drawing.Size(200, 200);
            this.btnFixAfter_MoveAfter.TabIndex = 4;
            this.btnFixAfter_MoveAfter.Text = "고정축 후방, 이동축 후방";
            this.btnFixAfter_MoveAfter.UseVisualStyleBackColor = true;
            this.btnFixAfter_MoveAfter.Click += new System.EventHandler(this.btnFixAfter_MoveAfter_Click);
            // 
            // btnFixAfter_MoveTop
            // 
            this.btnFixAfter_MoveTop.Location = new System.Drawing.Point(424, 218);
            this.btnFixAfter_MoveTop.Name = "btnFixAfter_MoveTop";
            this.btnFixAfter_MoveTop.Size = new System.Drawing.Size(200, 200);
            this.btnFixAfter_MoveTop.TabIndex = 5;
            this.btnFixAfter_MoveTop.Text = "고정축 후방, 이동축 상방";
            this.btnFixAfter_MoveTop.UseVisualStyleBackColor = true;
            this.btnFixAfter_MoveTop.Click += new System.EventHandler(this.btnFixAfter_MoveTop_Click);
            // 
            // cboUseVBlockZ
            // 
            this.cboUseVBlockZ.FormattingEnabled = true;
            this.cboUseVBlockZ.Items.AddRange(new object[] {
            "아니오",
            "예"});
            this.cboUseVBlockZ.Location = new System.Drawing.Point(218, 424);
            this.cboUseVBlockZ.Name = "cboUseVBlockZ";
            this.cboUseVBlockZ.Size = new System.Drawing.Size(200, 24);
            this.cboUseVBlockZ.TabIndex = 6;
            this.cboUseVBlockZ.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 427);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "V 블럭 이동 선택 :";
            this.label1.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(507, 454);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 40);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAdd.Location = new System.Drawing.Point(381, 454);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(120, 40);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = "추가";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // frmRecognizeSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 506);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboUseVBlockZ);
            this.Controls.Add(this.btnFixAfter_MoveTop);
            this.Controls.Add(this.btnFixAfter_MoveAfter);
            this.Controls.Add(this.btnFixAfter_MoveBefoe);
            this.Controls.Add(this.btnFixBefore_MoveTop);
            this.Controls.Add(this.btnFixBefore_MoveAfter);
            this.Controls.Add(this.btnFixBefore_MoveBefore);
            this.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmRecognizeSet";
            this.Text = "영상 인식 세트 추기";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFixBefore_MoveBefore;
        private System.Windows.Forms.Button btnFixBefore_MoveAfter;
        private System.Windows.Forms.Button btnFixBefore_MoveTop;
        private System.Windows.Forms.Button btnFixAfter_MoveBefoe;
        private System.Windows.Forms.Button btnFixAfter_MoveAfter;
        private System.Windows.Forms.Button btnFixAfter_MoveTop;
        private System.Windows.Forms.ComboBox cboUseVBlockZ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAdd;
    }
}