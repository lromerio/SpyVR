using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTest : MonoBehaviour
{

    public InputField inputField;
    public List<Light> lights;

    private List<int> valid_lights;

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
                    break;
            }
        }

        // Clean line and keep active
        inputField.text = "";
        inputField.ActivateInputField();
    }

    void HandleLight(string[] cmd)
    {
        // Validate arguments
        if (cmd[1].Length != 1 && !(cmd[2] == "0" || cmd[2] == "1"))
        {
            return;
        }

        // Get light ID
        int id = (int)cmd[1][0];

        // If valid ID modify light intensity
        if (valid_lights.Contains(id))
        {
            lights[id % 10].intensity = System.Int32.Parse(cmd[2]);
        }
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
    }
  
    // Update is called once per frame
    void Update()
    {
    }
}
