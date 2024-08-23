using Cysharp.Threading.Tasks;
using GameLogic.GamePlay.Factory;
using GamePlay.framework;
using UnityEngine;

namespace GameLogic.GamePlay
{
    public class PlaceGrid : MonoBehaviour
    {
         public Animal_Attacker m_Attacker;

        /// <summary>
        /// 格子在第几排
        /// </summary>
        public int RowId;

        public void DoPutDown(Animal_Attacker attacker)
        {
            if (m_Attacker != null)
            {
                if (m_Attacker.CheckSynthesize(attacker))
                {
                    Debug.Log("合成成功");
                    var newId = m_Attacker.ID+1;
                    ManagerLocator.Get<PlaceGridManager>().RemoveAttacker(attacker);
                    ManagerLocator.Get<PlaceGridManager>().RemoveAttacker(m_Attacker);
                    ManagerLocator.Get<PlaceGridManager>().CreateNewAttacker(newId, this,attacker.animalType);
                    ManagerLocator.Get<FactoryManager>().Get<EffectFactory>().CreateEffect(EffectType.Synthesize, transform.Find("CenterPoint").position).Forget();
                }
                else
                {
                    ReplaceAttacker(attacker);
                }
            }
            else
            {
                PlaceAttacker(attacker);
            }
        }
        
        public void PlaceAttacker(Animal_Attacker attacker)
        {
            m_Attacker = attacker;
            if (attacker.m_PlacementGrid != null)
            {
                ManagerLocator.Get<PlaceGridManager>().ClearGrid(attacker.m_PlacementGrid);

            }
            SetAttackerGrid(attacker,this);
        }
        
        public void ReplaceAttacker(Animal_Attacker attacker)
        {
            Debug.Log($"交换位置  {m_Attacker.ID}  {attacker.ID}");
            var targetParent = attacker.m_PlacementGrid;
            var targetAttacker = m_Attacker;
            SetAttackerGrid(attacker, this);
            SetAttackerGrid(targetAttacker, targetParent);

        }
        
        public void SetAttacker(Animal_Attacker attacker)
        {
            m_Attacker = attacker;
            if (attacker != null)
            {
                attacker.m_PlacementGrid = this;

            }
        }

        private void SetAttackerGrid(Animal_Attacker attacker,PlaceGrid placement)
        {
            var parent = placement.transform.Find("SpawnPoint");
            if (parent == null)
            {
                parent = placement.transform;
            }
            attacker.transform.SetParent(parent, false);
            if (attacker.transform.TryGetComponent<RectTransform>(out var rect))
            {
                rect.anchoredPosition3D = Vector3.zero;
                rect.localScale = Vector3.one;
            }
            else
            {
                attacker.transform.localPosition = Vector3.back;
            }
            
            ManagerLocator.Get<PlaceGridManager>().PlaceGrid(placement,attacker);
        }
    }
}