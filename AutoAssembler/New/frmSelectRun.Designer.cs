namespace AutoAssembler
{
    partial class frmSelectRun
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
            this.components = new System.ComponentModel.Container();
            this.btnRunOne = new System.Windows.Forms.Button();
            this.btnRunContinue = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.timerAlarm = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btnRunOne
            // 
            this.btnRunOne.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnRunOne.Location = new System.Drawing.Point(12, 12);
            this.btnRunOne.Name = "btnRunOne";
            this.btnRunOne.Size = new System.Drawing.Size(440, 60);
            this.btnRunOne.TabIndex = 0;
            this.btnRunOne.Text = "선택한 단일 항목 실행하기";
            this.btnRunOne.UseVisualStyleBackColor = true;
            this.btnRunOne.Click += new System.EventHandler(this.btnRunOne_Click);
            // 
            // btnRunContinue
            // 
            this.btnRunContinue.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.btnRunContinue.Location = new System.Drawing.Point(12, 78);
            this.btnRunContinue.Name = "btnRunContinue";
            this.btnRunContinue.Size = new System.Drawing.Size(440, 60);
            this.btnRunContinue.TabIndex = 1;
            this.btnRunContinue.Text = "현재 위치부터 실행하기";
            this.btnRunContinue.UseVisualStyleBackColor = true;
            this.btnRunContinue.Click += new System.EventHandler(this.btnRunContinue_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(12, 144);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(440, 60);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // timerAlarm
            // 
            this.timerAlarm.Interval = 500;
            this.timerAlarm.Tick += new System.EventHandler(this.timerAlarm_Tick);
            // 
            // frmSelectRun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 212);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRunContinue);
            this.Controls.Add(this.btnRunOne);
            this.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmSelectRun";
            this.Text = "실행 방법 지정 ...";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRunOne;
        private System.Windows.Forms.Button btnRunContinue;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Timer timerAlarm;
    }
}