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
        public Dictionary<string, Vector2> RandomRanges { get; private set; }
        public Dictionary<string, float> StatusValues { get; private set; }
        
        private AiDifficulty difficulty;
        private Data.AiDifficultyData difficultyData;

        AiDifficultyController()
        {
            Probabilities = new Dictionary<string, float>();
            RandomRanges = new Dictionary<string, Vector2>();
            StatusValues = new Dictionary<string, float>();
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
            difficulty = TeamController.AiDifficulty;

            difficultyData = Resources.Load<Data.AiDifficultyData>("Data/Ai/AiDifficultyData_" + difficulty.ToString());
            difficultyData.Probabilities.GetDatas(Probabilities);
            difficultyData.RandomRanges.GetDatas(RandomRanges);
            difficultyData.StatusValues.GetDatas(StatusValues);
        }

        public Vector2 GetRawRandomValue(string statusId)
        {
            if (RandomRanges.ContainsKey(statusId) == false)
            {
                Debug.LogError(string.Format("{0} status not found. Difficulty : {1}", statusId, difficulty.ToString()));
                return Vector2.zero;
            }

            return RandomRanges[statusId];
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

            return Probabilities[statusId] > Random.Range(0f, 0.9999f); // 1f로 하면 1f도 나올 수 있음 : min [inclusive] and max [inclusive]
        }

        public float GetRandomRangeValue(string statusId)
        {
            if(RandomRanges.ContainsKey(statusId) == false)
            {
                Debug.LogError(string.Format("{0} status not found. Difficulty : {1}", statusId, difficulty.ToString()));
                return 0f;
            }

            return Random.Range(RandomRanges[statusId].x, RandomRanges[statusId].y);
        }

        public float GetStatusValue(string statusId)
        {
            if(StatusValues.ContainsKey(statusId) == false)
            {
                Debug.LogError(string.Format("{0} status not found. Difficulty : {1}", statusId, difficulty.ToString()));
                return 0f;
            }

            return StatusValues[statusId];
        }
    }
}