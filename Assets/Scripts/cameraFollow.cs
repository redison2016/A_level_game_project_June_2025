using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject target;
    float positionX;
    float positionY;
    float selfPosX;
    float selfPosY;
    float displacementX;
    float displacementY;
    float speedX;
    float speedY;
    public int speedConstant;
    public float tolorence;
    bool smoothingIsOn = true;
    GameObject mainManager;

    private void Start()
    {
        // default target player
        target = GameObject.FindGameObjectWithTag("Player");

        // find main manager
        mainManager = GameObject.Find("MainManager");

        // initialise camera position
        Vector3 playerPosition = mainManager.GetComponent<Player>().position;
        playerPosition.z = -10;
        transform.position = playerPosition;
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget(target);
    }

    void FollowTarget(GameObject target)
    {
        // horizontal component
        positionX = target.transform.position.x;
        selfPosX = transform.position.x;
        displacementX = positionX - selfPosX;

        if (smoothingIsOn && Math.Abs(displacementX) < tolorence) // smoothing camera
        {
            displacementX = 0;
        }

        if (displacementX < 0) // if displacement is negative
        {
            displacementX = Math.Abs(displacementX);
            speedX = - (float)(speedConstant * Math.Sqrt(displacementX));
        }
        else
        {
            speedX = (float)(speedConstant * Math.Sqrt(displacementX));
        }

        // vertical component
        positionY = target.transform.position.y;
        selfPosY = transform.position.y;
        displacementY = positionY - selfPosY;

        if (Math.Abs(displacementY) < tolorence) // smoothing camera
        {
            displacementY = 0;
        }

        if (displacementY < 0) // if displacement is negative
        {
            displacementY = Math.Abs(displacementY);
            speedY = - (float)(speedConstant * Math.Sqrt(displacementY));
        }
        else
        {
            speedY = (float)(speedConstant * Math.Sqrt(displacementY));
        }

        GetComponent<Rigidbody2D>().velocity = new Vector2(speedX, speedY);
    }

    public void ChangeTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    [ContextMenu("ScreenShakeOn")]
    public void ScreenShakeOn()
    {
        smoothingIsOn = false;
        speedConstant = 100;
    }

    [ContextMenu("ScreenShakeOff")]
    public void ScreenShakeOff()
    {
        smoothingIsOn = true;
        speedConstant = 10;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }
}
