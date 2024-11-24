using DefaultNamespace;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip scoreClip;
    [SerializeField] private AudioClip healingClip;
    
    [SerializeField] private EItemType itemType;
    
    public EItemType ItemType => itemType;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            sfxSource.clip = itemType == EItemType.Healing ? healingClip : scoreClip;    
            sfxSource.Play();
            
            Destroy(gameObject);
        }
    }
}