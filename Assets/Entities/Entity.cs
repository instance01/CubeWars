using UnityEngine;
using System.Collections;
using Assets.GamePlay;
using Assets.Entities;
using System.Collections.Generic;

public class Entity
{
    EntityID entityid;

    public float speed = 1F;
    private GameObject health_green;
    private GameObject health_red;
    private int health = 3;
    private int maxhealth = 3;
    private bool canspeak = true;
    private int shootcooldown_max = 100;
    public float bullet_fly_power = 1F;

    public GameObject cube;

    public bool dead = false;
    public bool move_ = true;

    public Vector3 moveto;

    bool said = false;
    int said_c = 0;
    private GameObject txt;
    private TextMesh txtMesh;

    int classifierID = 0;

    GameObject smoke;

    public Entity target;

    /// <summary>
    /// Initializes a new entity
    /// </summary>
    /// <param name="classifierID">The classifier id (e.g. RED=0, BLUE=1)</param>
    public Entity(int classifierID, float speed = 1F, int maxhealth = 3, int shootcooldown_max = 100, bool canspeak = true)
    {
        this.classifierID = classifierID;
        this.speed = speed;
        this.maxhealth = maxhealth;
        this.health = maxhealth;
        this.shootcooldown_max = shootcooldown_max;
        if (canspeak)
        {
            createTextGraphics();
        }
        smoke = GameObject.Find("Smoke");
    }

    Vector3 txt_default_size = new Vector3(0.2F, 0.2F, 0.2F);
    public void createTextGraphics()
    {
        txt = new GameObject();
        txt.name = "EntityThought";
        txtMesh = (TextMesh)txt.AddComponent(typeof(TextMesh));
        txtMesh.font = (Font)Resources.Load("pf_arma_five", typeof(Font));
        txt.transform.renderer.material = (Material)Resources.Load("pf_arma_five", typeof(Material));
        txt.transform.Rotate(67.5F, 0F, 0F);
        txtMesh.text = ".";
        txtMesh.color = Color.black;
        txtMesh.richText = true;
        txtMesh.fontSize = 45;
        txt.transform.localScale = txt_default_size;
        MeshRenderer meshrenderer = (MeshRenderer)txt.GetComponent(typeof(MeshRenderer));
        meshrenderer.enabled = false;
    }

    public void saySomething(string msg)
    {
        if (canspeak)
        {
            txt.transform.localPosition = cube.transform.localPosition;
            txt.transform.Translate(0, 1, 0);
            txtMesh.text = msg;
            MeshRenderer meshrenderer = (MeshRenderer)txt.GetComponent(typeof(MeshRenderer));
            meshrenderer.enabled = true;
        }
    }

    public void createHealthbarGraphics()
    {
        // health bar
        health_green = GameObject.CreatePrimitive(PrimitiveType.Plane);
        health_red = GameObject.CreatePrimitive(PrimitiveType.Plane);
        health_green.transform.renderer.material = Materials.greenMat;
        health_red.transform.renderer.material = Materials.redMat;
        health_green.transform.localScale = new Vector3(health * 0.05F, 0.005F, 0.0075F);
        health_red.transform.localScale = new Vector3(health * 0.05F, 0.005F, 0.005F);
        health_green.transform.Rotate(new Vector3(-90F, 0F, 0F));
        health_red.transform.Rotate(new Vector3(-90F, 0F, 0F));
        health_green.transform.Rotate(67.5F, 0F, 0F);
        health_red.transform.Rotate(67.5F, 0F, 0F);
        MeshCollider m = (MeshCollider)health_green.GetComponent(typeof(MeshCollider));
        Component.Destroy(m);
        MeshCollider m_ = (MeshCollider)health_red.GetComponent(typeof(MeshCollider));
        Component.Destroy(m_);
    }


    public void updateHealthbar(bool updateHealth)
    {
        health_green.transform.localPosition = cube.transform.localPosition;
        health_red.transform.localPosition = cube.transform.localPosition;
        health_red.transform.Translate(0F, 0.5F, 1F);
        health_green.transform.Translate(0F - (maxhealth - health) * 0.25F, 0.525F, 1F);
        if (updateHealth)
        {
            //health_green.transform.localScale.Set(health * 0.05F, 0.005F, 0.0075F);
            Vector3 t = health_green.transform.localScale;
            t.x = health * 0.05F;
            health_green.transform.localScale = t;
        }
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
                Main.getMain().cursorcone.renderer.enabled = false;
            }
        }
    }

    public virtual void move()
    {
        // update text coordinates
        if (said && canspeak)
        {
            txt.transform.localPosition = cube.transform.localPosition;
            txt.transform.Translate(0, 1, 0);
        }
    }

    public virtual void die(bool explosion)
    {
        dead = true;
        Main.getMain().entities.Remove(this);
        said = false;
        if (canspeak)
        {
            txt.renderer.enabled = false;
            GameObject.Destroy(txt);
        }
        GameObject.Destroy(health_green);
        GameObject.Destroy(health_red);
        Color cubecolor = cube.renderer.material.color;
        Vector3 cubepos = cube.transform.localPosition;
        GameObject.Destroy(cube, 0);

        if (explosion)
        {
            int[,] pos = new int[16, 2] { { 0, 0 }, { 0, 1 }, { 0, 2 }, { 0, 3 }, { 1, 0 }, { 1, 1 }, { 1, 2 }, { 1, 3 }, { 2, 0 }, { 2, 1 }, { 2, 2 }, { 2, 3 }, { 3, 0 }, { 3, 1 }, { 3, 2 }, { 3, 3 } };

            Vector3 cubesize = new Vector3(0.0625F * 2, 0.0625F * 2, 0.0625F * 2);
            Vector3 explosionForcePos = new Vector3(cubepos.x * 1.01F, cubepos.y * 0.9F, cubepos.z * 1.01F);

            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    GameObject cube_ = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube_.renderer.material.color = cubecolor;
                    cube_.transform.localScale = cubesize;
                    cube_.transform.localPosition = (new VectorHelper(cubepos).add(pos[i, 0] * 0.0625F * 2, j * 0.0625F * 2, pos[i, 1] * 0.0625F * 2));

                    cube_.AddComponent<Rigidbody>();
                    cube_.rigidbody.AddExplosionForce((Random.Range(100.0f, 250.0f)), explosionForcePos, (Random.Range(50.0f, 75.0f)));
                    cube_.rigidbody.SetDensity(cube_.transform.localPosition.x);

                    GameObject.Destroy(cube_, 2);
                }
            }
        }

        if (getClassifierID() == 0)
        {
            Main.getMain().updateWarriorCounts(Main.getMain().redwarriors - 1, Main.getMain().bluewarriors);
        }
        else
        {
            Main.getMain().updateWarriorCounts(Main.getMain().redwarriors, Main.getMain().bluewarriors - 1);
        }
    }

    public virtual void update()
    {
        if (dead)
        {
            return;
        }
        updateHealthbar(false);
        if (!said)
        {
            if (Random.Range(0, 1500) < 1)
            {
                saySomething(Main.words[Random.Range(0, Main.words.Count)]);
                said = true;
            }
        }
        else
        {
            said_c++;
            if (said_c > 200)
            {
                said_c = 0;
                said = false;
                if (canspeak)
                {
                    txt.renderer.enabled = false;
                }
            }
        }
    }

    public void checkCollide(Collision c)
    {
        if (c.gameObject.name.EndsWith("_BULLET") && !c.gameObject.name.StartsWith(classifierID.ToString()))
        {
            GameObject.Destroy(c.gameObject);
            this.health -= 1;
            updateHealthbar(true);
            if (this.health < 1)
            {
                // dead
                die(true);
                return;
            }
        }
    }

    public void attack(Entity e)
    {
        if (e.getCube() == null)
        {
            target = null;
            return;
        }
        GameObject attackcube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        attackcube.name = getClassifierName() + "_BULLET";
        attackcube.transform.localScale = new Vector3(0.1F, 0.1F, 0.1F);
        if (getClassifierID() == 0)
        {
            cube.renderer.material = Materials.redMat;
        }
        else if (getClassifierID() == 1)
        {
            cube.renderer.material = Materials.blueMat;
        }
        attackcube.transform.localRotation = cube.transform.localRotation;
        attackcube.transform.localPosition = new VectorHelper(cube.transform.localPosition).add(0, 1, 0);

        GameObject smoketrail = GameObject.Instantiate(smoke, attackcube.transform.localPosition, Quaternion.Inverse(attackcube.transform.localRotation)) as GameObject;
        smoketrail.transform.localScale = new Vector3(0.01F, 0.01F, 0.01F);
        smoketrail.transform.parent = attackcube.transform;

        Rigidbody rigid = (Rigidbody)attackcube.AddComponent(typeof(Rigidbody));
        rigid.velocity = new VectorHelper((e.getCube().transform.position - attackcube.transform.position).normalized * bullet_fly_power * 15).add(0F, 2.5F, 0F);
        //rigid.velocity.Set (rigid.velocity.x, 25, rigid.velocity.z);

        GameObject.Destroy(attackcube, 4);

        getCube().transform.LookAt(target.getCube().transform);

        if (Vector3.Distance(getCube().transform.position, e.getCube().transform.position) > 15)
        {
            this.target = null;
        }
    }

    public List<GameObject> targets = new List<GameObject>();
    private void SortTargetsByDistance()
    {
        targets.Sort(delegate(GameObject t1, GameObject t2)
        {
            if (t1 != null && t2 != null)
            {
                return Vector3.Distance(t1.transform.position, getCube().transform.position).CompareTo(Vector3.Distance(t2.transform.position, getCube().transform.position));
            }
            return 100;
        });
    }
    public void tryFindAttackTarget()
    {
        if (targets.Count > 3)
        {
            for (int i = 0; i < 4; i++)
            {
                if (targets[i] != null)
                {
                    if (Vector3.Distance(getCube().transform.position, targets[i].transform.position) < 10)
                    {
                        EntityID e = (EntityID)targets[i].GetComponent(typeof(EntityID));
                        int tempid = e.getID();

                        foreach (Entity e_ in Main.getMain().entities)
                        {
                            if (e_.getEntityID().getID() == tempid)
                            {
                                target = e_;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (Main.getMain().entities.Count > 8)
            {
                calculated = false;
            }
        }
        List<GameObject> temp = new List<GameObject>(targets);
        foreach (GameObject go in temp)
        {
            if (go == null)
            {
                targets.Remove(go);
            }
        }
    }
    bool calculated = false;
    public virtual void findTarget(int enemyId)//string fullEnemyClassifier)
    {
        if (!calculated)
        {
            targets.Clear();
            calculated = true;
            //GameObject[] go = GameObject.FindGameObjectsWithTag(fullEnemyClassifier);
            foreach (Entity e in Main.getMain().entities)
            {
                if (e.getClassifierID() == enemyId)
                {
                    targets.Add(e.getCube());
                }
            }
            SortTargetsByDistance();
            tryFindAttackTarget();
        }
        else
        {
            tryFindAttackTarget();
        }

        /*var currentPos = getCube().transform.position;

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
        }*/
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

    public GameObject getCube()
    {
        return cube;
    }

    public int getClassifierID()
    {
        return classifierID;
    }

    public string getClassifierName()
    {
        return Classifier.classifier[getClassifierID()];
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

    public EntityID getEntityID()
    {
        return entityid;
    }

    public void setEntityID(EntityID id)
    {
        entityid = id;
    }

    public void jump()
    {
        cube.rigidbody.AddForce(0, 100, 0);
    }

    public void jump(int force)
    {
        cube.rigidbody.AddForce(0, force, 0);
    }

    public int getMaxShootCooldown()
    {
        return shootcooldown_max;
    }
}
