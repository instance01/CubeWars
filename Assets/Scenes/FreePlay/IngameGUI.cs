using UnityEngine;
using System.Collections;
using Assets.Entities;
using Assets.GamePlay.Util;
using System.Collections.Generic;
using Assets.GamePlay;

public class IngameGUI : MonoBehaviour
{
    System.Random random;
    public GUISkin skin;
    private static bool open = false;
    public string cmd = "";
    public string response = "";
    bool ret = false;
    int currentheight = 20;
    float size;

    // fps
    public float updateInterval = 0.5F;

    private float accum = 0;
    private int frames = 0;
    private float timeleft;
    string fps_string = "";

    Texture gui_red_entity;
    Texture gui_blue_entity;
    GameObject current_entity;
    int current_entity_id = 0;

    Ray ray;
    RaycastHit hit;

    void Start()
    {
        timeleft = updateInterval;
        random = new System.Random();

        gui_red_entity = Resources.Load("gui_red_entity", typeof(Texture)) as Texture;
        gui_blue_entity = Resources.Load("gui_blue_entity", typeof(Texture)) as Texture;
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;
        if (timeleft <= 0.0)
        {
            float fps = accum / frames;
            fps_string = System.String.Format("{0:F2} FPS", fps);
            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
        if (current_entity != null)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            current_entity.transform.position = ray.GetPoint(10);
            current_entity.transform.Translate(-1, 0, 0);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name == "Terrain")
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        EntityUtil.spawnEntity(current_entity_id, hit.point.x, hit.point.y - 0.5F, hit.point.z);
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Destroy(current_entity);
                        current_entity = null;
                    }
                }
            }
        }
    }

    void OnGUI()
    {
        GUI.skin = skin;

        // FPS Label
        GUI.Label(new Rect(Screen.width - 150, 20, 150, 20), fps_string);

        // GameObject and Entity count Labels
        GUI.Label(new Rect(Screen.width - 150, 40, 150, 20), UnityEngine.Object.FindObjectsOfType<GameObject>().Length.ToString() + " GameObjects");
        GUI.Label(new Rect(Screen.width - 150, 60, 150, 20), Main.getMain().entities.Count.ToString() + " Entities");

        if (size < 1)
        {
            size = GUI.skin.GetStyle("Label").CalcHeight(new GUIContent("test"), 20F);
        }
        GUI.Label(new Rect(10, Screen.height - 40 - currentheight, 600, currentheight), response);
        Event e = Event.current;
        if (e.keyCode == KeyCode.Return)
        {
            ret = true;
            GUI.SetNextControlName("chatfield");
            GUI.TextField(new Rect(10, Screen.height - 40, 250, 30), "", 40);
            if (cmd == "")
            {
                ret = false;
                return;
            }
            execute();
            cmd = "";
            ret = false;
            return;
        }
        else if (ret == false)
        {
            cmd = GUI.TextField(new Rect(10, Screen.height - 40, 250, 30), cmd, 40);
            if (GUI.Button(new Rect(Screen.width - 50, 100, 40, 40), gui_red_entity))
            {
                if (current_entity == null)
                {
                    spawnSelectionTool(0, Materials.redMat);
                }
                else if (current_entity != null && current_entity_id != 0)
                {
                    current_entity.renderer.material = Materials.redMat;
                    current_entity_id = 0;
                }
                
            }
            if (GUI.Button(new Rect(Screen.width - 50, 140, 40, 40), gui_blue_entity))
            {
                if (current_entity == null)
                {
                    spawnSelectionTool(1, Materials.blueMat);
                }
                else if (current_entity != null && current_entity_id != 1)
                {
                    current_entity.renderer.material = Materials.blueMat;
                    current_entity_id = 1;
                }
            }
        }

        if (open)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, 80, 200, 30), "Camera"))
            {
                //
            }
        }
    }

    public void spawnSelectionTool(int id, Material m)
    {
        current_entity = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        current_entity.transform.position = ray.GetPoint(10);
        current_entity.transform.localScale = new Vector3(0.5F, 0.5F, 0.5F);
        current_entity.renderer.material = m;
        current_entity_id = id;
    }

    public static void setOpen(bool o)
    {
        open = o;
    }

    public static bool isOpen()
    {
        return open;
    }

    public void execute()
    {
        print(cmd);
        if (cmd.StartsWith("/"))
        {
            executeCmd();
        }
        else
        {
            sendMessage();
        }
        response += "\n";

        currentheight += (int)size;
    }

    public void sendMessage()
    {
        response += cmd;
    }

    public void executeCmd()
    {
        if (cmd == "/help")
        {
            response += "Here's a list of currently available commands:";
            response += "\n/spawnentity";
            response += "\n/killall";

            currentheight += (int)size * 2;
        }
        else if (cmd.ToLower().StartsWith("/spawnentity"))
        {
            string[] args = cmd.Split(' ');
            int count = 1;
            int id = 0;
            if (args.Length < 2)
            {
                response += "Usage. /spawnentity [id] [count]. Possible ids: 0, 1";
                return;
            }
            else if (args.Length > 2)
            {
                int.TryParse(args[2], out count);
            }
            int.TryParse(args[1], out id);
            int x = random.Next(20, 30);
            int z = random.Next(20, 30);
            if (id == 1)
            {
                x = random.Next(10, 20);
                z = random.Next(10, 20);
            }
            for (int i = 0; i < count; i++)
            {
                EntityUtil.spawnEntityAtSpawn(id, x, z);
            }
            response += "Successfully spawned " + args[2] + " entities of classifier " + Classifier.classifier[id] + ".";
        }
        else if (cmd.ToLower().StartsWith("/killall"))
        {
            List<Entity> temp = new List<Entity>(Main.getMain().entities);
            foreach (Entity e in temp)
            {
                e.die(false);
            }
            response += "Successfully killed all entities.";
        }
        else
        {
            response += "Command not found.";
        }
    }
}
