using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Project.Scripts
{
    public class ScoreKeeper : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text clickCounterText;
        [SerializeField] private TMP_Text scorePerClickText;
        [SerializeField] private Image endGamePanel;
        [SerializeField] private CookieParticle cookieParticlePrefab;
        [SerializeField] private SpriteRenderer grannySpriteRenderer;
    
        private int _score;
        private int _counter;
        private float _grannyTimer = 5f;
        private float _invisibleTimer = 5f;
        private bool _firstTimeGrannyVisible;

        private void Start()
        {
            scoreText.text = "Очки: " + _score;
        
            endGamePanel.gameObject.SetActive(false);
            
            grannySpriteRenderer.enabled = false;
        }
        
        private void Update()
        {
            switch (grannySpriteRenderer.enabled)
            {
                case true:
                {
                    _grannyTimer -= Time.deltaTime;
                    if (!(_grannyTimer <= 0)) return;
                    grannySpriteRenderer.enabled = false;
                    _grannyTimer = 5f;
                    break;
                }
                case false when _score >= 20:
                {
                    _invisibleTimer -= Time.deltaTime;
                    if (!(_invisibleTimer <= 0)) return;
                    grannySpriteRenderer.enabled = true;
                    _invisibleTimer = 5f;
                    break;
                }
            }
        }

        private void OnMouseDown()
        {
            AddScore();

            ShowScore();

            SpawnCookiePrefab();
        }
    
        private void AddScore()
        {

            if (grannySpriteRenderer.enabled) return;
            _counter++;
            
            switch (_score)
            {
                case < 20:
                    _score++;
                    scorePerClickText.text += "+1 ";
                    break;
                case < 40:
                    _score += 2;
                    scorePerClickText.text += "+2 ";
                    break;
                case < 60:
                    _score += 3;
                    scorePerClickText.text += "+3 ";
                    break;
                case < 80:
                    _score += 4;
                    scorePerClickText.text += "+4 ";
                    break;
                case < 100:
                    _score += 5;
                    scorePerClickText.text += "+5 ";
                    break;
            }
            
            if (_counter % 20 == 0)
            {
                scorePerClickText.text += "\n";
            }

            if (_score < 20 || _firstTimeGrannyVisible) return;
            grannySpriteRenderer.enabled = true;
            _firstTimeGrannyVisible = true;
        }

        private void ShowScore()
        {
            if (_score < 100)
            {
                scoreText.text = "Очки: " + _score;
            }
            else
            {
                scoreText.text = "Очки: " + _score;
                clickCounterText.text = $"Клики: {_counter}";
                endGamePanel.gameObject.SetActive(true);
                grannySpriteRenderer.enabled = false;
            }
        }
    
        private void SpawnCookiePrefab()
        {
            var randomX = Random.Range(-10f, 10f);
            var randomY = Random.Range(-1f, 3f);
            Instantiate(cookieParticlePrefab, 
                new Vector3(randomX, randomY, 0), Quaternion.identity, transform);
        }
    }
}