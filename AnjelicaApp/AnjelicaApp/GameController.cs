using System;
using System.Collections;
using Sifteo;
using System.Collections.Generic;
using Sifteo.Util;

namespace AnjelicaApp
{
	public class GameController : IStateController
	{
		private CubeSet cubeSet;
		private CubePainter cubePainter;
		private StateMachine sm;
        private StateMachineLock smLock;

		public GameController (CubeSet cubeSet, CubePainter cubePainter, StateMachine sm)
		{
            this.cubeSet = cubeSet;
            this.cubePainter = cubePainter;
            this.sm = sm;
            smLock = new StateMachineLock(this.sm);
		}

		public void OnSetup(string transition)
		{
            Log.Debug("GameController Setup");
		}

		public void OnTick(float n){}

		public void OnPaint(bool dirtyCanvas){
            if (dirtyCanvas)
            {
                dirtyCanvas = false;
            }
        }
		public void OnDispose(){
            cubeSet.ClearEvents();
        }
	}
}

