using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

    public Button button;
    GameObject mainManager;
    GameObject player;
    int refNo;

    // Start is called before the first frame update
    void Start()
    {
        mainManager = GameObject.Find("MainManager");
        player = GameObject.FindGameObjectWithTag("Player");
        refNo = this.transform.parent.gameObject.transform.parent.gameObject.GetComponent<CheckPointScript>().checkPointRefNo;
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick()
    {
        player.GetComponent<CheckPointVisitedScript>().Visited(refNo);
        mainManager.GetComponent<Player>().SavePlayer();
    }

}
