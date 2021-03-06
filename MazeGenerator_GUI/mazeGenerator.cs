using System;
using System.Drawing;
using System.Threading;
using Set;

namespace MazeGenerator_GUI {
	public class MazeGenerator{
		/* disjoint set for generating maze */
		DisjointSet ds = null;

		/* the number of rows, cols, and cells of maze  */
		int numRow = 0, numCol = 0, numCell = 0;

		/* each cell is element of disjoint set */
		int[,] cellArr = null;

		/* Point array to represent edges of maze.
		 * Two points (edgeArr[i,0] & edgeArr[i,1]) = One edge. */
		Point[,] edgeArr = null;
		Point[,] innerEdgeArr = null; //except outer edge. Shouldn't destroy outer wall of maze.

		/* -1: visited, but not deleted.  0: deleted.  1: not visited. */
		int[] edgeStat = null;
		int[] innerEdgeStat = null;

		int numOfEdge = 0;
		int numOfInnerEdge = 0;
		int horizontalOrVertical = 0;

		Random rd = null;

		public MazeGenerator() {
			Init(10,10);
		}

		public MazeGenerator(int numRow, int numCol){
			Init(numRow, numCol);
		}

		public void Init(int numRow, int numCol){
			/* size of maze */
			this.numRow = numRow;
			this.numCol = numCol;
			numCell = numRow * numCol;

			ds = new DisjointSet(numCell);

			cellArr = new int[numRow, numCol];

			/* the number of horizontal edges + vertical edges  */
			numOfEdge = numCol * (numRow + 1) + numRow * (numCol + 1);

			/* the number of edges except outer edges(top&bottom edges, leftmost&rightmost edges) */
			numOfInnerEdge = numOfEdge - (numCol * 2) - (numRow * 2);


			edgeArr = new Point[numOfEdge, 2]; 
			edgeStat = new int[numOfEdge]; 

			innerEdgeArr = new Point[numOfInnerEdge, 2];
			innerEdgeStat = new int[numOfInnerEdge];

			/* each cell is element of disjoint set */
			int cell = 1;
			for(int i = 0; i < numRow; i++){
				for(int j = 0; j < numCol; j++){
					cellArr[i,j] = cell++;
				}
			}

			/* initialize edgeArr and innerEdgeArr */
			InitEdgeArr();

			rd = new Random();
		}

		private void InitEdgeArr(){
			int x = 0, y = 0;
			int i = 0, j = 0;

			/* horizontal edge */
			for(y= 0; y < numRow + 1; y++){
				for(x = 0; x < numCol; x++) {
					edgeArr[i, 0] = new Point(x, y);
					edgeArr[i, 1] = new Point(x + 1, y);
					i++;
				}
			}

			/* horizontal inner edge */
			for (y = 1; y < numRow ; y++) {
				for (x = 0; x < numCol; x++) {
					innerEdgeArr[j, 0] = new Point(x, y);
					innerEdgeArr[j, 1] = new Point(x + 1, y);
					j++;
				}
			}

			/* innerEdgeArr[j,0] -> innerEdgeArr[j,1] (if j < horizontalOrVertical) is horizontal edge */
			horizontalOrVertical = j;

			/* vertical edge */
			for(x = 0; x < numCol + 1; x++){
				for(y = 0; y < numRow; y++) {
					edgeArr[i, 0] = new Point(x, y);
					edgeArr[i, 1] = new Point(x, y + 1);
					i++;
				}
			}

			/* vertical inner edge */
			for (x = 1; x < numCol; x++) {
				for (y = 0; y < numRow; y++) {
					innerEdgeArr[j, 0] = new Point(x, y);
					innerEdgeArr[j, 1] = new Point(x, y + 1);
					j++;
				}
			}
			/* initialize edgeStat[] and innerEdgeStat[] */
			InitEdgeStat();
		}

		private void InitEdgeStat() {
			/* initialize status of edge. 1: never visited */
			for (int i = 0; i < numOfEdge; i++)
				edgeStat[i] = 1;

			for (int j = 0; j < numOfInnerEdge; j++)
				innerEdgeStat[j] = 1;

			/* delete left wall(edge) of start cell and right wall(edge) of end cell */
			edgeStat[0] = 0;
			edgeStat[numOfEdge - 1] = 0;
		}

		private int EdgeSelect(){
			int randomEdge = 0;

			while (true) {
				randomEdge = rd.Next(numOfInnerEdge);
				if (innerEdgeStat[randomEdge] == 1) break; //not visited edge
			}

			return randomEdge;
		}

		public void GenerateMaze(){
			int count = 1;
			int row = 0, col = 0;
			int e1 = 0, e2 = 0;
			int r1 = 0, r2 = 0;
			int randomEdge = 0;

			/* initialize edgeStat[] and innerEdgeStat[] to regenerate maze*/
			InitEdgeStat();

			/* initialize disjoint set to regenerate maze */
			ds.Init(numCell);

			/* maze is generated by only (numCell -1) times union operation  */
			while(count < numCell){
				/* random edge selection */
				randomEdge = EdgeSelect();

				/* selected horizontal edge*/
				if(isHorizontalEdge(randomEdge)){
					row = innerEdgeArr[randomEdge, 0].Y - 1;
					col = innerEdgeArr[randomEdge, 0].X;

					e1 = cellArr[row, col];
					e2 = cellArr[row + 1, col];

					r1 = ds.Find(e1);
					r2 = ds.Find(e2);

					if(r1 != r2){
						ds.Union(r1, r2);
						innerEdgeStat[randomEdge] = 0; //delete edge
						count++;
					}
					else {
						innerEdgeStat[randomEdge] = -1; //visited edge
					}
				}

				/* selected vertical edge */
				else{
					row = innerEdgeArr[randomEdge, 0].Y;
					col = innerEdgeArr[randomEdge, 0].X - 1;

					e1 = cellArr[row, col];
					e2 = cellArr[row, col + 1];

					r1 = ds.Find(e1);
					r2 = ds.Find(e2);

					if(r1 != r2){
						ds.Union(r1, r2);
						innerEdgeStat[randomEdge] = 0; //delete edge
						count++;
					}
					else {
						innerEdgeStat[randomEdge] = -1; //visited edge.
					}
				}
			}

			/* duplicate innerEdgeStat -> edgeStat */
			int i = 0, j = 0;
			for (i = numCol; j < horizontalOrVertical ; i++, j++) {
				edgeStat[i] = innerEdgeStat[j];
			}

			i += numCol + numRow;
			for (; j < numOfInnerEdge; i++, j++) {
				edgeStat[i] = innerEdgeStat[j];
			}
		}

		private bool isHorizontalEdge(int randomEdge) {
			return randomEdge < horizontalOrVertical;
		}

		public void PrintMaze(Graphics gp, Pen pen){
			gp.Clear(Color.White);

			Point p1 = new Point();
			Point p2 = new Point();

			for(int i = 0; i < numOfEdge; i++) {
				if (edgeStat[i] == 0) continue;

				p1.X = edgeArr[i, 0].X * 10 + 20;
				p1.Y = edgeArr[i, 0].Y * 10 + 20;
				p2.X = edgeArr[i, 1].X * 10 + 20;
				p2.Y = edgeArr[i, 1].Y * 10 + 20;
				gp.DrawLine(pen, p1, p2);
				Thread.Sleep(2); // just print nice
			}
		}

	}
}
