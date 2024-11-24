using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollision : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.volume = other.gameObject.GetComponent<PlayerMovement>().MoveSpeed / 100;
                _audioSource.Play();
            }
        }
    }
}
