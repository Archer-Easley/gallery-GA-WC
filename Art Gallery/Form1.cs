using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsPolygonGenerator
{
    public partial class Form1 : Form
    {
        List<Point> points = new List<Point>();
        Map map = new Map();
        Random r = new Random();
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            tb_numPoints.Text = "27";
            panel1.BackColor = Color.White;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            createPolygon();
            panel1.Invalidate();
            map.Solve(points);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = panel1.CreateGraphics();

            for (int i = 0; i < points.Count; i++)
            {
                g.DrawEllipse(Pens.Black, points[i].X, points[i].Y, 5, 5);
            }

            //draw the lines
            for (int i = 0; i < points.Count - 1; i++)
            {
                g.DrawLine(Pens.Black, points[i], points[i + 1]);
            }
        }

        private void createPolygon()
        {
            points.Clear();
            int NumPoints = int.Parse(tb_numPoints.Text);
            int startX = panel1.Location.X;
            int startY = (panel1.Location.Y + panel1.Size.Height) / 2;
            int endX = panel1.Size.Width - panel1.Location.X;
            int rangeX = endX - startX;
            int rangeY = startY;
            int division = rangeX / ((NumPoints / 2));

            //add first point
            Point temp = new Point();
            temp.X = startX;
            temp.Y = startY;
            points.Add(temp);

            if (NumPoints < 3)
            {
                MessageBox.Show("Number must be larger than 3");
                return;
            }

            //add points on top half
            for (int i = 0; i < NumPoints / 2; i++)
            {
                temp = new Point();
                temp.X = startX + ((i + 1) * division);
                temp.Y = startY - r.Next(rangeY);
                points.Add(temp);
            }

            int numTopPoints = points.Count;

            division = rangeX / (NumPoints - numTopPoints);

            //add bottom half of line
            for (int i = numTopPoints; i < NumPoints; i++)
            {
                if (i == NumPoints - 1)
                {
                    temp = new Point();
                    temp.X = startX + division / 2;
                    temp.Y = startY + r.Next(rangeY);
                    points.Add(temp);
                }
                else
                {
                    temp = new Point();
                    temp.X = endX - ((i - numTopPoints + 1) * division);
                    temp.Y = startY + r.Next(rangeY);
                    points.Add(temp);
                }
            }

            points.Add(points[0]); //complete the circuit
        }


    }
}
