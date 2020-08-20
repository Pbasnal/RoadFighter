using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace UnityCode.CameraScripts
{
    public class CameraShaker : MonoBehaviour
    {
        private CinemachineVirtualCamera cinemachine;
        private CinemachineBasicMultiChannelPerlin cinemachinePerlin;
        private CinemachineFramingTransposer cinemachineFraming;

        private float cinemachineFrameInitialScreenY;

        private void Awake()
        {
            cinemachine = GetComponent<CinemachineVirtualCamera>();
            cinemachinePerlin = cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineFraming = cinemachine.GetCinemachineComponent<CinemachineFramingTransposer>();
            cinemachineFrameInitialScreenY = cinemachineFraming.m_ScreenY;
        }

        public void ShakeCamera(ShakeParameters shakeParameters)
        {
            StartCoroutine(TriggerCameraShake(shakeParameters));
        }

        private IEnumerator TriggerCameraShake(ShakeParameters shakeParameters)
        {
            cinemachinePerlin.m_FrequencyGain = shakeParameters.frequency;
            var elapsedTime = 0f;

            while (elapsedTime < shakeParameters.smoothInTime)
            {
                elapsedTime += Time.deltaTime;
                cinemachinePerlin.m_AmplitudeGain = 
                    Mathf.Lerp(0, shakeParameters.intensity, 
                        elapsedTime/ shakeParameters.smoothInTime);
                yield return null;
            }

            cinemachinePerlin.m_AmplitudeGain = shakeParameters.intensity;
            while (elapsedTime < shakeParameters.duration - shakeParameters.smoothOutTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            var total = shakeParameters.duration - elapsedTime;
            while (elapsedTime < shakeParameters.duration)
            {
                var currentDiff = shakeParameters.duration - elapsedTime;
                elapsedTime += Time.deltaTime;
                cinemachinePerlin.m_AmplitudeGain = 
                    Mathf.Lerp(0, shakeParameters.intensity, 
                        currentDiff / total);
                yield return null;
            }
        }

        public void CameraDragTo(float screenY, float duration)
        {
            StartCoroutine(DragCamera(screenY, duration));
        }

        public void ResetCameraScreenPosition(float duration)
        {
            StartCoroutine(DragCamera(cinemachineFrameInitialScreenY, duration));
        }

        private IEnumerator DragCamera(float screenYToDragTo, float duration)
        {
            var elapsedTime = 0f;
            
            while (elapsedTime < duration)
            {
                cinemachineFraming.m_ScreenY = 
                    Mathf.Lerp(
                        cinemachineFraming.m_ScreenY, 
                        screenYToDragTo, 
                        elapsedTime / duration);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        [Serializable]
        public struct ShakeParameters
        {
            public float intensity;
            public float frequency;
            public float duration;
            public float smoothInTime;
            public float smoothOutTime;

            public ShakeParameters(float intensity, float frequency,
            float duration, float smoothInTime, float smoothOutTime)
            {
                if (smoothOutTime + smoothInTime > duration)
                {
                    throw new UnityException("SmoothInTime + SmoothOutTime should be less or equal to the duration");
                }

                this.intensity = intensity;
                this.frequency = frequency;
                this.duration = duration;
                this.smoothInTime = smoothInTime;
                this.smoothOutTime = smoothOutTime;

            }
        }
    }
}
