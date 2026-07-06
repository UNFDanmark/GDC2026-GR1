using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerScript : MonoBehaviour
{
    public GameObject thingToSpawn;
    public float cooldown;
    float cooldownTime = 0;
    public bool spawnInitial;
    public float range;
    GameObject player;
    void Start()
    {
        if (spawnInitial) cooldownTime = cooldown;
        player = FindAnyObjectByType<PlayerController>().gameObject;
    }

    void Update()
    {
        if (cooldownTime > cooldown)
        {
            Vector3 spawn = transform.position + new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
            Instantiate(thingToSpawn, spawn, player.transform.rotation * Quaternion.AngleAxis(180f,Vector3.up));
            cooldownTime -= cooldown;
        }
        cooldownTime += Time.deltaTime;
    }
}