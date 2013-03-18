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
        private Random mRandomizer = new Random();

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
                DrawShake(cube);
            }
            else if (action.Equals("flip"))
            {
                DrawFlip(cube);
            }
            else if (action.Equals("click"))
            {
                // yellow
                //ClearScreen(cube, new Color(255, 255, 0));
                DrawClick(cube);
            }
            System.Threading.Thread.Sleep(800);
            ClearScreen(cube);
            cube.Paint();
        }

        public void DrawColoredCircle(Cube cube)
        {

            // Increment the frame counter so that we can keep track of how long
            // we've been running.
            for (int mFrameCount = 0; mFrameCount < 65; mFrameCount++)
            {

                // Pick a point on a circle based on the current frame counter. The
                // cube's screen is 128x128 pixels, so the center of our circle is at
                // (64,64), and it has a radius of 56 pixels.
                double theta = (double)mFrameCount / 30.0 * Math.PI;
                int x = 64 + (int)(56.0 * Math.Cos(theta)) - 2;
                int y = 64 + (int)(56.0 * Math.Sin(theta)) - 2;

                // Pick a random color to draw the dot.
                int r = mRandomizer.Next(256);
                int g = mRandomizer.Next(256);
                int b = mRandomizer.Next(256);
                Color color = new Color(r, g, b);

                // Draw the dot.
                cube.FillRect(color, x, y, 4, 4);

                // Remember to call Paint!
                cube.Paint();
            }
        }

        public void DrawFlip(Cube cube)
        {
            int x = 24;
            int y = 24;
            int width = 80;
            int height = 80;
            Color color = new Color(255, 32, 255);
            cube.FillRect(color, x, y, width, height);

            // F
            cube.FillRect(Color.Black, 42, 45, 2, 32);
            cube.FillRect(Color.Black, 44, 45, 8, 2);
            cube.FillRect(Color.Black, 44, 60, 4, 2);

            // L
            cube.FillRect(Color.Black, 54, 45, 2, 32);
            cube.FillRect(Color.Black, 56, 75, 8, 2);

            // I
            cube.FillRect(Color.Black, 67, 45, 2, 32);

            // P
            cube.FillRect(Color.Black, 74, 45, 2, 32);
            cube.FillRect(Color.Black, 76, 45, 8, 2);
            cube.FillRect(Color.Black, 76, 60, 8, 2);
            cube.FillRect(Color.Black, 82, 45, 2, 15);

            DrawColoredCircle(cube);
        }

        public void DrawClick(Cube cube)
        {
            // ### FillRect ###
            // FillRect draws a rectangle on the cube's screen at a given location
            // in a given size and color. A cube's screen is 128x128 pixels. Here
            // we draw a big square in the center of the screen.
            int x = 24;
            int y = 24;
            int width = 80;
            int height = 80;
            Color color = new Color(255, 145, 0);
            cube.FillRect(color, x, y, width, height);

            // C
            cube.FillRect(Color.Black, 35, 45, 2, 32);
            cube.FillRect(Color.Black, 37, 45, 8, 2);
            cube.FillRect(Color.Black, 37, 75, 8, 2);

            // L
            cube.FillRect(Color.Black, 49, 45, 2, 32);
            cube.FillRect(Color.Black, 51, 75, 8, 2);

            // I
            cube.FillRect(Color.Black, 62, 45, 2, 32);

            // C
            cube.FillRect(Color.Black, 69, 45, 2, 32);
            cube.FillRect(Color.Black, 71, 45, 8, 2);
            cube.FillRect(Color.Black, 71, 75, 8, 2);

            // K
            cube.FillRect(Color.Black, 83, 45, 2, 32);
            cube.FillRect(Color.Black, 85, 60, 6, 2);
            cube.FillRect(Color.Black, 91, 45, 2, 32);
            cube.FillRect(color, 91, 59, 2, 4);

            DrawColoredCircle(cube);
        }

        public void DrawShake(Cube cube)
        {
            int x = 24;
            int y = 24;
            int width = 80;
            int height = 80;
            Color color = new Color(0, 145, 255);
            cube.FillRect(color, x, y, width, height);

            // S
            cube.FillRect(Color.Black, 28, 45, 2, 16);
            cube.FillRect(Color.Black, 36, 60, 2, 16);
            cube.FillRect(Color.Black, 30, 45, 8, 2);
            cube.FillRect(Color.Black, 28, 60, 8, 2);
            cube.FillRect(Color.Black, 28, 75, 8, 2);

            // H
            cube.FillRect(Color.Black, 42, 45, 2, 32);
            cube.FillRect(Color.Black, 44, 60, 6, 2);
            cube.FillRect(Color.Black, 50, 45, 2, 32);

            // A
            cube.FillRect(Color.Black, 56, 45, 2, 32);
            cube.FillRect(Color.Black, 58, 60, 6, 2);
            cube.FillRect(Color.Black, 58, 45, 6, 2);
            cube.FillRect(Color.Black, 64, 45, 2, 32);

            // K
            cube.FillRect(Color.Black, 71, 45, 2, 32);
            cube.FillRect(Color.Black, 73, 60, 6, 2);
            cube.FillRect(Color.Black, 79, 45, 2, 32);
            cube.FillRect(color, 79, 59, 2, 4);

            // E
            cube.FillRect(Color.Black, 86, 45, 2, 32);
            cube.FillRect(Color.Black, 88, 45, 8, 2);
            cube.FillRect(Color.Black, 88, 60, 4, 2);
            cube.FillRect(Color.Black, 88, 75, 8, 2);


            DrawColoredCircle(cube);
        }

        public void DrawHelloWorld(Cube cube)
        {

            Color color = new Color(255, 145, 0);

            // Draw the word "HELLO" to the cube's display.
            cube.FillRect(color, 35, 30, 2, 32);
            cube.FillRect(color, 37, 45, 6, 2);
            cube.FillRect(color, 43, 30, 2, 32);

            cube.FillRect(color, 47, 30, 2, 32);
            cube.FillRect(color, 49, 30, 8, 2);
            cube.FillRect(color, 49, 45, 4, 2);
            cube.FillRect(color, 49, 60, 8, 2);

            cube.FillRect(color, 59, 30, 2, 32);
            cube.FillRect(color, 61, 60, 8, 2);

            cube.FillRect(color, 71, 30, 2, 32);
            cube.FillRect(color, 73, 60, 8, 2);

            cube.FillRect(color, 83, 30, 2, 32);
            cube.FillRect(color, 91, 30, 2, 32);
            cube.FillRect(color, 85, 30, 6, 2);
            cube.FillRect(color, 85, 60, 6, 2);

            // Draw the word "WORLD" to the cube's display.
            cube.FillRect(color, 35, 66, 2, 32);
            cube.FillRect(color, 37, 96, 6, 2);
            cube.FillRect(color, 39, 88, 2, 8);
            cube.FillRect(color, 43, 66, 2, 32);

            cube.FillRect(color, 47, 66, 2, 32);
            cube.FillRect(color, 55, 66, 2, 32);
            cube.FillRect(color, 49, 66, 6, 2);
            cube.FillRect(color, 49, 96, 6, 2);

            cube.FillRect(color, 59, 66, 2, 32);
            cube.FillRect(color, 66, 82, 2, 16);
            cube.FillRect(color, 61, 66, 6, 2);
            cube.FillRect(color, 61, 80, 6, 2);
            cube.FillRect(color, 67, 66, 2, 16);

            cube.FillRect(color, 71, 66, 2, 32);
            cube.FillRect(color, 73, 96, 8, 2);

            cube.FillRect(color, 83, 66, 2, 32);
            cube.FillRect(color, 91, 68, 2, 28);
            cube.FillRect(color, 85, 66, 6, 2);
            cube.FillRect(color, 85, 96, 6, 2);

        }
	}
}
