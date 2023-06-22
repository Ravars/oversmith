using MadSmith.Scripts.SavingSystem;
using UnityEngine;

namespace MadSmith.Scripts.BaseClasses
{
    public class DescriptionBaseSO : SerializableScriptableObject
    {
        [TextArea] public string description;
    }
}