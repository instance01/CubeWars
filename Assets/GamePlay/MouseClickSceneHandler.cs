using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.GamePlay
{
    class MouseClickSceneHandler
    {
        Ray ray;
        RaycastHit hit;

        Entity selected;

        public MouseClickSceneHandler()
        {

        }

        public void update(List<EntityWarrior> warriors)
        {
            // check for mouse input
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name == "EntityWarriorRED")
                {
                    hit.collider.renderer.material = Materials.redMatoutline;
                    if (Input.GetMouseButtonDown(0))
                    {
                        EntityID e = (EntityID)hit.collider.GetComponent(typeof(EntityID));
                        int tempid = e.getID();

                        foreach (EntityWarrior ew in warriors)
                        {
                            if (ew.getEntityID().getID() == tempid)
                            {
                                // found
                                ew.jump();
                                selected = ew;
                            }
                        }
                    }
                }
                else if (hit.collider.name == "EntityWarriorBLUE")
                {
                    hit.collider.renderer.material = Materials.blueMatoutline;
                    if (Input.GetMouseButtonDown(0))
                    {
                        EntityID e = (EntityID)hit.collider.GetComponent(typeof(EntityID));
                        int tempid = e.getID();

                        foreach (EntityWarrior ew in warriors)
                        {
                            if (ew.getEntityID().getID() == tempid)
                            {
                                // found
                                ew.jump();
                                selected = ew;
                            }
                        }
                    }
                }
                else
                {
                    foreach (EntityWarrior ew in warriors)
                    {
                        ew.updateMaterial(false);
                    }

                    if (selected != null)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            EntityWarrior ew_ = (EntityWarrior)selected;
                            ew_.setMove(false, hit.point);
                            // ew_.moveTo(hit.point, 1.5F);
                            // spawn cool pointer
                            Main.getMain().cursorcone.renderer.enabled = true;
                            Main.getMain().cursorcone.transform.position = new VectorHelper(hit.point).add(0F, 1F, 0F);
                        }
                        else if (Input.GetMouseButton(1))
                        {
                            selected = null;
                        }

                    }
                }
                //print (hit.collider.name);
            }
        }
    }
}
