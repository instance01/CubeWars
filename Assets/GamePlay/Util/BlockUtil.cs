using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.GamePlay.Util
{
    public static class BlockUtil
    {
        public static GameObject createBlock(float x, float y, float z, string name, string clazz, float addX, float addY, float addZ, float widthX, float widthY, float widthZ, Material m)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = name + clazz;
            cube.transform.position = new VectorHelper(new Vector3(x, y, z)).add(addX, addY, addZ);
            cube.transform.localScale = new Vector3(widthX, widthY, widthZ);
            cube.renderer.material = m;
            return cube;
        }

        public static GameObject createBlock(float x, float y, float z, string name, string clazz, float addX, float addY, float addZ, float widthX, float widthY, float widthZ)
        {
            if (clazz == "BLUE")
            {
                return createBlock(x, y, z, name, clazz, addX, addY, addZ, widthX, widthY, widthZ, Materials.blueMat);
            }
            else if (clazz == "RED")
            {
                return createBlock(x, y, z, name, clazz, addX, addY, addZ, widthX, widthY, widthZ, Materials.redMat);
            }

            return createBlock(x, y, z, name, clazz, addX, addY, addZ, widthX, widthY, widthZ, Materials.blueMat);
        }
    }
}
