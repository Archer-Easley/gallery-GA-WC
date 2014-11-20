using System;
using System.Collections.Generic;
using System.Linq;

namespace GeometryUtility
{
	/// <summary>
	/// Summary description for CPolygon.
	/// </summary>
	public class CPolygon
	{
		
		public CPoint2D[] m_aVertices;

		public CPoint2D   this[int index] 
		{
			set
			{
				m_aVertices[index]=value;
			}
			get
			{
				return m_aVertices[index];
			}
		}

        public int numberOfVertices
        {
            get
            {
                return this.m_aVertices.Length;
            }
        }

		public CPolygon()
		{
		
		}

		public CPolygon(CPoint2D[] points)
		{
			int nNumOfPoitns=points.Length;
			try
			{
				if (nNumOfPoitns<3 )
				{     
					InvalidInputGeometryDataException ex=
						new InvalidInputGeometryDataException();
					throw ex;
				}
				else
				{
					m_aVertices=new CPoint2D[nNumOfPoitns];
					for (int i=0; i<nNumOfPoitns; i++)
					{
						m_aVertices[i]=points[i];
					}
				}
			}
			catch (Exception e)
			{
				System.Diagnostics.Trace.WriteLine(
					e.Message+e.StackTrace);
			}
		}

		/***********************************
		 From a given point, get its vertex index.
		 If the given point is not a polygon vertex, 
		 it will return -1 
		 ***********************************/
		public int VertexIndex(CPoint2D vertex)
		{
			int nIndex=-1;

			int nNumPts=m_aVertices.Length;
			for (int i=0; i<nNumPts; i++) //each vertex
			{
				if (CPoint2D.SamePoints(m_aVertices[i], vertex))
					nIndex=i;
			}
			return nIndex;
		}

		/***********************************
		 From a given vertex, get its previous vertex point.
		 If the given point is the first one, 
		 it will return  the last vertex;
		 If the given point is not a polygon vertex, 
		 it will return null; 
		 ***********************************/
		public CPoint2D PreviousPoint(CPoint2D vertex)
		{
			int nIndex;
			
			nIndex=VertexIndex(vertex);
			if (nIndex==-1)
				return null;
			else //a valid vertex
			{
				if (nIndex==0) //the first vertex
				{
					int nPoints=m_aVertices.Length;
					return m_aVertices[nPoints-1];
				}
				else //not the first vertex
					return m_aVertices[nIndex-1];
			}			
		}

		/***************************************
			 From a given vertex, get its next vertex point.
			 If the given point is the last one, 
			 it will return  the first vertex;
			 If the given point is not a polygon vertex, 
			 it will return null; 
		***************************************/
		public CPoint2D NextPoint(CPoint2D vertex)
		{
			CPoint2D nextPt=new CPoint2D();

			int nIndex;
			nIndex=VertexIndex(vertex);
			if (nIndex==-1)
				return null;
			else //a valid vertex
			{
				int nNumOfPt=m_aVertices.Length;
				if (nIndex==nNumOfPt-1) //the last vertex
				{
					return m_aVertices[0];
				}
				else //not the last vertex
					return m_aVertices[nIndex+1];
			}			
		}

		
		/******************************************
		To calculate the polygon's area

		Good for polygon with holes, but the vertices make the 
		hole  should be in different direction with bounding 
		polygon.
		
		Restriction: the polygon is not self intersecting
		ref: www.swin.edu.au/astronomy/pbourke/
			geometry/polyarea/
		*******************************************/
		public double PolygonArea()
		{
			double dblArea=0;
			int nNumOfVertices=m_aVertices.Length;
			
			int j;
			for (int i=0; i<nNumOfVertices; i++)
			{
				j=(i+1) % nNumOfVertices;
				dblArea += m_aVertices[i].X*m_aVertices[j].Y;
				dblArea -= (m_aVertices[i].Y*m_aVertices[j].X);
			}

			dblArea=dblArea/2;
			return Math.Abs(dblArea);
		}
		
		/******************************************
		To calculate the area of polygon made by given points 

		Good for polygon with holes, but the vertices make the 
		hole  should be in different direction with bounding 
		polygon.
		
		Restriction: the polygon is not self intersecting
		ref: www.swin.edu.au/astronomy/pbourke/
			geometry/polyarea/

		As polygon in different direction, the result coulb be
		in different sign:
		If dblArea>0 : polygon in clock wise to the user 
		If dblArea<0: polygon in count clock wise to the user 		
		*******************************************/
		public static double PolygonArea(CPoint2D[] points)
		{
			double dblArea=0;
			int nNumOfPts=points.Length;
			
			int j;
			for (int i=0; i<nNumOfPts; i++)
			{
				j=(i+1) % nNumOfPts;
				dblArea += points[i].X*points[j].Y;
				dblArea -= (points[i].Y*points[j].X);
			}

			dblArea=dblArea/2;
			return dblArea;
		}
		
		/***********************************************
			To check a vertex concave point or a convex point
			-----------------------------------------------------------
			The out polygon is in count clock-wise direction
		************************************************/
		public VertexType PolygonVertexType(CPoint2D vertex)
		{
			VertexType vertexType=VertexType.ErrorPoint;

			if (PolygonVertex(vertex))			
			{
				CPoint2D pti=vertex;
				CPoint2D ptj=PreviousPoint(vertex);
				CPoint2D ptk=NextPoint(vertex);

                double dArea = PolygonArea(new CPoint2D[] { ptj, pti, ptk });

                if (dArea > 0)
                    vertexType = VertexType.ConvexPoint;
                else if (dArea < 0)
                    vertexType = VertexType.ConcavePoint;
			}	
			return vertexType;
		}

		
		/*********************************************
		To check the Line of vertex1, vertex2 is a Diagonal or not
  
		To be a diagonal, Line vertex1-vertex2 has no intersection 
		with polygon lines.
		
		If it is a diagonal, return true;
		If it is not a diagonal, return false;
		*********************************************/
		public bool Diagonal(CPoint2D vertex1, CPoint2D vertex2)
		{
			bool bDiagonal=true;
			int nNumOfVertices=m_aVertices.Length;
			int j=0;
            CLineSegment diagonal = new CLineSegment(vertex1, vertex2); 
			for (int i= 0; i<nNumOfVertices; i++) //each point
			{
                
				j= (i+1) % nNumOfVertices;  //next point of i

                CLineSegment polygonLine = new CLineSegment(m_aVertices[i], m_aVertices[j]);

                if (polygonLine.IntersectedWith(diagonal) && !vertex2.InLine(polygonLine))
                {
                    bDiagonal = false;
                    break;
                }

			}
            // if not matches found, diagonal is either entirely inside, or outside polygon
            if (bDiagonal)
            {
                //if (PolygonVertex(vertex2))
                {
                    CPoint2D midpoint = new CPoint2D((vertex1.X + vertex2.X) / 2, (vertex1.Y + vertex2.Y) / 2);
                    if (!midpoint.PointInsidePolygon(m_aVertices))
                    {
                        bDiagonal = false;
                    }
                }
                //else
                //{
                //    CLineSegment V1 = new CLineSegment(vertex1, PreviousPoint(vertex1));
                //    CLineSegment V2 = new CLineSegment(vertex1, NextPoint(vertex1));
                //    CLineSegment V3 = new CLineSegment(vertex1, vertex2);
                //    double theta12 = Math.Atan2(CLineSegment.cross2D(V1, V2), CLineSegment.dot(V1, V2));
                //    double theta13 = Math.Atan2(CLineSegment.cross2D(V1, V3), CLineSegment.dot(V1, V3));
                //    double theta32 = Math.Atan2(CLineSegment.cross2D(V2, V3), CLineSegment.dot(V2, V3));
                //    if (CLineSegment.dot(V1, V2) < 0)
                //    {
                //        if (theta12 > 0)
                //        {
                //            theta12 = Math.Abs(theta12) + Math.PI;
                //        }
                //        else
                //        {
                //            theta12 = Math.PI - theta12;
                //        }
                        
                //    }
                //    if (CLineSegment.dot(V1, V3) < 0)
                //    {
                //        if (theta13 > 0)
                //        {
                //            theta13 = Math.Abs(theta13) + Math.PI;
                //        }
                //        else
                //        {
                //            theta13 = Math.PI - theta13;
                //        }
                //    }
                //    if (CLineSegment.dot(V2, V3) < 0)
                //    {
                //        if (theta32 > 0)
                //        {
                //            theta32 = Math.Abs(theta32) + Math.PI;
                //        }
                //        else
                //        {
                //            theta32 = Math.PI - theta32;
                //        }

                //    }
                //    if ((theta12 - theta13 - theta32) > ConstantValue.SmallValue)
                //    {
                //        bDiagonal = false;
                //    }
                //}

            }
            return bDiagonal;
		}

		
		/*************************************************
		To check FaVertices make a convex polygon or 
		concave polygon

		Restriction: the polygon is not self intersecting
		********************************************/
		public PolygonType GetPolygonType()
		{
			int nNumOfVertices=m_aVertices.Length;
			bool bSignChanged=false;
			int nCount=0;
			int j=0, k=0;

			for (int i=0; i<nNumOfVertices; i++)
			{
				j=(i+1) % nNumOfVertices; //j:=i+1;
				k=(i+2) % nNumOfVertices; //k:=i+2;

				double crossProduct=(m_aVertices[j].X- m_aVertices[i].X)
					*(m_aVertices[k].Y- m_aVertices[j].Y);
				crossProduct=crossProduct-(
					(m_aVertices[j].Y- m_aVertices[i].Y)
					*(m_aVertices[k].X- m_aVertices[j].X)
					);

				//change the value of nCount
				if ((crossProduct>0) && (nCount==0) )
					nCount=1;
				else if ((crossProduct<0) && (nCount==0))
					nCount=-1;

				if (((nCount==1) && (crossProduct<0))
					||( (nCount==-1) && (crossProduct>0)) )
					bSignChanged=true;
			}

			if (bSignChanged)
				return PolygonType.Concave;
			else
				return PolygonType.Convex;
		}

		/***************************************************
		Check a Vertex is a principal vertex or not
  
		PrincipalVertex: a vertex pi of polygon P is a principal vertex if the
		diagonal pi-1, pi+1 intersects the boundary of P only at pi-1 and pi+1.
		*********************************************************/
		public bool PrincipalVertex(CPoint2D vertex)
		{
			bool bPrincipal=false;
			if (PolygonVertex(vertex)) //valid vertex
			{
				CPoint2D pt1=PreviousPoint(vertex);
				CPoint2D pt2=NextPoint(vertex);
					
				if (Diagonal(pt1, pt2))
					bPrincipal=true;
			}
			return bPrincipal;
		}

		/*********************************************
        To check whether a given point is a CPolygon Vertex
		**********************************************/
		public bool PolygonVertex(CPoint2D point)
		{
			bool bVertex=false;
			int nIndex=VertexIndex(point);

			if ((nIndex>=0) && (nIndex<=m_aVertices.Length-1))
							   bVertex=true;

			return bVertex;
		}

		/*****************************************************
		To reverse polygon vertices to different direction:
		clock-wise <------->count-clock-wise
		******************************************************/
		public void ReverseVerticesDirection()
		{
			int nVertices=m_aVertices.Length;
			CPoint2D[] aTempPts=new CPoint2D[nVertices];
			
			for (int i=0; i<nVertices; i++)
				aTempPts[i]=m_aVertices[i];
	
			for (int i=0; i<nVertices; i++)
			m_aVertices[i]=aTempPts[nVertices-1-i];	
		}

		/*****************************************
		To check vertices make a clock-wise polygon or
		count clockwise polygon

		Restriction: the polygon is not self intersecting
		Ref: www.swin.edu.au/astronomy/pbourke/
		geometry/clockwise/index.html
		*****************************************/
		public PolygonDirection VerticesDirection()
		{
			int nCount=0, j=0, k=0;
			int nVertices=m_aVertices.Length;
			
			for (int i=0; i<nVertices; i++)
			{
				j=(i+1) % nVertices; //j:=i+1;
				k=(i+2) % nVertices; //k:=i+2;

				double crossProduct=(m_aVertices[j].X - m_aVertices[i].X)
					*(m_aVertices[k].Y- m_aVertices[j].Y);
				crossProduct=crossProduct-(
					(m_aVertices[j].Y- m_aVertices[i].Y)
					*(m_aVertices[k].X- m_aVertices[j].X)
					);

				if (crossProduct>0)
					nCount++;
				else
					nCount--;
			}
		
			if( nCount<0) 
				return PolygonDirection.Count_Clockwise;
			else if (nCount> 0)
				return PolygonDirection.Clockwise;
			else
				return PolygonDirection.Unknown;
  		}
		
		/*****************************************
		To check given points make a clock-wise polygon or
		count clockwise polygon

		Restriction: the polygon is not self intersecting
		*****************************************/
		public static PolygonDirection PointsDirection(
			CPoint2D[] points)
		{
			int nCount=0, j=0, k=0;
			int nPoints=points.Length;
			
			if (nPoints<3)
				return PolygonDirection.Unknown;
			
			for (int i=0; i<nPoints; i++)
			{
				j=(i+1) % nPoints; //j:=i+1;
				k=(i+2) % nPoints; //k:=i+2;

				double crossProduct=(points[j].X - points[i].X)
					*(points[k].Y- points[j].Y);
				crossProduct=crossProduct-(
					(points[j].Y- points[i].Y)
					*(points[k].X- points[j].X)
					);

				if (crossProduct>0)
					nCount++;
				else
					nCount--;
			}
		
			if( nCount<0) 
				return PolygonDirection.Count_Clockwise;
			else if (nCount> 0)
				return PolygonDirection.Clockwise;
			else
				return PolygonDirection.Unknown;
		}

		/*****************************************************
		To reverse points to different direction (order) :
		******************************************************/
		public static void ReversePointsDirection(
			CPoint2D[] points)
		{
			int nVertices=points.Length;
			CPoint2D[] aTempPts=new CPoint2D[nVertices];
			
			for (int i=0; i<nVertices; i++)
				aTempPts[i]=points[i];
	
			for (int i=0; i<nVertices; i++)
				points[i]=aTempPts[nVertices-1-i];	
		}

        public List<CPoint2D> VisibilitySet(CPoint2D p)
        {
            List<CPoint2D> temp = new List<CPoint2D>();
            List<CPoint2D> intersections = new List<CPoint2D>();
            if (PolygonVertex(p))
            {
                temp.Add(PreviousPoint(p));
                temp.Add(p);
                temp.Add(NextPoint(p));
            }
            else
            {
                for (var i = 0; i < m_aVertices.Length; i++)
                {
                    if ((p.InLine(new CLineSegment(this[i], NextPoint(this[i])))))
                    {
                        temp.Add(this[i]);
                        temp.Add(p);
                        temp.Add(NextPoint(this[i]));
                    }
                }
            }
            for (var i = 0; i < m_aVertices.Length; i++)
            {
                if (i != VertexIndex(p)) 
                { 
                    if (Diagonal(p, this[i]))
                    {
                        if (!temp.Contains(this[i]))
                        {
                            if (temp.FindIndex(x=> VertexIndex(x) > VertexIndex(this[i])) == -1)
                            {
                                temp.Add(this[i]);
                            }
                            else
                            {
                                temp.Insert(temp.IndexOf(temp.First(x => VertexIndex(x) > VertexIndex(this[i]))), this[i]);
                            }

                        }
                        if (PolygonVertexType(this[i]).Equals(VertexType.ConcavePoint))
                        {
                            CLine line = new CLine(p, this[i]);
                            CLineSegment lineSegment = new CLineSegment(p,this[i]);
                            for (int j = 0; j < m_aVertices.Length; j++)
                            {
                                if (j != i || j != VertexIndex(p))
                                {
                                
                                    CLineSegment edge = new CLineSegment(this[j], NextPoint(this[j]));
                                    CPoint2D intersection = edge.IntersecctionWith(line);
                                    CLineSegment PToIntersection = new CLineSegment(p, intersection);
                                    if (intersection.InLine(edge) && intersection.X > 0 && intersection.Y > 0 && !temp.Contains(intersection) && Diagonal(this[i],intersection) && lineSegment.InLine(PToIntersection))
                                    {
                                        intersections.Add(intersection);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            var mynewList = new List<CPoint2D>();
            if (intersections.Count > 2)
            {
                var PTemp = new CPolygon(temp.ToArray());
                var PIntersections = new CPolygon(intersections.ToArray());
                mynewList = this.JoinPolygon(PTemp, PIntersections);
            }
            else
            {
                var PTemp = new CPolygon(temp.ToArray());
                mynewList = this.JoinPolygon(PTemp, intersections);
            }
            //for (var i = 0; i < m_aVertices.Length; i++)
            //{
            //    for (var j = 0; j < intersections.Count; j++)
            //    {
            //        if (intersections[j].InLine(new CLineSegment(this[i], NextPoint(this[i]))))
            //        {
            //            if (VertexIndex(NextPoint(this[i])) == 0)
            //            {
            //                temp.Add(intersections[j]);
            //            }
            //            else 
            //            {
            //                try
            //                {
            //                    temp.Insert(temp.IndexOf(temp.First(x => VertexIndex(x) > VertexIndex(this[i]))), intersections[j]);
            //                }
            //                catch
            //                {
            //                    temp.Add(intersections[j]);
            //                }
            //            }
                        
            //            intersections.RemoveAt(j);
            //            j--;
            //        }
            //    }
            //}
            return mynewList;
        }

        public List<CPoint2D> JoinPolygon(CPolygon P1, CPolygon P2)
        {
            List<CPoint2D> LP1 = new List<CPoint2D>(P1.m_aVertices);
            List<CPoint2D> LP2 = new List<CPoint2D>(P2.m_aVertices);
            List<CPoint2D> Union = new List<CPoint2D>(LP1.Union(LP2));
            List<CPoint2D> combo = new List<CPoint2D>();
            for (var i = 0; i < this.m_aVertices.Length; i++)
            {
                if (Union.Contains(m_aVertices[i]))
                {
                    if (!combo.Contains(m_aVertices[i]))
                    {
                        combo.Add(m_aVertices[i]);
                    }
                    Union.Remove(m_aVertices[i]);
                }
                CLineSegment line = new CLineSegment(m_aVertices[i], NextPoint(m_aVertices[i]));
                List<CPoint2D> PointsOnLine = new List<CPoint2D>(Union.FindAll(x => x.InLine(line) && !this.PolygonVertex(x)));
                if (PointsOnLine.Count > 0)
                {
                    if (line.GetLineAngle() > 0)
                    {
                        if (line.VerticalLine())
                        {
                            // order by y
                        }
                        else if (line.GetYmax() == line.StartPoint.Y)
                        {
                            PointsOnLine.OrderByDescending(x => x.Y);
                        }
                        else
                        {
                            PointsOnLine.OrderBy(x => x.Y);
                        }
                    }
                    else if (line.GetLineAngle() < 0)
                    {
                        if (line.GetYmax() == line.StartPoint.Y)
                        {
                            PointsOnLine.OrderByDescending(x => x.Y);
                        }
                        else
                        {
                            PointsOnLine.OrderBy(x => x.Y);
                        }
                    }
                }
                for (var j = 0; j < PointsOnLine.Count; j++)
                {
                    if (!combo.Contains(PointsOnLine[j]))
                    {
                        combo.Add(PointsOnLine[j]);
                        PointsOnLine.Remove(PointsOnLine[j]);
                        j--;
                    }
                }
            }
            return combo;
        }

        public List<CPoint2D> JoinPolygon(CPolygon P1, List<CPoint2D> L1)
        {
            List<CPoint2D> LP1 = new List<CPoint2D>(P1.m_aVertices);
            List<CPoint2D> Union = new List<CPoint2D>(LP1.Union(L1));
            List<CPoint2D> combo = new List<CPoint2D>();
            for (var i = 0; i < this.m_aVertices.Length; i++)
            {
                if (Union.Contains(m_aVertices[i]))
                {
                    if (!combo.Contains(m_aVertices[i]))
                    {
                        combo.Add(m_aVertices[i]);
                    }
                    Union.Remove(m_aVertices[i]);
                }
                CLineSegment line = new CLineSegment(m_aVertices[i], NextPoint(m_aVertices[i]));
                List<CPoint2D> PointsOnLine = new List<CPoint2D>(Union.FindAll(x => x.InLine(line) && !this.PolygonVertex(x)));
                if (PointsOnLine.Count > 0)
                {
                    if (line.GetLineAngle() > 0)
                    {
                        if (line.VerticalLine())
                        {
                            // order by y
                        }
                        else if (line.GetYmax() == line.StartPoint.Y)
                        {
                            PointsOnLine.OrderByDescending(x => x.Y);
                        }
                        else
                        {
                            PointsOnLine.OrderBy(x => x.Y);
                        }
                    }
                    else if (line.GetLineAngle() < 0)
                    {
                        if (line.GetYmax() == line.StartPoint.Y)
                        {
                            PointsOnLine.OrderByDescending(x => x.Y);
                        }
                        else
                        {
                            PointsOnLine.OrderBy(x => x.Y);
                        }
                    }
                }
                for (var j = 0; j < PointsOnLine.Count; j++)
                {
                    if (!combo.Contains(PointsOnLine[j]))
                    {
                        combo.Add(PointsOnLine[j]);
                        PointsOnLine.Remove(PointsOnLine[j]);
                        j--;
                    }
                }
            }
            return combo;
        }

        public static bool IntersectedWith(CPolygon P1, CPolygon P2, bool includeVertices = false)
        {
            List<CPoint2D> list_P1 = new List<CPoint2D>(P1.m_aVertices);
            List<CPoint2D> list_P2 = new List<CPoint2D>(P2.m_aVertices);
            if (includeVertices)
            {
                if (list_P1.Union(list_P2).Count() > 0)
                {
                    return true;
                }
            }
            for (var i = 0; i < P1.numberOfVertices; i++)
            {
                for (var j = 0; j < P2.numberOfVertices; j++)
                {
                    if (CLineSegment.IntersectedWith(new CLineSegment(P1[i], P1.NextPoint(P1[i])), new CLineSegment(P2[j], P2.NextPoint(P2[j]))))
                    {
                        if (!P1[i].Equals(P2[j]) && !P1[i].Equals(P2.NextPoint(P2[j])) && !P1.NextPoint(P1[i]).Equals(P2[j]) && !P1.NextPoint(P1[i]).Equals(P2.NextPoint(P2[j])))
                        {
                            return true;
                        }
                    }
                }
            }
            if (list_P1.Union(list_P2).Count() == list_P1.Count() && list_P1.Count == list_P2.Count)
            {
                return true;
            }
            return false;
        }
	}		
}
