using System.Collections.Generic;
using Code.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementsList", menuName = "UI/Elements List", order = 0)]
public class UIWindowsElementsList : ScriptableObject
{
    [field: SerializeField] public List<UIWindowElement> List { get; private set; }
}