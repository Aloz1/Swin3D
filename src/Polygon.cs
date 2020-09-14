using System;
using System.Collections.Generic;
using SwinGameSDK;
using OpenTK;

namespace MyGame
{
	public class Polygon
	{
		private List<Vertex> _vertices;

		public Polygon (List<Vertex> vxList)
		{
			_vertices = vxList;
		}

		public Polygon Clone ()
		{
			List<Vertex> clonedList = new List<Vertex> ();
			foreach (Vertex v in _vertices)
			{
				clonedList.Add (v.Clone ());
			}
			return new Polygon (clonedList);
		}

		public Vector3d[] ProjectedVertices (Camera cam)
		{
			List<Vector3d> result = new List<Vector3d> ();

			foreach (Vertex v in _vertices)
			{
				result.Add (v.ProjectedPos (cam));
			}

			ClipToCamera (cam, ref result);

			return result.ToArray ();
		}

		public Vector3d[] ProjectedNormals (Camera cam)
		{
			List<Vector3d> result = new List<Vector3d> ();

			foreach (Vertex v in _vertices)
			{
				result.Add (v.Normal);
			}

			return result.ToArray ();
		}

		private void ClipToCamera(Camera cam, ref List<Vector3d> vertices)
		{
			// Clip to front and back of screen
			//ClipToPlane (Vector3d.UnitZ, new Vector3d (0, 0, cam.Zmin), ref vertices);
			//ClipToPlane (-Vector3d.UnitZ, new Vector3d (0, 0, cam.Zmax), ref vertices);
			ClipToPlane (Vector3d.UnitZ, new Vector3d (0, 0, 0), ref vertices);
			ClipToPlane (-Vector3d.UnitZ, new Vector3d (0, 0, 1), ref vertices);

			// Clip to left and right of screen
			//ClipToPlane (Vector3d.UnitX, new Vector3d (cam.Xmin, 0, 0), ref vertices);
			//ClipToPlane (-Vector3d.UnitX, new Vector3d (cam.Xmax, 0, 0), ref vertices);
			ClipToPlane (Vector3d.UnitX, new Vector3d (0, 0, 0), ref vertices);
			ClipToPlane (-Vector3d.UnitX, new Vector3d (1, 0, 0), ref vertices);

			// Clip to top and bottom of screen
			//ClipToPlane (Vector3d.UnitY, new Vector3d (0, cam.Ymin, 0), ref vertices);
			//ClipToPlane (-Vector3d.UnitY, new Vector3d (0, cam.Ymax, 0), ref vertices);
			ClipToPlane (Vector3d.UnitY, new Vector3d (0, 0, 0), ref vertices);
			ClipToPlane (-Vector3d.UnitY, new Vector3d (1, 0, 0), ref vertices);
		}

		private void ClipToPlane (Vector3d normal, Vector3d offset, ref List<Vector3d> vertices)
		{
			List<Vector3d> result = new List<Vector3d> ();
			for (int i = 0; i < vertices.Count; i++)
			{
				int j = (i + 1) % vertices.Count;
				double v1Side = Vector3d.Dot (vertices [i] - offset, normal);
				double v2Side = Vector3d.Dot (vertices [j] - offset, normal);

				// d = offset . normal
				//
				//      d - (P0.normal)
				// t = -----------------
				//      (P-P0).normal

				double t = (Vector3d.Dot (normal, offset) - Vector3d.Dot (normal, vertices [i])) / Vector3d.Dot (normal, vertices [j] - vertices [i]);

				// Save V2
				if ((v1Side > 0) && (v2Side > 0))
				{
					result.Add (vertices [j]);
				}
				// Save V2'
				else if ((v1Side > 0) && (v2Side <= 0))
				{
					result.Add(vertices[i] + (vertices[j]-vertices[i]) * t);
				}
				// Save V1' & V2
				else if ((v1Side <= 0) && (v2Side > 0))
				{
					result.Add(vertices[i] + (vertices[j]-vertices[i]) * t);
					result.Add (vertices [j]);
				}
			}
			//vertices = result;
		}

		public void Rotate (Matrix4d rotMx)
		{
			foreach (Vertex v in _vertices)
			{
				v.Rotate (rotMx);
			}
		}

		public void Transform (Matrix4d trMx)
		{
			foreach (Vertex v in _vertices)
			{
				v.Transform (trMx);
			}
		}

		public Vector3d[] Normals
		{
			get
			{
				List<Vector3d> result = new List<Vector3d> ();

				foreach (Vertex vert in _vertices)
				{
					result.Add (vert.Normal);
				}

				return result.ToArray ();
			}
		}
	}
}

