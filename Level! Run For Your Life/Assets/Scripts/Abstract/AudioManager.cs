using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Backrooms
{
    public class AudioManager : Singleton<AudioManager>
    {
        public void PlayOneShotSound(AudioSource _audioSource,AudioClip audioClip) 
        {
            _audioSource.PlayOneShot(audioClip);
        }
        public void PlayLoopSound(AudioSource _audioSource, AudioClip audioClip = null,bool stopPreviusSound = false) 
        {
            if (!_audioSource.isPlaying) 
            {
                _audioSource.volume = 1;
                _audioSource.loop = true;
                if(audioClip != null) _audioSource.clip = audioClip;
                _audioSource.Play();
            }
            else if (stopPreviusSound) 
            {
                _audioSource.Stop();
                _audioSource.volume = 1;
                _audioSource.loop = true;
                if (audioClip != null) _audioSource.clip = audioClip;
                _audioSource.Play();
            }
        }
        public void StopSound(AudioSource _audioSource,float _duration = 1) 
        {
            if(Mathf.Approximately(_audioSource.volume,1))
                _audioSource.DOFade(0, _duration).OnComplete(() => CompleteSound(_audioSource));         
        }
        void CompleteSound(AudioSource _audioSource) 
        {
            _audioSource.Stop();
        }
    }
}
