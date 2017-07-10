using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Ai
{
    public enum AiDifficulty
    {
        Easy,
        Normal,
        Hard,
    }

    public struct RandomProbability
    {
        public RandomProbability(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Min;
        public float Max;
        public float GetRandom()
        {
            return Random.Range(Min, Max);
        }
    }

    public class AiDifficultyController : Singleton<AiDifficultyController>
    {
        public bool IsInitialized { get; protected set; }
        public Dictionary<string, float> Probabilities { get; private set; }
        public Dictionary<string, float> RandomValues { get; private set; }
        
        private AiDifficulty difficulty;
        private Data.AiDifficultyData difficultyData;

        AiDifficultyController()
        {
            Probabilities = new Dictionary<string, float>();
            RandomValues = new Dictionary<string, float>();
        }

        protected override void Awake()
        {
            base.Awake();
        }

        public void Initialize()
        {
            if(IsInitialized == true)
            {
                return;
            }
            // 일단 임시로 Difficulty Easy로 설정
            difficulty = AiDifficulty.Easy;

            difficultyData = Resources.Load<Data.AiDifficultyData>("Data/Ai/AiDifficultyData_" + difficulty.ToString());
            difficultyData.Probabilities.GetDatas(Probabilities);
            difficultyData.RandomValues.GetDatas(RandomValues);
        }

        public float GetRawRandomValue(string statusId)
        {
            if (RandomValues.ContainsKey(statusId) == false)
            {
                Debug.LogError(string.Format("{0} status not found. Difficulty : {1}", statusId, difficulty.ToString()));
                return 0f;
            }

            return RandomValues[statusId];
        }
        
        public float GetRawProbability(string statusId)
        {
            if (Probabilities.ContainsKey(statusId) == false)
            {
                Debug.LogError(string.Format("{0} status not found. Difficulty : {1}", statusId, difficulty.ToString()));
                return 0f;
            }

            return Probabilities[statusId];
        }
        public bool IsRandomActivated(string statusId)
        {
            if(Probabilities.ContainsKey(statusId) == false)
            {
                Debug.LogError(string.Format("{0} status not found. Difficulty : {1}", statusId, difficulty.ToString()));
                return false;
            }

            return Probabilities[statusId] < Random.Range(0f, 1f);
        }

        public float GetRandomValue(string statusId)
        {
            if(RandomValues.ContainsKey(statusId) == false)
            {
                Debug.LogError(string.Format("{0} status not found. Difficulty : {1}", statusId, difficulty.ToString()));
                return 0f;
            }

            return Random.Range(0f, RandomValues[statusId]);
        }
    }
}