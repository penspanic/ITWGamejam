using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ai
{
    /// <summary>
    /// 오브젝트간의 상호 공격 내용을 저장.
    /// 만든 이유 : Ai의 공격 우선순위 결정할 때 "가장 많이 데미지를 준 캐릭터를 우선으로 한다" 이런 것 구현하기 위해.
    /// </summary>
    public class AttackListener : Singleton<AttackListener>
    {
        private Dictionary<IObject/*Target*/, Dictionary<IObject/*Attacker*/, int/*Damage*/>> damagedData = new Dictionary<IObject, Dictionary<IObject, int>>();

        public void OnDamaged(IObject attacker, IObject target, int damage)
        {
            if(damagedData.ContainsKey(target) == false)
            {
                damagedData.Add(target, new Dictionary<IObject, int>());
            }
            if(damagedData[target].ContainsKey(attacker) == false)
            {
                damagedData[target].Add(attacker, 0);
            }
            damagedData[target][attacker] += damage;
        }

        // 가장 많은 데미지를 준 오브젝트 리턴.
        public IObject GetHighestAttackEnemy(IObject target)
        {
            if(damagedData.ContainsKey(target) == false || damagedData[target].Count == 0)
            {
                return null;
            }

            IObject highestObject = null;
            int highestDamage = 0;
            foreach(var pair in damagedData[target])
            {
                if(pair.Value > highestDamage)
                {
                    highestObject = pair.Key;
                    highestDamage = pair.Value;
                }
            }

            return highestObject;
        }
    }
}