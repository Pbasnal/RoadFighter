using System;
using UnityEngine;

namespace UnityCode
{
    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;

        [Range(0, 1)]
        public float volume;
        [Range(0.3f, 3f)]
        public float pitch;
        public bool loop;

        public AudioSource Source
        {
            get { return _source; }
            set
            {
                _source = value;
                _source.volume = volume;
                _source.pitch = pitch;
                _source.loop = loop;
                _source.clip = clip;
            }
        }

        private AudioSource _source;

        public void ToggleMuteAudio()
        {
            _source.mute = !_source.mute;
        }
    }
}
