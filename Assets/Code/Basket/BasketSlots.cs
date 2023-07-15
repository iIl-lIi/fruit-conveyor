using UnityEngine;

public class BasketSlots : MonoBehaviour
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public Transform[] Slots { get; private set; }
}