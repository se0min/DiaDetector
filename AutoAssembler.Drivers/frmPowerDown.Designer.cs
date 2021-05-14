namespace AutoAssembler.Drivers
{
    partial class frmPowerDown
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
            this.btnStopAll = new System.Windows.Forms.Button();
            this.btnSystemDown = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStopAll
            // 
            this.btnStopAll.Location = new System.Drawing.Point(14, 14);
            this.btnStopAll.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnStopAll.Name = "btnStopAll";
            this.btnStopAll.Size = new System.Drawing.Size(240, 240);
            this.btnStopAll.TabIndex = 0;
            this.btnStopAll.Text = "비상 정지";
            this.btnStopAll.UseVisualStyleBackColor = true;
            this.btnStopAll.Click += new System.EventHandler(this.btnStopAll_Click);
            // 
            // btnSystemDown
            // 
            this.btnSystemDown.Location = new System.Drawing.Point(264, 14);
            this.btnSystemDown.Margin = new System.Windows.Forms.Padding(5);
            this.btnSystemDown.Name = "btnSystemDown";
            this.btnSystemDown.Size = new System.Drawing.Size(240, 240);
            this.btnSystemDown.TabIndex = 1;
            this.btnSystemDown.Text = "시스템 다운";
            this.btnSystemDown.UseVisualStyleBackColor = true;
            this.btnSystemDown.Click += new System.EventHandler(this.btnSystemDown_Click);
            // 
            // frmPowerDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 266);
            this.Controls.Add(this.btnSystemDown);
            this.Controls.Add(this.btnStopAll);
            this.Font = new System.Drawing.Font("돋움", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "frmPowerDown";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "전원 / 원점 복귀 비상";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmPowerDown_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStopAll;
        private System.Windows.Forms.Button btnSystemDown;
    }
}