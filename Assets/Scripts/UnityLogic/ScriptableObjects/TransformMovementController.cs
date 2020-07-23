using Assets.Scripts.UnityLogic.BehaviourInterface;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UnityLogic.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Controllers/Transform", fileName = "TransformController", order = 51)]
    public class TransformMovementController : MovementContoller
    {
        public Direction prevDirection { get; set; }
        public override Vector2 UnitLocation => startingPosition;

        private Vector2 startingPosition;
        private float timeLerped;
        private IMoveableActor actor;

        private Queue<Vector2> dests;

        private bool isLeftBlocked;
        private bool isRightBlocked;

        public override void Move()
        {
            if (dests.Count == 0)
            {
                prevDirection = Direction.None;
                return;
            }
            if ((prevDirection == Direction.Left && isLeftBlocked)
                || (prevDirection == Direction.Right && isRightBlocked))
            {
                return;
            }

            var dest = dests.Peek();

            if ((int)prevDirection * (dest.x - actor.CurrentPosition.x) <= 0)
            {
                timeLerped = 0;
                actor.CurrentPosition = dest;
                startingPosition = dest;
                dests.Dequeue();
                return;
            }

            timeLerped += Time.deltaTime;
            actor.CurrentPosition = Vector2.MoveTowards(startingPosition, dest, actor.Speed * timeLerped);
        }

        public override void SetDirectionBlocked(Direction dir, bool isBlocked)
        {
            if (dir == Direction.Left)
                isLeftBlocked = isBlocked;
            else if (dir == Direction.Right)
                isRightBlocked = isBlocked;
        }

        public override void MoveDestinationLeft()
        {
            switch (prevDirection)
            {
                case Direction.None:
                    Vector2 dest;
                    if (actor.CurrentPosition.x % actor.MoveUnits == 0)
                    {
                        dest = new Vector2((int)(actor.CurrentPosition.x - actor.MoveUnits), actor.CurrentPosition.y);
                    }
                    else
                    {
                        if(actor.CurrentPosition.x >= 0)
                            dest = new Vector2((int)(actor.CurrentPosition.x / actor.MoveUnits), actor.CurrentPosition.y);
                        else
                            dest = new Vector2((int)(actor.CurrentPosition.x / actor.MoveUnits - actor.MoveUnits), actor.CurrentPosition.y);
                    }
                    if (dest.x < actor.LeftLimit)
                    {
                        return;
                    }

                    dests.Enqueue(dest);
                    startingPosition = actor.CurrentPosition;
                    prevDirection = Direction.Left;
                    timeLerped = 0;
                    break;
                case Direction.Left:
                    if (dests.Count == 0)
                    {
                        dest = new Vector2(actor.CurrentPosition.x - actor.MoveUnits, actor.CurrentPosition.y);
                        
                    }
                    else
                    {
                        dest = dests.Peek();
                        dest = new Vector2(dest.x - actor.MoveUnits, dest.y);                        
                    }
                    if (dest.x < actor.LeftLimit)
                    {
                        return;
                    }
                    dests.Enqueue(dest);
                    break;
                case Direction.Right:
                    dest = dests.Dequeue();
                    dests.Clear();
                    dests.Enqueue(new Vector2(dest.x - actor.MoveUnits, dest.y));
                    startingPosition = actor.CurrentPosition;
                    prevDirection = Direction.Left;
                    timeLerped = 0;
                    break;
            }
        }

        public override void MoveDestinationRight()
        {
            switch (prevDirection)
            {
                case Direction.None:
                    Vector2 dest;
                    if (actor.CurrentPosition.x % actor.MoveUnits == 0)
                    {
                        dest = new Vector2(actor.CurrentPosition.x + actor.MoveUnits, actor.CurrentPosition.y);
                    }
                    else
                    {
                        dest = new Vector2(actor.CurrentPosition.x / actor.MoveUnits + actor.MoveUnits, actor.CurrentPosition.y);
                    }

                    if (dest.x > actor.RightLimit)
                    {
                        return;
                    }

                    dests.Enqueue(dest);
                    startingPosition = actor.CurrentPosition;
                    timeLerped = 0;
                    break;
                case Direction.Right:
                    if (dests.Count == 0)
                    {
                        dest = new Vector2(actor.CurrentPosition.x + actor.MoveUnits, actor.CurrentPosition.y);
                    }
                    else
                    {
                        dest = dests.Peek();
                        dest = new Vector2(dest.x + actor.MoveUnits, dest.y);
                    }

                    if (dest.x > actor.RightLimit)
                    {
                        return;
                    }

                    dests.Enqueue(dest);
                    break;
                case Direction.Left:
                    dest = dests.Dequeue();
                    dests.Clear();
                    dests.Enqueue(new Vector2(dest.x + actor.MoveUnits, dest.y));
                    startingPosition = actor.CurrentPosition;
                    timeLerped = 0;
                    break;
            }
            prevDirection = Direction.Right;
        }

        public override void SetActor(IMoveableActor actor)
        {
            this.actor = actor;
            dests = new Queue<Vector2>();
            startingPosition = actor.CurrentPosition;
            prevDirection = Direction.None;
        }

        public enum Direction
        {
            None = 0,
            Left = -1,
            Right = 1
        }
    }
}
