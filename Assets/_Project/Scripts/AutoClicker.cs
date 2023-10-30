using UnityEngine;

namespace _Project.Scripts
{
    public class AutoClicker : MonoBehaviour 
    {
        [SerializeField] private int clicksCount = 10;
        [SerializeField] private float clickDelay = 0.05f;
        [SerializeField] private bool isAvailableWhenGranny; 
        [SerializeField] private ScoreKeeper scoreKeeper;

        private bool _isActive;
        private float _timer;
        private int _clicksCompleteCount;

        private void Start()
        {
            _isActive = false;
            _timer = 0;
            _clicksCompleteCount = 0;
        }

        [ContextMenu("StartClicks")]
        private void StartClicks()
        {
            if (_isActive)
            {
                return;
            }

            _isActive = true;
            _timer = 0;
            _clicksCompleteCount = 0;
        }

        private void Update()
        {
            DoClicks();
        }

        private void DoClicks()
        {
            if (!_isActive)
            {
                return;
            }

            _timer += Time.deltaTime;

            if (_timer >= clickDelay)
            {
                _timer -= clickDelay;

                if(!isAvailableWhenGranny && scoreKeeper.GrannyVisible)
                {
                    return;
                }

                scoreKeeper.OnMouseDown();
                _clicksCompleteCount++;
            }

            if (_clicksCompleteCount >= clicksCount)
            {
                _isActive = false;
            }
        }
    }
}