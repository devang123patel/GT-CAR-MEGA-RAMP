using UnityEngine;

public class EnemyHandlers : MonoBehaviour
{
    public float treeSpeed = 10f;
    
    void Update()
    {
        // Move the enemy based on treeSpeed
        transform.Translate(Vector3.left * treeSpeed * Time.deltaTime);
        
        // Destroy when off screen
        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }
}