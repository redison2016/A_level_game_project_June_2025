using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this script can apply on any character
[RequireComponent(typeof(AudioSource))]
public class HPManager : MonoBehaviour
{

    public int maxHP = 100;
    public int currentHP;
    public Animator animator;
    public float despondTimer = 5;
    private Image healthBar;
    int XPAmount;
    int goldAmount;
    public bool parrying = false;
    public AudioSource audioData;
    public AudioClip parrySoundEffect;
    public AudioClip hurtSoundEffect;
    public GameObject difficultyManager;
    private float difficulty;
    public Canvas deathScreen;

    // Start is called before the first frame update
    void Start()
    {
        // find difficulty manager
        difficultyManager = GameObject.Find("DifficultyManager");

        // get difficulty
        difficulty = difficultyManager.GetComponent<DifficultyScript>().GetDifficultyScale();

        // adjust max HP
        maxHP = (int)(maxHP * difficulty);

        // initialise health
        currentHP = maxHP;

        // find UI
        healthBar = gameObject.transform.Find("HealthBar").gameObject.transform.Find("Red").gameObject.GetComponent<Image>();

        // set XP and gold drop amount
        switch (GetComponent<Character>().characterName)
        {
            case "Orc":
                XPAmount = 2;
                goldAmount = 10;
                break;
            case "Slime":
                XPAmount = 2;
                goldAmount = 10;
                break;
            case "ArmoredOrc":
                XPAmount = 4;
                goldAmount = 20;
                break;
        }
    }

    private void Update()
    {
        // update health bar 
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHP / (float)maxHP;
        }
    }
    public void TakeDamage(int damage)
    {
        if (parrying)
        {
            // play parry sound
            audioData.clip = parrySoundEffect;
            audioData.Play();

            // dont take damage
            return;
        }

        if (currentHP <= 0) // if dead
        {
            return;
        }

        if (gameObject.tag == "Player") // if player gets hit
        {
            gameObject.GetComponent<PlayerKillCount>().IncrementHitCount();
        }

        currentHP -= damage; // deduct health

        // play hurt animation
        TurnRed();

        // play hurt sound effect
        audioData.clip = hurtSoundEffect;
        audioData.Play();

        if (currentHP <= 0) 
        {
            Die();
        }
    }

    public void TurnRed()
    {
        // make character red
        GetComponent<SpriteRenderer>().color = Color.red;

        // stun character


        // make character back to normal
        Invoke("TurnNormal", 0.1f);
    }

    void TurnNormal()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void Die()
    {
        // play die animaion
        animator.SetTrigger("die");

        // give player xp
        GameObject.FindGameObjectWithTag("Player").GetComponent<XPManager>().GainXP(XPAmount);

        // give player gold
        GameObject.FindGameObjectWithTag("Player").GetComponent<GoldManager>().GainGold(goldAmount);

        if (gameObject.tag != "Player")
        {
            // disable enemy health bar
            Canvas fullHealthBar = transform.Find("HealthBar").gameObject.GetComponent<Canvas>(); 
            if (fullHealthBar != null)
            {
                fullHealthBar.enabled = false;
            }

            // increment player kill count
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerKillCount>().Increment();

            // destroy object after a delay
            Invoke("Despond", despondTimer);
        }
        else // if player dies
        {
            // display death screen
            deathScreen.GetComponent<Canvas>().enabled = true;

            // decrease player skill value
            difficultyManager.GetComponent<DifficultyScript>().ChangeSkillLevel(false);

            // disable input
            GameObject.Find("InputManager").GetComponent<UserInput>().enabled = false;
        }

        // disable object
        gameObject.tag = "DeadObject"; // change tag
        transform.Find("Collider").GetComponent<Collider2D>().enabled = false; // disable collider
        GetComponent<Rigidbody2D>().gravityScale = 0; // clear gravity
    }

    public void Despond()
    {
        Destroy(gameObject);
    }
}
