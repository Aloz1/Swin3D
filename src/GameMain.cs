using System;
using SwinGameSDK;
using OpenTK;

namespace MyGame
{
	public class GameMain
	{
		public static void Main ()
		{
			MeshBucket meshBucket = new MeshBucket ();
			meshBucket.LoadMesh ("monkey2.obj");
			//meshBucket.LoadMesh ("monkey.obj");
			meshBucket.LoadMesh ("icosphere.obj");
			meshBucket.LoadMesh ("triangle.obj", "triangle");

			World world = new World ();
			Camera theCam = new Camera ();

			Mesh icofl = meshBucket.CreateInstance ("icosphere");
			icofl.GlobalMove (new Vector3d (-0, 0, -20));
			icofl.LocalScale (new Vector3d (5, 5, 5));
				
			Mesh icofm = meshBucket.CreateInstance ("icosphere");
			icofm.GlobalMove (new Vector3d (0, 0, -20));
			icofm.LocalScale (new Vector3d (5, 5, 5));

			Mesh icofr = meshBucket.CreateInstance ("suzanne");
			icofr.GlobalMove (new Vector3d (20, 0, -20));
			icofr.LocalScale (new Vector3d (5, 5, 5));

			Mesh icoml = meshBucket.CreateInstance ("icosphere");
			icoml.GlobalMove (new Vector3d (-20, 0, 0));
			icoml.LocalScale (new Vector3d (5, 5, 5));

			Mesh icomr = meshBucket.CreateInstance ("icosphere");
			icomr.GlobalMove (new Vector3d (20, 0, 0));
			icomr.LocalScale (new Vector3d (5, 5, 5));

			Mesh icobl = meshBucket.CreateInstance ("icosphere");
			icobl.GlobalMove (new Vector3d (-20, 0, 20));
			icobl.LocalScale (new Vector3d (5, 5, 5));

			Mesh icobm = meshBucket.CreateInstance ("icosphere");
			icobm.GlobalMove (new Vector3d (0, 0, 20));
			icobm.LocalScale (new Vector3d (5, 5, 5));

			Mesh icobr = meshBucket.CreateInstance ("icosphere");
			icobr.GlobalMove (new Vector3d (20, 0, 20));
			icobr.LocalScale (new Vector3d (5, 5, 5));
		
			world.AttachChild (icofl);
			//world.AttachChild (icofm);
			world.AttachChild (icofr);
			//world.AttachChild (icoml);
			//world.AttachChild (icomr);
			//world.AttachChild (icobl);
			//world.AttachChild (icobm);
			//world.AttachChild (icobr);

			//icosphere.AttachCommands (world);
			theCam.AttachCommands (world);

			//Open the game window
			SwinGame.OpenGraphicsWindow ("GameMain", 800, 600);
            
			//Run the game loop
			while (!SwinGame.WindowCloseRequested () && !SwinGame.KeyDown (KeyCode.EscapeKey))
			{
				//Fetch the next batch of UI interaction
				SwinGame.ProcessEvents ();

				world.Update ();
				theCam.Update ();

				// Mesh controls

				// Camera controls
                
				//Clear the screen and draw the framerate
				SwinGame.ClearScreen (Color.White);
				/*
				Color pixColor = Color.Navy;

				for (int j = 0; j <= 600; j++)
				{
					for (int i = 0; i <= 800; i++)
					{
						SwinGame.DrawPixel (pixColor, i, j);
					}
				}
				*/

				SwinGame.DrawFramerate (0, 0);

				//icosphere.DrawInfo (Color.DarkGreen, 0, 10);
				theCam.DrawInfo (Color.DarkOrange, 0, 40);

				world.Draw (theCam);
                
				//Draw onto the screen
				SwinGame.RefreshScreen (60);
			}
		}
	}
}