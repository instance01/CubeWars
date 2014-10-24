using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.GamePlay;

// A warrior moves around very aggressively and fast and is a short ranged entity

public class EntityWarrior : Entity
{
    BoxCollider cubeCollider;
    string enemyClassifier = "";

    bool shootcooldown = false;
    int shootcooldown_c = 0;

    public EntityWarrior(int c, Vector3 spawn) : base(c, 0.9F)
    {
        createGraphics(spawn);
        if (getClassifierID() == 0)
        {
            enemyClassifier = "BLUE";
        }
        else if (getClassifierID() == 1)
        {
            enemyClassifier = "RED";
        }
    }

    public void createGraphics(Vector3 spawn)
    {
        // creates the cube
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = "EntityWarrior" + getClassifierName();
        cube.tag = "EntityWarrior" + getClassifierName();
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

        base.createHealthbarGraphics();

    }

    public void findTarget()
    {
        var currentPos = getCube().transform.position;

        GameObject[] gos = GameObject.FindGameObjectsWithTag("EntityWarrior" + enemyClassifier);
        if (gos.Length > 0)
        {
            GameObject closestGameObject = gos
                .Select(go => new { go = go, position = go.transform.position })
                    .Aggregate((current, next) =>
                               (current.position - currentPos).sqrMagnitude <
                               (next.position - currentPos).sqrMagnitude
                               ? current : next).go;

            if (Vector3.Distance(getCube().transform.position, closestGameObject.transform.position) < 10)
            {
                EntityID e = (EntityID)closestGameObject.GetComponent(typeof(EntityID));
                int tempid = e.getID();

                foreach (EntityWarrior ew in Main.getMain().entities)
                {
                    if (ew.getEntityID().getID() == tempid)
                    {
                        target = ew;
                    }
                }
            }
        }
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
            findTarget();
        }
        else
        {
            if (shootcooldown)
            {
                shootcooldown_c++;
                if (shootcooldown_c > 100)
                {
                    shootcooldown = false;
                    shootcooldown_c = 0;
                }
                return;
            }
            if (target is EntityWarrior)
            {
                shootcooldown = true;
                attack((EntityWarrior)target);
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

    public void updateMaterial(bool jump)
    {
        if (getClassifierID() == 0)
        {
            if (!jump)
            {
                getCube().transform.renderer.material = Materials.redMat;
            }
            else
            {
                getCube().transform.renderer.material = Materials.redMatoutline;
            }
        }
        else if (getClassifierID() == 1)
        {
            if (!jump)
            {
                getCube().transform.renderer.material = Materials.blueMat;
            }
            else
            {
                getCube().transform.renderer.material = Materials.blueMatoutline;
            }
        }
    }

}
