using UnityEngine;
using System.Collections;

public class DelayDestroy : MonoBehaviour
{
    public float DelayTime;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(DelayTime);
        Destroy(this.gameObject);
    }
}