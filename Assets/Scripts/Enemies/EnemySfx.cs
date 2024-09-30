using Audio;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Enemy))]
    public class EnemySfx : MonoBehaviour
    {
        [SerializeField] private RandomContainer<AudioClipData> spawnClips;
        [SerializeField] private RandomContainer<AudioClipData> explosionClips;
        private Enemy _enemy;
        private AudioService _audioService;

        private void Reset() => FetchComponents();

        private void Awake()
        {
            FetchComponents();
            _audioService = FindObjectOfType<AudioService>(); // Find the AudioService instance
            if (_audioService == null)
            {
                Debug.LogError("AudioService not found in the scene!");
            }
        }

        private void FetchComponents()
        {
            _enemy ??= GetComponent<Enemy>();
        }

        private void OnEnable()
        {
            _enemy.OnSpawn += HandleSpawn;
            _enemy.OnDeath += HandleDeath;
        }

        private void OnDisable()
        {
            _enemy.OnSpawn -= HandleSpawn;
            _enemy.OnDeath -= HandleDeath;
        }

        private void HandleDeath()
        {
            PlayRandomClip(explosionClips);
        }

        private void HandleSpawn()
        {
            PlayRandomClip(spawnClips);
        }

        private void PlayRandomClip(RandomContainer<AudioClipData> container)
        {
            if (!container.TryGetRandom(out var clipData))
                return;

            // Call the AudioService to play the audio clip instead of instantiating AudioPlayer
            _audioService?.PlayAudio(clipData);
        }
    }
}