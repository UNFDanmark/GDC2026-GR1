using TMPro;
using UnityEngine;

public class dooropener : MonoBehaviour
{
    bool open;
    Animator animator;
    public GameObject[] enemies;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      
        
        if (open) return;
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
                return;
        }
        open = true;
        animator.SetTrigger("open");


    }
}
