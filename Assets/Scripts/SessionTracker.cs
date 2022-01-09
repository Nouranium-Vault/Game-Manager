using UnityEngine;

public class SessionTracker : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ProgressData progressData;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        progressData.LoadProgression();
        gameManager.StartSession();
        gameManager.LoadLevel();
    }

    private void OnApplicationQuit()
    {
        gameManager.EndSession();
    }
}