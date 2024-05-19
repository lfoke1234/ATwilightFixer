using Cinemachine;
using UnityEngine;

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
