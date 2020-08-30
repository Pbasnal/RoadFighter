using System.Collections.Generic;
using UnityEngine;
using UnityLogic.BehaviourInterface;

namespace UnityCode.Managers
{
    public class AudioManager : APausableBehaviour
    {
        public Sound[] sounds;

        private IDictionary<string, Sound> soundsMap;
        private static AudioManager _instance;
        private string _currentlyPlayingClipName;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            soundsMap = new Dictionary<string, Sound>();
            foreach (var sound in sounds)
            {
                sound.Source = gameObject.AddComponent<AudioSource>();
                if (!soundsMap.ContainsKey(sound.name))
                {
                    soundsMap.Add(sound.name, sound);
                }
            }
        }

        private void Start()
        {
            PlayAudio("Menu");
        }

        public void PlayAudio(string audioName)
        {
            if (!soundsMap.TryGetValue(audioName, out Sound audio))
            {
                Debug.LogError($"Audio {audioName} not found");
                return;
            }

            audio.Source.Play();
        }

        public void StopAudio(string audioName)
        {
            if (!soundsMap.TryGetValue(audioName, out Sound audio))
            {
                Debug.LogError($"Audio {audioName} not found");
                return;
            }
            audio.Source.Stop();
        }

        public override void OnPlay()
        {
            if (soundsMap == null)
            {
                return;
            }

            StopAudio("Menu");
            PlayAudio("InGame");
        }

        public override void OnPause()
        {
            if (soundsMap == null)
            {
                return;
            }
            StopAudio("InGame");
            PlayAudio("Menu");
        }

        public void ToggelMuteAllAudio()
        {
            foreach (var sound in soundsMap.Values)
            {
                sound.ToggleMuteAudio();
            }
        }
    }
}
