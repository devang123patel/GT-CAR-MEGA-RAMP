using UnityEngine;

public class SnowTreeHandlers : MonoBehaviour
{
    public float snowTreeSpeed = 10f;
    
    void Update()
    {
        // Move the snow tree based on snowTreeSpeed
        transform.Translate(Vector3.left * snowTreeSpeed * Time.deltaTime);
        
        // Destroy when off screen
        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }
}