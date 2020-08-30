using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityEngine.UI;
using UnityLogic.BehaviourInterface;

namespace Assets.Scripts.UnityCode
{
    public class PlayerStatTextUI : FloatValueListener
    {
        public Text textField;
        public FloatValue fieldValue;

        private void Update()
        {
            textField.text = fieldValue.value.ToString();
        }
    }
}
