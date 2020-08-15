using Assets.Scripts.UnityLogic.BehaviourInterface;
using NSubstitute;
using UnityEngine;

namespace Assets.Scripts.Tests.EditorTests.EnemyAITests
{
    public class EnemyBuilder
    {
        private IEnemy enemy;

        public EnemyBuilder()
        {
            enemy = Substitute.For<IEnemy>();
        }

        public EnemyBuilder WhoseNameIs(string name)
        {
            enemy.Name.Returns(name);

            return this;
        }

        public EnemyBuilder StartedAt(float startTime)
        {
            enemy.StartTime.Returns(startTime);

            return this;
        }

        public EnemyBuilder WithSpeed(float speed)
        {
            enemy.Speed.Returns(speed);

            return this;
        }

        public EnemyBuilder CalculateTimeFor(float minY, float maxy)
        {
            enemy.MinYTime = Mathf.Abs(minY) / enemy.Speed + enemy.StartTime;
            enemy.MaxYTime = Mathf.Abs(maxy) / enemy.Speed + enemy.StartTime;

            return this;
        }

        public IEnemy Build()
        {
            return enemy;
        }
    }
}
