using UnityEngine;

public class ConveyorSpawner : MonoBehaviour
{
    [SerializeField] private Transform _SpawnPoint;
    [SerializeField] private ConveyorBelt _ConveyorBelt;
    [SerializeField] private ConveyorBeltObjectsList _PrefabsList;

    private float _timer;

    private void SpawnObject(ConveyorBeltObject obj)
    {
        var position = _SpawnPoint.position + obj.SpawnOffset;
        var rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
        var instance = Instantiate(obj, position, rotation, _SpawnPoint);
        _ConveyorBelt.PutObject(instance);
        _timer = instance.WaitAfterSpawn / _ConveyorBelt.Speed;
    }

    private void Update() 
    {
        if(_timer > 0)
        {
            _timer -= Time.deltaTime;
            return;
        }

        SpawnObject(_PrefabsList.List[Random.Range(0, _PrefabsList.List.Count)]);
    }
}