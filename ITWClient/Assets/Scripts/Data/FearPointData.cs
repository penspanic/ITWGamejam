using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "FearPointData", menuName = "FearPointData", order = 2)]
    public class FearPointData : ScriptableObject
    {
        public int HpStepChangeValue;
        public int MpCheckLimitValue;
        public int MpStepChangeValue;
        public int DecreaseValueForSecond;
    }
}