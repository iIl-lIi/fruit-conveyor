using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ConveyorBeltObject : MonoBehaviour
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public Vector3 SpawnOffset { get; private set; }
    [field: SerializeField] public float WaitAfterSpawn { get; private set; } = 1f;

    public float Magnitude => (_startPosition - transform.position).magnitude;

    private Lazy<Collider> _colliderLazy;
    private Vector3 _startPosition;

    public void UpdateMovement(Vector3 velocity)
    {
        transform.position += velocity;
    }
    public void SetEnabledCollider(bool value)
    {
        if (!_colliderLazy.Value) return;
        _colliderLazy.Value.enabled = value;
    }

    private void Awake() 
    {
        _colliderLazy = new (() => GetComponent<MeshCollider>());
        _startPosition = transform.position;
    }
    private void OnMouseDown()
    {
        CharacterEvents.StartedGrabbingFruit.SafeInvoke(this);
    }
}