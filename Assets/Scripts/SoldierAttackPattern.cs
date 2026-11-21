using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierAttackPattern : AttackPattern
{

    public GameObject arrow;
    public float shootDelay;
    public float attack1DashRange;
    public float attack2Offset;
    public float attack2EffectDelay;
    public int attack1Cost;
    public int attack2Cost;
    public int attack3Cost;
    bool enoughEnergy;
    public Canvas EnergyWarning;
    public int playerLevel;
    public float damageScale;

    public void SoldierAttack(int move)
    {

        // check current CD
        if (GetCD(move) > 0)
        {
            return;
        }

        // check if attacking
        if (AnimationIsPlaying())
        {
            return;
        }

        // check energy
        enoughEnergy = CheckEnergy(move);

        if (!enoughEnergy)
        {
            // UI indication
            WarningMessage();

            return;
        }

        // play animation
        animator.SetInteger("attackMove", move);
        animator.SetTrigger("attack");

        // check player level
        playerLevel = GetComponent<XPManager>().playerLevel;

        // scale damage with level
        damageScale = 1f + (playerLevel - 1) * 0.05f;

        if (move == 3) // ranged arrow attack
        {
            // shoot with delay 
            // to line up with animation
            Invoke("SpawnArrow", shootDelay);
        }
        else // melee attack
        {
            SoldierDoDamage(move);

            // if move == 2 then invoke attack effect animation
            if (move == 2)
            {
                Invoke("PlayEffectAnimation", attack2EffectDelay);
            }
        }

        // cool down
        SetCD(move);
    }

    public bool AnimationIsPlaying() // check if animation is playing
    {
        return this.animator.GetCurrentAnimatorStateInfo(0).IsName("playerAttack1") ||
            this.animator.GetCurrentAnimatorStateInfo(0).IsName("playerAttack2") ||
            this.animator.GetCurrentAnimatorStateInfo(0).IsName("playerAttack3");
    }

    void WarningMessage()
    {
        EnergyWarning.enabled = true;
        Invoke("RemoveWarning", 0.5f);
    }

    void RemoveWarning()
    {
        EnergyWarning.enabled = false;
    }

    bool CheckEnergy(int move)
    {
        if (move == 1)
        {
            return GetComponent<EnergyManager>().EnergyCheck(attack1Cost);
        }
        else if (move == 2)
        {
            return GetComponent<EnergyManager>().EnergyCheck(attack2Cost);
        }
        else if (move == 3)
        {
            return GetComponent<EnergyManager>().EnergyCheck(attack3Cost);
        }
        return false;
    }

    void PlayEffectAnimation()
    {
        GameObject EffectManager = transform.Find("AttackEffectManager").gameObject;
        EffectManager.GetComponent<Animator>().SetTrigger("attack");
    }

    void SoldierDoDamage(int move)
    {
        StartCoroutine(DelayedAttack(move));
    }

    IEnumerator DelayedAttack(int move)
    {
        if (move == 1)
        {
            yield return new WaitForSeconds(attack1Delay);

            // move forward when attack
            if (GetComponent<Transform>().localScale.x > 0) // facing right
            {
                GetComponent<Rigidbody2D>().velocity = Vector3.right * attack1DashRange;
            }
            else // facing left
            {
                GetComponent<Rigidbody2D>().velocity = Vector3.left * attack1DashRange;
            }

            // check enemies in range
            Collider2D[] enemyHit = AttackRange(1);

            // do damage to each
            foreach (Collider2D enemy in enemyHit)
            {
                enemy.GetComponent<HPManager>().TakeDamage((int)(attack1Damage * damageScale));
            }
        }
        if (move == 2)
        {
            yield return new WaitForSeconds(attack2Delay);

            // check enemies in range
            Collider2D[] enemyHit = AttackRange(2);

            // do damage to each
            foreach (Collider2D enemy in enemyHit)
            {
                enemy.GetComponent<HPManager>().TakeDamage((int)(attack2Damage * damageScale));
            }
        }
    }

    void SpawnArrow()
    {
        Instantiate(arrow, attackPoint.transform.position, attackPoint.transform.rotation);
    }

    // detect enemy in range
    Collider2D[] AttackRange(int move)
    {
        if (move == 1)
        {
            // detect enemies in circle area
            return Physics2D.OverlapCircleAll(attackPoint.position, attack1Radius, enemyLayers);
        }
        else if (move == 2)
        {
            Vector3 position = attackPoint.position;

            // check face direction
            // get offset position
            if (transform.localScale.x < 0)
            { 
                position = new Vector3(position.x - attack2Offset, position.y, position.z);
            }
            else
            {
                position = new Vector3(position.x + attack2Offset, position.y, position.z);
            }

            // detect enemies in box area
            return Physics2D.OverlapBoxAll(position, new Vector2(attack2Range, 1), 0, enemyLayers);
        }
        return null;
    }

        
    // display attack area #testing purpose
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Vector3 position = attackPoint.position;

        // check face direction
        // get offset position
        if (transform.localScale.x < 0)
        {
            position = new Vector3(position.x - attack2Offset, position.y, position.z);
        }
        else
        {
            position = new Vector3(position.x + attack2Offset, position.y, position.z);
        }
        Gizmos.DrawWireSphere(attackPoint.position, attack1Radius);
        Gizmos.DrawWireCube(position, new Vector3(attack2Range, 1, 1));
    }
}
