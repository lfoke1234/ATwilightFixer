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
        float deltaX = cam.transform.position.x - lastCameraX; // ī�޶� �̵� �Ÿ� ���
        lastCameraX = cam.transform.position.x; // ���� ī�޶� ��ġ�� ������Ʈ

        // �� ��濡 ���� ȿ�� ����
        for (int i = 0; i < backgrounds.Count; i++)
        {
            float parallaxX = deltaX * parallaxEffect; // ī�޶� �̵��� ���� ��� �̵� �Ÿ� ���
            backgrounds[i].position = new Vector3(backgrounds[i].position.x + parallaxX, cam.transform.position.y, 
                                                  backgrounds[i].position.z);
        }

        // ī�޶� ������ �� ����� ��ũ�� ������ �Ѿ�� ��� ���������� ��ũ��
        if (cam.transform.position.x > backgrounds[rightIndex].position.x - viewZoneX)
        {
            ScrollRight();
        }
        // ī�޶� ���� �� ����� ��ũ�� ������ �Ѿ�� ��� �������� ��ũ��
        else if (cam.transform.position.x < backgrounds[leftIndex].position.x + viewZoneX)
        {
            ScrollLeft();
        }
    }

    // ������ �� ����� ���� �� �ڷ� �̵���Ű�� �޼���
    private void ScrollRight()
    {
        backgrounds[leftIndex].position = new Vector3(backgrounds[rightIndex].position.x + backgroundWidth, 
                                                      cam.transform.position.y, backgrounds[leftIndex].position.z);

        // �ε��� ����
        rightIndex = leftIndex; 
        leftIndex++;
        if (leftIndex >= backgrounds.Count)
        {
            leftIndex = 0;
        }
    }

    // ���� �� ����� ������ �� ������ �̵���Ű�� �޼���
    private void ScrollLeft()
    {
        backgrounds[rightIndex].position = new Vector3(backgrounds[leftIndex].position.x - backgroundWidth, cam.transform.position.y, 
                                                       backgrounds[rightIndex].position.z);

        // �ε��� ����
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
        {
            rightIndex = backgrounds.Count - 1;
        }
    }
}
