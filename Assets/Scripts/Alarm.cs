using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))] 
[RequireComponent(typeof(Detector))]
public class Alarm : MonoBehaviour
{
    [SerializeField] private float _duration;

    private AudioSource _audioSource;
    private Detector _house;
    private Coroutine _coroutine;

    private void Awake()
    {
        _house = GetComponent<Detector>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0;
    }

    private void OnEnable()
    {
        _house.StateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        _house.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(bool state)
    {
        int value = Convert.ToInt32(state);

        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(ChangingSoundValue(value));
    }

    private IEnumerator ChangingSoundValue(int targetVolume)
    {
        bool isWorking = Convert.ToBoolean(targetVolume);

        if (isWorking == true)
        {
            _audioSource.Play();
        }

        while (_audioSource.volume != targetVolume)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, targetVolume, Time.deltaTime / _duration);
            yield return null;
        }

        if (isWorking == false)
        {
            _audioSource.Stop();
        }
    }
}
