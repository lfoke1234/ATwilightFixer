using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLightningTrigger : MonoBehaviour
{
    [SerializeField] private GameObject warningBox;
    [SerializeField] private GameObject lightnig;

    [SerializeField] private float warningTime = 0.8f;

    private void Start()
    {
        StartCoroutine(SetLightning());
        Destroy(gameObject, 3);
    }

    private IEnumerator SetLightning()
    {
        yield return new WaitForSeconds(warningTime);
        lightnig.SetActive(true);
    }
}
