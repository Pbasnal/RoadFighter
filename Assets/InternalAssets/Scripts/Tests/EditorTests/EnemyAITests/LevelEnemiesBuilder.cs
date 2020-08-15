using Assets.Scripts.UnityLogic.BehaviourInterface;
using NSubstitute;
using System.Linq;

namespace Assets.Scripts.Tests.EditorTests.EnemyAITests
{
    public class LevelEnemiesBuilder
    {
        private ILevelEnemies levelEnemies;

        public LevelEnemiesBuilder()
        {
            levelEnemies = Substitute.For<ILevelEnemies>();
        }

        public LevelEnemiesBuilder WithLanes(int numberOfLanes)
        {
            levelEnemies.NumberOfLanes.Returns(numberOfLanes);
            return this;
        }

        public LevelEnemiesBuilder SetEnemyForType(int enemyType, IEnemy enemy)
        {
            levelEnemies.GetEnemyOfType(enemyType).Returns(enemy);
            return this;
        }

        public LevelEnemiesBuilder WithCollisionBoundary(float min, float max)
        {
            levelEnemies.Miny.Returns(min);
            levelEnemies.Maxy.Returns(max);

            return this;
        }

        public LevelEnemiesBuilder ForSelectedLanes(params int[] lanes)
        {
            levelEnemies.SelectRandomLanesToSpawnEnemies().Returns(lanes.ToList());

            return this;
        }

        public LevelEnemiesBuilder SetTimeForEnemy(int enemyType, int time)
        {
            levelEnemies.GetTime(enemyType).Returns(time);

            return this;
        }

        public LevelEnemiesBuilder GeneratesEnemyForLane(int lane, int enemy)
        {
            levelEnemies.GetARandomEnemyForLane(lane).Returns(enemy);
            return this;
        }

        public LevelEnemiesBuilder HasEnemyInLane(int lane, IEnemy enemy)
        {
            levelEnemies.GetEnemyInTheLane(lane).Returns(enemy);

            return this;
        }

        public ILevelEnemies Build()
        {
            return levelEnemies;
        }
    }
}
