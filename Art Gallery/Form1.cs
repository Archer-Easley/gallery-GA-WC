using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using GeometryUtility;

namespace FormsPolygonGenerator
{
    public partial class Form1 : Form
    {
        List<Point> points = new List<Point>();
        List<CPolygon> GuardAreas = new List<CPolygon>();
        Random r = new Random();
        bool init = false;

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            tb_numPoints.Text = "27";
            panel1.BackColor = Color.White;
            tb_generationCount.Text = "100";
            tb_populationCount.Text = "100";
            textBox1.Text = "0";
            this.Name = "Art Gallery";
            this.Text = "Art Gallery";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            init = true;
            Map map = new Map();
            createPolygon();
            //ExportPointListCSV();
            populateDataGridView();
            map.Solve(points, int.Parse(tb_generationCount.Text), int.Parse(tb_populationCount.Text));
            panel1.Invalidate();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = panel1.CreateGraphics();

            //get all necessary colors
            SolidBrush b;
            KnownColor[] knownNames = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            List<Color> names = new List<Color>();
            Color tempColor;
            for (int i = 0; i < GuardAreas.Count; i++)
            {
                tempColor = new Color();
                tempColor = ControlPaint.Dark(Color.FromKnownColor(knownNames[r.Next(knownNames.Length)]));
                names.Add(tempColor);
            }

            List<PointF> temp;

            //draw the guard Areas
            for (int i = 0; i < GuardAreas.Count; i++)
            {
                temp = new List<PointF>(convertToGUIPolygon(GuardAreas[i]));
                b = new SolidBrush(names[i]);
                g.FillPolygon(b, temp.ToArray());
            }

            //draw the lines
            for (int i = 0; i < points.Count - 1; i++)
            {
                g.DrawLine(Pens.Black, points[i], points[i + 1]);
            }

            //draw final connection
            if(init)
                g.DrawLine(Pens.Black, points[points.Count - 1], points[0]);

            for (int i = 0; i < points.Count; i++)
            {
                g.DrawEllipse(Pens.Black, points[i].X - 5, points[i].Y - 5, 10, 10);
                SolidBrush br = new SolidBrush(Color.Black);
                g.FillEllipse(br, points[i].X - 5, points[i].Y - 5, 10, 10);
            }

            //Fill vertex in with red if it's a guard
            if (init)
            {
                SolidBrush br = new SolidBrush(Color.Red);
                g.FillEllipse(br, points[int.Parse(textBox1.Text)].X - 5, points[int.Parse(textBox1.Text)].Y - 5, 10, 10);
            }
        }

        #region Graphics Helpers

        private void createPolygon()
        {
            points.Clear();
            int NumPoints = int.Parse(tb_numPoints.Text);
            int startX = panel1.Location.X;
            int startY = (panel1.Location.Y + panel1.Size.Height) / 2;
            int endX = panel1.Size.Width - panel1.Location.X;
            int rangeX = endX - startX;
            int rangeY = panel1.Size.Height / 2;
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
        }

        private List<PointF> convertToGUIPolygon(CPolygon c)
        {
            PointF tempPoint;
            List<PointF> tempPointList = new List<PointF>();
            //convert CPoint to GUI Points
            for(int i = 0; i < c.numberOfVertices; i++)
            {
                tempPoint = new PointF();
                tempPoint.X = int.Parse(Math.Round(c[i].X).ToString());
                tempPoint.Y = int.Parse(Math.Round(c[i].Y).ToString());
                tempPointList.Add(tempPoint);
            }
            return tempPointList;
        }

        private void populateDataGridView()
        {
            DataTable table = new DataTable();
            table.Columns.Add("X", typeof(int));
            table.Columns.Add("Y", typeof(int));

            foreach(Point p in points)
            {
                table.Rows.Add(p.X, p.Y);
            }
            dataGridView1.DataSource = table;
        }

        private CPoint2D[] convertToGeometryUtilityPolygon()
        {
            List<CPoint2D> tempList = new List<CPoint2D>();
            CPoint2D temp;
            foreach(Point p in points)
            {
                temp = new CPoint2D();
                temp.X = p.X;
                temp.Y = p.Y;
                tempList.Add(temp);
            }
            return tempList.ToArray();
        }

        #endregion

        #region testing
        private void ExportPointListCSV()
        {
            var csv = new StringBuilder();
            string header = string.Format("X,Y{0}", Environment.NewLine);
            csv.Append(header);
            string X = string.Empty;
            string Y = string.Empty;
            string line = string.Empty;

            foreach(Point p in points)
            {
                X = p.X.ToString();
                Y = p.Y.ToString();
                line = string.Format("{0},{1}{2}", X, Y, Environment.NewLine);
                csv.Append(line);
            }
            string filename = AppDomain.CurrentDomain.BaseDirectory + "output.csv";
            File.WriteAllText(filename, csv.ToString());
        }

        private void createTestingGuardArea()
        {
            GuardAreas.Clear();
            List<CPoint2D> templist = new List<CPoint2D>();
            CPoint2D temp;
            for (int i = 0; i < 5; i++)
            {
                temp = new CPoint2D();
                temp.X = points[i].X;
                temp.Y = points[i].Y;
                templist.Add(temp);
            }
            CPolygon test = new CPolygon(templist.ToArray());
            GuardAreas.Add(test);

            templist.Clear();
            for (int i = 13; i < 16; i++)
            {
                temp = new CPoint2D();
                temp.X = points[i].X;
                temp.Y = points[i].Y;
                templist.Add(temp);
            }
            test = new CPolygon(templist.ToArray());
            GuardAreas.Add(test);
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            GuardAreas.Clear();
            CPolygon temp = new CPolygon(convertToGeometryUtilityPolygon()); //creates total polygon
            List<CPoint2D> tempList = new List<CPoint2D>();

            tempList = temp.VisibilitySet(temp[int.Parse(textBox1.Text) % temp.numberOfVertices]); //if guard is at this vertex, generate a list of points that creates the polygon
            CPolygon guardArea = new CPolygon(tempList.ToArray());

            GuardAreas.Add(guardArea);
            panel1.Invalidate();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            GuardAreas.Clear();
            //increment guard
            int tempGuard;
            tempGuard = (int.Parse(textBox1.Text) + 1) % (points.Count - 1);
            textBox1.Text = tempGuard.ToString();

            CPolygon temp = new CPolygon(convertToGeometryUtilityPolygon()); //creates total polygon
            List<CPoint2D> tempList = new List<CPoint2D>();

            tempList = temp.VisibilitySet(temp[int.Parse(textBox1.Text) % temp.numberOfVertices]); //if guard is at this vertex, generate a list of points that creates the polygon
            CPolygon guardArea = new CPolygon(tempList.ToArray());

            GuardAreas.Add(guardArea);
            panel1.Invalidate();
        }

        private void btn_previousGuard_Click(object sender, EventArgs e)
        {
            GuardAreas.Clear();
            //decrement guard
            if(textBox1.Text == "0")
            {
                textBox1.Text = (points.Count - 1).ToString();
            }
            else
            {
                int tempGuard;
                tempGuard = (int.Parse(textBox1.Text) - 1) % (points.Count - 1);
                textBox1.Text = tempGuard.ToString();
            }

            CPolygon temp = new CPolygon(convertToGeometryUtilityPolygon()); //creates total polygon
            List<CPoint2D> tempList = new List<CPoint2D>();

            tempList = temp.VisibilitySet(temp[int.Parse(textBox1.Text) % temp.numberOfVertices]); //if guard is at this vertex, generate a list of points that creates the polygon
            CPolygon guardArea = new CPolygon(tempList.ToArray());

            GuardAreas.Add(guardArea);
            panel1.Invalidate();
        }

    }
}
