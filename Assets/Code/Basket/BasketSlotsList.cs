using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasketSlotsList", menuName = "Basket/Slots List", order = 0)]
public class BasketSlotsList : ScriptableObject 
{
    [field: SerializeField] public List<BasketSlots> List { get; private set; }
}