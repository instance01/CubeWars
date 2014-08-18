using UnityEngine;
using System.Collections;
using Assets.GamePlay;

public class Entity {
    public GameObject health_green;
    public GameObject health_red;
    public int health = 3;
    public int maxhealth = 3;

    public GameObject cube;

    public bool dead = false;
    public bool move_ = true;

    public Vector3 moveto;

	public Entity(){

	}

    public void createHealthbarGraphics()
    {
        // health bar
        health_green = new GameObject();
        health_red = new GameObject();
        health_green = GameObject.CreatePrimitive(PrimitiveType.Plane);
        health_red = GameObject.CreatePrimitive(PrimitiveType.Plane);
        health_green.transform.renderer.material = Materials.greenMat;
        health_red.transform.renderer.material = Materials.redMat;
        health_green.transform.localScale = new Vector3(health * 0.05F, 0.005F, 0.005F);
        health_red.transform.localScale = new Vector3(health * 0.05F, 0.005F, 0.005F);
        health_green.transform.Rotate(new Vector3(-90F, 0F, 0F));
        health_red.transform.Rotate(new Vector3(-90F, 0F, 0F));
        health_green.transform.Rotate(67.5F, 0F, 0F);
        health_red.transform.Rotate(67.5F, 0F, 0F);
    }


    public void updateHealthbar()
    {
        health_green.transform.localPosition = cube.transform.localPosition;
        health_red.transform.localPosition = cube.transform.localPosition;
        health_red.transform.Translate(0F, 0.5F, 1F);
        health_green.transform.Translate(0F - (maxhealth - health) * 0.25F, 0.525F, 1F);
        health_green.transform.localScale = new Vector3(health * 0.05F, 0.005F, 0.01F);
    }


    public void moveTo(Vector3 v, float speed_)
    {
        if (cube != null)
        {
            cube.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(cube.transform.position, v - cube.transform.position, Time.deltaTime * speed_, 0.0F));
            cube.transform.position = Vector3.MoveTowards(cube.transform.position, v, Time.deltaTime * speed_);
            if (Mathf.Abs(cube.transform.position.x - v.x) < 0.1 && Mathf.Abs(cube.transform.position.z - v.z) < 0.1)
            {
                move_ = true;
                Main.cursorcone.renderer.enabled = false;
            }
        }
    }

    public GameObject getCube()
    {
        return cube;
    }

    public void setMove(bool m, Vector3 v)
    {
        this.move_ = m;
        moveto = v;
    }

    public void setMove(bool m)
    {
        this.move_ = m;
    }
}
