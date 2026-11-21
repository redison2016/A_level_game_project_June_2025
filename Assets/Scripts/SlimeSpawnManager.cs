using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SlimeSpawnManager : MonoBehaviour
{

    private bool playerInArea = false;
    public GameObject enemyType;
    [SerializeField] private float spawnTimer;
    private float currentTime;
    [SerializeField] private LayerMask enemyLayers;

    // Update is called once per frame
    void Update()
    {
        // check if player is in area
        if (!playerInArea)
        {
            return;
        }

        // decrement timer
        currentTime -= Time.deltaTime;

        // check spawn timer
        if (currentTime > 0)
        {
            return;
        }

        SpawnEnemy(enemyType);

        // reset timer
        currentTime = spawnTimer;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInArea = false;
            currentTime = 0;

            // clear enemy in area
            Collider2D[] enemies = Physics2D.OverlapBoxAll(gameObject.transform.position, new Vector2(60f, 35f), 0, enemyLayers);
            foreach (Collider2D enemy in enemies)
            {
                enemy.GetComponent<HPManager>().Despond();
            }
        }
    }

    private void SpawnEnemy(GameObject enemy)
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-30f, 30f), Random.Range(-115f, -88f), 0);
        Instantiate(enemy, spawnPosition, Quaternion.identity);
    }

}
