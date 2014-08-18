using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Blocks
{
    class Block
    {
        public bool breakable = true;
        public String name = "Block";

        public GameObject cube;

        public Block(GameObject cube)
        {
            this.cube = cube;
        }
    }
}
