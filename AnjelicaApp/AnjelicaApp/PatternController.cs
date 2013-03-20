using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sifteo;
using Sifteo.Util;

namespace AnjelicaApp
{
    public class PatternController : IStateController
    {
        private CubeSet cubeSet;
		private CubePainter cubePainter;
		private StateMachine sm;
        private StateMachineLock smLock;
        private Cube actionCube;
        private Random random = new Random();
        private int actionIndex;
        private String[] actions = new String[] {"shake", "flip", "click"};
        private List<Actions> acts;

		public PatternController (CubeSet cubeSet, CubePainter cubePainter, StateMachine sm, List<Actions> acts)
		{
            this.cubeSet = cubeSet;
            this.cubePainter = cubePainter;
            this.sm = sm;
            smLock = new StateMachineLock(this.sm);
            this.acts = acts;
		}

        public void OnSetup(string transition)
        {
            Log.Debug("PatternController Setup");
            actionCube = cubeSet[random.Next(cubeSet.Count)];
            actionIndex = random.Next(3);
            
            if (transition.Equals("titleToPattern"))
            {
                // clear actions array
                acts.Clear();
            }
            acts.Add(new Actions(actions[actionIndex], actionCube));
            System.Threading.Thread.Sleep(1000);
            cubePainter.PaintAllImage(cubeSet, "watch");
            cubePainter.Commit(cubeSet);
            System.Threading.Thread.Sleep(1000);
            Paint();
        }

        public void OnTick(float n)
        {
        }

        public void OnPaint(bool dirtyCanvas)
        {
            if (dirtyCanvas)
            {
                Paint();
            }
        }
        public void OnDispose()
        {
            cubeSet.ClearEvents();
        }

        public void Paint()
        {
            cubePainter.ClearScreen(cubeSet);
            foreach (Actions act in acts)
            {
                foreach (Cube cube in cubeSet)
                {
                    if (!cube.Equals(act.Cube))
                    {
                        cubePainter.ClearScreen(cube, new Color(0, 0, 0));
                    }
                    else
                    {
                        Log.Debug("{0} me!", act.Action);
                        cubePainter.PaintAction(cube, act.Action);
                    }
                }
                cubePainter.Commit(cubeSet);
                /* paint each action for 1 second */
                System.Threading.Thread.Sleep(1000);
            }


            sm.QueueTransition("patternToGame");
            sm.Tick(1);
        }
    }
}
