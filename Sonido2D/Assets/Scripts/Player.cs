using System;
using Cinemachine;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private int playerHp;
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float jumpForce = 7.5f;

    [Header("UI")]
    [SerializeField] private TMP_Text scoreTxt;
    [SerializeField] private TMP_Text hpTxt;

    [Header("SFX")]
    [SerializeField] private AudioClip[] stepSounds; 
    [SerializeField] private AudioClip landingSound;
    [SerializeField] private AudioClip dmgSound;

    private float _horizontalMovement;
    private Vector3 _direction;
    private bool _isJumping;
    private bool _isGrounded;

    private int _collectedItems;
    private bool _receivingDamage;
    
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private AudioSource _audioSource;
    private CinemachineImpulseSource _impulseSource;
    private Random _random;

    private static readonly int IsWalking = Animator.StringToHash("isWalking");

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _random = new Random();
        
        hpTxt.text = $"Salud: {playerHp}"; 
    }

    private void Update()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal");

        if (_horizontalMovement != 0 & !_isJumping)
        {
            _animator.SetBool(IsWalking, true);
            _spriteRenderer.flipX = _horizontalMovement > 0;
            
            _direction = new Vector3(_horizontalMovement, 0, 0).normalized;
        }
        else
            _animator.SetBool(IsWalking, false);
        
        if (Input.GetKeyDown(KeyCode.Space) && !_isJumping)
            _isJumping = true;
        
    }

    private void FixedUpdate()
    {
        if (_horizontalMovement != 0 && _isGrounded)
        {
            _rigidbody2D.MovePosition(transform.position + _direction * (moveSpeed * Time.fixedDeltaTime));

            if (!_audioSource.isPlaying)
            {
                _audioSource.clip = stepSounds[_random.Next(0, stepSounds.Length - 1)];
                _audioSource.Play();
                
            }
        }

        if (_isJumping && _isGrounded)
        {
            _isGrounded = false;
            _rigidbody2D.AddForce(new Vector2(_horizontalMovement / 2, 1) * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log($"<{gameObject.name}> has collided with <{other.gameObject.name}>");

        var otherGO = other.gameObject;
        
        if (otherGO.CompareTag("Ground"))
        {
            _isJumping = false;
            _isGrounded = true;
            _rigidbody2D.velocity = Vector2.zero; // Cancel any remaining speed
            
            _audioSource.clip = landingSound;
            _audioSource.Play();
        }

        if (otherGO.CompareTag("Item"))
        {
            if (otherGO.GetComponent<Item>().ItemType == EItemType.Score)
            {
                _collectedItems++;
                scoreTxt.text = $"Puntuacion: {_collectedItems}";
            }

            if (otherGO.GetComponent<Item>().ItemType == EItemType.Healing)
            {
                playerHp = Mathf.Min(playerHp + 10, 100); // Asegura que la vida no supere 100
                hpTxt.text = $"Salud: {playerHp}";
            }
        }

        if (otherGO.CompareTag("Enemy"))
        {
            _impulseSource.GenerateImpulse();
            _audioSource.clip = dmgSound;
            _audioSource.Play();
            playerHp = Mathf.Max(playerHp - 20, 0); // Asegura que la vida no baje de 0
            hpTxt.text = $"Salud: {playerHp}";
        }

        if (otherGO.CompareTag("SafeZone"))
        {
            Debug.Log($"<{gameObject.name}> ha entrado en la zona segura:  <{other.gameObject.name}>");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("SafeZone"))
        {
            Debug.Log($"<{gameObject.name}> ha entrado en la zona segura:  <{other.gameObject.name}>");
            if (playerHp < 100)
            {
                playerHp = 100;
                hpTxt.text = $"Salud: {playerHp}";
            }
        }
    }
}
