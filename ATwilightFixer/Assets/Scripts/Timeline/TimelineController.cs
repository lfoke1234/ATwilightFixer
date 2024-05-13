using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    public PlayableDirector director;

    private void OnEnable()
    {
        director.played += OnTimelinePlayed;
        director.stopped += OnTimelineStopped;
    }

    void Start()
    {
        UpdatePlayer();
    }

    private void OnDisable()
    {
        director.played -= OnTimelinePlayed;
        director.stopped -= OnTimelineStopped;
    }

    private void UpdatePlayer()
    {
        if (PlayerManager.instance != null && PlayerManager.instance.player != null)
        {
            Animator playerAnimator = PlayerManager.instance.player.GetComponentInChildren<Animator>();

            if (playerAnimator != null)
            {
                GameObject animatorObj = playerAnimator.gameObject;

                foreach (var playableAsset in director.playableAsset.outputs)
                {
                    if (playableAsset.streamName.Contains("Player Animation Track"))
                    {
                        director.SetGenericBinding(playableAsset.sourceObject, animatorObj);
                    }
                    else if (playableAsset.streamName.Contains("Player Transform Track"))
                    {
                        director.SetGenericBinding(playableAsset.sourceObject, animatorObj);
                    }
                }
            }
        }
    }

    private void OnTimelinePlayed(PlayableDirector obj)
    {
        GameManager.Instance.PlayCutScene();
    }

    private void OnTimelineStopped(PlayableDirector obj)
    {
        GameManager.Instance.StopCutScene();
    }

}