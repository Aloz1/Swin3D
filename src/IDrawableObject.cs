using System;
using System.Collections.Generic;
using OpenTK;

namespace MyGame
{
	public interface IDrawableObject
	{
		void Draw (Camera cam);
	}
}