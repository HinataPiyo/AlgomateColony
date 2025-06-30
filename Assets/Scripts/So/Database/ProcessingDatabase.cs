using UnityEngine;

[CreateAssetMenu(fileName = "ProcessingDatabase", menuName = "Database/ProcessingDatabase")]
public class ProcessingDatabase : ScriptableObject
{
    [Header("加工品の一覧"), SerializeField]
    MaterialSO[] processingDatabase;
    public MaterialSO[] DB => processingDatabase;
}