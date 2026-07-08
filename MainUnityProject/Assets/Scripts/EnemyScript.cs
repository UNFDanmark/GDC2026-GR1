using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public AudioSource audioSourceDeath;
    GameObject player;
    NavMeshAgent agent;
    Rigidbody rb;


    private float waitTime = 3.0f;
    private float timer = 0.0f;
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>().gameObject;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        SettingsManager.SettingsChangedEvent.AddListener(ReloadSettings);
        ReloadSettings();
    }

    void OnDisable()
    {
        SettingsManager.SettingsChangedEvent.RemoveListener(ReloadSettings);
    }

    void ReloadSettings()
    {
        audioSourceDeath.volume = SettingsManager.SoundVolume;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > waitTime)
        {
            
            

            if(agent.isActiveAndEnabled)
                agent.SetDestination(player.transform.position);

            timer = 0;
        }
        
      
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
            audioSourceDeath.Play();
            Destroy(gameObject, 2f); // 2f er standin for animation.
        }
    }
}
