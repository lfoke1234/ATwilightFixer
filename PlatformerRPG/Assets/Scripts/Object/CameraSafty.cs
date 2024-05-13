using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraSafty : MonoBehaviour
{
    public static CameraSafty instance;
    private Transform player;

    public GameObject mainCamera;
    public GameObject vCamPlayerFollow;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

}