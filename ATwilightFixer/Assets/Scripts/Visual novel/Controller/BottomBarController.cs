using RPG.VisualNovel;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.VisualNovel
{
    public class BottomBarController : MonoBehaviour
    {
        // UI
        public TextMeshProUGUI barText;
        public TextMeshProUGUI personNameText;

        // 현재 대사 관리
        private int sentenceIndex = 0;
        public StoryScene currentScene;

        // 현재 애니메이션 관리
        private Animator animator;
        private State state = State.Completed;
        private bool isHidden = false;

        private bool skipToFullText = false;

        // 캐릭터 스프라이트
        private Dictionary<Speaker, SpriteController> sprites;
        public GameObject spritesPrefab;
        private enum State
        {
            Playing,
            Completed,
        }

        private void Awake()
        {
            sprites = new Dictionary<Speaker, SpriteController>();
            animator = GetComponent<Animator>();
        }

        // 현재 문장의 인덱스를 반환
        public int GetSentenceIndex()
        {
            return sentenceIndex;
        }

        public void Show()
        {
            animator.SetTrigger("Show");
            isHidden = false;
        }

        public void Hide()
        {
            if (!isHidden)
            {
                animator.SetTrigger("Hide");
                isHidden = true;
            }
        }

        public void ClearText()
        {
            barText.text = "";
        }

        // 새로운 스토리 장면을 재생
        public void PlayScene(StoryScene scene)
        {
            currentScene = scene;
            sentenceIndex = -1;
            PlayNextSentence();
        }

        // 다음 문장을 재생
        public void PlayNextSentence()
        {
            StartCoroutine(TypeText(currentScene.sentences[++sentenceIndex].text));
            personNameText.text = currentScene.sentences[sentenceIndex].speaker.speakerName;
            personNameText.color = currentScene.sentences[sentenceIndex].speaker.textColor;
            ActSpeakers();
        }

        // 현재 문장이 마지막 문장인지 확인
        public bool IsLastScentence()
        {
            return sentenceIndex + 1 == currentScene.sentences.Count;
        }

        // 현재 문장의 출력이 완료되었는지 확인
        public bool IsCompleted()
        {
            return state == State.Completed;
        }

        // 현재 문장을 즉시 완료
        public void CompleteCurrentSentence()
        {
            if (state == State.Playing)
            {
                skipToFullText = true;
            }
            else if (state == State.Completed && !IsLastScentence())
            {
                PlayNextSentence();
            }
        }

        // 문장을 한 글자씩 출력하는 코루틴
        private IEnumerator TypeText(string text)
        {
            barText.text = "";
            state = State.Playing;
            skipToFullText = false;
            int charIndex = 0;

            while (charIndex < text.Length)
            {
                if (skipToFullText)
                {
                    barText.text = text.Replace("[n]", "\n");
                    break;
                }

                if (text.Substring(charIndex).StartsWith("[n]"))
                {
                    barText.text += "\n";
                    charIndex += 3;                
                }
                else
                {
                    barText.text += text[charIndex];
                    charIndex++;
                }

                yield return new WaitForSeconds(0.05f);
            }

            state = State.Completed;
        }


        // 문장에 따라 각 캐릭터의 행동을 실행
        private void ActSpeakers()
        {
            List<StoryScene.Sentence.Action> actions = currentScene.sentences[sentenceIndex].actions;

            for (int i = 0; i < actions.Count; i++)
            {
                ActSpeaker(actions[i]);
            }
        }

        // 현재 장면을 스킵하여 다음 장면으로 이동
        public void SkipCurrentScene()
        {
            if (currentScene != null && currentScene.nextScene != null)
            {
                FindObjectOfType<GameController>().PlayScene(currentScene.nextScene);
            }
        }

        // 특정 캐릭터의 행동을 실행
        private void ActSpeaker(StoryScene.Sentence.Action action)
        {
            SpriteController controller = null;
            switch (action.actionType)
            {
                // 캐릭터가 나타나도록 설정
                // 만약 해당 스피커의 스프라이트가 아직 존재하지 않으면 새로 생성
                case StoryScene.Sentence.Action.Type.Appear:
                    if (!sprites.ContainsKey(action.speaker))
                    {
                        controller = Instantiate(action.speaker.prefab.gameObject, spritesPrefab.transform)
                            .GetComponent<SpriteController>();
                        sprites.Add(action.speaker, controller);
                    }
                    else
                    {
                        controller = sprites[action.speaker];
                    }
                    controller.Setup(action.speaker.sprites[action.spriteIndex]);
                    controller.Show(action.coords);
                    return;
                // 캐릭터가 빠르게 나타나도록 설정
                case StoryScene.Sentence.Action.Type.AppearFast:
                    if (!sprites.ContainsKey(action.speaker))
                    {
                        controller = Instantiate(action.speaker.prefab.gameObject, spritesPrefab.transform)
                            .GetComponent<SpriteController>();
                        sprites.Add(action.speaker, controller);
                    }
                    else
                    {
                        controller = sprites[action.speaker];
                    }
                    controller.Setup(action.speaker.sprites[action.spriteIndex]);
                    controller.ShowInstantly(action.coords);
                    break;
                // 캐릭터가 빠르게 사라지도록 설정
                case StoryScene.Sentence.Action.Type.DisappearFast:
                    if (sprites.ContainsKey(action.speaker))
                    {
                        controller = sprites[action.speaker];
                        controller.HideInstantly();
                    }
                    break;
                // 캐릭터가 지정된 좌표로 이동하도록 설정
                case StoryScene.Sentence.Action.Type.Move:
                    if (sprites.ContainsKey(action.speaker))
                    {
                        controller = sprites[action.speaker];
                        controller.Move(action.coords, action.moveSpeed);
                    }
                    break;
                // 캐릭터가 천천히 사라지도록 설정
                case StoryScene.Sentence.Action.Type.Disappear:
                    if (sprites.ContainsKey(action.speaker))
                    {
                        controller = sprites[action.speaker];
                        controller.Hide();
                    }
                    break;
                // 캐릭터가 아무런 동작도 수행하지 않도록 설정
                case StoryScene.Sentence.Action.Type.None:
                    if (sprites.ContainsKey(action.speaker))
                    {
                        controller = sprites[action.speaker];
                    }
                    break;
            }
            if (controller != null)
            {
                controller.SwitchSprite(action.speaker.sprites[action.spriteIndex]);
            }
        }
    }
}
