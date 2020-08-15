using UnityEngine;

namespace Assets.InternalAssets.Scripts.UnityLogic.BehaviourInterface
{
    public interface IGridSpawnBeaviour
    {
        Vector3 SpawnPosition { get; }
        float ColOffset { get; }
        float RowOffset { get; }
    }
}
