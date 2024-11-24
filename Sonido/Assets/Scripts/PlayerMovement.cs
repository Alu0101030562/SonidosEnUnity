using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10.0f;
    
    public float MoveSpeed => moveSpeed;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            moveSpeed += 10.0f;
        }
        
        transform.Translate(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
    }
}