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

		public GameController (CubeSet cubeSet, CubePainter cubePainter, StateMachine sm)
		{
			
		}

		public void OnSetup(string transition)
		{

		}

		public void OnTick(float n){}
		public void OnPaint(bool dirtyCanvas){}
		public void OnDispose(){}
	}
}

