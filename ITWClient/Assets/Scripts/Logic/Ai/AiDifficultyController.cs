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

    public static class AiStatusIds
    {
        public const string HeavyDodgeDetectWhenLaunch = "HeavyDodgeDetectWhenLaunch";
        public const string LaunchRandomDirectionValue = "LaunchRandomDirectionValue"; // 런치할 때 랜덤하게 이상한 위치를 향하도록 하는 값.
        public const string DodgeDangerProbability = "DodgeDangerProbability";
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
        
        private AiDifficulty Difficulty;

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
            Difficulty = AiDifficulty.Easy;

            // TODO : json이나 xml 파일로 데이터 빼기
            if(Difficulty == AiDifficulty.Easy)
            {
                Probabilities.Add(AiStatusIds.HeavyDodgeDetectWhenLaunch, 0.2f);
                Probabilities.Add(AiStatusIds.DodgeDangerProbability, 0.5f);

                RandomValues.Add(AiStatusIds.LaunchRandomDirectionValue, 0.5f);
            }
        }

        public float GetRawRandomValue(string statusId)
        {
            if (RandomValues.ContainsKey(statusId) == false)
            {
                Debug.LogError(string.Format("{0} status not found. Difficulty : {1}", statusId, Difficulty.ToString()));
                return 0f;
            }

            return RandomValues[statusId];
        }
        
        public float GetRawProbability(string statusId)
        {
            if (Probabilities.ContainsKey(statusId) == false)
            {
                Debug.LogError(string.Format("{0} status not found. Difficulty : {1}", statusId, Difficulty.ToString()));
                return 0f;
            }

            return Probabilities[statusId];
        }
        public bool IsRandomActivated(string statusId)
        {
            if(Probabilities.ContainsKey(statusId) == false)
            {
                Debug.LogError(string.Format("{0} status not found. Difficulty : {1}", statusId, Difficulty.ToString()));
                return false;
            }

            return Probabilities[statusId] < Random.Range(0f, 1f);
        }

        public float GetRandomValue(string statusId)
        {
            if(RandomValues.ContainsKey(statusId) == false)
            {
                Debug.LogError(string.Format("{0} status not found. Difficulty : {1}", statusId, Difficulty.ToString()));
                return 0f;
            }

            return Random.Range(0f, RandomValues[statusId]);
        }
    }
}