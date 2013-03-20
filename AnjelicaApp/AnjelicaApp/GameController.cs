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
        private Random random = new Random();
        private int eventCount;
        private String[] actions = new String[] {"shake", "flip", "click"};
        private List<Actions> acts;
        private Sound incorrect;
        private Sound correct;
        private SoundSet sounds;

        public GameController(CubeSet cubeSet, CubePainter cubePainter, StateMachine sm, List<Actions> acts, SoundSet sounds)
		{
            this.cubeSet = cubeSet;
            this.cubePainter = cubePainter;
            this.sm = sm;
            smLock = new StateMachineLock(this.sm);
            this.acts = acts;
            this.sounds = sounds;
		}

		public void OnSetup(string transition)
		{
            Log.Debug("GameController Setup");
            correct = sounds.CreateSound("correct");
            incorrect = sounds.CreateSound("incorrect");
            eventCount = 0;
            cubePainter.PaintAllImage(cubeSet, "go");
            cubePainter.Commit(cubeSet);
            listenForEvents();

		}

		public void OnTick(float n){
            if (sm.Current != "game")
                return;
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
                    //cubePainter.PaintAction(cube, action);
                    cubePainter.PaintFullImage(cube, action);
                    System.Threading.Thread.Sleep(800);
                    cube.Paint();
     
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
                if (acts[eventCount].Action.Equals("click"))
                {
                    if (cube.Equals(acts[eventCount].Cube))
                    {
                        correct.Play(1, 0);
                        Log.Debug("Correct!");
                        checkEventCount();
                    }
                    else
                    {
                        Log.Debug("Wrong cube!");
                        incorrect.Play(1, 0);
                        sm.QueueTransition("gameToTitle");
                        sm.Tick(1);
                    }
                }
                else
                {
                    Log.Debug("Wrong action!");
                    incorrect.Play(1, 0);
                    sm.QueueTransition("gameToTitle");
                    sm.Tick(1);
                }
            }
        }

        private void OnShakeStarted(Cube cube)
        { 
            Log.Debug("Shake start");
            Paint(cube, "shake");
            if (acts[eventCount].Action.Equals("shake"))
            {
                if (cube.Equals(acts[eventCount].Cube))
                {
                    correct.Play(1, 0);
                    Log.Debug("Correct!");
                    checkEventCount();
                }
                else
                {
                    Log.Debug("Wrong cube!");
                    incorrect.Play(1, 0);
                    sm.QueueTransition("gameToTitle");
                    sm.Tick(1);
                }
            }
            else
            {
                Log.Debug("Wrong action!");
                incorrect.Play(1, 0);
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
                Paint(cube, "flip");
                if (acts[eventCount].Action.Equals("flip"))
                {
                    if (cube.Equals(acts[eventCount].Cube))
                    {
                        correct.Play(1, 0);
                        Log.Debug("Correct!");
                        checkEventCount();
                    }
                    else
                    {
                        Log.Debug("Wrong cube!");
                        incorrect.Play(1, 0);
                        sm.QueueTransition("gameToTitle");
                        sm.Tick(1);
                    }
                }
                else
                {
                    Log.Debug("Wrong action!");
                    incorrect.Play(1, 0);
                    sm.QueueTransition("gameToTitle");
                    sm.Tick(1);
                }
            }
            else
            {
                Log.Debug("Flip face down");

            }
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

        private void checkEventCount()
        {
            eventCount++;
            if (eventCount == acts.Count)
            {
                Log.Debug("next round!");
                sm.QueueTransition("gameToPattern");
                sm.Tick(1);
            }
        }
	}
}

