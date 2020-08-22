using UnityEngine;

namespace Assets.Scripts.UnityCode
{
    [CreateAssetMenu(menuName = "Input/Keyboard", fileName = "KeyboardInputManager", order = 51)]
    public class KeyboardInputManager : ScriptableInputManager
    {
        public override float TimeWhenCommandCame { get; set; }

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
