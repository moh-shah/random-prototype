using CardMatch.Models;
using UnityEngine;

namespace CardMatch.Gameplay
{
    public class AudioManager : MonoBehaviour
    {
        public AudioSource audioSource;
        private GameSettings _settings;

        public void Setup(GameSettings settings)
        {
            _settings = settings;
        }

        public void OnCardClicked() => audioSource.PlayOneShot(_settings.onCardClicked);
        public void OnMatch() => audioSource.PlayOneShot(_settings.onCardsMatch);
        public void OnMisMatch() => audioSource.PlayOneShot(_settings.onCardsMisMatch);
        public void OnGameEnded() => audioSource.PlayOneShot(_settings.onBoardCleared);
    }
}