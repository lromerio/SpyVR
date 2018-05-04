using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTest : MonoBehaviour
{

    public InputField inputField;
    public Text feedback;
    public List<Light> lights;

    private List<int> valid_lights;
    private Color success;
    private Color failure;

    void CallMe(InputField input)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Tokenize command
            string[] cmd = input.text.Split();

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
        }

        // Clean line and keep active
        feedback.text = inputField.text;
        inputField.text = "";
        inputField.ActivateInputField();
    }

    void HandleLight(string[] cmd)
    {
        // Validate arguments
        if (cmd[1].Length != 1 || !(cmd[2] == "0" || cmd[2] == "1"))
        {
            feedback.color = failure;
            return;
        }

        // Get light ID
        int id = (int)cmd[1][0];

        // If valid ID modify light intensity
        if (valid_lights.Contains(id))
        {
            lights[id % 10].intensity = System.Int32.Parse(cmd[2]);
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

    }

    // Use this for initialization
    void Start()
    {
        // Add listener
        inputField.onEndEdit.AddListener(delegate { CallMe(inputField);});

        // Initialize list of valid lights ID (use ascii code)
        valid_lights = new List<int> {50, 61, 72, 83}; // 2 = H S

        // Initialize colors
        success = new Color(0.2f, 0.7f, 0.1f, 1.0f);
        failure = new Color(0.7f, 0.2f, 0.1f, 1.0f);
    }
  
    // Update is called once per frame
    void Update()
    {
    }
}
