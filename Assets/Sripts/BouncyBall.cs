using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Обов'язково для перезавантаження сцени

public class BouncyBall : MonoBehaviour
{
    public float minY = -5.5f;
    public float maxVelocity = 15f;

    Rigidbody2D rb;

    public int score = 0;
    public int lives = 5;

    public TextMeshProUGUI scoreTxt;
    public GameObject[] livesImage;
    public GameObject gameOverPanel;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Переконуємося, що час іде, коли сцена завантажується заново
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (transform.position.y < minY)
        {
            lives--;

            if (lives > 0)
            {
                // Якщо життя ще є — ховаємо іконку і повертаємо м'яч
                livesImage[lives].SetActive(false);
                transform.position = Vector3.zero;
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                // Ховаємо останнє серце (індекс 0)
                if (livesImage.Length > 0) livesImage[0].SetActive(false);
                GameOver();
            }
        }

        if (rb.linearVelocity.magnitude > maxVelocity)
        {
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxVelocity);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Brick"))
        {
            Destroy(collision.gameObject);
            score += 25;
            scoreTxt.text = score.ToString("00000");
        }
    }

    void GameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        // Зупиняємо м'яч
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        // Автоматично перезавантажуємо всю сцену через 2 секунди
        // Це автоматично поверне 5 життів та всі цеглинки
        Invoke("AutoRestart", 2f);
    }

    void AutoRestart()
    {
        // Завантажує поточну сцену з нуля
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}