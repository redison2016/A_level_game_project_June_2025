using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class EnemyBehavior : MonoBehaviour
{
    GameObject player;
    Vector3 playerPosition;
    float playerPositionX;
    float playerPositionY;
    Vector3 ownPosition;
    float ownPositionX;
    float ownPositionY;
    int velocityX = 0;
    int velocityY = 0;
    public bool enemyInRange = false;
    public float sightRange = 60f;
    private float distanceX;
    private float distanceY;

    private void Start()
    {
        // find player
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            // try find player again
            player = GameObject.FindGameObjectWithTag("Player");
        }
        FollowPlayer();
    }

    void FollowPlayer()
    {
        if (player == null) // don't move if can't find player
        {
            GetComponent<CharacterMovement>().Move(velocityX, velocityY);
            enemyInRange = false;
            return;
        }

        if (enemyInRange || gameObject.tag == "DeadObject") // dont move if player in attack range OR the enemy has died
        {
            GetComponent<CharacterMovement>().Move(velocityX, velocityY);
            return;
        }

        // check player position
        playerPosition = player.transform.position;
        playerPositionX = playerPosition.x;
        playerPositionY = playerPosition.y;

        // check own position
        ownPosition = transform.position;
        ownPositionX = ownPosition.x;
        ownPositionY = ownPosition.y;

        // check if player within sight range
        distanceX = Math.Abs(playerPositionX - ownPositionX);
        distanceY = Math.Abs(playerPositionY - ownPositionY);

        if (distanceX > sightRange || distanceY > sightRange)
        {
            GetComponent<CharacterMovement>().Move(velocityX, velocityY);
            return;
        }

        // set velocity towards player
        if ((int)playerPositionX > (int)ownPositionX)
        {
            velocityX += 1;
        }
        else if ((int)playerPositionX < (int)ownPositionX)
        {
            velocityX -= 1;
        }

        if (playerPositionY > ownPositionY)
        {
            velocityY += 1;
        }
        else if (playerPositionY < ownPositionY)
        {
            velocityY -= 1;
        }

        // call move from CharacterMovement
        GetComponent<CharacterMovement>().Move(velocityX, velocityY);

        // reset velocity
        velocityX = 0;
        velocityY = 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(sightRange, sightRange, 1));
    }
}
