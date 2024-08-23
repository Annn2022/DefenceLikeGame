using System;
using System.Linq;
using GameLogic.GamePlay.Factory;
using GameLogic.GamePlay.UI;
using GamePlay.framework;
using TMPro;
using UnityEngine;

namespace GameLogic.GamePlay
{
    public class Animal_Attacker : DragHandler_SceneObject
    {
        /// <summary>
        /// 每种类型的攻击者单独id
        /// </summary>
        [SerializeField]
        private uint id;

        public uint ID
        {
            get { return id; }
            set
            {
                id = value;
                if (m_Text!= null)
                {
                    m_Text.text = Damage.ToString();

                }
            }
        }
        
        public int animalType;


        public PlaceGrid m_PlacementGrid;
        public string        Bulletpath;

        public float bulletInterval = 2f;
        private float bulletTimer    = 0;

        /// <summary>
        /// 发射子弹的伤害
        /// </summary>
        public float Damage;

            [SerializeField]
        private TMP_Text m_Text;
        

        public bool CheckSynthesize(Animal_Attacker attacker)
        {
            if (ID == attacker.ID && attacker.animalType == animalType)
            {
                return true;
            }

            return false;
        }


        protected  virtual void Update()
        {
            if (GameManager.Instance.IsGameOver)
            {
                return;
            }

            if (isDragging)
            {
                bulletTimer = 0;
                return;
            }

            bulletTimer += Time.deltaTime;
            if (bulletTimer >= bulletInterval)
            {
                SpawnBullet();
                bulletTimer= 0;
            }
        }                                

        

        protected override void OnBeginDrag(Vector3 position, PointerData pointerData)
        {
            base.OnBeginDrag(position, pointerData);
            MainUIWindow.Instance.SetClearButton(true);
        }

        protected override void OnEndDrag(Vector3 position, PointerData pointerData)
        {
            base.OnEndDrag(position, pointerData);
            MainUIWindow.Instance.SetClearButton(false);

            if (pointerData.target == null)
            {
                return;
            }
            if (pointerData.target.transform.TryGetComponent<PlaceGrid>(out var placementGrid) && placementGrid != m_PlacementGrid)
            {
                placementGrid.DoPutDown(this);
                GameManager.Instance.NextTurn();

            }

            else if (pointerData.target.transform.TryGetComponent<Animal_Attacker>(out var attacker))
            {
                attacker.m_PlacementGrid.DoPutDown(this);
                GameManager.Instance.NextTurn();


            }

            else if (pointerData.target.transform.TryGetComponent<BulletClear>(out var bulletClear))
            {
                ManagerLocator.Get<PlaceGridManager>().RemoveAttacker(this);

            }
        }


        protected virtual void SpawnBullet()
        {
            //var go =  ManagerLocator.Get<FactoryManager>().Get<BulletFactory>().CreateBullet(ID);
            var go =  ManagerLocator.Get<FactoryManager>().Get<BulletFactory>().CreateBulletAsName(Bulletpath,ID);
            go.GetComponentInChildren<Bullet>().Damage =(int)Damage;
            go.GetComponentInChildren<Bullet>().Level =(int)ID;
            go.transform.position = transform.position;

            go.GetComponentInChildren<Bullet>().Init();
        }
    }
}