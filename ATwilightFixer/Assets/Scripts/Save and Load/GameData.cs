using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;
    public int level;

    public SerializableDictionary<string, bool> skillCanUnlock;
    public SerializableDictionary<string, bool> skillUnlocked;

    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentID;
    public SerializableDictionary<string, int> quickSlot;

    public SerializableDictionary<string, float> volumSettings;
    public SerializableDictionary<string, string> keyBindings;

    public SerializableDictionary<string, bool> clearStage;

    public GameData()
    {
        this.currency = 0;
        this.level = 0;

        skillCanUnlock = new SerializableDictionary<string, bool>();
        skillUnlocked = new SerializableDictionary<string, bool>();

        inventory = new SerializableDictionary<string, int>();
        equipmentID = new List<string>();
        quickSlot = new SerializableDictionary<string, int>();

        volumSettings = new SerializableDictionary<string, float>();
        keyBindings = new SerializableDictionary<string, string>();

        clearStage = new SerializableDictionary<string, bool>();

    }
}
