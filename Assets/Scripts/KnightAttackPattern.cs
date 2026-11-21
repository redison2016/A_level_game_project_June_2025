using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnightAttackPattern : AttackPattern
{
    public float attack1DashRange;
    public float attack3Offset;
    public int attack3Cost;
    bool enoughEnergy;
    private Canvas EnergyWarning;
    public int playerLevel;
    public float damageScale = 1;
    int lastMove = 2;
    [SerializeField] float parryDuration;

    private void Start()
    {
        // find UI
        EnergyWarning = GameObject.Find("Main Camera").transform.Find("EnergyWarning").gameObject.GetComponent<Canvas>();
    }

    public void KnightAttack(int move)
    {
        // check if attacking
        if (InAttackDuration())
        {
            return;
        }
        
        // give knight the unique style
        // combining move 1 and 2 so it alternates

        // parry
        if (move == 3)
        {
            Parry();
            SetCD(4);
            return;
        }
        
        // make input for move 2 becomes move 3
        if (move == 2)
        {
            move = 3;
        }

        // check current CD
        if (GetCD(move) > 0)
        {
            return;
        }

        // alternate attacks
        if (move == 1 && lastMove == 1)
        {
            move = 2;
        }
        if (move == 1 && lastMove == 2) 
        {
            move = 1;
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

        DoDamage(move);

        // cool down
        SetCD(move);

        lastMove = move;
    }

    private void Parry()
    {
        animator.SetTrigger("Parry");
        GetComponent<HPManager>().parrying = true;
        Invoke("EndParry", parryDuration);
    }

    private void EndParry()
    {
        GetComponent<HPManager>().parrying = false;
    }

    public bool AnimationIsPlaying() // check if animation is playing
    {
        return this.animator.GetCurrentAnimatorStateInfo(0).IsName("KnightAttack1") ||
            this.animator.GetCurrentAnimatorStateInfo(0).IsName("KnightAttack2") ||
            this.animator.GetCurrentAnimatorStateInfo(0).IsName("KnightAttack3") ||
            this.animator.GetCurrentAnimatorStateInfo(0).IsName("KnightBlock");
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
            return true;
        }
        if (move == 2)
        {
            return true;
        }
        else if (move == 3)
        {
            return GetComponent<EnergyManager>().EnergyCheck(attack3Cost);
        }
        return false;
    }

    void DoDamage(int move)
    {
        StartCoroutine(DelayedAttack(move));
        if (move == 2)
        {
            Invoke("SecondAttack", attack1Delay + 0.3f);
        }
    }

    void SecondAttack()
    {
        StartCoroutine(DelayedAttack(2));
    }

    IEnumerator DelayedAttack(int move)
    {
        if (move == 1)
        {
            yield return new WaitForSeconds(attack1Delay);
            
            // play attack sound effect
            audioData.clip = attackSoundEffect;
            audioData.Play(0);

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

            // play attack sound effect
            audioData.clip = attackSoundEffect;
            audioData.Play(0);

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
                enemy.GetComponent<HPManager>().TakeDamage((int)(attack2Damage * damageScale));
            }
        }
        if (move == 3)
        {
            yield return new WaitForSeconds(attack3Delay);

            // play attack sound effect
            audioData.clip = attackSoundEffect;
            audioData.Play(0);

            // check enemies in range
            Collider2D[] enemyHit = AttackRange(2);

            // do damage to each
            foreach (Collider2D enemy in enemyHit)
            {
                enemy.GetComponent<HPManager>().TakeDamage((int)(attack3Damage * damageScale));
            }
        }
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
                position = new Vector3(position.x - attack3Offset, position.y, position.z);
            }
            else
            {
                position = new Vector3(position.x + attack3Offset, position.y, position.z);
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
            position = new Vector3(position.x - attack3Offset, position.y, position.z);
        }
        else
        {
            position = new Vector3(position.x + attack3Offset, position.y, position.z);
        }
        Gizmos.DrawWireSphere(attackPoint.position, attack1Radius);
        Gizmos.DrawWireCube(position, new Vector3(attack2Range, 1, 1));
    }
}
