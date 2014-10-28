using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.GamePlay
{
    public class MouseClickSceneHandler
    {
        Ray ray;
        RaycastHit hit;
        RaycastHit lastrayhit;

        public List<Entity> selected = new List<Entity>();

        float clickTime = 0F;

        GameObject maincam;
        GameObject origcam;
        Camera cam;

        public MouseClickSceneHandler()
        {
            maincam = GameObject.FindGameObjectWithTag("MainCamera");
            origcam = GameObject.Find("Camera");
            origcam.camera.enabled = false;
        }

        public void update()
        {
            if (Main.getMain().heroMode && Main.currentHero != null && cam != null)
            {
                if (Main.currentHero.getCube() == null)
                {
                    // entity is dead, go back to normal camera view
                    maincam.camera.enabled = true;
                    cam.enabled = false;
                    Main.getMain().heroMode = false;
                    Main.currentHero = null;
                    return;
                }
                Vector3 v = new VectorHelper(Main.currentHero.getCube().transform.position).add(0, 1F, 0);
                Vector3 v_ = new VectorHelper(Main.currentHero.getCube().transform.position).add(0, 0.85F, 0);
                if (Vector3.Distance(v, cam.transform.position) > 0.1F)
                {
                    cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, Quaternion.LookRotation(v_ - cam.transform.position, Vector3.up), Time.deltaTime * 1.1F * 4F);
                }
                cam.transform.position = Vector3.MoveTowards(cam.transform.position, v, Time.deltaTime * 1.1F);
            }

            List<Entity> entities = new List<Entity>(Main.getMain().entities);
            // check for mouse input
            if (Camera.main != null)
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            }
            else if (cam != null)
            {
                ray = cam.ScreenPointToRay(Input.mousePosition);
            }
            else
            {
                return;
            }

            if (Physics.Raycast(ray, out hit))
            {
                if (Input.GetMouseButton(0))
                {
                    if (!isHandled)
                    {
                        lastrayhit = hit;
                        isHandled = true;
                    }
                }
                else
                {
                    if (isHandled)
                    {

                        Vector3 point1 = lastrayhit.point;
                        Vector3 point2 = hit.point;
                        //Debug.Log(point1.ToString() + " " + point2.ToString());
                        foreach (Entity e in Main.getMain().entities)
                        {
                            Vector3 pos = e.cube.transform.localPosition;
                            //Debug.Log("## " + pos.ToString() + " || " + point1.ToString() + " || " + point2.ToString());
                            //Debug.Log("##### " + (pos.x > Math.Min(point1.x, point2.x)).ToString() + (pos.x < Math.Max(point1.x, point2.x)).ToString() + (pos.z > Math.Min(point1.z, point2.z)).ToString() + (pos.z < Math.Max(point1.z, point2.z)).ToString());
                            if (pos.x > Math.Min(point1.x, point2.x) && pos.x < Math.Max(point1.x, point2.x) && pos.z > Math.Min(point1.z, point2.z) && pos.z < Math.Max(point1.z, point2.z))
                            {
                                selected.Add(e);
                                e.updateMaterial(true);
                            }
                        }
                        isHandled = false;
                    }
                    return;
                }

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

                    if (selected.Count > 0)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            foreach (Entity e_ in selected)
                            {
                                e_.setMove(false, hit.point);
                                // spawn cool pointer
                                Main.getMain().cursorcone.renderer.enabled = true;
                                Main.getMain().cursorcone.transform.position = new VectorHelper(hit.point).add(0F, 1F, 0F);
                            }
                        }
                        else if (Input.GetMouseButton(1))
                        {
                            selected = new List<Entity>();
                        }
                    }
                    return;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    Entity e = null;
                    EntityID eid = (EntityID)hit.collider.GetComponent(typeof(EntityID));
                    int tempid = eid.getID();

                    foreach (Entity e_ in entities)
                    {
                        if (e_.getEntityID().getID() == tempid)
                        {
                            // found
                            e = e_;
                        }
                    }
                    if (Time.time - clickTime < 0.25F)
                    {
                        Main.currentHero = e;
                        if (cam != null)
                        {
                            GameObject.Destroy(cam.gameObject);
                        }
                        cam = (Camera)Camera.Instantiate(origcam.camera, new VectorHelper(e.getCube().transform.position).add(0, 0.5F, 0), e.getCube().transform.rotation);
                        //cam.transform.parent = e.getCube().transform;
                        cam.transform.Translate(Vector3.back * 1.5F);
                        if (Camera.main != null)
                        {
                            Camera.main.enabled = false;
                        }
                        cam.enabled = true;
                        Main.getMain().heroMode = true;
                    }
                    else
                    {
                        if (e != null)
                        {
                            e.jump();
                            selected.Add(e);
                        }
                    }
                    clickTime = Time.time;
                }
            }
        }

        bool isHandled = false;
        public void updateMouseDrag()
        {

        }

    }
}
