using UnityEngine;

namespace Nouranium
{
    public class SessionTracker : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            gameManager.StartSession();
            gameManager.LoadLevel();
        }

        private void OnApplicationQuit()
        {
            gameManager.EndSession();
        }
    }
}