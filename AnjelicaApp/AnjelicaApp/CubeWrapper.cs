using Sifteo;
using System.Collections;
using System;
using System.Collections.Generic;

namespace AnjelicaApp
{
	// ## Wrapper ##
	// "Wrapper" is not a specific API, but a pattern that is used in many Sifteo
	// apps. A wrapper is an object that bundles a Cube object with game-specific
	// data and behaviors.
	public class CubeWrapper {
		
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
		
		public CubeWrapper(AnjelicaApp app, Cube cube) {
			mApp = app;
			mCube = cube;
			mCube.userData = this;
			mIndex = 0;
			mId = Convert.ToInt32(mCube.UniqueId.Substring(0,2));
			Log.Debug("id is: {0}", mId);
			
			// Here we attach more event handlers for button and accelerometer actions.
			mCube.ButtonEvent += OnButton;
			mCube.TiltEvent += OnTilt;
			mCube.ShakeStartedEvent += OnShakeStarted;
			mCube.ShakeStoppedEvent += OnShakeStopped;
			mCube.FlipEvent += OnFlip;
		}
		
		// ## Button ##
		// This is a handler for the Button event. It is triggered when a cube's
		// face button is either pressed or released. The `pressed` argument
		// is true when you press down and false when you release.
		private void OnButton(Cube cube, bool pressed) {
			if (pressed) {
				Log.Debug("Button pressed");
			} else {
				Log.Debug("Button released");
				
				// Advance the image index so that the next image is drawn on this
				// cube.
				if (this.mIndex == 2)
				{
					this.mIndex++;
					/*if (mIndex >= mApp.mImageNames.Length) {
	          mIndex = 0;
	        }*/
					mNeedDraw = true;
				}
			}
		}
		
		// ## Tilt ##
		// This is a handler for the Tilt event. It is triggered when a cube is
		// tilted past a certain threshold. The x, y, and z arguments are filtered
		// values for the cube's three-axis acceleromter. A tilt event is only
		// triggered when the filtered value changes, i.e., when the accelerometer
		// crosses certain thresholds.
		private void OnTilt (Cube cube, int tiltX, int tiltY, int tiltZ)
		{
			Log.Debug ("Tilt: {0} {1} {2}", tiltX, tiltY, tiltZ);
			
			// If the X axis tilt reads 0, the cube is tilting to the left. <br/>
			// If it reads 1, the cube is centered. <br/>
			// If it reads 2, the cube is tilting to the right.
			if (this.mIndex == 3)
			{
				if (tiltX == 0) {
					this.mIndex--;
					this.mNeedDraw = true;
				} else if (tiltX == 1) {
					mXOffset = 0;
				} else if (tiltX == 2) {
					this.mIndex = 0;
					this.mNeedDraw = true;
				}
			}
			
			// If the Y axis tilt reads 0, the cube is tilting down. <br/>
			// If it reads 1, the cube is centered. <br/>
			// If it reads 2, the cube is tilting up.
			if (tiltY == 0) {
			} else if (tiltY == 1) {
				mYOffset = 0;
			} else if (tiltY == 2) {
			}
			
			// If the Z axis tilt reads 2, the cube is face up. <br/>
			// If it reads 1, the cube is standing on a side. <br/>
			// If it reads 0, the cube is face down.
			if (tiltZ == 1) {
			}
			
			mNeedDraw = true;
		}
		
		// ## Shake Started ##
		// This is a handler for the ShakeStarted event. It is triggered when the
		// player starts shaking a cube. When the player stops shaking, a
		// corresponding ShakeStopped event will be fired (see below).
		//
		// Note: while a cube is shaking, it will still fire tilt and flip events
		// as its internal accelerometer goes around and around. If your game wants
		// to treat shaking separately from tilting or flipping, you need to add
		// logic to filter events appropriately.
		private void OnShakeStarted (Cube cube)
		{
			Log.Debug ("Shake start");
			
			// verify that image was "shake". then change
			if (mIndex == 0) 
			{
				this.mIndex = 1;
				mNeedDraw = true;
			}
		}
		
		// ## Shake Stopped ##
		// This is a handler for the ShakeStarted event. It is triggered when the
		// player stops shaking a cube. The `duration` argument tells you
		// how long (in milliseconds) the cube was shaken.
		private void OnShakeStopped(Cube cube, int duration) {
			Log.Debug("Shake stop: {0}", duration);
			mNeedDraw = true;
		}
		
		// ## Flip ##
		// This is a handler for the Flip event. It is triggered when the player
		// turns a cube face down or face up. The `newOrientationIsUp` argument
		// tells you which way the cube is now facing.
		//
		// Note that when a Flip event is triggered, a Tilt event is also
		// triggered.
		private void OnFlip (Cube cube, bool newOrientationIsUp)
		{
			if (this.mIndex == 1) {
				this.mIndex++;
				this.mNeedDraw = true;
			}
			if (newOrientationIsUp) {
				Log.Debug ("Flip face up");
			} else {
				Log.Debug ("Flip face down");
			}
		}
		
		// ## Cube.Image ##
		// This method draws the current image to the cube's display. The
		// Cube.Image method has a lot of arguments, but many of them are optional
		// and have reasonable default values.
		public void DrawSlide() {
			
			// Here we specify the name of the image to draw, in this case by pulling
			// it from the array of names we read out of the image set (see
			// LoadImageIndex, above).
			//
			// When specifying the image name, leave off any file type extensions
			// (png, gif, etc). Refer to the index file that ImageHelper generates
			// during asset conversion.
			//
			// If you specify an image name that is not in the index, the Image call
			// will be ignored.
			String imageName = "images";
			
			// You can specify the top/left point on the screen to start drawing at.
			int screenX = mXOffset;
			int screenY = mYOffset;
			
			// You can draw a portion of an image by specifying coordinates to start
			// reading from (top/left). In this case, we're just going to draw the
			// whole image every time.
			int imageX = 0;
			//int imageY = 0;
			
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
			mCube.FillScreen(Color.Black);
			
			mCube.Image(imageName, screenX, screenY, imageX, Cube.SCREEN_HEIGHT*mIndex, width, height, scale, rotation);
			
			// Remember: always call Paint if you actually want to see anything on the cube's display.
			mCube.Paint();
		}
		
		// This method is called every frame by the Tick in SlideShowApp (see above.)
		public void Tick() {
			
			// You can check whether a cube is being shaken at this moment by looking
			// at the IsShaking flag.
			if (mCube.IsShaking) {
				//mRotation = mApp.mRandom.Next(4);
				mNeedDraw = true;
			}
			
			// If anyone has raised the mNeedDraw flag, redraw the image on the cube.
			if (mNeedDraw) {
				mNeedDraw = false;
				DrawSlide();
			}
		}
	}
}

