using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;

namespace Svg
{
    public class SvgTransDocument
    {

        private XmlDocument svgnewdoc;                         //new SVG
        private XmlDocument m_svgdoc;                          //main SVG
        private XmlDocument s_svgdoc;                          //sub SVG
        private XmlNode target_node;                           //target node
        private XmlNamespaceManager mgr;                       //s_svgdoc  XmlNamespaceManager
        private XmlNamespaceManager newmgr;                    //svgnewdoc XmlNamespaceManager
        private float rate;                                    //rate  
        private string id;                                     //element ID 
        private string[] nodenames = new string[] { "path", "circle", "line", "rect", "ellipse", "polygon", "polyline" };
        private string command = @"([A-z])";                   //command
        private string format = "####0.#######";               //format
        private char[] pointSeparator = new char[] { ',' };    //point


 
        #region svgMovePath()
        /*
        //move path
        public void svgMovePath()
        {
            string move_id;
            string m_d, s_d, n_d, o_d;
            string t_x, t_y;
            float x_dt = 0, y_dt = 0;
            string chk_upper = @"([A-Z])";
            XmlNodeList n_nodelist;
            XmlNode m_node, s_node;

            XmlNodeList main_nodelist = m_svgdoc.GetElementsByTagName("path");
            XmlNodeList sub_nodelist = s_svgdoc.GetElementsByTagName("path");

            //rate에 따른 id check
            if (rate > 0.5)
            {
                n_nodelist = sub_nodelist;
            }
            else
            {
                n_nodelist = main_nodelist;
            }

            for (int i = 0; i < n_nodelist.Count; i++)
            {
                move_id = n_nodelist[i].Attributes["id"].Value;

                if (!(move_id.IndexOf("^") > 0)) continue;

                //calc distance//
                string st_id = move_id.Substring(move_id.IndexOf("^") + 1);

                if (st_id.Substring(0, 4).ToUpper() != "JOIN") continue;

                m_node = m_svgdoc.SelectSingleNode("//svg:path[@id='" + st_id + "']", mgr);
                s_node = s_svgdoc.SelectSingleNode("//svg:path[@id='" + st_id + "']", mgr);

                if (m_node != null && s_node != null)
                {
                    m_d = m_node.Attributes["d"].Value;
                    s_d = s_node.Attributes["d"].Value;

                    string[] m_points = Regex.Split(m_d, command);
                    string[] s_points = Regex.Split(s_d, command);

                    //시작점 기준
                    if (char.IsLetter(m_points[1], 0) && m_points[1] == "M" && s_points[1] == "M")
                    {
                        string[] m_p = m_points[2].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);
                        string[] s_p = s_points[2].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                        t_x = getCalcPoint(m_p[0], s_p[0]); //X
                        t_y = getCalcPoint(m_p[1], s_p[1]); //Y

                        if (rate > 0.5)
                        {
                            x_dt = float.Parse(t_x) - float.Parse(s_p[0]);
                            y_dt = float.Parse(t_y) - float.Parse(s_p[1]);
                        }
                        else
                        {
                            x_dt = float.Parse(t_x) - float.Parse(m_p[0]);
                            y_dt = float.Parse(t_y) - float.Parse(m_p[1]);
                        }

                    }
                }
                else
                {
                    return;
                }

                //move//
                XmlNode n_node = svgnewdoc.SelectSingleNode("//svg:path[@id='" + move_id + "']", newmgr);

                o_d = n_node.Attributes["d"].Value;

                //command별 points
                string[] o_points = Regex.Split(o_d, command);

                //d연산
                //command와 points분리 및 point연산
                n_d = "";
                for (int k = 0; k < o_points.Length; k++)
                {
                    if (o_points[k] == "") continue;

                    //command check                        
                    if (char.IsLetter(o_points[k], 0) && Regex.IsMatch(o_points[k], chk_upper)) //절대좌표
                    {
                        n_d = n_d + o_points[k];
                        k++;
                        if (k < o_points.Length)
                        {
                            string[] o_p = o_points[k].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                            for (int m = 0; m < o_p.Length; m++)
                            {
                                if (m < o_p.Length - 1)
                                {
                                    if (m % 2 == 0)
                                    {
                                        n_d = n_d + (float.Parse(o_p[m]) + x_dt).ToString() + ",";
                                    }
                                    else
                                    {
                                        n_d = n_d + (float.Parse(o_p[m]) + y_dt).ToString() + ",";
                                    }
                                }
                                else
                                {
                                    if (m % 2 == 0)
                                    {
                                        n_d = n_d + (float.Parse(o_p[m]) + x_dt).ToString();
                                    }
                                    else
                                    {
                                        n_d = n_d + (float.Parse(o_p[m]) + y_dt).ToString();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        n_d = n_d + o_points[k];
                    }
                }
                n_node.Attributes["d"].Value = n_d;
            }
        }
        */
        #endregion


        #region svgAdddocument()
        //add svg
        public XmlDocument svgAdddocument(XmlDocument add_svg, XmlDocument target_svg)
        {
            string add_id = "", join_id = "Joint-", jointarget_id = "", joinadd_id = "";
            bool joincheck = false, findcheck = false;
            string chk_upper = @"([A-Z])";
            string x1, x2, y1, y2;
            float join_x = 0, join_y = 0;
            XmlNode targetnode = null, joinnode = null;

            string xmlns = target_svg.DocumentElement.Attributes["xmlns"].Value;
            XmlNamespaceManager t_mgr = new XmlNamespaceManager(target_svg.NameTable);
            t_mgr.AddNamespace("svg", xmlns);

            string xmlns1 = add_svg.DocumentElement.Attributes["xmlns"].Value;
            XmlNamespaceManager t_mgr1 = new XmlNamespaceManager(add_svg.NameTable);
            t_mgr1.AddNamespace("svg", xmlns1);

            for (int n = 0; n < add_svg.DocumentElement.ChildNodes.Count; n++)
            {
                joinnode = add_svg.DocumentElement.ChildNodes[n];
                if (joinnode != null)
                {
                    try
                    {
                        joinadd_id = joinnode.Attributes["id"].Value;
                    }
                    catch
                    {
                        continue;
                    }

                    if (joinadd_id.IndexOf(join_id) < 0)
                    {
                        continue;
                    }
                }

                for (int m = 0; m < target_svg.DocumentElement.ChildNodes.Count; m++)
                {
                    targetnode = target_svg.DocumentElement.ChildNodes[m];
                    if (targetnode != null)
                    {
                        try
                        {
                            jointarget_id = targetnode.Attributes["id"].Value;
                        }
                        catch
                        {
                            continue;
                        }
                        if (jointarget_id.IndexOf(joinadd_id) > -1)
                        {
                            findcheck = true;
                            break;
                        }
                    }
                }

                if (findcheck)
                {
                    break;
                }
            }

            if (findcheck == false)
            {
                joinnode = null;
                targetnode = null;
            }


            if (joinnode != null && targetnode != null)
            {
                string add_d = joinnode.Attributes["d"].Value;
                string tgt_d = targetnode.Attributes["d"].Value;

                add_d = add_d.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("&#xD;&#xA;", "");
                tgt_d = tgt_d.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("&#xD;&#xA;", "");

                string[] add_points = Regex.Split(add_d, command);
                string[] tgt_points = Regex.Split(tgt_d, command);

                if (char.IsLetter(add_points[1], 0) && add_points[1] == "M" && char.IsLetter(tgt_points[1], 0) && tgt_points[1] == "M")
                {
                    string[] a_p = add_points[2].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);
                    string[] t_p = tgt_points[2].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                    join_x = float.Parse(t_p[0]) - float.Parse(a_p[0]);
                    join_y = float.Parse(t_p[1]) - float.Parse(a_p[1]);

                    joincheck = true;
                }
                else
                {
                    join_x = 0;
                    join_y = 0;
                    joincheck = false;
                }
            }

            for (int i = 0; i < add_svg.DocumentElement.ChildNodes.Count; i++)
            {
                XmlNode addnode = target_svg.ImportNode(add_svg.DocumentElement.ChildNodes[i], true);

                if (joincheck)
                {
                    if (addnode != null)
                    {
                        switch (addnode.Name)
                        {
                            case "path":
                                string new_d = "";
                                string path_d = addnode.Attributes["d"].Value;
                                path_d = path_d.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("&#xD;&#xA;", "");

                                string[] points = Regex.Split(path_d, command);

                                for (int k = 0; k < points.Length; k++)
                                {
                                    if (points[k] == "") continue;

                                    //command check                        
                                    if (char.IsLetter(points[k], 0) && Regex.IsMatch(points[k], chk_upper)) //절대좌표
                                    {
                                        new_d = new_d + points[k];
                                        // "V" 인경우는 Y좌표계만 처리됨
                                        if (points[k] == "V")
                                        {
                                            k++;
                                            new_d = new_d + (float.Parse(points[k].Replace(",", "")) + join_y).ToString();
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
                                                        new_d = new_d + (float.Parse(d_point[m]) + join_x).ToString() + ",";
                                                    }
                                                    else
                                                    {
                                                        new_d = new_d + (float.Parse(d_point[m]) + join_y).ToString() + ",";
                                                    }
                                                }
                                                else
                                                {
                                                    if (m % 2 == 0)
                                                    {
                                                        new_d = new_d + (float.Parse(d_point[m]) + join_x).ToString();
                                                    }
                                                    else
                                                    {
                                                        new_d = new_d + (float.Parse(d_point[m]) + join_y).ToString();
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
                                addnode.Attributes["d"].Value = new_d;

                                break;
                            case "circle":
                                x1 = addnode.Attributes["cx"].Value;
                                y1 = addnode.Attributes["cy"].Value;

                                addnode.Attributes["cx"].Value = (float.Parse(x1) + join_x).ToString();
                                addnode.Attributes["cy"].Value = (float.Parse(y1) + join_y).ToString();

                                break;
                            case "line":
                                x1 = addnode.Attributes["x1"].Value;
                                y1 = addnode.Attributes["y1"].Value;
                                x2 = addnode.Attributes["x2"].Value;
                                y2 = addnode.Attributes["y2"].Value;

                                addnode.Attributes["x1"].Value = (float.Parse(x1) + join_x).ToString();
                                addnode.Attributes["y1"].Value = (float.Parse(y1) + join_y).ToString();
                                addnode.Attributes["x2"].Value = (float.Parse(x2) + join_x).ToString();
                                addnode.Attributes["y2"].Value = (float.Parse(y2) + join_y).ToString();

                                break;
                            case "rect":
                                x1 = addnode.Attributes["x"].Value;
                                y1 = addnode.Attributes["y"].Value;

                                addnode.Attributes["x"].Value = (float.Parse(x1) + join_x).ToString();
                                addnode.Attributes["y"].Value = (float.Parse(y1) + join_y).ToString();

                                break;
                            case "ellipse":
                                x1 = addnode.Attributes["cx"].Value;
                                y1 = addnode.Attributes["cy"].Value;

                                addnode.Attributes["cx"].Value = (float.Parse(x1) + join_x).ToString();
                                addnode.Attributes["cy"].Value = (float.Parse(y1) + join_y).ToString();

                                break;
                            case "polygon":
                            case "polyline":
                                string pg_points, pg_new_points = "";

                                pg_points = addnode.Attributes["points"].Value;
                                pg_points = pg_points.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("&#xD;&#xA;", "");

                                string[] pg_point = pg_points.Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                                for (int k = 0; k < pg_point.Length; k++)
                                {
                                    if (k < pg_point.Length - 1)
                                    {
                                        if (k % 2 == 0)
                                        {
                                            pg_new_points = pg_new_points + (float.Parse(pg_point[k]) + join_x).ToString() + ",";
                                        }
                                        else
                                        {
                                            pg_new_points = pg_new_points + (float.Parse(pg_point[k]) + join_y).ToString() + ",";
                                        }
                                    }
                                    else
                                    {
                                        if (k % 2 == 0)
                                        {
                                            pg_new_points = pg_new_points + (float.Parse(pg_point[k]) + join_x).ToString();
                                        }
                                        else
                                        {
                                            pg_new_points = pg_new_points + (float.Parse(pg_point[k]) + join_y).ToString();
                                        }
                                    }
                                }
                                addnode.Attributes["points"].Value = pg_new_points;

                                break;
                        }
                    }
                }

                try
                {
                    add_id = addnode.Attributes["id"].Value;
                }
                catch
                {
                    continue;
                }

                for (int j = 0; j < nodenames.Length; j++)
                {
                    targetnode = target_svg.SelectSingleNode("//svg:" + nodenames[j] + "[@id='" + add_id + "']", t_mgr);
                    if (targetnode != null) break;
                }

                if (targetnode == null)
                {
                    target_svg.DocumentElement.AppendChild(addnode);
                }
                else
                {
                    target_svg.DocumentElement.ReplaceChild(addnode, targetnode);
                }
            }

            return target_svg;

        }
        #endregion


        #region svgDeldocument()
        //Del svg
        public XmlDocument svgDeldocument(XmlDocument add_svg, XmlDocument target_svg)
        {
            string del_id = "";
            XmlNode targetnode = null;
            string xmlns = target_svg.DocumentElement.Attributes["xmlns"].Value;
            XmlNamespaceManager t_mgr = new XmlNamespaceManager(target_svg.NameTable);
            t_mgr.AddNamespace("svg", xmlns);

            for (int i = 0; i < add_svg.DocumentElement.ChildNodes.Count; i++)
            {
                XmlNode addnode = target_svg.ImportNode(add_svg.DocumentElement.ChildNodes[i], true);

                try
                {
                    del_id = addnode.Attributes["id"].Value;
                }
                catch
                {
                    continue;
                }

                if (del_id.ToUpper().IndexOf("JOINT-") > -1) continue;

                for (int j = 0; j < nodenames.Length; j++)
                {
                    targetnode = target_svg.SelectSingleNode("//svg:" + nodenames[j] + "[@id='" + del_id + "']", t_mgr);
                    if (targetnode != null) break;
                }

                if (targetnode != null)
                {
                    target_svg.DocumentElement.RemoveChild(targetnode);
                }
            }

            return target_svg;

        }
        #endregion


        #region svgDeleteObject()
        public XmlDocument svgDeleteObject(XmlDocument target_svg, string deleteobject)
        {
            string del_id = "";
            XmlNode targetnode = null;

            for (int i = 0; i < target_svg.DocumentElement.ChildNodes.Count; i++)
            {
                targetnode = target_svg.DocumentElement.ChildNodes[i];

                try
                {
                    del_id = targetnode.Attributes["id"].Value;
                }
                catch
                {
                    continue;
                }

                if (del_id.Substring(0, deleteobject.Length).ToUpper() != deleteobject.ToUpper()) continue;

                try
                {
                    target_svg.DocumentElement.RemoveChild(targetnode);
                }
                catch
                {
                    continue;
                }
            }

            return target_svg;

        }
        #endregion

        
        #region setScaledocument
        public XmlDocument setScaledocument(XmlDocument scale_svg, XmlDocument target_svg, float scale, string scaleobject)
        {
            string scale_id = "";
            string chk_upper = @"([A-Z])";
            string chk_lower = @"([a-z])";
            string x1, x2, y1, y2, rx, ry;
            float move_dx = 0, move_dy = 0;
            XmlNode targetnode = null;
            string xmlns = target_svg.DocumentElement.Attributes["xmlns"].Value;
            XmlNamespaceManager t_mgr = new XmlNamespaceManager(target_svg.NameTable);
            t_mgr.AddNamespace("svg", xmlns);

            for (int i = 0; i < scale_svg.DocumentElement.ChildNodes.Count; i++)
            {
                XmlNode scalenode = scale_svg.DocumentElement.ChildNodes[i];

                try
                {
                    scale_id = scalenode.Attributes["id"].Value;

                    if (scaleobject != "")
                    {
                        if (scale_id.Substring(0, scaleobject.Length).ToUpper() != scaleobject.ToUpper()) continue;
                    }
                }
                catch
                {
                    continue;
                }
                for (int j = 0; j < nodenames.Length; j++)
                {
                    targetnode = target_svg.SelectSingleNode("//svg:" + nodenames[j] + "[@id='" + scale_id + "']", t_mgr);
                    if (targetnode != null) break;
                }

                if (targetnode != null)
                {
                    switch (targetnode.Name)
                    {
                        case "path":
                            string new_d = "";
                            string path_d = scalenode.Attributes["d"].Value;
                            path_d = path_d.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("&#xD;&#xA;", "");

                            string[] points = Regex.Split(path_d, command);

                            for (int k = 0; k < points.Length; k++)
                            {
                                if (points[k] == "") continue;

                                //command check                        
                                if (char.IsLetter(points[k], 0) && Regex.IsMatch(points[k], chk_upper)) //절대좌표
                                {
                                    new_d = new_d + points[k];

                                    // "V" 인경우는 Y좌표계만 처리됨
                                    if (points[k] == "V")
                                    {
                                        k++;
                                        new_d = new_d + ((float.Parse(points[k].Replace(",", "")) * scale) - move_dy).ToString();
                                        continue;
                                    }

                                    k++;
                                    if (k < points.Length)
                                    {
                                        string[] d_point = points[k].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                                        for (int m = 0; m < d_point.Length; m++)
                                        {
                                            if (points[k - 1] == "M" && move_dx == 0 && move_dy == 0)
                                            {
                                                move_dx = (float.Parse(d_point[0]) * scale) - float.Parse(d_point[0]);
                                                move_dy = (float.Parse(d_point[1]) * scale) - float.Parse(d_point[1]);
                                            }


                                            if (m < d_point.Length - 1)
                                            {
                                                if (m % 2 == 0)
                                                {
                                                    new_d = new_d + ((float.Parse(d_point[m]) * scale) - move_dx).ToString() + ",";
                                                }
                                                else
                                                {
                                                    new_d = new_d + ((float.Parse(d_point[m]) * scale) - move_dy).ToString() + ",";
                                                }
                                            }
                                            else
                                            {
                                                if (m % 2 == 0)
                                                {
                                                    new_d = new_d + ((float.Parse(d_point[m]) * scale) - move_dx).ToString();
                                                }
                                                else
                                                {
                                                    new_d = new_d + ((float.Parse(d_point[m]) * scale) - move_dy).ToString();
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (char.IsLetter(points[k], 0) && Regex.IsMatch(points[k], chk_lower)) //상대좌표
                                {
                                    new_d = new_d + points[k];

                                    k++;
                                    if (k < points.Length)
                                    {
                                        string[] d_point = points[k].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                                        for (int m = 0; m < d_point.Length; m++)
                                        {
                                            if (m < d_point.Length - 1)
                                            {
                                                new_d = new_d + (float.Parse(d_point[m]) * scale).ToString() + ",";
                                            }
                                            else
                                            {
                                                new_d = new_d + (float.Parse(d_point[m]) * scale).ToString();
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    new_d = new_d + points[k];
                                }
                            }
                            targetnode.Attributes["d"].Value = new_d;

                            break;
                        case "circle":
                            x1 = scalenode.Attributes["cx"].Value;
                            y1 = scalenode.Attributes["cy"].Value;
                            rx = scalenode.Attributes["r"].Value;

                            if (move_dx == 0 && move_dy == 0)
                            {
                                move_dx = (float.Parse(x1) * scale) - float.Parse(x1);
                                move_dy = (float.Parse(y1) * scale) - float.Parse(y1);
                            }

                            targetnode.Attributes["cx"].Value = ((float.Parse(x1) * scale) - move_dx).ToString();
                            targetnode.Attributes["cy"].Value = ((float.Parse(y1) * scale) - move_dy).ToString();
                            targetnode.Attributes["r"].Value = (float.Parse(rx) * scale).ToString();

                            break;
                        case "line":
                            x1 = scalenode.Attributes["x1"].Value;
                            y1 = scalenode.Attributes["y1"].Value;
                            x2 = scalenode.Attributes["x2"].Value;
                            y2 = scalenode.Attributes["y2"].Value;

                            if (move_dx == 0 && move_dy == 0)
                            {
                                move_dx = (float.Parse(x1) * scale) - float.Parse(x1);
                                move_dy = (float.Parse(y1) * scale) - float.Parse(y1);
                            }

                            targetnode.Attributes["x1"].Value = ((float.Parse(x1) * scale) - move_dx).ToString();
                            targetnode.Attributes["y1"].Value = ((float.Parse(y1) * scale) - move_dy).ToString();
                            targetnode.Attributes["x2"].Value = ((float.Parse(x2) * scale) - move_dx).ToString();
                            targetnode.Attributes["y2"].Value = ((float.Parse(y2) * scale) - move_dy).ToString();

                            break;
                        case "rect":
                            x1 = scalenode.Attributes["x"].Value;
                            y1 = scalenode.Attributes["y"].Value;
                            rx = scalenode.Attributes["width"].Value;
                            ry = scalenode.Attributes["height"].Value;

                            if (move_dx == 0 && move_dy == 0)
                            {
                                move_dx = (float.Parse(x1) * scale) - float.Parse(x1);
                                move_dy = (float.Parse(y1) * scale) - float.Parse(y1);
                            }

                            targetnode.Attributes["x"].Value = ((float.Parse(x1) * scale) - move_dx).ToString();
                            targetnode.Attributes["y"].Value = ((float.Parse(y1) * scale) - move_dy).ToString();
                            targetnode.Attributes["width"].Value = (float.Parse(rx) * scale).ToString();
                            targetnode.Attributes["height"].Value = (float.Parse(ry) * scale).ToString();

                            break;
                        case "ellipse":
                            x1 = scalenode.Attributes["cx"].Value;
                            y1 = scalenode.Attributes["cy"].Value;
                            rx = scalenode.Attributes["rx"].Value;
                            ry = scalenode.Attributes["ry"].Value;

                            if (move_dx == 0 && move_dy == 0)
                            {
                                move_dx = (float.Parse(x1) * scale) - float.Parse(x1);
                                move_dy = (float.Parse(y1) * scale) - float.Parse(y1);
                            }

                            targetnode.Attributes["cx"].Value = ((float.Parse(x1) * scale) - move_dx).ToString();
                            targetnode.Attributes["cy"].Value = ((float.Parse(y1) * scale) - move_dy).ToString();
                            targetnode.Attributes["rx"].Value = (float.Parse(rx) * scale).ToString();
                            targetnode.Attributes["ry"].Value = (float.Parse(ry) * scale).ToString();

                            break;
                        case "polygon":
                        case "polyline":
                            string pg_points, pg_new_points = "";

                            pg_points = scalenode.Attributes["points"].Value;
                            pg_points = pg_points.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("&#xD;&#xA;", "");

                            string[] pg_point = pg_points.Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                            if (move_dx == 0 && move_dy == 0)
                            {
                                move_dx = (float.Parse(pg_point[0]) * scale) - float.Parse(pg_point[0]);
                                move_dy = (float.Parse(pg_point[1]) * scale) - float.Parse(pg_point[1]);
                            }

                            for (int k = 0; k < pg_point.Length; k++)
                            {
                                if (k < pg_point.Length - 1)
                                {
                                    if (k % 2 == 0)
                                    {
                                        pg_new_points = pg_new_points + (float.Parse(pg_point[k]) * scale).ToString() + ",";
                                    }
                                    else
                                    {
                                        pg_new_points = pg_new_points + (float.Parse(pg_point[k]) * scale).ToString() + ",";
                                    }
                                }
                                else
                                {
                                    if (k % 2 == 0)
                                    {
                                        pg_new_points = pg_new_points + (float.Parse(pg_point[k]) * scale).ToString();
                                    }
                                    else
                                    {
                                        pg_new_points = pg_new_points + (float.Parse(pg_point[k]) * scale).ToString();
                                    }
                                }
                            }
                            targetnode.Attributes["points"].Value = pg_new_points;

                            break;
                    }
                }
            }

            return target_svg;

        }

        #endregion


        #region svgColorchange
        public XmlDocument svgColorchange(XmlDocument target_svg, string fill, string colorobject)
        {
            string color_id = "";
            XmlNode targetnode = null;

            for (int i = 0; i < target_svg.DocumentElement.ChildNodes.Count; i++)
            {
                targetnode = target_svg.DocumentElement.ChildNodes[i];

                try
                {
                    color_id = targetnode.Attributes["id"].Value;
                }
                catch
                {
                    continue;
                }

                if (color_id.Substring(0, colorobject.Length).ToUpper() != colorobject.ToUpper()) continue;

                try
                {
                    targetnode.Attributes["fill"].Value = fill;
                }
                catch
                {
                    continue;
                }
            }

            return target_svg;
        }
        #endregion


        #region getCalcFill(string fill1, string fill2)
        //calc point
        public string getCalcFill(string fill1, string fill2)
        {
            int m_R, m_G, m_B, s_R, s_G, s_B, n_R, n_G, n_B;
            string n_fill;

            m_R = Convert.ToInt32(fill1.Substring(1, 2), 16);
            m_G = Convert.ToInt32(fill1.Substring(3, 2), 16);
            m_B = Convert.ToInt32(fill1.Substring(5, 2), 16);
            s_R = Convert.ToInt32(fill2.Substring(1, 2), 16);
            s_G = Convert.ToInt32(fill2.Substring(3, 2), 16);
            s_B = Convert.ToInt32(fill2.Substring(5, 2), 16);

            n_R = (int)((float)m_R + (((float)s_R - (float)m_R) * rate));
            n_G = (int)((float)m_G + (((float)s_G - (float)m_G) * rate));
            n_B = (int)((float)m_B + (((float)s_B - (float)m_B) * rate));

            n_fill = "#" + Convert.ToString(n_R, 16).ToUpper();
            n_fill = n_fill + Convert.ToString(n_G, 16).ToUpper();
            n_fill = n_fill + Convert.ToString(n_B, 16).ToUpper();

            return n_fill;
        }
        #endregion


        #region getFill(r,g,b)
        public string getFill(int r, int g, int b)
        {
            string fill = "", s_r = "", s_g = "", s_b = "";

            if (Convert.ToString(r, 16).ToUpper().Length == 1)
            {
                s_r = "0" + Convert.ToString(r, 16).ToUpper();
            }
            else
            {
                s_r = Convert.ToString(r, 16).ToUpper();
            }
            if (Convert.ToString(g, 16).ToUpper().Length == 1)
            {
                s_g = "0" + Convert.ToString(g, 16).ToUpper();
            }
            else
            {
                s_g = Convert.ToString(g, 16).ToUpper();
            }
            if (Convert.ToString(b, 16).ToUpper().Length == 1)
            {
                s_b = "0" + Convert.ToString(b, 16).ToUpper();
            }
            else
            {
                s_b = Convert.ToString(b, 16).ToUpper();
            }

            fill = "#" + s_r + s_g + s_b;

            return fill;
        }
        #endregion







        #region SvgScale(완료)

        public void SvgScale(XmlNode basenode, XmlNode scalenode, float scale)
        {
            XmlNode findnode;
            XmlNode target_node = basenode;

            SvgScaleNode(basenode, scalenode, scale);
          
            foreach (XmlNode childNode in target_node.ChildNodes)
            {
                if (childNode.Value == "\r\n") continue;

                try
                {
                    findnode = scalenode.SelectSingleNode("//*[@id='" + childNode.Attributes["id"].Value + "']");                    
                }
                catch
                {
                    return;
                }

                SvgScale(childNode, findnode, scale);
            }
        }

        public void SvgScaleNode(XmlNode basenode, XmlNode scalenode, float scale)
        {
            string scale_id = "";
            string chk_upper = @"([A-Z])";
            string chk_lower = @"([a-z])";
            string x1, x2, y1, y2, rx, ry;
            float move_dx = 0, move_dy = 0;

            try
            {
                scale_id = basenode.Attributes["id"].Value;
            }
            catch
            {
                return;
            }

            switch (basenode.Name)
            {
                case "path":
                    string new_d = "";
                    string path_d = basenode.Attributes["d"].Value;
                    path_d = path_d.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "");

                    string[] points = Regex.Split(path_d, command);

                    for (int k = 0; k < points.Length; k++)
                    {
                        if (points[k] == "") continue;

                        //command check                        
                        if (char.IsLetter(points[k], 0) && Regex.IsMatch(points[k], chk_upper)) //절대좌표
                        {
                            new_d = new_d + points[k];

                            // "V" 인경우는 Y좌표계만 처리됨
                            if (points[k] == "V")
                            {
                                k++;
                                new_d = new_d + ((float.Parse(points[k].Replace(",", "")) * scale) - move_dy).ToString();
                                continue;
                            }

                            k++;
                            if (k < points.Length)
                            {
                                string[] d_point = points[k].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                                for (int m = 0; m < d_point.Length; m++)
                                {
                                    if (points[k - 1] == "M" && move_dx == 0 && move_dy == 0)
                                    {
                                        move_dx = (float.Parse(d_point[0]) * scale) - float.Parse(d_point[0]);
                                        move_dy = (float.Parse(d_point[1]) * scale) - float.Parse(d_point[1]);
                                    }


                                    if (m < d_point.Length - 1)
                                    {
                                        if (m % 2 == 0)
                                        {
                                            new_d = new_d + ((float.Parse(d_point[m]) * scale) - move_dx).ToString() + ",";
                                        }
                                        else
                                        {
                                            new_d = new_d + ((float.Parse(d_point[m]) * scale) - move_dy).ToString() + ",";
                                        }
                                    }
                                    else
                                    {
                                        if (m % 2 == 0)
                                        {
                                            new_d = new_d + ((float.Parse(d_point[m]) * scale) - move_dx).ToString();
                                        }
                                        else
                                        {
                                            new_d = new_d + ((float.Parse(d_point[m]) * scale) - move_dy).ToString();
                                        }
                                    }
                                }
                            }
                        }
                        else if (char.IsLetter(points[k], 0) && Regex.IsMatch(points[k], chk_lower)) //상대좌표
                        {
                            new_d = new_d + points[k];

                            k++;
                            if (k < points.Length)
                            {
                                string[] d_point = points[k].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                                for (int m = 0; m < d_point.Length; m++)
                                {
                                    if (m < d_point.Length - 1)
                                    {
                                        new_d = new_d + (float.Parse(d_point[m]) * scale).ToString() + ",";
                                    }
                                    else
                                    {
                                        new_d = new_d + (float.Parse(d_point[m]) * scale).ToString();
                                    }
                                }
                            }

                        }
                        else
                        {
                            new_d = new_d + points[k];
                        }
                    }
                    scalenode.Attributes["d"].Value = new_d;

                    break;
                case "circle":
                    x1 = basenode.Attributes["cx"].Value;
                    y1 = basenode.Attributes["cy"].Value;
                    rx = basenode.Attributes["r"].Value;

                    if (move_dx == 0 && move_dy == 0)
                    {
                        move_dx = (float.Parse(x1) * scale) - float.Parse(x1);
                        move_dy = (float.Parse(y1) * scale) - float.Parse(y1);
                    }

                    scalenode.Attributes["cx"].Value = ((float.Parse(x1) * scale) - move_dx).ToString();
                    scalenode.Attributes["cy"].Value = ((float.Parse(y1) * scale) - move_dy).ToString();
                    scalenode.Attributes["r"].Value = (float.Parse(rx) * scale).ToString();

                    break;
                case "line":
                    x1 = basenode.Attributes["x1"].Value;
                    y1 = basenode.Attributes["y1"].Value;
                    x2 = basenode.Attributes["x2"].Value;
                    y2 = basenode.Attributes["y2"].Value;

                    if (move_dx == 0 && move_dy == 0)
                    {
                        move_dx = (float.Parse(x1) * scale) - float.Parse(x1);
                        move_dy = (float.Parse(y1) * scale) - float.Parse(y1);
                    }

                    scalenode.Attributes["x1"].Value = ((float.Parse(x1) * scale) - move_dx).ToString();
                    scalenode.Attributes["y1"].Value = ((float.Parse(y1) * scale) - move_dy).ToString();
                    scalenode.Attributes["x2"].Value = ((float.Parse(x2) * scale) - move_dx).ToString();
                    scalenode.Attributes["y2"].Value = ((float.Parse(y2) * scale) - move_dy).ToString();

                    break;
                case "rect":
                    x1 = basenode.Attributes["x"].Value;
                    y1 = basenode.Attributes["y"].Value;
                    rx = basenode.Attributes["width"].Value;
                    ry = basenode.Attributes["height"].Value;

                    if (move_dx == 0 && move_dy == 0)
                    {
                        move_dx = (float.Parse(x1) * scale) - float.Parse(x1);
                        move_dy = (float.Parse(y1) * scale) - float.Parse(y1);
                    }

                    scalenode.Attributes["x"].Value = ((float.Parse(x1) * scale) - move_dx).ToString();
                    scalenode.Attributes["y"].Value = ((float.Parse(y1) * scale) - move_dy).ToString();
                    scalenode.Attributes["width"].Value = (float.Parse(rx) * scale).ToString();
                    scalenode.Attributes["height"].Value = (float.Parse(ry) * scale).ToString();

                    break;
                case "ellipse":
                    x1 = basenode.Attributes["cx"].Value;
                    y1 = basenode.Attributes["cy"].Value;
                    rx = basenode.Attributes["rx"].Value;
                    ry = basenode.Attributes["ry"].Value;

                    if (move_dx == 0 && move_dy == 0)
                    {
                        move_dx = (float.Parse(x1) * scale) - float.Parse(x1);
                        move_dy = (float.Parse(y1) * scale) - float.Parse(y1);
                    }

                    scalenode.Attributes["cx"].Value = ((float.Parse(x1) * scale) - move_dx).ToString();
                    scalenode.Attributes["cy"].Value = ((float.Parse(y1) * scale) - move_dy).ToString();
                    scalenode.Attributes["rx"].Value = (float.Parse(rx) * scale).ToString();
                    scalenode.Attributes["ry"].Value = (float.Parse(ry) * scale).ToString();

                    break;
                case "polygon":
                case "polyline":
                    string pg_points, pg_new_points = "";

                    pg_points = basenode.Attributes["points"].Value;
                    pg_points = pg_points.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("&#xD;&#xA;", "");

                    string[] pg_point = pg_points.Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                    if (move_dx == 0 && move_dy == 0)
                    {
                        move_dx = (float.Parse(pg_point[0]) * scale) - float.Parse(pg_point[0]);
                        move_dy = (float.Parse(pg_point[1]) * scale) - float.Parse(pg_point[1]);
                    }

                    for (int k = 0; k < pg_point.Length; k++)
                    {
                        if (k < pg_point.Length - 1)
                        {
                            if (k % 2 == 0)
                            {
                                pg_new_points = pg_new_points + (float.Parse(pg_point[k]) * scale).ToString() + ",";
                            }
                            else
                            {
                                pg_new_points = pg_new_points + (float.Parse(pg_point[k]) * scale).ToString() + ",";
                            }
                        }
                        else
                        {
                            if (k % 2 == 0)
                            {
                                pg_new_points = pg_new_points + (float.Parse(pg_point[k]) * scale).ToString();
                            }
                            else
                            {
                                pg_new_points = pg_new_points + (float.Parse(pg_point[k]) * scale).ToString();
                            }
                        }
                    }
                    scalenode.Attributes["points"].Value = pg_new_points;

                    break;
            }
        }

        #endregion

        #region SvgJointMove(완료)
        public void SvgJointMove(XmlDocument target, XmlDocument add)
        {
            float move_x, move_y;

            if (target == null || target.InnerXml.Length == 0) return;
            if (add == null || add.InnerXml.Length == 0) return;

            XmlNode add_joint = add.SelectSingleNode("//*[contains(@id,'Joint')]");
            XmlNode target_joint = target.SelectSingleNode("//*[@id='" + add_joint.Attributes["id"].Value + "']");

            if (target_joint == null || add_joint == null) return;

            string target_d = target_joint.Attributes["d"].Value;
            string add_d = add_joint.Attributes["d"].Value;

            string[] target_points = Regex.Split(target_d, command);
            string[] add_points = Regex.Split(add_d, command);

            if (char.IsLetter(target_points[1], 0) && target_points[1] == "M" && add_points[1] == "M")
            {
                string[] target_p = target_points[2].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);
                string[] add_p = add_points[2].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                move_x = float.Parse(target_p[0]) - float.Parse(add_p[0]);
                move_y = float.Parse(target_p[1]) - float.Parse(add_p[1]);

                SvgJointMoveNode(add.DocumentElement, move_x, move_y);
            }
        }

        public void SvgJointMoveNode(XmlNode add_node, float x, float y)
        {
            string chk_upper = @"([A-Z])";

            switch (add_node.Name)
            {
                case "path":
                    string add_d = add_node.Attributes["d"].Value;

                    add_d = add_d.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "");

                    //command별 points
                    string[] add_points = Regex.Split(add_d, command);

                    //d연산
                    //command와 points분리 및 point연산
                    string new_d = "";
                    for (int k = 0; k < add_points.Length; k++)
                    {
                        if (add_points[k] == "") continue;

                        //command check                        
                        if (char.IsLetter(add_points[k], 0) && Regex.IsMatch(add_points[k], chk_upper)) //절대좌표
                        {
                            new_d = new_d + add_points[k];
                            k++;
                            if (k < add_points.Length)
                            {
                                string[] add_p = add_points[k].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                                for (int m = 0; m < add_p.Length; m++)
                                {
                                    if (m < add_p.Length - 1)
                                    {
                                        if (m % 2 == 0)
                                        {
                                            new_d = new_d + (float.Parse(add_p[m]) + x).ToString() + ",";
                                        }
                                        else
                                        {
                                            new_d = new_d + (float.Parse(add_p[m]) + y).ToString() + ",";
                                        }
                                    }
                                    else
                                    {
                                        if (m % 2 == 0)
                                        {
                                            new_d = new_d + (float.Parse(add_p[m]) + x).ToString();
                                        }
                                        else
                                        {
                                            new_d = new_d + (float.Parse(add_p[m]) + y).ToString();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            new_d = new_d + add_points[k];
                        }
                    }
                    add_node.Attributes["d"].Value = new_d;
                    break;
                case "circle":
                    add_node.Attributes["cx"].Value = (float.Parse(add_node.Attributes["cx"].Value) + x).ToString();
                    add_node.Attributes["cy"].Value = (float.Parse(add_node.Attributes["cy"].Value) + y).ToString();
                    break;
                case "line":
                    add_node.Attributes["x1"].Value = (float.Parse(add_node.Attributes["x1"].Value) + x).ToString();
                    add_node.Attributes["y1"].Value = (float.Parse(add_node.Attributes["y1"].Value) + y).ToString();
                    add_node.Attributes["x2"].Value = (float.Parse(add_node.Attributes["x2"].Value) + x).ToString();
                    add_node.Attributes["y2"].Value = (float.Parse(add_node.Attributes["y2"].Value) + y).ToString();
                    break;
                case "rect":
                    add_node.Attributes["x"].Value = (float.Parse(add_node.Attributes["X"].Value) + x).ToString();
                    add_node.Attributes["y"].Value = (float.Parse(add_node.Attributes["Y"].Value) + y).ToString();  
                    break;
                case "ellipse":
                    add_node.Attributes["cx"].Value = (float.Parse(add_node.Attributes["cx"].Value) + x).ToString();
                    add_node.Attributes["cy"].Value = (float.Parse(add_node.Attributes["cy"].Value) + y).ToString();
                    add_node.Attributes["rx"].Value = (float.Parse(add_node.Attributes["rx"].Value) + x).ToString();
                    add_node.Attributes["ry"].Value = (float.Parse(add_node.Attributes["ry"].Value) + y).ToString();  
                    break;
                case "polygon":
                    string add_pgpoints = add_node.Attributes["points"].Value;
                    add_pgpoints = add_pgpoints.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "");

                    string[] add_pgpoint = add_pgpoints.Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                    string new_pgpoints = "";
                    for (int k = 0; k < add_pgpoint.Length; k++)
                    {
                        if (add_pgpoint[k] == "") continue;

                        if (k < add_pgpoint.Length - 1)
                        {
                            if (k % 2 == 0)
                            {
                                new_pgpoints = new_pgpoints + (float.Parse(add_pgpoint[k]) + x).ToString() + ",";
                            }
                            else
                            {
                                new_pgpoints = new_pgpoints + (float.Parse(add_pgpoint[k]) + y).ToString() + ",";
                            }
                        }
                        else
                        {
                            if (k % 2 == 0)
                            {
                                new_pgpoints = new_pgpoints + (float.Parse(add_pgpoint[k]) + x).ToString();
                            }
                            else
                            {
                                new_pgpoints = new_pgpoints + (float.Parse(add_pgpoint[k]) + y).ToString();
                            }
                        }
                    }

                    add_node.Attributes["points"].Value = new_pgpoints;
                    break;
                case "polyline":                    
                    string add_plpoints = add_node.Attributes["points"].Value;
                    add_plpoints = add_plpoints.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "");

                    string[] add_plpoint = add_plpoints.Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                    string new_points = "";
                    for (int k = 0; k < add_plpoint.Length; k++)
                    {
                        if (add_plpoint[k] == "") continue;

                        if (k < add_plpoint.Length - 1)
                        {
                            if (k % 2 == 0)
                            {
                                new_points = new_points + (float.Parse(add_plpoint[k]) + x).ToString() + ",";
                            }
                            else
                            {
                                new_points = new_points + (float.Parse(add_plpoint[k]) + y).ToString() + ",";
                            }
                        }
                        else
                        {
                            if (k % 2 == 0)
                            {
                                new_points = new_points + (float.Parse(add_plpoint[k]) + x).ToString();
                            }
                            else
                            {
                                new_points = new_points + (float.Parse(add_plpoint[k]) + y).ToString();
                            }
                        }
                    }

                    add_node.Attributes["points"].Value = new_points;
                    break;
            }

            foreach (XmlNode childNode in add_node.ChildNodes)
            {
                SvgJointMoveNode(childNode, x, y);
            }
        }
        #endregion

        #region SvgAdd(완료)
        public void SvgAdd(XmlDocument target, XmlDocument add)
        {
            XmlNode xml_find;

            if (target == null || target.InnerXml.Length == 0) return;

            try
            {
                SvgJointMove(target, add);
            }
            catch { }

            int nodecnt = add.DocumentElement.ChildNodes.Count;
            for (int i = 0; i < nodecnt; i++)
            {
                XmlNode addnode = target.ImportNode(add.DocumentElement.ChildNodes[i], true);

                if (addnode.Value == "\r\n") continue;

                try
                {
                    if (addnode.Attributes["id"].Value.Substring(0, 5).ToUpper() == "JOINT") continue;

                    xml_find = target.SelectSingleNode("//*[@id='" + addnode.Attributes["id"].Value + "']");
                }
                catch
                {
                    xml_find = null;
                }

                if (xml_find == null)
                {
                    target.DocumentElement.AppendChild(addnode);
                }
                else
                {
                    try
                    {
                        xml_find.ParentNode.ReplaceChild(addnode, xml_find);
                    }
                    catch { }
                }
            }
        }
        #endregion 

        #region SvgOnlyAdd(완료)
        public void SvgOnlyAdd(XmlDocument target, XmlDocument add)
        {
            XmlNode xml_find;

            if (target == null || target.InnerXml.Length == 0)
            {
                if(add != null && add.InnerXml.Length > 0)
                {
                    target.XmlResolver = null;
                    target.LoadXml(add.InnerXml);
                }
                return;
            }

            try
            {
                SvgJointMove(target, add);
            }
            catch { }

            int nodecnt = add.DocumentElement.ChildNodes.Count;
            for (int i = 0; i < nodecnt; i++)
            {
                XmlNode addnode = target.ImportNode(add.DocumentElement.ChildNodes[i], true);

                if (addnode.Value == "\r\n") continue;

                try
                {
                    if (addnode.Attributes["id"].Value.Substring(0, 5).ToUpper() == "JOINT") continue;

                    xml_find = target.SelectSingleNode("//*[@id='" + addnode.Attributes["id"].Value + "']");

                }
                catch
                {
                    xml_find = null;
                }

                if (xml_find == null)
                {
                    target.DocumentElement.AppendChild(addnode);
                }
            }
        }
        #endregion 

        #region SvgMove(완료)

        public void SvgMove(XmlNode node, float x, float y)
        {
            XmlNode target_node = node;

            SvgMoveNode(node, x, y);

            foreach (XmlNode childNode in target_node.ChildNodes)
            {
                SvgMove(childNode, x, y);
            }
        }

        public void SvgMoveNode(XmlNode node, float x, float y)
        {
            string chk_upper = @"([A-Z])";
            string chk_small = @"([a-z])";
            string x1, x2, y1, y2;
            float x_temp, y_temp;
            x_temp = 0;
            y_temp = 0;

            switch (node.Name)
            {
                case "path":
                    //    relative Coordinate -> absolute coordinate    //  by ki
                    string new_d_temp = "";
                    string ttt_str = "";
                    string path_d_temp = node.Attributes["d"].Value;
                    path_d_temp = path_d_temp.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "").Replace("\t\t", "").Replace("\t", "");
                    string[] points_temp = Regex.Split(path_d_temp, command);

                    for (int k = 0; k < points_temp.Length; k++)
                    {
                        if (points_temp[k] == "") continue;

                        if (char.IsLetter(points_temp[k], 0) && Regex.IsMatch(points_temp[k], chk_small))
                        {
                            if (points_temp[k] == "h")
                            {
                                points_temp[k] = "H";
                                new_d_temp = new_d_temp + points_temp[k];
                                k++;
                                points_temp[k] = points_temp[k].Replace(",", "");
                                x_temp = x_temp + float.Parse(points_temp[k]);
                                new_d_temp = new_d_temp + x_temp.ToString() + " ";
                            }
                            else if (points_temp[k] == "v")
                            {
                                points_temp[k] = "V";
                                new_d_temp = new_d_temp + points_temp[k];
                                k++;
                                points_temp[k] = points_temp[k].Replace(",", "");
                                y_temp = y_temp + float.Parse(points_temp[k]);
                                new_d_temp = new_d_temp + y_temp.ToString() + " ";
                            }
                            else if (points_temp[k] == "c")
                            {
                                points_temp[k] = "C";
                                new_d_temp = new_d_temp + points_temp[k];
                                k++;
                                string[] d_point = points_temp[k].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);
                                float p1_x, p1_y, p2_x, p2_y, p3_x, p3_y;
                                p1_x = float.Parse(d_point[0]) + x_temp;
                                p1_y = float.Parse(d_point[1]) + y_temp;
                                p2_x = float.Parse(d_point[2]) + x_temp;
                                p2_y = float.Parse(d_point[3]) + y_temp;
                                p3_x = float.Parse(d_point[4]) + x_temp;
                                p3_y = float.Parse(d_point[5]) + y_temp;

                                new_d_temp = new_d_temp + p1_x.ToString() + "," + p1_y.ToString() + "," + p2_x.ToString() + "," + p2_y.ToString() + "," + p3_x.ToString() + "," + p3_y.ToString() + " ";

                                x_temp = p3_x; y_temp = p3_y;
                            }
                            else if (points_temp[k] == "l")
                            {
                                points_temp[k] = "L";
                                new_d_temp = new_d_temp + points_temp[k];
                                k++;
                                string[] d_point = points_temp[k].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);
                                float p1_x, p1_y;
                                p1_x = float.Parse(d_point[0]) + x_temp;
                                p1_y = float.Parse(d_point[1]) + y_temp;
                                new_d_temp = new_d_temp + p1_x.ToString() + "," + p1_y.ToString() + " ";

                                x_temp = p1_x; y_temp = p1_y;
                            }
                            else if (points_temp[k] == "s")
                            {
                                points_temp[k] = "S";
                                new_d_temp = new_d_temp + points_temp[k];
                                k++;
                                string[] d_point = points_temp[k].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);
                                float p1_x, p1_y, p2_x, p2_y;
                                p1_x = float.Parse(d_point[0]) + x_temp;
                                p1_y = float.Parse(d_point[1]) + y_temp;
                                p2_x = float.Parse(d_point[2]) + x_temp;
                                p2_y = float.Parse(d_point[3]) + y_temp;
                                new_d_temp = new_d_temp + p1_x.ToString() + "," + p1_y.ToString() + "," + p2_x.ToString() + "," + p2_y.ToString() + " ";

                                x_temp = p2_x; y_temp = p2_y;
                            }
                            else if (points_temp[k] == "q")
                            {
                                points_temp[k] = "Q";
                                new_d_temp = new_d_temp + points_temp[k];
                                k++;
                                string[] d_point = points_temp[k].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);
                                float p1_x, p1_y, p2_x, p2_y;
                                p1_x = float.Parse(d_point[0]) + x_temp;
                                p1_y = float.Parse(d_point[1]) + y_temp;
                                p2_x = float.Parse(d_point[2]) + x_temp;
                                p2_y = float.Parse(d_point[3]) + y_temp;
                                new_d_temp = new_d_temp + p1_x.ToString() + "," + p1_y.ToString() + "," + p2_x.ToString() + "," + p2_y.ToString() + " ";

                                x_temp = p2_x; y_temp = p2_y;
                            }
                            else if (points_temp[k] == "t")
                            {
                                points_temp[k] = "T";
                                new_d_temp = new_d_temp + points_temp[k];
                                k++;
                                string[] d_point = points_temp[k].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);
                                float p1_x, p1_y;
                                p1_x = float.Parse(d_point[0]) + x_temp;
                                p1_y = float.Parse(d_point[1]) + y_temp;
                                new_d_temp = new_d_temp + p1_x.ToString() + "," + p1_y.ToString() + " ";

                                x_temp = p1_x; y_temp = p1_y;
                            }
                            else if (points_temp[k] == "a")
                            {
                                points_temp[k] = "A";
                                new_d_temp = new_d_temp + points_temp[k];
                                k++;
                                string[] d_point = points_temp[k].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);
                                float p1_x, p1_y, p2_x, p2_y, p3_x, p3_y, sweep_flag;
                                p1_x = float.Parse(d_point[0]);
                                p1_y = float.Parse(d_point[1]);
                                p2_x = float.Parse(d_point[2]);
                                p2_y = float.Parse(d_point[3]);
                                sweep_flag = float.Parse(d_point[4]);
                                p3_x = float.Parse(d_point[5]) + x_temp;
                                p3_y = float.Parse(d_point[6]) + y_temp;
                                new_d_temp = new_d_temp + p1_x.ToString() + "," + p1_y.ToString() + "," + p2_x.ToString() + "," + p2_y.ToString() + "," + sweep_flag.ToString() + "," + p3_x.ToString() + "," + p3_y.ToString() + " ";

                                x_temp = p3_x; y_temp = p3_y;
                            }
                            else
                            {
                                new_d_temp = new_d_temp + points_temp[k];
                                k++;
                                new_d_temp = new_d_temp + points_temp[k];
                            }
                        }
                        else
                        {
                            new_d_temp = new_d_temp + points_temp[k];
                            ttt_str = points_temp[k];
                            k++;
                            
                            string[] d_point = points_temp[k].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);
                            if (ttt_str == "M")
                            {
                                x_temp = float.Parse(d_point[0]);
                                y_temp = float.Parse(d_point[1]);
                            }
                            else if (ttt_str == "H")
                            {
                                x_temp = float.Parse(d_point[0]);
                            }
                            else if (ttt_str == "V")
                            {
                                y_temp = float.Parse(d_point[0]);
                            }
                            else if (ttt_str == "C")
                            {
                                x_temp = float.Parse(d_point[4]);
                                y_temp = float.Parse(d_point[5]);
                            }
                            else if (ttt_str == "L")
                            {
                                x_temp = float.Parse(d_point[0]);
                                y_temp = float.Parse(d_point[1]);
                            }
                            else if (ttt_str == "S")
                            {
                                x_temp = float.Parse(d_point[2]);
                                y_temp = float.Parse(d_point[3]);
                            }
                            else if (ttt_str == "Q")
                            {
                                x_temp = float.Parse(d_point[2]);
                                y_temp = float.Parse(d_point[3]);
                            }
                            else if (ttt_str == "T")
                            {
                                x_temp = float.Parse(d_point[0]);
                                y_temp = float.Parse(d_point[1]);
                            }
                            else if (ttt_str == "A")
                            {
                                x_temp = float.Parse(d_point[5]);
                                y_temp = float.Parse(d_point[6]);
                            }
                            new_d_temp = new_d_temp + points_temp[k];
                        }
                    }
                    node.Attributes["d"].Value = new_d_temp;

                    string new_d = "";
                    string path_d = node.Attributes["d"].Value;
                    path_d = path_d.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "").Replace("\t\t", "").Replace("\t", "");

                    string[] points = Regex.Split(path_d, command);

                    for (int k = 0; k < points.Length; k++)
                    {
                        if (points[k] == "") continue;

                        //command check                        
                        if (char.IsLetter(points[k], 0) && Regex.IsMatch(points[k], chk_upper)) //절대좌표
                        {
                            new_d = new_d + points[k];
                            // "V" 인경우는 Y좌표계만 처리됨
                            if (points[k] == "V")
                            {
                                k++;
                                new_d = new_d + (float.Parse(points[k].Replace(",", "")) + y).ToString();
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
                                            new_d = new_d + (float.Parse(d_point[m]) + x).ToString() + ",";
                                        }
                                        else
                                        {
                                            new_d = new_d + (float.Parse(d_point[m]) + y).ToString() + ",";
                                        }
                                    }
                                    else
                                    {
                                        if (m % 2 == 0)
                                        {
                                            new_d = new_d + (float.Parse(d_point[m]) + x).ToString();
                                        }
                                        else
                                        {
                                            new_d = new_d + (float.Parse(d_point[m]) + y).ToString();
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
                    x1 = node.Attributes["cx"].Value;
                    y1 = node.Attributes["cy"].Value;

                    node.Attributes["cx"].Value = (float.Parse(x1) + x).ToString();
                    node.Attributes["cy"].Value = (float.Parse(y1) + y).ToString();

                    break;
                case "line":
                    x1 = node.Attributes["x1"].Value;
                    y1 = node.Attributes["y1"].Value;
                    x2 = node.Attributes["x2"].Value;
                    y2 = node.Attributes["y2"].Value;

                    node.Attributes["x1"].Value = (float.Parse(x1) + x).ToString();
                    node.Attributes["y1"].Value = (float.Parse(y1) + y).ToString();
                    node.Attributes["x2"].Value = (float.Parse(x2) + x).ToString();
                    node.Attributes["y2"].Value = (float.Parse(y2) + y).ToString();

                    break;
                case "rect":
                    x1 = node.Attributes["x"].Value;
                    y1 = node.Attributes["y"].Value;

                    node.Attributes["x"].Value = (float.Parse(x1) + x).ToString();
                    node.Attributes["y"].Value = (float.Parse(y1) + y).ToString();

                    break;
                case "ellipse":
                    if (node.Attributes["transform"] != null) node.Attributes.Remove(node.Attributes["transform"]);    //  by ki

                    x1 = node.Attributes["cx"].Value;
                    y1 = node.Attributes["cy"].Value;

                    node.Attributes["cx"].Value = (float.Parse(x1) + x).ToString();
                    node.Attributes["cy"].Value = (float.Parse(y1) + y).ToString();

                    break;
                case "polygon":
                case "polyline":
                    string pg_points, pg_new_points = "";

                    pg_points = node.Attributes["points"].Value;
                    pg_points = pg_points.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "").Replace("\t\t", "").Replace("\t", "");

                    string[] pg_point = pg_points.Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                    for (int k = 0; k < pg_point.Length; k++)
                    {
                        if (k < pg_point.Length - 1)
                        {
                            if (k % 2 == 0)
                            {
                                pg_new_points = pg_new_points + (float.Parse(pg_point[k]) + x).ToString() + ",";
                            }
                            else
                            {
                                pg_new_points = pg_new_points + (float.Parse(pg_point[k]) + y).ToString() + ",";
                            }
                        }
                        else
                        {
                            if (k % 2 == 0)
                            {
                                pg_new_points = pg_new_points + (float.Parse(pg_point[k]) + x).ToString();
                            }
                            else
                            {
                                pg_new_points = pg_new_points + (float.Parse(pg_point[k]) + y).ToString();
                            }
                        }
                    }
                    node.Attributes["points"].Value = pg_new_points;

                    break;
            }
        }

        
        ////  backup
        //public void SvgMoveNode(XmlNode node, float x, float y)
        //{
        //    //string move_id = "";    //  by ki
        //    string chk_upper = @"([A-Z])";
        //    string x1, x2, y1, y2;

        //    //대상이동      
        //    //try    //  by ki
        //    //{
        //    //    move_id = node.Attributes["id"].Value;
        //    //}
        //    //catch
        //    //{
        //    //    return;
        //    //}
            
        //    switch (node.Name)
        //    {
        //        case "path":
                    
        //            string new_d = "";
        //            string path_d = node.Attributes["d"].Value;
        //            path_d = path_d.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "").Replace("\t\t", "");

        //            string[] points = Regex.Split(path_d, command);

        //            for (int k = 0; k < points.Length; k++)
        //            {
        //                if (points[k] == "") continue;

        //                //command check                        
        //                if (char.IsLetter(points[k], 0) && Regex.IsMatch(points[k], chk_upper)) //절대좌표
        //                {
        //                    new_d = new_d + points[k];
        //                    // "V" 인경우는 Y좌표계만 처리됨
        //                    if (points[k] == "V")
        //                    {
        //                        k++;
        //                        new_d = new_d + (float.Parse(points[k].Replace(",", "")) + y).ToString();
        //                        continue;
        //                    }

        //                    k++;
        //                    if (k < points.Length)
        //                    {
        //                        string[] d_point = points[k].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

        //                        for (int m = 0; m < d_point.Length; m++)
        //                        {
        //                            if (m < d_point.Length - 1)
        //                            {
        //                                if (m % 2 == 0)
        //                                {
        //                                    new_d = new_d + (float.Parse(d_point[m]) + x).ToString() + ",";
        //                                }
        //                                else
        //                                {
        //                                    new_d = new_d + (float.Parse(d_point[m]) + y).ToString() + ",";
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (m % 2 == 0)
        //                                {
        //                                    new_d = new_d + (float.Parse(d_point[m]) + x).ToString();
        //                                }
        //                                else
        //                                {
        //                                    new_d = new_d + (float.Parse(d_point[m]) + y).ToString();
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    new_d = new_d + points[k];
        //                }
        //            }
        //            node.Attributes["d"].Value = new_d;

        //            break;
        //        case "circle":
        //            x1 = node.Attributes["cx"].Value;
        //            y1 = node.Attributes["cy"].Value;

        //            node.Attributes["cx"].Value = (float.Parse(x1) + x).ToString();
        //            node.Attributes["cy"].Value = (float.Parse(y1) + y).ToString();

        //            break;
        //        case "line":
        //            x1 = node.Attributes["x1"].Value;
        //            y1 = node.Attributes["y1"].Value;
        //            x2 = node.Attributes["x2"].Value;
        //            y2 = node.Attributes["y2"].Value;

        //            node.Attributes["x1"].Value = (float.Parse(x1) + x).ToString();
        //            node.Attributes["y1"].Value = (float.Parse(y1) + y).ToString();
        //            node.Attributes["x2"].Value = (float.Parse(x2) + x).ToString();
        //            node.Attributes["y2"].Value = (float.Parse(y2) + y).ToString();

        //            break;
        //        case "rect":
        //            x1 = node.Attributes["x"].Value;
        //            y1 = node.Attributes["y"].Value;

        //            node.Attributes["x"].Value = (float.Parse(x1) + x).ToString();
        //            node.Attributes["y"].Value = (float.Parse(y1) + y).ToString();

        //            break;
        //        case "ellipse":
        //            if (node.Attributes["transform"] != null) node.Attributes.Remove(node.Attributes["transform"]);    //  by ki

        //            x1 = node.Attributes["cx"].Value;
        //            y1 = node.Attributes["cy"].Value;

        //            node.Attributes["cx"].Value = (float.Parse(x1) + x).ToString();
        //            node.Attributes["cy"].Value = (float.Parse(y1) + y).ToString();

        //            break;
        //        case "polygon":
        //        case "polyline":
        //            string pg_points, pg_new_points = "";

        //            pg_points = node.Attributes["points"].Value;
        //            pg_points = pg_points.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "").Replace("\t\t", "");

        //            string[] pg_point = pg_points.Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

        //            for (int k = 0; k < pg_point.Length; k++)
        //            {
        //                if (k < pg_point.Length - 1)
        //                {
        //                    if (k % 2 == 0)
        //                    {
        //                        pg_new_points = pg_new_points + (float.Parse(pg_point[k]) + x).ToString() + ",";
        //                    }
        //                    else
        //                    {
        //                        pg_new_points = pg_new_points + (float.Parse(pg_point[k]) + y).ToString() + ",";
        //                    }
        //                }
        //                else
        //                {
        //                    if (k % 2 == 0)
        //                    {
        //                        pg_new_points = pg_new_points + (float.Parse(pg_point[k]) + x).ToString();
        //                    }
        //                    else
        //                    {
        //                        pg_new_points = pg_new_points + (float.Parse(pg_point[k]) + y).ToString();
        //                    }
        //                }
        //            }
        //            node.Attributes["points"].Value = pg_new_points;

        //            break;
        //    }
        //}
        
        #endregion

        #region SvgMix(완료)
        public XmlDocument SvgMix(XmlDocument xml_doc1, XmlDocument xml_doc2, float rate)
        {
            XmlDocument xml_result = new XmlDocument();
          
            xml_result.XmlResolver = null;
  
            if (xml_doc1 == null && xml_doc2 != null) return xml_doc2;
            else if (xml_doc1 != null && xml_doc2 == null) return xml_doc1;
            else if (xml_doc1 == null && xml_doc2 == null) return xml_doc1;

            if (rate > 0.5)
            {
                xml_result.LoadXml(xml_doc2.InnerXml);
            }
            else
            {
                xml_result.LoadXml(xml_doc1.InnerXml);
            }
            
            SvgMixDoc(xml_doc1.DocumentElement, xml_doc2.DocumentElement, xml_result.DocumentElement, rate);

            return xml_result;
        }


        public void SvgMixDoc(XmlNode xml_main, XmlNode xml_sub, XmlNode xml_result, float rate)
        {
            SvgMixNode(xml_main, xml_sub, xml_result, rate);

            foreach (XmlNode childNode in xml_main.ChildNodes)
            {
                SvgMixDoc(childNode, xml_sub, xml_result, rate);
            }
        }


        public void SvgMixNode(XmlNode xml_main, XmlNode xml_sub, XmlNode xml_result, float rate)
        {
            string mix_id = "";

            try
            {
                mix_id = xml_main.Attributes["id"].Value;
                if (mix_id.Substring(0, 5).ToUpper() == "JOINT") return;
            }
            catch
            {
                return;
            }

            XmlNode xml_find = xml_sub.SelectSingleNode("//*[@id='" + mix_id + "']");
            XmlNode xml_newnode = xml_result.SelectSingleNode("//*[@id='" + mix_id + "']");

            if (xml_find == null) return;

            switch (xml_main.Name)
            {
                case "path":
                    string s_point = "", n_d = "";
                    string m_d = xml_main.Attributes["d"].Value;
                    string s_d = xml_find.Attributes["d"].Value;

                    m_d = m_d.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "");
                    s_d = s_d.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "");

                    string[] m_points = Regex.Split(m_d, command);
                    string[] s_points = Regex.Split(s_d, command);

                    for (int k = 0; k < m_points.Length; k++)
                    {
                        if (m_points[k] == "") continue;

                        if (k < s_points.Length)
                        {
                            s_point = s_points[k];
                        }
                        else
                        {
                            s_point = "";
                        }

                        if (char.IsLetter(m_points[k], 0) && s_point == m_points[k])
                        {
                            n_d = n_d + m_points[k];
                            k++;

                            if (k < m_points.Length)
                            {
                                string[] m_p = m_points[k].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);
                                string[] s_p = s_points[k].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                                for (int m = 0; m < m_p.Length; m++)
                                {
                                    if (m < m_p.Length - 1)
                                    {
                                        n_d = n_d + getCalcPoint(m_p[m], s_p[m], rate) + ",";
                                    }
                                    else
                                    {
                                        n_d = n_d + getCalcPoint(m_p[m], s_p[m], rate);
                                    }
                                }
                            }
                        }
                        else
                        {
                            n_d = n_d + m_points[k];
                            k++;
                            if (k < m_points.Length)
                            {
                                n_d = n_d + m_points[k];
                            }
                        }
                    }

                    xml_newnode.Attributes["d"].Value = n_d;

                    break;
                case "circle":
                    string m_cx, m_cy, m_r, s_cx, s_cy, s_r;

                    m_cx = xml_main.Attributes["cx"].Value;
                    m_cy = xml_main.Attributes["cy"].Value;
                    m_r = xml_main.Attributes["r"].Value;

                    s_cx = xml_find.Attributes["cx"].Value;
                    s_cy = xml_find.Attributes["cy"].Value;
                    s_r = xml_find.Attributes["r"].Value;

                    xml_newnode.Attributes["cx"].Value = getCalcPoint(m_cx, s_cx, rate);
                    xml_newnode.Attributes["cy"].Value = getCalcPoint(m_cy, s_cy, rate);
                    xml_newnode.Attributes["r"].Value = getCalcPoint(m_cy, s_cy, rate);

                    break;
                case "line":
                    string m_x1, m_y1, m_x2, m_y2, s_x1, s_y1, s_x2, s_y2;

                    m_x1 = xml_main.Attributes["x1"].Value;
                    m_y1 = xml_main.Attributes["y1"].Value;
                    m_x2 = xml_main.Attributes["x2"].Value;
                    m_y2 = xml_main.Attributes["y2"].Value;

                    s_x1 = xml_find.Attributes["x1"].Value;
                    s_y1 = xml_find.Attributes["y1"].Value;
                    s_x2 = xml_find.Attributes["x2"].Value;
                    s_y2 = xml_find.Attributes["y2"].Value;

                    xml_newnode.Attributes["x1"].Value = getCalcPoint(m_x1, s_x1, rate);
                    xml_newnode.Attributes["y1"].Value = getCalcPoint(m_y1, s_y1, rate);
                    xml_newnode.Attributes["x2"].Value = getCalcPoint(m_x2, s_x2, rate);
                    xml_newnode.Attributes["y2"].Value = getCalcPoint(m_y2, s_y2, rate);

                    break;
                case "rect":
                    string m_x, m_y, m_width, m_height, m_rx, m_ry, s_x, s_y, s_width, s_height, s_rx, s_ry;

                    m_x = xml_main.Attributes["x"].Value;
                    m_y = xml_main.Attributes["y"].Value;
                    m_width = xml_main.Attributes["width"].Value;
                    m_height = xml_main.Attributes["height"].Value;

                    //rx, ry는 없는 경우있음(모서리 라운딩)
                    m_rx = null;
                    m_ry = null;
                    try
                    {
                        m_rx = xml_main.Attributes["rx"].Value;
                        m_ry = xml_main.Attributes["ry"].Value;
                    }
                    catch { }

                    s_x = xml_find.Attributes["x"].Value;
                    s_y = xml_find.Attributes["y"].Value;
                    s_width = xml_find.Attributes["width"].Value;
                    s_height = xml_find.Attributes["height"].Value;

                    //rx, ry는 없는 경우있음(모서리 라운딩)
                    s_rx = null;
                    s_ry = null;
                    try
                    {
                        s_rx = xml_find.Attributes["rx"].Value;
                        s_ry = xml_find.Attributes["ry"].Value;
                    }
                    catch { }

                    xml_newnode.Attributes["x"].Value = getCalcPoint(m_x, s_x, rate);
                    xml_newnode.Attributes["y"].Value = getCalcPoint(m_y, s_y, rate);
                    xml_newnode.Attributes["width"].Value = getCalcPoint(m_width, s_width, rate);
                    xml_newnode.Attributes["height"].Value = getCalcPoint(m_height, s_height, rate);

                    if (m_rx != null && s_rx != null && m_ry != null && s_ry != null)
                    {
                        xml_newnode.Attributes["rx"].Value = getCalcPoint(m_rx, s_rx, rate);
                        xml_newnode.Attributes["ry"].Value = getCalcPoint(m_ry, s_ry, rate);
                    }

                    break;
                case "ellipse":
                    string m_ecx, m_ecy, m_erx, m_ery, s_ecx, s_ecy, s_erx, s_ery;

                    m_ecx = xml_main.Attributes["cx"].Value;
                    m_ecy = xml_main.Attributes["cy"].Value;
                    m_erx = xml_main.Attributes["rx"].Value;
                    m_ery = xml_main.Attributes["ry"].Value;

                    s_ecx = xml_find.Attributes["cx"].Value;
                    s_ecy = xml_find.Attributes["cy"].Value;
                    s_erx = xml_find.Attributes["rx"].Value;
                    s_ery = xml_find.Attributes["ry"].Value;

                    xml_newnode.Attributes["cx"].Value = getCalcPoint(m_ecx, s_ecx, rate);
                    xml_newnode.Attributes["cy"].Value = getCalcPoint(m_ecy, s_ecy, rate);
                    xml_newnode.Attributes["rx"].Value = getCalcPoint(m_erx, s_erx, rate);
                    xml_newnode.Attributes["ry"].Value = getCalcPoint(m_ery, s_ery, rate);

                    break;
                case "polygon":
                    string m_pgpoints, s_pgpoints, n_pgpoints;

                    n_pgpoints = "";
                    m_pgpoints = xml_main.Attributes["points"].Value;
                    s_pgpoints = xml_find.Attributes["points"].Value;

                    m_pgpoints = m_pgpoints.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "");
                    s_pgpoints = s_pgpoints.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "");

                    string[] m_pgpoint = m_pgpoints.Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);
                    string[] s_pgpoint = s_pgpoints.Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                    for (int k = 0; k < m_pgpoint.Length; k++)
                    {
                        if (m_pgpoint[k] == "") continue;

                        if (k < m_pgpoint.Length - 1)
                        {
                            n_pgpoints = n_pgpoints + getCalcPoint(m_pgpoint[k], s_pgpoint[k], rate) + ",";
                        }
                        else
                        {
                            n_pgpoints = n_pgpoints + getCalcPoint(m_pgpoint[k], s_pgpoint[k], rate);
                        }
                    }

                    xml_newnode.Attributes["points"].Value = n_pgpoints;

                    break;
                case "polyline":
                    string m_plpoints, s_plpoints, n_plpoints;

                    n_plpoints = "";
                    m_plpoints = xml_main.Attributes["points"].Value;
                    s_plpoints = xml_find.Attributes["points"].Value;

                    m_plpoints = m_plpoints.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "");
                    s_plpoints = s_plpoints.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "");

                    string[] m_plpoint = m_plpoints.Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);
                    string[] s_plpoint = s_plpoints.Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                    for (int k = 0; k < m_plpoint.Length; k++)
                    {
                        if (m_plpoint[k] == "") continue;

                        if (k < m_plpoint.Length - 1)
                        {
                            n_plpoints = n_plpoints + getCalcPoint(m_plpoint[k], s_plpoint[k], rate) + ",";
                        }
                        else
                        {
                            n_plpoints = n_plpoints + getCalcPoint(m_plpoint[k], s_plpoint[k], rate);
                        }
                    }

                    xml_newnode.Attributes["points"].Value = n_plpoints;

                    break;
            }
        }

        public string getCalcPoint(string p1, string p2, float rate)
        {
            float f_p1, f_p2;

            f_p1 = float.Parse(p1);
            f_p2 = float.Parse(p2);

            return (f_p1 + ((f_p2 - f_p1) * rate)).ToString(format);
        }

        #endregion

        #region SvgOneWayMix(완료)
        public XmlDocument SvgOneWayMix(XmlDocument xml_doc1, XmlDocument xml_doc2, float rate)
        {
            XmlDocument xml_result = new XmlDocument();

            xml_result.XmlResolver = null;

            if (xml_doc1 == null && xml_doc2 != null) return xml_doc2;
            else if (xml_doc1 != null && xml_doc2 == null) return xml_doc1;
            else if (xml_doc1 == null && xml_doc2 == null) return xml_doc1;

            xml_result.LoadXml(xml_doc1.InnerXml);

            try
            {
                SvgJointMove(xml_doc1, xml_doc2);
            }
            catch { }


            SvgOneWayMixDoc(xml_doc1.DocumentElement, xml_doc2.DocumentElement, xml_result.DocumentElement, rate);

            return xml_result;
        }


        public void SvgOneWayMixDoc(XmlNode xml_main, XmlNode xml_sub, XmlNode xml_result, float rate)
        {
            SvgOneWayMixNode(xml_main, xml_sub, xml_result, rate);

            foreach (XmlNode childNode in xml_main.ChildNodes)
            {
                SvgOneWayMixDoc(childNode, xml_sub, xml_result, rate);
            }
        }


        public void SvgOneWayMixNode(XmlNode xml_main, XmlNode xml_sub, XmlNode xml_result, float rate)
        {
            string mix_id = "";

            try
            {
                mix_id = xml_main.Attributes["id"].Value;
                if (mix_id.Substring(0, 5).ToUpper() == "JOINT") return;
            }
            catch
            {
                return;
            }

            XmlNode xml_find = xml_sub.SelectSingleNode("//*[@id='" + mix_id + "']");
            XmlNode xml_newnode = xml_result.SelectSingleNode("//*[@id='" + mix_id + "']");

            if (xml_find == null) return;

            switch (xml_main.Name)
            {
                case "path":
                    string s_point = "", n_d = "";
                    string m_d = xml_main.Attributes["d"].Value;
                    string s_d = xml_find.Attributes["d"].Value;

                    m_d = m_d.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "");
                    s_d = s_d.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "");

                    string[] m_points = Regex.Split(m_d, command);
                    string[] s_points = Regex.Split(s_d, command);

                    for (int k = 0; k < m_points.Length; k++)
                    {
                        if (m_points[k] == "") continue;

                        if (k < s_points.Length)
                        {
                            s_point = s_points[k];
                        }
                        else
                        {
                            s_point = "";
                        }

                        if (char.IsLetter(m_points[k], 0) && s_point == m_points[k])
                        {
                            n_d = n_d + m_points[k];
                            k++;

                            if (k < m_points.Length)
                            {
                                string[] m_p = m_points[k].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);
                                string[] s_p = s_points[k].Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                                for (int m = 0; m < m_p.Length; m++)
                                {
                                    if (m < m_p.Length - 1)
                                    {
                                        n_d = n_d + getCalcPoint(m_p[m], s_p[m], rate) + ",";
                                    }
                                    else
                                    {
                                        n_d = n_d + getCalcPoint(m_p[m], s_p[m], rate);
                                    }
                                }
                            }
                        }
                        else
                        {
                            n_d = n_d + m_points[k];
                            k++;
                            if (k < m_points.Length)
                            {
                                n_d = n_d + m_points[k];
                            }
                        }
                    }

                    xml_newnode.Attributes["d"].Value = n_d;

                    break;
                case "circle":
                    string m_cx, m_cy, m_r, s_cx, s_cy, s_r;

                    m_cx = xml_main.Attributes["cx"].Value;
                    m_cy = xml_main.Attributes["cy"].Value;
                    m_r = xml_main.Attributes["r"].Value;

                    s_cx = xml_find.Attributes["cx"].Value;
                    s_cy = xml_find.Attributes["cy"].Value;
                    s_r = xml_find.Attributes["r"].Value;

                    xml_newnode.Attributes["cx"].Value = getCalcPoint(m_cx, s_cx, rate);
                    xml_newnode.Attributes["cy"].Value = getCalcPoint(m_cy, s_cy, rate);
                    xml_newnode.Attributes["r"].Value = getCalcPoint(m_cy, s_cy, rate);

                    break;
                case "line":
                    string m_x1, m_y1, m_x2, m_y2, s_x1, s_y1, s_x2, s_y2;

                    m_x1 = xml_main.Attributes["x1"].Value;
                    m_y1 = xml_main.Attributes["y1"].Value;
                    m_x2 = xml_main.Attributes["x2"].Value;
                    m_y2 = xml_main.Attributes["y2"].Value;

                    s_x1 = xml_find.Attributes["x1"].Value;
                    s_y1 = xml_find.Attributes["y1"].Value;
                    s_x2 = xml_find.Attributes["x2"].Value;
                    s_y2 = xml_find.Attributes["y2"].Value;

                    xml_newnode.Attributes["x1"].Value = getCalcPoint(m_x1, s_x1, rate);
                    xml_newnode.Attributes["y1"].Value = getCalcPoint(m_y1, s_y1, rate);
                    xml_newnode.Attributes["x2"].Value = getCalcPoint(m_x2, s_x2, rate);
                    xml_newnode.Attributes["y2"].Value = getCalcPoint(m_y2, s_y2, rate);

                    break;
                case "rect":
                    string m_x, m_y, m_width, m_height, m_rx, m_ry, s_x, s_y, s_width, s_height, s_rx, s_ry;

                    m_x = xml_main.Attributes["x"].Value;
                    m_y = xml_main.Attributes["y"].Value;
                    m_width = xml_main.Attributes["width"].Value;
                    m_height = xml_main.Attributes["height"].Value;

                    //rx, ry는 없는 경우있음(모서리 라운딩)
                    m_rx = null;
                    m_ry = null;
                    try
                    {
                        m_rx = xml_main.Attributes["rx"].Value;
                        m_ry = xml_main.Attributes["ry"].Value;
                    }
                    catch { }

                    s_x = xml_find.Attributes["x"].Value;
                    s_y = xml_find.Attributes["y"].Value;
                    s_width = xml_find.Attributes["width"].Value;
                    s_height = xml_find.Attributes["height"].Value;

                    //rx, ry는 없는 경우있음(모서리 라운딩)
                    s_rx = null;
                    s_ry = null;
                    try
                    {
                        s_rx = xml_find.Attributes["rx"].Value;
                        s_ry = xml_find.Attributes["ry"].Value;
                    }
                    catch { }

                    xml_newnode.Attributes["x"].Value = getCalcPoint(m_x, s_x, rate);
                    xml_newnode.Attributes["y"].Value = getCalcPoint(m_y, s_y, rate);
                    xml_newnode.Attributes["width"].Value = getCalcPoint(m_width, s_width, rate);
                    xml_newnode.Attributes["height"].Value = getCalcPoint(m_height, s_height, rate);

                    if (m_rx != null && s_rx != null && m_ry != null && s_ry != null)
                    {
                        xml_newnode.Attributes["rx"].Value = getCalcPoint(m_rx, s_rx, rate);
                        xml_newnode.Attributes["ry"].Value = getCalcPoint(m_ry, s_ry, rate);
                    }

                    break;
                case "ellipse":
                    string m_ecx, m_ecy, m_erx, m_ery, s_ecx, s_ecy, s_erx, s_ery;

                    m_ecx = xml_main.Attributes["cx"].Value;
                    m_ecy = xml_main.Attributes["cy"].Value;
                    m_erx = xml_main.Attributes["rx"].Value;
                    m_ery = xml_main.Attributes["ry"].Value;

                    s_ecx = xml_find.Attributes["cx"].Value;
                    s_ecy = xml_find.Attributes["cy"].Value;
                    s_erx = xml_find.Attributes["rx"].Value;
                    s_ery = xml_find.Attributes["ry"].Value;

                    xml_newnode.Attributes["cx"].Value = getCalcPoint(m_ecx, s_ecx, rate);
                    xml_newnode.Attributes["cy"].Value = getCalcPoint(m_ecy, s_ecy, rate);
                    xml_newnode.Attributes["rx"].Value = getCalcPoint(m_erx, s_erx, rate);
                    xml_newnode.Attributes["ry"].Value = getCalcPoint(m_ery, s_ery, rate);

                    break;
                case "polygon":
                    string m_pgpoints, s_pgpoints, n_pgpoints;

                    n_pgpoints = "";
                    m_pgpoints = xml_main.Attributes["points"].Value;
                    s_pgpoints = xml_find.Attributes["points"].Value;

                    m_pgpoints = m_pgpoints.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "");
                    s_pgpoints = s_pgpoints.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "");

                    string[] m_pgpoint = m_pgpoints.Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);
                    string[] s_pgpoint = s_pgpoints.Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                    for (int k = 0; k < m_pgpoint.Length; k++)
                    {
                        if (m_pgpoint[k] == "") continue;

                        if (k < m_pgpoint.Length - 1)
                        {
                            n_pgpoints = n_pgpoints + getCalcPoint(m_pgpoint[k], s_pgpoint[k], rate) + ",";
                        }
                        else
                        {
                            n_pgpoints = n_pgpoints + getCalcPoint(m_pgpoint[k], s_pgpoint[k], rate);
                        }
                    }

                    xml_newnode.Attributes["points"].Value = n_pgpoints;

                    break;
                case "polyline":
                    string m_plpoints, s_plpoints, n_plpoints;

                    n_plpoints = "";
                    m_plpoints = xml_main.Attributes["points"].Value;
                    s_plpoints = xml_find.Attributes["points"].Value;

                    m_plpoints = m_plpoints.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "");
                    s_plpoints = s_plpoints.Replace(" ", ",").Replace(",-", "-").Replace("-", ",-").Replace("\r\n\t", "").Replace("\r\n", "").Replace("&#xD;&#xA;", "");

                    string[] m_plpoint = m_plpoints.Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);
                    string[] s_plpoint = s_plpoints.Split(pointSeparator, StringSplitOptions.RemoveEmptyEntries);

                    for (int k = 0; k < m_plpoint.Length; k++)
                    {
                        if (m_plpoint[k] == "") continue;

                        if (k < m_plpoint.Length - 1)
                        {
                            n_plpoints = n_plpoints + getCalcPoint(m_plpoint[k], s_plpoint[k], rate) + ",";
                        }
                        else
                        {
                            n_plpoints = n_plpoints + getCalcPoint(m_plpoint[k], s_plpoint[k], rate);
                        }
                    }

                    xml_newnode.Attributes["points"].Value = n_plpoints;

                    break;
            }
        }

        #endregion


    }
}
