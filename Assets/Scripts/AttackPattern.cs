using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attack Pattern Parent Script
[RequireComponent(typeof(AudioSource))]
public class AttackPattern : MonoBehaviour
{

    public Animator animator;
    public float attack1CD;
    public float attack2CD;
    public float attack3CD;
    public float attack4CD;
    float attack1CurrentCD;
    float attack2CurrentCD;
    float attack3CurrentCD;
    public Transform attackPoint;
    public float attack1Radius;
    public float attack2Range;
    public float attack1Delay;
    public float attack2Delay;
    public float attack3Delay;
    public LayerMask enemyLayers;
    public int attack1Damage;
    public int attack2Damage;
    public int attack3Damage;
    private float attackDuration = 0;
    public AudioSource audioData;
    public AudioClip attackSoundEffect;
    private GameObject difficultyManager;
    private float difficulty;

    private void Start()
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

    // Update is called once per frame
    void Update() // calculate CD
    {
        if (attack1CurrentCD > 0)
        {
            attack1CurrentCD -= Time.deltaTime;
        }
        if (attack2CurrentCD > 0)
        {
            attack2CurrentCD -= Time.deltaTime;
        }
        if (attack3CurrentCD > 0)
        {
            attack3CurrentCD -= Time.deltaTime;
        }
        if (attackDuration > 0)
        {
            attackDuration -= Time.deltaTime;
        }
    }

    public void SetCD(int move)
    {
        if (move == 1)
        {
            attack1CurrentCD = attack1CD;
            attackDuration = attack1CD;
        }
        else if (move == 2)
        {
            attack2CurrentCD = attack2CD;
            attackDuration = attack2CD;
        }
        else if (move == 3)
        {
            attack3CurrentCD = attack3CD;
            attackDuration = attack3CD;
        }
        else if (move == 4)
        {
            attackDuration = attack4CD;
        }
    }

    public float GetCD(int move)
    {
        if (move == 1)
        {
            return attack1CurrentCD;
        }
        if (move == 2)
        {
            return attack2CurrentCD;
        }
        if (move == 3)
        {
            return attack3CurrentCD;
        }
        return -1;
    }

    public bool InAttackDuration()
    {
        if (attackDuration > 0)
        {
            return true;
        }
        return false;
    }


    public void EnemyDoDamage(int move)
    {
        StartCoroutine(DelayedAttack(move));
    }

    IEnumerator DelayedAttack(int move)
    {
        if (move == 1)
        {
            yield return new WaitForSeconds(attack1Delay);

            // play attack sound effect
            audioData.clip = attackSoundEffect;
            audioData.Play(0);

            Collider2D[] enemyHit = AttackRange(1);
            if (GetComponent<HPManager>().currentHP > 0) // final check if character is not dead
            {
                foreach (Collider2D enemy in enemyHit)
                {
                    enemy.GetComponent<HPManager>().TakeDamage(attack1Damage);
                }
            }
        }
        if (move == 2)
        {
            yield return new WaitForSeconds(attack2Delay);

            // play attack sound effect
            audioData.clip = attackSoundEffect;
            audioData.Play(0);
            
            Collider2D[] enemyHit = AttackRange(2);
            if (GetComponent<HPManager>().currentHP > 0) // final check if character is not dead
            {
                foreach (Collider2D enemy in enemyHit)
                {
                    Debug.Log(enemy);
                    enemy.GetComponent<HPManager>().TakeDamage(attack2Damage);
                }
            }
        }
    }

    // detect enemy in range
    Collider2D[] AttackRange(int move)
    {
        if (move == 1)
        {
            return Physics2D.OverlapCircleAll(attackPoint.position, attack1Radius, enemyLayers);
        }
        else if (move == 2)
        {
            return Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(attack2Range, 1), 0, enemyLayers);
        }
        return null;
    }

    // display range #test
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attack1Radius);
    }
}

