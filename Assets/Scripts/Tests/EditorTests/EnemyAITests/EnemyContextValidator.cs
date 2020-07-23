using Assets.Scripts.UnityLogic.BehaviourInterface;
using Assets.Scripts.UnityLogic.ScriptableObjects;
using NUnit.Framework;

namespace Assets.Scripts.Tests.EditorTests.EnemyAITests
{
    public class EnemyContextValidator
    {
        private EnemySpawnContext enemySpawnContext;

        public EnemyContextValidator(EnemySpawnContext enemySpawnContext)
        {
            this.enemySpawnContext = enemySpawnContext;
        }

        public EnemyContextValidator IsNotNull()
        {
            Assert.IsNotNull(enemySpawnContext);

            return this;
        }

        public EnemyContextValidator IsNotSet()
        {
            Assert.AreEqual(0, enemySpawnContext.lane);
            Assert.AreEqual(0, enemySpawnContext.enemyType);
            Assert.IsFalse(enemySpawnContext.spawnCar);

            return this;
        }

        public EnemyContextValidator IsSet(int lane, int enemyType, IEnemy enemy)
        {
            Assert.IsTrue(enemySpawnContext.spawnCar);

            Assert.AreEqual(lane, enemySpawnContext.lane);
            Assert.AreEqual(enemyType, enemySpawnContext.enemyType);
            Assert.AreEqual(enemy.MinYTime, enemySpawnContext.MinyTime);
            Assert.AreEqual(enemy.MaxYTime, enemySpawnContext.MaxyTime);

            return this;
        }
    }
}
