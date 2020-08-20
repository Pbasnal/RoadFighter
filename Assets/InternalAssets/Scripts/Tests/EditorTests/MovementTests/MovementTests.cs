using Assets.Scripts.Tests.EditorTests;
using Assets.Scripts.UnityLogic.ScriptableObjects;
using NUnit.Framework;
using UnityEngine;

namespace MovementTests
{
    public class StartingAtOrigin
    {
        // A Test behaves as an ordinary method
        [Test]
        public void left_moves_actor_moveunits_to_left()
        {
            var moveUnits = 2;
            var actor = BuildAn.Actor.WithSpeed(2)
                .WhichCanMoveUnits(moveUnits)
                .At(new Vector2(0, 0)).Build();

            RigidbodyController controller =
                BuildA.ControllerScenario.ForActor(actor)
                    .MoveDestinationLeft()
                    .MoveTillDestination(out var deltaTime);

            Assert.AreEqual(-moveUnits, actor.CurrentPosition.x);
        }

        [Test]
        public void left_full_right_full_moves_actor_to_origin()
        {
            var moveUnits = 2;
            var actor = BuildAn.Actor.WithSpeed(2)
                .WhichCanMoveUnits(moveUnits)
                .At(new Vector2(0, 0)).Build();

            RigidbodyController controller =
                BuildA.ControllerScenario.ForActor(actor)
                    .MoveDestinationLeft()
                    .MoveTillDestination(out var deltaTime)
                    .MoveDestinationRight()
                    .MoveTillDestination(out deltaTime);

            Assert.AreEqual(0, actor.CurrentPosition.x);
        }

        [Test]
        public void double_left_moves_actor_2moveunits_to_left()
        {
            var moveUnits = 2;
            var actor = BuildAn.Actor.WithSpeed(2)
                .WhichCanMoveUnits(moveUnits)
                .At(new Vector2(0, 0)).Build();

            RigidbodyController controller =
                BuildA.ControllerScenario.ForActor(actor)
                    .MoveDestinationLeft()
                    .MoveDestinationLeft()
                    .MoveTillDestination(out var deltaTime);

            Assert.IsTrue(deltaTime > 2 && deltaTime < 3);
            Assert.AreEqual(-moveUnits * 2, actor.CurrentPosition.x);
        }

        [Test]
        public void double_left_waitfor0_9_right_moves_actor_backto_origin()
        {
            var moveUnits = 2;
            var actor = BuildAn.Actor.WithSpeed(2)
                .WhichCanMoveUnits(moveUnits)
                .At(new Vector2(0, 0)).Build();

            RigidbodyController controller =
                BuildA.ControllerScenario.ForActor(actor)
                    .MoveDestinationLeft()
                    .MoveDestinationLeft()
                    .MoveFor(0.9f)
                    .MoveDestinationRight()
                    .MoveTillDestination(out var deltaTime);

            Assert.AreEqual(0, actor.CurrentPosition.x);
        }

        [Test]
        public void double_left_waitfor1_5_right_moves_actor_moveunits_to_left()
        {
            var moveUnits = 2;
            var actor = BuildAn.Actor.WithSpeed(2)
                .WhichCanMoveUnits(moveUnits)
                .At(new Vector2(0, 0)).Build();

            RigidbodyController controller =
                BuildA.ControllerScenario.ForActor(actor)
                    .MoveDestinationLeft()
                    .MoveDestinationLeft()
                    .MoveFor(1.5f)
                    .MoveDestinationRight()
                    .MoveTillDestination(out var deltaTime);

            Assert.AreEqual(-moveUnits, actor.CurrentPosition.x);
        }

        [Test]
        public void right_moves_actor_moveunits_to_right()
        {
            var moveUnits = 2;
            var actor = BuildAn.Actor.WithSpeed(2)
                .WhichCanMoveUnits(moveUnits)
                .At(new Vector2(0, 0)).Build();

            RigidbodyController controller =
                BuildA.ControllerScenario.ForActor(actor)
                    .MoveDestinationRight()
                    .MoveTillDestination(out var deltaTime);

            Assert.AreEqual(moveUnits, actor.CurrentPosition.x);
        }

        [Test]
        public void right_full_left_full_moves_actor_to_origin()
        {
            var moveUnits = 2;
            var actor = BuildAn.Actor.WithSpeed(2)
                .WhichCanMoveUnits(moveUnits)
                .At(new Vector2(0, 0)).Build();

            RigidbodyController controller =
                BuildA.ControllerScenario.ForActor(actor)
                    .MoveDestinationRight()
                    .MoveTillDestination(out var deltaTime)
                    .MoveDestinationLeft()
                    .MoveTillDestination(out deltaTime);

            Assert.AreEqual(0, actor.CurrentPosition.x);
        }

        [Test]
        public void double_right_moves_actor_2moveunits_to_right()
        {
            var moveUnits = 2;
            var actor = BuildAn.Actor.WithSpeed(2)
                .WhichCanMoveUnits(moveUnits)
                .At(new Vector2(0, 0)).Build();

            RigidbodyController controller =
                BuildA.ControllerScenario.ForActor(actor)
                    .MoveDestinationRight()
                    .MoveDestinationRight()
                    .MoveTillDestination(out var deltaTime);

            Assert.IsTrue(deltaTime > 2 && deltaTime < 3);
            Assert.AreEqual(moveUnits * 2, actor.CurrentPosition.x);
        }

        [Test]
        public void double_right_waitfor0_9_left_moves_actor_backto_origin()
        {
            var moveUnits = 2;
            var actor = BuildAn.Actor.WithSpeed(2)
                .WhichCanMoveUnits(moveUnits)
                .At(new Vector2(0, 0)).Build();

            RigidbodyController controller =
                BuildA.ControllerScenario.ForActor(actor)
                    .MoveDestinationRight()
                    .MoveDestinationRight()
                    .MoveFor(0.9f)
                    .MoveDestinationLeft()
                    .MoveTillDestination(out var deltaTime);

            Assert.AreEqual(0, actor.CurrentPosition.x);
        }

        [Test]
        public void double_right_waitfor1_5_left_moves_actor_moveunits_to_right()
        {
            var moveUnits = 2;
            var actor = BuildAn.Actor.WithSpeed(2)
                .WhichCanMoveUnits(moveUnits)
                .At(new Vector2(0, 0)).Build();

            RigidbodyController controller =
                BuildA.ControllerScenario.ForActor(actor)
                    .MoveDestinationRight()
                    .MoveDestinationRight()
                    .MoveFor(1.5f)
                    .MoveDestinationLeft()
                    .MoveTillDestination(out var deltaTime);

            Assert.AreEqual(moveUnits, actor.CurrentPosition.x);
        }
    }

    public class StartingAtMoveUnitsToLeft
    {
        private Vector2 startingPosition;
        private int moveUnits = 2;

        public StartingAtMoveUnitsToLeft()
        {
            startingPosition = new Vector2(-moveUnits, 0);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void left_moves_actor_moveunits_to_left()
        {

            var actor = BuildAn.Actor.WithSpeed(2)
                .WhichCanMoveUnits(moveUnits)
                .At(startingPosition).Build();

            RigidbodyController controller =
                BuildA.ControllerScenario.ForActor(actor)
                    .MoveDestinationLeft()
                    .MoveTillDestination(out var deltaTime);

            Assert.AreEqual(-moveUnits + startingPosition.x, actor.CurrentPosition.x);
        }

        [Test]
        public void left_full_right_full_moves_actor_to_origin()
        {

            var actor = BuildAn.Actor.WithSpeed(2)
                .WhichCanMoveUnits(moveUnits)
                .At(startingPosition).Build();

            RigidbodyController controller =
                BuildA.ControllerScenario.ForActor(actor)
                    .MoveDestinationLeft()
                    .MoveTillDestination(out var deltaTime)
                    .MoveDestinationRight()
                    .MoveTillDestination(out deltaTime);

            Assert.AreEqual(startingPosition.x, actor.CurrentPosition.x);
        }

        [Test]
        public void double_left_moves_actor_2moveunits_to_left()
        {

            var actor = BuildAn.Actor.WithSpeed(2)
                .WhichCanMoveUnits(moveUnits)
                .At(startingPosition).Build();

            RigidbodyController controller =
                BuildA.ControllerScenario.ForActor(actor)
                    .MoveDestinationLeft()
                    .MoveDestinationLeft()
                    .MoveTillDestination(out var deltaTime);

            Assert.IsTrue(deltaTime > 2 && deltaTime < 3);
            Assert.AreEqual(-moveUnits * 2 + startingPosition.x, actor.CurrentPosition.x);
        }

        [Test]
        public void double_left_waitfor0_9_right_moves_actor_backto_origin()
        {

            var actor = BuildAn.Actor.WithSpeed(2)
                .WhichCanMoveUnits(moveUnits)
                .At(startingPosition).Build();

            RigidbodyController controller =
                BuildA.ControllerScenario.ForActor(actor)
                    .MoveDestinationLeft()
                    .MoveDestinationLeft()
                    .MoveFor(0.9f)
                    .MoveDestinationRight()
                    .MoveTillDestination(out var deltaTime);

            Assert.AreEqual(startingPosition.x, actor.CurrentPosition.x);
        }

        [Test]
        public void double_left_waitfor1_5_right_moves_actor_moveunits_to_left()
        {

            var actor = BuildAn.Actor.WithSpeed(2)
                .WhichCanMoveUnits(moveUnits)
                .At(startingPosition).Build();

            RigidbodyController controller =
                BuildA.ControllerScenario.ForActor(actor)
                    .MoveDestinationLeft()
                    .MoveDestinationLeft()
                    .MoveFor(1.5f)
                    .MoveDestinationRight()
                    .MoveTillDestination(out var deltaTime);

            Assert.AreEqual(-moveUnits + startingPosition.x, actor.CurrentPosition.x);
        }

        [Test]
        public void right_moves_actor_moveunits_to_right()
        {

            var actor = BuildAn.Actor.WithSpeed(2)
                .WhichCanMoveUnits(moveUnits)
                .At(startingPosition).Build();

            RigidbodyController controller =
                BuildA.ControllerScenario.ForActor(actor)
                    .MoveDestinationRight()
                    .MoveTillDestination(out var deltaTime);

            Assert.AreEqual(moveUnits + startingPosition.x, actor.CurrentPosition.x);
        }

        [Test]
        public void right_full_left_full_moves_actor_to_origin()
        {

            var actor = BuildAn.Actor.WithSpeed(2)
                .WhichCanMoveUnits(moveUnits)
                .At(startingPosition).Build();

            RigidbodyController controller =
                BuildA.ControllerScenario.ForActor(actor)
                    .MoveDestinationRight()
                    .MoveTillDestination(out var deltaTime)
                    .MoveDestinationLeft()
                    .MoveTillDestination(out deltaTime);

            Assert.AreEqual(startingPosition.x, actor.CurrentPosition.x);
        }

        [Test]
        public void double_right_moves_actor_2moveunits_to_right()
        {

            var actor = BuildAn.Actor.WithSpeed(2)
                .WhichCanMoveUnits(moveUnits)
                .At(startingPosition).Build();

            RigidbodyController controller =
                BuildA.ControllerScenario.ForActor(actor)
                    .MoveDestinationRight()
                    .MoveDestinationRight()
                    .MoveTillDestination(out var deltaTime);

            Assert.IsTrue(deltaTime > 2 && deltaTime < 3);
            Assert.AreEqual(moveUnits * 2 + startingPosition.x, actor.CurrentPosition.x);
        }

        [Test]
        public void double_right_waitfor0_9_left_moves_actor_backto_origin()
        {

            var actor = BuildAn.Actor.WithSpeed(2)
                .WhichCanMoveUnits(moveUnits)
                .At(startingPosition).Build();

            RigidbodyController controller =
                BuildA.ControllerScenario.ForActor(actor)
                    .MoveDestinationRight()
                    .MoveDestinationRight()
                    .MoveFor(0.9f)
                    .MoveDestinationLeft()
                    .MoveTillDestination(out var deltaTime);

            Assert.AreEqual(startingPosition.x, actor.CurrentPosition.x);
        }

        [Test]
        public void double_right_waitfor1_5_left_moves_actor_moveunits_to_right()
        {

            var actor = BuildAn.Actor.WithSpeed(2)
                .WhichCanMoveUnits(moveUnits)
                .At(startingPosition).Build();

            RigidbodyController controller =
                BuildA.ControllerScenario.ForActor(actor)
                    .MoveDestinationRight()
                    .MoveDestinationRight()
                    .MoveFor(1.5f)
                    .MoveDestinationLeft()
                    .MoveTillDestination(out var deltaTime);

            Assert.AreEqual(moveUnits + startingPosition.x, actor.CurrentPosition.x);
        }
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    //[UnityTest]
    //public IEnumerator MovementTestsWithEnumeratorPasses()
    //{
    //    // Use the Assert class to test conditions.
    //    // Use yield to skip a frame.
    //    yield return null;
    //}
}
