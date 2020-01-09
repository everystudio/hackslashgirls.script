using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackslashGirls
{
	public class TextureHolder : Singleton<TextureHolder>
	{

		public List<Texture> texture_list = new List<Texture>();

		public Texture GetTexture(string _strTextureName)
		{
			foreach (Texture t in texture_list)
			{
				if (t.name == _strTextureName)
				{
					return t;
				}
			}
			return null;
		}

	}
}