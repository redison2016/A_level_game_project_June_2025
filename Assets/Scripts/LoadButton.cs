using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour
{
    public Button button;
    GameObject mainManager;

    // Start is called before the first frame update
    void Start()
    {
        mainManager = GameObject.Find("MainManager");
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick()
    {
        mainManager.GetComponent<Player>().LoadPlayer();
    }
}
