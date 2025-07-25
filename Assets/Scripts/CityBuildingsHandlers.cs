using UnityEngine;

public class CityBuildingsHandlers : MonoBehaviour
{
    public float towerSpeed = 10f;
    
    void Update()
    {
        // Move the building based on towerSpeed
        transform.Translate(Vector3.left * towerSpeed * Time.deltaTime);
        
        // Destroy when off screen
        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }
}