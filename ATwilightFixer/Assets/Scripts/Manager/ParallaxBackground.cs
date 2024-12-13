using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float parallaxEffect;
    [SerializeField] private List<Transform> backgrounds;

    private float viewZoneX;
    private int leftIndex;
    private int rightIndex;
    private float backgroundWidth;
    private float lastCameraX;

    private void Start()
    {
        cam = GameObject.Find("Main Camera");

        backgroundWidth = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
        viewZoneX = backgroundWidth / 2;

        leftIndex = 0;
        rightIndex = backgrounds.Count - 1;

        lastCameraX = cam.transform.position.x;
    }

    private void Update()
    {
        float deltaX = cam.transform.position.x - lastCameraX; // 카메라 이동 거리 계산
        lastCameraX = cam.transform.position.x; // 현재 카메라 위치로 업데이트

        // 각 배경에 대해 효과 적용
        for (int i = 0; i < backgrounds.Count; i++)
        {
            float parallaxX = deltaX * parallaxEffect; // 카메라 이동에 따른 배경 이동 거리 계산
            backgrounds[i].position = new Vector3(backgrounds[i].position.x + parallaxX, cam.transform.position.y, 
                                                  backgrounds[i].position.z);
        }

        // 카메라가 오른쪽 끝 배경의 스크롤 범위를 넘어갔을 경우 오른쪽으로 스크롤
        if (cam.transform.position.x > backgrounds[rightIndex].position.x - viewZoneX)
        {
            ScrollRight();
        }
        // 카메라가 왼쪽 끝 배경의 스크롤 범위를 넘어갔을 경우 왼쪽으로 스크롤
        else if (cam.transform.position.x < backgrounds[leftIndex].position.x + viewZoneX)
        {
            ScrollLeft();
        }
    }

    // 오른쪽 끝 배경을 왼쪽 끝 뒤로 이동시키는 메서드
    private void ScrollRight()
    {
        backgrounds[leftIndex].position = new Vector3(backgrounds[rightIndex].position.x + backgroundWidth, 
                                                      cam.transform.position.y, backgrounds[leftIndex].position.z);

        // 인덱스 설정
        rightIndex = leftIndex; 
        leftIndex++;
        if (leftIndex >= backgrounds.Count)
        {
            leftIndex = 0;
        }
    }

    // 왼쪽 끝 배경을 오른쪽 끝 앞으로 이동시키는 메서드
    private void ScrollLeft()
    {
        backgrounds[rightIndex].position = new Vector3(backgrounds[leftIndex].position.x - backgroundWidth, cam.transform.position.y, 
                                                       backgrounds[rightIndex].position.z);

        // 인덱스 설정
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
        {
            rightIndex = backgrounds.Count - 1;
        }
    }
}
