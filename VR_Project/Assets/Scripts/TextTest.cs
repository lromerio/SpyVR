using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TextTest : MonoBehaviour
{
    // Input field
	public InputField inputField;
	public Text feedback;
	public Cables cables;
	private Color success;
	private Color failure;
	private Color hints;

	// Hidden symbols
	public TextMesh dark_text;
    public List<Light> mustBeOff;

    // Command history
    private bool up; 
    private bool down;
    private List<string> cmd_history;
    private int history_index;

    // Paper
    public GameObject paper;

    private string color_to_cut;
    void CallMe()
    {
        if (Input.GetKeyDown(KeyCode.Return) && inputField.text.Length > 0)
        {
            // Tokenize command
            string[] cmd = inputField.text.Split();

			feedback.text = "";

            // Handle valid commands
            switch (cmd[0])
			{
                case "light":
                    HandleLight(cmd);
                    break;
                /*
                case "door":
                    HandleDoor(cmd);
                    break;
                */
                case "pc":
                    HandlePc(cmd);
                    break;
				default:
					Help();
					feedback.color = failure;
                    break;
            }

			// Update command history and feedback field
			feedback.text += inputField.text;
            cmd_history.Add(inputField.text);
            history_index = cmd_history.Count;

            // Clear inputfield
            inputField.text = "";
            inputField.ActivateInputField();
        }
    }

	void Help() {
		feedback.color = hints;
		feedback.text = "Hints: ";
		//feedback.text += "light <id> on|off, ";
		feedback.text += "pc <id> hack";
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
		if (cmd.Length == 3 && l != null && (cmd[2] == "off" || cmd[2] == "on"))
        {
            l.enabled = cmd[2] == "on";
            feedback.color = success;

            // Show hidden text
			dark_text.gameObject.SetActive(!mustBeOff.Any(c => c.enabled));
			return;
        }

		feedback.text += "Invalid argument: ";
        feedback.color = failure;
    }

	void HandleDoor(string[] cmd)
    {
		// Try to get door
		GameObject obj = null;
		try
		{
			obj = GameObject.Find(cmd[1]);
		}
		catch
		{
			// Do nothing
		}

		// If valid light and command, execute it
		if (obj != null)
		{
			Door door = obj.GetComponent<Door> (); 
			if (cmd [2] == "open") {
				door.move_y(1.7f * door.transform.localScale.y);
				feedback.color = success;
				return;
			}

			if (cmd [2] == "close") {
				door.move_y(0.1857729f * door.transform.localScale.y);
				feedback.color = success;
				return;
			}
		}

		feedback.text += "Invalid argument: ";
		feedback.color = failure;
    }

    void HandlePc(string[] cmd)
    {
		if (cmd.Length == 3 && cmd[1] == "5684668" && cmd[2] == "hack")
        {
            // "Print" paper
            paper.SetActive(true);
            feedback.color = success;
            return;
        }

		feedback.text += "Invalid argument: ";
        feedback.color = failure;
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
		hints = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        // Initialize command history
        up = false;
        down = false;
        cmd_history = new List<string>();
        history_index = 0;

		// Initialize text
		dark_text.gameObject.SetActive(false);
        paper.SetActive(false);
 

        color_to_cut = cables.ChooseToCutColor ();
        string text = dark_text.text;
        text = text.Replace ("%v", color_to_cut);
        print (text);
        dark_text.text = text;
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
