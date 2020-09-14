using System;
using SwinGameSDK;
using OpenTK;

namespace MyGame
{
	public class Rasterizer
	{
		public void Rasterize (Polygon poly, Camera cam)
		{
			Vector3d[] prVertices = poly.ProjectedVertices (cam);

			Vector3d centreOfPoly = new Vector3d ();
			Vector3d totalNormal = new Vector3d ();

			foreach (Vector3d pnt in prVertices)
			{
				centreOfPoly += pnt;
			}

			centreOfPoly /= prVertices.Length;

			foreach (Vector3d norm in poly.Normals)
			{
				totalNormal += norm;
			}

			totalNormal.Normalize ();

			//Bitmap bit = new Bitmap ();

			if (Vector3d.Dot (totalNormal, centreOfPoly.Normalized ()) > 0)
			{
				for (int i = 0; i < prVertices.Length; i++)
				{
					ToScreenCoords (cam, ref prVertices [i]);
				}

				ToScreenCoords (cam, ref centreOfPoly);

				for (int i = 2; i < prVertices.Length; i++)
				{
					RasterizeTriangle (cam, prVertices [0], prVertices [i - 1], prVertices [i]);
					//SwinGame.FillTriangle (Color.Grey, (float)prVertices [0].X, (float)prVertices [0].Y, (float)prVertices [i - 1].X, (float)prVertices [i - 1].Y, (float)prVertices [i].X, (float)prVertices [i].Y);

				}

				for (int i = 0; i < prVertices.Length; i++)
				{
					SwinGame.DrawLine (Color.Black, (float)prVertices [i].X, (float)prVertices [i].Y, (float)prVertices [(i + 1) % prVertices.Length].X, (float)prVertices [(i + 1) % prVertices.Length].Y);
				}

				SwinGame.FillCircle (Color.Black, (float)centreOfPoly.X * SwinGame.ScreenHeight (), (float)centreOfPoly.Y * SwinGame.ScreenHeight (), 1);
			}
		}

		private void ToScreenCoords (Camera c, ref Vector3d vec)
		{
			vec.X = (vec.X + 1) * c.ScreenWidth - 1;
			vec.Y = (vec.Y + 1) * c.ScreenHeight - 1;
		}

		private void RasterizeTriangle (Camera cam, Vector3d v1, Vector3d v2, Vector3d v3)
		{
			RearrangeVertices (ref v1, ref v2, ref v3);

			Vector3d v4 = SplitTriangle (v1, v2, v3);

			DrawTopTriangle (cam, v1, v2, v4);
			DrawBottomTriangle (cam, v2, v4, v3);
		}

		// Warning: This does not preserve the normal of the face.
		//			As long as the returned vectors are temporary,
		//			this should not matter.
		private void RearrangeVertices (ref Vector3d p1, ref Vector3d p2, ref Vector3d p3)
		{
			if (p1.Y > p2.Y)
				Swap (ref p1, ref p2);
			if (p2.Y > p3.Y)
				Swap (ref p2, ref p3);
			if (p1.Y > p3.Y)
				Swap (ref p1, ref p3);
			if (p1.Y > p2.Y)
				Swap (ref p1, ref p2);
		}

		private Vector3d SplitTriangle (Vector3d p1, Vector3d p2, Vector3d p3)
		{
			Vector3d vec = new Vector3d ();

			vec.X = (p2.Y - p1.Y) * (p3.X - p1.X) / (p3.Y - p1.Y) + p1.X;
			vec.Y = p2.Y;
			vec.Z = (p2.Y - p1.Y) * (p3.Z - p1.Z) / (p3.Y - p1.Y) + p1.Z;

			return vec;
		}

		private void DrawTopTriangle (Camera cam, Vector3d p1, Vector3d p2, Vector3d p3)
		{
			double currY = p1.Y;
			double currLeftX = p1.X;
			double currRightX = p1.X;

			double invLeftGrad = (p2.X - p1.X) / (p2.Y - p1.Y);
			double invRightGrad = (p3.X - p1.X) / (p3.Y - p1.Y);

			while (currY < p2.Y)
			{
				//SwinGame.DrawLine (Color.Grey, (float)currLeftX, (float)currY, (float)currRightX, (float)currY);
				DrawHorizontalLine (cam, Color.Grey, currLeftX, currRightX, currY, 0);

				currLeftX += invLeftGrad;
				currRightX += invRightGrad;
				currY += 1;
			}
		}

		private void DrawBottomTriangle (Camera cam, Vector3d p1, Vector3d p2, Vector3d p3)
		{
			double currY = p1.Y;
			double currLeftX = p1.X;
			double currRightX = p2.X;

			double invLeftGrad = (p3.X - p1.X) / (p3.Y - p1.Y);
			double invRightGrad = (p3.X - p2.X) / (p3.Y - p2.Y);

			while (currY < p3.Y)
			{
				//SwinGame.DrawLine (Color.Grey, (float)currLeftX, (float)currY, (float)currRightX, (float)currY);
				DrawHorizontalLine (cam, Color.Grey, currLeftX, currRightX, currY, 0);

				currLeftX += invLeftGrad;
				currRightX += invRightGrad;
				currY += 1;
			}
		}

		private void DrawHorizontalLine (Camera cam, Color c, double X1, double X2, double Y, double Z)
		{
			if (X1 > X2)
				Swap (ref X1, ref X2);

			int x = (int)Math.Floor (X1);
			int y = (int)Math.Floor (Y);

			while (x < X2)
			{
				cam.DrawPixel (c, x, y, Z);
				x++;
			}
		}

		private void Swap<T> (ref T lhs, ref T rhs)
		{
			T tmp;
			tmp = lhs;
			lhs = rhs;
			rhs = tmp;
		}
	}
}

