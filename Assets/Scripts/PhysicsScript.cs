using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhysicsScript : MonoBehaviour
{

    public bool gravity = false;
    public float scale;
    int[] characterLayers = {6, 7};

    // Update is called once per frame
    void Update()
    {
        //// test
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    if (gravity == false)
        //    {
        //        ChangeGravityScale();
        //        gravity = true;
        //    }
        //    else
        //    {
        //        ClearGravityScale();
        //        gravity = false;
        //    }
        //}
    }

    public void ChangeGravityScale()
    {
        gravity = true;

        // get all objects
        GameObject[] characters = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject character in characters)
        {
            // check if object is character
            int layer = character.layer;
            if (characterLayers.Contains(layer))
            {
                // change gravity scale
                character.GetComponent<Rigidbody2D>().gravityScale = scale;

                // change character collider
                character.GetComponent<Collider2D>().isTrigger = false;
            }
        }
    }

    public void ClearGravityScale()
    {
        gravity = false;

        GameObject[] characters = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject character in characters)
        {
            int layer = character.layer;
            if (characterLayers.Contains(layer))
            {
                // change gravity scale
                character.GetComponent<Rigidbody2D>().gravityScale = 0;
                
                // clear charater velocity
                character.GetComponent<Rigidbody2D>().velocity = new Vector2(character.GetComponent<Rigidbody2D>().velocity.x, 0);

                // change character collider
                character.GetComponent<Collider2D>().isTrigger = true;
            }
        }
    }
}
