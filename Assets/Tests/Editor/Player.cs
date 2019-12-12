using NSubstitute;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

namespace Tests
{
    public class Player
    {
        // A Test behaves as an ordinary method
        [Test]
        public void PlayerMovesLeft()
        {
            var move = Substitute.For<IMove>();
            move.MovementRange = 1;
            move.MoveUnits = 1;
            var playerController = new PlayerMovementController(move);

            var direction = playerController.MoveLeft();

            Assert.IsTrue(-1 == direction);
            Assert.IsTrue(-1 * move.MoveUnits == move.CurrentPositionInUnits);
        }

        [Test]
        public void PlayerMovesRight()
        {
            var move = Substitute.For<IMove>();
            move.MovementRange = 1;
            move.MoveUnits = 1;
            var playerController = new PlayerMovementController(move);

            var direction = playerController.MoveRight();

            Assert.IsTrue(1 == direction);
            Assert.IsTrue(move.MoveUnits == move.CurrentPositionInUnits);
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
