using System;
using System.IO;
using System.Collections.Generic;
using OpenTK;

namespace MyGame
{
	public static class ObjMeshLoader
	{
		public static Mesh LoadMesh (string fileName, string meshName = null)
		{
			string toLoad = FindMeshFile (fileName);

			List<Vector4d> vertices = new List<Vector4d> ();
			List<Vector3d> normals = new List<Vector3d> ();
			List<Polygon> polygons = new List<Polygon> ();

			int lineCount = 0;

			if (toLoad == null)
			{
				Console.Error.WriteLine ("Error: File '{0}' does not exist", fileName);
				return null;
			}

			try
			{
				using (StreamReader objFile = File.OpenText (toLoad))
				{
					while (!objFile.EndOfStream)
					{
						string[] l = objFile.ReadLine ().Split (' ');
						lineCount++;

						switch (l [0])
						{
						case "#":
							// Nothing to do, this is a comment
							break;

						case "o":
							if (l.Length > 1)
							{
								if (meshName == null)
									meshName = String.Join (" ", l, 1, l.Length - 1);
							}
							else
								Console.Error.WriteLine ("Warning: Malformed object name on line [{0}] where line reads '{1}'", lineCount, String.Join (" ", l));
							break;

						case "v":
							vertices.Add (ParseVertex (l));
							break;

						case "vn":
							normals.Add (ParseNormal (l));
							break;

						case "f":
							polygons.Add (ParsePolygon (l, vertices, normals));
							break;

						default:
							Console.Error.WriteLine ("Warning: Ignoring line [{0}] where line reads '{1}'. It is possible this command has not been implemented.", lineCount, String.Join (" ", l));
							break;
						}
					}
				}
			}
			catch (Exception e)
			{
				Console.Error.WriteLine ("Error: Oops, something went wrong on line [{0}] when loading file '{1}'\n\tMessage: {2}", lineCount, toLoad, e.Message);
				return null;
			}

			if (meshName == null)
			{
				Console.Error.WriteLine ("Error: No name was given to the mesh, file will be ignored");
			}

			return new Mesh (meshName, polygons);
		}

		private static string FindMeshFile (string fileName)
		{
			if (!fileName.Contains (".obj"))
			{
				fileName += ".obj";
			}

			if (File.Exists (fileName))
			{
				return fileName;
			}

			if (File.Exists ("Resources" + Path.DirectorySeparatorChar + fileName))
			{
				return "Resources" + Path.DirectorySeparatorChar + fileName;
			}

			if (File.Exists ("Resources" + Path.DirectorySeparatorChar + "models" + Path.DirectorySeparatorChar + fileName))
			{
				return "Resources" + Path.DirectorySeparatorChar + "models" + Path.DirectorySeparatorChar + fileName;
			}

			return null;
		}

		private static Vector4d ParseVertex (string[] line)
		{
			double x = double.Parse (line [1]);
			double y = double.Parse (line [2]);
			double z = double.Parse (line [3]);
			double w;

			if (line.Length == 5)
				w = double.Parse (line [4]);
			else
				w = 1;

			return new Vector4d (x, y, z, w);
		}

		private static Vector3d ParseNormal (string[] line)
		{
			double x = double.Parse (line [1]);
			double y = double.Parse (line [2]);
			double z = double.Parse (line [3]);

			return new Vector3d (x, y, z);
		}

		private static Polygon ParsePolygon (string[] line, List<Vector4d> points, List<Vector3d> normals)
		{
			List<Vertex> vertices = new List<Vertex> ();

			int p1 = int.Parse (line [1].Split ('/') [0]) - 1;
			int p2 = int.Parse (line [2].Split ('/') [0]) - 1;
			int p3 = int.Parse (line [3].Split ('/') [0]) - 1;

			Vector3d polyNormal = Vector3d.Cross (new Vector3d (points [p2] - points [p1]), new Vector3d (points [p3] - points [p1]));

			for (int i = 1; i < line.Length; i++)
			{
				string[] vxString = line [i].Split ('/');

				vertices.Add (
					new Vertex (
						points [int.Parse (vxString [0]) - 1],
						(vxString.Length > 2 ? normals [int.Parse (vxString [2]) - 1] : polyNormal)
					)
				);
			}
			return new Polygon (vertices);
		}
	}
}

