using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class AiConstants
{
    public const string HeavyDodgeDetectWhenLaunch = "HeavyDodgeDetectWhenLaunch";
    public const string LaunchRandomDirection = "LaunchRandomDirection";
    public const string DodgeDanger = "DodgeDanger";
}

namespace Data
{

    [System.Serializable]
    public struct AiProbabilities
    {
        [Range(0f,1f)]
        public float DodgeDanger;
        [Range(0f, 1f)]
        public float HeavyDodgeDetectWhenLaunch;

        public void GetDatas(Dictionary<string, float> dic)
        {
            dic.Add(AiConstants.DodgeDanger, DodgeDanger);
            dic.Add(AiConstants.HeavyDodgeDetectWhenLaunch, HeavyDodgeDetectWhenLaunch);
        }
    }

    [System.Serializable]
    public struct AiRandomValues
    {
        public float LaunchRandomDirection;

        public void GetDatas(Dictionary<string, float> dic)
        {
            dic.Add(AiConstants.LaunchRandomDirection, LaunchRandomDirection);
        }
    }

    [CreateAssetMenu(fileName = "AiDifficultyData", menuName = "AiDifficultyData", order = 1)]
    public class AiDifficultyData : ScriptableObject
    {
        public AiProbabilities Probabilities;
        public AiRandomValues RandomValues;
    }
}