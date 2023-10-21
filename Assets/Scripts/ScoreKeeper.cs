using TMPro;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private CookieParticle cookieParticlePrefab;
    private int _score;

    private void OnMouseDown()
    {
        AddScore();

        SpawnCookiePrefab();
    }

    private void AddScore()
    {
        _score++;
        scoreText.text = "Очки: " + _score;
    }
    
    private void SpawnCookiePrefab()
    {
        var randomX = Random.Range(-10f, 10f);
        var randomY = Random.Range(-1f, 3f);
        Instantiate(cookieParticlePrefab, new Vector3(randomX, randomY, 0), Quaternion.identity, transform);
    }
}