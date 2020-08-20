﻿using System.Collections.Generic;
using Assets.Scripts.UnityLogic.BehaviourInterface;
using UnityEngine;

namespace Assets.Scripts.UnityLogic.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Controllers/Rigidbody", fileName = "RigidbodyController", order = 51)]
    public class RigidbodyController : MovementContoller
    {
        [Header("Lane settings")]
        public float distanceBetweenLanes;
        public float leftMostLaneXPosition;
        public float rightMostLaneXPosition;

        private float lanePositionToMoveTo;
        private List<Direction> _inputCollection;
        private Vector2 dest;

        private IMoveableActor actor;

        public override Vector2 UnitLocation { get; }

        public override void Move()
        {
            if (_inputCollection.Count == 0)
            {
                return;
            }

            _inputCollection.ForEach(dir => lanePositionToMoveTo += distanceBetweenLanes * (int)dir);
            lanePositionToMoveTo = Mathf.Clamp(lanePositionToMoveTo, leftMostLaneXPosition, rightMostLaneXPosition);

            dest.x = lanePositionToMoveTo;
            dest.y = actor.CurrentPosition.y;

            actor.MoveTo(dest);
            _inputCollection.Clear();
        }

        public override void SetDirectionBlocked(Direction dir, bool isBlocked)
        {
            if (!isBlocked) 
            {
                return;
            }
                
            if (dir == Direction.Left)
            {
                _inputCollection.Add(Direction.Right);
                return;
            }
            if (dir == Direction.Right)
            {
                _inputCollection.Add(Direction.Left);
                return;
            }
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
            startingPosition = actor.CurrentPosition;
            prevDirection = Direction.None;
            _inputCollection = new List<Direction>();
            dest = new Vector2();
            lanePositionToMoveTo = actor.CurrentPosition.x;
        }
    }
}