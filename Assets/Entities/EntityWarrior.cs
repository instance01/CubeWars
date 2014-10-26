using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.GamePlay;
using Assets.GamePlay.Util;
using System.Collections.Generic;
using Assets.Entities;

// A warrior moves around very aggressively and fast and is a short ranged entity

public class EntityWarrior : Entity
{
    BoxCollider cubeCollider;
    int enemyClassifier = 0;

    bool shootcooldown = false;
    int shootcooldown_c = 0;

    public EntityWarrior(int c, Vector3 spawn)
        : base(0, c, 1.1F)
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
    }

    public void createGraphics(Vector3 spawn)
    {
        // creates the cube
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = "EntityWarrior" + getClassifierName();
        //cube.tag = "EntityWarrior" + getClassifierName();
        cube.transform.position = spawn;
        cube.transform.localScale = new Vector3(0.5F, 0.5F, 0.5F);
        if (getClassifierID() == 0)
        {
            cube.renderer.material = Materials.redMat;
        }
        else if (getClassifierID() == 1)
        {
            cube.renderer.material = Materials.blueMat;
        }

        // adds components to cube
        cube.AddComponent<Rigidbody>();
        cubeCollider = cube.AddComponent<BoxCollider>();
        cubeCollider.size = new Vector3(1.25F, 1.25F, 1.25F);
        base.setEntityID(cube.AddComponent<EntityID>());
        cube.AddComponent<EntityCollider>().init(this);

        cube.rigidbody.mass = 1.5F;

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
                cube.transform.Rotate(0, Random.Range(-5, 5), 0); // they move to the left more often because we never reach 5 here (just -5 to 4).
                cube.transform.Translate(Vector3.forward * Time.deltaTime * speed);

                if (Random.Range(0, 1000) < 1)
                {
                    // will jump randomly
                    // TODO jump when obstacle in front of entity
                    jump(150);
                }
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
                attack(target);
            }
        }
    }

    public Entity getTarget()
    {
        return target;
    }

}
