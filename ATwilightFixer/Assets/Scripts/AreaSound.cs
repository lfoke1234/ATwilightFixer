using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [SerializeField] private int areaSoundIndex;
    private bool isPlaying;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && !isPlaying)
        {
            //AudioManager.instance.PlaySFX(areaSoundIndex, null);
            isPlaying = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && isPlaying)
        {
            AudioManager.instance.StopSFXWithTime(areaSoundIndex);
            isPlaying = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && !isPlaying)
        {
            //AudioManager.instance.PlaySFX(areaSoundIndex, null);
            isPlaying = true;
        }
    }
}
