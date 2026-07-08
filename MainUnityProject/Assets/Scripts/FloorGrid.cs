using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class FloorGrid : MonoBehaviour
{
    public GameObject gridder;
    public int x, y, z;
    public float xDist, yDist, zDist;
    public Material[] mats;
    void Start()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                for (int h = 0; h < z; h++)
                {
                    if (i == 0 && j == 0 && h == 0) continue;
                    Instantiate(gridder, transform.position + new Vector3(i * xDist, j * yDist, h * zDist),
                        gridder.transform.rotation);
                    gridder.GetComponent<Renderer>().material = mats[Random.Range(0, mats.Length)];
                }
            }
        }
    }
}
