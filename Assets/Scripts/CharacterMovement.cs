using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

// this script can apply on any character
public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 10;
    public Transform tf;
    public Animator animator;
    public float jumpStrength;
    float speedConstant;
    float scaleX;
    float scaleY;
    float scaleZ;
    bool attacking = false;
    public float decelerateConstant = 10;
    public GameObject attachments;
    private GameObject mainManager;

    private void Start()
    {
        // get main Manager
        mainManager = GameObject.Find("MainManager");

        if (gameObject.tag == "Player")
        {
            // get position from save file
            transform.position = mainManager.GetComponent<Player>().position;
        }
    }

    // Update is called once per frame
    void Update() 
    {
        // check if animation is playing
        // add new character here
        switch (gameObject.GetComponent<Character>().characterName)
        {
            case "Soldier":
                attacking = GetComponent<SoldierAttackPattern>().AnimationIsPlaying();
                break;
            case "Orc":
                attacking = GetComponent<OrcAttackPattern>().AnimationIsPlaying();
                break;
            case "Slime":
                attacking = GetComponent<SlimeAttackPattern>().AnimationIsPlaying();
                break;
            case "Knight":
                attacking = GetComponent<KnightAttackPattern>().AnimationIsPlaying();
                break;
            case "ArmoredOrc":
                attacking = GetComponent<OrcAttackPattern>().AnimationIsPlaying();
                break;
            case "OrcKing":
                attacking = GetComponent<OrcKingAttackPattern>().AnimationIsPlaying();
                break;
        }

        // slows down character 
        float newVelocityX = GetComponent<Rigidbody2D>().velocity.x - (GetComponent<Rigidbody2D>().velocity.x * decelerateConstant * Time.deltaTime);
        float newVelocityY = GetComponent<Rigidbody2D>().velocity.y;
        GetComponent<Rigidbody2D>().velocity = new Vector2(newVelocityX, newVelocityY);
    }

     public void Move(int velocityX, int velocityY)
    {
        if (attacking)
        {
            return;
        }

        // prevent diagnal walking speed difference
        speedConstant = moveSpeed;

        if (velocityY != 0 && velocityX != 0)
        {
            speedConstant = (float)(speedConstant * 0.7);
        }

        // movement
        if (velocityY > 0) // up
        {
            transform.position = transform.position + Vector3.up * speedConstant * Time.deltaTime;
        }
        else if (velocityY < 0) // down
        {
            transform.position = transform.position + Vector3.down * speedConstant * Time.deltaTime;
        }
        if (velocityX > 0) // right
        {
            transform.position = transform.position + Vector3.right * speedConstant * Time.deltaTime;
        }
        else if (velocityX < 0) // left
        {
            transform.position = transform.position + Vector3.left * speedConstant * Time.deltaTime;
        }

        // changing sprite face direction
        scaleX = tf.localScale.x;
        scaleY = tf.localScale.y;
        scaleZ = tf.localScale.z;

        if ((velocityX == 1 && scaleX < 0) || (velocityX == -1 && scaleX > 0))
        {
            tf.localScale = new Vector3(-scaleX, scaleY, scaleZ);

            // prevent changing attachments face direction
            if (attachments != null)
            {
                Vector3 barScale = attachments.transform.localScale;
                attachments.transform.localScale = new Vector3(-barScale.x, barScale.y, barScale.z);
            }
        }

        // change animation if walking
        animator.SetFloat("speed", Math.Abs(velocityX) + Math.Abs(velocityY));
    }
}
