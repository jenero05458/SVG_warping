namespace SVG_Morph
{
    partial class ControlLineEdit
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
            this.svg_image = new System.Windows.Forms.PictureBox();
            this.point_list_box = new System.Windows.Forms.ListBox();
            this.MoveLine_btn = new System.Windows.Forms.Button();
            this.SaveLine_btn = new System.Windows.Forms.Button();
            this.Ok_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.FileName_edit = new System.Windows.Forms.TextBox();
            this.cancel_btn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ContrLineVer_edit = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.svg_image)).BeginInit();
            this.SuspendLayout();
            // 
            // svg_image
            // 
            this.svg_image.BackColor = System.Drawing.Color.White;
            this.svg_image.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.svg_image.Location = new System.Drawing.Point(32, 33);
            this.svg_image.Name = "svg_image";
            this.svg_image.Size = new System.Drawing.Size(360, 360);
            this.svg_image.TabIndex = 0;
            this.svg_image.TabStop = false;
            this.svg_image.MouseDown += new System.Windows.Forms.MouseEventHandler(this.svg_image_MouseDown);
            this.svg_image.MouseMove += new System.Windows.Forms.MouseEventHandler(this.svg_image_MouseMove);
            this.svg_image.MouseUp += new System.Windows.Forms.MouseEventHandler(this.svg_image_MouseUp);
            // 
            // point_list_box
            // 
            this.point_list_box.FormattingEnabled = true;
            this.point_list_box.ItemHeight = 12;
            this.point_list_box.Location = new System.Drawing.Point(433, 33);
            this.point_list_box.Name = "point_list_box";
            this.point_list_box.Size = new System.Drawing.Size(158, 232);
            this.point_list_box.TabIndex = 1;
            this.point_list_box.SelectedIndexChanged += new System.EventHandler(this.point_list_box_SelectedIndexChanged);
            // 
            // MoveLine_btn
            // 
            this.MoveLine_btn.Location = new System.Drawing.Point(433, 322);
            this.MoveLine_btn.Name = "MoveLine_btn";
            this.MoveLine_btn.Size = new System.Drawing.Size(76, 40);
            this.MoveLine_btn.TabIndex = 2;
            this.MoveLine_btn.Text = "Move";
            this.MoveLine_btn.UseVisualStyleBackColor = true;
            this.MoveLine_btn.Click += new System.EventHandler(this.MoveLine_btn_Click);
            // 
            // SaveLine_btn
            // 
            this.SaveLine_btn.Location = new System.Drawing.Point(515, 322);
            this.SaveLine_btn.Name = "SaveLine_btn";
            this.SaveLine_btn.Size = new System.Drawing.Size(76, 40);
            this.SaveLine_btn.TabIndex = 3;
            this.SaveLine_btn.Text = "File save";
            this.SaveLine_btn.UseVisualStyleBackColor = true;
            this.SaveLine_btn.Click += new System.EventHandler(this.SaveLine_btn_Click);
            // 
            // Ok_btn
            // 
            this.Ok_btn.Location = new System.Drawing.Point(515, 368);
            this.Ok_btn.Name = "Ok_btn";
            this.Ok_btn.Size = new System.Drawing.Size(76, 40);
            this.Ok_btn.TabIndex = 4;
            this.Ok_btn.Text = "Save and\r\nClose\r\n";
            this.Ok_btn.UseVisualStyleBackColor = true;
            this.Ok_btn.Click += new System.EventHandler(this.Ok_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(434, 278);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "Name :";
            // 
            // FileName_edit
            // 
            this.FileName_edit.Location = new System.Drawing.Point(484, 274);
            this.FileName_edit.Name = "FileName_edit";
            this.FileName_edit.Size = new System.Drawing.Size(107, 21);
            this.FileName_edit.TabIndex = 6;
            // 
            // cancel_btn
            // 
            this.cancel_btn.Location = new System.Drawing.Point(433, 368);
            this.cancel_btn.Name = "cancel_btn";
            this.cancel_btn.Size = new System.Drawing.Size(76, 40);
            this.cancel_btn.TabIndex = 7;
            this.cancel_btn.Text = "Cancel";
            this.cancel_btn.UseVisualStyleBackColor = true;
            this.cancel_btn.Click += new System.EventHandler(this.cancel_btn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(432, 302);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "Control-line Ver :";
            // 
            // ContrLineVer_edit
            // 
            this.ContrLineVer_edit.Location = new System.Drawing.Point(539, 298);
            this.ContrLineVer_edit.Name = "ContrLineVer_edit";
            this.ContrLineVer_edit.Size = new System.Drawing.Size(50, 21);
            this.ContrLineVer_edit.TabIndex = 9;
            // 
            // ControlLineEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 428);
            this.Controls.Add(this.ContrLineVer_edit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cancel_btn);
            this.Controls.Add(this.FileName_edit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Ok_btn);
            this.Controls.Add(this.SaveLine_btn);
            this.Controls.Add(this.MoveLine_btn);
            this.Controls.Add(this.point_list_box);
            this.Controls.Add(this.svg_image);
            this.Name = "ControlLineEdit";
            this.Text = "ControlLineEdit";
            ((System.ComponentModel.ISupportInitialize)(this.svg_image)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox svg_image;
        private System.Windows.Forms.ListBox point_list_box;
        private System.Windows.Forms.Button MoveLine_btn;
        private System.Windows.Forms.Button SaveLine_btn;
        private System.Windows.Forms.Button Ok_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FileName_edit;
        private System.Windows.Forms.Button cancel_btn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ContrLineVer_edit;
    }
}