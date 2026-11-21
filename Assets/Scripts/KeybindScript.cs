using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KeybindScript : MonoBehaviour
{

    /* keys: (0 indexed)
    
    0 : MoveUp
    1 : MoveDown
    2 : MoveLeft
    3 : MoveRight
    4 : Interact
    5 : Attack1
    6 : Attack2
    7 : Attack3
    8 : Heal

    */

    public Canvas notice;
    public Canvas warning;
    public GameObject inputManager;
    private int target;

    private void Update()
    {
        if (inputManager.GetComponent<UserInput>().changingKeybinds == true)
        {
            DetectInput();
        }
    }

    public void ChangeKeybinds(int key)
    {
        target = key;
        notice.GetComponent<Canvas>().enabled = true;
        inputManager.GetComponent<UserInput>().changingKeybinds = true;
    }

    void DetectInput()
    {
        // detect input
        string input = Input.inputString;
        if (input != "")
        {
            // change keybind
            bool changeIsSuccessful = inputManager.GetComponent<UserInput>().ChangeKey(input, target);

            // switch off notice
            notice.GetComponent<Canvas>().enabled = false;
            inputManager.GetComponent<UserInput>().changingKeybinds = false;

            // change button text
            if (changeIsSuccessful)
            {
                gameObject.transform.GetChild(target).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = input;
            }
            else
            {
                // show warning message
                warning.GetComponent<Canvas>().enabled = true;

                // remove warning after delay
                Invoke("RemoveWarning", 1.5f);
            }
        }
    }

    void RemoveWarning()
    {
        // remove warning
        warning.GetComponent<Canvas>().enabled = false;
    }

    public void BackToStartMenu()
    {
        SceneManager.LoadScene("StartScene");
    }
}
