﻿using UnityEngine;

namespace Assets.Scripts.UnityCode
{
    [CreateAssetMenu(menuName = "Input/Keyboard", fileName = "KeyboardInputManager", order = 51)]
    public class KeyboardInputManager : InputManager
    {
        public override InputCommand GetCommand()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                return InputCommand.Left;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                return InputCommand.Right;
            }

            return InputCommand.None;
        }
    }
}
