using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts
{
    public sealed class RestarterGame : MonoBehaviour
    {
        public void Restart()
        {
            SceneManager.LoadScene(0);
        }
    }
}