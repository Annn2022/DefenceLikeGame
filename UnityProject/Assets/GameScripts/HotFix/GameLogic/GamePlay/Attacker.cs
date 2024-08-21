using System.Collections.Generic;
using System.Linq;
using GamePlay.framework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameLogic.GamePlay
{
    public class Attacker:DragableObject, ISynthesize
    {
        /// <summary>
        /// 每种类型的攻击者单独id
        /// </summary>
        [SerializeField]
        private uint id;

        public uint ID
        {
            get { return id; }
        }

        public PlacementGrid m_PlacementGrid;
        public string Bulletpath;

        public bool CheckSynthesize(uint _id)
        {
            if (id == _id && id < 5)
            {
                return true;
            }

            return false;
        }

        protected override void OnEndDrag(Vector3 position, PointerEventData eventData)
        {
            base.OnEndDrag(position, eventData);
            
            if (eventData.pointerCurrentRaycast.gameObject.transform.TryGetComponent<PlacementGrid>(out var placementGrid) && placementGrid != m_PlacementGrid)
            {
                placementGrid.DoPutDown(this);
                GameManager.Instance.NextTurn();
            }

            else if (eventData.pointerCurrentRaycast.gameObject.transform.TryGetComponent<Attacker>(out var attacker))
            {
                var placementGrid1 = attacker.GetComponentInParent<PlacementGrid>();
                placementGrid1.DoPutDown(this);
                GameManager.Instance.NextTurn();
            }
        }

        /// <summary>
        /// 每次移动，合成或换位置时触发
        /// 所有植物发射子弹
        /// </summary>
        public void NextTurn()
        {
            if (GameManager.Instance.MainTurnNum % 2 == 0)
            {
                SpawnBullet(m_PlacementGrid.RowId, ID);
            }
        }
        
        public void SpawnBullet(int RowId,uint AttackerId)
        {
            ManagerLocator.Get<EnemyGridManager>().EnemyGrid_Dic[RowId].Last().SpawnBullet(Bulletpath);
            
        }
        
    }
    
}