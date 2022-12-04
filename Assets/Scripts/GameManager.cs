using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

namespace SkinColorRunner.Manager
{
    public class GameManager : MonoBehaviour
    {
        #region Variables
        public static GameManager Instance;
        private bool gameStarted = false;

        // events
        public static event Action GameStart;

        [Header("Buttons")]
        [Space(3)]
        [SerializeField] private Button startButton;
        [SerializeField] private Button restartButton;

        [Space(5)]

        [Header("Panels")]
        [Space(3)]
        [SerializeField] private GameObject startPanel;
        [SerializeField] private GameObject gamePanel;

        [Space(5)]

        [Header("Text")]
        [Space(3)]
        [SerializeField] private TextMeshProUGUI timeText;

        [Space(5)]

        [Header("Obstacles")]
        [Space(3)]
        [SerializeField] private List<GameObject> obstacles = new();

        [Space(5)]

        [Header("Door")]
        [Space(3)]
        [SerializeField] private GameObject doorPrefab;

        // timer variables
        private int timerSecond = 0;
        private float timerRate = 0f;
        #endregion

        private void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            gameStarted = false;
            SetButtonListeners();
        }

        // Update is called once per frame
        void Update()
        {
            if (gameStarted)
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
            gameStarted = true;
            startPanel.SetActive(false);
            gamePanel.SetActive(true);

            GameStart?.Invoke();
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
            }

            timeText.text = timerSecond.ToString();
        }
    }
}