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
	private Color success;
	private Color failure;
	private Color hints;

    // Command history
    private bool up; 
    private bool down;
    private List<string> cmd_history;
    private int history_index;

    // Puzzles related
    public GameObject paper;
    public TextMesh hidden_text;
    public List<Light> must_be_off;
    public Cables cables;
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
                case "pc":
                    HandlePc(cmd);
                    break;
				default:
					Help();
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
		feedback.text += "pc <id> hack\n";
        feedback.color = failure;
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
			hidden_text.gameObject.SetActive(!must_be_off.Any(c => c.enabled));
			return;
        }

		feedback.text += "Invalid argument: ";
        feedback.color = failure;
    }

    void HandlePc(string[] cmd)
    {
        // Verify cmd and pc ID
		if (cmd.Length == 3 && cmd[1] == "5684668" && cmd[2] == "hack")
        {
            // "Print" paper
			if (!paper.active) {
				paper.SetActive (true);
				paper.GetComponent<AudioSource> ().Play ();
			}
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

		// Initialize puzzles related
		hidden_text.gameObject.SetActive(false);
        paper.SetActive(false);
        color_to_cut = cables.ChooseToCutColor ();
        string text = hidden_text.text;
        text = text.Replace ("%v", color_to_cut);
        hidden_text.text = text;

        // Print cables to cut
        print(text);
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
