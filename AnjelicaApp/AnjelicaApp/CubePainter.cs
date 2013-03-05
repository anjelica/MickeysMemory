/* CubePainter.cs
 * Written by Sean Voisen and modified for Mickey's Memory
 * Used to handle image data to be displayed on cubes
 */

using System;
using Sifteo;
using Sifteo.Util;

namespace AnjelicaApp
{
	public class CubePainter
	{
		public void PaintAllImage (CubeSet cubes, String imageName)
		{
			foreach (Cube cube in cubes) {
				PaintFullImage (cube, imageName);
			}
		}
		
		public void PaintImage (Cube cube, string imageName, int x, int y, int width, int height)
		{
			cube.Image (imageName, x, y, 0, 0, width, height, 1, 0);
		}
		
		public void PaintSprite (Cube cube, SpriteData data, int x, int y)
		{
			cube.Image (data.imageName, x, y, data.source.x, data.source.y, data.size.x, data.size.y, 1, 0);
		}
		
		public void PaintSpriteCentered (Cube cube, SpriteData data)
		{
			int x = (int)((Cube.SCREEN_WIDTH - data.size.x) * 0.5);
			int y = (int)((Cube.SCREEN_HEIGHT - data.size.y) * 0.5);
			
			cube.Image (data.imageName, x, y, data.source.x, data.source.y, data.size.x, data.size.y, 1, 0);
		}
		
		/* PaintFullImage()
		 * Paints 128x128 image on cube
		 */
		public void PaintFullImage (Cube cube, string imageName)
		{
			cube.Image (imageName, 0, 0, 0, 0, Cube.SCREEN_WIDTH, Cube.SCREEN_HEIGHT, 1, 0);
		}
		
		/* ClearScreen()
		 * Fill cube with black color
		 */
		public void ClearScreen (Cube cube)
		{
			cube.FillScreen (Color.Black);	
		}
		
		public void ClearScreen (Cube cube, Color c)
		{
			cube.FillScreen (c);	
		}
		
		public void ClearScreen (CubeSet cubeSet)
		{
			foreach (Cube cube in cubeSet) {
				ClearScreen (cube);	
			}
		}
		
		/* Commit()
		 * Commits data stored to be painted onto cubes and displays them
		 */
		public void Commit (CubeSet cubeSet)
		{
			foreach (Cube cube in cubeSet) {
				cube.Paint ();
			}
		}
		
		public void Commit (Cube cube)
		{
			cube.Paint ();
		}

        public void PaintAction(Cube cube, String action)
        { 
            if (action.Equals("shake"))
            {
                // purple
                ClearScreen(cube, new Color(127, 0, 255));
            }
            else if (action.Equals("tilt"))
            {
                // pink
                ClearScreen(cube, new Color(255, 0, 255));
            }
            else if (action.Equals("flip"))
            {
                // light blue
                ClearScreen(cube, new Color(0, 127, 255));
            }
            else if (action.Equals("click"))
            {
                // yellow
                ClearScreen(cube, new Color(255, 255, 0));
            }
        }
	}
}
