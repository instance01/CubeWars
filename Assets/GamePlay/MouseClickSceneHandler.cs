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

        public void update()
        {
            List<Entity> entities = new List<Entity>(Main.getMain().entities);
            // check for mouse input
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name.EndsWith("RED"))
                {
                    hit.collider.renderer.material = Materials.redMatoutline;
                }
                else if (hit.collider.name.EndsWith("BLUE"))
                {
                    hit.collider.renderer.material = Materials.blueMatoutline;
                }
                else
                {
                    foreach (Entity e in entities)
                    {
                        e.updateMaterial(false);
                    }

                    if (selected != null)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            Entity e_ = (Entity)selected;
                            e_.setMove(false, hit.point);
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
                    return;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    EntityID e = (EntityID)hit.collider.GetComponent(typeof(EntityID));
                    int tempid = e.getID();

                    foreach (Entity e_ in entities)
                    {
                        if (e_.getEntityID().getID() == tempid)
                        {
                            // found
                            e_.jump();
                            selected = e_;
                        }
                    }
                }
                //print (hit.collider.name);
            }
        }
    }
}
