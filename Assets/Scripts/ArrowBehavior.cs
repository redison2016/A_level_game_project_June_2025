using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ArrowBehavior : MonoBehaviour
{

    public float travelSpeed;
    public int arrowDamage;
    string direction;
    public Transform tf;
    public float despondTimer = 5;
    public float damageScale;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Soldier").transform.localScale.x > 0)
        {
            direction = "right";
        }
        else
        {
            direction = "left";

            // change arrow facing direction
            tf.localScale = new Vector3(-tf.localScale.x, tf.localScale.y, tf.localScale.z);
        }

        Invoke("DestroyArrow", despondTimer); // Destroy arrow if it doesn't hit anything
    }

    private void Update()
    {
        if (direction == "right")
        {
            transform.position += Vector3.right * travelSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.left * travelSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D enemy)
    {
        if (enemy.gameObject.layer == LayerMask.NameToLayer("Enemy")) // check if it is an enemy
        {
            // check damage scale
            damageScale = GameObject.FindGameObjectWithTag("Player").GetComponent<SoldierAttackPattern>().damageScale;

            enemy.GetComponent<HPManager>().TakeDamage((int)(arrowDamage * damageScale));
            DestroyArrow(); // Destroy arrow instantly if it hits an enemy
        }
        if (enemy.gameObject.layer == LayerMask.NameToLayer("Structure"))
        {
            DestroyArrow(); // Destroy arrow if it hits any structure
        }
    }

    void DestroyArrow()
    {
        Destroy(gameObject);
    }

}
