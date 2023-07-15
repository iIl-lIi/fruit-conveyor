using System;
using UnityEngine;

public class AnimatorEventsReactor : MonoBehaviour
{
    public event Action<string> Reacted;

    private void CallTheReaction(string eventIndex)
    {
        Reacted?.Invoke(eventIndex);
    }
}