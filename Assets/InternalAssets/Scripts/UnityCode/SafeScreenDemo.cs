using System;
using UnityEngine;

namespace UnityCode
{
    public class SafeScreenDemo : MonoBehaviour
    {
        [SerializeField] KeyCode KeySafeArea = KeyCode.A;
        SafeArea.SimDevice[] Sims;
        int SimIdx;

        public AnimationPlayer animationPlayer;

        void Awake()
        {
            if (!Application.isEditor)
                Destroy(gameObject);

            Sims = (SafeArea.SimDevice[])Enum.GetValues(typeof(SafeArea.SimDevice));
        }

        void Update()
        {
            if (Input.GetKeyDown(KeySafeArea))
                ToggleSafeArea();

            if (Input.GetKeyDown(KeyCode.P))
            {
                animationPlayer.PlayFromBegining("MultiplierIncreased");
            }
        }

        /// <summary>
        /// Toggle the safe area simulation device.
        /// </summary>
        void ToggleSafeArea()
        {
            SimIdx++;

            if (SimIdx >= Sims.Length)
                SimIdx = 0;

            SafeArea.Sim = Sims[SimIdx];
            
            Debug.LogFormat("Switched to sim device {0} with debug key '{1}'", Sims[SimIdx], KeySafeArea);
        }
    }

}
