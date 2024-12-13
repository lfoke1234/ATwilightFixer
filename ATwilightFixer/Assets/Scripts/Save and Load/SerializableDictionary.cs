using System.Collections.Generic;
using UnityEngine;

// SerializableDictionary 클래스는 Dictionary를 상속받으며, 게임 데이터를 저장하고 불러오기 위해 직렬화가 가능
// TKey와 TValue 타입을 사용하여 키와 값을 유연하게 사용
[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>(); // 직렬화된 키 리스트
    [SerializeField] private List<TValue> values = new List<TValue>(); // 직렬화된 값 리스트

    // 데이터를 직렬화하기 전에 호출되는 메서드. Dictionary의 데이터를 리스트 형태로 변환하여 저장
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

    // 데이터를 역직렬화한 후 호출되는 메서드. 리스트의 데이터를 Dictionary로 다시 추가
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

