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
        if (isActive)
            Strength();
    }

    private void Strength()
    {
        thunderSpawner.GetComponent<LightningSpawner>().isStrength = true;
        chainSpawner.GetComponent<ChainSpawner>().isStr = true;
        bulletSpawner.GetComponent<BulletPattern>().isStr = true;
    }

}
