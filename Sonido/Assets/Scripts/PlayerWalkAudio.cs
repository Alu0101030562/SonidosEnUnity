using UnityEngine;
using Random = System.Random;

public class PlayerWalkAudio : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;

    [SerializeField] private AudioClip[] audioClips;
    
    private AudioSource _audioSource;
    private Random _random;
    
    public float MoveSpeed => moveSpeed;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _random = new Random();
    }

    void Update()
    {
        var horizontalMovement = Input.GetAxis("Horizontal");
        var verticalMovement = Input.GetAxis("Vertical");

        if (horizontalMovement != 0 || verticalMovement != 0)
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.clip = audioClips[_random.Next(0, audioClips.Length - 1)];
                _audioSource.Play();
            }
            transform.Translate(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
        }
        
    }
}
