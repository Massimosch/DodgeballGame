using UnityEngine;

public class DodgeballDamage : MonoBehaviour
{
    private GameObject thrower; // Säilytetään tieto siitä, kuka heitti pallon
    public Transform outOfGamePosition;
    private static float yOffset = 0f;

    public void SetThrower(GameObject thrower)
    {
        this.thrower = thrower;
    }

    public void HandleCollision(GameObject other)
    {
        if (other.CompareTag("Wall"))
        {
            HandleWallHit();
        }
            else if (other.CompareTag("Player"))
            {
                HandlePlayerHit(other);
            }
                else if (other.CompareTag("Enemy"))
                {
                    HandleEnemyHit(other);
                }
    }

    void HandleWallHit()
    {
        Destroy(gameObject);
    }

    void HandlePlayerHit(GameObject player)
    {
        if (thrower != null && thrower.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    void HandleEnemyHit(GameObject enemy)
    {
        Vector3 newPosition = outOfGamePosition.position;
        newPosition.y -= yOffset;
        enemy.transform.position = newPosition;
        yOffset += 1.5f;
    }
}
