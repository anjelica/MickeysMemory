/* StateMachineLock.cs()
 * Written by Sean Voisen and used for Mickey's Memory
 */

using System;
using Sifteo;
using Sifteo.Util;

namespace  AnjelicaApp
{
    public class StateMachineLock
    {
        private int tickCount;
        private int unlockAt;
        private StateMachine sm;
        private bool locked;
        private IDisposable smLock;

        public StateMachineLock(StateMachine stateMachine)
        {
            this.sm = stateMachine;
            locked = false;
            unlockAt = -1;
        }

        public StateMachine Sm
        {
            get
            {
                return this.sm;
            }
            set
            {
                sm = value;
            }
        }

        public bool Locked
        {
            get
            {
                return this.locked;
            }
        }

        public void LockForTickCount(int numTicks)
        {
            locked = true;
            smLock = sm.AquireLock();
            unlockAt = numTicks;
            tickCount = 0;
        }

        public void Tick()
        {
            if (!locked)
                return;

            tickCount++;
            if (tickCount >= unlockAt)
                Unlock();
        }

        public void Unlock()
        {
            locked = false;
            unlockAt = -1;
            smLock.Dispose();
        }
    }
}