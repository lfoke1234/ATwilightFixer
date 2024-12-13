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

        // ���� ��� ����
        private int sentenceIndex = 0;
        public StoryScene currentScene;

        // ���� �ִϸ��̼� ����
        private Animator animator;
        private State state = State.Completed;
        private bool isHidden = false;

        private bool skipToFullText = false;

        // ĳ���� ��������Ʈ
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

        // ���� ������ �ε����� ��ȯ
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

        // ���ο� ���丮 ����� ���
        public void PlayScene(StoryScene scene)
        {
            currentScene = scene;
            sentenceIndex = -1;
            PlayNextSentence();
        }

        // ���� ������ ���
        public void PlayNextSentence()
        {
            StartCoroutine(TypeText(currentScene.sentences[++sentenceIndex].text));
            personNameText.text = currentScene.sentences[sentenceIndex].speaker.speakerName;
            personNameText.color = currentScene.sentences[sentenceIndex].speaker.textColor;
            ActSpeakers();
        }

        // ���� ������ ������ �������� Ȯ��
        public bool IsLastScentence()
        {
            return sentenceIndex + 1 == currentScene.sentences.Count;
        }

        // ���� ������ ����� �Ϸ�Ǿ����� Ȯ��
        public bool IsCompleted()
        {
            return state == State.Completed;
        }

        // ���� ������ ��� �Ϸ�
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

        // ������ �� ���ھ� ����ϴ� �ڷ�ƾ
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


        // ���忡 ���� �� ĳ������ �ൿ�� ����
        private void ActSpeakers()
        {
            List<StoryScene.Sentence.Action> actions = currentScene.sentences[sentenceIndex].actions;

            for (int i = 0; i < actions.Count; i++)
            {
                ActSpeaker(actions[i]);
            }
        }

        // ���� ����� ��ŵ�Ͽ� ���� ������� �̵�
        public void SkipCurrentScene()
        {
            if (currentScene != null && currentScene.nextScene != null)
            {
                FindObjectOfType<GameController>().PlayScene(currentScene.nextScene);
            }
        }

        // Ư�� ĳ������ �ൿ�� ����
        private void ActSpeaker(StoryScene.Sentence.Action action)
        {
            SpriteController controller = null;
            switch (action.actionType)
            {
                // ĳ���Ͱ� ��Ÿ������ ����
                // ���� �ش� ����Ŀ�� ��������Ʈ�� ���� �������� ������ ���� ����
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
                // ĳ���Ͱ� ������ ��Ÿ������ ����
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
                // ĳ���Ͱ� ������ ��������� ����
                case StoryScene.Sentence.Action.Type.DisappearFast:
                    if (sprites.ContainsKey(action.speaker))
                    {
                        controller = sprites[action.speaker];
                        controller.HideInstantly();
                    }
                    break;
                // ĳ���Ͱ� ������ ��ǥ�� �̵��ϵ��� ����
                case StoryScene.Sentence.Action.Type.Move:
                    if (sprites.ContainsKey(action.speaker))
                    {
                        controller = sprites[action.speaker];
                        controller.Move(action.coords, action.moveSpeed);
                    }
                    break;
                // ĳ���Ͱ� õõ�� ��������� ����
                case StoryScene.Sentence.Action.Type.Disappear:
                    if (sprites.ContainsKey(action.speaker))
                    {
                        controller = sprites[action.speaker];
                        controller.Hide();
                    }
                    break;
                // ĳ���Ͱ� �ƹ��� ���۵� �������� �ʵ��� ����
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
