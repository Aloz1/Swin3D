using System;
using OpenTK;
using SwinGameSDK;

namespace MyGame
{
	public class TransformableSceneGraphObject : SceneGraphObject
	{
		private Vector3d _position;
		private Vector3d _scale;
		private Quaterniond _rotation;

		private Matrix4d _transMatrix;
		private Matrix4d _rotMatrix;
		private Matrix4d _scMatrix;

		public TransformableSceneGraphObject ()
		{
			_position = Vector3d.Zero;
			_rotation = Quaterniond.Identity;
			_scale = Vector3d.One;

			_transMatrix = Matrix4d.CreateTranslation (_position);
			_scMatrix = Matrix4d.Scale (_scale);
			_rotMatrix = Matrix4d.CreateFromQuaternion (_rotation);
		}

		// Movement
		public void GlobalMove (Vector3d mov)
		{

			_position += mov;
			_transMatrix = Matrix4d.CreateTranslation (_position);
			TransformChange ();
			MoveChange ();
		}

		public void GlobalMove (double x, double y, double z)
		{
			GlobalMove (new Vector3d (x, y, z));
		}

		public void LocalMove (Vector3d mov)
		{
			_position += Vector3d.Transform (mov, _rotation.Inverted ());
			_transMatrix = Matrix4d.CreateTranslation (_position);
			TransformChange ();
			MoveChange ();
		}

		public void LocalMove (double x, double y, double z)
		{
			LocalMove (new Vector3d (x, y, z));
		}
		// End Movement

		// Scale
		public void GlobalScale (Vector3d sc)
		{
			_scale *= Vector3d.Transform (sc, _rotMatrix);
			_scMatrix = Matrix4d.Scale (_scale);
			TransformChange ();
			ScaleChange ();
		}

		public void GlobalScale (double x, double y, double z)
		{
			GlobalScale (new Vector3d (x, y, z));
		}

		public void LocalScale (Vector3d sc)
		{
			_scale *= sc;
			_scMatrix = Matrix4d.Scale (_scale);
			TransformChange ();
			ScaleChange ();
		}

		public void LocalScale (double x, double y, double z)
		{
			LocalScale (new Vector3d (x, y, z));
		}
		// End Scale

		// Rotation
		public void GlobalRotate (double x, double y, double z)
		{
			GlobalRotateZ (z);
			GlobalRotateX (x);
			GlobalRotateY (y);
		}

		public void GlobalRotate (Vector3d rot)
		{
			GlobalRotate (rot.X, rot.Y, rot.Z);
		}

		public void GlobalRotateX (double a)
		{
			Vector3d normal = Vector3d.TransformNormal (Vector3d.UnitX, _rotMatrix);
			_rotation = new Quaterniond (
				normal.X * Math.Sin (a * Math.PI / 2),
				normal.Y * Math.Sin (a * Math.PI / 2),
				normal.Z * Math.Sin (a * Math.PI / 2),
				Math.Cos (a * Math.PI / 2)
			) * _rotation;

			_rotMatrix = Matrix4d.CreateFromQuaternion (_rotation);

			TransformChange ();
			RotateChange ();
		}

		public void GlobalRotateY (double a)
		{
			Vector3d normal = Vector3d.TransformNormal (Vector3d.UnitY, _rotMatrix);
			_rotation = new Quaterniond (
				normal.X * Math.Sin (a * Math.PI / 2),
				normal.Y * Math.Sin (a * Math.PI / 2),
				normal.Z * Math.Sin (a * Math.PI / 2),
				Math.Cos (a * Math.PI / 2)
			) * _rotation;

			_rotMatrix = Matrix4d.CreateFromQuaternion (_rotation);

			TransformChange ();
			RotateChange ();
		}

		public void GlobalRotateZ (double a)
		{
			Vector3d normal = Vector3d.TransformNormal (Vector3d.UnitZ, _rotMatrix);
			_rotation = new Quaterniond (
				normal.X * Math.Sin (a * Math.PI / 2),
				normal.Y * Math.Sin (a * Math.PI / 2),
				normal.Z * Math.Sin (a * Math.PI / 2),
				Math.Cos (a * Math.PI / 2)
			) * _rotation;

			_rotMatrix = Matrix4d.CreateFromQuaternion (_rotation);

			TransformChange ();
			RotateChange ();
		}

		public void LocalRotate (double x, double y, double z)
		{
			LocalRotateX (x);
			LocalRotateY (y);
			LocalRotateZ (z);
		}

		public void LocalRotateX (double a)
		{
			_rotation = new Quaterniond (
				Math.Sin (a * Math.PI / 2),
				0,
				0,
				Math.Cos (a * Math.PI / 2)
			) * _rotation;

			_rotMatrix = Matrix4d.CreateFromQuaternion (_rotation);

			TransformChange ();
			RotateChange ();
		}

		public void LocalRotateY (double a)
		{
			_rotation = new Quaterniond (
				0,
				Math.Sin (a * Math.PI / 2),
				0,
				Math.Cos (a * Math.PI / 2)
			) * _rotation;

			_rotMatrix = Matrix4d.CreateFromQuaternion (_rotation);

			TransformChange ();
			RotateChange ();
		}

		public void LocalRotateZ (double a)
		{
			_rotation = new Quaterniond (
				0,
				0,
				Math.Sin (a * Math.PI / 2),
				Math.Cos (a * Math.PI / 2)
			) * _rotation;

			_rotMatrix = Matrix4d.CreateFromQuaternion (_rotation);

			TransformChange ();
			RotateChange ();
		}
		// End Rotation

		public void DrawInfo (Color c, int x, int y)
		{
			SwinGame.DrawText ("Translation: " + _position.X + ", " + _position.Y + ", " + _position.Z, c, x, y);
			SwinGame.DrawText ("Scale: " + _scale.X + ", " + _scale.Y + ", " + _scale.Z, c, x, y + 10);
			SwinGame.DrawText ("Rotation: " + _rotation.X + ", " + _rotation.Y + ", " + _rotation.Z + ", " + _rotation.W, c, x, y + 20);
		}

		public Matrix4d Transformation
		{
			get { return _scMatrix * _rotMatrix * _transMatrix; }
			//get { return  _transMatrix * _rotMatrix * _scMatrix; }
		}

		public Matrix4d Translation
		{
			get { return _transMatrix; }
		}

		public Matrix4d Rotation
		{
			get { return _rotMatrix; }
		}

		public Matrix4d Scale
		{
			get { return _scMatrix; }
		}

		protected virtual void TransformChange ()
		{
		}

		protected virtual void MoveChange ()
		{
		}

		protected virtual void RotateChange ()
		{
		}

		protected virtual void ScaleChange ()
		{
		}
	}
}

