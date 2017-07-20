using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class AiConstants
{
    #region AiProbabilities
    public const string DangerDetactProbability = nameof(DangerDetactProbability);
    public const string HeavyDodgeDetectWhenLaunch = nameof(HeavyDodgeDetectWhenLaunch);
    #endregion

    #region AiRandomRanges
    public const string LaunchRandomDirection = nameof(LaunchRandomDirection);
    #endregion

    #region AiStatusValues
    public const string DangerDetactDistance = nameof(DangerDetactDistance);
    public const string DangerDetactInterval = nameof(DangerDetactInterval);
    public const string LaunchAdditionalDetactInterval = nameof(LaunchAdditionalDetactInterval);
    public const string LaunchAdditionalDetactLength = nameof(LaunchAdditionalDetactLength);
    #endregion
}

namespace Data
{

    [System.Serializable]
    public struct AiProbabilities
    {
        [Range(0f, 1f)]
        public float HeavyDodgeDetectWhenLaunch;
        [Range(0f, 1f)]
        public float DangerDetactProbability;

        public void GetDatas(Dictionary<string, float> dic)
        {
            dic.Add(AiConstants.DangerDetactProbability, DangerDetactProbability);
            dic.Add(AiConstants.HeavyDodgeDetectWhenLaunch, HeavyDodgeDetectWhenLaunch);
        }
    }

    [System.Serializable]
    public struct AiRandomRanges
    {
        public Vector2 LaunchRandomDirection;

        public void GetDatas(Dictionary<string, Vector2> dic)
        {
            dic.Add(AiConstants.LaunchRandomDirection, LaunchRandomDirection);
        }
    }

    [System.Serializable]
    public struct AiStatusValues
    {
        public float DangerDetactDistance;
        public float DangerDetactInterval;
        public float LaunchAdditionalDetactInterval;
        public float LaunchAdditionalDetactLength;
        public void GetDatas(Dictionary<string, float> dic)
        {
            dic.Add(AiConstants.DangerDetactDistance, DangerDetactDistance);
            dic.Add(AiConstants.DangerDetactInterval, DangerDetactInterval);
            dic.Add(AiConstants.LaunchAdditionalDetactInterval, LaunchAdditionalDetactInterval);
            dic.Add(AiConstants.LaunchAdditionalDetactLength, LaunchAdditionalDetactLength);
        }
    }

    [CreateAssetMenu(fileName = "AiDifficultyData", menuName = "AiDifficultyData", order = 1)]
    public class AiDifficultyData : ScriptableObject
    {
        public AiProbabilities Probabilities;
        public AiRandomRanges RandomRanges;
        public AiStatusValues StatusValues;
    }
}