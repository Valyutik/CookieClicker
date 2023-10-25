using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Project.Scripts
{
    public class ScoreKeeper
    {
        private ScoreView _scoreText;
        private CounterView _clickCounterText;
        private ScorePerClickView _scorePerClickText;
        private EndGamePanelView _endGamePanel;
        private CookieParticle _cookieParticlePrefab;
    
        private int _score;
        private int _counter;

        private ScoreKeeper()
        {
            _scoreText.text = "Очки: " + _score;
        
            _endGamePanel.gameObject.SetActive(false);
        }

        private void OnMouseDown()
        {
            AddScore();

            ShowScore();

            SpawnCookiePrefab();
        }
    
        private void AddScore()
        {
            _counter++;
            _clickCounterText.text = $"Клики: {_counter}";

            switch (_score)
            {
                case < 20:
                    _score++;
                    _scorePerClickText.text += "+1 ";
                    break;
                case < 40:
                    _score += 2;
                    _scorePerClickText.text += "+2 ";
                    break;
                case < 60:
                    _score += 3;
                    _scorePerClickText.text += "+3 ";
                    break;
                case < 80:
                    _score += 4;
                    _scorePerClickText.text += "+4 ";
                    break;
                case < 100:
                    _score += 5;
                    _scorePerClickText.text += "+5 ";
                    break;
            }
            
            if (_counter % 20 == 0)
            {
                _scorePerClickText.text += "\n";
            }
        }

        private void ShowScore()
        {
            if (_score < 100)
            {
                _scoreText.text = "Очки: " + _score;
            }
            else
            {
                _scoreText.text = "Очки: " + _score;
                _endGamePanel.gameObject.SetActive(true);
            }
        }
    
        private void SpawnCookiePrefab()
        {
            var randomX = Random.Range(-10f, 10f);
            var randomY = Random.Range(-1f, 3f);
            Object.Instantiate(_cookieParticlePrefab, 
                new Vector3(randomX, randomY, 0), Quaternion.identity, transform);
        }
    }
}