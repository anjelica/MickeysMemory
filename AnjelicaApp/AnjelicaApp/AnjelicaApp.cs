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
		public Random mRandom = new Random();
		public int magicId, magicIndex; //TODO
		
		// Here we initialize our app.
		public override void Setup() {
			cubePainter = new CubePainter();
			SetupStateMachine();

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
		

		public override void Tick() {
			// Call current state's OnTick() function
			sm.CurrentState.OnTick (1);
			
			if (canvasDirty) {
				sm.Paint (canvasDirty);
				canvasDirty = false;
			}
		}
	}
	



}

