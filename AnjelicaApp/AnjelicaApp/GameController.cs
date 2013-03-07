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
        private int score, actionIndex;
        private String[] actions = new String[] {"shake", "flip", "click"};

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
            score = 0;
            actionCube = cubeSet[random.Next(cubeSet.Count)];
            actionIndex = random.Next(3);
            Paint();
            listenForEvents();

		}

		public void OnTick(float n){
        }

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
                    cubePainter.ClearScreen(cube, new Color(0, 0, 0));
                }
                else
                {
                    Log.Debug("{0} me!", actions[actionIndex]);
                    cubePainter.PaintAction(cube, actions[actionIndex]);
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

                if (actionIndex == 2)
                {
                    if (cube.Equals(actionCube))
                    {
                        Log.Debug("Correct!");
                        nextAction(cube);
                    }
                    else
                    {
                        Log.Debug("Wrong cube!");
                        sm.QueueTransition("gameToTitle");
                        sm.Tick(1);
                    }
                }
                else
                {
                    Log.Debug("Wrong action!");
                    sm.QueueTransition("gameToTitle");
                    sm.Tick(1);
                }
            }
        }

        private void OnShakeStarted(Cube cube)
        {
            Log.Debug("Shake start");
            if (actionIndex == 0)
            {
                if (cube.Equals(actionCube))
                {
                    Log.Debug("Correct!");
                    nextAction(cube);
                }
                else
                {
                    Log.Debug("Wrong cube!");
                    sm.QueueTransition("gameToTitle");
                    sm.Tick(1);
                }
            }
            else
            {
                Log.Debug("Wrong action!");
                sm.QueueTransition("gameToTitle");
                sm.Tick(1);
            }
        }

        private void OnShakeStopped(Cube cube, int duration)
        {
            Log.Debug("Shake stop: {0}", duration);

        }

        private void OnFlip(Cube cube, bool newOrientationIsUp)
        {
            if (newOrientationIsUp)
            {
                Log.Debug("Flip face up");
                if (actionIndex == 1)
                {
                    if (cube.Equals(actionCube))
                    {
                        Log.Debug("Correct!");
                        nextAction(cube);
                    }
                    else
                    {
                        Log.Debug("Wrong cube!");
                        sm.QueueTransition("gameToTitle");
                        sm.Tick(1);
                    }
                }
                else
                {
                    Log.Debug("Wrong action!");
                    sm.QueueTransition("gameToTitle");
                    sm.Tick(1);
                }
            }
            else
            {
                Log.Debug("Flip face down");

            }
        }

        private void nextAction(Cube cube)
        {
            foreach (Cube cubey in cubeSet)
            {
                cubey.ClearEvents();
                cubePainter.ClearScreen(cubey, Color.White);
            }
            score++;
            Log.Debug("Score: {0}", score);
            actionCube = cubeSet[random.Next(cubeSet.Count)];
            actionIndex = random.Next(3);
            listenForEvents();
            Paint();
        }

        private void listenForEvents()
        {
            foreach (Cube cube in cubeSet)
            {
                cube.ButtonEvent += OnButton;
                cube.ShakeStartedEvent += OnShakeStarted;
                cube.ShakeStoppedEvent += OnShakeStopped;
                cube.FlipEvent += OnFlip;
            }
        }
	}
}

