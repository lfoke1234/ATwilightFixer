using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VCPlayerFollow_Controller : MonoBehaviour
{
    private void Start()
    {
        var vcam = GetComponent<CinemachineVirtualCamera>();

        if (PlayerManager.instance != null)
        {
            vcam.LookAt = PlayerManager.instance.player.transform;
            vcam.Follow = PlayerManager.instance.player.transform;
        }
    }

}
