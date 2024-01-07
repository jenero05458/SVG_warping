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
//using System.Text.RegularExpressions;

public struct my_xml_node_format
{
    public string m_node_path_name;
    public string m_tag_name;
    public string m_value;
}

namespace SVG_Morph
{
    public partial class ControlLineEdit : Form
    {
        public XmlDocument display_xml = new XmlDocument();
        public SvgDocument display_svgdoc;

        public int display_x_size = 360;
        public int display_y_size = 360;

        control_line[] contr_lines = new control_line[100];

        int ImageWidth = 0;
        int ImageHeight = 0;

        SVG_Morph ParentForm;

        int X_Padding = 0;
        int Y_Padding = 0;

        int svgmorph_X_Padding = 0;
        int svgmorph_Y_Padding = 0;

        int ContrLineNum;

        int CurrentSelectedLineNum = 0;
        int NearPoint = 0;    //  1=Px, 2=Qx

        public bool move_btn_check = false;

        private Thread Thread_svg_image;

        public int svg_image_move_x, svg_image_move_y, svg_image_move_base_x, svg_image_move_base_y;

        string InputFileName;
        string InputFilePath;

        int face_direction;

        public ControlLineEdit(SVG_Morph form)
        {
            this.ParentForm = form;

            InitializeComponent();

            SVGDataRead();

            SVGDisplay();

            //ContrLineDisplay();
            listbox_init();

            setbutton(MoveLine_btn, false);
        }

        public void SVGDataRead()
        {
            display_xml = ParentForm.main_xml;

            ContrLineNum = ParentForm.NumDefaultContrLine;

            for (int i = 0; i < ContrLineNum; i++)
            {
                contr_lines[i].Px = ParentForm.default_contr_lines_Backup[i].Px;
                contr_lines[i].Py = ParentForm.default_contr_lines_Backup[i].Py;
                contr_lines[i].Qx = ParentForm.default_contr_lines_Backup[i].Qx;
                contr_lines[i].Qy = ParentForm.default_contr_lines_Backup[i].Qy;
            }

            InputFileName = ParentForm.global_input_file_name;
            FileName_edit.Text = InputFileName;
            InputFilePath = ParentForm.global_input_file_path;
            face_direction = ParentForm.face_direct;
            if (face_direction == 0) ContrLineVer_edit.Text = "4";
            else if (face_direction == 1) ContrLineVer_edit.Text = "2";
            svgmorph_X_Padding = ParentForm.InputImage_X_Padding;
            svgmorph_Y_Padding = ParentForm.InputImage_Y_Padding;
        }

        public void SVGDisplay()
        {
            display_svgdoc = SvgDocument.Open(display_xml);

            X_Padding = (int)(display_x_size - display_svgdoc.Width) / 2;
            Y_Padding = (int)(display_y_size - display_svgdoc.Height) / 2;

            ImageWidth = (int)display_svgdoc.Width;
            ImageHeight = (int)display_svgdoc.Height;

            Bitmap background = new Bitmap(360, 360);
            Bitmap InputSVG = display_svgdoc.Draw();
            Bitmap result = new Bitmap(background.Width, background.Height);
            Graphics g = Graphics.FromImage(result);
            g.DrawImageUnscaled(background, 0, 0);
            g.Flush();
            g.DrawImageUnscaled(InputSVG, X_Padding, Y_Padding);
            g.Flush();
            g.DrawRectangle(Pens.Black, X_Padding, Y_Padding, ImageWidth, ImageHeight);
            g.Flush();

            /////////
            //Pen p = new Pen(Color.Blue, 2);
            Pen p = new Pen(Color.Orange, 2);
            int cnt;
            for (cnt = 0; cnt < ContrLineNum; cnt++)
            {
                contr_lines[cnt].Px = contr_lines[cnt].Px + X_Padding;
                contr_lines[cnt].Py = contr_lines[cnt].Py + Y_Padding;

                contr_lines[cnt].Qx = contr_lines[cnt].Qx + X_Padding;
                contr_lines[cnt].Qy = contr_lines[cnt].Qy + Y_Padding;

                if (contr_lines[cnt].Px < X_Padding) contr_lines[cnt].Px = X_Padding;
                if (contr_lines[cnt].Px > (X_Padding + ImageWidth)) contr_lines[cnt].Px = X_Padding + ImageWidth;

                if (contr_lines[cnt].Py < Y_Padding) contr_lines[cnt].Py = Y_Padding;
                if (contr_lines[cnt].Py > (Y_Padding + ImageHeight)) contr_lines[cnt].Py = Y_Padding + ImageHeight;

                if (contr_lines[cnt].Qx < X_Padding) contr_lines[cnt].Qx = X_Padding;
                if (contr_lines[cnt].Qx > (X_Padding + ImageWidth)) contr_lines[cnt].Qx = X_Padding + ImageWidth;

                if (contr_lines[cnt].Qy < Y_Padding) contr_lines[cnt].Qy = Y_Padding;
                if (contr_lines[cnt].Qy > (Y_Padding + ImageHeight)) contr_lines[cnt].Qy = Y_Padding + ImageHeight;
            }

            for (cnt = 0; cnt < ContrLineNum; cnt++)
            {
                Point point1 = new Point(contr_lines[cnt].Px, contr_lines[cnt].Py);
                Point point2 = new Point(contr_lines[cnt].Qx, contr_lines[cnt].Qy);
                g.DrawLine(p, point1, point2);
            }
            /////////

            svg_image.Image = result;
        }

        public void SVG_re_disp()
        {
            display_svgdoc = SvgDocument.Open(display_xml);

            Bitmap background = new Bitmap(360, 360);
            Bitmap InputSVG = display_svgdoc.Draw();
            Bitmap result = new Bitmap(background.Width, background.Height);
            Graphics g = Graphics.FromImage(result);
            g.DrawImageUnscaled(background, 0, 0);
            g.Flush();
            g.DrawImageUnscaled(InputSVG, X_Padding, Y_Padding);
            g.Flush();
            g.DrawRectangle(Pens.Black, X_Padding, Y_Padding, ImageWidth, ImageHeight);
            g.Flush();

            /////////
            //Pen blue = new Pen(Color.Blue, 2);
            Pen orange = new Pen(Color.Orange, 2);
            Pen red = new Pen(Color.Red, 3);

            int cnt;

            for (cnt = 0; cnt < ContrLineNum; cnt++)
            {
                Point point1 = new Point(contr_lines[cnt].Px, contr_lines[cnt].Py);
                Point point2 = new Point(contr_lines[cnt].Qx, contr_lines[cnt].Qy);

                if (cnt == CurrentSelectedLineNum) g.DrawLine(red, point1, point2);
                else g.DrawLine(orange, point1, point2);

            }
            /////////

            svg_image.Image = result;
        }


        //public void ContrLineDisplay()
        //{
        //    Graphics g = svg_image.CreateGraphics();
        //    Pen p = new Pen(Color.Blue, 2);
        //    int cnt;
        //    for (cnt = 0; cnt < ContrLineNum; cnt++)
        //    {
        //        contr_lines[cnt].Px = contr_lines[cnt].Px + X_Padding;
        //        contr_lines[cnt].Py = contr_lines[cnt].Py + Y_Padding;

        //        contr_lines[cnt].Qx = contr_lines[cnt].Qx + X_Padding;
        //        contr_lines[cnt].Qy = contr_lines[cnt].Qy + Y_Padding;
        //    }

        //    for (cnt = 0; cnt < ContrLineNum; cnt++)
        //    {
        //        Point point1 = new Point(contr_lines[cnt].Px, contr_lines[cnt].Py);
        //        Point point2 = new Point(contr_lines[cnt].Qx, contr_lines[cnt].Qy);
        //        g.DrawLine(p, point1, point2);
        //    }
        //}

        public void listbox_init()
        {
            int cnt;
            int temp;
            string str_temp;
            for (cnt = 0; cnt < ContrLineNum; cnt++)
            {
                temp = cnt + 1;
                str_temp = "Line " + temp.ToString();
                point_list_box.Items.Add(str_temp);
            }
        }

        private void point_list_box_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            CurrentSelectedLineNum = point_list_box.SelectedIndex;

            SVG_re_disp();
        }

        private void MoveLine_btn_Click(object sender, EventArgs e)
        {
            if (move_btn_check)
            {
                move_btn_check = false;
                setbutton(MoveLine_btn, false);
            }
            else
            {
                move_btn_check = true;
                setbutton(MoveLine_btn, true);
            }
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

        private void svg_image_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Thread_svg_image != null) Thread_svg_image.Abort();

                svg_image_move_base_x = e.X;
                svg_image_move_base_y = e.Y;
                ////////
                double p_dist = Math.Sqrt(Math.Pow(svg_image_move_base_x - contr_lines[CurrentSelectedLineNum].Px, 2) + Math.Pow(svg_image_move_base_y - contr_lines[CurrentSelectedLineNum].Py, 2));
                double q_dist = Math.Sqrt(Math.Pow(svg_image_move_base_x - contr_lines[CurrentSelectedLineNum].Qx, 2) + Math.Pow(svg_image_move_base_y - contr_lines[CurrentSelectedLineNum].Qy, 2));
                if (p_dist <= q_dist) NearPoint = 1;
                else NearPoint = 2;
                ////////
                svg_image_move_x = svg_image_move_base_x;
                svg_image_move_y = svg_image_move_base_x;
                Thread_svg_image = new Thread(new ThreadStart(SvgMoveObject_svg_image));
                Thread_svg_image.Priority = ThreadPriority.Highest;
                Thread_svg_image.Start();
            }
        }

        private void svg_image_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                svg_image_move_x = e.X;
                svg_image_move_y = e.Y;
            }
        }

        private void svg_image_MouseUp(object sender, MouseEventArgs e)
        {
            if (Thread_svg_image != null)
            {
                Thread_svg_image.Abort();
            }
        }

        public void SvgMoveObject_svg_image()
        {
            int x = 0, y = 0;

            while (move_btn_check)
            {
                x = svg_image_move_x - svg_image_move_base_x;
                y = svg_image_move_y - svg_image_move_base_y;

                if (NearPoint == 1)    //  1=P, 2=Q
                {
                    contr_lines[CurrentSelectedLineNum].Px = contr_lines[CurrentSelectedLineNum].Px + x;
                    contr_lines[CurrentSelectedLineNum].Py = contr_lines[CurrentSelectedLineNum].Py + y;

                    if (contr_lines[CurrentSelectedLineNum].Px < X_Padding) contr_lines[CurrentSelectedLineNum].Px = X_Padding;
                    if (contr_lines[CurrentSelectedLineNum].Px > (X_Padding + ImageWidth)) contr_lines[CurrentSelectedLineNum].Px = X_Padding + ImageWidth;
                    if (contr_lines[CurrentSelectedLineNum].Py < Y_Padding) contr_lines[CurrentSelectedLineNum].Py = Y_Padding;
                    if (contr_lines[CurrentSelectedLineNum].Py > (Y_Padding + ImageHeight)) contr_lines[CurrentSelectedLineNum].Py = Y_Padding + ImageHeight;
                }
                else
                {
                    contr_lines[CurrentSelectedLineNum].Qx = contr_lines[CurrentSelectedLineNum].Qx + x;
                    contr_lines[CurrentSelectedLineNum].Qy = contr_lines[CurrentSelectedLineNum].Qy + y;

                    if (contr_lines[CurrentSelectedLineNum].Qx < X_Padding) contr_lines[CurrentSelectedLineNum].Qx = X_Padding;
                    if (contr_lines[CurrentSelectedLineNum].Qx > (X_Padding + ImageWidth)) contr_lines[CurrentSelectedLineNum].Qx = X_Padding + ImageWidth;
                    if (contr_lines[CurrentSelectedLineNum].Qy < Y_Padding) contr_lines[CurrentSelectedLineNum].Qy = Y_Padding;
                    if (contr_lines[CurrentSelectedLineNum].Qy > (Y_Padding + ImageHeight)) contr_lines[CurrentSelectedLineNum].Qy = Y_Padding + ImageHeight;
                }

                SVG_re_disp();

                Thread.Sleep(1);
                //Thread.Sleep(10);
                //Thread.Sleep(100);

                svg_image_move_base_x = x + svg_image_move_base_x;
                svg_image_move_base_y = y + svg_image_move_base_y;

            }
        }

        private void SaveLine_btn_Click(object sender, EventArgs e)
        {
            int cnt;

            for (cnt = 0; cnt < ContrLineNum; cnt++)
            {
                contr_lines[cnt].Px = contr_lines[cnt].Px - X_Padding;
                contr_lines[cnt].Py = contr_lines[cnt].Py - Y_Padding;
                contr_lines[cnt].Qx = contr_lines[cnt].Qx - X_Padding;
                contr_lines[cnt].Qy = contr_lines[cnt].Qy - Y_Padding;
            }

            string face_direct_str = null;

            if (face_direction == 0) face_direct_str = "front";
            else if (face_direction == 1) face_direct_str = "side";

            XmlDocument streaming_list_xml_handle = new XmlDocument();
            XmlProcessingInstruction w_xml_title = streaming_list_xml_handle.CreateProcessingInstruction("xml", "version='1.0' encoding='utf-8'");
            streaming_list_xml_handle.AppendChild(w_xml_title);

            my_xml_node_format[] w_node_arr = new my_xml_node_format[6];

            w_node_arr[0].m_tag_name = "morphing_control_data";
            w_node_arr[0].m_node_path_name = "";
            w_node_arr[0].m_value = "";

            w_node_arr[1].m_tag_name = "name";
            w_node_arr[1].m_node_path_name = "morphing_control_data";
            //w_node_arr[1].m_value = InputFileName + ".clp";
            w_node_arr[1].m_value = FileName_edit.Text + ".clp";

            w_node_arr[2].m_tag_name = "direction";
            w_node_arr[2].m_node_path_name = "morphing_control_data";
            w_node_arr[2].m_value = face_direct_str;

            w_node_arr[3].m_tag_name = "line_num";
            w_node_arr[3].m_node_path_name = "morphing_control_data";
            w_node_arr[3].m_value = ContrLineNum.ToString();

            w_node_arr[4].m_tag_name = "version";
            w_node_arr[4].m_node_path_name = "morphing_control_data";
            w_node_arr[4].m_value = ContrLineVer_edit.Text;

            w_node_arr[5].m_tag_name = "lines";
            w_node_arr[5].m_node_path_name = "morphing_control_data";
            w_node_arr[5].m_value = "";

            XmlElement w_parent = null;
            XmlElement w_scen = null;
            for (int i = 0; i < 6; i++)
            {
                if (w_node_arr[i].m_tag_name == "") continue;

                XmlElement w_element = streaming_list_xml_handle.CreateElement(w_node_arr[i].m_tag_name);

                if (i == 0)
                {
                    streaming_list_xml_handle.AppendChild(w_element);
                    w_parent = w_element;
                }
                else
                {
                    w_parent = streaming_list_xml_handle.SelectSingleNode(w_node_arr[i].m_node_path_name) as XmlElement;
                    if (w_parent != null)
                    {
                        w_parent.AppendChild(w_element);
                        w_element.InnerXml = w_node_arr[i].m_value;
                        // ------------------
                        w_parent = w_element;
                        // ------------------
                    }

                    if (i == 5)
                    {
                        w_scen = w_element;
                        for (int j = 0; j < ContrLineNum; j++)
                        {
                            w_element = streaming_list_xml_handle.CreateElement("line");
                            w_scen.AppendChild(w_element);
                            w_parent = w_element;

                            w_element = streaming_list_xml_handle.CreateElement("num");
                            w_parent.AppendChild(w_element);
                            //w_element.InnerXml = contr_lines[j].num.ToString();
                            w_element.InnerXml = j.ToString();

                            w_element = streaming_list_xml_handle.CreateElement("px");
                            w_parent.AppendChild(w_element);
                            w_element.InnerXml = contr_lines[j].Px.ToString();

                            w_element = streaming_list_xml_handle.CreateElement("py");
                            w_parent.AppendChild(w_element);
                            w_element.InnerXml = contr_lines[j].Py.ToString();

                            w_element = streaming_list_xml_handle.CreateElement("qx");
                            w_parent.AppendChild(w_element);
                            w_element.InnerXml = contr_lines[j].Qx.ToString();

                            w_element = streaming_list_xml_handle.CreateElement("qy");
                            w_parent.AppendChild(w_element);
                            w_element.InnerXml = contr_lines[j].Qy.ToString();

                            w_element = streaming_list_xml_handle.CreateElement("parts_name");
                            w_parent.AppendChild(w_element);
                            w_element.InnerXml = "";
                        }
                    }
                }
            }

            InputFileName = InputFileName + ".clp";
            //InputFileName = FileName_edit.Text + ".clp";


            XmlWriterSettings settings = new XmlWriterSettings();
            settings.NewLineChars = "\r\n";
            settings.Indent = true;
            XmlWriter w_annotationFile = XmlWriter.Create((InputFileName), settings);
            streaming_list_xml_handle.WriteTo(w_annotationFile);
            w_annotationFile.Flush();
            w_annotationFile.Close();

        }

        private void Ok_btn_Click(object sender, EventArgs e)
        {
            int cnt;

            for (cnt = 0; cnt < ContrLineNum; cnt++)
            {
                contr_lines[cnt].Px = contr_lines[cnt].Px - X_Padding;
                contr_lines[cnt].Py = contr_lines[cnt].Py - Y_Padding;
                contr_lines[cnt].Qx = contr_lines[cnt].Qx - X_Padding;
                contr_lines[cnt].Qy = contr_lines[cnt].Qy - Y_Padding;

                contr_lines[cnt].Px = contr_lines[cnt].Px + svgmorph_X_Padding;
                contr_lines[cnt].Py = contr_lines[cnt].Py + svgmorph_Y_Padding;
                contr_lines[cnt].Qx = contr_lines[cnt].Qx + svgmorph_X_Padding;
                contr_lines[cnt].Qy = contr_lines[cnt].Qy + svgmorph_Y_Padding;
            }

            for (cnt = 0; cnt < ContrLineNum; cnt++)
            {
                ParentForm.default_contr_lines[cnt].Px = contr_lines[cnt].Px;
                ParentForm.default_contr_lines[cnt].Py = contr_lines[cnt].Py;
                ParentForm.default_contr_lines[cnt].Qx = contr_lines[cnt].Qx;
                ParentForm.default_contr_lines[cnt].Qy = contr_lines[cnt].Qy;
            }

            Close();
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
