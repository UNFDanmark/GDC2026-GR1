using TMPro;
using UnityEngine;

public class specialautisticdoor : MonoBehaviour
{
    bool open;
    Animator animator;
    public GameObject[] enemies;

    public TextMeshPro text;
  
    int counter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        counter = 0;
        if (open) return;
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
                counter++;
        }

        if (counter != 0) return;
        open = true;
        animator.SetTrigger("open");


    }
}