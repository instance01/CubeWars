using Assets.GamePlay;
using Assets.GamePlay.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Blocks
{
    class BlockSpawner : Block
    {
        public float x, y, z;
        public string clazz;

        private GameObject midblock;

        public BlockSpawner(float x, float y, float z, string clazz) : base(createFullBlock(x, y, z, clazz), "BlockSpawner" + clazz)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.clazz = clazz;
            this.midblock = this.rawcubes[this.rawcubes.Count - 1];
        }

        private static List<GameObject> createFullBlock(float x, float y, float z, string clazz)
        {
            y += 0.5F;
            List<GameObject> cubes = new List<GameObject>();

            cubes.Add(createBlock(x, y, z, clazz, +0F, +0.25F, +0.25F, 0.05F, 0.05F, 0.5F, Materials.blackMat));
            cubes.Add(createBlock(x, y, z, clazz, +0F, +0F, +0F, 0.05F, 0.5F, 0.05F, Materials.blackMat));
            cubes.Add(createBlock(x, y, z, clazz, +0.25F, +0.25F, +0F, 0.5F, 0.05F, 0.05F, Materials.blackMat));

            cubes.Add(createBlock(x, y, z, clazz, +0.5F, +0.25F, +0.25F, 0.05F, 0.05F, 0.5F, Materials.blackMat));
            cubes.Add(createBlock(x, y, z, clazz, +0.5F, +0F, +0.5F, 0.05F, 0.5F, 0.05F, Materials.blackMat));
            cubes.Add(createBlock(x, y, z, clazz, +0.25F, +0.25F, +0.5F, 0.5F, 0.05F, 0.05F, Materials.blackMat));

            cubes.Add(createBlock(x, y, z, clazz, +0F, -0.25F, +0.25F, 0.05F, 0.05F, 0.5F, Materials.blackMat));
            cubes.Add(createBlock(x, y, z, clazz, +0.5F, +0F, +0F, 0.05F, 0.5F, 0.05F, Materials.blackMat));
            cubes.Add(createBlock(x, y, z, clazz, +0.25F, -0.25F, +0F, 0.5F, 0.05F, 0.05F, Materials.blackMat));

            cubes.Add(createBlock(x, y, z, clazz, +0.5F, -0.25F, +0.25F, 0.05F, 0.05F, 0.5F, Materials.blackMat));
            cubes.Add(createBlock(x, y, z, clazz, +0F, +0F, +0.5F, 0.05F, 0.5F, 0.05F, Materials.blackMat));
            cubes.Add(createBlock(x, y, z, clazz, +0.25F, -0.25F, +0.5F, 0.5F, 0.05F, 0.05F, Materials.blackMat));

            GameObject mid = createBlock(x, y, z, clazz, +0.25F, -0F, +0.25F, 0.25F, 0.25F, 0.25F);
            //midblock = mid;

            cubes.Add(mid);

            return cubes;
        }

        private static GameObject createBlock(float x, float y, float z, string clazz, float addX, float addY, float addZ, float widthX, float widthY, float widthZ, Material m)
        {
            return BlockUtil.createBlock(x, y, z, "BlockSpawner", clazz, addX, addY, addZ, widthX, widthY, widthZ, m);
        }

        private static GameObject createBlock(float x, float y, float z, string clazz, float addX, float addY, float addZ, float widthX, float widthY, float widthZ)
        {
            if (clazz == "BLUE")
            {
                return createBlock(x, y, z, clazz, addX, addY, addZ, widthX, widthY, widthZ, Materials.blueMat);
            }
            else if (clazz == "RED")
            {
                return createBlock(x, y, z, clazz, addX, addY, addZ, widthX, widthY, widthZ, Materials.redMat);
            }

            return createBlock(x, y, z, clazz, addX, addY, addZ, widthX, widthY, widthZ, Materials.blueMat);
        }

        public override void update()
        {
            midblock.transform.Rotate(new Vector3(0, 1, 0));
            if (UnityEngine.Random.Range(0, 100) > 98)
            {
                if (clazz == "RED")
                {
                    EntityWarrior ew = new EntityWarrior(0, new VectorHelper(new Vector3(x, y, z)).add(UnityEngine.Random.Range(1, 5), 1, UnityEngine.Random.Range(1, 5)));
                    Main.getMain().entities.Add(ew);
                    Main.getMain().addWarriorCount(true, false);
                }
                else if (clazz == "BLUE")
                {
                    EntityWarrior ew = new EntityWarrior(1, new VectorHelper(new Vector3(x, y, z)).add(UnityEngine.Random.Range(1, 5), 1, UnityEngine.Random.Range(1, 5)));
                    Main.getMain().entities.Add(ew);
                    Main.getMain().addWarriorCount(false, true);
                }
                
            }
        }
    }
}
