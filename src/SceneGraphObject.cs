using System;
using System.Collections.Generic;
using OpenTK;

namespace MyGame
{
	public class SceneGraphObject
	{
		private SceneGraphObject _parent;
		private List<SceneGraphObject> _children;

		public SceneGraphObject ()
		{
			_parent = null;
			_children = new List<SceneGraphObject> ();
		}

		public void DrawChildren (Camera camera)
		{
			foreach (SceneGraphObject obj in Children)
			{
				if (obj is IDrawableObject)
					(obj as IDrawableObject).Draw (camera);
				obj.DrawChildren (camera);
			}
		}

		public void AttachChild (SceneGraphObject dr)
		{
			if (dr.Parent != null)
				dr.Parent.RemoveChild (dr);

			_children.Add (dr);
			dr.Parent = this;
		}

		public void RemoveChild (SceneGraphObject dr)
		{
			dr.Parent = null;
			_children.Remove (dr);
		}

		public virtual Matrix4d TransformationMatrix
		{
			get
			{
				Matrix4d pTransform = Matrix4d.Identity;

				if (_parent != null)
					pTransform = _parent.TransformationMatrix;
				
				if (this is TransformableSceneGraphObject)
					return pTransform * (this as TransformableSceneGraphObject).Transformation;
				else
					return pTransform;
			}
		}

		public SceneGraphObject[] Children
		{
			get { return _children.ToArray (); }
		}

		public SceneGraphObject Parent
		{
			get { return _parent; }

			set
			{
				foreach (SceneGraphObject d in value.Children)
				{
					if (d == this)
					{
						_parent = value;
						return;
					}
				}

				value.AttachChild (this);
			}
		}
	}
}

