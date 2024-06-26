using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;       
    public Vector3 offset;         

    void Start()
    {
        
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        
        Vector3 newPosition = player.position + offset;
        newPosition.x = transform.position.x;
        newPosition.y = transform.position.y; 
        transform.position = newPosition;
    }
}
