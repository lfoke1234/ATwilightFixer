using RPG.VisualNovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.VisualNovel
{
    public class GameController : MonoBehaviour
    {
        public GameScene currentScene;
        public BottomBarController bottomBar;
        public SpriteSwitcher backgroundController;
        public ChooseController chooseController;
        public AudioController audioController;

        private bool nextSceneisTitle;
        private string nextSceneName;

        private State state = State.IDLE;

        private enum State
        {
            IDLE, ANIMATE, CHOOSE
        }

        // 게임 시작 시 초기화 작업을 수행
        // 다음 씬 세팅과 비주얼 노벨 시작
        void Start()
        {
            if (NovelScriptManager.Instance.nextPlayScene != null)
            {
                nextSceneName = NovelScriptManager.Instance.nextSceneName;
                nextSceneisTitle = NovelScriptManager.Instance.nextSceneisTitle;
                currentScene = NovelScriptManager.Instance.nextPlayScene;
                StoryScene storyScene = currentScene as StoryScene;
                bottomBar.PlayScene(storyScene);
                backgroundController.SetImage(storyScene.background);
                PlayAudio(storyScene.sentences[0]);
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                bottomBar.CompleteCurrentSentence();

                if (state == State.IDLE && bottomBar.IsCompleted())
                {
                    if (bottomBar.IsLastScentence())
                    {
                        PlayScene((currentScene as StoryScene).nextScene);
                    }
                    else
                    {
                        bottomBar.PlayNextSentence();
                        PlayAudio((currentScene as StoryScene)
                                   .sentences[bottomBar.GetSentenceIndex()]);
                    }
                }
            }
        }

        // 다음씬 재생
        public void PlayScene(GameScene scene)
        {
            StartCoroutine(SwitchScene(scene));
        }

        // 장면 전환을 처리하는 코루틴
        private IEnumerator SwitchScene(GameScene scene)
        {
            state = State.ANIMATE;
            currentScene = scene;
            bottomBar.Hide();
            yield return new WaitForSeconds(1f);
            if (scene is StoryScene)
            {
                StoryScene storyScene = scene as StoryScene;
                backgroundController.SwitchImage(storyScene.background);
                PlayAudio(storyScene.sentences[0]);
                yield return new WaitForSeconds(1f);
                bottomBar.ClearText();
                bottomBar.Show();
                yield return new WaitForSeconds(1f);
                bottomBar.PlayScene(storyScene);
                state = State.IDLE;
            }
            else if (scene is ChooseScene)
            {
                state = State.CHOOSE;
                chooseController.SetupChoose(scene as ChooseScene);
            }
            else if (scene is null)
            {
                if (nextSceneisTitle)
                {
                    SceneManager.LoadScene("MainMenu");
                }
                else
                {
                    SceneManager.LoadScene(nextSceneName);
                }
                //int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                //SceneManager.LoadScene(currentSceneIndex + 1);
            }
        }

        // 현재 문장의 배경음악 및 사운드를 재생
        private void PlayAudio(StoryScene.Sentence sentence)
        {
            audioController.PlayAudio(sentence.music, sentence.sound);
        }
    }
}