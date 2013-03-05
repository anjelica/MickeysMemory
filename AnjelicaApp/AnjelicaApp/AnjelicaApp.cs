using Sifteo;
using System.Collections;
using System;
using System.Collections.Generic;
using Sifteo.Util;

namespace AnjelicaApp
{
  public class AnjelicaApp : BaseApp
  {
		private TitleController titleController;
		private GameController gameController;
		private CubePainter cubePainter;
		private StateMachine sm;
		private Boolean canvasDirty;

		public String[] mImageNames;
		public List<CubeWrapper> mWrappers = new List<CubeWrapper>();
		public Random mRandom = new Random();
		public int magicId, magicIndex; //TODO
		
		// Here we initialize our app.
		public override void Setup() {
			cubePainter = new CubePainter();
			SetupStateMachine();

			// Load up the list of images.
			mImageNames = LoadImageIndex();
			/*
			// Loop through all the cubes and set them up.
			foreach (Cube cube in CubeSet) {
				
				// Create a wrapper object for each cube. The wrapper object allows us
				// to bundle a cube with extra information and behavior.
				CubeWrapper wrapper = new CubeWrapper(this, cube);
				mWrappers.Add(wrapper);
				wrapper.DrawSlide();
			}
			
			// ## Event Handlers ##
			// Objects in the Sifteo API (particularly BaseApp, CubeSet, and Cube)
			// fire events to notify an app of various happenings, including actions
			// that the player performs on the cubes.
			//
			// To listen for an event, just add the handler method to the event. The
			// handler method must have the correct signature to be added. Refer to
			// the API documentation or look at the examples below to get a sense of
			// the correct signatures for various events.
			//
			// **NeighborAddEvent** and **NeighborRemoveEvent** are triggered when
			// the player puts two cubes together or separates two neighbored cubes.
			// These events are fired by CubeSet instead of Cube because they involve
			// interaction between two Cube objects. (There are Cube-level neighbor
			// events as well, which comes in handy in certain situations, but most
			// of the time you will find the CubeSet-level events to be more useful.)
			CubeSet.NeighborAddEvent += OnNeighborAdd;
			CubeSet.NeighborRemoveEvent += OnNeighborRemove;*/
		}

		public void SetupStateMachine()
		{
			sm = new StateMachine();

			// Initialize Controllers
			titleController = new TitleController (this.CubeSet, this.cubePainter, sm);
			/*menuController = new MenuController (this.CubeSet,this.cubePainter,sm);*/
			gameController = new GameController (this.CubeSet, this.cubePainter, sm);

			// Set states and transitions
			sm.State("title", titleController);
			sm.State("game", gameController);

			sm.Transition("null", "nullToTitle", "title");
			sm.Transition("title", "titleToGame", "game");
            sm.Transition("game", "gameToTitle", "title");

			sm.SetState("title", "nullToTitle");
		}
		
		// ## Neighbor Add ##
		// This method is a handler for the NeighborAdd event. It is triggered when
		// two cubes are placed side by side.
		//
		// Cube1 and cube2 are the two cubes that are involved in this neighboring.
		// The two cube arguments can be in any order; if your logic depends on
		// cubes being in specific positions or roles, you need to add logic to
		// this handler to sort the two cubes out.
		//
		// Side1 and side2 are the sides that the cubes neighbored on.
		private void OnNeighborAdd(Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)  {
			Log.Debug("Neighbor add: {0}.{1} <-> {2}.{3}", cube1.UniqueId, side1, cube2.UniqueId, side2);

			CubeWrapper wrapper = (CubeWrapper)cube1.userData;
			if (wrapper != null) {
				// Here we set our wrapper's rotation value so that the image gets
				// drawn with its top side pointing towards the neighbor cube.
				//
				// Cube.Side is an enumeration (TOP, LEFT, BOTTOM, RIGHT, NONE). The
				// values of the enumeration can be cast to integers by counting
				// counterclockwise:
				//
				// * TOP = 0
				// * LEFT = 1
				// * BOTTOM = 2
				// * RIGHT = 3
				// * NONE = 4
				wrapper.mRotation = (int)side1;
				wrapper.mNeedDraw = true;
			}
			
			wrapper = (CubeWrapper)cube2.userData;
			if (wrapper != null) {
				wrapper.mRotation = (int)side2;
				wrapper.mNeedDraw = true;
			}
			
		}
		
		// ## Neighbor Remove ##
		// This method is a handler for the NeighborRemove event. It is triggered
		// when two cubes that were neighbored are separated.
		//
		// The side arguments for this event are the sides that the cubes
		// _were_ neighbored on before they were separated. If you check the
		// current state of their neighbors on those sides, they should of course
		// be NONE.
		private void OnNeighborRemove(Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)  {
			Log.Debug("Neighbor remove: {0}.{1} <-> {2}.{3}", cube1.UniqueId, side1, cube2.UniqueId, side2);
			
			CubeWrapper wrapper = (CubeWrapper)cube1.userData;
			if (wrapper != null) {
				wrapper.mScale = 1;
				wrapper.mRotation = 0;
				wrapper.mNeedDraw = true;
			}
			
			wrapper = (CubeWrapper)cube2.userData;
			if (wrapper != null) {
				wrapper.mScale = 1;
				wrapper.mRotation = 0;
				wrapper.mNeedDraw = true;
			}
		}

		public override void Tick() {
			// Call current state's OnTick() function
			sm.CurrentState.OnTick (1);
			
			if (canvasDirty) {
				sm.Paint (canvasDirty);
				canvasDirty = false;
			}
		}
		
		// ImageSet is an enumeration of your app's images. It is populated based
		// on your app's siftbundle and index. You rarely have to interact with it
		// directly, since you can refer to images by name.
		//
		// In this method, we scan the image set to build an array with the names
		// of all the images.
		private String[] LoadImageIndex() {
			ImageSet imageSet = this.Images;
			ArrayList nameList = new ArrayList();
			foreach (ImageInfo image in imageSet) {
				nameList.Add(image.name);
				Log.Debug("{0}", image.name);
			}
			String[] rv = new String[nameList.Count];
			for (int i=0; i<nameList.Count; i++) {
				rv[i] = (string)nameList[i];
			}
			return rv;
		}
		
	}
	
	// ------------------------------------------------------------------------
	


}

