using UnityEngine;

namespace Code.StaticData.CountdownTimer
{
    [CreateAssetMenu( menuName = "Static Data/Countdown Timer Data", order = 0)]
    public class CountdownTimerStaticData : ScriptableObject
    {
        [field: SerializeField] public int Seconds { get; private set; } = 60;
    }
}