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
        Map map = new Map();
        bool init = false;

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            tb_numPoints.Text = "11";
            panel1.BackColor = Color.White;
            tb_generationCount.Text = "20";
            tb_populationCount.Text = "20";
            textBox1.Text = "0";
            this.Name = "Art Gallery";
            this.Text = "Art Gallery";
            rb_GA.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            init = true;
            createPolygon();
            //ExportPointListCSV();
            populateDataGridView();
            map.Solve(points, int.Parse(tb_generationCount.Text), int.Parse(tb_populationCount.Text));
            updateLabels();
            panel1.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = panel1.CreateGraphics();

            List<Color> names = getColors();
            SolidBrush b;

            drawGAorWoC(g, names);
            drawIndividualGuardAreas(g, names);
            drawLines(g);
            drawAllVertices(g);
            drawReflexVertices(g);
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

        private void updateLabels()
        {
            int guardCount = 0;
            if(rb_GA.Checked && init)
            {
                foreach (Vertex v in map.GASolution)
                {
                    if (v.hasGuard)
                        guardCount++;
                }
            }
            else if (init)
            {
                guardCount = map.WoCSolution.Count;
            }
            lbl_guardCount.Text =  "Guard Count: " + guardCount.ToString();

            lbl_gatime.Text = "GA time: " + map.GAtime;
            lbl_woctime.Text = "WOC time: " + map.WOCtime;
            lbl_GAavg.Text = "GA Average: " + map.GAavgFitness;
        }

        private List<Color> getColors()
        {
            KnownColor[] knownNames = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            List<Color> names = new List<Color>();
            Color tempColor;
            int index;
            if (init)
            {
                for (int i = 0; i < Math.Max(map.GASolution.Count, map.WoCSolution.Count); i++)
                {
                    tempColor = new Color();
                    tempColor = ControlPaint.Dark(Color.FromKnownColor(knownNames[r.Next(knownNames.Length)]));
                    names.Add(tempColor);
                    index = i;
                }

                if (names.Contains(Color.Black)) //ensure no area gets painted black; used for default vertex color
                {
                    names.Remove(Color.Black);
                    do
                    {
                        tempColor = new Color();
                        tempColor = ControlPaint.Dark(Color.FromKnownColor(knownNames[r.Next(knownNames.Length)]));
                    } while (tempColor == Color.Black);
                    names.Add(tempColor);
                }
            }
            return names;
        }

        private void drawGAorWoC(Graphics g, List<Color> names)
        {
            List<PointF> temp;
            SolidBrush b;
            if (init && GuardAreas.Count == 0)
            {
                //draw the guard Areas
                if (rb_GA.Checked) //draw GA solution
                {
                    for (int i = 0; i < map.GASolution.Count; i++)
                    {
                        temp = new List<PointF>(convertToGUIPolygon(map.GASolution[i].LOS));
                        b = new SolidBrush(names[i]);
                        g.FillPolygon(b, temp.ToArray()); //draws guard areas
                    }
                }
                else //draw WoC solution
                {
                    for (int i = 0; i < map.WoCSolution.Count; i++)
                    {
                        temp = new List<PointF>(convertToGUIPolygon(map.WoCSolution[i].LOS));
                        b = new SolidBrush(names[i]);
                        g.FillPolygon(b, temp.ToArray());
                    }
                }
            }
        }

        private void drawIndividualGuardAreas(Graphics g, List<Color> names)
        {
            List<PointF> temp;
            SolidBrush b;
            //leave for individual guard area drawing
            for (int i = 0; i < GuardAreas.Count; i++)
            {
                temp = new List<PointF>(convertToGUIPolygon(GuardAreas[i]));
                b = new SolidBrush(names[i]);
                g.FillPolygon(b, temp.ToArray());
            }
        }

        private void drawLines(Graphics g)
        {
            //draw the lines
            for (int i = 0; i < points.Count - 1; i++)
            {
                g.DrawLine(Pens.Black, points[i], points[i + 1]);
            }

            //draw final connection
            if (init)
                g.DrawLine(Pens.Black, points[points.Count - 1], points[0]);
        }
        
        private void drawReflexVertices(Graphics g)
        {
            SolidBrush red = new SolidBrush(Color.Red);
            SolidBrush black = new SolidBrush(Color.Black);

            if (init)
            {
                if (rb_GA.Checked)
                {
                    foreach (Vertex v in map.GASolution)
                    {
                        if (v.hasGuard)
                        {
                            g.FillEllipse(red, points[v.ID].X - 5, points[v.ID].Y - 5, 10, 10);
                        }
                        else
                        {
                            g.FillEllipse(black, points[v.ID].X - 5, points[v.ID].Y - 5, 10, 10);
                        }
                    }
                }
                else
                {
                    foreach (Vertex v in map.WoCSolution)
                    {
                        if (v.hasGuard)
                        {
                            g.FillEllipse(red, points[v.ID].X - 5, points[v.ID].Y - 5, 10, 10);
                        }
                        else
                        {
                            g.FillEllipse(black, points[v.ID].X - 5, points[v.ID].Y - 5, 10, 10);
                        }
                    }
                }
            }
        }

        private void drawAllVertices(Graphics g)
        {
            SolidBrush black = new SolidBrush(Color.Black);
            if(init)
            {
                foreach(Point p in points)
                {
                    g.FillEllipse(black, p.X - 5, p.Y - 5, 10, 10);
                }
            }
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

        #region Graphic Event Handlers
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

        private void rb_GA_CheckedChanged(object sender, EventArgs e)
        {
            updateLabels();
            panel1.Invalidate();
        }

        private void rb_WOC_CheckedChanged(object sender, EventArgs e)
        {
            updateLabels();
            panel1.Invalidate();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            GuardAreas.Clear();
            panel1.Invalidate();
        }
        #endregion
    }
}
