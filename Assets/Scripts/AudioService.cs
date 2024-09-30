using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioService : MonoBehaviour
    {
        [SerializeField] private AudioPlayer audioPlayerPrefab; 
        private Queue<AudioPlayer> _audioPlayerPool = new Queue<AudioPlayer>();

        public void PlayAudio(AudioClipData clipData)
        {

            AudioPlayer audioPlayer = GetAudioPlayer();
            audioPlayer.Play(clipData);
        }

        private AudioPlayer GetAudioPlayer()
        {
            if (_audioPlayerPool.Count > 0)
            {
                return _audioPlayerPool.Dequeue();
            }


            return Instantiate(audioPlayerPrefab);
        }
    }
}