using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SortingLayerController : Singleton<SortingLayerController>
{
    private Dictionary<IObject, SortingGroup> targets = new Dictionary<IObject, SortingGroup>();
    protected override void Awake()
    {

    }

    public void AddTarget(IObject target)
    {
        // 일단 Character랑 Obstacle만 적용.
        if(target as ICharacter != null || target as IObstacle != null)
        {
            targets.Add(target, (target as MonoBehaviour).GetComponent<SortingGroup>());
            target.OnDestroyed += OnDestroyed;
        }
    }

    private void OnDestroyed(IObject destroyedObject)
    {
        targets.Remove(destroyedObject);
    }

    private void Update()
    {
        List<IObject> sorted = new List<IObject>();
        sorted.AddRange(targets.Keys);
        sorted.Sort((lhs, rhs) =>
        {
            float lhsPosY = (lhs as MonoBehaviour).transform.position.y;
            float rhsPosY = (rhs as MonoBehaviour).transform.position.y;
            if(lhsPosY < rhsPosY)
                return 1;
            else if(lhsPosY > rhsPosY)
                return -1;
            return 0;
        });
        for(int i = 0; i < sorted.Count; ++i)
        {
            targets[sorted[i]].sortingOrder = i;
        }
    }
}