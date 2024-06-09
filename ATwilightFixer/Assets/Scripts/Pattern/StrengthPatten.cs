using Unity.VisualScripting;
using UnityEngine;

public class StrengthPatten : MonoBehaviour
{
    public int deadCount;

    [SerializeField] private GameObject[] thunderSpawner;
    [SerializeField] private GameObject chainSpawner;
    [SerializeField] private GameObject bulletSpawner;
    [SerializeField] private GameObject potal;
 
    [SerializeField] private bool isActive;


    private void Start()
    {
    }

    private void Update()
    {
        BuffPatten();
    }

    private void BuffPatten()
    {
        if (deadCount == 1)
        {
            foreach (GameObject thunder in thunderSpawner)
            {
                thunder.SetActive(true);
            }
        }
        else if (deadCount == 2)
        {
            bulletSpawner.SetActive(true);
        }
        else if (deadCount == 3)
        {
            chainSpawner.SetActive(true);
        }
        else if (deadCount == 4)
        {
            Strength();
        }
        else if (deadCount >= 6)
        {
            foreach (GameObject thunder in thunderSpawner)
            {
                thunder.SetActive(false);
            }
            bulletSpawner.SetActive(false);
            chainSpawner.SetActive(false);
            potal.SetActive(true);
        }
    }

    private void Strength()
    {
        foreach (GameObject thunder in thunderSpawner)
        {
            thunder.GetComponent<LightningSpawner>().isStrength = true;
        }
        chainSpawner.GetComponent<ChainSpawner>().isStr = true;
        bulletSpawner.GetComponent<BulletPattern>().isStr = true;
    }

}
