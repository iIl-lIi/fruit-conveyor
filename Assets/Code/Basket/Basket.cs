using UnityEngine;

public class Basket : MonoBehaviour
{
    [SerializeField] private Transform _SlotsParent;
    [SerializeField] private BasketSlotsList _SlotsList;

    [SerializeField] private Transform _TextParent;
    [SerializeField] private BasketText _TextPrefab;

    private BasketSlots _currentSlots;
    private int _slotIndex;

    public void InitializeWithGrabbingTask(GrabbingTask task)
    {
        foreach (var slots in _SlotsList.List)
        {
            if (slots.Id != task.taskId) continue;
            if (_currentSlots != null) Destroy(_currentSlots.gameObject);
            _slotIndex = 0;
            _currentSlots = Instantiate(slots, _SlotsParent);
            return;
        }
    }
    public void PutConveyorBeltObject(ConveyorBeltObject obj)
    {
        var invalidId = _currentSlots.Id != obj.Id;
        var isFull = _slotIndex == _currentSlots.Slots.Length;
        if (invalidId || isFull) return;
        obj.SetEnabledCollider(false);
        PutInside(obj.transform);
    }

    private void PutInside(Transform t)
    {
        t.parent = _currentSlots.Slots[_slotIndex++];
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        SpawnText();
    }

    private void SpawnText()
    {
        var offset = Random.insideUnitCircle * 0.15f;
        var text = Instantiate(_TextPrefab, _TextParent);
        text.transform.localPosition = new Vector3(offset.x, 0, offset.y);
        text.transform.localRotation = Quaternion.Euler(0, 180, 0);
    }
}