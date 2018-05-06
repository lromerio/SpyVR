using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTest : MonoBehaviour
{
    // Input field
    public InputField inputField;
    public Text feedback;
    private Color success;
    private Color failure;

    // Command history
    private bool up; 
    private bool down;
    private List<string> cmd_history;
    private int history_index;

    // Paper
    public GameObject paper;

    void CallMe()
    {
        if (Input.GetKeyDown(KeyCode.Return) && inputField.text.Length > 0)
        {
            // Tokenize command
            string[] cmd = inputField.text.Split();

            // Handle valid commands
            switch (cmd[0])
            {
                case "light":
                    HandleLight(cmd);
                    break;
                case "door":
                    HandleDoor(cmd);
                    break;
                case "pc":
                    HandlePc(cmd);
                    break;
                default:
                    feedback.color = failure;
                    break;
            }

            // Update command history and feedback field
            feedback.text = inputField.text;
            cmd_history.Add(inputField.text);
            history_index = cmd_history.Count;

            // Clear inputfield
            inputField.text = "";
            inputField.ActivateInputField();
        }
    }

    void HandleLight(string[] cmd)
    {
        // Try to get light
        Light l = null;
        try
        {
            l = GameObject.Find(cmd[1]).GetComponent<Light>();
        }
        catch
        {
            // Do nothing
        }

        // If valid light and command, execute it
        if (l != null && (cmd[2] == "off" || cmd[2] == "on"))
        {
            l.enabled = cmd[2] == "on";
            feedback.color = success;
            return;
        }
        
        feedback.color = failure;
    }

    void HandleDoor(string[] cmd)
    {

    }

    void HandlePc(string[] cmd)
    {
        paper.SetActive(true);

        feedback.color = success;
    }

    void UpdateCurrentCmd(int x)
    {
        history_index = x;
        if (cmd_history.Count > 0)  // Avoid index out of bound
        {
            inputField.text = cmd_history[history_index];
            inputField.MoveTextEnd(false);
        }
    }

    void Start()
    {
        // Add listener
        inputField.onEndEdit.AddListener(delegate { CallMe();});

        // Initialize colors
        success = new Color(0.2f, 0.7f, 0.1f, 1.0f);
        failure = new Color(0.7f, 0.2f, 0.1f, 1.0f);

        // Initialize command history
        up = false;
        down = false;
        cmd_history = new List<string>();
        history_index = 0;

        paper.SetActive(false);
    }

    void Update()
    {
        // Get current key state
        bool up_new = Input.GetKey("up");
        bool down_new = Input.GetKey("down");

        // Navigate command history
        if (!up_new && up)
            UpdateCurrentCmd(Math.Max(0, --history_index));
        if (!down_new && down)
            UpdateCurrentCmd(Math.Min(cmd_history.Count - 1, ++history_index));

        // Update key state
        up = up_new;
        down = down_new;
    }
}
