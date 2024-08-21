using System;
using System.Linq;
using GameLogic.GamePlay.Factory;
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
                m_Text.text = Damage.ToString();
            }
        }

        public PlaceGrid m_PlacementGrid;
        public string        Bulletpath;

        private float bulletInterval = 3f;
        private float bulletTimer    = 0;

        public float Damage
        {
            get { return Mathf.Pow(2f,(float)id); }
        }
        
        [SerializeField]
        private TMP_Text m_Text;
        

        public bool CheckSynthesize(Animal_Attacker attacker)
        {
            if (ID == attacker.ID)
            {
                return true;
            }

            return false;
        }


        private void Update()
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

        protected override void OnEndDrag(Vector3 position, PointerData pointerData)
        {
            base.OnEndDrag(position, pointerData);
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
        }


        public void SpawnBullet()
        {
            var go =  ManagerLocator.Get<FactoryManager>().Get<BulletFactory>().CreateBullet(ID);
            go.transform.position = transform.position;
        }
    }
}