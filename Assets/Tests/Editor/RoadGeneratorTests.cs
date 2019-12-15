using NSubstitute;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class RoadGeneratorTests
    {
        [Test]
        public void RoadController_DeactivateAndRecycle_RecyclesRoadSection()
        {
            var roadSectionFactory = Substitute.For<IGameObjectFactory<IRoadSection>>();
            var mockSections = GetMockRoadSections(3);
            roadSectionFactory.GetMultipleRoadSections(3).Returns(mockSections);

            var levelSpeed = ScriptableObject.CreateInstance<FloatValue>();
            levelSpeed.value = 1;
            var pool = ScriptableObject.CreateInstance<RoadSectionPool>();

            var roadController = new RoadController(roadSectionFactory, pool, levelSpeed);
            var sections = roadController.InitializeSections(Vector2.zero, 3);

            Assert.IsNotNull(sections);
            Assert.AreEqual(3, sections.Length);
            Assert.IsNotNull(roadController.roadSectionFactory);

            var position = Vector2.zero;
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(position.x, sections[i].Position.x);
                Assert.AreEqual(position.y, sections[i].Position.y);

                position = sections[i].NewSpawnPosition;
            }

            sections[0].SetActive(false);
            pool.DeactivateObject(sections[0]);
            roadController.EnqueueForRecycle(sections[0]);
            roadController.SpawnNewSectionIfAny();
            Assert.AreEqual(position.y, sections[0].Position.y);
            sections[0].Received(2).SetActive(true);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void RoadController_ConstructedWith3Sections_Creates3RoadSections()
        {
            var roadSectionFactory = Substitute.For<IGameObjectFactory<IRoadSection>>();
            var mockSections = GetMockRoadSections(3);
            roadSectionFactory.GetMultipleRoadSections(3).Returns(mockSections);

            var levelSpeed = ScriptableObject.CreateInstance<FloatValue>();
            levelSpeed.value = 1;
            var pool = ScriptableObject.CreateInstance<RoadSectionPool>();

            var roadController = new RoadController(roadSectionFactory, pool, levelSpeed);
            var sections = roadController.InitializeSections(Vector2.zero, 3);

            Assert.IsNotNull(sections);
            Assert.AreEqual(3, sections.Length);
            Assert.IsNotNull(roadController.roadSectionFactory);

            var position = Vector2.zero;
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(position.x, sections[i].Position.x);
                Assert.AreEqual(position.y, sections[i].Position.y);

                position = sections[i].NewSpawnPosition;
            }
        }

        private IRoadSection[] GetMockRoadSections(int count)
        {
            IRoadSection[] sections = new IRoadSection[count];

            sections[0] = Substitute.For<IRoadSection>();
            sections[0].When(x => x.SetPosition(Arg.Any<Vector2>()))
                .Do(x =>
                {
                    var position = x.ArgAt<Vector2>(0);
                    sections[0].NewSpawnPosition.Returns(new Vector2(position.x, position.y + 5));
                    sections[0].Position.Returns(position);                    
                });

            sections[1] = Substitute.For<IRoadSection>();
            sections[1].When(x => x.SetPosition(Arg.Any<Vector2>()))
                .Do(x =>
                {
                    var position = x.ArgAt<Vector2>(0);
                    sections[1].NewSpawnPosition.Returns(new Vector2(position.x, position.y + 5));
                    sections[1].Position.Returns(position);
                });

            sections[2] = Substitute.For<IRoadSection>();
            sections[2].When(x => x.SetPosition(Arg.Any<Vector2>()))
                .Do(x =>
                {
                    var position = x.ArgAt<Vector2>(0);
                    sections[2].NewSpawnPosition.Returns(new Vector2(position.x, position.y + 5));
                    sections[2].Position.Returns(position);
                });

            return sections;
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator PlayerWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
