using System.Collections.Generic;

namespace Assets.Scripts.UnityLogic.BehaviourInterface
{
    public interface ILevelEnemies
    {
        float Miny { get; }
        float Maxy { get; }

        int NumberOfLanes { get; }

        IEnemy GetEnemyInTheLane(int lane);
        List<int> SelectRandomLanesToSpawnEnemies();
        int GetARandomEnemyForLane(int lane);
        IEnemy GetEnemyOfType(int enemyType);
        float GetTime(int enemyType);
    }
}