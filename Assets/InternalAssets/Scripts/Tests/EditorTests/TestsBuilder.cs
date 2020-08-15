using Assets.Scripts.Tests.EditorTests.EnemyAITests;
using Assets.Scripts.UnityLogic.ScriptableObjects;
using MovementTests;

namespace Assets.Scripts.Tests.EditorTests
{
    public class BuildAn
    {
        public static ActorBuilder Actor => new ActorBuilder();
        public static EnemyBuilder Enemy => new EnemyBuilder();
    }

    public class BuildA
    {
        public static ControllerScenario ControllerScenario => new ControllerScenario();

        public static LevelEnemiesBuilder LevelEnemies => new LevelEnemiesBuilder();
    }

    public class ValidateThat
    {
        public static EnemyContextValidator EnemySpawnContext(EnemySpawnContext enemySpawnContext)
        {
            return new EnemyContextValidator(enemySpawnContext);
        }
    }
}
