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

        // level generation
        private int objectPosZ = 10;
        private readonly int objectDistance = 10;
        private readonly int objectInARow = 2;
        private readonly int sectionAmount = 5;
        private readonly int objectPerSection = 3;
        private int objectCounter = 0;
        private Vector3 objectPosition = new(0f, 0.1f, 0f);
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
            GenerateLevel();
        }

        #region Level Generator
        private void GenerateLevel()
        {
            InstantiateObject(doorPrefab);

            for (int i = 0; i < sectionAmount; i++)
            {
                for (int j = 0; j < objectPerSection; j++)
                {
                    if (objectCounter != objectInARow)
                    {
                        System.Random rand = new();
                        int randIndex = rand.Next(obstacles.Count);
                        InstantiateObject(obstacles[randIndex]);
                        objectCounter++;
                    }
                    else
                    {
                        if (i != sectionAmount - 1)
                        {
                            InstantiateObject(doorPrefab);
                        }

                        objectCounter = 0;
                    }
                }
            }
        }

        private void InstantiateObject(GameObject gameObject)
        {
            Vector3 position = objectPosition + Vector3.forward * objectPosZ;
            Instantiate(gameObject, position, Quaternion.identity);
            objectPosZ += objectDistance;
        }
        #endregion

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

        // Update is called once per frame
        void Update()
        {
            if (gameStarted)
            {
                Timer();
            }
        }

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