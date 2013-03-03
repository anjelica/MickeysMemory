using System;
using System.Collections;
using Sifteo;
using System.Collections.Generic;
using Sifteo.Util;

namespace AnjelicaApp
{
	public class TitleController : IStateController
	{
		private CubeSet cubeSet;
		private CubePainter cubePainter;
		private StateMachine sm;
		private List<TitleCubeWrapper> tWrappers = new List<TitleCubeWrapper>();
		public AnjelicaApp mApp;

		public TitleController (AnjelicaApp mApp, CubeSet cubeSet, CubePainter cubePainter, StateMachine sm)
		{
			this.mApp = mApp;
			this.cubeSet = cubeSet;
			this.cubePainter = cubePainter;
			this.sm = sm;
		}

		public void OnSetup(string transition)
		{
			Log.Debug("In TitleController");
			// Loop through all the cubes and set them up.
			foreach (Cube cube in cubeSet) {
				
				// Create a wrapper object for each cube. The wrapper object allows us
				// to bundle a cube with extra information and behavior.
				TitleCubeWrapper wrapper = new TitleCubeWrapper(mApp, cube);
				tWrappers.Add(wrapper);
				wrapper.DrawSlide();
			}
			/*cubePainter.ClearScreen(cubeSet);
			cubePainter.PaintSpriteCentered (cubeSet [0], Sprites.TITLE1);
			cubePainter.PaintSpriteCentered (cubeSet [1], Sprites.TITLE2);
			cubePainter.PaintSpriteCentered (cubeSet [2], Sprites.TITLE3);
			cubePainter.Commit(cubeSet);*/
		}
		
		public void OnTick(float n){}
		public void OnPaint (bool canvasDirty)
		{
			if (canvasDirty) {
				canvasDirty = false;
			}
		}
		
		public void OnDispose ()
		{
			cubeSet.ClearEvents ();
		}
	}
}

