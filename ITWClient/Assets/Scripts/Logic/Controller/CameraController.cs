using UnityEngine;
using System.Collections;

/// <summary>
/// 인게임에서 플레이어들의 위치에 따라 줌인아웃
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] targets;
    [SerializeField]
    private float minOrthoSize;
    private void Awake()
    {
        StartAnimating();
    }

    public void SetTargets(GameObject[] targets)
    {
        this.targets = targets;
    }

    public void StartAnimating()
    {
        StartCoroutine(ZoomInOutProcess());
    }

    private IEnumerator ZoomInOutProcess()
    {
        while(true)
        {
            Rect targetRect = new Rect();
            foreach(GameObject target in targets)
            {
                Vector3 pos = target.transform.position;
                if(pos.x < targetRect.xMin)
                    targetRect.xMin = pos.x;
                if(pos.x > targetRect.xMax)
                    targetRect.xMax = pos.x;
                if(pos.y < targetRect.yMin)
                    targetRect.yMin = pos.y;
                if(pos.y > targetRect.yMax)
                    targetRect.yMax = pos.y;
            }
            Vector3 newPos = Camera.main.transform.position;
            newPos.x = targetRect.center.x;
            newPos.y = targetRect.center.y;
            Camera.main.transform.position = newPos;
            float orthoSize = targetRect.height / 2 + 0.5f;
            if(orthoSize < minOrthoSize)
                orthoSize = minOrthoSize;
            Camera.main.orthographicSize = orthoSize;

            yield return null;
        }
    }
}