using Assets.GamePlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Blocks
{
    class BlockSpawner : Block
    {
        public int x, y, z;
        public string clazz;

        private static GameObject midblock;

        public BlockSpawner(int x, int y, int z, string clazz) : base(createFullBlock(x, y, z, clazz), "BlockSpawner" + clazz) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.clazz = clazz;
        }

        private static List<GameObject> createFullBlock(int x, int y, int z, string clazz)
        {
            List<GameObject> cubes = new List<GameObject>();

            cubes.Add(createBlock(x, y, z, clazz, +0F, +0.25F, +0.25F, 0.05F, 0.05F, 0.5F));
            cubes.Add(createBlock(x, y, z, clazz, +0F, +0F, +0F, 0.05F, 0.5F, 0.05F));
            cubes.Add(createBlock(x, y, z, clazz, +0.25F, +0.25F, +0F, 0.5F, 0.05F, 0.05F));

            cubes.Add(createBlock(x, y, z, clazz, +0.5F, +0.25F, +0.25F, 0.05F, 0.05F, 0.5F));
            cubes.Add(createBlock(x, y, z, clazz, +0.5F, +0F, +0.5F, 0.05F, 0.5F, 0.05F));
            cubes.Add(createBlock(x, y, z, clazz, +0.25F, +0.25F, +0.5F, 0.5F, 0.05F, 0.05F));

            cubes.Add(createBlock(x, y, z, clazz, +0F, -0.25F, +0.25F, 0.05F, 0.05F, 0.5F));
            cubes.Add(createBlock(x, y, z, clazz, +0.5F, +0F, +0F, 0.05F, 0.5F, 0.05F));
            cubes.Add(createBlock(x, y, z, clazz, +0.25F, -0.25F, +0F, 0.5F, 0.05F, 0.05F));

            cubes.Add(createBlock(x, y, z, clazz, +0.5F, -0.25F, +0.25F, 0.05F, 0.05F, 0.5F));
            cubes.Add(createBlock(x, y, z, clazz, +0F, +0F, +0.5F, 0.05F, 0.5F, 0.05F));
            cubes.Add(createBlock(x, y, z, clazz, +0.25F, -0.25F, +0.5F, 0.5F, 0.05F, 0.05F));

            GameObject mid = createBlock(x, y, z, clazz, +0.25F, -0F, +0.25F, 0.25F, 0.25F, 0.25F);
            midblock = mid;

            cubes.Add(mid);

            return cubes;
        }

        private static GameObject createBlock(int x, int y, int z, string clazz, float addX, float addY, float addZ, float widthX, float widthY, float widthZ)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = "BlockSpawner" + clazz;
            cube.transform.position = new VectorHelper(new Vector3(x, y, z)).add(addX, addY, addZ);
            cube.transform.localScale = new Vector3(widthX, widthY, widthZ);
            if (clazz == "BLUE")
            {
                cube.renderer.material = Materials.blueMat;
            }
            else if (clazz == "RED")
            {
                cube.renderer.material = Materials.redMat;
            }
            return cube;
        }

        public override void update()
        {
            midblock.transform.Rotate(new Vector3(0, 1, 0));
            if (UnityEngine.Random.Range(0, 100) > 98)
            {
                if (clazz == "RED")
                {
                    EntityWarriorRED ew = new EntityWarriorRED(new VectorHelper(new Vector3(x, y, z)).add(UnityEngine.Random.Range(1, 5), 1, UnityEngine.Random.Range(1, 5)));
                    Main.warriors.Add(ew);
                    Main.redwarriors++;
                }
                else if (clazz == "BLUE")
                {
                    EntityWarriorBLUE ew = new EntityWarriorBLUE(new VectorHelper(new Vector3(x, y, z)).add(UnityEngine.Random.Range(1, 5), 1, UnityEngine.Random.Range(1, 5)));
                    Main.warriors.Add(ew);
                    Main.bluewarriors++;
                }
                
            }
        }
    }
}
