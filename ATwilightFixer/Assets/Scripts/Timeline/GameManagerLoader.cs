using UnityEngine;

public class GameManagerLoader : MonoBehaviour
{
    public void StartCutScene()
    {
        GameManager.Instance.PlayCutScene();
    }
    public void EndCutScene()
    {
        GameManager.Instance.StopCutScene();
    }

}
