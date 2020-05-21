using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vevidi.FindDiff.GameLogic
{
    public class SoundsManager : MonoBehaviour
    {
        [System.Serializable]
        public enum eSoundType
        {
            Click,
            Correct,
            Wrong,
            Win,
            Lose
        }

        [System.Serializable]
        public class SoundItem
        {
            public eSoundType type;
            public AudioClip clip;
        }

        public static SoundsManager Instance { get; private set; }

#pragma warning disable 0649
        [SerializeField]
        private List<SoundItem> soundsList;
        [SerializeField]
        private AudioClip[] backSounds;
        [SerializeField]
        private AudioSource sourceOneShot;
        [SerializeField]
        private AudioSource sourceBackground;
#pragma warning restore 0649

        private Coroutine backSoundCoroutine;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }

        public void PlaySound(eSoundType type)
        {
            //if (SudokuController.Instance.gameData.SoundOn)
            //{
                sourceOneShot.Stop();
                sourceOneShot.PlayOneShot(soundsList.Find(x => x.type == type).clip);
            //}
        }

        public void PlayBackgroundSound()
        {
            //if (SudokuController.Instance.gameData.SoundOn)
            //{
                if (backSoundCoroutine != null)
                    StopCoroutine(backSoundCoroutine);
                backSoundCoroutine = StartCoroutine(BackgroundSoundCoroutine());
            //}
        }

        private IEnumerator BackgroundSoundCoroutine()
        {
            while (true)
            {
                int ID = Random.Range(0, 3);
                sourceBackground.clip = backSounds[ID];
                sourceBackground.volume = 0.15f;
                sourceBackground.Play();
                yield return new WaitForSeconds(sourceBackground.clip.length);
                sourceBackground.Stop();
            }
        }

        public void StopSoundOneShot() => sourceOneShot.Stop();

        public void StopBackgroundSound()
        {
            if (backSoundCoroutine != null)
                StopCoroutine(backSoundCoroutine);
            sourceBackground.Stop();
        }
    }
}