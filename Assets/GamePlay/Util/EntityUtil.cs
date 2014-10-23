using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.GamePlay.Util
{
    class EntityUtil
    {
        public static void spawnEntity(int id, int spawndx, int spawndy)
        {
            EntityWarrior ew = new EntityWarrior(id, new VectorHelper(Main.getMain().spawnLocation).add(spawndx, 0, spawndy));
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
