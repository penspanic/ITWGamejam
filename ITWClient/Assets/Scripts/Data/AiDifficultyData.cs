using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class AiConstants
{
    #region AiProbabilities
    public const string AvoidDangerByDodge = "AvoidDangerByDodge";
    public const string HeavyDodgeDetectWhenLaunch = "HeavyDodgeDetectWhenLaunch";
    public const string DangerDetactProbability = "DangerDetactProbability";
    #endregion

    #region AiRandomRanges
    public const string LaunchRandomDirection = "LaunchRandomDirection";
    #endregion

    #region AiStatusValues
    public const string DangerDetactDistance = "DangerDetactDistance";
    public const string DangerDetactInterval = "DangerDetactInterval";
    #endregion
}

namespace Data
{

    [System.Serializable]
    public struct AiProbabilities
    {
        [Range(0f, 1f)]
        public float AvoidDangerByDodge;
        [Range(0f, 1f)]
        public float HeavyDodgeDetectWhenLaunch;
        [Range(0f, 1f)]
        public float DangerDetactProbability;

        public void GetDatas(Dictionary<string, float> dic)
        {
            dic.Add(AiConstants.AvoidDangerByDodge, AvoidDangerByDodge);
            dic.Add(AiConstants.HeavyDodgeDetectWhenLaunch, HeavyDodgeDetectWhenLaunch);
            dic.Add(AiConstants.DangerDetactProbability, DangerDetactProbability);
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
        public void GetDatas(Dictionary<string, float> dic)
        {
            dic.Add(AiConstants.DangerDetactDistance, DangerDetactDistance);
            dic.Add(AiConstants.DangerDetactInterval, DangerDetactInterval);
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