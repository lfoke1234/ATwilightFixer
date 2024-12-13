using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using RPG.VisualNovel;

namespace RPG.VisualNovel
{
    public class ChooseLabelController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Color defaultColor;
        public Color hoverColor;
        private StoryScene scene;
        private TextMeshProUGUI textMesh;
        private ChooseController controller;

        void Awake()
        {
            textMesh = GetComponent<TextMeshProUGUI>();
            textMesh.color = defaultColor;
        }

        // 레이블의 높이를 반환하며 선태지의 위치 계산
        public float GetHeight()
        {
            return textMesh.rectTransform.sizeDelta.y * textMesh.rectTransform.localScale.y;
        }

        // 선택지의 텍스트, 다음 진행될 장면 및 위치를 설정
        public void Setup(ChooseScene.ChooseLabel label, ChooseController controller, float y)
        {
            scene = label.nextScene;
            textMesh.text = label.text;
            this.controller = controller;

            // 레이블의 위치를 설정.
            Vector3 position = textMesh.rectTransform.localPosition;
            position.y = y;
            textMesh.rectTransform.localPosition = position;
        }

        // 선택한 선택지의 다음 장면으로 진행
        public void OnPointerClick(PointerEventData eventData)
        {
            controller.PerformChoose(scene); 
        }

        // 마우스가 선택지 위에 올라갔을 때 호출
        public void OnPointerEnter(PointerEventData eventData)
        {
            textMesh.color = hoverColor;
        }

        // 마우스가 선택지에서 벗어났을 때 호출
        public void OnPointerExit(PointerEventData eventData)
        {
            textMesh.color = defaultColor;
        }
    }
}
