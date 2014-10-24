using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.GamePlay.Util
{
    public static class BlockUtil
    {
        public static GameObject createBlock(float x, float y, float z, string name, int id, float addX, float addY, float addZ, float widthX, float widthY, float widthZ, Material m)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = name + id;
            cube.transform.position = new VectorHelper(new Vector3(x, y, z)).add(addX, addY, addZ);
            cube.transform.localScale = new Vector3(widthX, widthY, widthZ);
            cube.renderer.material = m;
            return cube;
        }

        public static GameObject createBlock(float x, float y, float z, string name, int id, float addX, float addY, float addZ, float widthX, float widthY, float widthZ)
        {
            if (id == 1)
            {
                return createBlock(x, y, z, name, id, addX, addY, addZ, widthX, widthY, widthZ, Materials.blueMat);
            }
            else if (id == 0)
            {
                return createBlock(x, y, z, name, id, addX, addY, addZ, widthX, widthY, widthZ, Materials.redMat);
            }

            return createBlock(x, y, z, name, id, addX, addY, addZ, widthX, widthY, widthZ, Materials.blueMat);
        }
    }
}
