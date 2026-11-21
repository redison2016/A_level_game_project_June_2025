using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OrcKingAttackPattern : MonoBehaviour
{
    Animator animator;
    public float attack1CD;
    public float attack2CD;
    public float attack3CD;
    float attack1CurrentCD = 0;
    float attack2CurrentCD = 0;
    float attack3CurrentCD = 0;
    public AudioSource audioData;
    public AudioClip attackSoundEffect;
    BladeScript blade;
    cameraFollow mainCamera;
    bool gravityIsOn;
    PhysicsScript physics;
    public float dashSpeed;
    GameObject player;
    public float attack3DashConstant;
    public float attack1Radius;
    public float[] attack2Range;
    public float attack3Radius;

    private void Start()
    {
        animator = GetComponent<Animator>();

        blade = transform.Find("Blade").GetComponent<BladeScript>();

        mainCamera = GameObject.Find("Main Camera").GetComponent<cameraFollow>();

        gravityIsOn = true;

        physics = GameObject.Find("PhysicsManager").GetComponent<PhysicsScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "DeadObject")
        {
            return;
        }

        UpdateCurrentCD();

        if (AnimationIsPlaying())
        {
            return;
        }

        if (attack1CurrentCD <= 0 && InAttack1Range())
        {
            Attack1();
            attack1CurrentCD = attack1CD;
        }

        if (attack2CurrentCD <= 0 && InAttack2Range())
        {
            Attack2();
            attack2CurrentCD = attack2CD;
        }

        if (attack3CurrentCD <= 0 && InAttack3Range())
        {
            Attack3();
            attack3CurrentCD = attack3CD;
        }
    }

    void UpdateCurrentCD()
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
    }

    public void InitialiseCD()
    {
        attack1CurrentCD = 0;
        attack2CurrentCD = 5;
        attack3CurrentCD = 20;
    }

    bool InAttack1Range()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            return false;
        }

        float displacementX = player.transform.position.x - gameObject.transform.position.x;
        float displacementY = player.transform.position.y - gameObject.transform.position.y;

        float distance = (float)Math.Sqrt(Math.Pow(displacementX, 2) + Math.Pow(displacementY, 2));

        if (distance <= attack1Radius)
        {
            return true;
        }

        return false;
    }

    bool InAttack2Range()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            return false;
        }

        float displacementX = player.transform.position.x - gameObject.transform.position.x;
        float displacementY = player.transform.position.y - gameObject.transform.position.y;

        if (Math.Abs(displacementX) <= attack2Range[0]/2 && Math.Abs(displacementY) <= attack2Range[1]/2)
        {
            return true;
        }

        return false;
    }

    bool InAttack3Range()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            return false;
        }

        float displacementX = player.transform.position.x - gameObject.transform.position.x;
        float displacementY = player.transform.position.y - gameObject.transform.position.y;

        float distance = (float)Math.Sqrt(Math.Pow(displacementX, 2) + Math.Pow(displacementY, 2));

        if (distance <= attack3Radius)
        {
            return true;
        }

        return false;
    }

    [ContextMenu("Attack1")]
    void Attack1()
    {
        blade.ChangeCurrentMove(1);
        animator.SetInteger("attackMove", 1);
        animator.SetTrigger("attack");
    }

    [ContextMenu("Attack2")]
    void Attack2()
    {
        blade.ChangeCurrentMove(2);
        animator.SetInteger("attackMove", 2);
        animator.SetTrigger("attack");
    }

    [ContextMenu("Attack3")]
    void Attack3()
    {
        blade.ChangeCurrentMove(3);
        animator.SetInteger("attackMove", 3);
        animator.SetTrigger("attack");
    }

    // animation event
    public void CameraShakeOn()
    {
        mainCamera.ScreenShakeOn();
    }

    // animation events
    public void CameraShakeOff()
    {
        mainCamera.ScreenShakeOff();
    }

    // animtion event
    public void ChangeGravity()
    {
        if (gravityIsOn)
        {
            // switch it off
            physics.ClearGravityScale();
            gravityIsOn = false;
        }
        else
        {
            // switch it on
            physics.ChangeGravityScale();
            gravityIsOn = true;
        }
    }

    // animation event
    public void GainVelocity()
    {
        if (transform.localScale.x > 0) // facing right
        {
            Vector3 vel = GetComponent<Rigidbody2D>().velocity;
            vel.x = dashSpeed;
            GetComponent<Rigidbody2D>().velocity = vel;
        }
        else // facing left
        {
            Vector3 vel = GetComponent<Rigidbody2D>().velocity;
            vel.x = - dashSpeed;
            GetComponent<Rigidbody2D>().velocity = vel;
        }
    }

    // aniation event
    public void GainVelocity2D()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            return;
        }

        float displacementX = player.transform.position.x - gameObject.transform.position.x;
        float displacementY = player.transform.position.y - gameObject.transform.position.y;

        GetComponent<Rigidbody2D>().velocity = new Vector3(displacementX * attack3DashConstant, displacementY * attack3DashConstant, transform.position.z);
    }

    // animation event
    public void ClearVelocity()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    // animation event
    public void PlaySoundEffect()
    {
        // play attack sound effect
        audioData.clip = attackSoundEffect;
        audioData.Play(0);
    }

    public bool AnimationIsPlaying() // check if animation is playing
    {
        return this.animator.GetCurrentAnimatorStateInfo(0).IsName("OrcKingAttack1") ||
            this.animator.GetCurrentAnimatorStateInfo(0).IsName("OrcKingAttack2") ||
            this.animator.GetCurrentAnimatorStateInfo(0).IsName("OrcKingAttack3");
    }

    private void OnDrawGizmos()
    {
        // attack 1 radius 
        Gizmos.DrawWireSphere(transform.position, attack1Radius);

        // attack 2 range
        Gizmos.DrawWireCube(transform.position, new Vector3(attack2Range[0], attack2Range[1], 0));

        // attack 3 radius
        Gizmos.DrawWireSphere(transform.position, attack3Radius);
    }
}
