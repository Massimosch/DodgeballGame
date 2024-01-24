using UnityEngine;

public class DodgeballDamage : MonoBehaviour
{
    private GameObject thrower;
    private Dodgeball dodgeballScript;

    public Transform outOfGamePosition;
    private static float xOffset = 0f;

    void Start()
    {
        dodgeballScript = GetComponent<Dodgeball>();
    }

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
        dodgeballScript.thrownByPlayer = false;
        dodgeballScript.thrownByEnemy = false;
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
        if (dodgeballScript.thrownByPlayer)
        {
            Vector3 newPosition = outOfGamePosition.position;
            newPosition.x -= xOffset;
            xOffset += 1.5f;
            enemy.transform.position = newPosition;
        }
    }
}
