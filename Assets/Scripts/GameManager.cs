using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SkinColorRunner.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public bool GameStarted { get; private set; } // reach that from player controller

        [Header("Buttons")]
        [Space(3)]
        [SerializeField] private Button startButton;
        [SerializeField] private Button restartButton;

        [Space(5)]

        [Header("Panels")]
        [Space(3)]
        [SerializeField] private GameObject startPanel;
        [SerializeField] private GameObject gamePanel;

        private int timerSecond = 0;
        private float timerRate = 0f;

        private void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            GameStarted = false;
            SetButtonListeners();
        }

        // Update is called once per frame
        void Update()
        {
            if (GameStarted)
            {
                Timer();
            }
        }

        #region Button Click Listeners
        private void SetButtonListeners()
        {
            startButton.onClick.AddListener(StartClick);
            restartButton.onClick.AddListener(RestartClick);
        }

        private void StartClick()
        {
            GameStarted = true;
            startPanel.SetActive(false);
            gamePanel.SetActive(true);
        }

        private void RestartClick()
        {
            SceneManager.LoadScene("GameScene");
        }
        #endregion

        private void Timer()
        {
            timerRate += Time.deltaTime;
            if (timerRate>=1)
            {
                timerRate = 0f;
                timerSecond++;
                Debug.Log(timerSecond);
            }
        }
    }
}