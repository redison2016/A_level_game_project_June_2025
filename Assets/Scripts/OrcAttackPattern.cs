using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class OrcAttackPattern : AttackPattern
{

    public void OrcAttack(int move)
    {
        if (GetCD(move) > 0) // if ability in CD, don't do anything
        {
            return;
        }

        // play animation
        animator.SetInteger("attackMove", move);
        animator.SetTrigger("attack");

        // melee attack
        EnemyDoDamage(move);

        // cool down
        SetCD(move);
    }

    public bool AnimationIsPlaying() // check if animation is playing
    {
        if (gameObject.tag == "Orc")
        {
            return this.animator.GetCurrentAnimatorStateInfo(0).IsName("orcAttack");
        }
        return this.animator.GetCurrentAnimatorStateInfo(0).IsName("ArmoredOrcAttack");
    }
}


