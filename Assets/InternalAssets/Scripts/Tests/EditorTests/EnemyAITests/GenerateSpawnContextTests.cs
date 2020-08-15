using Assets.Scripts.Tests.EditorTests;
using Assets.Scripts.UnityLogic.ScriptableObjects;
using NSubstitute;
using NUnit.Framework;
using System.Linq;
using UnityEngine;

namespace EnemyAITests
{
    public class LevelEnemyAITests
    {
        public class PositiveBoundaries
        {
            private float minY = 10;
            private float maxY = 15;

            [Test]
            public void if_collision_detected_in_same_lane_dont_generate_enemy()
            {
                var e1 = BuildAn.Enemy.WhoseNameIs("lol1").WithSpeed(1).CalculateTimeFor(minY, maxY).Build();
                var e2 = BuildAn.Enemy.WhoseNameIs("lol2").WithSpeed(2).Build();

                var levelEnemies = BuildA.LevelEnemies.WithLanes(3)
                    .SetEnemyForType(Arg.Any<int>(), e2)
                    .WithCollisionBoundary(10, 15)
                    .ForSelectedLanes(0)
                    .SetTimeForEnemy(0, 4)
                    .GeneratesEnemyForLane(Arg.Any<int>(), 0)
                    .HasEnemyInLane(0, e1)
                    .HasEnemyInLane(1, null).HasEnemyInLane(2, null)
                    .Build();

                var ai = ScriptableObject.CreateInstance<LevelEnemyAI>();
                ai.SetLevelEnemies(levelEnemies);

                var contexts = ai.GenerateEnemySpawnContexts().ToList();

                contexts.ForEach(c =>
                    ValidateThat.EnemySpawnContext(c)
                        .IsNotNull()
                        .IsNotSet()
                );
            }

            [Test]
            public void if_no_collision_detected_in_same_lane_generate_enemy()
            {
                var e1 = BuildAn.Enemy.WhoseNameIs("lol1").WithSpeed(1).CalculateTimeFor(minY, maxY).Build();
                var e2 = BuildAn.Enemy.WhoseNameIs("lol2").StartedAt(6).WithSpeed(1).CalculateTimeFor(minY, maxY).Build();

                var levelEnemies = BuildA.LevelEnemies.WithLanes(3)
                    .SetEnemyForType(Arg.Any<int>(), e2)
                    .WithCollisionBoundary(10, 15)
                    .ForSelectedLanes(0)
                    .SetTimeForEnemy(0, 6)
                    .GeneratesEnemyForLane(Arg.Any<int>(), 0)
                    .HasEnemyInLane(0, e1)
                    .HasEnemyInLane(1, null).HasEnemyInLane(2, null)
                    .Build();

                var ai = ScriptableObject.CreateInstance<LevelEnemyAI>();
                ai.SetLevelEnemies(levelEnemies);

                var contexts = ai.GenerateEnemySpawnContexts().ToList();

                ValidateThat.EnemySpawnContext(contexts[0])
                    .IsNotNull()
                    .IsSet(0, 0, e2);

                ValidateThat.EnemySpawnContext(contexts[1])
                        .IsNotNull()
                        .IsNotSet();

                ValidateThat.EnemySpawnContext(contexts[2])
                        .IsNotNull()
                        .IsNotSet();
            }

            [Test]
            public void if_enemies_block_path_dont_generate_enemy()
            {
                var e1 = BuildAn.Enemy.WhoseNameIs("lol1").WithSpeed(1).CalculateTimeFor(minY, maxY).Build();
                var e2 = BuildAn.Enemy.WhoseNameIs("lol2").WithSpeed(2).Build();

                var levelEnemies = BuildA.LevelEnemies.WithLanes(3)
                    .SetEnemyForType(Arg.Any<int>(), e2)
                    .WithCollisionBoundary(10, 15)
                    .ForSelectedLanes(0)
                    .SetTimeForEnemy(0, 4)
                    .GeneratesEnemyForLane(Arg.Any<int>(), 0)
                    .HasEnemyInLane(0, null)
                    .HasEnemyInLane(1, e1).HasEnemyInLane(2, e1)
                    .Build();

                var ai = ScriptableObject.CreateInstance<LevelEnemyAI>();
                ai.SetLevelEnemies(levelEnemies);

                var contexts = ai.GenerateEnemySpawnContexts().ToList();

                contexts.ForEach(c =>
                    ValidateThat.EnemySpawnContext(c)
                        .IsNotNull()
                        .IsNotSet()
                );
            }

            [Test]
            public void if_enemies_dont_block_path_generate_enemy()
            {
                var e1 = BuildAn.Enemy.WhoseNameIs("lol1").WithSpeed(1).CalculateTimeFor(minY, maxY).Build();
                var e2 = BuildAn.Enemy.WhoseNameIs("lol2").WithSpeed(1).StartedAt(5).CalculateTimeFor(minY, maxY).Build();
                var e3 = BuildAn.Enemy.WhoseNameIs("lol1").WithSpeed(1).StartedAt(10).CalculateTimeFor(minY, maxY).Build();

                var levelEnemies = BuildA.LevelEnemies.WithLanes(3)
                    .SetEnemyForType(Arg.Any<int>(), e2)
                    .WithCollisionBoundary(10, 15)
                    .ForSelectedLanes(0)
                    .SetTimeForEnemy(0, 5)
                    .GeneratesEnemyForLane(Arg.Any<int>(), 0)
                    .HasEnemyInLane(0, null)
                    .HasEnemyInLane(1, e1).HasEnemyInLane(2, e3)
                    .Build();

                var ai = ScriptableObject.CreateInstance<LevelEnemyAI>();
                ai.SetLevelEnemies(levelEnemies);

                var contexts = ai.GenerateEnemySpawnContexts().ToList();

                ValidateThat.EnemySpawnContext(contexts[0])
                    .IsNotNull()
                    .IsSet(0, 0, e2);

                ValidateThat.EnemySpawnContext(contexts[1])
                        .IsNotNull()
                        .IsNotSet();

                ValidateThat.EnemySpawnContext(contexts[2])
                        .IsNotNull()
                        .IsNotSet();
            }

            [Test]
            public void if_enemies_dont_block_path_with_2overlapping_enemies_generate_enemy()
            {
                var e1 = BuildAn.Enemy.WhoseNameIs("lol1").WithSpeed(1).CalculateTimeFor(minY, maxY).Build();
                var e2 = BuildAn.Enemy.WhoseNameIs("lol2").WithSpeed(2).StartedAt(4).CalculateTimeFor(minY, maxY).Build();

                var levelEnemies = BuildA.LevelEnemies.WithLanes(3)
                    .SetEnemyForType(Arg.Any<int>(), e2)
                    .WithCollisionBoundary(10, 15)
                    .ForSelectedLanes(0)
                    .SetTimeForEnemy(0, 4)
                    .GeneratesEnemyForLane(Arg.Any<int>(), 0)
                    .HasEnemyInLane(0, null)
                    .HasEnemyInLane(1, e1).HasEnemyInLane(2, null)
                    .Build();

                var ai = ScriptableObject.CreateInstance<LevelEnemyAI>();
                ai.SetLevelEnemies(levelEnemies);

                var contexts = ai.GenerateEnemySpawnContexts().ToList();

                ValidateThat.EnemySpawnContext(contexts[0])
                    .IsNotNull()
                    .IsSet(0, 0, e2);

                ValidateThat.EnemySpawnContext(contexts[1])
                        .IsNotNull()
                        .IsNotSet();

                ValidateThat.EnemySpawnContext(contexts[2])
                        .IsNotNull()
                        .IsNotSet();
            }
        }

        public class NegativeBoundaries
        {
            private float minY = -6;
            private float maxY = -9;

            [Test]
            public void if_collision_detected_in_same_lane_dont_generate_enemy()
            {
                var e1 = BuildAn.Enemy.WhoseNameIs("lol1").WithSpeed(1).CalculateTimeFor(minY, maxY).Build();
                var e2 = BuildAn.Enemy.WhoseNameIs("lol2").WithSpeed(2).Build();

                var levelEnemies = BuildA.LevelEnemies.WithLanes(3)
                    .SetEnemyForType(Arg.Any<int>(), e2)
                    .WithCollisionBoundary(minY, maxY)
                    .SetTimeForEnemy(0, 4)
                    .ForSelectedLanes(0)
                    .GeneratesEnemyForLane(Arg.Any<int>(), 0)
                    .HasEnemyInLane(0, e1)
                    .HasEnemyInLane(1, null).HasEnemyInLane(2, null)
                    .Build();

                var ai = ScriptableObject.CreateInstance<LevelEnemyAI>();
                ai.SetLevelEnemies(levelEnemies);

                var contexts = ai.GenerateEnemySpawnContexts().ToList();

                contexts.ForEach(c =>
                    ValidateThat.EnemySpawnContext(c)
                        .IsNotNull()
                        .IsNotSet()
                );
            }

            [Test]
            public void if_no_collision_detected_in_same_lane_generate_enemy()
            {
                var e1 = BuildAn.Enemy.WhoseNameIs("lol1").WithSpeed(1).CalculateTimeFor(minY, maxY).Build();
                var e2 = BuildAn.Enemy.WhoseNameIs("lol2").WithSpeed(1).StartedAt(6).CalculateTimeFor(minY, maxY).Build();

                var levelEnemies = BuildA.LevelEnemies.WithLanes(3)
                    .SetEnemyForType(Arg.Any<int>(), e2)
                    .WithCollisionBoundary(minY, maxY)
                    .ForSelectedLanes(0)
                    .SetTimeForEnemy(0, 6)
                    .GeneratesEnemyForLane(Arg.Any<int>(), 0)
                    .HasEnemyInLane(0, e1)
                    .HasEnemyInLane(1, null).HasEnemyInLane(2, null)
                    .Build();

                var ai = ScriptableObject.CreateInstance<LevelEnemyAI>();
                ai.SetLevelEnemies(levelEnemies);

                var contexts = ai.GenerateEnemySpawnContexts().ToList();

                ValidateThat.EnemySpawnContext(contexts[0])
                    .IsNotNull()
                    .IsSet(0, 0, e2);

                ValidateThat.EnemySpawnContext(contexts[1])
                        .IsNotNull()
                        .IsNotSet();

                ValidateThat.EnemySpawnContext(contexts[2])
                        .IsNotNull()
                        .IsNotSet();
            }

            [Test]
            public void if_enemies_block_path_dont_generate_enemy()
            {
                var e1 = BuildAn.Enemy.WhoseNameIs("lol1").WithSpeed(1).CalculateTimeFor(minY, maxY).Build();
                var e2 = BuildAn.Enemy.WhoseNameIs("lol2").WithSpeed(2).Build();

                var levelEnemies = BuildA.LevelEnemies.WithLanes(3)
                    .SetEnemyForType(Arg.Any<int>(), e2)
                    .WithCollisionBoundary(minY, maxY)
                    .ForSelectedLanes(0)
                    .SetTimeForEnemy(0, 4)
                    .GeneratesEnemyForLane(Arg.Any<int>(), 0)
                    .HasEnemyInLane(0, null)
                    .HasEnemyInLane(1, e1).HasEnemyInLane(2, e1)
                    .Build();

                var ai = ScriptableObject.CreateInstance<LevelEnemyAI>();
                ai.SetLevelEnemies(levelEnemies);

                var contexts = ai.GenerateEnemySpawnContexts().ToList();

                contexts.ForEach(c =>
                    ValidateThat.EnemySpawnContext(c)
                        .IsNotNull()
                        .IsNotSet()
                );
            }


            [Test]
            public void if_all_lanes_selected_enemies_block_path_dont_generate_enemy()
            {
                var e1 = BuildAn.Enemy.WhoseNameIs("lol1").WithSpeed(1).StartedAt(0).CalculateTimeFor(minY, maxY).Build();
                var e2 = BuildAn.Enemy.WhoseNameIs("lol2").WithSpeed(2).StartedAt(0).CalculateTimeFor(minY, maxY).Build();
                var e3 = BuildAn.Enemy.WhoseNameIs("lol1").WithSpeed(1).StartedAt(0).CalculateTimeFor(minY, maxY).Build();

                var levelEnemies = BuildA.LevelEnemies.WithLanes(3)
                    .SetEnemyForType(Arg.Any<int>(), e2)
                    .WithCollisionBoundary(minY, maxY)
                    .ForSelectedLanes(0, 1, 2)
                    .SetTimeForEnemy(0, 0)
                    .GeneratesEnemyForLane(Arg.Any<int>(), 0)
                    .HasEnemyInLane(0, null)
                    .HasEnemyInLane(1, null).HasEnemyInLane(2, null)
                    .Build();

                var ai = ScriptableObject.CreateInstance<LevelEnemyAI>();
                ai.SetLevelEnemies(levelEnemies);

                var contexts = ai.GenerateEnemySpawnContexts().ToList();

                ValidateThat.EnemySpawnContext(contexts[0])
                    .IsNotNull()
                    .IsSet(0, 0, e2);

                ValidateThat.EnemySpawnContext(contexts[1])
                    .IsNotNull()
                    .IsSet(1, 0, e2);

                ValidateThat.EnemySpawnContext(contexts[2])
                    .IsNotNull()
                    .IsNotSet();
            }

            [Test]
            public void if_enemies_dont_block_path_generate_enemy()
            {
                var e1 = BuildAn.Enemy.WhoseNameIs("lol1").WithSpeed(1).CalculateTimeFor(minY, maxY).Build();
                var e2 = BuildAn.Enemy.WhoseNameIs("lol2").WithSpeed(1).StartedAt(6).CalculateTimeFor(minY, maxY).Build();

                var levelEnemies = BuildA.LevelEnemies.WithLanes(3)
                    .SetEnemyForType(Arg.Any<int>(), e2)
                    .WithCollisionBoundary(minY, maxY)
                    .ForSelectedLanes(0)
                    .SetTimeForEnemy(0, 6)
                    .GeneratesEnemyForLane(Arg.Any<int>(), 0)
                    .HasEnemyInLane(0, null)
                    .HasEnemyInLane(1, e1).HasEnemyInLane(2, e1)
                    .Build();

                var ai = ScriptableObject.CreateInstance<LevelEnemyAI>();
                ai.SetLevelEnemies(levelEnemies);

                var contexts = ai.GenerateEnemySpawnContexts().ToList();

                ValidateThat.EnemySpawnContext(contexts[0])
                    .IsNotNull()
                    .IsSet(0, 0, e2);

                ValidateThat.EnemySpawnContext(contexts[1])
                        .IsNotNull()
                        .IsNotSet();

                ValidateThat.EnemySpawnContext(contexts[2])
                        .IsNotNull()
                        .IsNotSet();
            }

            [Test]
            public void if_enemies_dont_block_path_with_2overlapping_enemies_generate_enemy()
            {
                var e1 = BuildAn.Enemy.WhoseNameIs("lol1").WithSpeed(1).CalculateTimeFor(minY, maxY).Build();
                var e2 = BuildAn.Enemy.WhoseNameIs("lol2").WithSpeed(2).StartedAt(4).CalculateTimeFor(minY, maxY).Build();

                var levelEnemies = BuildA.LevelEnemies.WithLanes(3)
                    .SetEnemyForType(Arg.Any<int>(), e2)
                    .WithCollisionBoundary(minY, maxY)
                    .ForSelectedLanes(0)
                    .SetTimeForEnemy(0, 4)
                    .GeneratesEnemyForLane(Arg.Any<int>(), 0)
                    .HasEnemyInLane(0, null)
                    .HasEnemyInLane(1, e1).HasEnemyInLane(2, null)
                    .Build();

                var ai = ScriptableObject.CreateInstance<LevelEnemyAI>();
                ai.SetLevelEnemies(levelEnemies);

                var contexts = ai.GenerateEnemySpawnContexts().ToList();

                ValidateThat.EnemySpawnContext(contexts[0])
                    .IsNotNull()
                    .IsSet(0, 0, e2);

                ValidateThat.EnemySpawnContext(contexts[1])
                    .IsNotNull()
                    .IsNotSet();

                ValidateThat.EnemySpawnContext(contexts[2])
                    .IsNotNull()
                    .IsNotSet();
            }
        }
    }
}
