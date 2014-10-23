using UnityEngine;
using System.Collections;
using Assets.Entities;
using Assets.GamePlay.Util;
using System.Collections.Generic;

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

    void Start()
    {
        timeleft = updateInterval;
        random = new System.Random();
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
    }

    void OnGUI()
    {
        GUI.skin = skin;

        // FPS Label
        GUI.Label(new Rect(Screen.width - 150, 20, 150, 20), fps_string);

        // GameObject and Entity count Labels
        GUI.Label(new Rect(Screen.width - 150, 40, 150, 20), UnityEngine.Object.FindObjectsOfType<GameObject>().Length.ToString() + " GameObjects");
        GUI.Label(new Rect(Screen.width - 150, 60, 150, 20), Main.getMain().entities.Count.ToString() + " Entities");

        size = GUI.skin.GetStyle("Label").CalcHeight(new GUIContent("test"), 20F);
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
        }

        if (open)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, 80, 200, 30), "Camera"))
            {
                //
            }
        }
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
            response += "Here's a list of current available commands:";
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
            int y = random.Next(20, 30);
            if (id == 1)
            {
                x = random.Next(10, 20);
                y = random.Next(10, 20);
            }
            for (int i = 0; i < count; i++)
            {
                EntityUtil.spawnEntity(id, x, y);
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
        }
        else
        {
            response = "Command not found.";
        }
    }
}
