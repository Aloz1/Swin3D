using System;
using System.Collections.Generic;
using SwinGameSDK;
using OpenTK;

namespace MyGame
{
	public class Mesh : TransformableSceneGraphObject, IDrawableObject, IHasCommands
	{
		string _name;
		List<Polygon> _polygons;

		public Mesh (Vector3d pos, Vector3d rot, Vector3d sc, string name, List<Polygon> polys)
		{
			_name = name;
			_polygons = polys;

			GlobalMove (pos);
			GlobalScale (sc);
			GlobalRotate (rot);
		}

		public Mesh (string name, List<Polygon> polys) :
			this (Vector3d.Zero, Vector3d.Zero, Vector3d.One, name, polys)
		{
		}

		public Mesh Clone ()
		{
			List<Polygon> clonedList = new List<Polygon> ();
			foreach (Polygon p in _polygons)
			{
				clonedList.Add (p.Clone ());
			}
			return new Mesh (_name, clonedList);
		}

		public void Draw (Camera cam)
		{
			foreach (Polygon poly in _polygons)
			{
				cam.Render (poly);
			}
		}
			
		protected override void TransformChange ()
		{
			for (int i = 0; i < _polygons.Count; i++)
			{
				_polygons [i].Transform (Transformation);
			}
		}

		protected override void RotateChange ()
		{
			for (int i = 0; i < _polygons.Count; i++)
			{
				_polygons [i].Rotate (Rotation);
			}
		}

		public void AttachCommands (World world)
		{
			world.AttachKeyDownEvent (MoveBackwardsAndUp, KeyCode.WKey);
			world.AttachKeyDownEvent (MoveForwardsAndDown, KeyCode.SKey);
			world.AttachKeyDownEvent (MoveLeft, KeyCode.AKey);
			world.AttachKeyDownEvent (MoveRight, KeyCode.DKey);

			world.AttachKeyDownEvent (RotateAboutZplus, KeyCode.QKey);
			world.AttachKeyDownEvent (RotateAboutZminus, KeyCode.EKey);
			world.AttachKeyDownEvent (RotateAboutXplus, KeyCode.RKey);
			world.AttachKeyDownEvent (RotateAboutXminus, KeyCode.FKey);
		}

		private void MoveBackwardsAndUp (uint dTime)
		{
			if (SwinGame.KeyDown (KeyCode.ShiftKey))
				GlobalMove (0, 0, 0.01 * dTime);
			else
				GlobalMove (0, -0.01 * dTime, 0);
		}

		private void MoveForwardsAndDown (uint dTime)
		{
			if (SwinGame.KeyDown (KeyCode.ShiftKey))
				GlobalMove (0, 0, -0.01 * dTime);
			else
				GlobalMove (0, 0.01 * dTime, 0);
		}

		private void MoveLeft (uint dTime)
		{
			GlobalMove (0.01 * dTime, 0, 0);
		}

		private void MoveRight (uint dTime)
		{
			GlobalMove (-0.01 * dTime, 0, 0);
		}


		private void RotateAboutZplus (uint dTime)
		{
			GlobalRotateZ (0.001 * dTime);
		}

		private void RotateAboutZminus (uint dTime)
		{
			GlobalRotateZ (-0.001 * dTime);
		}

		private void RotateAboutXplus (uint dTime)
		{
			GlobalRotateX (0.001 * dTime);
		}

		private void RotateAboutXminus (uint dTime)
		{
			GlobalRotateX (-0.001 * dTime);
		}

		/*
		public Transformation Transformation
		{
			get { return _trans; }
		}
		*/

		public string Name
		{
			get { return _name; }
		}
	}
}

