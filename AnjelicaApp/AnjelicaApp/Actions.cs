using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sifteo;

namespace AnjelicaApp
{
    public class Actions
    {
        private string action;
        private Cube cube;

        public Actions(string action, Cube cube)
        {
            this.action = action;
            this.cube = cube;
        }

        public Cube Cube
        {
            get 
            {
                return this.cube;
            }
        }

        public string Action
        {
            get 
            {
                return this.action;
            }
        }
    }
}
