using Oversmith.Scripts.SavingSystem;
using UnityEngine;

namespace Oversmith.Scripts.BaseClasses
{
    public class DescriptionBaseSO : SerializableScriptableObject
    {
        [TextArea] public string description;
    }
}