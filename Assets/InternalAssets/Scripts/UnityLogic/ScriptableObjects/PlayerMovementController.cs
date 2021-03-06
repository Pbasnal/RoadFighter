﻿using System.Collections.Generic;
using Assets.Scripts.UnityLogic.BehaviourInterface;
using UnityEngine;

namespace Assets.Scripts.UnityLogic.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Controllers/Rigidbody", fileName = "RigidbodyController", order = 51)]
    public class PlayerMovementController : MovementContoller
    {
        [Header("Lane settings")]
        public float distanceBetweenLanes;
        public float leftMostLaneXPosition;
        public float rightMostLaneXPosition;

        private float lanePositionToMoveTo;
        private List<Direction> _inputCollection;
        private Vector3 dest;

        private IMoveableActor actor;

        public override Vector3 GetDestination()
        {
            if (_inputCollection.Count == 0)
            {
                return actor.Destination;
            }

            _inputCollection.ForEach(dir => lanePositionToMoveTo += distanceBetweenLanes * (int)dir);
            lanePositionToMoveTo = Mathf.Clamp(lanePositionToMoveTo, leftMostLaneXPosition, rightMostLaneXPosition);

            dest.x = lanePositionToMoveTo;
            dest.y = actor.CurrentPosition.y;

            //actor.MoveTo(dest);
            _inputCollection.Clear();

            return dest;
        }

        public override void MoveDestinationLeft()
        {
            _inputCollection.Add(Direction.Left);
        }

        public override void MoveDestinationRight()
        {
            _inputCollection.Add(Direction.Right);
        }

        public override void SetActor(IMoveableActor actor)
        {
            this.actor = actor;
            _inputCollection = new List<Direction>();
            dest = new Vector2();
            lanePositionToMoveTo = actor.CurrentPosition.x;
        }
    }
}
