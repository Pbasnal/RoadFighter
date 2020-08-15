using System;
using System.Collections.Generic;
using UnityLogic.CommonInterface;
using Assets.InternalAssets.Scripts.UnityLogic.Mechanics;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace MechanicsTest
{
    public class CarPoolingTests
    {
        [Test]
        public void Recylcing_one_object_10_times_should_pass()
        {
            var car = new MockPoolObject();
            var pool = new GameObjectPool
            {
                poolableObject = car
            };
            
            pool.AddObjectsToPool(1);

            for (int i = 0; i < 10; i++)
            {
                var carInPool = pool.LockObjectForRecycling();
                Assert.IsTrue(carInPool.IsActive);
                pool.DeactivateObject(carInPool);
                Assert.IsFalse(carInPool.IsActive);
            }            
        }

        [Test]
        public void Recylcing_2_object_10_times_should_not_create_more_than_2_objects()
        {
            var car = new MockPoolObject();
            var pool = new GameObjectPool
            {
                poolableObject = car
            };

            pool.AddObjectsToPool(2);

            var poolObjects = new Dictionary<string, ICanBePooled>();

            for (int i = 0; i < 10; i++)
            {
                var carInPool = pool.LockObjectForRecycling();
                Assert.IsTrue(carInPool.IsActive);
                pool.DeactivateObject(carInPool);
                Assert.IsFalse(carInPool.IsActive);

                if (poolObjects.ContainsKey(carInPool.Name))
                {
                    continue;
                }

                poolObjects.Add(carInPool.Name, carInPool);

                Assert.IsTrue(poolObjects.Count <= 2);
            }
        }

        [Test]
        public void Deactivating_inactive_object_shouldnt_change_state()
        {
            var car = new MockPoolObject();
            var pool = new GameObjectPool
            {
                poolableObject = car
            };

            pool.AddObjectsToPool(1);

            var carInPool = pool.LockObjectForRecycling();
            Assert.IsTrue(carInPool.IsActive);
            pool.DeactivateObject(carInPool);
            Assert.IsFalse(carInPool.IsActive);
            pool.DeactivateObject(carInPool);
            Assert.IsFalse(carInPool.IsActive);
        }
    }

    public class MockPoolObject : ICanBePooled
    {
        public string Name { get; private set; }

        public bool IsActive { get; private set; }

        public string Id => "MockPoolObject";

        public GameObject SpawnedObject => null;

        public MockPoolObject()
        {
            Name = Guid.NewGuid().ToString();
        }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }

        public ICanBePooled SpawnANewInstance()
        {
            return new MockPoolObject();
        }
    }
}
