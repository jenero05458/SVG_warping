using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Threading;
using Svg;
using System.Text.RegularExpressions;



public struct point_i
{
    public int x, y;
    public point_i(int x1, int y1)
    {
        x = x1;
        y = y1;
    }
}

public struct point_f
{
    public float x, y;
    public point_f(float x1, float y1)
    {
        x = x1;
        y = y1;
    }
}

public struct control_point
{
    public int num;
    public int x, y;
    public string parts_name;
    public control_point(int num1, int x1, int y1, string parts_name1)
    {
        num = num1;
        x = x1;
        y = y1;
        parts_name = parts_name1;
    }
}

public struct control_line
{
    public int num;
    public int Px, Py, Qx, Qy;
    public string parts_name;

    public control_line(int num1, int p1, int p2, int p3, int p4, string parts_name1)
    {
        num = num1;
        Px = p1;
        Py = p2;
        Qx = p3;
        Qy = p4;
        parts_name = parts_name1;
    }
}


namespace SVG_Morph
{
    public partial class SVG_Morph : Form
    {
        public XmlDocument main_xml = new XmlDocument();
        public SvgDocument main_svgdoc;
        public XmlDocument target_xml = new XmlDocument();
        public SvgDocument target_svgdoc;

        public XmlDocument main_xml_trans = new XmlDocument();
        public SvgDocument main_svgdoc_trans;

        string[] FaceDirectionDisplayText = { "Front", "Side" };
        string[] FaceDirectionDisplayPath = { "Front", "Side" };

        public int display_x_size = 360;
        public int display_y_size = 360;

        public int ToolStripImageInsertPosition = 2;
        public int ToolStripImageSelectedIndex = 0;
        public int ToolStripImageDisplayCount = 10;
        public int ToolStripImageVisibleStart = 2;

        public XmlDocument XMLSelectToolImage = new XmlDocument();

        bool InputSVGFileOpenCheck = false;
        bool TargetSVGFileOpenCheck = false;

        public control_line[] default_contr_lines = new control_line[100];
        control_line[] dest_contr_lines = new control_line[100];

        public  control_line[] default_contr_lines_Backup = new control_line[100];

        control_line[,] morphing_contr_lines = new control_line[10, 100];

        public int NumDefaultContrLine = 0;
        public int NumDestContrLine = 0;

        public int InputImage_X_Padding = 0;
        public int InputImage_Y_Padding = 0;
        int TargetImage_X_Padding = 0;
        int TargetImage_Y_Padding = 0;
        int InputImageWidth = 0;
        int InputImageHeight = 0;
        int TargetImageWidth = 0;
        int TargetImageHeight = 0;

        Bitmap gBitmap_input_backup = new Bitmap(360, 360);
        Bitmap gBitmap_target_backup = new Bitmap(360, 360);

        public bool input_move_check = false;
        public bool target_move_check = false;

        private Thread Thread_svg_input;
        private Thread Thread_svg_target;

        public float input_svg_move_x, input_svg_move_y, input_svg_move_base_x, input_svg_move_base_y;
        public float target_svg_move_x, target_svg_move_y, target_svg_move_base_x, target_svg_move_base_y;

        private string command = @"([A-z])";  //command
        private char[] pointSeparator = new char[] { ',' };  //point

        public string global_input_file_path;

        public string global_input_file_name;

        int morphing_step = 10;

        int morphing_image_source_step = 10;

        public int face_direct;

        public SVG_Morph()
        {
            InitializeComponent();

            aValuetextBox.Text = "0.001";
            bValuetextBox.Text = "2.0";
            pValuetextBox.Text = "0.75";
        }

        public void CheckSVGDataFolder()
        {
            string sDirPath_front, sDirPath_side;
            //string sDirPath_r_side, sDirPath_l_side;

            sDirPath_front = Application.StartupPath + "\\" + FaceDirectionDisplayPath[0];
            sDirPath_side = Application.StartupPath + "\\" + FaceDirectionDisplayPath[1];
            //sDirPath_l_side = Application.StartupPath + "\\" + FaceDirectionDisplayPath[2];

            DirectoryInfo di_front = new DirectoryInfo(sDirPath_front);
            DirectoryInfo di_r_face = new DirectoryInfo(sDirPath_side);
            //DirectoryInfo di_l_face = new DirectoryInfo(sDirPath_l_side);

            if (di_front.Exists == false)
            {
                MessageBox.Show("Front 폴더가 없습니다.");
                return;
            }

            if (di_r_face.Exists == false)
            {
                MessageBox.Show("Side 폴더가 없습니다.");
                return;
            }

            input_svg_open_btn.Enabled = true;

        }

        public void face_direction_set()
        {
            face_direction.Items.AddRange(FaceDirectionDisplayText);
            face_direction.SelectedIndex = 0;
        }

        public void getSvgImage(string path)
        {
            XmlDocument xml_file = new XmlDocument();
            SvgDocument svgdoc_file;
            ToolStripButton[] toolStripImage = new ToolStripButton[100];
            ToolStripImageVisibleStart = 2;

            string[] imagelistfilepath = new string[0];
            imagelistfilepath = null;

            if (face_direction.SelectedIndex == 0)
                path = path + "\\" + FaceDirectionDisplayPath[0];
            else if (face_direction.SelectedIndex == 1)
                path = path + "\\" + FaceDirectionDisplayPath[1];
            //else if (face_direction.SelectedIndex == 2)
            //    path = path + "\\" + FaceDirectionDisplayPath[2];

            face_direct = face_direction.SelectedIndex;

            imagelistfilepath = Directory.GetFiles(path, "*.svg");

            xml_file.XmlResolver = null;

            left_shift_Button.Image = global::SVG_Morph.Properties.Resources.arrow_l_off;
            right_shift_Button.Image = global::SVG_Morph.Properties.Resources.arrow_r_off;

            //이미지리스트 초기화
            ToolStripImageSelectedIndex = 0;
            int image_cnt = image_animal_svg.Items.Count;
            for (int j = ToolStripImageInsertPosition; j < (image_cnt - 1); j++)
            {
                image_animal_svg.Items.RemoveAt(ToolStripImageInsertPosition);
            }

            for (int i = 0; i < imagelistfilepath.Length; i++)
            {
                xml_file.Load(imagelistfilepath[i]);
                svgdoc_file = SvgDocument.Open(xml_file);

                svgdoc_file.Width = 60;
                svgdoc_file.Height = 60;

                toolStripImage[i] = new ToolStripButton();
                toolStripImage[i].Tag = imagelistfilepath[i];
                toolStripImage[i].Image = svgdoc_file.Draw();
                toolStripImage[i].ImageScaling = ToolStripItemImageScaling.None;
                toolStripImage[i].BackColor = Color.White;
                toolStripImage[i].Margin = new Padding(0, 0, 15, 0);
                toolStripImage[i].AutoSize = false;
                toolStripImage[i].Size = new Size(70, 70);
                toolStripImage[i].CheckOnClick = true;
                toolStripImage[i].Click += new System.EventHandler(this.sel_toolStripImage_Click);
                toolStripImage[i].DoubleClickEnabled = true;
                toolStripImage[i].DoubleClick += new System.EventHandler(this.sel_toolStripImage_DoubleClick);
                toolStripImage[i].ToolTipText = imagelistfilepath[i];

                if (ToolStripImageDisplayCount - 1 < i) toolStripImage[i].Visible = false;

                image_animal_svg.Items.Insert(image_animal_svg.Items.Count - 1, toolStripImage[i]);
            }

            if (imagelistfilepath.Length > ToolStripImageDisplayCount)
            {
                left_shift_Button.Image = global::SVG_Morph.Properties.Resources.arrow_l;
            }
        }

        private void sel_toolStripImage_Click(object sender, EventArgs e)
        {
            toolStripImage_losefocus((ToolStripButton)sender);
        }

        public void toolStripImage_losefocus(ToolStripButton item)
        {
            for (int i = ToolStripImageInsertPosition; i < image_animal_svg.Items.Count; i++)
            {
                if (item != ((ToolStripButton)image_animal_svg.Items[i]))
                {
                    ((ToolStripButton)image_animal_svg.Items[i]).Checked = false;
                }
                else
                {
                    XMLSelectToolImage.XmlResolver = null;
                    ToolStripImageSelectedIndex = i;
                    XMLSelectToolImage.Load(item.Tag.ToString());
                }
            }
        }

        private void target_image_clp_load(string clp_path)
        {
            XmlDocument w_xml_doc = new XmlDocument();
            w_xml_doc.Load(clp_path);

            int w_map_point_count;
            string w_node_str_value;
            XmlElement w_element = null;
            w_element = w_xml_doc.SelectSingleNode("morphing_control_data/line_num") as XmlElement;
            w_node_str_value = w_element.InnerXml;
            w_map_point_count = Convert.ToInt32(w_node_str_value);
            
            int temp_cnt = 0;

            for (temp_cnt = 0; temp_cnt > 100; temp_cnt++)
            {
                dest_contr_lines[temp_cnt].Px = 0;
                dest_contr_lines[temp_cnt].Py = 0;
                dest_contr_lines[temp_cnt].Qx = 0;
                dest_contr_lines[temp_cnt].Qy = 0;
            }

            temp_cnt = 0;
            XmlNodeList w_node_list = w_xml_doc.SelectNodes("morphing_control_data/lines/line");
            foreach (XmlNode w_node in w_node_list)
            {
                string w_point_str = w_node.SelectSingleNode("px").InnerXml;
                dest_contr_lines[temp_cnt].Px = Convert.ToInt32(w_point_str);

                w_point_str = w_node.SelectSingleNode("py").InnerXml;
                dest_contr_lines[temp_cnt].Py = Convert.ToInt32(w_point_str);

                w_point_str = w_node.SelectSingleNode("qx").InnerXml;
                dest_contr_lines[temp_cnt].Qx = Convert.ToInt32(w_point_str);

                w_point_str = w_node.SelectSingleNode("qy").InnerXml;
                dest_contr_lines[temp_cnt].Qy = Convert.ToInt32(w_point_str);

                temp_cnt++;
            }

            NumDestContrLine = temp_cnt;
            if (w_map_point_count != NumDestContrLine)
            {
                MessageBox.Show("CLP 파일의 Point 개수가 맞지 않습니다.");
                return;
            }
            
            DestControlLineUpdate(TargetImage_X_Padding, TargetImage_Y_Padding);

            target_control_line_display.Enabled = true;

        }//eo_target_image_clp_load();

        private void DestControlLineUpdate(int x_padding, int y_padding)
        {
            int temp_cnt;
            for (temp_cnt = 0; temp_cnt < NumDestContrLine; temp_cnt++)
            {
                dest_contr_lines[temp_cnt].Px = Convert.ToInt32(dest_contr_lines[temp_cnt].Px + x_padding);
                dest_contr_lines[temp_cnt].Py = Convert.ToInt32(dest_contr_lines[temp_cnt].Py + y_padding);
                dest_contr_lines[temp_cnt].Qx = Convert.ToInt32(dest_contr_lines[temp_cnt].Qx + x_padding);
                dest_contr_lines[temp_cnt].Qy = Convert.ToInt32(dest_contr_lines[temp_cnt].Qy + y_padding);
            }
        }

        private void sel_toolStripImage_DoubleClick(object sender, EventArgs e)
        {
            string path, path_clp;
            toolStripImage_losefocus((ToolStripButton)image_animal_svg.Items[ToolStripImageSelectedIndex]);
            path = ((ToolStripButton)image_animal_svg.Items[ToolStripImageSelectedIndex]).Tag.ToString();

            path_clp = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + ".clp";

            FileInfo clp_finfo = new FileInfo(path_clp);

            if (clp_finfo.Exists == false)
            {
                MessageBox.Show("선택한 이미지의 CLP 파일이 없습니다.");
                return;
            }

            svgDraw_target(path);

            target_image_clp_load(path_clp);

            TargetSVGFileOpenCheck = true;

            if (InputSVGFileOpenCheck == true)
            {
                warphing_btn.Enabled = true;
                morphing_btn.Enabled = true;
            }
        }

        private void SVG_Morph_Load(object sender, EventArgs e)
        {
            input_svg_open_btn.Enabled = false;
            warphing_btn.Enabled = false;
            morphing_btn.Enabled = false;
            input_control_line_display.Enabled = false;
            target_control_line_display.Enabled = false;
            InputControlLineEdit_btn.Enabled = false;

            CheckSVGDataFolder();
            face_direction_set();

            setbutton(input_image_move_btn, false);
            setbutton(target_image_move_btn, false);
        }

        private void left_shift_Button_Click(object sender, EventArgs e)
        {
            if ((ToolStripImageVisibleStart + ToolStripImageDisplayCount) < (image_animal_svg.Items.Count - 1))
            {
                image_animal_svg.Items[ToolStripImageVisibleStart].Visible = false;
                image_animal_svg.Items[ToolStripImageVisibleStart + ToolStripImageDisplayCount].Visible = true;
                ToolStripImageVisibleStart = ToolStripImageVisibleStart + 1;
            }

            if ((ToolStripImageVisibleStart + ToolStripImageDisplayCount) < (image_animal_svg.Items.Count - 1))
            {
                right_shift_Button.Image = global::SVG_Morph.Properties.Resources.arrow_r;
            }

            if ((ToolStripImageVisibleStart + ToolStripImageDisplayCount) == (image_animal_svg.Items.Count - 1))
            {
                left_shift_Button.Image = global::SVG_Morph.Properties.Resources.arrow_l_off;
            }
        }

        private void right_shift_Button_Click(object sender, EventArgs e)
        {
            if (ToolStripImageInsertPosition < ToolStripImageVisibleStart)
            {
                image_animal_svg.Items[ToolStripImageVisibleStart + ToolStripImageDisplayCount - 1].Visible = false;
                image_animal_svg.Items[ToolStripImageVisibleStart - 1].Visible = true;
                ToolStripImageVisibleStart = ToolStripImageVisibleStart - 1;
            }

            if (ToolStripImageVisibleStart == ToolStripImageInsertPosition)
            {
                right_shift_Button.Image = global::SVG_Morph.Properties.Resources.arrow_r_off;
            }

            if ((ToolStripImageVisibleStart + ToolStripImageDisplayCount) < (image_animal_svg.Items.Count - 1))
            {
                left_shift_Button.Image = global::SVG_Morph.Properties.Resources.arrow_l;
            }
        }

        private void face_direction_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("Change test");
            getSvgImage(Application.StartupPath);
        }

        public void setbutton(Button btn, bool click)
        {
            if (click)
            {
                btn.Image = global::SVG_Morph.Properties.Resources.o_select;
            }
            else
            {
                btn.Image = global::SVG_Morph.Properties.Resources.u_select;
            }
        }

        public void svgDraw_move_input(SvgDocument svgdoc)
        {
            Bitmap background = new Bitmap(360, 360);
            Bitmap InputSVG = svgdoc.Draw();
            Bitmap result = new Bitmap(background.Width, background.Height);
            Graphics g = Graphics.FromImage(result);
            g.DrawImageUnscaled(background, 0, 0);
            g.Flush();
            g.DrawImageUnscaled(InputSVG, InputImage_X_Padding, InputImage_Y_Padding);
            g.Flush();

            input_svg.Image = result;

            gBitmap_input_backup = (Bitmap)input_svg.Image;
        }

        public void svgDraw_input(string filepath)
        {
            main_xml.XmlResolver = null;
            main_xml.Load(filepath);
            main_svgdoc = SvgDocument.Open(main_xml);

            global_input_file_path = filepath;

            InputImage_X_Padding = (int)(display_x_size - main_svgdoc.Width) / 2;
            InputImage_Y_Padding = (int)(display_y_size - main_svgdoc.Height) / 2;

            InputImageWidth = (int)main_svgdoc.Width;
            InputImageHeight = (int)main_svgdoc.Height;

            Bitmap background = new Bitmap(360, 360);
            Bitmap InputSVG = main_svgdoc.Draw();
            Bitmap result = new Bitmap(background.Width, background.Height);
            Graphics g = Graphics.FromImage(result);
            g.DrawImageUnscaled(background, 0, 0);
            g.Flush();
            g.DrawImageUnscaled(InputSVG, InputImage_X_Padding, InputImage_Y_Padding);
            g.Flush();

            //  제어선 파일 체크하는 부분 필요
            input_svg.Image = result;

            gBitmap_input_backup = (Bitmap)input_svg.Image;
        }


        public void svgDraw_move_target(SvgDocument svgdoc)
        {
            Bitmap background = new Bitmap(360, 360);
            Bitmap TargetSVG = target_svgdoc.Draw();
            Bitmap result = new Bitmap(background.Width, background.Height);
            Graphics g = Graphics.FromImage(result);
            g.DrawImageUnscaled(background, 0, 0);
            g.Flush();
            g.DrawImageUnscaled(TargetSVG, TargetImage_X_Padding, TargetImage_Y_Padding);
            g.Flush();

            target_svg.Image = result;

            gBitmap_target_backup = (Bitmap)target_svg.Image;
        }


        public void svgDraw_target(string filepath)
        {
            target_xml.XmlResolver = null;
            target_xml.Load(filepath);
            target_svgdoc = SvgDocument.Open(target_xml);

            TargetImage_X_Padding = (int)(display_x_size - target_svgdoc.Width) / 2;
            TargetImage_Y_Padding = (int)(display_y_size - target_svgdoc.Height) / 2;

            TargetImageWidth = (int)target_svgdoc.Width;
            TargetImageHeight = (int)target_svgdoc.Height;

            Bitmap background = new Bitmap(360, 360);
            Bitmap TargetSVG = target_svgdoc.Draw();
            Bitmap result = new Bitmap(background.Width, background.Height);
            Graphics g = Graphics.FromImage(result);
            g.DrawImageUnscaled(background, 0, 0);
            g.Flush();
            g.DrawImageUnscaled(TargetSVG, TargetImage_X_Padding, TargetImage_Y_Padding);
            g.Flush();

            target_svg.Image = result;

            gBitmap_target_backup = (Bitmap)target_svg.Image;
        }


        private void default_clp_load(string clp_path, bool state)
        {
            XmlDocument w_xml_doc = new XmlDocument();
            w_xml_doc.Load(clp_path);

            int temp_cnt = 0;

            for (temp_cnt = 0; temp_cnt > 100; temp_cnt++)
            {
                default_contr_lines[temp_cnt].Px = 0;
                default_contr_lines[temp_cnt].Py = 0;
                default_contr_lines[temp_cnt].Qx = 0;
                default_contr_lines[temp_cnt].Qy = 0;

                default_contr_lines_Backup[temp_cnt].Px = 0;
                default_contr_lines_Backup[temp_cnt].Py = 0;
                default_contr_lines_Backup[temp_cnt].Qx = 0;
                default_contr_lines_Backup[temp_cnt].Qy = 0;
            }

            temp_cnt = 0;
            XmlNodeList w_node_list;
            if (state)    //  others
            {
                w_node_list = w_xml_doc.SelectNodes("morphing_control_data/lines/line");
            }
            else  //  false: default
            {
                w_node_list = w_xml_doc.SelectNodes("morphing_control_data/Front/line");

                if (face_direction.SelectedIndex == 0)
                {
                    w_node_list = w_xml_doc.SelectNodes("morphing_control_data/Front/line");
                }
                else if (face_direction.SelectedIndex == 1)
                {
                    w_node_list = w_xml_doc.SelectNodes("morphing_control_data/Side/line");
                }
            }

            foreach (XmlNode w_node in w_node_list)
            {
                string w_point_str = w_node.SelectSingleNode("px").InnerXml;
                default_contr_lines[temp_cnt].Px = Convert.ToInt32(w_point_str);
                default_contr_lines_Backup[temp_cnt].Px = Convert.ToInt32(w_point_str);

                w_point_str = w_node.SelectSingleNode("py").InnerXml;
                default_contr_lines[temp_cnt].Py = Convert.ToInt32(w_point_str);
                default_contr_lines_Backup[temp_cnt].Py = Convert.ToInt32(w_point_str);

                w_point_str = w_node.SelectSingleNode("qx").InnerXml;
                default_contr_lines[temp_cnt].Qx = Convert.ToInt32(w_point_str);
                default_contr_lines_Backup[temp_cnt].Qx = Convert.ToInt32(w_point_str);

                w_point_str = w_node.SelectSingleNode("qy").InnerXml;
                default_contr_lines[temp_cnt].Qy = Convert.ToInt32(w_point_str);
                default_contr_lines_Backup[temp_cnt].Qy = Convert.ToInt32(w_point_str);

                w_point_str = w_node.SelectSingleNode("parts_name").InnerXml;
                default_contr_lines[temp_cnt].parts_name = w_point_str;

                default_contr_lines_Backup[temp_cnt].num = temp_cnt;


                temp_cnt++;
            }

            NumDefaultContrLine = temp_cnt;

            DefaultControlLineUpdate(InputImage_X_Padding, InputImage_Y_Padding);

            input_control_line_display.Enabled = true;
        }

        private void DefaultControlLineUpdate(int x_padding, int y_padding)
        {
            int temp_cnt;

            for (temp_cnt = 0; temp_cnt < NumDefaultContrLine; temp_cnt++)
            {
                default_contr_lines[temp_cnt].Px = Convert.ToInt32(default_contr_lines[temp_cnt].Px + x_padding);
                default_contr_lines[temp_cnt].Py = Convert.ToInt32(default_contr_lines[temp_cnt].Py + y_padding);
                default_contr_lines[temp_cnt].Qx = Convert.ToInt32(default_contr_lines[temp_cnt].Qx + x_padding);
                default_contr_lines[temp_cnt].Qy = Convert.ToInt32(default_contr_lines[temp_cnt].Qy + y_padding);
            }
        }

        private void input_svg_open_btn_Click(object sender, EventArgs e)
        {
            //setbutton(input_svg_open_btn, true);
            //setbutton(input_svg_open_btn, false);

            OpenFileDialog openfiledlg = new OpenFileDialog();

            openfiledlg.Title = "SVG 파일 열기";
            openfiledlg.Filter = "SVG File(*.svg)|*.svg|All Files(*.*)|*.*";

            if (openfiledlg.ShowDialog() == DialogResult.OK)
            {
                string path_clp = Path.GetDirectoryName(openfiledlg.FileName) + "\\" + Path.GetFileNameWithoutExtension(openfiledlg.FileName) + ".clp";
                global_input_file_name = Path.GetFileNameWithoutExtension(openfiledlg.FileName);
                FileInfo clp_finfo = new FileInfo(path_clp);
                if (clp_finfo.Exists == true)
                {
                    //MessageBox.Show("선택한 CLP 파일이 있습니다.");
                    svgDraw_input(openfiledlg.FileName);
                    default_clp_load(path_clp, true);    //  default: false  // others: true
                    toolStripImage_losefocus(null);
                    InputSVGFileOpenCheck = true;
                }
                else
                {
                    //MessageBox.Show("선택한 CLP 파일이 없습니다.");
                    string path_default_clp = Application.StartupPath + "\\" + "default.clp";
                    FileInfo default_clp_finfo = new FileInfo(path_default_clp);
                    if (default_clp_finfo.Exists == false)
                    {
                        MessageBox.Show("Default CLP 파일이 없습니다.");
                        return;
                    }

                    svgDraw_input(openfiledlg.FileName);
                    default_clp_load(path_default_clp, false);    //  default: false  // others: true
                    toolStripImage_losefocus(null);
                    InputSVGFileOpenCheck = true;
                }

                InputControlLineEdit_btn.Enabled = true;
            }


            if (TargetSVGFileOpenCheck == true)
            {
                warphing_btn.Enabled = true;
                morphing_btn.Enabled = true;
            }

        }

        private point_f point_warphing_inverse(float x, float y)
        {
            point_f result_point;
            int num_lines = NumDefaultContrLine;

            double u;
            double h;
            double d;
            double tx, ty;
            double xp, yp;
            double weight;
            double totalWeight;
            //  org
            double a = Convert.ToDouble(aValuetextBox.Text);
            double b = Convert.ToDouble(bValuetextBox.Text);
            double p = Convert.ToDouble(pValuetextBox.Text);

            int x1, x2, y1, y2;
            int tar_x1, tar_y1, tar_x2, tar_y2;
            double src_line_length, dest_line_length;
            int line;
            float target_x, target_y;
            float last_row, last_col;

            last_row = (float)input_svg.Height - 1;
            last_col = (float)input_svg.Width - 1;

            totalWeight = 0.0;
            tx = 0.0;
            ty = 0.0;

            for (line = 0; line < num_lines; line++)
            {
                    x1 = default_contr_lines[line].Px;
                    y1 = default_contr_lines[line].Py;
                    x2 = default_contr_lines[line].Qx;
                    y2 = default_contr_lines[line].Qy;
                
                src_line_length = Math.Sqrt((float)((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)));

                u = (double)((x - x1) * (x2 - x1) + (y - y1) * (y2 - y1)) / (double)((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
                h = (double)((y - y1) * (x2 - x1) - (x - x1) * (y2 - y1)) / src_line_length;

                if (u < 0) d = Math.Sqrt((float)((x - x1) * (x - x1) + (y - y1) * (y - y1)));
                else if (u > 1) d = Math.Sqrt((float)((x - x2) * (x - x2) + (y - y2) * (y - y2)));
                else d = Math.Abs(h);

                if ((morphing_step >= 0) && (morphing_step <= 9))
                {
                    tar_x1 = morphing_contr_lines[morphing_step,line].Px;
                    tar_y1 = morphing_contr_lines[morphing_step,line].Py;
                    tar_x2 = morphing_contr_lines[morphing_step,line].Qx;
                    tar_y2 = morphing_contr_lines[morphing_step,line].Qy;
                }
                else
                {
                    tar_x1 = dest_contr_lines[line].Px;
                    tar_y1 = dest_contr_lines[line].Py;
                    tar_x2 = dest_contr_lines[line].Qx;
                    tar_y2 = dest_contr_lines[line].Qy;
                }

                dest_line_length = Math.Sqrt((float)((tar_x2 - tar_x1) * (tar_x2 - tar_x1) + (tar_y2 - tar_y1) * (tar_y2 - tar_y1)));

                xp = tar_x1 + u * (tar_x2 - tar_x1) - h * (tar_y2 - tar_y1) / dest_line_length;
                yp = tar_y1 + u * (tar_y2 - tar_y1) + h * (tar_x2 - tar_x1) / dest_line_length;

                weight = Math.Pow((Math.Pow((double)(src_line_length), p) / (a + d)), b);

                tx += (xp - x) * weight;
                ty += (yp - y) * weight;
                totalWeight += weight;
            }

            target_x = x + (float)(tx / totalWeight + 0.5);
            target_y = y + (float)(ty / totalWeight + 0.5);

            if (target_x < 0) target_x = 0;
            if (target_x > last_col) target_x = last_col;
            if (target_y < 0) target_y = 0;
            if (target_y > last_row) target_y = last_row;

            result_point.x = target_x;
            result_point.y = target_y;

            return result_point;
        }

        private point_f point_warphing(float x, float y)
        {
            point_f result_point;
            int num_lines = NumDefaultContrLine;

            double u;
            double h;
            double d;
            double tx, ty;
            double xp, yp;
            double weight;
            double totalWeight;
            //  org
            double a = 0.001;
            double b = 2.0;
            double p = 0.75;
            //

            int x1, x2, y1, y2;
            int src_x1, src_y1, src_x2, src_y2;
            double src_line_length, dest_line_length;
            int line;
            //int x, y;
            float source_x, source_y;
            float last_row, last_col;

            last_row = (float)input_svg.Height - 1;
            last_col = (float)input_svg.Width - 1;

            totalWeight = 0.0;
            tx = 0.0;
            ty = 0.0;
            for (line = 0; line < num_lines; line++)
            {
                x1 = dest_contr_lines[line].Px;
                y1 = dest_contr_lines[line].Py;
                x2 = dest_contr_lines[line].Qx;
                y2 = dest_contr_lines[line].Qy;

                dest_line_length = Math.Sqrt((float)((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)));

                u = (double)((x - x1) * (x2 - x1) + (y - y1) * (y2 - y1)) / (double)((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
                h = (double)((y - y1) * (x2 - x1) - (x - x1) * (y2 - y1)) / dest_line_length;

                if (u < 0) d = Math.Sqrt((float)((x - x1) * (x - x1) + (y - y1) * (y - y1)));
                else if (u > 1) d = Math.Sqrt((float)((x - x2) * (x - x2) + (y - y2) * (y - y2)));
                else d = Math.Abs(h);

                src_x1 = default_contr_lines[line].Px;
                src_y1 = default_contr_lines[line].Py;
                src_x2 = default_contr_lines[line].Qx;
                src_y2 = default_contr_lines[line].Qy;
                src_line_length = Math.Sqrt((float)((src_x2 - src_x1) * (src_x2 - src_x1) + (src_y2 - src_y1) * (src_y2 - src_y1)));

                xp = src_x1 + u * (src_x2 - src_x1) - h * (src_y2 - src_y1) / src_line_length;
                yp = src_y1 + u * (src_y2 - src_y1) + h * (src_x2 - src_x1) / src_line_length;

                weight = Math.Pow((Math.Pow((double)(dest_line_length), p) / (a + d)), b);

                tx += (xp - x) * weight;
                ty += (yp - y) * weight;
                totalWeight += weight;
            }

            source_x = x + (float)(tx / totalWeight + 0.5);
            source_y = y + (float)(ty / totalWeight + 0.5);

            if (source_x < 0) source_x = 0;
            if (source_x > last_col) source_x = last_col;
            if (source_y < 0) source_y = 0;
            if (source_y > last_row) source_y = last_row;

            result_point.x = source_x;
            result_point.y = source_y;

            return result_point;
        }


        public void SvgWarping(XmlNode node)
        {
            XmlNode target_node = node;

            SvgWarpingNode(node);

            foreach (XmlNode childNode in target_node.ChildNodes)
            {
                SvgWarping(childNode);
            }
        }

        public static XmlNode RenameNode(XmlNode node, string namespaceUri, string qualifiedName)
        {
            if (node.NodeType == XmlNodeType.Element)
            {
                XmlElement oldElement = (XmlElement)node;
                XmlElement newElement = node.OwnerDocument.CreateElement(qualifiedName, namespaceUri);
                //XmlElement newElement = node.OwnerDocument.CreateElement(qualifiedName);
                while (oldElement.HasAttributes)
                {
                    newElement.SetAttributeNode(oldElement.RemoveAttributeNode(oldElement.Attributes[0]));
                }
                while (oldElement.HasChildNodes)
                {
                    newElement.AppendChild(oldElement.FirstChild);
                }
                if (oldElement.ParentNode != null)
                {
                    oldElement.ParentNode.ReplaceChild(newElement, oldElement);
                }
                return newElement;
            }
            else
            {
                return null;
            }
        }

        public void SvgWarpingNode(XmlNode node)
        {
            string chk_upper = @"([A-Z])";
            string x1, x2, y1, y2;

            point_f path_temp_point;
            point_f warp_result;

            path_temp_point.x = 0;
            path_temp_point.y = 0;

            switch (node.Name)
            {
                case "path":
                    string new_d = "";
                    string path_d = node.Attributes["d"].Value;
                    path_d = path_d.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "").Replace("\t\t", "");

                    string[] points = Regex.Split(path_d, command);

                    for (int k = 0; k < points.Length; k++)
                    {
                        if (points[k] == "") continue;

                        //command check                        
                        if (char.IsLetter(points[k], 0) && Regex.IsMatch(points[k], chk_upper)) //절대좌표
                        {
                            if ((points[k] == "V") | (points[k] == "H")) new_d = new_d + "L";
                            else new_d = new_d + points[k];

                            // "V" 인경우는 Y좌표계만 처리됨
                            if (points[k] == "V")
                            {
                                k++;

                                string temp_str = points[k];
                                path_temp_point.y = float.Parse(temp_str.Replace(",", ""));

                                warp_result = point_warphing_inverse(path_temp_point.x, path_temp_point.y);

                                new_d = new_d + warp_result.x.ToString() + ", " + warp_result.y.ToString() + " ";
                                continue;
                            }

                            // "H" 인경우는 X좌표계만 처리됨
                            if (points[k] == "H")
                            {
                                k++;

                                string temp_str = points[k];
                                path_temp_point.x = float.Parse(temp_str.Replace(",", ""));

                                warp_result = point_warphing_inverse(path_temp_point.x, path_temp_point.y);

                                new_d = new_d + warp_result.x.ToString() + "," + warp_result.y.ToString() + " ";
                                continue;
                            }

                            k++;
                            if (k < points.Length)
                            {
                                string[] d_point = points[k].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                                for (int m = 0; m < d_point.Length; m++)
                                {
                                    if (m < d_point.Length - 1)
                                    {
                                        if (m % 2 == 0)
                                        {
                                            path_temp_point.x = float.Parse(d_point[m]);
                                        }
                                        else
                                        {
                                            path_temp_point.y = float.Parse(d_point[m]);
                                            warp_result = point_warphing_inverse(path_temp_point.x, path_temp_point.y);
                                            new_d = new_d + warp_result.x.ToString() + "," + warp_result.y.ToString() + ",";
                                        }
                                    }
                                    else
                                    {
                                        if (m % 2 == 0)
                                        {
                                            path_temp_point.x = float.Parse(d_point[m]);
                                        }
                                        else
                                        {
                                            path_temp_point.y = float.Parse(d_point[m]);
                                            warp_result = point_warphing_inverse(path_temp_point.x, path_temp_point.y);
                                            new_d = new_d + warp_result.x.ToString() + "," + warp_result.y.ToString() + " ";
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            new_d = new_d + points[k];
                        }
                    }
                    node.Attributes["d"].Value = new_d;

                    break;
                case "circle":

                    MessageBox.Show("circle 와핑 에러_000");

                    
                    break;
                case "line":
                    x1 = node.Attributes["x1"].Value;
                    y1 = node.Attributes["y1"].Value;
                    x2 = node.Attributes["x2"].Value;
                    y2 = node.Attributes["y2"].Value;

                    warp_result = point_warphing_inverse(float.Parse(x1), float.Parse(y1));
                    node.Attributes["x1"].Value = warp_result.x.ToString();
                    node.Attributes["y1"].Value = warp_result.y.ToString();

                    warp_result = point_warphing_inverse(float.Parse(x2), float.Parse(y2));
                    node.Attributes["x2"].Value = warp_result.x.ToString();
                    node.Attributes["y2"].Value = warp_result.y.ToString();

                    break;
                case "rect":
                    MessageBox.Show("rect 와핑 에러_000");

                    
                    break;
                case "ellipse":
                    MessageBox.Show("ellipse 와핑 에러_000");
                    //attrib_fill = node.OwnerDocument.CreateAttribute("fill");
                    //attrib_fill.Value = "transparent";
                    //if (node.Attributes["fill"] == null) node.Attributes.Append(attrib_fill);

                    //attrib_stroke = node.OwnerDocument.CreateAttribute("stroke");
                    //attrib_stroke.Value = "black";
                    //if (node.Attributes["stroke"] == null) node.Attributes.Append(attrib_stroke);

                    break;
                case "polygon":
                case "polyline":
                    string pg_points, pg_new_points = "";

                    pg_points = node.Attributes["points"].Value;
                    pg_points = pg_points.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "").Replace("\t\t", "");

                    string[] pg_point = pg_points.Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                    for (int k = 0; k < pg_point.Length; k++)
                    {
                        if (k < pg_point.Length - 1)
                        {
                            if (k % 2 == 0)
                            {
                                path_temp_point.x = float.Parse(pg_point[k]);
                            }
                            else
                            {
                                path_temp_point.y = float.Parse(pg_point[k]);
                                warp_result = point_warphing_inverse(path_temp_point.x, path_temp_point.y);
                                pg_new_points = pg_new_points + warp_result.x.ToString() + "," + warp_result.y.ToString() + ",";
                            }
                        }
                        else
                        {
                            if (k % 2 == 0)
                            {
                                path_temp_point.x = float.Parse(pg_point[k]);
                            }
                            else
                            {
                                path_temp_point.y = float.Parse(pg_point[k]);
                                warp_result = point_warphing_inverse(path_temp_point.x, path_temp_point.y);
                                pg_new_points = pg_new_points + warp_result.x.ToString() + "," + warp_result.y.ToString() + " ";
                            }
                        }
                    }
                    node.Attributes["points"].Value = pg_new_points;

                    break;
            }
        }

        private void warphing_btn_Click(object sender, EventArgs e)
        {
            warphing_btn.Enabled = false;
            morphing_btn.Enabled = false;

            SvgTransDocument SvgTrans = new SvgTransDocument();

            main_xml_trans = main_xml;    //  org
            //main_xml_trans = (XmlDocument)main_xml.CloneNode(true);
            //main_xml_trans = (XmlDocument)main_xml.Clone();    //  by ki  too late
           

            
            XmlNode root = main_xml_trans.DocumentElement;
            root.Attributes["width"].Value = "360px";
            root.Attributes["height"].Value = "360px";
            if (root.Attributes["viewBox"] != null) root.Attributes["viewBox"].Value = "0 0 360 360";
            if (root.Attributes["enable-background"] != null) root.Attributes["enable-background"].Value = "new 0 0 360 360";

            foreach (XmlNode childNode in root.ChildNodes)
            {
                SvgTrans.SvgMove(childNode, (float)InputImage_X_Padding, (float)InputImage_Y_Padding);
                //SvgTrans.SvgMove(childNode, (float)200, (float)200);    //  for test
            }


            XmlNodeList circleNodeList = main_xml_trans.SelectNodes("//*[starts-with(name(), 'circle')]");
            foreach (XmlNode circleNode in circleNodeList)
            {
                XmlNode node = circleNode;

                XmlAttribute attrib = null;
                string center_x, center_y, r;
                point_f[] p_array = new point_f[24];
                double angle = 0;
                float temp_x, temp_y;

                center_x = node.Attributes["cx"].Value;
                center_y = node.Attributes["cy"].Value;
                r = node.Attributes["r"].Value;

                node.Attributes.Remove(node.Attributes["cx"]);
                node.Attributes.Remove(node.Attributes["cy"]);
                node.Attributes.Remove(node.Attributes["r"]);

                string temp_buffer_str = "";

                for (int cnt = 0; cnt < 24; cnt++)
                {
                    angle = (float)15 * cnt;
                    angle = angle * (Math.PI / 180);
                    temp_x = float.Parse(center_x) + (float.Parse(r) * (float)Math.Cos(angle));
                    temp_y = -1 * (float.Parse(r) * (float)Math.Sin(angle));
                    temp_y = temp_y + float.Parse(center_y);

                    if (cnt == 0)
                    {
                        temp_buffer_str = temp_buffer_str + "M" + temp_x.ToString() + "," + temp_y.ToString() + " ";
                    }
                    else
                    {
                        temp_buffer_str = temp_buffer_str + "L" + temp_x.ToString() + "," + temp_y.ToString() + " ";
                    }
                }

                temp_buffer_str = temp_buffer_str + "Z" + " ";

                attrib = node.OwnerDocument.CreateAttribute("d");
                attrib.Value = "";
                node.Attributes.Append(attrib);
                node.Attributes["d"].Value = temp_buffer_str;

                node = RenameNode(node, "circle", "path");
            }

            XmlNodeList rectNodeList = main_xml_trans.SelectNodes("//*[starts-with(name(), 'rect')]");
            foreach (XmlNode rectNode in rectNodeList)
            {
                XmlNode node = rectNode;

                point_f rect_temp_point_0;
                point_f rect_temp_point_1;
                point_f rect_temp_point_2;
                point_f rect_temp_point_3;

                XmlAttribute attrib = null;

                rect_temp_point_0.x = float.Parse(node.Attributes["x"].Value);
                rect_temp_point_0.y = float.Parse(node.Attributes["y"].Value);

                rect_temp_point_1.x = float.Parse(node.Attributes["x"].Value) + float.Parse(node.Attributes["width"].Value);
                rect_temp_point_1.y = float.Parse(node.Attributes["y"].Value);

                rect_temp_point_2.x = float.Parse(node.Attributes["x"].Value);
                rect_temp_point_2.y = float.Parse(node.Attributes["y"].Value) + float.Parse(node.Attributes["height"].Value);

                rect_temp_point_3.x = float.Parse(node.Attributes["x"].Value) + float.Parse(node.Attributes["width"].Value);
                rect_temp_point_3.y = float.Parse(node.Attributes["y"].Value) + float.Parse(node.Attributes["height"].Value);


                node.Attributes.Remove(node.Attributes["x"]);
                node.Attributes.Remove(node.Attributes["y"]);
                node.Attributes.Remove(node.Attributes["width"]);
                node.Attributes.Remove(node.Attributes["height"]);
                if (node.Attributes["rx"] != null) node.Attributes.Remove(node.Attributes["rx"]);
                if (node.Attributes["ry"] != null) node.Attributes.Remove(node.Attributes["ry"]);

                attrib = node.OwnerDocument.CreateAttribute("d");
                attrib.Value = "";
                node.Attributes.Append(attrib);

                string temp_buffer_str = "";

                temp_buffer_str = temp_buffer_str + "M";
                temp_buffer_str = temp_buffer_str + rect_temp_point_0.x.ToString() + "," + rect_temp_point_0.y.ToString() + " ";

                temp_buffer_str = temp_buffer_str + "L";
                temp_buffer_str = temp_buffer_str + rect_temp_point_1.x.ToString() + "," + rect_temp_point_1.y.ToString() + " ";

                temp_buffer_str = temp_buffer_str + "L";
                temp_buffer_str = temp_buffer_str + rect_temp_point_3.x.ToString() + "," + rect_temp_point_3.y.ToString() + " ";

                temp_buffer_str = temp_buffer_str + "L";
                temp_buffer_str = temp_buffer_str + rect_temp_point_2.x.ToString() + "," + rect_temp_point_2.y.ToString() + " ";

                temp_buffer_str = temp_buffer_str + "Z";

                node.Attributes["d"].Value = temp_buffer_str;

                node = RenameNode(node, "rect", "path");
            }

            XmlNodeList ellipseNodeList = main_xml_trans.SelectNodes("//*[starts-with(name(), 'ellipse')]");
            foreach (XmlNode ellipseNode in ellipseNodeList)
            {
                XmlNode node = ellipseNode;

                XmlAttribute attrib = null;

                string elli_cx, elli_cy, elli_rx, elli_ry;
                point_f[] elli_p_array = new point_f[24];

                double elli_angle = 0;
                float elli_temp_x, elli_temp_y;

                elli_cx = node.Attributes["cx"].Value;
                elli_cy = node.Attributes["cy"].Value;
                elli_rx = node.Attributes["rx"].Value;
                elli_ry = node.Attributes["ry"].Value;

                node.Attributes.Remove(node.Attributes["cx"]);
                node.Attributes.Remove(node.Attributes["cy"]);
                node.Attributes.Remove(node.Attributes["rx"]);
                node.Attributes.Remove(node.Attributes["ry"]);
                
                string temp_buffer_str = "";

                for (int cnt = 0; cnt < 24; cnt++)
                {
                    elli_angle = (float)15 * cnt;
                    elli_angle = elli_angle * (Math.PI / 180);

                    elli_temp_x = float.Parse(elli_cx) + (float.Parse(elli_rx) * (float)Math.Cos(elli_angle));
                    elli_temp_y = -1 * (float.Parse(elli_ry) * (float)Math.Sin(elli_angle));
                    elli_temp_y = elli_temp_y + float.Parse(elli_cy);

                    if (cnt == 0)
                    {
                        temp_buffer_str = temp_buffer_str + "M" + elli_temp_x.ToString() + "," + elli_temp_y.ToString() + " ";
                    }
                    else
                    {
                        temp_buffer_str = temp_buffer_str + "L" + elli_temp_x.ToString() + "," + elli_temp_y.ToString() + " ";
                    }
                }

                temp_buffer_str = temp_buffer_str + "Z" + " ";

                attrib = node.OwnerDocument.CreateAttribute("d");
                attrib.Value = "";
                node.Attributes.Append(attrib);
                node.Attributes["d"].Value = temp_buffer_str;

                node = RenameNode(node, "ellipse", "path");
            }

            morphing_step = 10;
            morphing_image_source_step = 10;
            foreach (XmlNode childNode in root.ChildNodes)
            {
                SvgWarping(childNode);
            }

            main_svgdoc_trans = SvgDocument.Open(main_xml_trans);
            morph_svg.Image = main_svgdoc_trans.Draw();

            main_xml_trans.Save("temp10");

            warphing_btn.Enabled = true;
            morphing_btn.Enabled = true;

            main_xml.Load(global_input_file_path);    //  input svg reload

        }//eo private void warphing_btn_Click(object sender, EventArgs e)


        private void make_morphing_contr_line()
        {
            float dx_p, dy_p, dx_q, dy_q;
            float div_rate;
            for (int i = 0; i < 10; i++)
            {
                div_rate = ((float)(i + 1)) / 10f;

                //for (int j = 0; j < NumDestContrLine; j++)
                for (int j = 0; j < 100; j++)
                {
                    dx_p = dest_contr_lines[j].Px - default_contr_lines[j].Px;
                    dy_p = dest_contr_lines[j].Py - default_contr_lines[j].Py;

                    dx_q = dest_contr_lines[j].Qx - default_contr_lines[j].Qx;
                    dy_q = dest_contr_lines[j].Qy - default_contr_lines[j].Qy;

                    dx_p = dx_p * div_rate;
                    dy_p = dy_p * div_rate;
                    dx_q = dx_q * div_rate;
                    dy_q = dy_q * div_rate;

                    morphing_contr_lines[i, j].Px = default_contr_lines[j].Px + (int)dx_p;
                    morphing_contr_lines[i, j].Py = default_contr_lines[j].Py + (int)dy_p;
                    morphing_contr_lines[i, j].Qx = default_contr_lines[j].Qx + (int)dx_q;
                    morphing_contr_lines[i, j].Qy = default_contr_lines[j].Qy + (int)dy_q;
                }
            }
        }

        private void morphing_btn_Click(object sender, EventArgs e)
        {
            int NUM_FRAMES = 10;
            int frame = 0;

            warphing_btn.Enabled = false;
            morphing_btn.Enabled = false;

            make_morphing_contr_line();
            //main_xml_trans = main_xml;

            //XmlNode root = main_xml_trans.DocumentElement;
            //root.Attributes["width"].Value = "360px";
            //root.Attributes["height"].Value = "360px";
            //if (root.Attributes["viewBox"] != null) root.Attributes["viewBox"].Value = "0 0 360 360";
            //if (root.Attributes["enable-background"] != null) root.Attributes["enable-background"].Value = "new 0 0 360 360";

            SvgTransDocument SvgTrans = new SvgTransDocument();

            for (frame = 0; frame < NUM_FRAMES; frame++)
            {
                morphing_step = frame;

                main_xml.Load(global_input_file_path);    //  input svg reload
                main_xml_trans = main_xml;

                XmlNode root = main_xml_trans.DocumentElement;
                root.Attributes["width"].Value = "360px";
                root.Attributes["height"].Value = "360px";
                if (root.Attributes["viewBox"] != null) root.Attributes["viewBox"].Value = "0 0 360 360";
                if (root.Attributes["enable-background"] != null) root.Attributes["enable-background"].Value = "new 0 0 360 360";

                foreach (XmlNode childNode in root.ChildNodes)
                {
                    SvgTrans.SvgMove(childNode, (float)InputImage_X_Padding, (float)InputImage_Y_Padding);
                }


                XmlNodeList circleNodeList = main_xml_trans.SelectNodes("//*[starts-with(name(), 'circle')]");
                foreach (XmlNode circleNode in circleNodeList)
                {
                    XmlNode node = circleNode;

                    XmlAttribute attrib = null;
                    string center_x, center_y, r;
                    point_f[] p_array = new point_f[24];
                    double angle = 0;
                    float temp_x, temp_y;

                    center_x = node.Attributes["cx"].Value;
                    center_y = node.Attributes["cy"].Value;
                    r = node.Attributes["r"].Value;

                    node.Attributes.Remove(node.Attributes["cx"]);
                    node.Attributes.Remove(node.Attributes["cy"]);
                    node.Attributes.Remove(node.Attributes["r"]);

                    string temp_buffer_str = "";

                    for (int cnt = 0; cnt < 24; cnt++)
                    {
                        angle = (float)15 * cnt;
                        angle = angle * (Math.PI / 180);
                        temp_x = float.Parse(center_x) + (float.Parse(r) * (float)Math.Cos(angle));
                        temp_y = -1 * (float.Parse(r) * (float)Math.Sin(angle));
                        temp_y = temp_y + float.Parse(center_y);

                        if (cnt == 0)
                        {
                            temp_buffer_str = temp_buffer_str + "M" + temp_x.ToString() + "," + temp_y.ToString() + " ";
                        }
                        else
                        {
                            temp_buffer_str = temp_buffer_str + "L" + temp_x.ToString() + "," + temp_y.ToString() + " ";
                        }
                    }

                    temp_buffer_str = temp_buffer_str + "Z" + " ";

                    attrib = node.OwnerDocument.CreateAttribute("d");
                    attrib.Value = "";
                    node.Attributes.Append(attrib);
                    node.Attributes["d"].Value = temp_buffer_str;

                    node = RenameNode(node, "circle", "path");
                }

                XmlNodeList rectNodeList = main_xml_trans.SelectNodes("//*[starts-with(name(), 'rect')]");
                foreach (XmlNode rectNode in rectNodeList)
                {
                    XmlNode node = rectNode;

                    point_f rect_temp_point_0;
                    point_f rect_temp_point_1;
                    point_f rect_temp_point_2;
                    point_f rect_temp_point_3;

                    XmlAttribute attrib = null;

                    rect_temp_point_0.x = float.Parse(node.Attributes["x"].Value);
                    rect_temp_point_0.y = float.Parse(node.Attributes["y"].Value);

                    rect_temp_point_1.x = float.Parse(node.Attributes["x"].Value) + float.Parse(node.Attributes["width"].Value);
                    rect_temp_point_1.y = float.Parse(node.Attributes["y"].Value);

                    rect_temp_point_2.x = float.Parse(node.Attributes["x"].Value);
                    rect_temp_point_2.y = float.Parse(node.Attributes["y"].Value) + float.Parse(node.Attributes["height"].Value);

                    rect_temp_point_3.x = float.Parse(node.Attributes["x"].Value) + float.Parse(node.Attributes["width"].Value);
                    rect_temp_point_3.y = float.Parse(node.Attributes["y"].Value) + float.Parse(node.Attributes["height"].Value);


                    node.Attributes.Remove(node.Attributes["x"]);
                    node.Attributes.Remove(node.Attributes["y"]);
                    node.Attributes.Remove(node.Attributes["width"]);
                    node.Attributes.Remove(node.Attributes["height"]);
                    if (node.Attributes["rx"] != null) node.Attributes.Remove(node.Attributes["rx"]);
                    if (node.Attributes["ry"] != null) node.Attributes.Remove(node.Attributes["ry"]);

                    attrib = node.OwnerDocument.CreateAttribute("d");
                    attrib.Value = "";
                    node.Attributes.Append(attrib);

                    string temp_buffer_str = "";

                    temp_buffer_str = temp_buffer_str + "M";
                    temp_buffer_str = temp_buffer_str + rect_temp_point_0.x.ToString() + "," + rect_temp_point_0.y.ToString() + " ";

                    temp_buffer_str = temp_buffer_str + "L";
                    temp_buffer_str = temp_buffer_str + rect_temp_point_1.x.ToString() + "," + rect_temp_point_1.y.ToString() + " ";

                    temp_buffer_str = temp_buffer_str + "L";
                    temp_buffer_str = temp_buffer_str + rect_temp_point_3.x.ToString() + "," + rect_temp_point_3.y.ToString() + " ";

                    temp_buffer_str = temp_buffer_str + "L";
                    temp_buffer_str = temp_buffer_str + rect_temp_point_2.x.ToString() + "," + rect_temp_point_2.y.ToString() + " ";

                    temp_buffer_str = temp_buffer_str + "Z";

                    node.Attributes["d"].Value = temp_buffer_str;

                    node = RenameNode(node, "rect", "path");
                }

                XmlNodeList ellipseNodeList = main_xml_trans.SelectNodes("//*[starts-with(name(), 'ellipse')]");
                foreach (XmlNode ellipseNode in ellipseNodeList)
                {
                    XmlNode node = ellipseNode;

                    XmlAttribute attrib = null;

                    //if (node.Attributes["transform"] != null) node.Attributes.Remove(node.Attributes["transform"]);    //  by ki
                    string elli_cx, elli_cy, elli_rx, elli_ry;
                    point_f[] elli_p_array = new point_f[24];

                    double elli_angle = 0;
                    float elli_temp_x, elli_temp_y;

                    elli_cx = node.Attributes["cx"].Value;
                    elli_cy = node.Attributes["cy"].Value;
                    elli_rx = node.Attributes["rx"].Value;
                    elli_ry = node.Attributes["ry"].Value;

                    node.Attributes.Remove(node.Attributes["cx"]);
                    node.Attributes.Remove(node.Attributes["cy"]);
                    node.Attributes.Remove(node.Attributes["rx"]);
                    node.Attributes.Remove(node.Attributes["ry"]);

                    string temp_buffer_str = "";

                    for (int cnt = 0; cnt < 24; cnt++)
                    {
                        elli_angle = (float)15 * cnt;
                        elli_angle = elli_angle * (Math.PI / 180);

                        elli_temp_x = float.Parse(elli_cx) + (float.Parse(elli_rx) * (float)Math.Cos(elli_angle));
                        elli_temp_y = -1 * (float.Parse(elli_ry) * (float)Math.Sin(elli_angle));
                        elli_temp_y = elli_temp_y + float.Parse(elli_cy);

                        if (cnt == 0)
                        {
                            temp_buffer_str = temp_buffer_str + "M" + elli_temp_x.ToString() + "," + elli_temp_y.ToString() + " ";
                        }
                        else
                        {
                            temp_buffer_str = temp_buffer_str + "L" + elli_temp_x.ToString() + "," + elli_temp_y.ToString() + " ";
                        }
                    }

                    temp_buffer_str = temp_buffer_str + "Z" + " ";

                    attrib = node.OwnerDocument.CreateAttribute("d");
                    attrib.Value = "";
                    node.Attributes.Append(attrib);
                    node.Attributes["d"].Value = temp_buffer_str;

                    node = RenameNode(node, "ellipse", "path");
                }


                foreach (XmlNode childNode in root.ChildNodes)
                {
                    SvgWarping(childNode);
                }

                if (frame == 0)
                {
                    main_svgdoc_trans = SvgDocument.Open(main_xml_trans);
                    panel_morph_01.Image = main_svgdoc_trans.Draw();
                    panel_morph_01.Refresh();
                    main_xml_trans.Save("temp00");
                }
                else if (frame == 1)
                {
                    main_svgdoc_trans = SvgDocument.Open(main_xml_trans);
                    panel_morph_02.Image = main_svgdoc_trans.Draw();
                    panel_morph_02.Refresh();
                    main_xml_trans.Save("temp01");
                }
                else if (frame == 2)
                {
                    main_svgdoc_trans = SvgDocument.Open(main_xml_trans);
                    panel_morph_03.Image = main_svgdoc_trans.Draw();
                    panel_morph_03.Refresh();
                    main_xml_trans.Save("temp02");
                }
                else if (frame == 3)
                {
                    main_svgdoc_trans = SvgDocument.Open(main_xml_trans);
                    panel_morph_04.Image = main_svgdoc_trans.Draw();
                    panel_morph_04.Refresh();
                    main_xml_trans.Save("temp03");
                }
                else if (frame == 4)
                {
                    main_svgdoc_trans = SvgDocument.Open(main_xml_trans);
                    panel_morph_05.Image = main_svgdoc_trans.Draw();
                    panel_morph_05.Refresh();
                    main_xml_trans.Save("temp04");
                }
                else if (frame == 5)
                {
                    main_svgdoc_trans = SvgDocument.Open(main_xml_trans);
                    panel_morph_06.Image = main_svgdoc_trans.Draw();
                    panel_morph_06.Refresh();
                    main_xml_trans.Save("temp05");
                }
                else if (frame == 6)
                {
                    main_svgdoc_trans = SvgDocument.Open(main_xml_trans);
                    panel_morph_07.Image = main_svgdoc_trans.Draw();
                    panel_morph_07.Refresh();
                    main_xml_trans.Save("temp06");
                }
                else if (frame == 7)
                {
                    main_svgdoc_trans = SvgDocument.Open(main_xml_trans);
                    panel_morph_08.Image = main_svgdoc_trans.Draw();
                    panel_morph_08.Refresh();
                    main_xml_trans.Save("temp07");
                }
                else if (frame == 8)
                {
                    main_svgdoc_trans = SvgDocument.Open(main_xml_trans);
                    panel_morph_09.Image = main_svgdoc_trans.Draw();
                    panel_morph_09.Refresh();
                    main_xml_trans.Save("temp08");
                }
                else if (frame == 9)
                {
                    main_svgdoc_trans = SvgDocument.Open(main_xml_trans);
                    panel_morph_10.Image = main_svgdoc_trans.Draw();
                    panel_morph_10.Refresh();
                    main_xml_trans.Save("temp09");
                }
            }

            warphing_btn.Enabled = true;
            morphing_btn.Enabled = true;

            main_xml.Load(global_input_file_path);    //  input svg reload

        }//eo_morphing_btn_Click()



        private void panel_morph_01_Click(object sender, EventArgs e)
        {
            morph_svg.Image = panel_morph_01.Image;
            morphing_image_source_step = 0;
        }

        private void panel_morph_02_Click(object sender, EventArgs e)
        {
            morph_svg.Image = panel_morph_02.Image;
            morphing_image_source_step = 1;
        }

        private void panel_morph_03_Click(object sender, EventArgs e)
        {
            morph_svg.Image = panel_morph_03.Image;
            morphing_image_source_step = 2;
        }

        private void panel_morph_04_Click(object sender, EventArgs e)
        {
            morph_svg.Image = panel_morph_04.Image;
            morphing_image_source_step = 3;
        }

        private void panel_morph_05_Click(object sender, EventArgs e)
        {
            morph_svg.Image = panel_morph_05.Image;
            morphing_image_source_step = 4;
        }

        private void panel_morph_06_Click(object sender, EventArgs e)
        {
            morph_svg.Image = panel_morph_06.Image;
            morphing_image_source_step = 5;
        }

        private void panel_morph_07_Click(object sender, EventArgs e)
        {
            morph_svg.Image = panel_morph_07.Image;
            morphing_image_source_step = 6;
        }

        private void panel_morph_08_Click(object sender, EventArgs e)
        {
            morph_svg.Image = panel_morph_08.Image;
            morphing_image_source_step = 7;
        }

        private void panel_morph_09_Click(object sender, EventArgs e)
        {
            morph_svg.Image = panel_morph_09.Image;
            morphing_image_source_step = 8;
        }

        private void panel_morph_10_Click(object sender, EventArgs e)
        {
            morph_svg.Image = panel_morph_10.Image;
            morphing_image_source_step = 9;
        }


        private void input_line_draw()
        {
            Graphics g = input_svg.CreateGraphics();
            Pen p = new Pen(Color.Blue, 2);
            int cnt;
            for (cnt = 0; cnt < NumDefaultContrLine; cnt++)
            {
                Point point1 = new Point(default_contr_lines[cnt].Px, default_contr_lines[cnt].Py);
                Point point2 = new Point(default_contr_lines[cnt].Qx, default_contr_lines[cnt].Qy);
                g.DrawLine(p, point1, point2);
            }
        }

        private void input_image_redraw()
        {
            input_svg.Image = gBitmap_input_backup;
            input_svg.Refresh();
        }


        private void input_control_line_display_CheckedChanged(object sender, EventArgs e)
        {
            if (input_control_line_display.Checked)
            {
                input_line_draw();
            }
            else
            {
                input_image_redraw();
            }

        }

        private void target_image_redraw()
        {
            target_svg.Image = gBitmap_target_backup;
            target_svg.Refresh();
        }

        private void target_line_draw()
        {
            Graphics g = target_svg.CreateGraphics();
            Pen p = new Pen(Color.Blue, 2);
            int cnt;
            for (cnt = 0; cnt < NumDestContrLine; cnt++)
            {
                Point point1 = new Point(dest_contr_lines[cnt].Px, dest_contr_lines[cnt].Py);
                Point point2 = new Point(dest_contr_lines[cnt].Qx, dest_contr_lines[cnt].Qy);
                g.DrawLine(p, point1, point2);
            }
        }

        private void target_control_line_display_CheckedChanged(object sender, EventArgs e)
        {
            if (target_control_line_display.Checked)
            {
                target_line_draw();
            }
            else
            {
                target_image_redraw();
            }
        }

        private void input_image_move_btn_Click(object sender, EventArgs e)
        {
            if (InputSVGFileOpenCheck)
            {
                if (input_move_check)
                {
                    input_move_check = false;
                    setbutton(input_image_move_btn, false);
                }
                else
                {
                    input_move_check = true;
                    setbutton(input_image_move_btn, true);
                }
            }
            else
            {
                MessageBox.Show("Input File이 로드되지 않았습니다.");
                return;
            }
        }


        private void target_image_move_btn_Click(object sender, EventArgs e)
        {
            if (TargetSVGFileOpenCheck)
            {
                if (target_move_check)
                {
                    target_move_check = false;
                    setbutton(target_image_move_btn, false);
                }
                else
                {
                    target_move_check = true;
                    setbutton(target_image_move_btn, true);
                }
            }
            else
            {
                MessageBox.Show("Target File이 로드되지 않았습니다.");
                return;
            }
        }

        private void input_svg_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Thread_svg_input != null) Thread_svg_input.Abort();

                input_svg_move_base_x = (float)e.X;
                input_svg_move_base_y = (float)e.Y;
                input_svg_move_x = input_svg_move_base_x;
                input_svg_move_y = input_svg_move_base_y;
                Thread_svg_input = new Thread(new ThreadStart(SvgMoveObject_input_svg));
                Thread_svg_input.Priority = ThreadPriority.Highest;
                Thread_svg_input.Start();
            }
        }

        private void input_svg_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                input_svg_move_x = (float)e.X;
                input_svg_move_y = (float)e.Y;
            }
        }

        private void input_svg_MouseUp(object sender, MouseEventArgs e)
        {
            if (Thread_svg_input != null)
            {
                Thread_svg_input.Abort();
            }
        }

        public void SvgMoveObject_input_svg()
        {
            float x = 0, y = 0;
            SvgTransDocument SvgTrans = new SvgTransDocument();

            int temp_x, temp_y;
            int padding_x_temp, padding_y_temp;

            while (input_move_check)
            {
                x = input_svg_move_x - input_svg_move_base_x;
                y = input_svg_move_y - input_svg_move_base_y;

                temp_x = (int)x;
                temp_y = (int)y;

                padding_x_temp = InputImage_X_Padding;
                padding_y_temp = InputImage_Y_Padding;

                InputImage_X_Padding = InputImage_X_Padding + (int)x;
                InputImage_Y_Padding = InputImage_Y_Padding + (int)y;

                if (InputImage_X_Padding < 0) InputImage_X_Padding = 0;
                if (InputImage_X_Padding > (display_x_size - InputImageWidth)) InputImage_X_Padding = (display_x_size - InputImageWidth);
                if (InputImage_Y_Padding < 0) InputImage_Y_Padding = 0;
                if (InputImage_Y_Padding > (display_y_size - InputImageHeight)) InputImage_Y_Padding = (display_y_size - InputImageHeight);

                if ((padding_x_temp + temp_x) < 0) temp_x = (-1) * padding_x_temp;
                if ((padding_x_temp + temp_x) > (display_x_size - InputImageWidth)) temp_x = (display_x_size - InputImageWidth) - padding_x_temp;
                if ((padding_y_temp + temp_y) < 0) temp_y = (-1) * padding_y_temp;
                if ((padding_y_temp + temp_y) > (display_y_size - InputImageHeight)) temp_y = (display_y_size - InputImageHeight) - padding_y_temp;

                DefaultControlLineUpdate(temp_x, temp_y);

                //  Don't Remove
                //XmlNode root = main_xml.DocumentElement;
                //foreach (XmlNode childNode in root.ChildNodes)
                //{
                //    SvgTrans.SvgMove(childNode, x, y);
                //}

                main_svgdoc = SvgDocument.Open(main_xml);

                svgDraw_move_input(main_svgdoc);

                if (input_control_line_display.Checked)
                {
                    input_line_draw();
                }

                Thread.Sleep(1);
                //Thread.Sleep(10);
                //Thread.Sleep(100);

                input_svg_move_base_x = x + input_svg_move_base_x;
                input_svg_move_base_y = y + input_svg_move_base_y;

            }
        }

        private void target_svg_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Thread_svg_target != null) Thread_svg_target.Abort();

                target_svg_move_base_x = (float)e.X;
                target_svg_move_base_y = (float)e.Y;
                target_svg_move_x = target_svg_move_base_x;
                target_svg_move_y = target_svg_move_base_y;
                Thread_svg_target = new Thread(new ThreadStart(SvgMoveObject_target_svg));
                Thread_svg_target.Priority = ThreadPriority.Highest;
                Thread_svg_target.Start();
            }
        }

        private void target_svg_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                target_svg_move_x = (float)e.X;
                target_svg_move_y = (float)e.Y;
            }
        }

        private void target_svg_MouseUp(object sender, MouseEventArgs e)
        {
            if (Thread_svg_target != null)
            {
                Thread_svg_target.Abort();
            }
        }

        public void SvgMoveObject_target_svg()
        {
            float x = 0, y = 0;
            SvgTransDocument SvgTrans = new SvgTransDocument();

            int temp_x, temp_y;
            int padding_x_temp, padding_y_temp;

            while (target_move_check)
            {
                x = target_svg_move_x - target_svg_move_base_x;
                y = target_svg_move_y - target_svg_move_base_y;

                temp_x = (int)x;
                temp_y = (int)y;

                padding_x_temp = TargetImage_X_Padding;
                padding_y_temp = TargetImage_Y_Padding;

                TargetImage_X_Padding = TargetImage_X_Padding + (int)x;
                TargetImage_Y_Padding = TargetImage_Y_Padding + (int)y;

                if (TargetImage_X_Padding < 0) TargetImage_X_Padding = 0;
                if (TargetImage_X_Padding > (display_x_size - TargetImageWidth)) TargetImage_X_Padding = (display_x_size - TargetImageWidth);
                if (TargetImage_Y_Padding < 0) TargetImage_Y_Padding = 0;
                if (TargetImage_Y_Padding > (display_y_size - TargetImageHeight)) TargetImage_Y_Padding = (display_y_size - TargetImageHeight);

                if ((padding_x_temp + temp_x) < 0) temp_x = (-1) * padding_x_temp;
                if ((padding_x_temp + temp_x) > (display_x_size - TargetImageWidth)) temp_x = (display_x_size - TargetImageWidth) - padding_x_temp;
                if ((padding_y_temp + temp_y) < 0) temp_y = (-1) * padding_y_temp;
                if ((padding_y_temp + temp_y) > (display_y_size - TargetImageHeight)) temp_y = (display_y_size - TargetImageHeight) - padding_y_temp;

                DestControlLineUpdate(temp_x, temp_y);

                //  Don't Remove
                //XmlNode root = target_xml.DocumentElement;
                //foreach (XmlNode childNode in root.ChildNodes)
                //{
                //    SvgTrans.SvgMove(childNode, x, y);
                //}

                target_svgdoc = SvgDocument.Open(target_xml);

                svgDraw_move_target(target_svgdoc);

                if (target_control_line_display.Checked)
                {
                    target_line_draw();
                }

                Thread.Sleep(1);
                //Thread.Sleep(10);
                //Thread.Sleep(100);

                target_svg_move_base_x = x + target_svg_move_base_x;
                target_svg_move_base_y = y + target_svg_move_base_y;

            }
        }

        private void morphing_result_save_btn_Click(object sender, EventArgs e)
        {
            XmlDocument save_xml = new XmlDocument();
            
            SaveFileDialog Morph_saveFileDialog = new SaveFileDialog();
            Morph_saveFileDialog.Filter = "SVG file|*.svg";
            Morph_saveFileDialog.Title = "Save an Morphing SVG File";
            Morph_saveFileDialog.ShowDialog();

            if (Morph_saveFileDialog.FileName != "")
            {
                if (morphing_image_source_step == 0)
                {
                    save_xml.Load("temp00");
                    save_xml.Save(Morph_saveFileDialog.FileName);
                }
                else if (morphing_image_source_step == 1)
                {
                    save_xml.Load("temp01");
                    save_xml.Save(Morph_saveFileDialog.FileName);
                }
                else if (morphing_image_source_step == 2)
                {
                    save_xml.Load("temp02");
                    save_xml.Save(Morph_saveFileDialog.FileName);
                }
                else if (morphing_image_source_step == 3)
                {
                    save_xml.Load("temp03");
                    save_xml.Save(Morph_saveFileDialog.FileName);
                }
                else if (morphing_image_source_step == 4)
                {
                    save_xml.Load("temp04");
                    save_xml.Save(Morph_saveFileDialog.FileName);
                }
                else if (morphing_image_source_step == 5)
                {
                    save_xml.Load("temp05");
                    save_xml.Save(Morph_saveFileDialog.FileName);
                }
                else if (morphing_image_source_step == 6)
                {
                    save_xml.Load("temp06");
                    save_xml.Save(Morph_saveFileDialog.FileName);
                }
                else if (morphing_image_source_step == 7)
                {
                    save_xml.Load("temp07");
                    save_xml.Save(Morph_saveFileDialog.FileName);
                }
                else if (morphing_image_source_step == 8)
                {
                    save_xml.Load("temp08");
                    save_xml.Save(Morph_saveFileDialog.FileName);
                }
                else if (morphing_image_source_step == 9)
                {
                    save_xml.Load("temp09");
                    save_xml.Save(Morph_saveFileDialog.FileName);
                }
                else if (morphing_image_source_step == 10)
                {
                    save_xml.Load("temp10");
                    save_xml.Save(Morph_saveFileDialog.FileName);
                }
            }
            
        }

        private void InputControlLineEdit_btn_Click(object sender, EventArgs e)
        {
            setbutton(input_image_move_btn, false);
            input_control_line_display.Checked = false;
            target_control_line_display.Checked = false;

            //Form ContrLineEdit = new ControlLineEdit();
            //ContrLineEdit.Owner = this;
            //ContrLineEdit.Show();

            Form ContrLineEdit = new ControlLineEdit(this);
            ContrLineEdit.Show();
            
        }

        private void paramInitBtn_Click(object sender, EventArgs e)
        {
            aValuetextBox.Text = "0.001";
            bValuetextBox.Text = "2.0";
            pValuetextBox.Text = "0.75";
        }


    }
}
