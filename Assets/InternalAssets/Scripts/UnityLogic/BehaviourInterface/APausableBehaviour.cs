using UnityEngine;

namespace UnityLogic.BehaviourInterface
{
    public abstract class APausableBehaviour : MonoBehaviour
    {
        public virtual void OnPause()
        {
            enabled = false;
        }

        public virtual void OnPlay()
        {
            enabled = true;
        }
    }
}
