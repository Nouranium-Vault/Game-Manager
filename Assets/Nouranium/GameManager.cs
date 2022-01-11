using ScriptableObjectArchitecture;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Nouranium
{
    [CreateAssetMenu(fileName = "GameManager.asset", menuName = "New Game Manager", order = 0)]
    public class GameManager : ScriptableObject
    {
        [SerializeField] private ProgressData progressData;
        [SerializeField] private List<SceneReference> levels;

        [Header("Events (Optional)")]
        [SerializeField] private UnityEvent onGameLaunched;

        [SerializeField] private UnityEvent<int, float> onSessionStarted;
        [SerializeField] private UnityEvent<int, float> onSessionEnded;
        [SerializeField] private UnityEvent<int> onLoadedLevel;
        [SerializeField] private UnityEvent<int> onWinLevel;
        [SerializeField] private UnityEvent<int> onLoseLevel;

        private readonly string currentLevel = "currentLevel";
        private readonly string sessionNo = "sessionNo";
        private readonly string startSessionTime = "startSessionTime";
        private readonly string endSessionTime = "endSessionTime";

        public int level => progressData.GetIntParameter("currentLevel") + 1;

        public void StartSession()
        {
            progressData.LoadProgression();
            int sessionNum = progressData.GetIntParameter(sessionNo);

            if (sessionNum == 0)
            {
                onGameLaunched.Invoke();
            }

            progressData.IncreaseIntParameter(sessionNo, 1);
            float offlineTime = (float)(DateTime.Now.ToUniversalTime() - progressData.GetDateTimeParameter(endSessionTime)).TotalMinutes;

            onSessionStarted.Invoke(sessionNum, offlineTime);

            progressData.SetDateTimeParameter(startSessionTime, DateTime.Now);
        }

        public void EndSession()
        {
            int sessionNum = progressData.GetIntParameter(sessionNo);
            float sessionLength = (float)(DateTime.Now.ToUniversalTime() - progressData.GetDateTimeParameter(startSessionTime)).TotalMinutes;

            onSessionEnded.Invoke(sessionNum, sessionLength);

            progressData.SetDateTimeParameter(endSessionTime, DateTime.Now);
        }

        public void WinLevel()
        {
            int levelNo = progressData.GetIntParameter(currentLevel);

            //int levelInRange = levelNo % levels.Count;
            //progressData.SetWinState(levels[levelInRange].Value.SceneIndex, true);

            progressData.IncreaseIntParameter(currentLevel, 1);

            onWinLevel.Invoke(levelNo);
        }

        public void LoseLevel()
        {
            int levelNo = progressData.GetIntParameter(currentLevel);

            onLoseLevel.Invoke(levelNo);
        }

        private string GetLevelName(SceneReference sceneReference)
        {
            string[] path = sceneReference.Value.SceneName.Split('/');
            return path[path.Length - 1].Split('.')[0].Replace(' ', '_');
        }

        public void LoadLevel()
        {
            int levelNo = progressData.GetIntParameter(currentLevel);
            int levelInRange = levelNo % levels.Count;

            SceneManager.LoadSceneAsync(levels[levelInRange].Value.SceneIndex).completed += (op) => onLoadedLevel.Invoke(levelNo + 1);
        }
    }
}