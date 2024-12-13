using System.Collections.Generic;
using UnityEngine;

// SerializableDictionary Ŭ������ Dictionary�� ��ӹ�����, ���� �����͸� �����ϰ� �ҷ����� ���� ����ȭ�� ����
// TKey�� TValue Ÿ���� ����Ͽ� Ű�� ���� �����ϰ� ���
[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>(); // ����ȭ�� Ű ����Ʈ
    [SerializeField] private List<TValue> values = new List<TValue>(); // ����ȭ�� �� ����Ʈ

    // �����͸� ����ȭ�ϱ� ���� ȣ��Ǵ� �޼���. Dictionary�� �����͸� ����Ʈ ���·� ��ȯ�Ͽ� ����
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // �����͸� ������ȭ�� �� ȣ��Ǵ� �޼���. ����Ʈ�� �����͸� Dictionary�� �ٽ� �߰�
    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
        {
            Debug.Log("Keys count is not equal to values count");
        }

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}

