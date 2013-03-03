using System;
using Sifteo;
using Sifteo.Util;

namespace AnjelicaApp
{
	public class TitleCubeWrapper
	{
		
		public AnjelicaApp mApp;
		public Cube mCube;
		public int mIndex;
		public int mXOffset = 0;
		public int mYOffset = 0;
		public int mScale = 1;
		public int mRotation = 0;
		public int mId;
		
		// This flag tells the wrapper to redraw the current image on the cube. (See Tick, below).
		public bool mNeedDraw = false;
		
		public TitleCubeWrapper (AnjelicaApp app, Cube cube)
		{
			mApp = app;
			mCube = cube;
			mCube.userData = this;
			mIndex = 0;
			mId = Convert.ToInt32(mCube.UniqueId.Substring(0,2));
			Log.Debug("id is: {0}", mId);
		}
		
		public void DrawSlide ()
		{
			String imageName = "title"; 
			
			// You can specify the top/left point on the screen to start drawing at.
			int screenX = mXOffset;
			int screenY = mYOffset;
			
			// You can draw a portion of an image by specifying coordinates to start
			// reading from (top/left). In this case, we're just going to draw the
			// whole image every time.
			int imageX = mIndex * 128;
			int imageY = 0;
			
			// You should always specify the width and height of the image to be
			// drawn. If you specify values that are less than the size of the image,
			// only the portion you specify will be drawn. If you specify values
			// larger than the image, the behavior is undefined (so don't do that).
			//
			// In this example, we assume that the image is 128x128, big enough to
			// cover the full size of the display. If the image runs off the sides of
			// the display (because of offsets due to tilting; see OnTilt, above), it
			// will be clipped.
			int width = 128;
			int height = 128;
			
			// You can upscale an image by integer multiples. A scaled image still
			// starts drawing at the specified top/left point, but the area of the
			// display it covers (width/height) will be multipled by the scale.
			//
			// The default value is 1 (1:1 scale).
			int scale = mScale;
			
			// You can rotate an image by quarters. The rotation value is an integer
			// representing counterclockwise rotation.
			//
			// * 0 = no rotation
			// * 1 = 90 degrees counterclockwise
			// * 2 = 180 degrees
			// * 3 = 90 degrees clockwise
			//
			// A rotated image still starts drawing at the specified top/left point;
			// the pixels are just drawn in rotated order.
			//
			// The default value is 0 (no rotation).
			int rotation = mRotation;
			
			// Clear off whatever was previously on the display before drawing the new image.
			mCube.FillScreen (Color.White);
			
			mCube.Image (imageName, screenX, screenY, imageX, imageY, width, height, scale, rotation);
			
			Log.Debug ("{0} {1} {2}", imageName, imageX, imageY);
			
			// Remember: always call Paint if you actually want to see anything on the cube's display.
			mCube.Paint ();
		}
		
		// This method is called every frame by the Tick in SlideShowApp (see above.)
		public void Tick ()
		{
			
		}
		
	}
	
}

