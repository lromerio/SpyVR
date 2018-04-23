using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTest : MonoBehaviour
{

    public InputField inputField;
    public Light light;

    void CallMe(InputField input)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(input.text == "off")
            {
                light.intensity = 0;
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        inputField.onEndEdit.AddListener(delegate { CallMe(inputField);});
    }

  
    // Update is called once per frame
    void Update()
    {
        if (inputField.isFocused && Input.GetKeyUp(KeyCode.Return)) {
            inputField.text = "";
        }
    }
}
