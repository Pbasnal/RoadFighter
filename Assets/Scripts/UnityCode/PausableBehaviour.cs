using UnityEngine;

namespace UnityCode
{
    public abstract class PausableBehaviour : MonoBehaviour
    {
        public virtual void OnPause()
        { }

        public virtual void OnPlay()
        { }
    }
}
