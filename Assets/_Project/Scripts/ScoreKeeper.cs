using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Project.Scripts
{
    public class ScoreKeeper : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CookieParticle cookieParticlePrefab;
        [SerializeField] private SpriteRenderer grannySpriteRenderer;
        [SerializeField] public AudioClip scorePlusSound;
        [SerializeField] public AudioClip scoreMinusSound;
        [SerializeField] public AudioClip levelPassedSound;
        [SerializeField] public AudioClip winningTheGameSound;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text clickCounterText;
        [SerializeField] private TMP_Text scorePerClickText;
        [SerializeField] private Image endGamePanel;
        
        [Header("Config")]
        [SerializeField] private uint scoreToWin1 = 100;
        [SerializeField] private uint scoreToWin2 = 200;
        [SerializeField] private uint scoreToWin3 = 300;
        [SerializeField] private uint scoreSpeedUpStep = 20;
        [Range(0f, 100f)]
        [SerializeField] private float grannyVisibilityTime = 5f;
        [Range(0f, 100f)]
        [SerializeField] private float grannyInvisibilityTime = 5f;
    
        public bool GrannyVisible { get; private set; }
        
        private Vector3 _grannyOriginalPosition;
        private Transform _grannyTransform;
        private int _score;
        private int _scoreIncrement;
        private uint _level = 1;
        private uint _lastScoreStep;
        private uint _counter;
        private float _timer;
        private bool _firstTimeGrannyVisible;
        private bool _timerEnabled;
        private bool _isGameOver;
        private bool _changeLevelTo2;
        private bool _changeLevelTo3;

        #region MONO

        private void Start()
        {
            _grannyTransform = grannySpriteRenderer.transform;
            _grannyOriginalPosition = _grannyTransform.position;
            scoreText.text = "Очки: 0";
            endGamePanel.gameObject.SetActive(false);
            grannySpriteRenderer.enabled = false;
        }
        
        private void Update()
        {
            TimerTick();
            CheckRightClick();
            if (!GrannyVisible) return;
            var xPosition = _grannyOriginalPosition.x + Mathf.Sin(Time.time) * 0.5f;
            var localPosition = _grannyTransform.localPosition;
            localPosition = new Vector3(xPosition, localPosition.y, localPosition.z);
            _grannyTransform.localPosition = localPosition;
        }

        public void OnMouseDown()
        {
            if (GrannyVisible && _level == 1 || _isGameOver)
            {
                return;
            }

            if (GrannyVisible && _level > 1)
            {
                CheckGrannyClick();
            }
            else
            {
                IncreaseScoreIncrement();
                AddScore();
                StartGrannyLogic(); 
                NewLine();
                SpawnCookie();
                CheckLevel();
                transform.localScale *= 0.8f;
                Invoke(nameof(EnlargeCookie), 0.1f);
            }
        }

        #endregion
        
        private void EnlargeCookie()
        {
            transform.localScale /= 0.8f;
        }
        
        private void CheckGrannyClick()
        {
            switch (GrannyVisible)
            {
                case true when _level == 2:
                    SubScore("Клик при бабушке на уровне 2", 10);
                    break;
                case true when _level == 3:
                    SubScore("Клик при бабушке на уровне 3", 20);
                    break;
            }
        }
    
        private void CheckRightClick()
        {
            if (Input.GetMouseButtonDown(1) && _level > 1)
            {
                switch (_level)
                {
                    case 2:
                        SubScore("Правый клик на уровне 2", 15);
                        break;
                    case 3:
                        SubScore("Правый клик на уровне 3", 30);
                        break;
                }
            }
            scoreText.text = "Очки: " + _score;
        }
        
        private void SubScore(string reason, int number)
        {
            audioSource.PlayOneShot(scorePlusSound);
            Debug.Log(reason + ": -"  + number + " очков!");
            _score -= number;
            scoreText.text = "Очки: " + _score;
        }
        
        private void TimerTick()
        {
            if (!_timerEnabled)
            {
                return;
            }

            _timer += Time.deltaTime;
            TrySetActiveGranny();
        }
        
        private void TrySetActiveGranny()
        {
            switch (GrannyVisible)
            {
                case true when _timer >= grannyVisibilityTime:
                    SetActiveGranny(false);
                    break;
                case false when _timer >= grannyInvisibilityTime:
                    SetActiveGranny(true);
                    break;
            }
        }
        
        private void SetActiveGranny(bool value)
        {
            GrannyVisible = value;
            grannySpriteRenderer.enabled = value;
            ResetTimer();
        }
        
        private void ResetTimer()
        {
            _timer = 0;
        }
        
        private void NewLine()
        {
            if (_counter % 20 == 0 && _counter != 0)
            {
                scorePerClickText.text += "\n";
            }
        }
        
        private void CheckLevel()
        {
            if (_score >= scoreToWin1 && _score < scoreToWin2 && !_changeLevelTo2)
            {
                ChangeLevel(2);
            }
            else if (_score >= scoreToWin2 && _score < scoreToWin3 && !_changeLevelTo3)
            {
                ChangeLevel(3);
            }
            else if (_score >= scoreToWin3)
            {
                _isGameOver = true;
                audioSource.PlayOneShot(winningTheGameSound);
                _timerEnabled = false;
                endGamePanel.gameObject.SetActive(true);
                clickCounterText.text = $"Кол-во кликов: {_counter}";
            }
        }
        
        private void ChangeLevel(uint number)
        {
            if (number == _level)
            {
                return;
            }
            levelText.text = "Уровень " + number;
            _level = number;
            audioSource.PlayOneShot(levelPassedSound);
            levelText.fontSize = 110;
            Invoke(nameof(ResetLevelTextSize), 0.3f);
        }

        private void ResetLevelTextSize()
        {
            levelText.fontSize = 100;
        }
        
        private void IncreaseScoreIncrement()
        {
            if (_score < _lastScoreStep) return;
            _scoreIncrement++;
            _lastScoreStep += scoreSpeedUpStep;
        }

        private void AddScore()
        {
            audioSource.PlayOneShot(scorePlusSound);
            _score += _scoreIncrement;
            _counter++;
            
            scoreText.text = "Очки: " + _score;
            scorePerClickText.text += " + " + _scoreIncrement;
        }

        private void SpawnCookie()
        {
            var randomX = Random.Range(-10f, 10f);
            var randomY = Random.Range(-1f, 1f);

            Instantiate(cookieParticlePrefab, new Vector3(randomX, randomY, 0),
                Quaternion.identity);
        }

        private void StartGrannyLogic()
        {
            if (_score < 20 || _timerEnabled) return;
            SetActiveGranny(true);
            _timerEnabled = true;
        }
    }
}