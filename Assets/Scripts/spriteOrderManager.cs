using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteOrderManager : MonoBehaviour
{

    GameObject player;
    float playerPosition;
    float objectPosition;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }

        // find position
        playerPosition = player.transform.position.y;
        objectPosition = transform.position.y;

        // change order
        if (playerPosition > objectPosition)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 3;
        }
        else
        {
            GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }
}
