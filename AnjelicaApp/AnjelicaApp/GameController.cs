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
        private int eventCount, actionIndex;
        private String[] actions = new String[] {"shake", "flip", "click"};
        private List<Actions> acts;

        public GameController(CubeSet cubeSet, CubePainter cubePainter, StateMachine sm, List<Actions> acts)
		{
            this.cubeSet = cubeSet;
            this.cubePainter = cubePainter;
            this.sm = sm;
            smLock = new StateMachineLock(this.sm);
            this.acts = acts;
		}

		public void OnSetup(string transition)
		{
            Log.Debug("GameController Setup");
            eventCount = 0;
            actionCube = cubeSet[random.Next(cubeSet.Count)];
            actionIndex = random.Next(3);
            cubePainter.ClearScreen(cubeSet);
            cubePainter.Commit(cubeSet);
            listenForEvents();

		}

		public void OnTick(float n){
            if (eventCount == acts.Count)
            {
                Log.Debug("next round!");
                sm.QueueTransition("gameToPattern");
                sm.Tick(1);
            }
        }

		public void OnPaint(bool dirtyCanvas){
            if (dirtyCanvas)
            {
                //Paint();
            }
        }
		public void OnDispose(){
            cubeSet.ClearEvents();
        }

        /* private methods */
        void Paint(Cube cubey, string action)
        {
            cubePainter.ClearScreen(cubeSet);
            foreach (Cube cube in cubeSet)
            {
                if (!cube.Equals(cubey))
                {
                    cubePainter.ClearScreen(cube, new Color(0, 0, 0));
                }
                else
                {
                    Log.Debug("you did: {0}", action);
                    cubePainter.PaintAction(cube, action);
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
                Paint(cube, "click");
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
            eventCount++;
        }

        private void OnShakeStarted(Cube cube)
        { 
            Log.Debug("Shake start");
            cubePainter.PaintAction(cube, "shake");
            if (acts[eventCount].Action.Equals("shake"))
            {
                if (cube.Equals(acts[eventCount].Cube))
                {
                    Log.Debug("Correct!");
                    Paint(cube, "shake");
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
            eventCount++;
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
                cubePainter.PaintAction(cube, "flip");
                if (acts[eventCount].Action.Equals("flip"))
                {
                    if (cube.Equals(acts[eventCount].Cube))
                    {
                        Log.Debug("Correct!");
                        Paint(cube, "flip");
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
            eventCount++;
        }

        private void nextAction(Cube cube)
        {
            foreach (Cube cubey in cubeSet)
            {
                cubey.ClearEvents();
                cubePainter.ClearScreen(cubey, Color.White);
            }
            actionCube = cubeSet[random.Next(cubeSet.Count)];
            actionIndex = random.Next(3);
            listenForEvents();
            //Paint();
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

