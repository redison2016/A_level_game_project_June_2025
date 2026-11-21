using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{

    [SerializeField] AudioSource audioData;
    public AudioClip mainAreaMusic;
    public AudioClip generalAreaMusic;
    public AudioClip bossFightMusic;
    public GameObject mainManager;
    public GameObject volumeSlideBar;

    private void Start()
    {
        // get main manager
        mainManager = GameObject.Find("MainManager");

        // initialise volume
        float volume = mainManager.GetComponent<Player>().volume;
        ChangeVolume(volume);

        // change UI
        volumeSlideBar.GetComponent<Slider>().value = volume;
    }

    public void ChangeMusic(string area)
    {
        AudioClip previousAudio = audioData.clip;

        // determine new music
        switch (area)
        {
            case ("MainArea"):
                audioData.clip = mainAreaMusic;
                break;
            case ("Junction"):
                audioData.clip = mainAreaMusic;
                break;
            case ("Area1"):
                audioData.clip = mainAreaMusic;
                break;
            case ("CastleArea"):
                audioData.clip = generalAreaMusic;
                break;
            case ("CastleInDoor1"):
                audioData.clip = generalAreaMusic;
                break;
            case ("CastleInDoor2"):
                audioData.clip = generalAreaMusic;
                break;
        }

        // check if new audio is the same as the previous audio
        if (audioData.clip != previousAudio) // if different then
        {
            // stop current music
            audioData.Stop();

            // play new music
            audioData.Play(0);
        }
    }

    public void ChangeVolume(float volume)
    {
        audioData.volume = volume;
    }
}
