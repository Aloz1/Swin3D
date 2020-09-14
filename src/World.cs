using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
	public delegate void KeyCmd (uint dTime);
	public delegate void MouseCmd (uint dTime);
	public delegate void MouseMoveCmd (Vector dPos);

	public class World : SceneGraphObject, IDrawableObject
	{
		private Vector _lastMousePos;
		private Timer _timer;
		private uint _dTime;

		private Dictionary<KeyCode, KeyCmd> _keyDownCommands;
		private Dictionary<MouseButton, MouseCmd> _mouseBtnDownCommands;
		private MouseMoveCmd _mouseMoveCommands;

		public World ()
		{
			_timer = new Timer ();

			_keyDownCommands = new Dictionary<KeyCode, KeyCmd> ();
			_mouseBtnDownCommands = new Dictionary<MouseButton, MouseCmd> ();
			_mouseMoveCommands = null;

			_lastMousePos = new Vector ();
		}

		public void Update ()
		{
			_dTime = _timer.Ticks;
			_timer.Reset ();
			_timer.Start (); // Make sure timer is always running (I'm assuming this does nothing if the timer is already running)


			// Mouse Delta stuff
			Vector nextMousePos = SwinGame.MousePositionAsVector ();
			Vector dMousePos = nextMousePos - _lastMousePos;

			if (nextMousePos.X >= (SwinGame.ScreenWidth () - 1))
				nextMousePos.X -= (SwinGame.ScreenWidth () - 2);
			else if (nextMousePos.X <= 0)
				nextMousePos.X += (SwinGame.ScreenWidth () - 2);

			if (nextMousePos.Y >= (SwinGame.ScreenHeight () - 1))
				nextMousePos.Y -= (SwinGame.ScreenHeight () - 2);
			else if (nextMousePos.Y <= 0)
				nextMousePos.Y += SwinGame.ScreenHeight () - 2;
			
			SwinGame.MoveMouse (nextMousePos);
			_lastMousePos = nextMousePos;
			

			if (_mouseMoveCommands != null)
				_mouseMoveCommands (dMousePos);

			foreach (KeyCode key in _keyDownCommands.Keys)
				if (SwinGame.KeyDown (key))
					_keyDownCommands [key] (DeltaTime);

			foreach (MouseButton mBtn in _mouseBtnDownCommands.Keys)
				if (SwinGame.MouseDown (mBtn))
					_mouseBtnDownCommands [mBtn] (DeltaTime);
		}

		public void Draw (Camera cam)
		{
			DrawChildren (cam);
		}

		public void AttachKeyDownEvent (KeyCmd c, KeyCode k)
		{
			if (_keyDownCommands.ContainsKey (k))
				_keyDownCommands [k] += c;
			else
				_keyDownCommands.Add (k, c);
		}

		public void AttachMouseBtnDownEvent (MouseCmd c, MouseButton mb)
		{
			if (_mouseBtnDownCommands.ContainsKey (mb))
				_mouseBtnDownCommands [mb] += c;
			else
				_mouseBtnDownCommands.Add (mb, c);
		}

		public void AttachMouseMoveEvent (MouseMoveCmd mov)
		{
			_mouseMoveCommands += mov;
		}

		public uint DeltaTime
		{
			get { return _dTime; }
		}
	}
}

