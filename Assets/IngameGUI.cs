using UnityEngine;
using System.Collections;

public class IngameGUI : MonoBehaviour {

    public GUISkin skin;
    private static bool open = false;
    public string cmd = "";
    public string response = "";
    bool ret = false;
    int currentheight = 20;
    float size;

    void OnGUI()
    {
        GUI.skin = skin;
        size = GUI.skin.GetStyle("Label").CalcHeight(new GUIContent("test"), 20F);
        GUI.Label(new Rect(10, Screen.height - 40 - currentheight, 400, currentheight), response);
        Event e = Event.current;
        if (e.keyCode == KeyCode.Return)
        {
            ret = true;
            GUI.TextField(new Rect(10, Screen.height - 40, 200, 30), "", 25);
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
            cmd = GUI.TextField(new Rect(10, Screen.height - 40, 200, 30), cmd, 25);
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
        else
        {
            response = "Command not found.";
        }
    }
}
