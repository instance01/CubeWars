using Assets.GamePlay;
using Assets.GamePlay.Util;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Blocks
{
    class BlockSpawner : Block
    {
        public float x, y, z;
        public int id;

        private GameObject midblock;

        public BlockSpawner(float x, float y, float z, int id)
            : base(createFullBlock(x, y, z, id), "BlockSpawner" + id)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.id = id;
            this.midblock = this.rawcubes[this.rawcubes.Count - 1];
        }

        private static List<GameObject> createFullBlock(float x, float y, float z, int id)
        {
            y += 0.5F;
            List<GameObject> cubes = new List<GameObject>();

            cubes.Add(createBlock(x, y, z, id, +0F, +0.25F, +0.25F, 0.05F, 0.05F, 0.5F, Materials.blackMat));
            cubes.Add(createBlock(x, y, z, id, +0F, +0F, +0F, 0.05F, 0.5F, 0.05F, Materials.blackMat));
            cubes.Add(createBlock(x, y, z, id, +0.25F, +0.25F, +0F, 0.5F, 0.05F, 0.05F, Materials.blackMat));

            cubes.Add(createBlock(x, y, z, id, +0.5F, +0.25F, +0.25F, 0.05F, 0.05F, 0.5F, Materials.blackMat));
            cubes.Add(createBlock(x, y, z, id, +0.5F, +0F, +0.5F, 0.05F, 0.5F, 0.05F, Materials.blackMat));
            cubes.Add(createBlock(x, y, z, id, +0.25F, +0.25F, +0.5F, 0.5F, 0.05F, 0.05F, Materials.blackMat));

            cubes.Add(createBlock(x, y, z, id, +0F, -0.25F, +0.25F, 0.05F, 0.05F, 0.5F, Materials.blackMat));
            cubes.Add(createBlock(x, y, z, id, +0.5F, +0F, +0F, 0.05F, 0.5F, 0.05F, Materials.blackMat));
            cubes.Add(createBlock(x, y, z, id, +0.25F, -0.25F, +0F, 0.5F, 0.05F, 0.05F, Materials.blackMat));

            cubes.Add(createBlock(x, y, z, id, +0.5F, -0.25F, +0.25F, 0.05F, 0.05F, 0.5F, Materials.blackMat));
            cubes.Add(createBlock(x, y, z, id, +0F, +0F, +0.5F, 0.05F, 0.5F, 0.05F, Materials.blackMat));
            cubes.Add(createBlock(x, y, z, id, +0.25F, -0.25F, +0.5F, 0.5F, 0.05F, 0.05F, Materials.blackMat));

            GameObject mid = createBlock(x, y, z, id, +0.25F, -0F, +0.25F, 0.25F, 0.25F, 0.25F);
            //midblock = mid;

            cubes.Add(mid);

            return cubes;
        }

        private static GameObject createBlock(float x, float y, float z, int id, float addX, float addY, float addZ, float widthX, float widthY, float widthZ, Material m)
        {
            return BlockUtil.createBlock(x, y, z, "BlockSpawner", id, addX, addY, addZ, widthX, widthY, widthZ, m);
        }

        private static GameObject createBlock(float x, float y, float z, int id, float addX, float addY, float addZ, float widthX, float widthY, float widthZ)
        {
            if (id == 1)
            {
                return createBlock(x, y, z, id, addX, addY, addZ, widthX, widthY, widthZ, Materials.blueMat);
            }
            else if (id == 0)
            {
                return createBlock(x, y, z, id, addX, addY, addZ, widthX, widthY, widthZ, Materials.redMat);
            }

            return createBlock(x, y, z, id, addX, addY, addZ, widthX, widthY, widthZ, Materials.blueMat);
        }

        public override void update()
        {
            midblock.transform.Rotate(0F, 1F, 0F);
            if (Random.Range(0, 100) > 98)
            {
                if (id == 0)
                {
                    EntityUtil.spawnEntity(0, x + Random.Range(1, 5), y + 0.5F, z + Random.Range(1, 5));
                }
                else if (id == 1)
                {
                    EntityUtil.spawnEntity(1, x + Random.Range(1, 5), y + 0.5F, z + Random.Range(1, 5));
                }
            }
        }
    }
}
