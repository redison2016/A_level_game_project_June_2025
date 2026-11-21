using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttackPattern : AttackPattern
{
    public void SlimeAttack(int move)
    {
        if (GetCD(move) > 0) // if ability in CD, don't do anything
        {
            return;
        }

        // play animation
        animator.SetTrigger("attack");

        // melee attack
        EnemyDoDamage(move);

        // cool down
        SetCD(move);
    }

    public bool AnimationIsPlaying() // check if animation is playing
    {
        return this.animator.GetCurrentAnimatorStateInfo(0).IsName("slimeAttack1");
    }
}
