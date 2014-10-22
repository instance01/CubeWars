using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.GamePlay
{
    class Materials
    {
        public static Material redMatoutline;
        public static Material redMat;
        public static Material blueMatoutline;
        public static Material blueMat;
        public static Material greenMat;
        public static Material blackMat;

        public static void init()
        {
            redMatoutline = Resources.Load("redMaterialoutline", typeof(Material)) as Material;
            redMat = Resources.Load("redMaterial", typeof(Material)) as Material;
            blueMatoutline = Resources.Load("blueMaterialoutline", typeof(Material)) as Material;
            blueMat = Resources.Load("blueMaterial", typeof(Material)) as Material;
            greenMat = Resources.Load("greenMaterial", typeof(Material)) as Material;
            blackMat = Resources.Load("blackMaterial", typeof(Material)) as Material;
        }

    }
}
