using System;
using SwinGameSDK;
using OpenTK;

namespace MyGame
{
	public class Camera : TransformableSceneGraphObject, IHasCommands
	{
		private double _aspect;
		private double _fov;

		private double _zMin;
		private double _zMax;

		private int _scrW;
		private int _scrH;

		private readonly Rasterizer _raz;
		private double[,] _zBuff;

		private Matrix4d _proj;

		public Camera (Vector3d pos, Vector3d rot, Vector3d sc, double fov = Math.PI / 2, double zMin = 1, double zMax = 10000)
		{
			GlobalMove (pos);
			GlobalScale (sc);
			GlobalRotate (rot);

			_fov = fov;

			_aspect = 1;

			_zMin = zMin;
			_zMax = zMax;

			_scrW = SwinGame.ScreenWidth ();
			_scrW = SwinGame.ScreenHeight ();

			_proj = Matrix4d.CreatePerspectiveFieldOfView (_fov, _aspect, _zMin, _zMax);

			_raz = new Rasterizer ();
			_zBuff = new double[_scrW + 1, _scrH + 1];
		}

		public Camera (double fov = Math.PI / 2, int zMin = 1, int zMax = 10000) :
			this (Vector3d.Zero, Vector3d.Zero, Vector3d.One, fov, zMin, zMax)
		{
		}

		public void Update ()
		{
			if (_scrW != SwinGame.ScreenWidth () || _scrH != SwinGame.ScreenHeight ())
			{
				_scrW = SwinGame.ScreenWidth ();
				_scrH = SwinGame.ScreenHeight ();

				_aspect = _scrW / _scrH;
				_zBuff = new double[_scrW + 1, _scrH + 1];
				_proj = Matrix4d.CreatePerspectiveFieldOfView (_fov, _aspect, _zMin, _zMax);
			}
			
			for (int j = 0; j <= _scrH; j++)
			{
				for (int i = 0; i <= _scrW; i++)
				{
					_zBuff [i, j] = Zmax;
				}
			}
		}

		public void DrawPixel (Color clr, int x, int y, double z)
		{
			if ((x >= 0) && (x < ScreenWidth) && (y >= 0) && (y < ScreenHeight) && (_zBuff [x, y] > z))
			if ((_zBuff [x, y] > z))
			{
				SwinGame.DrawPixel (clr, x, y);
				_zBuff [x, y] = z;
			}
		}

		public void Render (Polygon poly)
		{
			_raz.Rasterize (poly, this);
		}

		public void AttachCommands (World w)
		{
			w.AttachKeyDownEvent (MoveForwardsAndUp, KeyCode.UpKey);
			w.AttachKeyDownEvent (MoveBackwardsAndDown, KeyCode.DownKey);
			w.AttachKeyDownEvent (MoveLeft, KeyCode.LeftKey);
			w.AttachKeyDownEvent (MoveRight, KeyCode.RightKey);

			w.AttachMouseMoveEvent (RotateXnY);
		}

		private void MoveForwardsAndUp (uint dTime)
		{
			if (SwinGame.KeyDown (KeyCode.ShiftKey))
				LocalMove (0, 0, 0.01 * dTime);
			else
				GlobalMove (0, -0.01 * dTime, 0);
		}

		private void MoveBackwardsAndDown (uint dTime)
		{
			if (SwinGame.KeyDown (KeyCode.ShiftKey))
				LocalMove (0, 0, -0.01 * dTime);
			else
				GlobalMove (0, 0.01 * dTime, 0);
		}

		private void MoveLeft (uint dTime)
		{
			if (SwinGame.KeyDown (KeyCode.ShiftKey))
				GlobalMove (0.01 * dTime, 0, 0);
			else
				LocalMove (0.01 * dTime, 0, 0);
		}

		private void MoveRight (uint dTime)
		{
			if (SwinGame.KeyDown (KeyCode.ShiftKey))
				GlobalMove (-0.01 * dTime, 0, 0);
			else
				LocalMove (-0.01 * dTime, 0, 0);
		}


		private void RotateXnY (Vector dPos)
		{
			LocalRotateX (-dPos.Y / 1000);
			GlobalRotateY (dPos.X / 1000);
		}

		public double FOV
		{
			get { return _fov; }
			set
			{
				if (value <= 0)
					value = 0.001 * Math.PI;
				if (value > Math.PI)
					value = Math.PI;
				
				_fov = value;
				_proj = Matrix4d.CreatePerspectiveFieldOfView (_fov, _aspect, _zMin, _zMax);
			}
		}

		public double Zmin
		{
			get { return _zMin; }
		}

		public double Zmax
		{
			get { return _zMax; }
		}

		public int ScreenWidth
		{
			get { return _scrW; }
		}

		public int ScreenHeight
		{
			get { return _scrH; }
		}

		/*
		public Transformation Transformation
		{
			get { return _trans; }
		}
		*/

		public Matrix4d AlignmentMatrix
		{
			get
			{
				//if (Parent != null)
				//return Parent.TransformationMatrix * Translation * Rotation * Scale * -1;
				return Translation * Rotation * -1;
				//Vector4d forward = Vector4d.Transform (new Vector4d(Vector3d.UnitZ), Rotation);
				//Vector4d up = Vector4d.Transform (new Vector4d(Vector3d.UnitY), Rotation);
				//Vector4d right = Vector4d.Transform (new Vector4d(Vector3d.UnitX), Rotation);

				//return Matrix4d.CreateTranslation() * new Matrix4d (right, up, forward, Vector4d.UnitW);
			}
		}

		public Matrix4d ProjectionMatrix
		{
			get
			{
				return _proj;
			}
		}
	}
}
