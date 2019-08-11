using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XTD
{
    public partial class Form1 : Form
    {
        double R = 6371000.0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.richTextBox1.Text = XTD(33, 120, 34, 121, 33.98389, 120.95361).ToString();//test
            //this.richTextBox1.Text = XTD(50.06639, -5.71472, 58.64389, -3.07, 0, 0).ToString();//right
            //this.richTextBox1.Text = XTD(53.3205, -1.7297, 53.1883, 0.1333, 0, 0).ToString();//right
            //PointF m = midPoint(50.06639,58.64389,-5.71472, -3.07);
            //this.richTextBox1.Text = m.X.ToString() + "," + m.Y.ToString();
        }
        /// <summary>
        /// 计算地球上两个地点之间的距离
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon1"></param>
        /// <param name="lon2"></param>
        /// <returns></returns>
        public double distance(double lat1, double lat2, double lon1, double lon2)
        {
            double lat_1 = toRadians(lat1);
            double lat_2 = toRadians(lat2);
            double deltaLat = toRadians(lat2-lat1);
            double deltaLon = toRadians(lon2-lon1);
            double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) + Math.Cos(lat_1) * Math.Cos(lat_2) * Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        
        }
        /// <summary>
        /// 角度转换成弧度
        /// </summary>
        /// <param name="angdeg"></param>
        /// <returns></returns>

        public static double toRadians(double angdeg)
        {
            return angdeg / 180.0 * Math.PI;
        }
        public static double toDegrees(double angrad)
        {
            return angrad * 180.0 / Math.PI;
        }
        public double wrap360(double degrees)
        {
            if (0 <= degrees && degrees < 360) return degrees; // avoid rounding due to arithmetic ops if within range
            return (degrees % 360 + 360) % 360; // sawtooth wave p:360, a:360

        }
        public double bearing(double lat1, double lat2, double lon1, double lon2)
        {
            double lat1_ = toRadians(lat1);
            double lat2_ = toRadians(lat2);
            double deltaLon = toRadians(lon2 - lon1);
            double y = Math.Sin(deltaLon) * Math.Cos(lat2_);
            double x = Math.Cos(lat1_) * Math.Sin(lat2_) -Math.Sin(lat1_) * Math.Cos(lat2_) * Math.Cos(deltaLon);
            double brng = toDegrees(Math.Atan2(y, x));
            return wrap360(brng);
        
        }
        /// <summary>
        /// 计算偏航距离。
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2"></param>
        /// <param name="lat3"></param>
        /// <param name="lon3"></param>
        /// <returns></returns>
        public double XTD(double lat1, double lon1, double lat2, double lon2,double lat3,double lon3)
        {
            double dd13 = distance(lat1, lat3, lon1, lon3)/R;
            double brng13 = toRadians(bearing(lat1, lat3, lon1, lon3));
            double brng12 = toRadians(bearing(lat1, lat2, lon1, lon2));
            double xtd=Math.Asin(Math.Sin(dd13)*Math.Sin(brng13-brng12))*R;
            //return distance(lat1, lat2, lon1, lon2);
            //return brng12;
            //return bearing(lat1, lat2, lon1, lon2);
            return xtd;//﹢为右偏航 -为左偏航
        
        
        }
        /// <summary>
        /// 计算两个经纬度点之间的中点。
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon1"></param>
        /// <param name="lon2"></param>
        /// <returns></returns>
        public PointF midPoint(double lat1, double lat2, double lon1, double lon2)
        {
            double lat1_ = toRadians(lat1);
            double lat2_ = toRadians(lat2);
            double lon1_=toRadians(lon1);
            double deltaLon = toRadians(lon2-lon1);

            double  Bx = Math.Cos(lat2_) * Math.Cos(deltaLon);
            double  By = Math.Cos(lat2_) * Math.Sin(deltaLon);

            double  x = Math.Sqrt((Math.Cos(lat1_) + Bx) * (Math.Cos(lat1_) + Bx) + By * By);
            double y = Math.Sin(lat1_) + Math.Sin(lat2_);
            PointF p=new PointF((float)(toDegrees(Math.Atan2(y, x))),(float)(toDegrees((lon1_ + Math.Atan2(By, Math.Cos(lat1_) + Bx)))));
            return p;        
        }
    }
}
