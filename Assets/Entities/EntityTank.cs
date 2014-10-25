using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.GamePlay;
using Assets.GamePlay.Util;
using System.Collections.Generic;
using Assets.Entities;

// A warrior moves around very aggressively and fast and is a short ranged entity

public class EntityTank : Entity
{
    BoxCollider cubeCollider;
    int enemyClassifier = 0;

    bool shootcooldown = false;
    int shootcooldown_c = 0;

    public EntityTank(int c, Vector3 spawn)
        : base(c, 0.6F, 40, 80)
    {
        createGraphics(spawn);
        if (getClassifierID() == 0)
        {
            enemyClassifier = 1;
        }
        else if (getClassifierID() == 1)
        {
            enemyClassifier = 0;
        }

        base.bullet_fly_power = 1.5F;
    }

    public void createGraphics(Vector3 spawn)
    {
        Material m = Materials.redMat;
        if (getClassifierID() == 0)
        {
            m = Materials.redMat;
        }
        else if (getClassifierID() == 1)
        {
            m = Materials.blueMat;
        }
        // creates the cube
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = "EntityTank" + getClassifierName();
        //cube.tag = "EntityTank" + getClassifierName();
        cube.transform.position = spawn;
        cube.transform.localScale = new Vector3(1F, 0.4F, 2F);
        if (getClassifierID() == 0)
        {
            cube.renderer.material = m;
        }
        else if (getClassifierID() == 1)
        {
            cube.renderer.material = m;
        }

        // adds components to cube
        cube.AddComponent<Rigidbody>();
        cubeCollider = cube.AddComponent<BoxCollider>();
        cubeCollider.size = new Vector3(1.5F, 1.25F, 1.25F);
        base.setEntityID(cube.AddComponent<EntityID>());
        cube.AddComponent<EntityCollider>().init(this);

        GameObject cube_ = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube_.transform.position = new VectorHelper(spawn).add(0F, 0.25F, 0F);
        cube_.transform.localScale = new Vector3(0.6F, 0.2F, 0.8F);
        cube_.transform.parent = cube.transform;
        cube_.renderer.material = m;

        GameObject cube__ = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube__.transform.position = new VectorHelper(spawn).add(0F, 0.45F, 0.5F);
        cube__.transform.localScale = new Vector3(0.2F, 0.2F, 1.5F);
        cube__.transform.parent = cube.transform;
        cube__.renderer.material = m;

        cube.rigidbody.mass = 10F;

        base.createHealthbarGraphics();

    }

    public override void move()
    {
        base.move();

        if (!move_)
        {
            moveTo(moveto, 1.5F);
        }

        // will just randomly move around if no target found
        if (getTarget() == null)
        {
            if (move_)
            {
                cube.transform.Rotate(0, Random.Range(-1, 2), 0);
                cube.transform.Translate(Vector3.forward * Time.deltaTime * speed);
            }

            // try to find a new target
            findTarget(enemyClassifier);
        }
        else
        {
            if (shootcooldown)
            {
                shootcooldown_c++;
                if (shootcooldown_c > getMaxShootCooldown())
                {
                    shootcooldown = false;
                    shootcooldown_c = 0;
                }
                return;
            }
            if (target is Entity)
            {
                shootcooldown = true;
                attack((Entity)target);
            }
        }
    }

    public Entity getTarget()
    {
        return target;
    }

    public override void update()
    {
        base.update();
        move();
    }

}
