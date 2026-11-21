using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script can apply on any attack point
public class RangeCheckScript : MonoBehaviour
{

    public GameObject attacker;

    private void Update()
    {
        // check if enemy is in range && make sure attacker is not dead
        if (attacker.GetComponent<EnemyBehavior>().enemyInRange == true && attacker.tag != "DeadObject")
        {
            // attack if enemy in range
            switch (attacker.GetComponent<Character>().characterName)
            {
                case "Orc":
                    attacker.GetComponent<OrcAttackPattern>().OrcAttack(1);
                    break;
                case "Slime":
                    attacker.GetComponent<SlimeAttackPattern>().SlimeAttack(1);
                    break;
                case "ArmoredOrc":
                    attacker.GetComponent<OrcAttackPattern>().OrcAttack(1);
                    break;
            }
        }
    }

    // when object enters the attack range
    private void OnTriggerEnter2D(Collider2D other)
    {
        // check if object is the player
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) 
        {
            return;
        }

        // stop attacker moving if enemy in range and attack
        attacker.GetComponent<EnemyBehavior>().enemyInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return;
        }

        // let attacker move if enemy not in range
        attacker.GetComponent<EnemyBehavior>().enemyInRange = false;
    }

}
