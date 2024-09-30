using System.Collections;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        private AudioSource _source;
        private AudioService _audioService;

        public AudioSource Source
        {
            get
            {
                _source ??= GetComponent<AudioSource>();
                return _source;
            }
        }

        public void Play(AudioClipData data)
        {
            Source.loop = data.Loop;
            Source.clip = data.Clip;
            Source.outputAudioMixerGroup = data.Group;
            Source.Play();
            StartCoroutine(CheckAudioFinished(data.Clip.length));
        }

        private IEnumerator CheckAudioFinished(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }
    }
}