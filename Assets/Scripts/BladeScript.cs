using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeScript : MonoBehaviour
{
    private GameObject difficultyManager;
    private float difficulty;
    public int attack1Damage;
    public int attack2Damage;
    public int attack3Damage;
    public int currentMove;

    // Start is called before the first frame update
    void Start()
    {
        // find difficulty manager
        difficultyManager = GameObject.Find("DifficultyManager");

        // get difficulty
        difficulty = difficultyManager.GetComponent<DifficultyScript>().GetDifficultyScale();

        // adjust damage
        attack1Damage = (int)(attack1Damage * difficulty);
        attack2Damage = (int)(attack2Damage * difficulty);
        attack3Damage = (int)(attack3Damage * difficulty);
    }

    public void ChangeCurrentMove(int move)
    {
        currentMove = move;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check if collision is player
        if (collision.tag == "Player")
        {
            // decide damage
            int damage = 0;
            switch (currentMove)
            {
                case 1:
                    damage = attack1Damage;
                    break;
                case 2:
                    damage = attack2Damage;
                    break;
                case 3:
                    damage = attack3Damage;
                    break;
            }

            // do damage
            collision.GetComponent<HPManager>().TakeDamage(damage);
        }
    }
}
