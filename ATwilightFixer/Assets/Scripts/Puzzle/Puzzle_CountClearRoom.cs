using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Puzzle_CountClearRoom : MonoBehaviour
{
    private BoxCollider2D col;
    private int clearCount;
    [SerializeField] private int clearCondition;

    [Header("Spawn Info")]
    [SerializeField] private Transform spawnPos1;
    [SerializeField] private Transform doorPos1;
    [SerializeField] private Vector2 colOffset1;
    [SerializeField] private Transform spawnPos2;
    [SerializeField] private Transform doorPos2;
    [SerializeField] private Vector2 colOffset2;

    [SerializeField] private float spawnDis1;
    [SerializeField] private float spawnDis2;

    [SerializeField] private List<GameObject> spawnList1;
    [SerializeField] private List<GameObject> spawnList2;
    private List<GameObject> currentMontor = new List<GameObject>();


    [Header("Object")]
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject clearDoor;
    [SerializeField] private GameObject guideObj1;
    [SerializeField] private GameObject guideObj2;
    [SerializeField] TextMeshPro guideText;
    private bool isActive;

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // ���� ���� Ȱ��ȭ�Ǿ���, ��� ���Ͱ� ������� ���
        if (isActive && currentMontor.TrueForAll(monster => monster == null))
        {
            Vector2 player = PlayerManager.instance.player.transform.position;

            clearCount++;
            door.SetActive(false);

            if (clearCondition > clearCount) // Ŭ���� ������ ���� �������� ���� ���
            {
                if (clearCount % 2 == 0)
                {
                    UpdateGuideText("���������� �̵�", new Vector2(player.x, player.y + 3f));
                    col.offset = colOffset1;
                }
                else
                {
                    UpdateGuideText("�������� �̵�", new Vector2(player.x, player.y + 3f));
                    col.offset = colOffset2;
                }
                isActive = false;
            }
            else if (clearCount >= clearCondition) // Ŭ���� ���� ���� �� ���� �Ϸ� ó��
            {
                UpdateGuideText("���� ���Ƚ��ϴ�.", new Vector2(player.x, player.y + 3f));
                clearDoor.SetActive(false);
                col.enabled = false;
            }

        }
    }

    // �÷��̾ �濡 �������� �� ���� ��ȯ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && isActive == false)
        {
            if (clearCount % 2 == 0)
            {
                SpawnMonstor1();
            }
            else
            {
                SpawnMonstor2();
            }
        }
    }

    // �ȳ� �ؽ�Ʈ ������Ʈ
    void UpdateGuideText(string text, Vector2 position)
    {
        guideText.text = text;
        guideText.transform.position = position;
        guideText.gameObject.SetActive(true);
    }

    // ���� �׷� 1 ��ȯ
    private void SpawnMonstor1()
    {
        if (guideObj2.activeSelf)
            guideObj2.SetActive(false);

        door.transform.position = doorPos1.position;
        door.SetActive(true);

        float spawnStartX = spawnPos1.position.x;
        float spaceDistance = (spawnList1.Count > 1) ? spawnDis1 / (spawnList1.Count - 1) : 0;

        for (int i = 0; i < spawnList1.Count; i++)
        {
            float posX = spawnStartX + spaceDistance * i;

            Vector2 spawnPos = new Vector2(posX, spawnPos1.position.y);
            GameObject monstor = Instantiate(spawnList1[i], spawnPos, Quaternion.identity);
            currentMontor.Add(monstor);
        }

        isActive = true;
    }

    // ���� �׷� 2 ��ȯ
    private void SpawnMonstor2()
    {
        if (guideObj1.activeSelf)
            guideObj1.SetActive(false);

        door.transform.position = doorPos2.position;
        door.SetActive(true);

        float spawnStartX = spawnPos2.position.x;
        float spaceDistance = (spawnList2.Count > 1) ? spawnDis2 / (spawnList2.Count - 1) : 0;

        for (int i = 0; i < spawnList2.Count; i++)
        {
            float posX = spawnStartX + spaceDistance * i;

            Vector2 spawnPos = new Vector2(posX, spawnPos2.position.y);
            GameObject monstor = Instantiate(spawnList2[i], spawnPos, Quaternion.identity);
            currentMontor.Add(monstor);
        }

        isActive = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(spawnPos1.position, new Vector2(spawnPos1.position.x + spawnDis1, spawnPos1.position.y));

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(spawnPos2.position, new Vector2(spawnPos2.position.x + spawnDis2, spawnPos2.position.y));
    }
}
