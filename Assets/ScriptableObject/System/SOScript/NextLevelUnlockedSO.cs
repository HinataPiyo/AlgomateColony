using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NextLevelUnlockedSO", menuName = "NextLevelUnlockedSO", order = 0)]
public class NextLevelUnlockedSO : ScriptableObject
{
    public bool batteryFacility;
    [SerializeField] List<BASE_NEXT_UNLOCK> next_unlocks = new List<BASE_NEXT_UNLOCK>();

    public List<BASE_NEXT_UNLOCK> GetBaseNextUnlocks_List() { return next_unlocks; }
}

[System.Serializable]
public class BASE_NEXT_UNLOCK
{
    public Sprite icon;
    public string name_text;
    [TextArea(3, 10)]
    public string exp_text;
}