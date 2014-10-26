using Assets.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.GamePlay.Util
{
    class EntityUtil
    {
        public static void spawnEntityAtSpawn(int type, int id, float spawndx, float spawndz)
        {
            Type t = EntityType.entityType[type];
            Entity ew = (Entity)Activator.CreateInstance(t, new object[] { id, new VectorHelper(Main.getMain().spawnLocation).add(spawndx, 0, spawndz) });
            //EntityWarrior ew = new EntityWarrior(id, new VectorHelper(Main.getMain().spawnLocation).add(spawndx, 0, spawndz));
            Main.getMain().entities.Add(ew);
            if (id == 0)
            {
                Main.getMain().red++;
            }
            else if (id == 1)
            {
                Main.getMain().blue++;
            }
        }

        public static void spawnEntity(int type, int id, float x, float y, float z)
        {
            Type t = EntityType.entityType[type];
            Entity ew = (Entity)Activator.CreateInstance(t, new object[] { id, new VectorHelper(Main.getMain().spawnLocation).add(x, y, z) });
            //EntityWarrior ew = new EntityWarrior(id, new VectorHelper(Main.getMain().spawnLocation).add(x, y, z));
            Main.getMain().entities.Add(ew);
            if (id == 0)
            {
                Main.getMain().red++;
            }
            else if (id == 1)
            {
                Main.getMain().blue++;
            }
        }
    }
}
