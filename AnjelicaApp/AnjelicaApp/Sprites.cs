/* Sprites.cs
 * Stores data for static Sprite Data
 */
using System;
using Sifteo;
using Sifteo.Util;

namespace AnjelicaApp
{
	public class Sprites
	{
		public static SpriteData TITLE1 = new SpriteData("title", 0, 0, 128, 128, 0, 0);
		public static SpriteData TITLE2 = new SpriteData("title", 0, 128, 128, 128, 0, 0);
		public static SpriteData TITLE3 = new SpriteData("title", 0, 256, 128, 128, 0, 0);

		// Images
		public static SpriteData IMAGE1 = new SpriteData("images", 0, 0, 128, 128, 0, 0);
		public static SpriteData IMAGE2 = new SpriteData("images", 0, 128, 128, 128, 0, 0);
		public static SpriteData IMAGE3 = new SpriteData("images", 0, 256, 128, 128, 0, 0);

		// Menu
		/*public static SpriteData MENU1 = new SpriteData ("menu", 0, 0, 128, 128, 0, 0);
		public static SpriteData MENU2 = new SpriteData ("menu", 128, 0, 128, 128, 0, 0);
		public static SpriteData MENU3 = new SpriteData ("menu", 256, 0, 128, 128, 0, 0);		
		
		// Correct Match
		public static SpriteData CORRECT = new SpriteData("score", 0, 0, 128, 128, 0, 0);*/
	}
}

