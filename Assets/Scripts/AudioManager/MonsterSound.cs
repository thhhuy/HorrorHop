using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSound : MonoBehaviour
{
    public AudioClip monsterScream;
    public AudioClip DeathScream;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Phương thức sẽ được gọi từ sự kiện animation
    public void PlayMonsterScream()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(monsterScream);
        }
    }
    public void PlayMonsterDeathScream()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(DeathScream);
        }
    }

}
