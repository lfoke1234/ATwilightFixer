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
        public TextMeshProUGUI barText;
        public TextMeshProUGUI personNameText;

        private int sentenceIndex = 0;
        public StoryScene currentScene;
        private State state = State.Completed;
        private Animator animator;
        private bool isHidden = false;
        private bool skipToFullText = false;

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

        private void Start()
        {
        }

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

        public void PlayScene(StoryScene scene)
        {
            currentScene = scene;
            sentenceIndex = -1;
            PlayNextSentence();
        }

        public void PlayNextSentence()
        {
            StartCoroutine(TypeText(currentScene.sentences[++sentenceIndex].text));
            personNameText.text = currentScene.sentences[sentenceIndex].speaker.speakerName;
            personNameText.color = currentScene.sentences[sentenceIndex].speaker.textColor;
            ActSpeakers();
        }

        public bool IsLastScentence()
        {
            return sentenceIndex + 1 == currentScene.sentences.Count;
        }

        public bool IsCompleted()
        {
            return state == State.Completed;
        }

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



        private void ActSpeakers()
        {
            List<StoryScene.Sentence.Action> actions = currentScene.sentences[sentenceIndex].actions;

            for (int i = 0; i < actions.Count; i++)
            {
                ActSpeaker(actions[i]);
            }
        }

        public void SkipCurrentScene()
        {
            if (currentScene != null && currentScene.nextScene != null)
            {
                FindObjectOfType<GameController>().PlayScene(currentScene.nextScene);
            }
            else
            {
                Debug.LogWarning("다음 씬으로 이동할 수 없습니다.");
            }
        }


        private void ActSpeaker(StoryScene.Sentence.Action action)
        {
            SpriteController controller = null;
            switch (action.actionType)
            {
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
                case StoryScene.Sentence.Action.Type.DisappearFast:
                    if (sprites.ContainsKey(action.speaker))
                    {
                        controller = sprites[action.speaker];
                        controller.HideInstantly();
                    }
                    break;
                case StoryScene.Sentence.Action.Type.Move:
                    if (sprites.ContainsKey(action.speaker))
                    {
                        controller = sprites[action.speaker];
                        controller.Move(action.coords, action.moveSpeed);
                    }
                    break;
                case StoryScene.Sentence.Action.Type.Disappear:
                    if (sprites.ContainsKey(action.speaker))
                    {
                        controller = sprites[action.speaker];
                        controller.Hide();
                    }
                    break;
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
