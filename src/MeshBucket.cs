using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;

namespace MyGame
{
	public class MeshBucket
	{
		private Dictionary<string, Mesh> _meshes;

		public MeshBucket ()
		{
			_meshes = new Dictionary<string, Mesh> ();
		}

		public void LoadMesh (string fileName, string meshName = null)
		{
			Mesh mesh = ObjMeshLoader.LoadMesh (fileName, meshName);

			if (mesh != null)
			{
				try
				{
					_meshes.Add (mesh.Name.ToLower (), mesh);
				}
				catch (Exception e)
				{
					Console.Error.WriteLine ("Error: Mesh file not loaded - " + e.Message);
				}
			}
			else
			{
				Console.Error.WriteLine ("Error: Mesh file not loaded");
			}
		}

		public Mesh CreateInstance (string meshId)
		{
			try
			{
				return _meshes [meshId.ToLower ()].Clone ();
			}
			catch (Exception e)
			{
				Console.Error.WriteLine ("Error: Unable to create mesh instance '{0}'\n{1}", meshId.ToLower (), e.Message);
				return null;
			}
		}
	}
}
