namespace DrawToText
{
    partial class form_Main
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox_draw = new System.Windows.Forms.PictureBox();
            this.timer_draw = new System.Windows.Forms.Timer(this.components);
            this.pictureBox0 = new System.Windows.Forms.PictureBox();
            this.label_draworder = new System.Windows.Forms.Label();
            this.timer_beep = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_draw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox0)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_draw
            // 
            this.pictureBox_draw.Location = new System.Drawing.Point(12, 12);
            this.pictureBox_draw.Name = "pictureBox_draw";
            this.pictureBox_draw.Size = new System.Drawing.Size(250, 250);
            this.pictureBox_draw.TabIndex = 0;
            this.pictureBox_draw.TabStop = false;
            this.pictureBox_draw.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox_draw.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // timer_draw
            // 
            this.timer_draw.Interval = 1;
            this.timer_draw.Tick += new System.EventHandler(this.timer_draw_Tick);
            // 
            // pictureBox0
            // 
            this.pictureBox0.Location = new System.Drawing.Point(268, 12);
            this.pictureBox0.Name = "pictureBox0";
            this.pictureBox0.Size = new System.Drawing.Size(28, 28);
            this.pictureBox0.TabIndex = 1;
            this.pictureBox0.TabStop = false;
            // 
            // label_draworder
            // 
            this.label_draworder.AutoSize = true;
            this.label_draworder.Location = new System.Drawing.Point(268, 65);
            this.label_draworder.Name = "label_draworder";
            this.label_draworder.Size = new System.Drawing.Size(38, 12);
            this.label_draworder.TabIndex = 2;
            this.label_draworder.Text = "label1";
            // 
            // timer_beep
            // 
            this.timer_beep.Interval = 1;
            this.timer_beep.Tick += new System.EventHandler(this.timer_beep_Tick);
            // 
            // form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 273);
            this.Controls.Add(this.label_draworder);
            this.Controls.Add(this.pictureBox0);
            this.Controls.Add(this.pictureBox_draw);
            this.Name = "form_Main";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.form_Main_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.form_Main_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_draw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox0)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_draw;
        private System.Windows.Forms.Timer timer_draw;
        private System.Windows.Forms.PictureBox pictureBox0;
        private System.Windows.Forms.Label label_draworder;
        private System.Windows.Forms.Timer timer_beep;
    }
}

