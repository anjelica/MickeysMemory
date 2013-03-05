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
        private StateMachineLock smLock;
        private bool found;

		public TitleController (CubeSet cubeSet, CubePainter cubePainter, StateMachine sm)
		{
			this.cubeSet = cubeSet;
			this.cubePainter = cubePainter;
			this.sm = sm;
            smLock = new StateMachineLock(sm);
		}

		public void OnSetup(string transition)
		{
			Log.Debug("In TitleController");
            found = false;
            Paint();
            ListenForEvents();
        }
		
		public void OnTick(float n){
            if (sm.Current != "title")
                return;
            // Increments tick counter when state is locked
            if (smLock.Locked)
            {
                smLock.Tick();
            }
            else
            {
                if (found)
                {
                    // Queues transition when cubes are placed in correct order
                    sm.QueueTransition("titleToGame");
                    sm.Tick(1);
                }
            }
        }
		public void OnPaint (bool canvasDirty)
		{
			if (canvasDirty) {
                Paint();
			}
		}
		
		public void OnDispose ()
		{
			cubeSet.ClearEvents ();
		}

        /* Private Methods */
        private void ListenForEvents()
        {
            cubeSet.NeighborAddEvent += NeighborAddHandler;
            cubeSet.NeighborRemoveEvent += NeighborRemoveHandler;
        }

        private void NeighborAddHandler(Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)
        {
            Log.Debug("title cube add");
            CheckCubes();
        }

        /* CheckCubes()
         * Checks if cubes are placed in correct order
         */
        private void CheckCubes()
        {
            Cube[] row = CubeHelper.FindRow(cubeSet);
            if (row.Length == 3)
            {
                if (row[0] == cubeSet[0] &&
                    row[1] == cubeSet[1] &&
                    row[2] == cubeSet[2])
                {
                    found = true;
                    smLock.LockForTickCount(10);
                }
            }
        }

        private void NeighborRemoveHandler(Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)
        {
            // Does nothing, but can check if program recognizes removed cubes
            // with a call to Log.Debug()
        }

        /* Paint()
         * Puts image information on specific cubes that will be used to display the title screen
         */
        private void Paint()
        {
            cubePainter.ClearScreen(cubeSet);

            cubePainter.ClearScreen(cubeSet[0], new Color(255, 0, 0));
            cubePainter.ClearScreen(cubeSet[1], new Color(0, 255, 0));
            cubePainter.ClearScreen(cubeSet[2], new Color(0, 0, 255));

            cubePainter.Commit(cubeSet);
        }
	}
}

