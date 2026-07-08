using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public AudioSource audioSourceDeath;
    GameObject player;
    NavMeshAgent agent;
    Rigidbody rb;
    public bool Dasher, Normal, Ranged;
    bool hasSeenPlayer;

    private float waitTime = 3.0f;
    private float timer = 0.0f;


    [Header("Ranged")] 
    public GameObject projectile;
    
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
        if (!hasSeenPlayer)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Vector3 origin = transform.position + direction;
            RaycastHit sexo;
            Physics.Raycast(origin, direction, out sexo);
            if (sexo.collider.gameObject.CompareTag("Player")) hasSeenPlayer = true;
            else return;
        }
        if (Dasher)
        {
            timer += Time.deltaTime;
            if (timer < waitTime) return;
            timer = 0;
        } else if (Ranged)
        {
            timer += Time.deltaTime;
            if (timer < waitTime) return;

            Instantiate(projectile, transform.position + new Vector3(0, 1, 0), transform.rotation);
            
            
            timer = 0;
        }
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
           /* Vector3 cool = (transform.position - player.transform.position).normalized * 80;// + new Vector3(0, 30, 0);
            cool.y = 30;
            rb.linearVelocity = cool;
            */
            
            // </insert animation here>
            audioSourceDeath.Play();
            Destroy(gameObject, 2f); // 2f er standin for animation.
        }
    }
}
