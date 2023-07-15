using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConveyorBeltObjectsList", menuName = "Conveyor Belt/Objects List", order = 0)]
public class ConveyorBeltObjectsList : ScriptableObject
{
    [field: SerializeField] public List<ConveyorBeltObject> List { get; private set; }
}