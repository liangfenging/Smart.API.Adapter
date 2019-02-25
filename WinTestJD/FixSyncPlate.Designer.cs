namespace WinTestJD
{
    partial class FixSyncPlate
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
            this.btn_FixSyncPlate = new System.Windows.Forms.Button();
            this.btn_SearchInfo = new System.Windows.Forms.Button();
            this.lbl_TotalCount = new System.Windows.Forms.Label();
            this.lbl_WhiteCount = new System.Windows.Forms.Label();
            this.lbl_GrayCount = new System.Windows.Forms.Label();
            this.lbl_DeleteCount = new System.Windows.Forms.Label();
            this.lbl_NoSync = new System.Windows.Forms.Label();
            this.lbl_jielinkCount = new System.Windows.Forms.Label();
            this.btn_Version = new System.Windows.Forms.Button();
            this.txt_Version = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_FixSyncPlate
            // 
            this.btn_FixSyncPlate.Location = new System.Drawing.Point(225, 52);
            this.btn_FixSyncPlate.Name = "btn_FixSyncPlate";
            this.btn_FixSyncPlate.Size = new System.Drawing.Size(93, 34);
            this.btn_FixSyncPlate.TabIndex = 0;
            this.btn_FixSyncPlate.Text = "修复同步车牌";
            this.btn_FixSyncPlate.UseVisualStyleBackColor = true;
            this.btn_FixSyncPlate.Click += new System.EventHandler(this.button_SyncFix_Click);
            // 
            // btn_SearchInfo
            // 
            this.btn_SearchInfo.Location = new System.Drawing.Point(33, 52);
            this.btn_SearchInfo.Name = "btn_SearchInfo";
            this.btn_SearchInfo.Size = new System.Drawing.Size(93, 34);
            this.btn_SearchInfo.TabIndex = 0;
            this.btn_SearchInfo.Text = "信息查询";
            this.btn_SearchInfo.UseVisualStyleBackColor = true;
            this.btn_SearchInfo.Click += new System.EventHandler(this.btn_SearchInfo_Click);
            // 
            // lbl_TotalCount
            // 
            this.lbl_TotalCount.AutoSize = true;
            this.lbl_TotalCount.Location = new System.Drawing.Point(33, 123);
            this.lbl_TotalCount.Name = "lbl_TotalCount";
            this.lbl_TotalCount.Size = new System.Drawing.Size(59, 12);
            this.lbl_TotalCount.TabIndex = 1;
            this.lbl_TotalCount.Text = "总用户数:";
            // 
            // lbl_WhiteCount
            // 
            this.lbl_WhiteCount.AutoSize = true;
            this.lbl_WhiteCount.Location = new System.Drawing.Point(31, 169);
            this.lbl_WhiteCount.Name = "lbl_WhiteCount";
            this.lbl_WhiteCount.Size = new System.Drawing.Size(59, 12);
            this.lbl_WhiteCount.TabIndex = 1;
            this.lbl_WhiteCount.Text = "合法车数:";
            // 
            // lbl_GrayCount
            // 
            this.lbl_GrayCount.AutoSize = true;
            this.lbl_GrayCount.Location = new System.Drawing.Point(31, 211);
            this.lbl_GrayCount.Name = "lbl_GrayCount";
            this.lbl_GrayCount.Size = new System.Drawing.Size(59, 12);
            this.lbl_GrayCount.TabIndex = 1;
            this.lbl_GrayCount.Text = "非法车数:";
            // 
            // lbl_DeleteCount
            // 
            this.lbl_DeleteCount.AutoSize = true;
            this.lbl_DeleteCount.Location = new System.Drawing.Point(31, 256);
            this.lbl_DeleteCount.Name = "lbl_DeleteCount";
            this.lbl_DeleteCount.Size = new System.Drawing.Size(251, 12);
            this.lbl_DeleteCount.TabIndex = 1;
            this.lbl_DeleteCount.Text = "异常需注销数量（同步表没有，jielink有）：";
            // 
            // lbl_NoSync
            // 
            this.lbl_NoSync.AutoSize = true;
            this.lbl_NoSync.Location = new System.Drawing.Point(31, 284);
            this.lbl_NoSync.Name = "lbl_NoSync";
            this.lbl_NoSync.Size = new System.Drawing.Size(239, 12);
            this.lbl_NoSync.TabIndex = 1;
            this.lbl_NoSync.Text = "异常未同步数量(同步表有，jielink没有)：";
            // 
            // lbl_jielinkCount
            // 
            this.lbl_jielinkCount.AutoSize = true;
            this.lbl_jielinkCount.Location = new System.Drawing.Point(33, 314);
            this.lbl_jielinkCount.Name = "lbl_jielinkCount";
            this.lbl_jielinkCount.Size = new System.Drawing.Size(77, 12);
            this.lbl_jielinkCount.TabIndex = 2;
            this.lbl_jielinkCount.Text = "jielink总数:";
            // 
            // btn_Version
            // 
            this.btn_Version.Location = new System.Drawing.Point(33, 13);
            this.btn_Version.Name = "btn_Version";
            this.btn_Version.Size = new System.Drawing.Size(75, 23);
            this.btn_Version.TabIndex = 3;
            this.btn_Version.Text = "JD版本查询";
            this.btn_Version.UseVisualStyleBackColor = true;
            this.btn_Version.Click += new System.EventHandler(this.btn_Version_Click);
            // 
            // txt_Version
            // 
            this.txt_Version.Location = new System.Drawing.Point(127, 13);
            this.txt_Version.Name = "txt_Version";
            this.txt_Version.Size = new System.Drawing.Size(100, 21);
            this.txt_Version.TabIndex = 4;
            // 
            // FixSyncPlate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 335);
            this.Controls.Add(this.txt_Version);
            this.Controls.Add(this.btn_Version);
            this.Controls.Add(this.lbl_jielinkCount);
            this.Controls.Add(this.lbl_NoSync);
            this.Controls.Add(this.lbl_DeleteCount);
            this.Controls.Add(this.lbl_GrayCount);
            this.Controls.Add(this.lbl_WhiteCount);
            this.Controls.Add(this.lbl_TotalCount);
            this.Controls.Add(this.btn_SearchInfo);
            this.Controls.Add(this.btn_FixSyncPlate);
            this.Name = "FixSyncPlate";
            this.Text = "FixSyncPlate";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_FixSyncPlate;
        private System.Windows.Forms.Button btn_SearchInfo;
        private System.Windows.Forms.Label lbl_TotalCount;
        private System.Windows.Forms.Label lbl_WhiteCount;
        private System.Windows.Forms.Label lbl_GrayCount;
        private System.Windows.Forms.Label lbl_DeleteCount;
        private System.Windows.Forms.Label lbl_NoSync;
        private System.Windows.Forms.Label lbl_jielinkCount;
        private System.Windows.Forms.Button btn_Version;
        private System.Windows.Forms.TextBox txt_Version;
    }
}