using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [field: SerializeField, Min(0.0001f)] public float Speed { get; private set; } = 1f; 
    [SerializeField] private Transform _StartPoint; 
    [SerializeField] private Transform _EndPoint; 

    public readonly List<ConveyorBeltObject> objects = new ();

    public void PutObject(ConveyorBeltObject obj) => objects.Add(obj);
    public void TakeObject(ConveyorBeltObject obj) => objects.Remove(obj);
    
    private void OnTouchedBeforeGrabbedFruit(ConveyorBeltObject obj)
    {
        if (!objects.Contains(obj)) return;
        objects.Remove(obj);
    }

    private void Awake() 
    {
        CharacterEvents.TouchedBeforeGrabbedFruit.Event += OnTouchedBeforeGrabbedFruit;
    }
    private void Update()
    {
        var direction = _EndPoint.position - _StartPoint.position;
        var magnitude = direction.magnitude;
        var velocity = direction.normalized * Speed * Time.deltaTime;

        for (int i = 0; i < objects.Count; i++)
        {
            var item = objects[i];
            if (item.Magnitude > magnitude)
            {
                Destroy(item.gameObject);
                objects.Remove(item);
                continue;
            }
            item.UpdateMovement(velocity);
        }
    }
    private void OnDestroy()
    {
        CharacterEvents.TouchedBeforeGrabbedFruit.Event -= OnTouchedBeforeGrabbedFruit;
    }
}