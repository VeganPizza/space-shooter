using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    // 0 - triple shot
    // 1 - speed
    // 2 - shields
    [SerializeField]
    private int _powerUpID;

    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerUpID)
                {
                    case 0:
                        player.SetTripleShot();
                        break;
                    case 1:
                        player.SetSpeed();
                        break;
                    case 2:
                        player.SetShield();
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
