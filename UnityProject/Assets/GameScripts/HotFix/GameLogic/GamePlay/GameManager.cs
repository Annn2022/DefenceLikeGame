using System;
using System.Diagnostics;
using Cysharp.Threading.Tasks;
using GameBase;
using GamePlay.framework;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace GameLogic.GamePlay
{
    /// <summary>
    /// 游戏的主流程控制器
    /// </summary>
    public class GameManager : SingletonBehaviour<GameManager>
    {
        private Camera mcamera;
        public Camera m_Camera
        {
            get
            {
                if (mcamera == null)
                {
                    mcamera = Camera.main;
                }

                return mcamera;
            }
        }
        private Camera uiCamera;

        public Camera m_UICamera {
            get
            {
                if (uiCamera == null)
                {
                    uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
                }

                return uiCamera;
            }
        }

        public bool IsGameOver;
        public GameState GameState{get;private set;}
        
        public float GamingTime {get; private set;}
        
        /// <summary>
        /// 游戏回合数，即总操作数
        /// </summary>
        public int MainTurnNum = 0;

        public  TMP_Text CoinText;
        private int      cointCount;
        public int CointCount
        {
            get => cointCount;
            set
            {
                cointCount = value;
                if (CoinText != null)
                {
                    CoinText.text = cointCount.ToString();
                    if (cointCount < spawnCost)
                    {
                        callButton.transform.GetComponentInChildren<TMP_Text>().color = Color.red;
                    }
                    else
                    {
                        callButton.transform.GetComponentInChildren<TMP_Text>().color = Color.black;
                    }
                    if (cointCount < technologyLevelCost)
                    {
                        levelButton.transform.GetComponentInChildren<TMP_Text>().color = Color.green;
                    }
                    else
                    {
                        levelButton.transform.GetComponentInChildren<TMP_Text>().color = Color.black;

                    }
                }
            }
        }

        public  int    technologyLevel;
        public  Button callButton;
        public  Button levelButton;
        private int    spawnCost = 10;
        private int    technologyLevelCost = 25;


        private void Start()
        {
            DelayInit().Forget();
            GameState = GameState.BeforeGame;
            CointCount = 100;
            callButton.transform.GetComponentInChildren<TMP_Text>().text = $"Spawn({spawnCost})";
            levelButton.transform.GetComponentInChildren<TMP_Text>().text = $"LevelUp({technologyLevelCost})";
            callButton.onClick.AddListener(() =>
            {
                if (CointCount >= spawnCost)
                {
                    CointCount -= spawnCost;
                    spawnCost += 10;
                    ManagerLocator.Get<AnimalManager>().SpawnAniaml();
                    callButton.transform.GetComponentInChildren<TMP_Text>().text = $"Spawn({spawnCost})";


                }
            });  
            levelButton.onClick.AddListener(() =>
            {
                if (CointCount >= technologyLevelCost)
                {
                    CointCount -= technologyLevelCost;
                    technologyLevelCost += 10;
                    technologyLevel++;
                    levelButton.transform.GetComponentInChildren<TMP_Text>().text = $"LevelUp({technologyLevelCost})";


                }
            });
        }

        private void Update()
        {
            if (GameState == GameState.InGame)
            {
                GamingTime += Time.deltaTime;
                
            }
        }

        private async UniTask DelayInit()
        {
            await UniTask.Delay( TimeSpan.FromSeconds( 1f ));
            //await ManagerLocator.Get<PlacementGridManager>().RandomInit();
            GameStart();
            await ManagerLocator.Get<PlaceGridManager>().RandomInit();
            await GameFlow();
        }

        private void GameStart()
        {
            GameState = GameState.InGame;
        }
        /// <summary>
        /// 每次移动，合成或换位置时触发
        /// 所有植物发射子弹 所有僵尸前进一格 生成新植物 生成新僵尸
        /// </summary>
        public void NextTurn()
        {
            MainTurnNum++;
            Debug.Log("NextTurn");
            //ManagerLocator.Get<EnemyGridManager>().NextTurn();
            //ManagerLocator.Get<PlacementGridManager>().NextTurn();
            
            //realtime
           // ManagerLocator.Get<AnimalManager>().SpawnAniaml();
        }

        public async UniTask GameFlow()
        {
            
            // while (!IsGameOver)
            // {
            //     //每8秒 随机生成一个新动物
            //     //await UniTask.Delay(TimeSpan.FromSeconds(8f));
            //     //await ManagerLocator.Get<PlaceGridManager>().RondomNewAttacker();
            // }
        }
    }

   public enum GameState
    {
        BeforeGame,
        InGame,
        GamePause,

        GameOver,
        GameWin,
        GameExit,
    
    }
}