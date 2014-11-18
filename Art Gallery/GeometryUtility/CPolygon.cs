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
		
		private CPoint2D[] m_aVertices;

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

				double dArea=PolygonArea(new CPoint2D[] {ptj,pti, ptk});
				
				if (dArea<0)
					vertexType= VertexType.ConvexPoint;
				else if (dArea> 0)
					vertexType= VertexType.ConcavePoint;
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
			bool bDiagonal=false;
			int nNumOfVertices=m_aVertices.Length;
            CLineSegment L1 = new CLineSegment(vertex1,vertex2);
			int j=0;
			for (int i= 0; i<nNumOfVertices; i++) //each point
			{
				bDiagonal=true;
				j= (i+1) % nNumOfVertices;  //next point of i
                
                CLineSegment L2 = new CLineSegment(this[i],this[j]);
                
                if (L1.IntersectedWith(L2))
                {
                    bDiagonal = false;
                }
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
            for (var i = 0; i < m_aVertices.Length; i++)
            {
                if (Diagonal(p, this[i]))
                {
                    temp.Add(this[i]);
                    if (PolygonVertexType(this[i]).Equals(VertexType.ConvexPoint))
                    {
                        var line = new CLine(p, this[i]);
                        for (var j = 0; j < m_aVertices.Length; j++)
                        {
                            if (j != i || j != VertexIndex(p))
                            {
                                var intersection = line.IntersectionWith(new CLine(this[j], this.NextPoint(this[j])));
                                if (intersection.InLine(new CLineSegment(this[j], this.NextPoint(this[j]))) && !temp.Contains(intersection))
                                {
                                    temp.Add(intersection);
                                }
                            }
                        }
                    }
                }
            }
            return temp;
        }

        public static bool IntersectedWith(CPolygon P1, CPolygon P2, bool includeVertices = false)
        {
            bool bIntersect = false;
            if (includeVertices)
            {
                List<CPoint2D> list_P1 = new List<CPoint2D>(P1.m_aVertices);
                List<CPoint2D> list_P2 = new List<CPoint2D>(P2.m_aVertices);
                if (list_P1.Union(list_P2).Count() > 0)
                {
                    bIntersect = true; ;
                }
            }
            for (var i = 0; i < P1.numberOfVertices; i++)
            {
                var L1 = new CLineSegment(P1[i],P1.NextPoint(P1[i]));
                for (var j = 0; j < P2.numberOfVertices; j++)
                {
                    var L2 = new CLineSegment(P2[j], P2.NextPoint(P2[j]));
                    if (L1.IntersectedWith(L2))
                    {
                        bIntersect = true; ;
                    }
                }
            }
            if (!bIntersect)
            {
                var numberOfIntersection = 0;
                for (var i = 0; i < P1.numberOfVertices; i++)
                {
                    var L1 = new CLineSegment(P1[i], P1.NextPoint(P1[i]));
                    for (var j = 0; j < P2.numberOfVertices; j++)
                    {
                        var L2 = new CLineSegment(P2[j], P2.NextPoint(P2[j]));
                        if (CLineSegment.LinesEqual(L1, L2))
                        {
                            numberOfIntersection++;
                        }
                    }
                }
                if (numberOfIntersection == P1.numberOfVertices && numberOfIntersection == P2.numberOfVertices)
                {
                    bIntersect = true;
                }
            }
            return bIntersect;
        }
	}		
}
