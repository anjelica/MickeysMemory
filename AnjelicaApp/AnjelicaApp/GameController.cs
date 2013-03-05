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
        private Cube actionCube;
        private Random random = new Random();

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
            actionCube = cubeSet[random.Next(cubeSet.Count)];
            Paint();
            foreach (Cube cube in cubeSet)
            {
                cube.ButtonEvent += OnButton;
            }

		}

		public void OnTick(float n){}

		public void OnPaint(bool dirtyCanvas){
            if (dirtyCanvas)
            {
                Paint();
            }
        }
		public void OnDispose(){
            cubeSet.ClearEvents();
        }

        /* private methods */
        void Paint()
        {
            cubePainter.ClearScreen(cubeSet);
            foreach (Cube cube in cubeSet)
            {
                if (!cube.Equals(actionCube))
                {
                    cubePainter.ClearScreen(cube, new Color(0, 0, 225));
                }
                else
                {
                    cubePainter.ClearScreen(cube, new Color(255, 0, 225));
                }
            }
            cubePainter.Commit(cubeSet);
        }

        private void OnButton(Cube cube, bool pressed)
        {
            if (pressed)
            {
                Log.Debug("Button pressed");
            }
            else
            {
                Log.Debug("Button released");

                // Advance the image index so that the next image is drawn on this
                // cube.
                if (cube.Equals(actionCube))
                {
                    Log.Debug("Correct!");
                    actionCube = cubeSet[random.Next(cubeSet.Count)];
                    Paint();
                }
                else {
                    Log.Debug("Incorrect :(");
                    sm.QueueTransition("gameToTitle");
                    sm.Tick(1);
                }
            }
        }

	}
}

