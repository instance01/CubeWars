using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Blocks
{
    public class Block
    {
        public bool breakable = true;
        public String name = "Block";

        public List<GameObject> rawcubes;
        public GameObject cube;

        public Block(List<GameObject> cubes, string name)
        {
            this.rawcubes = cubes;
            cube = new GameObject();
            this.name = name;
            cube.name = name;
            foreach(GameObject go in cubes){
                go.transform.parent = cube.transform;
            }
        }

        /*public Block(List<GameObject> cubes, GameObject parent)
        {
            foreach(GameObject go in cubes){
                go.transform.parent = parent.transform;
            }
            this.cube = parent;
        }*/

        public virtual void update()
        {

        }
    }
}
