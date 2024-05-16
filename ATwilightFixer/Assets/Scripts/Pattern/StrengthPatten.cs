using Unity.VisualScripting;
using UnityEngine;

public class StrengthPatten : MonoBehaviour
{
    public int deadCount;

    [SerializeField] private GameObject thunderSpawner;
    [SerializeField] private GameObject chainSpawner;
    [SerializeField] private GameObject bulletSpawner;

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
            Debug.Log(deadCount);
            thunderSpawner.SetActive(true);
        }
        else if (deadCount == 2)
        {
            Debug.Log(deadCount);
            bulletSpawner.SetActive(true);
        }
        else if (deadCount == 3)
        {
            Debug.Log(deadCount);
            chainSpawner.SetActive(true);
        }
        else if (deadCount == 4)
        {
            Debug.Log(deadCount);
            Strength();
        }
    }

    private void Strength()
    {
        thunderSpawner.GetComponent<LightningSpawner>().isStrength = true;
        chainSpawner.GetComponent<ChainSpawner>().isStr = true;
        bulletSpawner.GetComponent<BulletPattern>().isStr = true;
    }

}
