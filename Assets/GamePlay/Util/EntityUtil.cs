using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.GamePlay.Util
{
    class EntityUtil
    {
        public static void spawnEntityAtSpawn(int id, float spawndx, float spawndz)
        {
            EntityWarrior ew = new EntityWarrior(id, new VectorHelper(Main.getMain().spawnLocation).add(spawndx, 0, spawndz));
            Main.getMain().entities.Add(ew);
            if (id == 0)
            {
                Main.getMain().redwarriors++;
            }
            else if (id == 1)
            {
                Main.getMain().bluewarriors++;
            }
        }

        public static void spawnEntity(int id, float x, float y, float z)
        {
            EntityWarrior ew = new EntityWarrior(id, new VectorHelper(Main.getMain().spawnLocation).add(x, y, z));
            Main.getMain().entities.Add(ew);
            if (id == 0)
            {
                Main.getMain().redwarriors++;
            }
            else if (id == 1)
            {
                Main.getMain().bluewarriors++;
            }
        }
    }
}
