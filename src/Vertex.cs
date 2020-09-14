using System;
using SwinGameSDK;
using OpenTK;

namespace MyGame
{
	public class Vertex
	{
		private Vector4d _localPos;
		private Vector3d _localNormal;

		private Vector4d _globalPos;
		private Vector3d _globalNormal;

		public Vertex (Vector4d position, Vector3d normal)
		{
			_localPos = position;
			_localNormal = normal;

			_globalPos = position;
			_globalNormal = normal;
		}

		public Vertex Clone ()
		{
			return new Vertex (_localPos, _localNormal);
		}

		public Vector3d AlignedPos (Camera c)
		{
			//Vector3d result = new Vector3d (_globalPos) - c.Pos;
			//Vector3d.Transform (result, Matrix4d.CreateRotationY () * Matrix4d.CreateRotationX (), Matrix4d.CreateRotationZ ());

			//FIXME The virtex should be multiplied by the camera transformation matrix before its own matrix
			return Vector3d.Transform (new Vector3d (_globalPos), c.AlignmentMatrix);
		}

		public Vector3d ProjectedPos (Camera c)
		{
			//Vector3d result = AlignedPos (c);

			//if (result.Z <= 0)
			//	result.Z = 1;

			/*
			result.X = (c.FOV * result.X) / result.Z;
			result.Y = (c.FOV * result.Y) / result.Z;

			//if (result.Z >= 1)
			result.Z *= c.FOV;
			*/

			Vector3d result = Vector3d.Transform (new Vector3d (_globalPos), c.AlignmentMatrix);
			result = Vector3d.TransformPerspective (result, c.ProjectionMatrix);
			return result;
			//return Vector3d.TransformPerspective (new Vector3d (_globalPos), c.AlignmentMatrix * c.ProjectionMatrix);
		}

		public void Transform (Matrix4d trMx)
		{
			_globalPos = Vector4d.Transform (_localPos, trMx);
		}

		public void Rotate (Matrix4d rotMx)
		{
			_globalNormal = Vector3d.Transform (_localNormal, rotMx);
		}

		public Vector4d LocalPos
		{
			get { return _localPos; }
		}

		public Vector4d Pos
		{
			get { return _globalPos; }
		}

		private Vector3d LocalNormal
		{
			get { return _localNormal; }
		}

		public Vector3d Normal
		{
			get { return _globalNormal; }
		}
	}
}

