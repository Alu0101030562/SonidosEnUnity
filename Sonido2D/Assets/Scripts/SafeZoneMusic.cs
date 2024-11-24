using UnityEngine;

public class SafeZoneMusic : MonoBehaviour
{
    [SerializeField] private AudioSource ambientAudioSource;
    [SerializeField] private AudioSource ambientMusicAudioSource;
    [SerializeField] private AudioClip crowClip;
    [SerializeField] private AudioClip safeZoneClip;
    [SerializeField] private AudioClip ambientClip;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ambientMusicAudioSource.clip = ambientClip;
            ambientMusicAudioSource.Stop();
            ambientAudioSource.clip = safeZoneClip;
            ambientAudioSource.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ambientAudioSource.clip = crowClip;
            ambientAudioSource.Play();
            ambientMusicAudioSource.clip = ambientClip;
            ambientMusicAudioSource.Play();
        }
    }
}