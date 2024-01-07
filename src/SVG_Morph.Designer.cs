namespace SVG_Morph
{
    partial class SVG_Morph
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SVG_Morph));
            this.image_animal_svg = new System.Windows.Forms.ToolStrip();
            this.face_direction = new System.Windows.Forms.ToolStripComboBox();
            this.left_shift_Button = new System.Windows.Forms.ToolStripButton();
            this.right_shift_Button = new System.Windows.Forms.ToolStripButton();
            this.input_svg_open_btn = new System.Windows.Forms.Button();
            this.warphing_btn = new System.Windows.Forms.Button();
            this.morphing_btn = new System.Windows.Forms.Button();
            this.input_label = new System.Windows.Forms.Label();
            this.target_label = new System.Windows.Forms.Label();
            this.morph_label = new System.Windows.Forms.Label();
            this.morph_svg = new System.Windows.Forms.PictureBox();
            this.target_svg = new System.Windows.Forms.PictureBox();
            this.input_svg = new System.Windows.Forms.PictureBox();
            this.panel_morph_01 = new System.Windows.Forms.PictureBox();
            this.panel_morph_02 = new System.Windows.Forms.PictureBox();
            this.panel_morph_03 = new System.Windows.Forms.PictureBox();
            this.panel_morph_04 = new System.Windows.Forms.PictureBox();
            this.panel_morph_05 = new System.Windows.Forms.PictureBox();
            this.panel_morph_06 = new System.Windows.Forms.PictureBox();
            this.panel_morph_07 = new System.Windows.Forms.PictureBox();
            this.panel_morph_08 = new System.Windows.Forms.PictureBox();
            this.panel_morph_09 = new System.Windows.Forms.PictureBox();
            this.panel_morph_10 = new System.Windows.Forms.PictureBox();
            this.input_control_line_display = new System.Windows.Forms.CheckBox();
            this.target_control_line_display = new System.Windows.Forms.CheckBox();
            this.input_image_move_btn = new System.Windows.Forms.Button();
            this.target_image_move_btn = new System.Windows.Forms.Button();
            this.InputControlLineEdit_btn = new System.Windows.Forms.Button();
            this.morphing_result_save_btn = new System.Windows.Forms.Button();
            this.a_value_label = new System.Windows.Forms.Label();
            this.b_value_label = new System.Windows.Forms.Label();
            this.p_value_label = new System.Windows.Forms.Label();
            this.aValuetextBox = new System.Windows.Forms.TextBox();
            this.bValuetextBox = new System.Windows.Forms.TextBox();
            this.pValuetextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.paramInitBtn = new System.Windows.Forms.Button();
            this.image_animal_svg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.morph_svg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.target_svg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.input_svg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_morph_01)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_morph_02)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_morph_03)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_morph_04)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_morph_05)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_morph_06)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_morph_07)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_morph_08)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_morph_09)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_morph_10)).BeginInit();
            this.SuspendLayout();
            // 
            // image_animal_svg
            // 
            this.image_animal_svg.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.face_direction,
            this.left_shift_Button,
            this.right_shift_Button});
            this.image_animal_svg.Location = new System.Drawing.Point(0, 0);
            this.image_animal_svg.MinimumSize = new System.Drawing.Size(50, 80);
            this.image_animal_svg.Name = "image_animal_svg";
            this.image_animal_svg.Size = new System.Drawing.Size(1241, 80);
            this.image_animal_svg.TabIndex = 0;
            this.image_animal_svg.Text = "animal_svg";
            // 
            // face_direction
            // 
            this.face_direction.Name = "face_direction";
            this.face_direction.Size = new System.Drawing.Size(80, 80);
            this.face_direction.SelectedIndexChanged += new System.EventHandler(this.face_direction_SelectedIndexChanged);
            // 
            // left_shift_Button
            // 
            this.left_shift_Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.left_shift_Button.Image = ((System.Drawing.Image)(resources.GetObject("left_shift_Button.Image")));
            this.left_shift_Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.left_shift_Button.Name = "left_shift_Button";
            this.left_shift_Button.Size = new System.Drawing.Size(23, 77);
            this.left_shift_Button.Text = "<<";
            this.left_shift_Button.ToolTipText = "왼쪽으로 이동";
            this.left_shift_Button.Click += new System.EventHandler(this.left_shift_Button_Click);
            // 
            // right_shift_Button
            // 
            this.right_shift_Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.right_shift_Button.Image = ((System.Drawing.Image)(resources.GetObject("right_shift_Button.Image")));
            this.right_shift_Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.right_shift_Button.Name = "right_shift_Button";
            this.right_shift_Button.Size = new System.Drawing.Size(23, 77);
            this.right_shift_Button.Text = ">>";
            this.right_shift_Button.ToolTipText = "오른쪽으로 이동";
            this.right_shift_Button.Click += new System.EventHandler(this.right_shift_Button_Click);
            // 
            // input_svg_open_btn
            // 
            this.input_svg_open_btn.Location = new System.Drawing.Point(14, 116);
            this.input_svg_open_btn.Name = "input_svg_open_btn";
            this.input_svg_open_btn.Size = new System.Drawing.Size(76, 40);
            this.input_svg_open_btn.TabIndex = 1;
            this.input_svg_open_btn.Text = "Open";
            this.input_svg_open_btn.UseVisualStyleBackColor = true;
            this.input_svg_open_btn.Click += new System.EventHandler(this.input_svg_open_btn_Click);
            // 
            // warphing_btn
            // 
            this.warphing_btn.Location = new System.Drawing.Point(14, 170);
            this.warphing_btn.Name = "warphing_btn";
            this.warphing_btn.Size = new System.Drawing.Size(76, 40);
            this.warphing_btn.TabIndex = 2;
            this.warphing_btn.Text = "Warping";
            this.warphing_btn.UseVisualStyleBackColor = true;
            this.warphing_btn.Click += new System.EventHandler(this.warphing_btn_Click);
            // 
            // morphing_btn
            // 
            this.morphing_btn.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.morphing_btn.Location = new System.Drawing.Point(14, 225);
            this.morphing_btn.Name = "morphing_btn";
            this.morphing_btn.Size = new System.Drawing.Size(76, 40);
            this.morphing_btn.TabIndex = 3;
            this.morphing_btn.Text = "Successiv.\r\nWarping";
            this.morphing_btn.UseVisualStyleBackColor = true;
            this.morphing_btn.Click += new System.EventHandler(this.morphing_btn_Click);
            // 
            // input_label
            // 
            this.input_label.AutoSize = true;
            this.input_label.Location = new System.Drawing.Point(257, 92);
            this.input_label.Name = "input_label";
            this.input_label.Size = new System.Drawing.Size(80, 12);
            this.input_label.TabIndex = 7;
            this.input_label.Text = "Input Window";
            // 
            // target_label
            // 
            this.target_label.AutoSize = true;
            this.target_label.Location = new System.Drawing.Point(627, 92);
            this.target_label.Name = "target_label";
            this.target_label.Size = new System.Drawing.Size(89, 12);
            this.target_label.TabIndex = 8;
            this.target_label.Text = "Target Window";
            // 
            // morph_label
            // 
            this.morph_label.AutoSize = true;
            this.morph_label.Location = new System.Drawing.Point(992, 92);
            this.morph_label.Name = "morph_label";
            this.morph_label.Size = new System.Drawing.Size(88, 12);
            this.morph_label.TabIndex = 9;
            this.morph_label.Text = "Result Window";
            // 
            // morph_svg
            // 
            this.morph_svg.BackColor = System.Drawing.Color.White;
            this.morph_svg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.morph_svg.Location = new System.Drawing.Point(863, 110);
            this.morph_svg.Name = "morph_svg";
            this.morph_svg.Size = new System.Drawing.Size(360, 360);
            this.morph_svg.TabIndex = 6;
            this.morph_svg.TabStop = false;
            // 
            // target_svg
            // 
            this.target_svg.BackColor = System.Drawing.Color.White;
            this.target_svg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.target_svg.Location = new System.Drawing.Point(486, 110);
            this.target_svg.Name = "target_svg";
            this.target_svg.Size = new System.Drawing.Size(360, 360);
            this.target_svg.TabIndex = 5;
            this.target_svg.TabStop = false;
            this.target_svg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.target_svg_MouseDown);
            this.target_svg.MouseMove += new System.Windows.Forms.MouseEventHandler(this.target_svg_MouseMove);
            this.target_svg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.target_svg_MouseUp);
            // 
            // input_svg
            // 
            this.input_svg.BackColor = System.Drawing.Color.White;
            this.input_svg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.input_svg.Location = new System.Drawing.Point(109, 110);
            this.input_svg.Name = "input_svg";
            this.input_svg.Size = new System.Drawing.Size(360, 360);
            this.input_svg.TabIndex = 4;
            this.input_svg.TabStop = false;
            this.input_svg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.input_svg_MouseDown);
            this.input_svg.MouseMove += new System.Windows.Forms.MouseEventHandler(this.input_svg_MouseMove);
            this.input_svg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.input_svg_MouseUp);
            // 
            // panel_morph_01
            // 
            this.panel_morph_01.BackColor = System.Drawing.Color.White;
            this.panel_morph_01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_morph_01.Location = new System.Drawing.Point(28, 563);
            this.panel_morph_01.Name = "panel_morph_01";
            this.panel_morph_01.Size = new System.Drawing.Size(100, 100);
            this.panel_morph_01.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.panel_morph_01.TabIndex = 10;
            this.panel_morph_01.TabStop = false;
            this.panel_morph_01.Click += new System.EventHandler(this.panel_morph_01_Click);
            // 
            // panel_morph_02
            // 
            this.panel_morph_02.BackColor = System.Drawing.Color.White;
            this.panel_morph_02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_morph_02.Location = new System.Drawing.Point(147, 563);
            this.panel_morph_02.Name = "panel_morph_02";
            this.panel_morph_02.Size = new System.Drawing.Size(100, 100);
            this.panel_morph_02.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.panel_morph_02.TabIndex = 11;
            this.panel_morph_02.TabStop = false;
            this.panel_morph_02.Click += new System.EventHandler(this.panel_morph_02_Click);
            // 
            // panel_morph_03
            // 
            this.panel_morph_03.BackColor = System.Drawing.Color.White;
            this.panel_morph_03.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_morph_03.Location = new System.Drawing.Point(267, 563);
            this.panel_morph_03.Name = "panel_morph_03";
            this.panel_morph_03.Size = new System.Drawing.Size(100, 100);
            this.panel_morph_03.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.panel_morph_03.TabIndex = 12;
            this.panel_morph_03.TabStop = false;
            this.panel_morph_03.Click += new System.EventHandler(this.panel_morph_03_Click);
            // 
            // panel_morph_04
            // 
            this.panel_morph_04.BackColor = System.Drawing.Color.White;
            this.panel_morph_04.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_morph_04.Location = new System.Drawing.Point(388, 563);
            this.panel_morph_04.Name = "panel_morph_04";
            this.panel_morph_04.Size = new System.Drawing.Size(100, 100);
            this.panel_morph_04.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.panel_morph_04.TabIndex = 13;
            this.panel_morph_04.TabStop = false;
            this.panel_morph_04.Click += new System.EventHandler(this.panel_morph_04_Click);
            // 
            // panel_morph_05
            // 
            this.panel_morph_05.BackColor = System.Drawing.Color.White;
            this.panel_morph_05.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_morph_05.Location = new System.Drawing.Point(509, 563);
            this.panel_morph_05.Name = "panel_morph_05";
            this.panel_morph_05.Size = new System.Drawing.Size(100, 100);
            this.panel_morph_05.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.panel_morph_05.TabIndex = 14;
            this.panel_morph_05.TabStop = false;
            this.panel_morph_05.Click += new System.EventHandler(this.panel_morph_05_Click);
            // 
            // panel_morph_06
            // 
            this.panel_morph_06.BackColor = System.Drawing.Color.White;
            this.panel_morph_06.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_morph_06.Location = new System.Drawing.Point(630, 563);
            this.panel_morph_06.Name = "panel_morph_06";
            this.panel_morph_06.Size = new System.Drawing.Size(100, 100);
            this.panel_morph_06.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.panel_morph_06.TabIndex = 15;
            this.panel_morph_06.TabStop = false;
            this.panel_morph_06.Click += new System.EventHandler(this.panel_morph_06_Click);
            // 
            // panel_morph_07
            // 
            this.panel_morph_07.BackColor = System.Drawing.Color.White;
            this.panel_morph_07.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_morph_07.Location = new System.Drawing.Point(750, 563);
            this.panel_morph_07.Name = "panel_morph_07";
            this.panel_morph_07.Size = new System.Drawing.Size(100, 100);
            this.panel_morph_07.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.panel_morph_07.TabIndex = 16;
            this.panel_morph_07.TabStop = false;
            this.panel_morph_07.Click += new System.EventHandler(this.panel_morph_07_Click);
            // 
            // panel_morph_08
            // 
            this.panel_morph_08.BackColor = System.Drawing.Color.White;
            this.panel_morph_08.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_morph_08.Location = new System.Drawing.Point(871, 563);
            this.panel_morph_08.Name = "panel_morph_08";
            this.panel_morph_08.Size = new System.Drawing.Size(100, 100);
            this.panel_morph_08.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.panel_morph_08.TabIndex = 17;
            this.panel_morph_08.TabStop = false;
            this.panel_morph_08.Click += new System.EventHandler(this.panel_morph_08_Click);
            // 
            // panel_morph_09
            // 
            this.panel_morph_09.BackColor = System.Drawing.Color.White;
            this.panel_morph_09.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_morph_09.Location = new System.Drawing.Point(993, 563);
            this.panel_morph_09.Name = "panel_morph_09";
            this.panel_morph_09.Size = new System.Drawing.Size(100, 100);
            this.panel_morph_09.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.panel_morph_09.TabIndex = 18;
            this.panel_morph_09.TabStop = false;
            this.panel_morph_09.Click += new System.EventHandler(this.panel_morph_09_Click);
            // 
            // panel_morph_10
            // 
            this.panel_morph_10.BackColor = System.Drawing.Color.White;
            this.panel_morph_10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_morph_10.Location = new System.Drawing.Point(1115, 563);
            this.panel_morph_10.Name = "panel_morph_10";
            this.panel_morph_10.Size = new System.Drawing.Size(100, 100);
            this.panel_morph_10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.panel_morph_10.TabIndex = 19;
            this.panel_morph_10.TabStop = false;
            this.panel_morph_10.Click += new System.EventHandler(this.panel_morph_10_Click);
            // 
            // input_control_line_display
            // 
            this.input_control_line_display.AutoSize = true;
            this.input_control_line_display.Location = new System.Drawing.Point(314, 489);
            this.input_control_line_display.Name = "input_control_line_display";
            this.input_control_line_display.Size = new System.Drawing.Size(136, 16);
            this.input_control_line_display.TabIndex = 20;
            this.input_control_line_display.Text = "Control-line Display";
            this.input_control_line_display.UseVisualStyleBackColor = true;
            this.input_control_line_display.CheckedChanged += new System.EventHandler(this.input_control_line_display_CheckedChanged);
            // 
            // target_control_line_display
            // 
            this.target_control_line_display.AutoSize = true;
            this.target_control_line_display.Location = new System.Drawing.Point(594, 489);
            this.target_control_line_display.Name = "target_control_line_display";
            this.target_control_line_display.Size = new System.Drawing.Size(136, 16);
            this.target_control_line_display.TabIndex = 21;
            this.target_control_line_display.Text = "Control-line Display";
            this.target_control_line_display.UseVisualStyleBackColor = true;
            this.target_control_line_display.CheckedChanged += new System.EventHandler(this.target_control_line_display_CheckedChanged);
            // 
            // input_image_move_btn
            // 
            this.input_image_move_btn.Location = new System.Drawing.Point(125, 476);
            this.input_image_move_btn.Name = "input_image_move_btn";
            this.input_image_move_btn.Size = new System.Drawing.Size(76, 40);
            this.input_image_move_btn.TabIndex = 22;
            this.input_image_move_btn.Text = "Move";
            this.input_image_move_btn.UseVisualStyleBackColor = true;
            this.input_image_move_btn.Click += new System.EventHandler(this.input_image_move_btn_Click);
            // 
            // target_image_move_btn
            // 
            this.target_image_move_btn.Location = new System.Drawing.Point(497, 476);
            this.target_image_move_btn.Name = "target_image_move_btn";
            this.target_image_move_btn.Size = new System.Drawing.Size(76, 40);
            this.target_image_move_btn.TabIndex = 23;
            this.target_image_move_btn.Text = "Move";
            this.target_image_move_btn.UseVisualStyleBackColor = true;
            this.target_image_move_btn.Click += new System.EventHandler(this.target_image_move_btn_Click);
            // 
            // InputControlLineEdit_btn
            // 
            this.InputControlLineEdit_btn.Location = new System.Drawing.Point(218, 476);
            this.InputControlLineEdit_btn.Name = "InputControlLineEdit_btn";
            this.InputControlLineEdit_btn.Size = new System.Drawing.Size(76, 40);
            this.InputControlLineEdit_btn.TabIndex = 24;
            this.InputControlLineEdit_btn.Text = "Control Line Edit";
            this.InputControlLineEdit_btn.UseVisualStyleBackColor = true;
            this.InputControlLineEdit_btn.Click += new System.EventHandler(this.InputControlLineEdit_btn_Click);
            // 
            // morphing_result_save_btn
            // 
            this.morphing_result_save_btn.Location = new System.Drawing.Point(881, 476);
            this.morphing_result_save_btn.Name = "morphing_result_save_btn";
            this.morphing_result_save_btn.Size = new System.Drawing.Size(76, 40);
            this.morphing_result_save_btn.TabIndex = 25;
            this.morphing_result_save_btn.Text = "Save";
            this.morphing_result_save_btn.UseVisualStyleBackColor = true;
            this.morphing_result_save_btn.Click += new System.EventHandler(this.morphing_result_save_btn_Click);
            // 
            // a_value_label
            // 
            this.a_value_label.AutoSize = true;
            this.a_value_label.Location = new System.Drawing.Point(5, 352);
            this.a_value_label.Name = "a_value_label";
            this.a_value_label.Size = new System.Drawing.Size(52, 12);
            this.a_value_label.TabIndex = 26;
            this.a_value_label.Text = "a Value:";
            // 
            // b_value_label
            // 
            this.b_value_label.AutoSize = true;
            this.b_value_label.Location = new System.Drawing.Point(5, 378);
            this.b_value_label.Name = "b_value_label";
            this.b_value_label.Size = new System.Drawing.Size(52, 12);
            this.b_value_label.TabIndex = 27;
            this.b_value_label.Text = "b Value:";
            // 
            // p_value_label
            // 
            this.p_value_label.AutoSize = true;
            this.p_value_label.Location = new System.Drawing.Point(5, 406);
            this.p_value_label.Name = "p_value_label";
            this.p_value_label.Size = new System.Drawing.Size(52, 12);
            this.p_value_label.TabIndex = 28;
            this.p_value_label.Text = "p Value:";
            // 
            // aValuetextBox
            // 
            this.aValuetextBox.Location = new System.Drawing.Point(57, 347);
            this.aValuetextBox.Name = "aValuetextBox";
            this.aValuetextBox.Size = new System.Drawing.Size(49, 21);
            this.aValuetextBox.TabIndex = 29;
            // 
            // bValuetextBox
            // 
            this.bValuetextBox.Location = new System.Drawing.Point(57, 375);
            this.bValuetextBox.Name = "bValuetextBox";
            this.bValuetextBox.Size = new System.Drawing.Size(49, 21);
            this.bValuetextBox.TabIndex = 30;
            // 
            // pValuetextBox
            // 
            this.pValuetextBox.Location = new System.Drawing.Point(57, 403);
            this.pValuetextBox.Name = "pValuetextBox";
            this.pValuetextBox.Size = new System.Drawing.Size(49, 21);
            this.pValuetextBox.TabIndex = 31;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Silver;
            this.label1.Font = new System.Drawing.Font("굴림", 10F);
            this.label1.Location = new System.Drawing.Point(19, 309);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 28);
            this.label1.TabIndex = 32;
            this.label1.Text = "Warping\r\nParameter";
            // 
            // paramInitBtn
            // 
            this.paramInitBtn.Location = new System.Drawing.Point(16, 431);
            this.paramInitBtn.Name = "paramInitBtn";
            this.paramInitBtn.Size = new System.Drawing.Size(75, 26);
            this.paramInitBtn.TabIndex = 33;
            this.paramInitBtn.Text = "Initial.";
            this.paramInitBtn.UseVisualStyleBackColor = true;
            this.paramInitBtn.Click += new System.EventHandler(this.paramInitBtn_Click);
            // 
            // SVG_Morph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1241, 681);
            this.Controls.Add(this.paramInitBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pValuetextBox);
            this.Controls.Add(this.bValuetextBox);
            this.Controls.Add(this.aValuetextBox);
            this.Controls.Add(this.p_value_label);
            this.Controls.Add(this.b_value_label);
            this.Controls.Add(this.a_value_label);
            this.Controls.Add(this.morphing_result_save_btn);
            this.Controls.Add(this.InputControlLineEdit_btn);
            this.Controls.Add(this.target_image_move_btn);
            this.Controls.Add(this.input_image_move_btn);
            this.Controls.Add(this.target_control_line_display);
            this.Controls.Add(this.input_control_line_display);
            this.Controls.Add(this.panel_morph_10);
            this.Controls.Add(this.panel_morph_09);
            this.Controls.Add(this.panel_morph_08);
            this.Controls.Add(this.panel_morph_07);
            this.Controls.Add(this.panel_morph_06);
            this.Controls.Add(this.panel_morph_05);
            this.Controls.Add(this.panel_morph_04);
            this.Controls.Add(this.panel_morph_03);
            this.Controls.Add(this.panel_morph_02);
            this.Controls.Add(this.panel_morph_01);
            this.Controls.Add(this.morph_label);
            this.Controls.Add(this.target_label);
            this.Controls.Add(this.input_label);
            this.Controls.Add(this.morph_svg);
            this.Controls.Add(this.target_svg);
            this.Controls.Add(this.input_svg);
            this.Controls.Add(this.morphing_btn);
            this.Controls.Add(this.warphing_btn);
            this.Controls.Add(this.input_svg_open_btn);
            this.Controls.Add(this.image_animal_svg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SVG_Morph";
            this.Text = "SVG_Warping";
            this.Load += new System.EventHandler(this.SVG_Morph_Load);
            this.image_animal_svg.ResumeLayout(false);
            this.image_animal_svg.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.morph_svg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.target_svg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.input_svg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_morph_01)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_morph_02)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_morph_03)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_morph_04)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_morph_05)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_morph_06)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_morph_07)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_morph_08)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_morph_09)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_morph_10)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip image_animal_svg;
        private System.Windows.Forms.ToolStripButton left_shift_Button;
        private System.Windows.Forms.ToolStripButton right_shift_Button;
        private System.Windows.Forms.ToolStripComboBox face_direction;
        private System.Windows.Forms.Button input_svg_open_btn;
        private System.Windows.Forms.Button warphing_btn;
        private System.Windows.Forms.Button morphing_btn;
        private System.Windows.Forms.PictureBox input_svg;
        private System.Windows.Forms.PictureBox target_svg;
        private System.Windows.Forms.PictureBox morph_svg;
        private System.Windows.Forms.Label input_label;
        private System.Windows.Forms.Label target_label;
        private System.Windows.Forms.Label morph_label;
        private System.Windows.Forms.PictureBox panel_morph_01;
        private System.Windows.Forms.PictureBox panel_morph_02;
        private System.Windows.Forms.PictureBox panel_morph_03;
        private System.Windows.Forms.PictureBox panel_morph_04;
        private System.Windows.Forms.PictureBox panel_morph_05;
        private System.Windows.Forms.PictureBox panel_morph_06;
        private System.Windows.Forms.PictureBox panel_morph_07;
        private System.Windows.Forms.PictureBox panel_morph_08;
        private System.Windows.Forms.PictureBox panel_morph_09;
        private System.Windows.Forms.PictureBox panel_morph_10;
        private System.Windows.Forms.CheckBox input_control_line_display;
        private System.Windows.Forms.CheckBox target_control_line_display;
        private System.Windows.Forms.Button input_image_move_btn;
        private System.Windows.Forms.Button target_image_move_btn;
        private System.Windows.Forms.Button InputControlLineEdit_btn;
        private System.Windows.Forms.Button morphing_result_save_btn;
        private System.Windows.Forms.Label a_value_label;
        private System.Windows.Forms.Label b_value_label;
        private System.Windows.Forms.Label p_value_label;
        private System.Windows.Forms.TextBox aValuetextBox;
        private System.Windows.Forms.TextBox bValuetextBox;
        private System.Windows.Forms.TextBox pValuetextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button paramInitBtn;

        public System.EventHandler SVG_ { get; set; }
    }
}

