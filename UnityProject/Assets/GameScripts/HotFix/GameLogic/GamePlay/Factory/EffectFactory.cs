using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameLogic.Common.Particle;
using TEngine;
using UnityEngine;

namespace GameLogic.GamePlay.Factory
{
    public class EffectFactory: IFactory
    {
        public async UniTask<GameObject> CreateEffect(string effectType, Vector3 position)
        {
            var go = await GameModule.Resource.LoadGameObjectAsync(effectType);
            go.AddComponent<AutoDestroyParticle>();
            go.transform.position = position;
            return go;
        }
        
    }
    

    
    public static class EffectType
    {
        //合成特效
        public static string Synthesize = "PAR_VM2SSBurstChunksA_0000";
        public static string Hit = "PAR_VM2SSBurstCircleA_0000";
        public static string Explore = "Effect_Explore01";

    }
}