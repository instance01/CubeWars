using UnityEngine;
using System.Collections;
using Assets.GamePlay;
using Assets.Entities;

public class Entity
{
    public GameObject health_green;
    public GameObject health_red;
    public int health = 3;
    public int maxhealth = 3;

    public GameObject cube;

    public bool dead = false;
    public bool move_ = true;

    public Vector3 moveto;

    bool said = false;
    int said_c = 0;
    private GameObject txt;
    private TextMesh txtMesh;

    int classifierID = 0;

    /// <summary>
    /// Initializes a new entity
    /// </summary>
    /// <param name="classifierID">The classifier id (e.g. RED=0, BLUE=1)</param>
    public Entity(int classifierID)
    {
        this.classifierID = classifierID;
    }

    public void createTextGraphics()
    {
        txt = new GameObject();
        txtMesh = (TextMesh)txt.AddComponent(typeof(TextMesh));
        txtMesh.font = (Font)Resources.Load("pf_arma_five", typeof(Font));
        txt.transform.renderer.material = (Material)Resources.Load("pf_arma_five", typeof(Material));
        txt.transform.Rotate(67.5F, 0F, 0F);
        txtMesh.text = ".";
        txtMesh.color = Color.black;
        txtMesh.richText = true;
        txtMesh.fontSize = 45;
        txt.transform.localScale = new Vector3(0.2F, 0.2F, 0.2F);
        MeshRenderer meshrenderer = (MeshRenderer)txt.GetComponent(typeof(MeshRenderer));
        meshrenderer.enabled = false;
    }

    public void saySomething(string msg)
    {
        txt.transform.localPosition = cube.transform.localPosition;
        txt.transform.Translate(0, 1, 0);
        txtMesh.text = msg;
        MeshRenderer meshrenderer = (MeshRenderer)txt.GetComponent(typeof(MeshRenderer));
        meshrenderer.enabled = true;
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
        health_green.transform.localScale = new Vector3(health * 0.05F, 0.005F, 0.0075F);
        health_red.transform.localScale = new Vector3(health * 0.05F, 0.005F, 0.005F);
        health_green.transform.Rotate(new Vector3(-90F, 0F, 0F));
        health_red.transform.Rotate(new Vector3(-90F, 0F, 0F));
        health_green.transform.Rotate(67.5F, 0F, 0F);
        health_red.transform.Rotate(67.5F, 0F, 0F);
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
        if (said)
        {
            txt.transform.localPosition = cube.transform.localPosition;
            txt.transform.Translate(0, 1, 0);
        }
    }

    public virtual void update()
    {
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
                txt.renderer.enabled = false;
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
                dead = true;
                Main.getMain().entities.Remove(this);
                said = false;
                txt.renderer.enabled = false;
                GameObject.Destroy(health_green);
                GameObject.Destroy(health_red);
                Vector3 cubepos = cube.transform.localPosition;
                GameObject.Destroy(cube, 0);

                int[,] pos = new int[16, 2] { { 0, 0 }, { 0, 1 }, { 0, 2 }, { 0, 3 }, { 1, 0 }, { 1, 1 }, { 1, 2 }, { 1, 3 }, { 2, 0 }, { 2, 1 }, { 2, 2 }, { 2, 3 }, { 3, 0 }, { 3, 1 }, { 3, 2 }, { 3, 3 } };

                Vector3 cubesize = new Vector3(0.0625F * 2, 0.0625F * 2, 0.0625F * 2);
                Vector3 explosionforce = new Vector3(cubepos.x * 1.01F, cubepos.y * 0.9F, cubepos.z * 1.01F);

                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        GameObject cube_ = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube_.renderer.material.color = Color.cyan;
                        cube_.transform.localScale = cubesize;
                        cube_.transform.localPosition = (new VectorHelper(cubepos).add(pos[i, 0] * 0.0625F * 2, j * 0.0625F * 2, pos[i, 1] * 0.0625F * 2));

                        cube_.AddComponent<Rigidbody>();
                        cube_.rigidbody.AddExplosionForce((Random.Range(300.0f, 700.0f)), explosionforce, (Random.Range(100.0f, 200.0f)));
                        cube_.rigidbody.SetDensity(cube_.transform.localPosition.x);

                        GameObject.Destroy(cube_, 2);
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
                return;
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
}
