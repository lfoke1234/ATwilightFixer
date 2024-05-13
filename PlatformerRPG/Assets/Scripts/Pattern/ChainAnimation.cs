using UnityEngine;

public class ChainAnimation : MonoBehaviour
{
    [SerializeField] private GameObject chain;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private float speed;


    private void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        if (particle != null)
        {
            particle.Play();
        }
    }

    public void SetDamage(int value)
    {
        chain.GetComponent<ChainTrigger>().SetChainDamage(value);
    }

    

    private void OnChainMovementComplete()
    {
    }
}
