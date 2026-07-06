using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    Rigidbody rb;
    AudioSource audiosource;
    public AudioClip[] deathSounds;
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>().gameObject;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
        SettingsManager.SettingsChangedEvent.AddListener(ReloadSettings);
        ReloadSettings();
    }

    void OnDisable()
    {
        SettingsManager.SettingsChangedEvent.RemoveListener(ReloadSettings);
    }

    void ReloadSettings()
    {
        audiosource.volume = SettingsManager.SoundVolume;
    }

    void Update()
    {
        if(agent.isActiveAndEnabled)
            agent.SetDestination(player.transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHit"))
        {
            agent.enabled = false;
            // <insert animation here>
            GetComponent<Collider>().enabled = false;
            Vector3 cool = (transform.position - player.transform.position).normalized * 80;// + new Vector3(0, 30, 0);
            cool.y = 30;
            rb.linearVelocity = cool;
            // </insert animation here>
            SoundPlayer.PlayRandomSound(audiosource, deathSounds);
            Destroy(gameObject, 2f); // 2f er standin for animation.
        }
    }
}
